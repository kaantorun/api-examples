using WinterwoodStock.Library.Interfaces.Repositories;

namespace WinterwoodStock.Library.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WinterwoodDbContext _context;

        public IBatchRepository Batches { get; set; }
        public IStockRepository Stocks { get; set; }
        public IUserRepository Users { get; set; }

        public UnitOfWork(
            WinterwoodDbContext context,
            IBatchRepository Batches,
            IStockRepository Stocks,
            IUserRepository Users)
        {
            _context = context;
            this.Batches = Batches;
            this.Stocks = Stocks;
            this.Users = Users;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
