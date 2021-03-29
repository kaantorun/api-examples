using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WinterwoodStock.Library.Entities;
using WinterwoodStock.Library.Interfaces.Repositories;
using WinterwoodStock.Library.Interfaces.Services;
using WinterwoodStock.Library.Models;

namespace WinterwoodStock.Library.Services
{
    // Add-Migration InitialCreate
    // Update-Database
    public class WinterwoodStockService : IWinterwoodStockService
    {
        private readonly ILogger<WinterwoodStockService> _logger;

        private readonly IUnitOfWork _uow;

        public WinterwoodStockService(ILogger<WinterwoodStockService> logger, IUnitOfWork uow)
        {
            _logger = logger;
            _uow = uow;
        }

        public bool Add(BatchModel batchModel)
        {
            bool added = true;

            var batchEntity = new Batch();

            try
            {
                _logger.LogInformation($"WinterwoodStockService -> Add -> ValidateBatch Check");

                ValideBatch(batchModel);

                _logger.LogInformation($"WinterwoodStockService -> Add -> ValidateBatch Success");

                batchEntity = new Batch
                {
                    CreatedDateTime = DateTime.UtcNow,
                    FruitType = batchModel.FruitType,
                    Quantity = batchModel.Quantity,
                    VarietyType = batchModel.VarietyType
                };

                _uow.Batches.Add(batchEntity);

                _logger.LogInformation($"WinterwoodStockService -> Add -> GetStocksByFruitAndVariety({batchModel.FruitType}, {batchModel.VarietyType})");

                Stock stock = _uow.Stocks.GetStocksByFruitAndVariety(batchModel.FruitType, batchModel.VarietyType);
                if (stock == null)
                {
                    _logger.LogInformation($"WinterwoodStockService -> Add -> GetStocksByFruitAndVariety -> Stock is empty)");

                    stock = new Stock
                    {
                        CreatedDateTime = DateTime.UtcNow,
                        FruitType = batchModel.FruitType,
                        Quantity = batchModel.Quantity,
                        VarietyType = batchModel.VarietyType
                    };

                    _uow.Stocks.Add(stock);

                    _logger.LogInformation($"WinterwoodStockService -> Add -> GetStocksByFruitAndVariety -> Stock Added {Newtonsoft.Json.JsonConvert.SerializeObject(stock)})");
                }
                else
                {
                    _logger.LogInformation($"WinterwoodStockService -> Add -> GetStocksByFruitAndVariety -> Stock Update -> Quantity Updated oldValue: {batchModel.Quantity}, newValue:{stock.Quantity})");

                    stock.Quantity += batchModel.Quantity;

                    _uow.Stocks.Update(stock);
                }

                _uow.Complete();
            }
            catch (Exception ex)
            {
                added = false;

                _logger.LogError($"Batch Add exception, " +
                    $"CreatedDateTime:{batchEntity.CreatedDateTime}, " +
                    $"FruitType:{batchEntity.FruitType}, " +
                    $"Quantity:{batchEntity.Quantity}, " +
                    $"VarietyType:{batchEntity.VarietyType}, " +
                    $"Exception: {ex}");
            }

            return added;
        }

        public bool Delete(int batchId)
        {
            bool deleted = true;

            try
            {
                if (batchId <= 0)
                {
                    _logger.LogWarning($"BatchId cannot be empty:{batchId}");
                    throw new Exception("BatchId cannot be empty!");
                }

                var existBatch = _uow.Batches.GetById(batchId);
                if (existBatch == null)
                {
                    _logger.LogWarning($"Batch Delete exception, Cannot find BatchId:{batchId}");
                    throw new Exception("Batch could not found!");
                }

                Stock stock = _uow.Stocks.GetStocksByFruitAndVariety(existBatch.FruitType, existBatch.VarietyType);
                if (stock != null)
                {
                    stock.Quantity -= existBatch.Quantity;

                    _uow.Stocks.Update(stock);
                }
                else
                {
                    throw new Exception($"Stock cannot be updated! Stock cannot be found with FruitType:{existBatch.FruitType} and VarietyType:{existBatch.VarietyType}");
                }

                _uow.Batches.Remove(existBatch);

                _uow.Complete();
            }
            catch (Exception ex)
            {
                deleted = false;
                _logger.LogError($"Batch Delete exception, BatchId:{batchId}, Exception: {ex}");
            }

            return deleted;
        }

        public BatchModel GetBatchById(int batchId)
        {
            var batchModel = new BatchModel();

            try
            {
                _logger.LogInformation($"WinterwoodStockService -> GetBatchById -> GetById Request)");

                Batch batch = _uow.Batches.GetById(batchId);

                if (batch == null)
                {
                    _logger.LogError($"WinterwoodStockService -> GetBatchById -> GetById -> Batch Cannot be found batchId: {batchId})");

                    throw new Exception($"Batch cannot be found! BatchId:{batchId}");
                }

                batchModel = new BatchModel
                {
                    BatchId = batch.BatchId,
                    FruitType = batch.FruitType,
                    Quantity = batch.Quantity,
                    VarietyType = batch.VarietyType
                };

                _logger.LogInformation($"WinterwoodStockService -> GetBatchById -> GetById -> Response -> Batch found batch: {Newtonsoft.Json.JsonConvert.SerializeObject(batch)})");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"WinterwoodStockService -> GetBatchById -> Exception: {ex})");
            }

            return batchModel;
        }

