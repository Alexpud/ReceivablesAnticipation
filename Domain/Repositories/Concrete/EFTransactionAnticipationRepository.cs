using Domain.Entities;
using Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories.Concrete
{
    public class EFTransactionAnticipationRepository : BaseRepository<TransactionAnticipation>, ITransactionAnticipationRepository
    {
        public readonly AppDbContext _context;
        public EFTransactionAnticipationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
