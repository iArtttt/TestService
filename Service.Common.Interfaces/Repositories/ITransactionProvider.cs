using System.Data;

namespace Service.Common.Interfaces.Repositories
{
    public interface ITransactionProvider
    {
        IRepositoryTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task<IRepositoryTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
