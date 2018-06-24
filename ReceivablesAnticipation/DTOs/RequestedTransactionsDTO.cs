using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReceivablesAnticipation.DTOs
{
    public class RequestedTransactionsDTO
    {
        public int ShopKeeperID { get; set; }
        public List<int> TransactionIDs { get; set; }
    }
}
