using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReceivablesAnticipation.DTOs
{
    public class TransactionDTO
    {
        public int TransactionID { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime PassThroughDate { get; set; }
        public bool FlConfirmation { get; set; }
        public decimal TransactionValue { get; set; }
        public decimal PassThroughValue { get; set; }
        public int InstalmentQuantity { get; set; }
    }
}
