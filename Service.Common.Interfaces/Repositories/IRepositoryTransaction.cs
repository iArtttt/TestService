namespace Service.Common.Interfaces.Repositories
{
    public interface IRepositoryTransaction : IDisposable, IAsyncDisposable
    {
        void Commit();
        Task CommitAsync();
        void Rollback();
        Task RollbackAsync();
    }
}
