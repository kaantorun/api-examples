using System.Collections.Generic;
using WinterwoodStock.Library.Models;

namespace WinterwoodStock.Library.Interfaces.Services
{
    public interface IWinterwoodStockService
    {
        bool Add(BatchModel batchModel);
        bool Update(BatchModel batchModel);
        bool Delete(int batchId);

        BatchModel GetBatchById(int batchId);
        List<BatchModel> GetAllBatch();
        List<StockModel> GetAllStock();
    }
}
