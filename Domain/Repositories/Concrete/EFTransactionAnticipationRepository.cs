using Domain.Entities;
using Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Repositories.Concrete
{
    public class EFTransactionAnticipationRepository : BaseRepository<TransactionAnticipation>, ITransactionAnticipationRepository
    {
        private readonly AppDbContext _context;
        public EFTransactionAnticipationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<TransactionAnticipation> OnGoingTransactionAnticipationForShopKeeper(int shopKeeperID)
        {
            return (from transactionAnti in _context.TransactionAnticipations
                    where transactionAnti.ShopKeeperID == shopKeeperID &&
                          transactionAnti.Status != 3
                    select transactionAnti);
        }
    }
}
