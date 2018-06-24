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
        /// <summary>
        /// Request the anticipation of the transactions with the IDs passed
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>
        /// 200 - Anticipation requested
        /// 400 - No anticipable transaction was found
        /// </returns>
        [HttpPost]
        [Route("Anticipation")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult RequestAnticipation(RequestedTransactionsDTO dto)
        {
            List<Transaction> transactions = new List<Transaction>();
            decimal transactionsAnticipationValue = 0;
            decimal totalTransactionValue = 0;

            bool ongoingAnticipations = _transactionAnticipationRepository
                .OnGoingTransactionAnticipationForShopKeeper(dto.ShopKeeperID).Any();

            if (ongoingAnticipations)
                return BadRequest("There are on going transaction anticipations for shop keeper");

            foreach (var transactionID in dto.TransactionIDs)
            {
                var transaction = _transactionRepository.ObtainAnticipatableTransactions()
                    .FirstOrDefault(x => x.TransactionID == transactionID);

                if (transaction != null && transaction.AcquirerApproval)
                {
                    decimal instalmentValue = transaction.TransactionValue / transaction.InstalmentQuantity;
                    totalTransactionValue += transaction.TransactionValue;
                    transactionsAnticipationValue += ((instalmentValue * (decimal)3.8) / 100) * transaction.InstalmentQuantity;
                    transactions.Add(transaction);
                }
            }

            if (!transactions.Any())
                return BadRequest("No anticipable transactions");

            TransactionAnticipation transactionAnticipation = new TransactionAnticipation()
            {
                AnticipationResult = null,
                Status = (int)Auxiliary.TransactionStatuses.WaitingForAnalysis, // Waiting for analysis
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
                new { transactionAnticipationID = transactionAnticipation.TransactionAnticipationID }, transactionAnticipation);
        }

        #endregion

        #region GetTransactionAnticipationRequest
        /// <summary>
        /// Obtain the transaction anticipation request with the ID passed
        /// </summary>
        /// <param name="transactionAnticipationID"></param>
        /// <returns>
        /// 200 - Transaction anticipation found
        /// 204 - Transaction anticipation not found
        /// </returns>
        [HttpGet(Name = "GetTransactionAnticipationRequest")]
        [Route("Anticipations/{transactionAnticipationID}")]
        [ProducesResponseType(200, Type = typeof(IQueryable<TransactionAnticipationDTO>))]
        public IActionResult GetTransactionAnticipationRequest(int transactionAnticipationID)
        {
            TransactionAnticipation transactionAnticipation = _transactionAnticipationRepository
                .ObtainById(transactionAnticipationID);

            if (transactionAnticipation == null)
                return StatusCode((int)HttpStatusCode.NoContent);

            var dto = _mapper.Map<TransactionAnticipation, TransactionAnticipationDTO>(transactionAnticipation);
            return Ok(dto);
        }

        #endregion

        #region GetAnticipationRequestsByPeriod
        /// <summary>
        /// Returns anticipations requests made in a period
        /// </summary>
        /// <param name="initialDate"></param>
        /// <param name="finalDate"></param>
        /// <returns>
        /// 200 - List of anticipation requests
        /// 204 - No anticipation request found
        /// </returns>
        [HttpGet]
        [Route("Anticipations")]
        [ProducesResponseType(200, Type = typeof(IQueryable<TransactionAnticipationDTO>))]
        public IActionResult GetAnticipationRequestsByPeriod(DateTime initialDate, DateTime finalDate)
        {
            var transactionsAnticipations = _transactionAnticipationRepository.ObtainAll()
                .Where(x => x.SolicitationDate > initialDate && x.SolicitationDate < finalDate);

            if (transactionsAnticipations.Any())
                return StatusCode((int)HttpStatusCode.NoContent);

            var dtos = transactionsAnticipations.ProjectTo<TransactionAnticipationDTO>(transactionsAnticipations);
            return Ok(dtos);
        }
        #endregion

        #region GetTransactions
        /// <summary>
        /// Obtain all transactions
        /// </summary>
        /// <returns>
        /// 200 - List of transactions
        /// 204 - No transaction was found
        /// </returns>
        [HttpGet(Name = "GetTransactions")]
        [ProducesResponseType(200, Type = typeof(IQueryable<TransactionDTO>))]
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

        #region GetAnticipatableTransactions
        /// <summary>
        /// Returns a list of anticipatable transactions to request anticipation
        /// </summary>
        /// <returns>
        /// 200 - List of available transactions
        /// 204 - No available transaction was found
        /// </returns>
        [HttpGet]
        [Route("Anticipatable")]
        [ProducesResponseType(200, Type = typeof(IQueryable<TransactionDTO>))]
        public IActionResult GetAnticipatableTransactions()
        {
            var transactions = _transactionRepository.ObtainAnticipatableTransactions();

            if (!transactions.Any())
                return StatusCode((int)HttpStatusCode.NoContent);

            var dtos = transactions.ProjectTo<TransactionDTO>(transactions);
            return Ok(dtos);
        }

        #endregion

        #region GetTransaction
        /// <summary>
        /// Returns the transaction with the following ID.
        /// </summary>
        /// <param name="transactionID"></param>
        /// <returns>
        /// 200 - Returns the transaction
        /// 204 - Transaction was not found
        /// </returns>
        [HttpGet(Name = "GetTransaction")]
        [Route("{transactionID}")]
        [ProducesResponseType(200, Type = typeof(IQueryable<TransactionDTO>))]
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
        /// <summary>
        /// Inserts the transaction
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>
        /// 200 - Transaction was created
        /// 304 - Transaction was not created
        /// </returns>
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