﻿using System.Threading.Tasks;
using T_API.Core.DTO.User;

namespace T_API.BLL.Abstract
{
    public interface IAuthService
    {
        Task Register(AddUserDto addUserDto);
        Task<LoginResponseDto> Login(LoginUserDto loginUser,bool jwt);

        Task Logout();
    }
}