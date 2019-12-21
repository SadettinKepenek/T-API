using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using T_API.BLL.Abstract;
using T_API.BLL.Validators.User;
using T_API.Core.DTO.User;
using T_API.Core.Exception;
using T_API.DAL.Abstract;
using T_API.Entity.Concrete;

namespace T_API.BLL.Concrete
{
    public class UserManager : IUserService
    {
        IUserRepository _userRepository;
        IMapper _mapper;

        public UserManager(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<int> CreateUser(AddUserDto addUserDto)
        {

            try
            {
                AddUserValidator validator=new AddUserValidator();
                var validation = validator.Validate(addUserDto);
                if (!validation.IsValid)
                {
                    throw new ValidationException(validation.Errors.ToString());
                }

                var mappedData = _mapper.Map<UserEntity>(addUserDto);
                var insertedId = await _userRepository.AddUser(mappedData);
                if (insertedId == 0)
                {
                    throw new ArgumentNullException("Kullanıcı eklenirken bir hata oluştu");
                }
                return insertedId;
            }
            catch (Exception e)
            {

                throw ExceptionHandler.HandleException(e);

            }
        }

        public async Task DeleteUser(DeleteUserDto deleteUserDto)
        {

            try
            {
                DeleteUserValidator validator=new DeleteUserValidator();
                var result = validator.Validate(deleteUserDto);
                if (!result.IsValid)
                {
                    throw new ValidationException(result.Errors.ToString());
                }
                var mappedData = _mapper.Map<UserEntity>(deleteUserDto);
                await _userRepository.DeleteUser(mappedData);
            }
            catch (Exception e)
            {

                throw ExceptionHandler.HandleException(e);

            }
        }

        public async Task<List<ListUserDto>> GetAll()
        {
            try
            {
                var users = await _userRepository.GetAll();
                if (users == null)
                {
                    throw new NullReferenceException("Kullanıcı yok");
                }
                var mappedData = _mapper.Map<List<ListUserDto>>(users);
                return mappedData;
            }
            catch (Exception e)
            {

                throw ExceptionHandler.HandleException(e);

            }
        }

        public async Task<DetailUserDto> GetById(int id)
        {
            try
            {
                var user = await _userRepository.GetById(id);
                if (user == null)
                {
                    throw new NullReferenceException("Kullanıcı yok");
                }
                var mappedData = _mapper.Map<DetailUserDto>(user);
                return mappedData;
            }
            catch (Exception e)
            {

                throw ExceptionHandler.HandleException(e);

            }
        }

        public async Task UpdateUser(UpdateUserDto updateUserDto)
        {
            try
            {
                UpdateUserValidator validator=new UpdateUserValidator();
                var result = validator.Validate(updateUserDto);
                if (!result.IsValid)
                {
                    throw new ValidationException(result.Errors.ToString());
                }
                var mappedData = _mapper.Map<UserEntity>(updateUserDto);
                await _userRepository.UpdateUser(mappedData);

            }
            catch (Exception e)
            {

                throw ExceptionHandler.HandleException(e);

            }
        }
    }
}
