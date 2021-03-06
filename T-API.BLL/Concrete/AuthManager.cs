﻿using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using T_API.BLL.Abstract;
using T_API.BLL.Validators.User;
using T_API.Core.DTO.User;
using T_API.Core.Exception;
using T_API.Core.Settings;
using T_API.DAL.Abstract;
using T_API.Entity.Concrete;

namespace T_API.BLL.Concrete
{
    public class AuthManager : IAuthService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public AuthManager(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _mapper = mapper;
        }


        private Tuple<string, DateTime> GenerateToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ConfigurationSettings.SecretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            };


            var signingCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var claimsIdentity = new ClaimsIdentity(claims);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.Now.AddYears(10),
                SigningCredentials = signingCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            //_httpContextAccessor.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
            return new Tuple<string, DateTime>(tokenHandler.WriteToken(token), DateTime.Now.AddYears(10));
        }

        public async Task Register(AddUserDto addUserDto)
        {
            try
            {
                AddUserValidator addUserValidator = new AddUserValidator();
                var result = addUserValidator.Validate(addUserDto);
                if (result.IsValid)
                {
                    var mappedEntity = _mapper.Map<UserEntity>(addUserDto);
                    int Id = await _userRepository.AddUser(mappedEntity);
                    if (Id == -1)
                    {
                        throw new Exception("Kullanıcı kayıt işlemi başarısız");
                    }
                }
                else
                {
                    throw new ValidationException(result.Errors.ToString());
                }

            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task<LoginResponseDto> Login(LoginUserDto loginUser, bool jwt)
        {
            try
            {
                LoginUserValidator loginUserValidator = new LoginUserValidator();
                var result = loginUserValidator.Validate(loginUser);
                if (result.IsValid)
                {
                    var user = await _userRepository.GetByUsername(loginUser.Username);
                    if (user != null)
                    {
                        if (user.Password.Equals(loginUser.Password))
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.Username),
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim(ClaimTypes.Role, user.Role),
                                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString())
                            };

                            var claimsIdentity = new ClaimsIdentity(
                                claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var authProperties = new AuthenticationProperties
                            {
                                AllowRefresh = true,
                                IsPersistent = true,
                                ExpiresUtc = DateTime.Now.AddYears(1),
                            };
                            if (!jwt)
                                await _httpContextAccessor.HttpContext.SignInAsync(
                                    CookieAuthenticationDefaults.AuthenticationScheme,
                                    new ClaimsPrincipal(claimsIdentity),
                                    null);
                            var token = GenerateToken(user);
                            return new LoginResponseDto()
                            {
                                Token = token.Item1,
                                ExpireDate = token.Item2
                            };
                        }
                        else
                        {
                            throw new UnauthorizedAccessException("Kullanıcı adı veya şifre doğru gönderilmedi");
                        }
                    }
                    else
                    {
                        throw new NullReferenceException($"{loginUser.Username} Kullancısının verisine ulaşılamadı");
                    }
                }
                else
                {
                    throw new ValidationException(result.Errors.ToString());
                }
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);

            }
        }

        public async Task Logout()
        {
            try
            {
                await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }


    }
}