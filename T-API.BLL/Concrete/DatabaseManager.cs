using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using T_API.BLL.Abstract;
using T_API.Core.DTO.Database;
using T_API.DAL.Abstract;

namespace T_API.BLL.Concrete
{
    public class DatabaseManager:IDatabaseService
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

            var databases =await  _databaseRepository.GetByUser(userId);
            if (databases==null)
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

        public Task<int> AddDatabase(AddDatabaseDto dto)
        {
            throw new NotImplementedException();
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