using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class ShopKeeper
    {
        [Key]
        public int ShopKeeperID { get; set; }
        public string Name { get; set; }
        public virtual List<TransactionAnticipation> TransactionAnticipations{ get; set; }
    }
}
