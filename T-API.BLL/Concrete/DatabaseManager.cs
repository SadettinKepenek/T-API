﻿using System;
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
        private IMapper _mapper;
        public DatabaseManager(IDatabaseRepository databaseRepository, IMapper mapper)
        {
            _databaseRepository = databaseRepository;
            _mapper = mapper;
        }


        public Task<List<ListDatabaseDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ListDatabaseDto>> GetByUser(int userId)
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

        public async Task<List<ListDatabaseDto>> GetByUser(string username)
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


        public async Task<DetailDatabaseDto> GetById(int databaseId)
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
                if (!result.IsValid)
                {
                    throw new ValidationException(result.Errors.ToString());
                }

                var mappedEntity = _mapper.Map<DatabaseEntity>(dto);
                return await _databaseRepository.AddDatabase(mappedEntity);

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public Task UpdateDatabase(UpdateDatabaseDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDatabase(DeleteDatabaseDto dto)
        {
            throw new NotImplementedException();
        }
    }
}