using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using T_API.BLL.Abstract;
using T_API.BLL.Validators.DatabasePackageValidator;
using T_API.Core.DTO.Database;
using T_API.Core.DTO.DatabasePackage;
using T_API.Core.Exception;
using T_API.DAL.Abstract;
using T_API.Entity.Concrete;

namespace T_API.BLL.Concrete
{
    public class PackageManager:IPackageService
    {
        private IPackageRepository _packageRepository;
        private IMapper _mapper;

        public PackageManager(IPackageRepository packageRepository, IMapper mapper)
        {
            _packageRepository = packageRepository;
            _mapper = mapper;
        }

        public async Task<List<DetailDatabasePackageDto>> Get()
        {
            try
            {
                var databasePackages = await _packageRepository.Get();
                if (databasePackages == null)
                {
                    throw new NullReferenceException("Paket Bulunamadı");
                }
                var mappedData = _mapper.Map<List<DetailDatabasePackageDto>>(databasePackages);
                return mappedData;
            }
            catch (Exception e)
            {

                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task<DetailDatabasePackageDto> GetById(int id)
        {
            try
            {
                var databasePackages = await _packageRepository.GetById(id);
                if (databasePackages == null)
                {
                    throw new NullReferenceException("Paket Bulunamadı");
                }
                var mappedData = _mapper.Map<DetailDatabasePackageDto>(databasePackages);
                return mappedData;
            }
            catch (Exception e)
            {

                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task<DetailDatabasePackageDto> GetByName(string id)
        {
            try
            {
                var databasePackages = await _packageRepository.GetByName(id);
                if (databasePackages == null)
                {
                    throw new NullReferenceException("Paket Bulunamadı");
                }
                var mappedData = _mapper.Map<DetailDatabasePackageDto>(databasePackages);
                return mappedData;
            }
            catch (Exception e)
            {

                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task Add(AddDatabasePackageDto package)
        {

            try
            {
                AddDatabasePackageValidator validator=new AddDatabasePackageValidator();
                var result = validator.Validate(package);
                if (!result.IsValid)
                {
                    throw new ValidationException(result.ToString());
                }

                var mappedEntity = _mapper.Map<DatabasePackage>(package);
                using TransactionScope scope=new TransactionScope();
                await _packageRepository.Add(mappedEntity);
                scope.Complete();

            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }


        }

        public async Task Update(UpdateDatabasePackageDto package)
        {
            try
            {
                UpdateDatabasePackageValidator validator = new UpdateDatabasePackageValidator();
                var result = validator.Validate(package);
                if (!result.IsValid)
                {
                    throw new ValidationException(result.ToString());
                }

                var mappedEntity = _mapper.Map<DatabasePackage>(package);
                using TransactionScope scope = new TransactionScope();
                await _packageRepository.Update(mappedEntity);
                scope.Complete();

            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task Delete(DeleteDatabasePackageDto package)
        {
            try
            {
                DeleteDatabasePackageValidator validator = new DeleteDatabasePackageValidator();
                var result = validator.Validate(package);
                if (!result.IsValid)
                {
                    throw new ValidationException(result.ToString());
                }

                var mappedEntity = _mapper.Map<DatabasePackage>(package);
                using TransactionScope scope = new TransactionScope();
                await _packageRepository.Delete(mappedEntity);
                scope.Complete();

            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }
    }
}