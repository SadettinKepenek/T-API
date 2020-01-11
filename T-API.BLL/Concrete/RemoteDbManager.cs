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
using T_API.BLL.Validators.Key;
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
    public class RemoteDbManager : IRemoteDbService
    {
        private IMapper _mapper;
        private IPackageService _packageService;
        private IDatabaseRepository _databaseRepository;
        

        //Factory Design Pattern

        public RemoteDbManager(IMapper mapper, IPackageService packageService, IDatabaseRepository databaseRepository)
        {
            _mapper = mapper;
            _packageService = packageService;
            _databaseRepository = databaseRepository;
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
                if (SqlCodeGeneratorFactory.CreateGenerator(database.Provider) is MySqlCodeGenerator generator)
                {
                    AddDatabaseValidator addDatabaseValidator = new AddDatabaseValidator();
                    var result = addDatabaseValidator.Validate(database);
                    if (result.IsValid)
                    {
                        var mappedEntity = _mapper.Map<Database>(database);
                        string createDatabaseCommand = generator.GenerateCreateDatabaseQuery(mappedEntity);
                        await ExecuteQueryOnRemote(createDatabaseCommand);
                    }
                    else
                    {
                        throw new ValidationException(result.ToString());
                    }
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




                if (SqlCodeGeneratorFactory.CreateGenerator(table.Provider) is MySqlCodeGenerator mySqlCodeGenerator)
                {
                    AddTableValidator validator = new AddTableValidator();
                    var validationResult = validator.Validate(table);
                    if (validationResult.IsValid)
                    {
                        var mappedEntity = _mapper.Map<Table>(table);
                        var command = mySqlCodeGenerator.GenerateCreateTableQuery(mappedEntity);
                        await ExecuteQueryOnRemote(command, dbInformation);

                    }
                    else
                    {
                        throw new ValidationException(validationResult.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }

        }

        public async Task DropTableOnRemote(DeleteTableDto table, DbInformation dbInformation)
        {
            try
            {
                if (SqlCodeGeneratorFactory.CreateGenerator(dbInformation.Provider) is MySqlCodeGenerator mySqlCodeGenerator)
                {
                    DeleteTableValidator validator = new DeleteTableValidator();
                    var validationResult = validator.Validate(table);
                    if (validationResult.IsValid)
                    {
                        var mappedEntity = _mapper.Map<Table>(table);
                        var command = mySqlCodeGenerator.GenerateDropTableQuery(mappedEntity);
                        await ExecuteQueryOnRemote(command, dbInformation);

                    }
                    else
                    {
                        throw new ValidationException(validationResult.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task CreateKeyOnRemote(AddKeyDto key, DbInformation dbInformation)
        {
            try
            {
                if (SqlCodeGeneratorFactory.CreateGenerator(key.Provider) is MySqlCodeGenerator codeGenerator)
                {
                    AddKeyValidator validator = new AddKeyValidator();
                    var validationResult = validator.Validate(key);
                    if (validationResult.IsValid)
                    {
                        var entity = _mapper.Map<Key>(key);
                        var dbTable = await GetTable(key.TableName, dbInformation);
                        var table = _mapper.Map<Table>(dbTable);
                        string command = codeGenerator.GenerateAddKeyQuery(entity, table);
                        await ExecuteQueryOnRemote(command, dbInformation);
                    }
                    else
                    {
                        throw new ValidationException(validationResult.ToString());
                    }
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
                if (SqlCodeGeneratorFactory.CreateGenerator(column.Provider) is MySqlCodeGenerator codeGenerator)
                {
                    AddColumnValidator validator = new AddColumnValidator();
                    var validationResult = validator.Validate(column);
                    if (validationResult.IsValid)
                    {
                        var entity = _mapper.Map<Column>(column);
                        var dbTable = await GetTable(column.TableName, dbInformation);
                        var table = _mapper.Map<Table>(dbTable);
                        string command = codeGenerator.GenerateAddColumnQuery(entity, table);
                        await ExecuteQueryOnRemote(command, dbInformation);


                    }
                    else
                    {
                        throw new ValidationException(validationResult.ToString());
                    }
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
                if (SqlCodeGeneratorFactory.CreateGenerator(column.Provider) is MySqlCodeGenerator codeGenerator)
                {
                    UpdateColumnValidator validator = new UpdateColumnValidator();
                    var validationResult = validator.Validate(column);
                    if (validationResult.IsValid)
                    {
                        var dbTable = await GetTable(column.TableName, dbInformation);
                        var table = _mapper.Map<Table>(dbTable);
                        var queries = new List<string>();
                        if (column.OldColumn != null)
                        {
                            if (column.OldColumn.Unique && column.Unique == false)
                            {
                                var idx = dbTable.Keys.FirstOrDefault(x =>
                                    x.KeyColumn == column.ColumnName && x.TableName == column.TableName);
                                queries.Add(codeGenerator.GenerateDropKeyQuery(_mapper.Map<Key>(idx), table));
                            }

                            if (column.OldColumn.PrimaryKey && column.PrimaryKey == false)
                            {
                                var idx = dbTable.Keys.FirstOrDefault(x =>
                                    x.KeyColumn == column.ColumnName && x.TableName == column.TableName);
                                queries.Add(codeGenerator.GenerateDropKeyQuery(_mapper.Map<Key>(idx), table));
                            }
                        }
                        column.DefaultValue = null;
                        var mappedEntity = _mapper.Map<Column>(column);
                        string command = codeGenerator.GenerateModifyColumnQuery(mappedEntity, table);
                        queries.Add(command);
                        await ExecuteQueryOnRemote(queries, dbInformation);
                    }
                    else
                    {
                        throw new ValidationException(validationResult.ToString());
                    }
                }

            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }

        }
        public async Task DropColumnOnRemote(DeleteColumnDto column, DbInformation dbInformation)
        {
            try
            {
                if (SqlCodeGeneratorFactory.CreateGenerator(dbInformation.Provider) is MySqlCodeGenerator codeGenerator)
                {
                    DeleteColumnValidator validator = new DeleteColumnValidator();
                    var validationResult = validator.Validate(column);
                    if (validationResult.IsValid)
                    {
                        var databaseEntity = await GetTable(column.TableName, dbInformation);

                        var mappedEntity = _mapper.Map<Column>(column);
                        var table = _mapper.Map<Table>(databaseEntity);
                        var command = databaseEntity.Columns.Count == 1 ? codeGenerator.GenerateDropTableQuery(table) : codeGenerator.GenerateDropColumnQuery(mappedEntity, table);
                        await ExecuteQueryOnRemote(command, dbInformation);
                    }
                    else
                    {
                        throw new ValidationException(validationResult.ToString());
                    }
                }

            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }

        }
        public async Task CreateForeignKeyOnRemote(AddForeignKeyDto foreignKey, DbInformation dbInformation)
        {
            try
            {

                if (SqlCodeGeneratorFactory.CreateGenerator(dbInformation.Provider) is MySqlCodeGenerator codeGenerator)
                {
                    AddForeignKeyValidator validator = new AddForeignKeyValidator();
                    var validationResult = validator.Validate(foreignKey);
                    if (validationResult.IsValid)
                    {
                        var mappedForeignKey = _mapper.Map<ForeignKey>(foreignKey);

                        var dbTable = await GetTable(foreignKey.SourceTable, dbInformation);
                        var table = _mapper.Map<Table>(dbTable);

                        string command = codeGenerator.GenerateAddForeignKeyQuery(mappedForeignKey, table);

                        await ExecuteQueryOnRemote(command, dbInformation);
                    }
                    else
                    {
                        throw new ValidationException(validationResult.ToString());
                    }
                }



            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }

        }

        public async Task AlterForeignKeyOnRemote(UpdateForeignKeyDto foreignKey, DbInformation dbInformation)
        {
            try
            {

                if (SqlCodeGeneratorFactory.CreateGenerator(dbInformation.Provider) is MySqlCodeGenerator codeGenerator)
                {
                    UpdateForeignKeyValidator validator = new UpdateForeignKeyValidator();
                    var validationResult = validator.Validate(foreignKey);
                    if (validationResult.IsValid)
                    {
                        var mappedNew = _mapper.Map<ForeignKey>(foreignKey);
                        var mappedOld = _mapper.Map<ForeignKey>(foreignKey.OldForeignKey);
                        var dbTable = await GetTable(foreignKey.SourceTable, dbInformation);
                        var table = _mapper.Map<Table>(dbTable);

                        List<string> queries = new List<string>
                        {
                            codeGenerator.GenerateDropRelationQuery(mappedOld,table),
                            codeGenerator.GenerateAddForeignKeyQuery(mappedNew, table)
                        };

                        await ExecuteQueryOnRemote(queries, dbInformation);

                    }
                    else
                    {
                        throw new ValidationException(validationResult.ToString());
                    }
                }


            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }

        }

        public async Task DropForeignKeyOnRemote(DeleteForeignKeyDto foreignKey, DbInformation dbInformation)
        {
            try
            {

                if (SqlCodeGeneratorFactory.CreateGenerator(dbInformation.Provider) is MySqlCodeGenerator codeGenerator)
                {
                    DeleteForeignKeyValidator validator = new DeleteForeignKeyValidator();
                    var validationResult = validator.Validate(foreignKey);
                    if (validationResult.IsValid)
                    {
                        var mappedNew = _mapper.Map<ForeignKey>(foreignKey);

                        var dbTable = await GetTable(foreignKey.SourceTable, dbInformation);
                        var table = _mapper.Map<Table>(dbTable);

                        var queries = new List<string>
                        {
                            codeGenerator.GenerateDropRelationQuery(mappedNew, table)
                        };

                        await ExecuteQueryOnRemote(queries, dbInformation);

                    }
                    else
                    {
                        throw new ValidationException(validationResult.ToString());
                    }
                }


            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task ExecuteQueryOnRemote(string query, DbInformation dbInformation)
        {
            try
            {

                if (RemoteDbRepositoryFactory.CreateRepository(dbInformation.Provider) is MySqlRemoteDbRepository realDbRepository)
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
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }

        }
        public async Task ExecuteQueryOnRemote(List<string> queries, DbInformation dbInformation)
        {
            try
            {

                if (RemoteDbRepositoryFactory.CreateRepository(dbInformation.Provider) is MySqlRemoteDbRepository realDbRepository)
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
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }

        }
        public async Task ExecuteQueryOnRemote(string query)
        {
            try
            {

                if (RemoteDbRepositoryFactory.CreateRepository(ConfigurationSettings.ServerDbInformation.Provider) is MySqlRemoteDbRepository realDbRepository)
                    await realDbRepository.ExecuteQueryOnRemote(query);
                else
                    throw new NullReferenceException("Mysql Real Db Repository Referansına Ulaşlamadı");

            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }

        }
        public async Task<List<DetailTableDto>> GetTables(DbInformation dbInformation)
        {
            try
            {

                if (RemoteDbRepositoryFactory.CreateRepository(dbInformation.Provider) is MySqlRemoteDbRepository remoteDbRepository)
                {
                    var result = await remoteDbRepository.GetTables(dbInformation);
                    var mappedResults = _mapper.Map<List<DetailTableDto>>(result);
                    return mappedResults;

                }

                throw new NullReferenceException("Real Db Repository Referansına Ulaşılamadı");
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }



        }
        public async Task<DetailTableDto> GetTable(string tableName, DbInformation dbInformation)
        {

            try
            {
                
                if (RemoteDbRepositoryFactory.CreateRepository(dbInformation.Provider) is MySqlRemoteDbRepository remoteDbRepository)
                {
                    var result = await remoteDbRepository.GetTable(tableName, dbInformation);
                    var mappedResults = _mapper.Map<DetailTableDto>(result);
                    return mappedResults;
                }
                throw new NullReferenceException("Real Db Repository Referansına Ulaşılamadı");
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }



        }

        public async Task<List<string>> GetAvailableProviders()
        {
            List<string> providers = new List<string>()
            {
                "MySql"
            };
            return providers;
        }

        public async Task<DbInformation> GetAvailableServer(string provider)
        {
            if (provider.Equals("MySql"))
            {
                return ConfigurationSettings.ServerDbInformation;
            }

            return ConfigurationSettings.ServerDbInformation;
        }

        public async Task<string> GenerateDatabaseName(int userId)
        {
            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");
            guidString = guidString.Substring(0, 5);
            guidString += $"U{userId}DB";

            return guidString.ToLower();
        }

        public async Task<string> GenerateUserName(int userId)
        {
            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");
            guidString = guidString.Substring(0, 5);
            guidString += $"U{userId}ID";
            return guidString;
        }

        public async Task<string> GeneratePassword(int userId)
        {

            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");
            guidString = guidString.Substring(0, 5);
            guidString += $"U{userId}PW";
            return guidString;
        }
    }
}