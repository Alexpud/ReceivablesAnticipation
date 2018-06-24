using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReceivablesAnticipation
{
    public static class Auxiliary
    {
        public enum TransactionStatuses
        {
           WaitingForAnalysis = 1,
           InAnalysis = 2,
           Finished = 3,
        }
    }
}
