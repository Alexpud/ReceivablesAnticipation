using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime PassThroughDate { get; set; }
        public bool FlConfirmation { get; set; }
        public decimal TransactionValue { get; set; }
        public decimal PassThroughValue { get; set; }
        public int InstalmentQuantity { get; set; }
        public int? TransactionAnticipationID{ get; set; }
        [ForeignKey("TransactionAnticipationID")]
        public virtual TransactionAnticipation TransactionAnticipation { get; set; }
    }
}
