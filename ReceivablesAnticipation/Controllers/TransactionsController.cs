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
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public TransactionsController(ITransactionRepository repository, IMapper mapper)
        {
            _transactionRepository = repository;
            _mapper = mapper;
        }

        #region ObtainTransactions

        [HttpGet(Name = "ObtainTransactions")]
        [Route("")]
        public IActionResult ObtainTransactions()
        {
            var transactions = _transactionRepository.ObtainAll();
            if (!transactions.Any())
                return StatusCode((int)HttpStatusCode.NoContent);

            var dtos = transactions.ProjectTo<TransactionDTO>(transactions);
            return Ok(dtos);
        }

        #endregion

        #region ObtainTransaction

        [HttpGet("ObtainTransaction")]
        [Route("{transactionID}")]
        public IActionResult ObtainTransaction(int transactionID)
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
            _transactionRepository.Insert(transacao);
            int result = _transactionRepository.SaveChanges();

            if (result == 0)
                return StatusCode((int)HttpStatusCode.NotModified);

            return CreatedAtAction("ObtainTransaction", new { id = transacao.TransactionID }, transacao);
        }

        #endregion
    }
}