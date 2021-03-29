using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WinterwoodStock.Library.Interfaces.Services;
using WinterwoodStock.Library.Models;

namespace WinterwoodStockApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WinterwoodStockController : ControllerBase
    {
        private readonly ILogger<WinterwoodStockController> _logger;

        private readonly IWinterwoodStockService _winterwoodStockService;
        public WinterwoodStockController(ILogger<WinterwoodStockController> logger, IWinterwoodStockService winterwoodStockService)
        {
            _logger = logger;

            _winterwoodStockService = winterwoodStockService;
        }

        [HttpGet("GetAllStock")]
        public List<StockModel> GetAllStock()
        {
            _logger.LogInformation("Method Request -> GetAllStock");

            List<StockModel> stocks = _winterwoodStockService.GetAllStock();

            _logger.LogInformation($"Method Response -> GetAllStock -> {Newtonsoft.Json.JsonConvert.SerializeObject(stocks)}");

            return stocks;
        }

        [HttpGet("GetAllBatch")]
        public List<BatchModel> GetAllBatch()
        {
            _logger.LogInformation("Method Request -> GetAllBatch");

            List<BatchModel> batches = _winterwoodStockService.GetAllBatch();

            _logger.LogInformation($"Method Response -> GetAllBatch -> {Newtonsoft.Json.JsonConvert.SerializeObject(batches)}");

            return batches;
        }

        [HttpPost("GetBatchById")]
        public BatchModel GetBatchById(int batchId)
        {
            _logger.LogInformation($"Method Request -> GetBatchById -> BatchId:{batchId}");

            BatchModel batchModel = _winterwoodStockService.GetBatchById(batchId);

            _logger.LogInformation($"Method Response -> GetBatchById -> {Newtonsoft.Json.JsonConvert.SerializeObject(batchModel)}");


            return batchModel;
        }

        [HttpPost]
        public bool Post(BatchModel batchModel)
        {
            _logger.LogInformation($"Method Request -> Post -> {Newtonsoft.Json.JsonConvert.SerializeObject(batchModel)}");

            bool added = _winterwoodStockService.Add(batchModel);

            _logger.LogInformation($"Method Response -> Post -> {added}");

            return added;
        }

        [HttpPut]
        public bool Put(BatchModel batchModel)
        {
            _logger.LogInformation($"Method Request -> Put -> {Newtonsoft.Json.JsonConvert.SerializeObject(batchModel)}");

            bool updated = _winterwoodStockService.Update(batchModel);

            _logger.LogInformation($"Method Response -> Put -> {updated}");

            return updated;
        }

        [HttpDelete]
        public bool Delete(int batchId)
        {
            _logger.LogInformation($"Method Request -> Delete -> BatchId:{batchId}");

            bool deleted = _winterwoodStockService.Delete(batchId);

            _logger.LogInformation($"Method Response -> Delete -> {deleted}");

            return deleted;
        }
    }
}
