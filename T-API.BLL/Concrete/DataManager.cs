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
using Newtonsoft.Json.Linq;
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

        public DataManager()
        {
        }

        public async Task<string> Get(string tableName, DbInformation dbInformation)
        {
            try
            {

                if (RemoteDbRepositoryFactory.CreateRepository(dbInformation.Provider) is MySqlRemoteDbRepository realDbRepository)
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

        public async Task Add(string tableName, DbInformation dbInformation, JObject jObject)
        {
            try
            {

                if (RemoteDbRepositoryFactory.CreateRepository(dbInformation.Provider) is MySqlRemoteDbRepository realDbRepository)
                {
                    var table = await realDbRepository.GetTable(tableName, dbInformation);

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

                            if (jObject[column.ColumnName].Type == JTokenType.String)
                                valuesString += $"'{jObject[column.ColumnName]}'";
                            else
                                valuesString += jObject[column.ColumnName];

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

        public async Task Update(string tableName, DbInformation dbInformation, JObject jObject)
        {
            try
            {

                if (RemoteDbRepositoryFactory.CreateRepository(dbInformation.Provider) is MySqlRemoteDbRepository realDbRepository)
                {
                    var table = await realDbRepository.GetTable(tableName, dbInformation);

                    if (table != null)
                    {
                        using TransactionScope scope = new TransactionScope();

                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine($"USE {dbInformation.DatabaseName};\n");
                        stringBuilder.AppendLine($"UPDATE {tableName} SET \n");
                        var pkColumn = table.Columns.FirstOrDefault(x => x.PrimaryKey);
                        if (pkColumn != null)
                        {
                            if (jObject[pkColumn.ColumnName] == null)
                                throw new NullReferenceException(
                                    "Herhangi bir primary key bulunamadığı için default update methodu kullanılamaz.");
                            foreach (Column column in table.Columns.Where(x => x.PrimaryKey == false))
                            {
                                if (jObject[column.ColumnName] != null)
                                {
                                    stringBuilder.AppendLine(jObject[column.ColumnName].Type == JTokenType.String
                                        ? $"\t{column.ColumnName}='{jObject[column.ColumnName]}' \n"
                                        : $"\t{column.ColumnName}={jObject[column.ColumnName]} \n");
                                    stringBuilder.Append(",");
                                }
                                
                            }

                            stringBuilder[^1] = stringBuilder[^1] == ',' ? ' ' : stringBuilder[^1];


                            string filter = jObject[pkColumn.ColumnName].Type == JTokenType.String
                                ? $"WHERE {pkColumn.ColumnName}='{jObject[pkColumn.ColumnName]}' \n"
                                : $"WHERE {pkColumn.ColumnName}={jObject[pkColumn.ColumnName]} \n";
                            stringBuilder.AppendLine(filter);

                            await realDbRepository.ExecuteQueryOnRemote(stringBuilder.ToString(), dbInformation);
                            scope.Complete();
                        }
                        else
                        {
                            throw new NullReferenceException(
                                "Herhangi bir primary key bulunamadığı için default update methodu kullanılamaz.");
                        }
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