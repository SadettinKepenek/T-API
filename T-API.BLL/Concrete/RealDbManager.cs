﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using T_API.BLL.Abstract;
using T_API.BLL.Validators.Database;
using T_API.BLL.Validators.Table;
using T_API.Core.DTO.Database;
using T_API.Core.DTO.Table;
using T_API.Core.Exception;
using T_API.DAL.Abstract;
using T_API.DAL.Concrete;
using T_API.Entity.Concrete;

namespace T_API.BLL.Concrete
{
    public class RealDbManager : IRealDbService
    {
        private IRealDbRepositoryFactory _realDbRepositoryFactory;
        private IMapper _mapper;

        //Factory Design Pattern

        public RealDbManager(IRealDbRepositoryFactory realDbRepositoryFactory, IMapper mapper)
        {
            _realDbRepositoryFactory = realDbRepositoryFactory;
            _mapper = mapper;
        }

        public async Task CreateDatabaseOnRemote(AddDatabaseDto database)
        {
            try
            {
                if (database.Provider.Equals("MySql"))
                {
                    using MySqlCodeGenerator generator = (MySqlCodeGenerator)SqlCodeGeneratorFactory.CreateGenerator(database.Provider);
                    if (generator != null)
                    {
                        AddDatabaseValidator addDatabaseValidator = new AddDatabaseValidator();
                        var result = addDatabaseValidator.Validate(database);
                        if (result.IsValid)
                        {
                            var mappedEntity = _mapper.Map<Database>(database);
                            string createDatabaseCommand = generator.CreateDatabase(mappedEntity);
                            if (!String.IsNullOrEmpty(createDatabaseCommand))
                            {
                                MySqlRealDbRepository realDbRepository =
                                    _realDbRepositoryFactory.CreateRepository(database.Provider) as
                                        MySqlRealDbRepository;
                                if (realDbRepository != null)
                                    await realDbRepository.CreateDatabaseOnRemote(createDatabaseCommand);
                                else
                                    throw new NullReferenceException("Mysql Real Db Repository Referansına Ulaşlamadı");
                            }
                            else
                            {
                                throw new NullReferenceException("Create Database Sql Referansı Bulunamadı");
                            }
                        }
                        else
                        {
                            throw new ValidationException(result.ToString());
                        }
                    }
                    else
                    {
                        throw new NullReferenceException("Code Generator Referansına Ulaşlamadı");
                    }
                }
                else
                {
                    throw new AmbiguousMatchException("Desteklenen Provider Verilmedi.");
                }
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }

        }

        public async Task CreateTableOnRemote(AddTableDto table)
        {
            try
            {
                if (table.Provider.Equals("MySql"))
                {
                    using MySqlCodeGenerator codeGenerator =
                        (MySqlCodeGenerator)SqlCodeGeneratorFactory.CreateGenerator(table.Provider);
                    if (codeGenerator != null)
                    {
                        AddTableValidator validator = new AddTableValidator();
                        var validationResult = validator.Validate(table);
                        if (validationResult.IsValid)
                        {
                            var mappedEntity = _mapper.Map<Table>(table);
                            string command = codeGenerator.CreateTable(mappedEntity);
                            if (!String.IsNullOrEmpty(command))
                            {
                                MySqlRealDbRepository realDbRepository =
                                    _realDbRepositoryFactory.CreateRepository(table.Provider) as MySqlRealDbRepository;
                                if (realDbRepository != null)
                                {
                                    using TransactionScope scope=new TransactionScope();
                                    await realDbRepository.CreateTableOnRemote(command);
                                    scope.Complete();
                                }
                                else
                                {
                                    throw new NullReferenceException("Db Repository Referansına Ulaşlamadı");
                                }
                            }
                            else
                            {
                                throw new NullReferenceException("Create Table Sql Referansı Bulunamadı");
                            }
                        }
                        else
                        {
                            throw new ValidationException(validationResult.ToString());
                        }
                    }
                    else
                    {
                        throw new NullReferenceException("Code Generator Referansına Ulaşlamadı");
                    }
                }
                else
                {
                    throw new AmbiguousMatchException("Desteklenen Provider Verilmedi.");
                }
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }

        }

        public async Task CreateColumnOnRemote(Database database)
        {
            throw new System.NotImplementedException();
        }
    }
}