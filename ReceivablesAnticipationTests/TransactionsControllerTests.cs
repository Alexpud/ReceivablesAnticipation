using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Repositories.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceivablesAnticipation;
using ReceivablesAnticipation.Controllers;
using ReceivablesAnticipation.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace ReceivablesAnticipationTests
{
    public class TransactionsControllerTests
    {
        private readonly AppDbContext _context;
        private readonly EFTransactionRepository _transactionRepository;
        private readonly EFTransactionAnticipationRepository _transactionAnticipationRepository;
        private readonly EFShopKeeperRepository _shopKeeperRepository;
        private TransactionsController _controller;

        public TransactionsControllerTests(ITestOutputHelper output)
        {
            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestCatalog")
                .Options;

            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            _context = new AppDbContext(dbOptions);
            _transactionRepository = new EFTransactionRepository(_context);
            _transactionAnticipationRepository = new EFTransactionAnticipationRepository(_context);
            _shopKeeperRepository = new EFShopKeeperRepository(_context);
            _controller = new TransactionsController
                (_transactionRepository, _transactionAnticipationRepository, mapper);
        }

        #region RequestAnticipationTest

        [Fact]
        public void RequestAnticipationTest()
        {
            ShopKeeper shopKeeper = new ShopKeeper()
            {
                Name = "Mario"
            };

            _shopKeeperRepository.Insert(shopKeeper);
            _shopKeeperRepository.SaveChanges();

            Transaction transaction = new Transaction()
            {
                AcquirerApproval = true,
                TransactionDate = DateTime.Now,
                PassThroughValue = 0,
                PassThroughDate = DateTime.Now,
                TransactionValue = 1000,
                InstalmentQuantity = 2
            };

            Transaction transaction2 = new Transaction()
            {
                AcquirerApproval = true,
                TransactionDate = DateTime.Now,
                PassThroughValue = 0,
                PassThroughDate = DateTime.Now,
                TransactionValue = 600,
                InstalmentQuantity = 4
            };


            Transaction transaction3 = new Transaction()
            {
                AcquirerApproval = true,
                TransactionDate = DateTime.Now,
                PassThroughValue = 0,
                PassThroughDate = DateTime.Now,
                TransactionValue = 600,
                InstalmentQuantity = 4
            };

            _context.Transactions.Add(transaction);
            _context.Transactions.Add(transaction2);
            _context.Transactions.Add(transaction3);
            _context.SaveChanges();

            RequestedTransactionsDTO dto = new RequestedTransactionsDTO()
            {
                ShopKeeperID = shopKeeper.ShopKeeperID,
                TransactionIDs = new List<int>()
                {
                    transaction.TransactionID, transaction2.TransactionID
                }
            };

            var result =  _controller.RequestAnticipation(dto) as CreatedAtActionResult;
            Assert.NotNull(result); 
        }
        #endregion

        #region GetTransactionAnticipationRequestTest

        [Fact]
        public void GetAnticipationRequestTest()
        {
            Transaction transaction = new Transaction()
            {
                AcquirerApproval = true,
                TransactionDate = DateTime.Now,
                PassThroughValue = 0,
                PassThroughDate = DateTime.Now,
                TransactionValue = 1000,
                InstalmentQuantity = 2
            };

            Transaction transaction2 = new Transaction()
            {
                AcquirerApproval = true,
                TransactionDate = DateTime.Now,
                PassThroughValue = 0,
                PassThroughDate = DateTime.Now,
                TransactionValue = 600,
                InstalmentQuantity = 4
            };


            Transaction transaction3 = new Transaction()
            {
                AcquirerApproval = true,
                TransactionDate = DateTime.Now,
                PassThroughValue = 0,
                PassThroughDate = DateTime.Now,
                TransactionValue = 600,
                InstalmentQuantity = 4
            };

            _context.Transactions.Add(transaction);
            _context.Transactions.Add(transaction2);
            _context.Transactions.Add(transaction3);
            _context.SaveChanges();

            RequestedTransactionsDTO dto = new RequestedTransactionsDTO()
            {
                TransactionIDs = new List<int>()
                {
                    transaction.TransactionID, transaction2.TransactionID
                }
            };

            var result = _controller.RequestAnticipation(dto) as CreatedAtActionResult;

            var result2 = _controller.GetTransactionAnticipationRequest(1) as OkObjectResult;
            Assert.NotNull(result2);
        }
        #endregion
    }
}
