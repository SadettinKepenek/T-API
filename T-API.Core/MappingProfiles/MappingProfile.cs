using AutoMapper;
using T_API.Core.DTO.Database;
using T_API.Core.DTO.User;
using T_API.Entity.Concrete;

namespace T_API.Core.MappingProfiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<AddDatabaseDto, DatabaseEntity>();
            CreateMap<UpdateDatabaseDto, DatabaseEntity>();
            CreateMap<DeleteDatabaseDto, DatabaseEntity>();
            CreateMap<ListDatabaseDto, DatabaseEntity>().ReverseMap();
            CreateMap<DetailDatabaseDto, DatabaseEntity>().ReverseMap();

            CreateMap<AddUserDto, UserEntity>();
            CreateMap<DeleteUserDto, UserEntity>();
            CreateMap<UpdateUserDto, UserEntity>();
            CreateMap<DetailUserDto, UserEntity>().ReverseMap();
            CreateMap<ListUserDto, UserEntity>().ReverseMap();
        }
    }
}