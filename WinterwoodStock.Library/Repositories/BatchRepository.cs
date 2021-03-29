using WinterwoodStock.Library.Entities;
using WinterwoodStock.Library.Interfaces.Repositories;

namespace WinterwoodStock.Library.Repositories
{
    public class BatchRepository : GenericRepository<Batch>, IBatchRepository
    {
        public BatchRepository(WinterwoodDbContext context) : base(context)
        {
        }
    }
}
