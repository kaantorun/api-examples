using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using WinterwoodStock.Library.Entities;
using WinterwoodStock.Library.Interfaces.Repositories;
using WinterwoodStock.Library.Interfaces.Services;
using WinterwoodStock.Library.Models;
using WinterwoodStock.Library.Repositories;
using WinterwoodStock.Library.Services;
using Xunit;

namespace WinterwoodStock.Test
{
    public class WinterwoodStockServiceTests
    {
        private readonly IWinterwoodStockService _winterwoodStockService;
        private readonly Mock<ILogger<WinterwoodStockService>> _logger;
        private readonly Mock<IUnitOfWork> _uowMock;

        public WinterwoodStockServiceTests()
        {
            //IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build(); 

            _logger = new Mock<ILogger<WinterwoodStockService>>();

            _uowMock = new Mock<IUnitOfWork>();

            _winterwoodStockService = new WinterwoodStockService(_logger.Object, _uowMock.Object);
        }

        #region GetAllStock

        [Fact]
        public void Should_GetAllStock_ValidParameter()
        {
            _uowMock.Setup(u => u.Stocks.GetAll()).Returns(() => new List<Stock> { new Stock { StockId = 7 } });

            var result = _winterwoodStockService.GetAllStock();
            Assert.True(result != null && result.Count != 0);
        }

        [Fact]
        public void Should_Not_GetAllStock_When_Stock_GetAll_ReturnsNull()
        {
            _uowMock.Setup(u => u.Stocks.GetAll()).Returns(() => null);

            var result = _winterwoodStockService.GetAllStock();
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void Should_Not_AllStock_When_Stocks_GetAll_ThrowException()
        {
            _uowMock.Setup(u => u.Stocks.GetAll()).Returns(() => throw new Exception("test"));

            var result = _winterwoodStockService.GetAllStock();
            Assert.True(result.Count == 0);
        }

        #endregion GetAllStock

        #region GetAllBatch

        [Fact]
        public void Should_GetAllBatch_ValidParameter()
        {
            _uowMock.Setup(u => u.Batches.GetAll()).Returns(() => new List<Batch> { new Batch { BatchId = 8 } });

            var result = _winterwoodStockService.GetAllBatch();
            Assert.True(result != null && result.Count != 0);
        }

        [Fact]
        public void Should_Not_GetAllBatch_When_Batch_GetAll_ReturnsNull()
        {
            _uowMock.Setup(u => u.Batches.GetAll()).Returns(() => null);

            var result = _winterwoodStockService.GetAllBatch();
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void Should_Not_GetAllBatch_When_Batch_GetAll_ThrowException()
        {
            _uowMock.Setup(u => u.Batches.GetAll()).Returns(() => throw new Exception("test"));

            var result = _winterwoodStockService.GetAllBatch();
            Assert.True(result.Count == 0);
        }

        #endregion GetAllBatch

        #region GetBatchById

        [Fact]
        public void Should_GetBatchById_When_ValidParameter()
        {
            _uowMock.Setup(u => u.Batches.GetById(It.IsAny<int>())).Returns(new Batch { BatchId = 5 });

            var result = _winterwoodStockService.GetBatchById(1);
            Assert.True(result.BatchId != 0);
        }

        [Fact]
        public void Should_Not_GetBatchById_When_Batches_GetById_ReturnsNull()
        {
            _uowMock.Setup(u => u.Batches.GetById(It.IsAny<int>())).Returns((Batch)null);

            var result = _winterwoodStockService.GetBatchById(1);
            Assert.True(result.BatchId == 0);
        }

        [Fact]
        public void Should_Not_GetBatchById_When_Batches_GetById_ThrowException()
        {
            _uowMock.Setup(u => u.Batches.GetById(It.IsAny<int>())).Returns(() => throw new Exception("test"));

            var result = _winterwoodStockService.GetBatchById(1);
            Assert.True(result.BatchId == 0);
        }

        #endregion GetBatchById
    }
}
