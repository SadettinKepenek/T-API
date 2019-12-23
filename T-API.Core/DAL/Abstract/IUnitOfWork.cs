using System;
using System.Data;

namespace T_API.Core.DAL.Abstract
{
    public interface IUnitOfWork:IDisposable
    {
        IDbCommand CreateCommand();
        IDbCommand CreateCommand(string sql);
        void SaveChanges();
    }
}