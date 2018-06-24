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

        public IQueryable<Transaction> ObtainTransactionsWithAnticipations()
        {
            return (from transaction in _context.Transactions.Include("TransactionAnticipation")
                    where transaction.TransactionAnticipation != null
                    select transaction);
        }
    }
}
