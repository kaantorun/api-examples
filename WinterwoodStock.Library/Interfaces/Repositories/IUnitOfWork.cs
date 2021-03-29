using System;

namespace WinterwoodStock.Library.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IBatchRepository Batches { get; }
        IStockRepository Stocks { get; }
        IUserRepository Users { get; }
        int Complete();
    }
}
