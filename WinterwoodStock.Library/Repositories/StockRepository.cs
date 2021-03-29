using System.Linq;
using WinterwoodStock.Library.Entities;
using WinterwoodStock.Library.Interfaces.Repositories;

namespace WinterwoodStock.Library.Repositories
{
    public class StockRepository : GenericRepository<Stock>, IStockRepository
    {
        public StockRepository(WinterwoodDbContext context) : base(context)
        {
        }

        public Stock GetStocksByFruitAndVariety(string fruitType, string varietyType)
        {
            return _context.Stocks.FirstOrDefault(x => x.FruitType == fruitType && x.VarietyType == varietyType);
        }
    }
}
