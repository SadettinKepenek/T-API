using AutoMapper;
using T_API.Core.DAL.Concrete;
using T_API.Core.DTO.Column;
using T_API.Core.DTO.Database;
using T_API.Core.DTO.DatabasePackage;
using T_API.Core.DTO.ForeignKey;
using T_API.Core.DTO.Index;
using T_API.Core.DTO.Key;
using T_API.Core.DTO.Table;
using T_API.Core.DTO.User;
using T_API.Entity.Concrete;
using T_API.UI.Areas.Admin.Models.Database;
using T_API.UI.Models.Account;
using T_API.UI.Areas.Admin.Models.User;
using T_API.UI.Models.Database;
using T_API.UI.Models.Security;

namespace T_API.UI.MappingProfiles
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<AddDatabaseDto, Database>();
            CreateMap<CreateDatabaseViewModel, AddDatabaseDto>();
            CreateMap<CreateServiceViewModel, AddDatabaseDto>();
            CreateMap<UpdateDatabaseDto, Database>();
            CreateMap<UpdateDatabaseViewModel, UpdateDatabaseDto>().ReverseMap();
            CreateMap<EditServiceViewModel, UpdateDatabaseDto>().ReverseMap();
            CreateMap<EditServiceViewModel, DetailDatabaseDto>().ReverseMap();
            CreateMap<DeleteDatabaseDto, Database>();
            CreateMap<ListDatabaseDto, Database>().ReverseMap();
            CreateMap<DetailDatabaseDto, Database>().ReverseMap();

            CreateMap<AddUserDto, UserEntity>().ReverseMap();
            CreateMap<DetailUserDto, UpdateUserDto>().ReverseMap();
            CreateMap<CreateUserViewModel, AddUserDto>();
            CreateMap<RegisterViewModel, AddUserDto>().ReverseMap();
            CreateMap<LoginViewModel, LoginUserDto>().ReverseMap();

            CreateMap<DeleteUserDto, UserEntity>();
            CreateMap<UpdateUserDto, UserEntity>();
            CreateMap<UpdateUserViewModel, UpdateUserDto>().ReverseMap();
            CreateMap<DetailUserDto, UserEntity>().ReverseMap();
            CreateMap<SettingsViewModel, UpdateUserDto>().ReverseMap();
            CreateMap<DetailUserDto, SettingsViewModel>().ReverseMap();
            CreateMap<UpdateUserViewModel, DetailUserDto>();
            CreateMap<ListUserDto, UserEntity>().ReverseMap();


            CreateMap<AddColumnDto, Column>().ReverseMap();
            CreateMap<UpdateColumnDto, Column>().ReverseMap();
            CreateMap<DeleteColumnDto, Column>().ReverseMap();
            CreateMap<Column, ListColumnDto>().ReverseMap();
            CreateMap<Column, DetailUserDto>().ReverseMap();




            CreateMap<AddKeyDto, Key>().ReverseMap();
            CreateMap<UpdateKeyDto, Key>().ReverseMap();
            CreateMap<DeleteKeyDto, Key>().ReverseMap();
            CreateMap<Key, DetailKeyDto>().ReverseMap();
            CreateMap<Key, ListKeyDto>().ReverseMap();


            CreateMap<AddTableDto, Table>().ReverseMap();
            CreateMap<AddTableViewModel, AddTableDto>().ReverseMap();
            CreateMap<UpdateDatabaseDto, Table>().ReverseMap();
            CreateMap<DeleteTableDto, Table>().ReverseMap();
            CreateMap<DetailTableDto, Table>().ReverseMap();
            CreateMap<ListTableDto, Table>().ReverseMap();


            CreateMap<AddIndexDto, Index>().ReverseMap();
            CreateMap<UpdateIndexDto, Index>().ReverseMap();
            CreateMap<DeleteIndexDto, Index>().ReverseMap();
            CreateMap<DetailIndexDto, Index>().ReverseMap();
            CreateMap<ListIndexDto, Index>().ReverseMap();


            CreateMap<AddForeignKeyDto, ForeignKey>().ReverseMap();
            CreateMap<UpdateForeignKeyDto, ForeignKey>().ReverseMap();
            CreateMap<DeleteForeignKeyDto, ForeignKey>().ReverseMap();
            CreateMap<DetailForeignKeyDto, ForeignKey>().ReverseMap();
            CreateMap<ListForeignKeyDto, ForeignKey>().ReverseMap();


            CreateMap<DetailDatabaseDto, DbInformation>().ReverseMap();


            CreateMap<DetailDatabasePackageDto, DatabasePackage>().ReverseMap();
            CreateMap<AddDatabasePackageDto, DatabasePackage>().ReverseMap();
            CreateMap<UpdateDatabasePackageDto, DatabasePackage>().ReverseMap();
            CreateMap<DeleteDatabasePackageDto, DatabasePackage>().ReverseMap();
            CreateMap<ListDatabasePackageDto, DatabasePackage>().ReverseMap();

        }
    }
}