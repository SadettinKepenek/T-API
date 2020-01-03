using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using T_API.BLL.Abstract;
using T_API.Core.DAL.Concrete;
using T_API.Core.Exception;
using T_API.DAL.Abstract;
using T_API.DAL.Concrete;
using T_API.Entity.Concrete;

namespace T_API.BLL.Concrete
{
    public class DataManager : IDataService
    {
        private IRealDbRepositoryFactory _realDbRepositoryFactory;

        public DataManager(IRealDbRepositoryFactory realDbRepositoryFactory)
        {
            _realDbRepositoryFactory = realDbRepositoryFactory;
        }

        public async Task<string> Get(string tableName, DbInformation dbInformation)
        {
            try
            {

                if (_realDbRepositoryFactory.CreateRepository(dbInformation.Provider) is MySqlRealDbRepository realDbRepository)
                {
                    using TransactionScope scope = new TransactionScope();

                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine($"USE {dbInformation.DatabaseName};\n");
                    stringBuilder.AppendLine($"Select * from {tableName}");
                    var dt = await realDbRepository.Get(stringBuilder.ToString(), dbInformation);
                    string result = JsonConvert.SerializeObject(dt);
                    scope.Complete();
                    return result;
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

        public async Task Add(string tableName, DbInformation dbInformation, IFormCollection form)
        {
            try
            {

                if (_realDbRepositoryFactory.CreateRepository(dbInformation.Provider) is MySqlRealDbRepository realDbRepository)
                {
                    var table = await realDbRepository.GetTable(tableName, dbInformation.DatabaseName);

                    if (table != null)
                    {
                        using TransactionScope scope = new TransactionScope();

                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine($"USE {dbInformation.DatabaseName};\n");
                        stringBuilder.AppendLine($"INSERT INTO {tableName} ");
                        string valuesString = "VALUES (";
                        stringBuilder.Append($"(");
                        var columns = table.Columns.Where(x => x.PrimaryKey == false).ToList();
                        foreach (Column column in columns)
                        {
                            stringBuilder.Append($"{column.ColumnName}");

                            valuesString += form[column.ColumnName];
                            
                            if (columns.IndexOf(column) != columns.Count - 1)
                            {
                                valuesString += ",";
                                stringBuilder.Append(",");
                            }
                        }

                        stringBuilder.Append($")");
                        valuesString += ")";
                        stringBuilder.AppendLine(valuesString);
                        await realDbRepository.ExecuteQueryOnRemote(stringBuilder.ToString(), dbInformation);
                        scope.Complete();
                    }
                    else
                    {
                        throw new NullReferenceException("Tablo bulunamadı");
                    }
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
    }
}