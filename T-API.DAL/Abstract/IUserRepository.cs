using System.Collections.Generic;
using System.Threading.Tasks;
using T_API.Entity.Concrete;

namespace T_API.DAL.Abstract
{
    public interface IUserRepository
    {
        Task<int> AddUser(UserEntity user);
        Task UpdateUser(UserEntity user);
        Task DeleteUser(UserEntity user);
        Task<List<UserEntity>> GetAll();
        Task<UserEntity> GetById(int userId);


    }
}