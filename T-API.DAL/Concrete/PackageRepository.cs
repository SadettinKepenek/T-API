using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using T_API.Core.DAL.Concrete;
using T_API.Core.Exception;
using T_API.Core.Settings;
using T_API.DAL.Abstract;
using T_API.Entity.Concrete;

namespace T_API.DAL.Concrete
{
    public class PackageRepository : IPackageRepository
    {
        public async Task<List<DatabasePackage>> Get()
        {
            try
            {
                using var conn = DbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
                string sql = "Select * from databasepackages";
                var cmd = conn.CreateCommand(sql);
                using (cmd)
                {

                    var sqlReader = cmd.ExecuteReader();


                    DataTable dt = new DataTable();
                    dt.Load(sqlReader);
                    List<DatabasePackage> databasePackages = new List<DatabasePackage>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var dataRow = dt.Rows[i];
                        var databasePackage = ProcessPackageEntity(dataRow);
                        databasePackages.Add(databasePackage);
                    }

                    return databasePackages;
                }


            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);

            }
        }

        private DatabasePackage ProcessPackageEntity(DataRow dataRow)
        {
            var package = new DatabasePackage()
            {
                PackageId = Convert.ToInt32(dataRow["PackageId"]),
                IsApiSupport = Convert.ToBoolean(dataRow["IsApiSupport"]),
                IsStorageSupport = Convert.ToBoolean(dataRow["IsStorageSupport"]),
                IsAuthSupport = Convert.ToBoolean(dataRow["IsAuthSupport"]),
                IsJobSupport = Convert.ToBoolean(dataRow["IsJobSupport"]),
                IsTriggerSupport = Convert.ToBoolean(dataRow["IsTriggerSupport"]),
                IsStoredProcedureSupport = Convert.ToBoolean(dataRow["IsStoredProcedureSupport"]),
                IsUserDefinedFunctionSupport = Convert.ToBoolean(dataRow["IsUserDefinedFunctionSupport"]),
                IsViewSupport = Convert.ToBoolean(dataRow["IsViewSupport"]),
                MaxJobCount = Convert.ToInt32(dataRow["MaxJobCount"]),
                ApiRequestCount = Convert.ToInt32(dataRow["ApiRequestCount"]),
                MaxColumnPerTable = Convert.ToInt32(dataRow["MaxColumnPerTable"]),
                MaxStoredProcedureCount = Convert.ToInt32(dataRow["MaxStoredProcedureCount"]),
                MaxTableCount = Convert.ToInt32(dataRow["MaxTableCount"]),
                MaxTriggerCount = Convert.ToInt32(dataRow["MaxTriggerCount"]),
                MaxUserDefinedFunctionCount = Convert.ToInt32(dataRow["MaxUserDefinedFunctionCount"]),
                MaxViewCount = Convert.ToInt32(dataRow["MaxViewCount"]),
                PackageName = dataRow["PackageName"] as string
            };
            return package;
        }

        public async Task<DatabasePackage> GetById(int id)
        {
            try
            {
                using var conn = DbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
                string sql = "Select * from databasepackages where PackageId=@PackageId";
                var cmd = conn.CreateCommand(sql);
                using (cmd)
                {
                    cmd.AddParameter("PackageId", id);
                    var sqlReader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sqlReader);
                    var databasePackage = ProcessPackageEntity(dt.Rows[0]);
                    return databasePackage;
                }


            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);

            }
        }

        public async Task<DatabasePackage> GetByName(string id)
        {
            try
            {
                using var conn = DbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
                string sql = "Select * from databasepackages where PackageName=@PackageName";
                var cmd = conn.CreateCommand(sql);
                using (cmd)
                {
                    cmd.AddParameter("PackageName", id);
                    var sqlReader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sqlReader);
                    var package = ProcessPackageEntity(dt.Rows[0]);
                    return package;
                }


            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);

            }
        }

        public async Task Add(DatabasePackage package)
        {
            try
            {
                using var conn = DbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
                string sql = "INSERT INTO databasepackages VALUES (@PackageName,@IsApiSupport,@IsAuthSupport," +
                             "@IsStorageSupport,@IsViewSupport,@IsStoredProcedureSupport,@IsUserDefinedFunctionSupport,@IsTriggerSupport," +
                             "@IsJobSupport,@ApiRequestCount,@MaxColumnPerTable,@MaxTableCount,@MaxStoredProcedureCount,@MaxTriggerCount,@MaxJobCount," +
                             "@MaxViewCount)";
                var cmd = conn.CreateCommand(sql);
                using (cmd)
                {

                    cmd.AddParameter("PackageName",package.PackageName);
                    cmd.AddParameter("IsApiSupport",package.IsApiSupport);
                    cmd.AddParameter("IsAuthSupport", package.IsAuthSupport);
                    cmd.AddParameter("IsViewSupport", package.IsViewSupport);
                    cmd.AddParameter("IsStorageSupport", package.IsStorageSupport);
                    cmd.AddParameter("IsStoredProcedureSupport", package.IsStoredProcedureSupport);
                    cmd.AddParameter("IsUserDefinedFunctionSupport", package.IsUserDefinedFunctionSupport);
                    cmd.AddParameter("IsTriggerSupport", package.IsTriggerSupport);
                    cmd.AddParameter("IsJobSupport", package.IsJobSupport);
                    cmd.AddParameter("ApiRequestCount", package.ApiRequestCount);
                    cmd.AddParameter("MaxColumnPerTable", package.MaxColumnPerTable);
                    cmd.AddParameter("MaxTableCount", package.MaxTableCount);
                    cmd.AddParameter("MaxStoredProcedureCount", package.MaxStoredProcedureCount);
                    cmd.AddParameter("MaxTriggerCount", package.MaxTriggerCount);
                    cmd.AddParameter("MaxJobCount", package.MaxJobCount);
                    cmd.AddParameter("MaxViewCount", package.MaxViewCount);

                    cmd.ExecuteNonQuery();


                }


            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);

            }
        }

        public Task Update(DatabasePackage package)
        {
            throw new System.NotImplementedException();
        }

        public async Task Delete(DatabasePackage package)
        {
            try
            {
                using var conn = DbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
                string sql = "Delete from databasepackages where PackageId=@PackageId";
                var cmd = conn.CreateCommand(sql);
                using (cmd)
                {

                   
                    cmd.AddParameter("PackageId", package.PackageId);

                    cmd.ExecuteNonQuery();


                }


            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);

            }
        }
    }
}