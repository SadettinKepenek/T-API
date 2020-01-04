using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using T_API.BLL.Abstract;
using T_API.BLL.Validators.Database;
using T_API.Core.DAL.Concrete;
using T_API.Core.DTO.Database;
using T_API.Core.DTO.User;
using T_API.Core.Exception;
using T_API.Core.Settings;
using T_API.DAL.Abstract;
using T_API.Entity.Concrete;

namespace T_API.BLL.Concrete
{
    public class DatabaseManager : IDatabaseService
    {
        private IDatabaseRepository _databaseRepository;
        private IMapper _mapper;
        private IRealDbService _realDbService;
        private IPackageService _packageService;
        private IUserService _userService;
        public DatabaseManager(IDatabaseRepository databaseRepository, IMapper mapper, IRealDbService realDbService, IPackageService packageService, IUserService userService)
        {
            _databaseRepository = databaseRepository;
            _mapper = mapper;
            _realDbService = realDbService;
            _packageService = packageService;
            _userService = userService;
        }


        public async Task<List<ListDatabaseDto>> GetAll()
        {
            try
            {
                var databases = await _databaseRepository.GetAll();
                if (databases == null)
                {
                    throw new NullReferenceException("Database Bulunamadı");
                }
                var mappedData = _mapper.Map<List<ListDatabaseDto>>(databases);
                return mappedData;
            }
            catch (Exception e)
            {

                throw ExceptionHandler.HandleException(e);
            }
        }
        public async Task<List<ListDatabaseDto>> GetByUser(int userId)
        {
            try
            {
                if (userId == 0)
                {
                    throw new ArgumentNullException("userId", "Kullanıcı Idsi boş olamaz");
                }

                var databases = await _databaseRepository.GetByUser(userId);
                if (databases == null)
                {
                    throw new NullReferenceException("İstenilen kullanıcıya ulaşılamadı");
                }

                var mappedEntities = _mapper.Map<List<ListDatabaseDto>>(databases);
                return mappedEntities;
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }
        public async Task<List<ListDatabaseDto>> GetByUser(string username)
        {
            try
            {
                if (String.IsNullOrEmpty(username))
                {
                    throw new ArgumentNullException("username", "Kullanıcı adı boş olamaz");
                }
                var databases = await _databaseRepository.GetByUser(username);
                if (databases == null)
                {
                    throw new NullReferenceException("İstenilen kullanıcıya ulaşılamadı");
                }

                var mappedEntities = _mapper.Map<List<ListDatabaseDto>>(databases);
                return mappedEntities;
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task<DetailDatabaseDto> GetById(int databaseId)
        {
            try
            {
                if (databaseId == 0)
                {
                    throw new ArgumentNullException("databaseId", "Database Idsi boş olamaz");
                }

                var databaseEntity = await _databaseRepository.GetById(databaseId);
                if (databaseEntity == null)
                {
                    throw new NullReferenceException("İstenilen database verisine ulaşılamadı");
                }

                var mappedEntities = _mapper.Map<DetailDatabaseDto>(databaseEntity);
                mappedEntities.Tables = await _realDbService.GetTables(databaseEntity.DatabaseName, databaseEntity.Provider);

                return mappedEntities;
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task<List<string>> GetDataTypes(string provider)
        {
            List<string> dataTypes = new List<string>();
            if (provider.Equals("MySql"))
            {
                foreach (FieldInfo field in typeof(MysqlProviderColumnType).GetFields())
                {
                    dataTypes.Add(field.GetValue(null) as string);
                }
            }
            else if (provider.Equals("MsSql"))
            {
                foreach (FieldInfo field in typeof(SqlServerProviderColumnType).GetFields())
                {
                    dataTypes.Add(field.GetValue(null) as string);
                }
            }
            else
            {
                throw new AmbiguousMatchException("Provider türü için support bulunamadı");
            }

            return dataTypes;
        }

        public async Task<int> AddDatabase(AddDatabaseDto dto)
        {
            try
            {
                var transactionCompletedEvent = new AutoResetEvent(true);

                var package = await _packageService.GetById(dto.PackageId);
                var user = await _userService.GetById(dto.UserId);
                var databases = await _databaseRepository.GetByUser(user.Username);


                if (package.Price <= 0.0 && databases.FirstOrDefault(x => x.PackageId == package.PackageId) != null)
                {
                    throw new UnauthorizedAccessException("Ücretsiz paket birden fazla alınamaz");
                }
                if ((Convert.ToDouble(user.Balance) - (package.Price * dto.MonthCount)) >= 0)
                {

                    var availableServer = await _realDbService.GetAvailableServer(dto.Provider);

                    dto.Port = availableServer.Port;
                    dto.Server = availableServer.Server;
                    dto.Username = await _realDbService.GenerateUserName(dto.UserId);
                    dto.Password = await _realDbService.GeneratePassword(dto.UserId);
                    dto.DatabaseName = await _realDbService.GenerateDatabaseName(dto.UserId);
                    dto.IsActive = false;
                    dto.StartDate = DateTime.Now;
                    dto.EndDate = DateTime.Now.AddMonths(dto.MonthCount);

                    AddDatabaseValidator validator = new AddDatabaseValidator();
                    var result = validator.Validate(dto);
                    if (result.IsValid)
                    {

                        var mappedEntity = _mapper.Map<Database>(dto);

                        int addDatabaseResult;

                        addDatabaseResult = await _databaseRepository.AddDatabase(mappedEntity);
                        await _realDbService.CreateDatabaseOnRemote(dto);
                        var mappedUser = _mapper.Map<UpdateUserDto>(user);
                        mappedUser.Balance -= Convert.ToDecimal(package.Price * dto.MonthCount);
                        await _userService.UpdateUser(mappedUser);



                        return addDatabaseResult;
                    }

                    throw new ValidationException(result.Errors.ToString());
                }
                else
                {
                    throw new Exception("Bakiye yetersiz.");
                }


            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }

        }

        public async Task UpdateDatabase(UpdateDatabaseDto dto)
        {
            try
            {
                if (dto == null)
                {
                    throw new NullReferenceException("Bilgiler boş geldi");
                }

                UpdateDatabaseValidator validator = new UpdateDatabaseValidator();
                var validation = validator.Validate(dto);
                if (!validation.IsValid)
                {
                    throw new ValidationException(validation.Errors.ToString());
                }

                using TransactionScope scope = new TransactionScope();
                var mappedData = _mapper.Map<Database>(dto);
                await _databaseRepository.UpdateDatabase(mappedData);
                scope.Complete();
            }
            catch (Exception e)
            {

                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task DeleteDatabase(DeleteDatabaseDto dto)
        {
            try
            {
                if (dto == null)
                {
                    throw new NullReferenceException("Bilgiler boş geldi");

                }

                DeleteDatabaseValidator validator = new DeleteDatabaseValidator();
                var validation = validator.Validate(dto);
                if (!validation.IsValid)
                {
                    throw new ValidationException(validation.Errors.ToString());
                }

                using TransactionScope scope = new TransactionScope();
                var mappedData = _mapper.Map<Database>(dto);
                await _databaseRepository.DeleteDatabase(mappedData);
                scope.Complete();
            }
            catch (Exception e)
            {

                throw ExceptionHandler.HandleException(e);
            }
        }
    }
}