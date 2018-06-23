﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class TransactionAnticipation
    {
        [Key]
        public int TransactionAnticipationID { get; set; }
        public DateTime SolicitationDate { get; set; }
        public DateTime AnalysisDate { get; set; }
        public bool AnticipationResult { get; set; }
        public decimal TotalTransactionValue { get; set; }
        public decimal TotalPassThroughValue { get; set; }
        public virtual List<Transaction> Transactions { get; set; }
    }
}