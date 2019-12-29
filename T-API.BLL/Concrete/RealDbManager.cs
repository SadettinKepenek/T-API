using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using T_API.BLL.Abstract;
using T_API.BLL.Validators.Column;
using T_API.BLL.Validators.Database;
using T_API.BLL.Validators.ForeignKey;
using T_API.BLL.Validators.Table;
using T_API.Core.DAL.Concrete;
using T_API.Core.DTO.Column;
using T_API.Core.DTO.Database;
using T_API.Core.DTO.ForeignKey;
using T_API.Core.DTO.Index;
using T_API.Core.DTO.Key;
using T_API.Core.DTO.Table;
using T_API.Core.Exception;
using T_API.Core.Settings;
using T_API.DAL.Abstract;
using T_API.DAL.Concrete;
using T_API.Entity.Concrete;
using Index = T_API.Entity.Concrete.Index;

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

        /// <summary>
        /// Ana makinede veritabanı oluşturulmak için kullanılan servis.
        /// </summary>
        /// <param name="database">Eklenilmek istenilen veritabanı bilgileri</param>
        /// <returns></returns>
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
                                await ExecuteQueryOnRemote(createDatabaseCommand);
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

        /// <summary>
        /// İstenilen veri tabanında tablo eklemek için kullanılan servis.
        /// </summary>
        /// <param name="table">Eklenilmek istenilen tablo</param>
        /// <param name="dbInformation">Bağlantı bilgileri.</param>
        /// <returns></returns>
        public async Task CreateTableOnRemote(AddTableDto table, DbInformation dbInformation)
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

                                await ExecuteQueryOnRemote(command, dbInformation);
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

        /// <summary>
        /// İstenilen veri tabanında belirtilen tabloya sütun eklemek için kullanılan servis.
        /// </summary>
        /// <param name="column">Sütun bilgileri</param>
        /// <param name="dbInformation">Bağlantı bilgileri</param>
        /// <returns></returns>
        public async Task CreateColumnOnRemote(AddColumnDto column, DbInformation dbInformation)
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
                                DatabaseName = dbInformation.DatabaseName
                            };

                            table.Columns.Add(mappedEntity);
                            string command = codeGenerator.AlterTable(table);
                            if (!String.IsNullOrEmpty(command))
                            {
                                await ExecuteQueryOnRemote(command, dbInformation);
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

        public async Task AlterColumnOnRemote(UpdateColumnDto column, DbInformation dbInformation)
        {
            try
            {
                if (column.Provider.Equals("MySql"))
                {
                    using MySqlCodeGenerator codeGenerator =
                        (MySqlCodeGenerator)SqlCodeGeneratorFactory.CreateGenerator(column.Provider);
                    if (codeGenerator != null)
                    {
                        UpdateColumnValidator validator = new UpdateColumnValidator();
                        var validationResult = validator.Validate(column);
                        if (validationResult.IsValid)
                        {
                            var databaseEntity = await GetTable(column.TableName, dbInformation.DatabaseName, dbInformation.Provider);
                            if (databaseEntity != null)
                            {

                                List<string> queries = new List<string>();

                                if (column.OldColumn != null)
                                {
                                    if (column.OldColumn.Unique && column.Unique == false)
                                    {
                                        var idx = databaseEntity.Keys.FirstOrDefault(x =>
                                            x.KeyColumn == column.ColumnName && x.TableName == column.TableName);
                                        queries.Add(codeGenerator.DropKey(_mapper.Map<Key>(idx)));
                                    }
                                    if (column.OldColumn.PrimaryKey && column.PrimaryKey == false)
                                    {
                                        var idx = databaseEntity.Keys.FirstOrDefault(x =>
                                            x.KeyColumn == column.ColumnName && x.TableName == column.TableName);
                                        queries.Add(codeGenerator.DropKey(_mapper.Map<Key>(idx)));
                                    }
                                }

                                column.DefaultValue = null;
                                var mappedEntity = _mapper.Map<Column>(column);

                                Table table = new Table
                                {
                                    TableName = column.TableName,
                                    DatabaseName = dbInformation.DatabaseName
                                };
                                table.Columns.Add(mappedEntity);
                                string command = codeGenerator.AlterTable(table);
                                queries.Add(command);


                                if (!String.IsNullOrEmpty(command))
                                {
                                    await ExecuteQueryOnRemote(queries, dbInformation);
                                }
                                else
                                {
                                    throw new NullReferenceException("Create Table Sql Referansı Bulunamadı");
                                }
                            }
                            else
                            {
                                throw new ArgumentNullException("Database", "Database Entity Null");
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

        public Task CreateIndexOnRemote(AddIndexDto index, DbInformation dbInformation)
        {
            throw new NotImplementedException();
        }

        public async Task CreateForeignKeyOnRemote(AddForeignKeyDto foreignKey, DbInformation dbInformation)
        {
            try
            {
                if (dbInformation.Provider.Equals("MySql"))
                {
                    using MySqlCodeGenerator codeGenerator =
                        (MySqlCodeGenerator)SqlCodeGeneratorFactory.CreateGenerator(dbInformation.Provider);
                    if (codeGenerator != null)
                    {
                        AddForeignKeyValidator validator = new AddForeignKeyValidator();
                        var validationResult = validator.Validate(foreignKey);
                        if (validationResult.IsValid)
                        {
                            var mappedEntity = _mapper.Map<ForeignKey>(foreignKey);

                            Table table = new Table
                            {
                                TableName = foreignKey.SourceTable,
                                DatabaseName = dbInformation.DatabaseName,
                            };
                            table.ForeignKeys.Add(mappedEntity);


                            string command = codeGenerator.AlterTable(table);
                            if (!String.IsNullOrEmpty(command))
                            {
                                await ExecuteQueryOnRemote(command, dbInformation);
                            }
                            else
                            {
                                throw new NullReferenceException("Create Foreign Key Sql Referansı Bulunamadı");
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

        public Task CreateKeyOnRemote(AddKeyDto key, DbInformation dbInformation)
        {
            throw new NotImplementedException();
        }

        public async Task ExecuteQueryOnRemote(string query, DbInformation dbInformation)
        {
            try
            {
                if (dbInformation.Provider.Equals("MySql"))
                {
                    if (_realDbRepositoryFactory.CreateRepository(dbInformation.Provider) is MySqlRealDbRepository realDbRepository)
                    {
                        using TransactionScope scope = new TransactionScope();
                        await realDbRepository.ExecuteQueryOnRemote(query, dbInformation);
                        scope.Complete();
                    }
                    else
                    {
                        throw new NullReferenceException("Db Repository Referansına Ulaşlamadı");
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
        public async Task ExecuteQueryOnRemote(List<string> queries, DbInformation dbInformation)
        {
            try
            {
                if (dbInformation.Provider.Equals("MySql"))
                {
                    if (_realDbRepositoryFactory.CreateRepository(dbInformation.Provider) is MySqlRealDbRepository realDbRepository)
                    {
                        using TransactionScope scope = new TransactionScope();
                        foreach (var query in queries)
                        {
                            await realDbRepository.ExecuteQueryOnRemote(query, dbInformation);
                        }
                        scope.Complete();
                    }
                    else
                    {
                        throw new NullReferenceException("Db Repository Referansına Ulaşlamadı");
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

        public async Task ExecuteQueryOnRemote(string query)
        {
            try
            {
                if (ConfigurationSettings.ServerDbInformation.Provider.Equals("MySql"))
                {

                    if (_realDbRepositoryFactory.CreateRepository(ConfigurationSettings.ServerDbInformation.Provider) is MySqlRealDbRepository realDbRepository)
                        await realDbRepository.ExecuteQueryOnRemote(query);
                    else
                        throw new NullReferenceException("Mysql Real Db Repository Referansına Ulaşlamadı");
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


    }
}