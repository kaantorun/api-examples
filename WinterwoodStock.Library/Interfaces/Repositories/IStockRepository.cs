using WinterwoodStock.Library.Entities;

namespace WinterwoodStock.Library.Interfaces.Repositories
{
    public interface IStockRepository : IGenericRepository<Stock>
    {
        Stock GetStocksByFruitAndVariety(string fruitType, string varietyType);
    }
}
