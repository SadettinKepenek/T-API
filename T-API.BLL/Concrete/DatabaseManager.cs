using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using T_API.BLL.Abstract;
using T_API.BLL.Validators.Database;
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
        private readonly IDatabaseRepository _databaseRepository;
        private readonly IMapper _mapper;
        private readonly IPackageService _packageService;
        private readonly IRemoteDbService _remoteDbService;
        private readonly IUserService _userService;
        private IMemoryCache _cache;
        public DatabaseManager(IDatabaseRepository databaseRepository, IMapper mapper, IRemoteDbService remoteDbService,
            IPackageService packageService, IUserService userService, IMemoryCache cache)
        {
            _databaseRepository = databaseRepository;
            _mapper = mapper;
            _remoteDbService = remoteDbService;
            _packageService = packageService;
            _userService = userService;
            _cache = cache;
        }


        public async Task<List<ListDatabaseDto>> GetAll()
        {
            try
            {
                var mappedData=await _cache.GetOrCreateAsync(CacheKeys.DatabaseKey(), async x =>
                {
                    x.SlidingExpiration = CacheKeys.SlidingExpirationCaching;
                    var databases =await _databaseRepository.GetAll();
                    return _mapper.Map<List<ListDatabaseDto>>(databases);
                });
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
                if (userId == 0) throw new ArgumentNullException("userId", "Kullanıcı Idsi boş olamaz");


                var mappedEntities=await _cache.GetOrCreateAsync(CacheKeys.DatabaseKeyByUser(userId), async x =>
                {
                    x.SlidingExpiration = CacheKeys.SlidingExpirationCaching;
                    var databases = await _databaseRepository.GetByUser(userId);
                    return _mapper.Map<List<ListDatabaseDto>>(databases);

                });
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
                if (string.IsNullOrEmpty(username))
                    throw new ArgumentNullException("username", "Kullanıcı adı boş olamaz");


                var mappedEntities = await _cache.GetOrCreateAsync(CacheKeys.DatabaseKeyByUser(username), async x =>
                {
                    x.SlidingExpiration = CacheKeys.SlidingExpirationCaching;
                    var databases = await _databaseRepository.GetByUser(username);
                    return _mapper.Map<List<ListDatabaseDto>>(databases);

                });
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
                if (databaseId == 0) throw new ArgumentNullException("databaseId", "Database Idsi boş olamaz");

                var mappedEntities = await _cache.GetOrCreateAsync(CacheKeys.DatabaseKeyById(databaseId), async x =>
                {
                    x.SlidingExpiration = CacheKeys.SlidingExpirationCaching;
                    var databases = await _databaseRepository.GetById(databaseId);
                    return _mapper.Map<DetailDatabaseDto>(databases);

                });

                return mappedEntities;
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task<List<string>> GetDataTypes(string provider)
        {
            var dataTypes = new List<string>();
            if (provider.Equals("MySql"))
                dataTypes.AddRange(typeof(MysqlProviderColumnType).GetFields().Select(field => field.GetValue(null) as string));
            else if (provider.Equals("MsSql"))
                dataTypes.AddRange(typeof(SqlServerProviderColumnType).GetFields().Select(field => field.GetValue(null) as string));
            else
                throw new AmbiguousMatchException("Provider türü için support bulunamadı");

            return dataTypes;
        }

        public async Task<int> AddDatabase(AddDatabaseDto dto)
        {
            try
            {
                var package = await _packageService.GetById(dto.PackageId);
                var user = await _userService.GetById(dto.UserId);
                var databases = await _databaseRepository.GetByUser(user.Username);


                if (package.Price <= 0.0 && databases.FirstOrDefault(x => x.PackageId == package.PackageId) != null)
                    throw new UnauthorizedAccessException("Ücretsiz paket birden fazla alınamaz");
                if (!(Convert.ToDouble(user.Balance) - package.Price * dto.MonthCount >= 0))
                    throw new Exception("Bakiye yetersiz.");

                var availableServer = await _remoteDbService.GetAvailableServer(dto.Provider);
                dto.Port = availableServer.Port;
                dto.Server = availableServer.Server;
                dto.Username = await _remoteDbService.GenerateUserName(dto.UserId);
                dto.Password = await _remoteDbService.GeneratePassword(dto.UserId);
                dto.DatabaseName = await _remoteDbService.GenerateDatabaseName(dto.UserId);
                dto.IsActive = true;
                dto.StartDate = DateTime.Now;
                dto.EndDate = DateTime.Now.AddMonths(dto.MonthCount);
                var validator = new AddDatabaseValidator();
                var result = validator.Validate(dto);
                if (!result.IsValid) throw new ValidationException(result.Errors.ToString());

                var mappedEntity = _mapper.Map<Database>(dto);
                using var scope = new TransactionScope();
                var addDatabaseResult = await _databaseRepository.AddDatabase(mappedEntity);
                var mappedUser = _mapper.Map<UpdateUserDto>(user);
                mappedUser.Balance -= Convert.ToDecimal(package.Price * dto.MonthCount);
                await _userService.UpdateUser(mappedUser);
                Transaction.Current.TransactionCompleted += async (sender, args) =>
                {
                    using var inlineTransactionScope = new TransactionScope();
                    await _remoteDbService.CreateDatabaseOnRemote(dto);
                    inlineTransactionScope.Complete();
                };
                scope.Complete();

                return addDatabaseResult;

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
                if (dto == null) throw new NullReferenceException("Bilgiler boş geldi");

                var validator = new UpdateDatabaseValidator();
                var validation = validator.Validate(dto);
                if (!validation.IsValid) throw new ValidationException(validation.Errors.ToString());

                using var scope = new TransactionScope();
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
                if (dto == null) throw new NullReferenceException("Bilgiler boş geldi");

                var validator = new DeleteDatabaseValidator();
                var validation = validator.Validate(dto);
                if (!validation.IsValid) throw new ValidationException(validation.Errors.ToString());

                using var scope = new TransactionScope();
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