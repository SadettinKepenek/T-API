using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using T_API.BLL.Abstract;
using T_API.BLL.Validators.Column;
using T_API.BLL.Validators.Database;
using T_API.BLL.Validators.Table;
using T_API.Core.DAL.Concrete;
using T_API.Core.DTO.Column;
using T_API.Core.DTO.Database;
using T_API.Core.DTO.ForeignKey;
using T_API.Core.DTO.Index;
using T_API.Core.DTO.Key;
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
        private IDatabaseRepository _databaseRepository;
        private IMapper _mapper;

        //Factory Design Pattern

        public RealDbManager(IRealDbRepositoryFactory realDbRepositoryFactory, IMapper mapper, IDatabaseRepository databaseRepository)
        {
            _realDbRepositoryFactory = realDbRepositoryFactory;
            _mapper = mapper;
            _databaseRepository = databaseRepository;
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
                                    using TransactionScope scope = new TransactionScope();
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

        public async Task CreateColumnOnRemote(AddColumnDto column)
        {

            try
            {
                if (column.Provider.Equals("MySql"))
                {
                    using MySqlCodeGenerator codeGenerator =
                        (MySqlCodeGenerator)SqlCodeGeneratorFactory.CreateGenerator(column.Provider);
                    if (codeGenerator != null)
                    {
                        AddColumnValidator validator = new AddColumnValidator();
                        var validationResult = validator.Validate(column);
                        if (validationResult.IsValid)
                        {
                            var mappedEntity = _mapper.Map<Column>(column);

                            Table table = new Table
                            {
                                TableName = column.TableName,
                            };
                            table.Columns.Add(mappedEntity);


                            string command = codeGenerator.AlterTable(table);
                            if (!String.IsNullOrEmpty(command))
                            {
                                MySqlRealDbRepository realDbRepository =
                                    _realDbRepositoryFactory.CreateRepository(column.Provider) as MySqlRealDbRepository;
                                if (realDbRepository != null)
                                {
                                    using TransactionScope scope = new TransactionScope();


                                    var db = await _databaseRepository.GetById(column.DatabaseId);
                                    if (db == null)
                                    {
                                        throw new NullReferenceException("Database bulunamadı");
                                    }
                                    DbInformation dbInformation = new DbInformation
                                    {
                                        Database = db.DatabaseName,
                                        Provider = db.Provider,
                                        Username = db.Username,
                                        Port = db.Port,
                                        Server = db.Server,
                                        Password = db.Password
                                    };
                                    await realDbRepository.ExecuteQueryOnRemote(command, dbInformation);
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

        public Task CreateIndexOnRemote(AddIndexDto index)
        {
            throw new NotImplementedException();
        }

        public Task CreateForeignKeyOnRemote(AddForeignKeyDto foreignKey)
        {
            throw new NotImplementedException();
        }

        public Task CreateKeyOnRemote(AddKeyDto key)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DetailTableDto>> GetTables(string databaseName, string provider)
        {
            if (String.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException("databaseName", "Database ismi boş gönderilemez");
            }
            MySqlRealDbRepository realDbRepository =
                _realDbRepositoryFactory.CreateRepository(provider) as MySqlRealDbRepository;

            if (realDbRepository == null)
            {
                throw new NullReferenceException("Real Db Repository Referansına Ulaşılamadı");
            }

            var result = await realDbRepository.GetTables(databaseName);
            var mappedResults = _mapper.Map<List<DetailTableDto>>(result);
            return mappedResults;
        }

        public async Task<DetailTableDto> GetTable(string tableName, string databaseName, string provider)
        {

            if (String.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException("databaseName", "Database ismi boş gönderilemez");
            }
            MySqlRealDbRepository realDbRepository =
                _realDbRepositoryFactory.CreateRepository(provider) as MySqlRealDbRepository;

            if (realDbRepository == null)
            {
                throw new NullReferenceException("Real Db Repository Referansına Ulaşılamadı");
            }

            var result = await realDbRepository.GetTable(tableName, databaseName);
            var mappedResults = _mapper.Map<DetailTableDto>(result);
            return mappedResults;
        }

        public Task<List<DetailForeignKeyDto>> GetForeignKeys(string databaseName, string provider)
        {
            throw new NotImplementedException();
        }

        public Task<List<DetailForeignKeyDto>> GetForeignKeys(string databaseName, string tableName, string provider)
        {
            throw new NotImplementedException();
        }

        public Task<DetailForeignKeyDto> GetForeignKey(string databaseName, string tableName, string foreignKeyName, string provider)
        {
            throw new NotImplementedException();
        }

        public Task<List<DetailKeyDto>> GetKeys(string databaseName, string provider)
        {
            throw new NotImplementedException();
        }

        public Task<List<DetailKeyDto>> GetKeys(string databaseName, string tableName, string provider)
        {
            throw new NotImplementedException();
        }

        public Task<DetailKeyDto> GetKey(string databaseName, string tableName, string keyName, string provider)
        {
            throw new NotImplementedException();
        }

        public Task<List<DetailIndexDto>> GetIndices(string databaseName, string provider)
        {
            throw new NotImplementedException();
        }

        public Task<List<DetailIndexDto>> GetIndices(string databaseName, string tableName, string provider)
        {
            throw new NotImplementedException();
        }

        public Task<DetailIndexDto> GetIndex(string databaseName, string tableName, string indexName, string provider)
        {
            throw new NotImplementedException();
        }

        public Task<List<DetailColumnDto>> GetColumns(string databaseName, string tableName, string provider)
        {
            throw new NotImplementedException();
        }

        public Task<DetailColumnDto> GetColumn(string databaseName, string tableName, string columnName, string provider)
        {
            throw new NotImplementedException();
        }

        public async Task CreateColumnOnRemote(Database database)
        {
            throw new System.NotImplementedException();
        }
    }
}