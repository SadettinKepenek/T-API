using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using T_API.BLL.Abstract;
using T_API.BLL.Validators.Database;
using T_API.Core.DTO.Database;
using T_API.Core.Exception;
using T_API.DAL.Abstract;
using T_API.Entity.Concrete;

namespace T_API.BLL.Concrete
{
    public class DatabaseManager : IDatabaseService
    {
        private IDatabaseRepository _databaseRepository;
        private IRealDbRepositoryFactory _dbRepositoryFactory;
        private IMapper _mapper;
        public DatabaseManager(IDatabaseRepository databaseRepository, IMapper mapper, IRealDbRepositoryFactory dbRepositoryFactory)
        {
            _databaseRepository = databaseRepository;
            _mapper = mapper;
            _dbRepositoryFactory = dbRepositoryFactory;
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
                return mappedEntities;
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task<int> AddDatabase(AddDatabaseDto dto)
        {
            try
            {
                dto.StartDate = DateTime.Now;
                dto.EndDate = DateTime.Now.AddMonths(1);
                dto.Port = "3306";
                dto.Provider = "MySql";
                dto.Server = "localhost";
                dto.IsActive = false;
                dto.IsApiSupport = true;
                dto.IsStorageSupport = false;
                
                AddDatabaseValidator validator = new AddDatabaseValidator();
                var result = validator.Validate(dto);
                if (result.IsValid)
                {



                    var mappedEntity = _mapper.Map<DatabaseEntity>(dto);
                    var addDatabase = await _databaseRepository.AddDatabase(mappedEntity);

                    return addDatabase;
                }

                throw new ValidationException(result.Errors.ToString());

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

                UpdateDatabaseValidator validator=new UpdateDatabaseValidator();
                var validation = validator.Validate(dto);
                if (!validation.IsValid)
                {
                    throw new ValidationException(validation.Errors.ToString());
                }

                var mappedData = _mapper.Map<DatabaseEntity>(dto);
                await _databaseRepository.UpdateDatabase(mappedData);
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

                DeleteDatabaseValidator validator=new DeleteDatabaseValidator();
                var validation = validator.Validate(dto);
                if (!validation.IsValid)
                {
                    throw new ValidationException(validation.Errors.ToString());
                }


                var mappedData = _mapper.Map<DatabaseEntity>(dto);
                await _databaseRepository.DeleteDatabase(mappedData);
            }
            catch (Exception e)
            {

                throw ExceptionHandler.HandleException(e);
            }
        }
    }
}