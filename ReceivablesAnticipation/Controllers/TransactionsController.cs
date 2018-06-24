using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReceivablesAnticipation.DTOs;

namespace ReceivablesAnticipation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionAnticipationRepository _transactionAnticipationRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        #region Constructor

        public TransactionsController(ITransactionRepository transactionRepository, 
            ITransactionAnticipationRepository transactionAnticipationRepository, 
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _transactionAnticipationRepository = transactionAnticipationRepository;
            _mapper = mapper;
        }

        #endregion

        #region RequestAnticipation

        [HttpPost]
        [Route("Anticipation")]
        public IActionResult RequestAnticipation(RequestedTransactionsDTO dto)
        {
            List<Transaction> transactions = new List<Transaction>();
            decimal transactionsAnticipationValue = 0;
            decimal totalTransactionValue = 0;

            // Get transactions
            foreach (var transactionID in dto.TransactionIDs)
            {
                var transaction = _transactionRepository.ObtainById(transactionID);
                if (transaction != null)
                {
                    decimal instalmentValue = transaction.TransactionValue / transaction.InstalmentQuantity;
                    totalTransactionValue += transaction.TransactionValue;
                    transactionsAnticipationValue += ((instalmentValue * (decimal)3.8) / 100) * transaction.InstalmentQuantity;
                    transactions.Add(transaction);
                }
            }

            TransactionAnticipation transactionAnticipation = new TransactionAnticipation()
            {
                AnticipationResult = null,
                Status = 1,
                SolicitationDate = DateTime.Now,
                TotalPassThroughValue = totalTransactionValue - transactionsAnticipationValue,
                TotalTransactionValue = totalTransactionValue,
                AnalysisDate = null,
                Transactions = transactions
            };

            _transactionAnticipationRepository.Insert(transactionAnticipation);
            int result = _transactionAnticipationRepository.SaveChanges();
            if (result == 0)
                return StatusCode((int)HttpStatusCode.NotModified);

            return CreatedAtAction("GetAnticipationRequest", 
                new { transactionAnticipationID = transactionAnticipation.TransactionAnticipationID}, transactionAnticipation);
        }

        #endregion

        #region GetAnticipationRequest

        [HttpGet(Name = "GetAnticipationRequest")]
        [Route("Anticipation/{transactionAnticipationID}")]
        public IActionResult GetAnticipationRequest(int transactionAnticipationID)
        {
            TransactionAnticipation transactionAnticipation = _transactionAnticipationRepository
                .ObtainById(transactionAnticipationID);

            if (transactionAnticipation == null)
                return StatusCode((int)HttpStatusCode.NoContent);

            var dto = _mapper.Map<TransactionAnticipation, TransactionAnticipationDTO>(transactionAnticipation);
            return Ok(dto);
        }

        #endregion

        #region GetTransactions

        [HttpGet(Name = "GetTransactions")]
        [Route("")]
        public IActionResult GetTransactions()
        {
            var transactions = _transactionRepository.ObtainAll();
            if (!transactions.Any())
                return StatusCode((int)HttpStatusCode.NoContent);

            var dtos = transactions.ProjectTo<TransactionDTO>(transactions);
            return Ok(dtos);
        }

        #endregion

        #region GetAvailableTransactions

        [HttpGet]
        [Route("Available")]
        public IActionResult GetAvailableTransactions()
        {
            var transactions = _transactionRepository.ObtainTransactionsWithAnticipations();
            if (!transactions.Any())
                return StatusCode((int)HttpStatusCode.NoContent);

            var dtos = transactions.ProjectTo<TransactionDTO>(transactions);
            return Ok(dtos);
        }

        #endregion

        #region GetTransaction

        [HttpGet("GetTransaction")]
        [Route("{transactionID}")]
        public IActionResult GetTransaction(int transactionID)
        {
            var transaction = _transactionRepository.ObtainById(transactionID);
            if (transaction == null)
                return StatusCode((int)HttpStatusCode.NoContent);

            var dto = _mapper.Map<Transaction, TransactionDTO>(transaction);
            return Ok(dto);
        }

        #endregion

        #region InsertTransaction

        [HttpPost("")]
        [Route("")]
        public IActionResult InsertTransaction(TransactionDTO dto)
        {

            var transacao = _mapper.Map<TransactionDTO, Transaction>(dto);

            transacao.PassThroughValue = transacao.TransactionValue - (decimal)0.90;
            transacao.PassThroughDate = DateTime.Now;

            _transactionRepository.Insert(transacao);
            int result = _transactionRepository.SaveChanges();

            if (result == 0)
                return StatusCode((int)HttpStatusCode.NotModified);

            return CreatedAtAction("ObtainTransaction", new { id = transacao.TransactionID }, transacao);
        }

        #endregion
    }
}