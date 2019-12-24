﻿using System.Threading.Tasks;
using T_API.Core.DTO.Database;
using T_API.Core.DTO.Table;
using T_API.Entity.Concrete;

namespace T_API.BLL.Abstract
{
    public interface IRealDbService
    {
        Task CreateDatabaseOnRemote(AddDatabaseDto database);
        Task CreateTableOnRemote(AddTableDto database);
        Task CreateColumnOnRemote(Database database);
    }
}