using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Repositories.Abstract
{
    public interface ITransactionAnticipationRepository : IBaseRepository<TransactionAnticipation>
    {
        IQueryable<TransactionAnticipation> OnGoingTransactionAnticipationForShopKeeper(int shopKeeperID);
    }
}
