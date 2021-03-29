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

        /// <summary>
        /// Gets all the stocks by batches of the system
        /// It can be used after logging the system
        /// Authentication uses Token
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllStock")]
        public List<StockModel> GetAllStock()
        {
            _logger.LogInformation("Method Request -> GetAllStock");

            List<StockModel> stocks = _winterwoodStockService.GetAllStock();

            _logger.LogInformation($"Method Response -> GetAllStock -> {Newtonsoft.Json.JsonConvert.SerializeObject(stocks)}");

            return stocks;
        }

        /// <summary>
        /// Gets all the batches of the system
        /// It can be used after logging the system
        /// Authentication uses Token
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllBatch")]
        public List<BatchModel> GetAllBatch()
        {
            _logger.LogInformation("Method Request -> GetAllBatch");

            List<BatchModel> batches = _winterwoodStockService.GetAllBatch();

            _logger.LogInformation($"Method Response -> GetAllBatch -> {Newtonsoft.Json.JsonConvert.SerializeObject(batches)}");

            return batches;
        }

        /// <summary>
        /// Gets a detail of a Batch by Id
        /// It can be used after logging the system
        /// Authentication uses Token
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetBatchById")]
        public BatchModel GetBatchById(int batchId)
        {
            _logger.LogInformation($"Method Request -> GetBatchById -> BatchId:{batchId}");

            BatchModel batchModel = _winterwoodStockService.GetBatchById(batchId);

            _logger.LogInformation($"Method Response -> GetBatchById -> {Newtonsoft.Json.JsonConvert.SerializeObject(batchModel)}");


            return batchModel;
        }

        /// <summary>
        /// It creates a new batch
        /// It can be used after logging the system
        /// Authentication uses Token
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool Post(BatchModel batchModel)
        {
            _logger.LogInformation($"Method Request -> Post -> {Newtonsoft.Json.JsonConvert.SerializeObject(batchModel)}");

            bool added = _winterwoodStockService.Add(batchModel);

            _logger.LogInformation($"Method Response -> Post -> {added}");

            return added;
        }

        /// <summary>
        /// It updates the batch
        /// It can be used after logging the system
        /// Authentication uses Token
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public bool Put(BatchModel batchModel)
        {
            _logger.LogInformation($"Method Request -> Put -> {Newtonsoft.Json.JsonConvert.SerializeObject(batchModel)}");

            bool updated = _winterwoodStockService.Update(batchModel);

            _logger.LogInformation($"Method Response -> Put -> {updated}");

            return updated;
        }

        /// <summary>
        /// It deletes the new batch
        /// It can be used after logging the system
        /// Authentication uses Token
        /// </summary>
        /// <returns></returns>
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
