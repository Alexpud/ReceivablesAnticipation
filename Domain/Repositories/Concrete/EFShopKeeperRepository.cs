using Domain.Entities;
using Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories.Concrete
{
    public class EFShopKeeperRepository : BaseRepository<ShopKeeper>, IShopKeeperRepository
    {
        private readonly AppDbContext _context;
        public EFShopKeeperRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
