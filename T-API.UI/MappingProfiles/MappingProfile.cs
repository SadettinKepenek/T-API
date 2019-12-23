using AutoMapper;
using T_API.Core.DTO.Database;
using T_API.Core.DTO.User;
using T_API.Entity.Concrete;
using T_API.UI.Areas.Admin.Models.Database;
using T_API.UI.Models.Database;
using T_API.UI.Models.Security;

namespace T_API.UI.MappingProfiles
{
    public class MappingProfile: Profile
    {
        
        public MappingProfile()
        {
            CreateMap<AddDatabaseDto, DatabaseEntity>();
            CreateMap<CreateServiceViewModel, AddDatabaseDto>();
            CreateMap<UpdateDatabaseDto, DatabaseEntity>();
            CreateMap<UpdateDatabaseViewModel, UpdateDatabaseDto>().ReverseMap();
            CreateMap<DeleteDatabaseDto, DatabaseEntity>();
            CreateMap<ListDatabaseDto, DatabaseEntity>().ReverseMap();
            CreateMap<DetailDatabaseDto, DatabaseEntity>().ReverseMap();

            CreateMap<AddUserDto, UserEntity>().ReverseMap();
            CreateMap<CreateUserViewModel,AddUserDto>();
            CreateMap<RegisterViewModel, AddUserDto>().ReverseMap();
            CreateMap<LoginViewModel, LoginUserDto>().ReverseMap();

            CreateMap<DeleteUserDto, UserEntity>();
            CreateMap<UpdateUserDto, UserEntity>();
            CreateMap<UpdateUserViewModel,UpdateUserDto >().ReverseMap();
            CreateMap<DetailUserDto, UserEntity>().ReverseMap();
            CreateMap<UpdateUserViewModel, DetailUserDto >();
            CreateMap<ListUserDto, UserEntity>().ReverseMap();

            
        }
    }
}