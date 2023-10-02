using Microsoft.EntityFrameworkCore.Storage;
using Service.Common.Interfaces.Repositories;

namespace Service.Repository
{
    internal class RepositoryTransaction : IRepositoryTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public RepositoryTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;            
        }
        public void Commit() => _transaction.Commit();

        public Task CommitAsync() => _transaction.CommitAsync();

        public void Dispose() => _transaction.Dispose();

        public ValueTask DisposeAsync() => _transaction.DisposeAsync();

        public void Rollback() => _transaction.Rollback();

        public Task RollbackAsync() => _transaction.RollbackAsync();
    }
}
