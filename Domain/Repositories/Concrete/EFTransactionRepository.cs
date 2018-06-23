using Domain.Entities;
using Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
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
    }
}
