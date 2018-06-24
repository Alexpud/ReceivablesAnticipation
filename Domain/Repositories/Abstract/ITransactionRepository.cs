using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Repositories.Abstract
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        IQueryable<Transaction> ObtainTransactionAnticipation();
        IQueryable<Transaction> ObtainAnticipatableTransactions();
    }
}
