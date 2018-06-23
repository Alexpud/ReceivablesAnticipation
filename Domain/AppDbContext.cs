using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class AppDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionAnticipation> TransactionAnticipations { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