        public List<BatchModel> GetAllBatch()
        {
            List<BatchModel> batchModel = new List<BatchModel>();
            List<Batch> stocks = new List<Batch>();

            try
            {
                _logger.LogInformation($"WinterwoodStockService -> GetAllBatch -> GetAll Request)");

                stocks = _uow.Batches.GetAll().ToList();

                if (stocks == null)
                {
                    _logger.LogError($"WinterwoodStockService -> GetAllBatch -> GetAll -> Batches Cannot be found!");
                    throw new Exception($"Stocks cannot be found!");
                }

                batchModel = stocks.Select(x => new BatchModel
                {
                    BatchId = x.BatchId,
                    FruitType = x.FruitType,
                    Quantity = x.Quantity,
                    VarietyType = x.VarietyType
                }).ToList();

                _logger.LogInformation($"WinterwoodStockService -> GetAllBatch -> GetAll -> Response -> Batches found: {Newtonsoft.Json.JsonConvert.SerializeObject(batchModel)})");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"WinterwoodStockService -> GetAllBatch -> Exception: {ex})");
            }

            return batchModel;
        }

        public List<StockModel> GetAllStock()
        {
            List<StockModel> stockModel = new List<StockModel>();
            List<Stock> stocks = new List<Stock>();

            try
            {
                _logger.LogInformation($"WinterwoodStockService -> GetAllStock -> GetAll Request)");

                stocks = _uow.Stocks.GetAll().ToList();

                if (stocks == null)
                {
                    _logger.LogError($"WinterwoodStockService -> GetAllStock -> GetAll -> Stocks Cannot be found!");
                    throw new Exception($"Stocks cannot be found!");
                }

                stockModel = stocks.Select(x => new StockModel
                {
                    StockId = x.StockId,
                    FruitType = x.FruitType,
                    Quantity = x.Quantity,
                    VarietyType = x.VarietyType
                }).ToList();

                _logger.LogInformation($"WinterwoodStockService -> GetAllStock -> GetAll -> Response -> Stocks found: {Newtonsoft.Json.JsonConvert.SerializeObject(stockModel)})");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"WinterwoodStockService -> GetAllStock -> Exception: {ex})");
            }

            return stockModel;
        }

        public bool Update(BatchModel batchModel)
        {
            bool updated = true;

            try
            {
                _logger.LogInformation($"WinterwoodStockService -> Update -> ValidateBatch Check");

                ValideBatch(batchModel);

                _logger.LogInformation($"WinterwoodStockService -> Update -> ValidateBatch Success");

                Batch currentBatch = _uow.Batches.GetById(batchModel.BatchId);
                if (currentBatch == null)
                {
                    _logger.LogError($"WinterwoodStockService -> Update -> Batch cannot be found batchId:{batchModel.BatchId}");

                    throw new Exception($"Batch cannot be updated! Record cannot be found! BatchId:{batchModel.BatchId}");
                }

                _logger.LogInformation($"WinterwoodStockService -> Update -> GetStocksByFruitAndVariety -> Request");

                Stock currentStock = _uow.Stocks.GetStocksByFruitAndVariety(batchModel.FruitType, batchModel.VarietyType);
                if (currentStock != null)
                {
                    _logger.LogInformation($"WinterwoodStockService -> Update -> " +
                                           $"GetStocksByFruitAndVariety -> Stock Update -> " +
                                           $"batch Quantity oldValue: {currentBatch.Quantity}, newValue:{batchModel.Quantity})");

                    //remove the old quantity
                    currentStock.Quantity = currentStock.Quantity - currentBatch.Quantity;

                    //add the new quantity
                    currentStock.Quantity = currentStock.Quantity + batchModel.Quantity;

                    currentStock.UpdatedDateTime = DateTime.UtcNow;

                    _uow.Stocks.Update(currentStock);

                    _logger.LogInformation($"WinterwoodStockService -> Update -> GetStocksByFruitAndVariety -> Stock Update -> {Newtonsoft.Json.JsonConvert.SerializeObject(currentStock)}");

                }
                else
                {
                    _logger.LogError($"Batch cannot be updated! Stock cannot be found with FruitType:{batchModel.FruitType} and VarietyType:{batchModel.VarietyType}");

                    throw new Exception($"Batch cannot be updated! Stock cannot be found with FruitType:{batchModel.FruitType} and VarietyType:{batchModel.VarietyType}");
                }

                currentBatch.Quantity = batchModel.Quantity;
                currentBatch.UpdatedDateTime = DateTime.UtcNow;

                _uow.Batches.Update(currentBatch);

                _uow.Complete();

                _logger.LogInformation($"WinterwoodStockService -> Update -> Batch Update -> {Newtonsoft.Json.JsonConvert.SerializeObject(currentBatch)}");

            }
            catch (Exception ex)
            {
                updated = false;
                _logger.LogError($"Batch Update exception, BatchId= {batchModel.BatchId}, Exception: {ex}");
            }

            return updated;
        }

        private void ValideBatch(BatchModel batchModel)
        {
            if (batchModel.Quantity < 1)
            {
                _logger.LogWarning("Quantity cannot be less than 1!");

                throw new Exception("Quantity cannot be less than 1!");
            }

            if (string.IsNullOrEmpty(batchModel.FruitType))
            {
                _logger.LogWarning("FruitType must not empty!");

                throw new Exception("FruitType must not empty!");
            }

            if (string.IsNullOrEmpty(batchModel.VarietyType))
            {
                _logger.LogWarning("VarietyType must not empty!");

                throw new Exception("VarietyType must not empty!");
            }
        }
    }
}
