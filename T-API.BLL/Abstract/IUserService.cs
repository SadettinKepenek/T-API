using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using T_API.Core.DTO.User;

namespace T_API.BLL.Abstract
{
    public interface IUserService
    {
        Task<List<ListUserDto>> GetAll();
        Task<DetailUserDto> GetById(int id);
        Task<int> CreateUser(AddUserDto addUserDto);
        Task UpdateUser(UpdateUserDto updateUserDto);
        Task DeleteUser(DeleteUserDto deleteUserDto);
        
    }
}
