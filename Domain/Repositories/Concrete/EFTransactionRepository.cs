using Domain.Entities;
using Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Repositories.Concrete
{
    public class EFTransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        private readonly AppDbContext _context;
        public EFTransactionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns transactions and their anticipation
        /// </summary>
        public IQueryable<Transaction> ObtainTransactionAnticipation()
        {
            return (from transaction in _context.Transactions.Include("TransactionAnticipation")
                    select transaction);
        }

        public IQueryable<Transaction> ObtainAnticipatableTransactions()
        {
            return (from transaction in _context.Transactions.Include("TransactionAnticipation")
                    where transaction.TransactionAnticipation == null
                    select transaction);
        }
    }
}
