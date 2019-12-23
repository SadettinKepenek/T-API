using System;
using System.Data;
using T_API.Core.DAL.Abstract;

namespace T_API.Core.DAL.Concrete
{
    public class AdoNetUnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private bool _ownsConnection;
        private IDbTransaction _transaction;
        
        public AdoNetUnitOfWork(DbInformation information, bool ownsConnection)
        {
            _connection = DbConnectionFactory.CreateConnection(information);
            _ownsConnection = ownsConnection;
            _transaction = _connection.BeginTransaction();
        }

        public IDbCommand CreateCommand()
        {
            var command = _connection.CreateCommand();
            command.Transaction = _transaction;
            return command;
        }

        public IDbCommand CreateCommand(string sql)
        {
            var command = _connection.CreateCommand(sql);
            command.Transaction = _transaction;
            return command;
        }

        public void SaveChanges()
        {
            if (_transaction == null)
                throw new InvalidOperationException
                    ("Transaction have already been committed. Check your transaction handling.");

            _transaction.Commit();
            _transaction = null;
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }

            if (_connection != null && _ownsConnection)
            {
                _connection.Close();
                _connection = null;
            }
        }
    }
}