using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LonghornBank.Models
{
    public enum DisputeStatus { Submitted, Accepted, Rejected, Adjusted }
    public class Transaction
    {
        public Int32 TransactionID { get; set; }

        public DisputeStatus TransactionDispute { get; set; }

        public DateTime TransactionDate { get; set; }

        public Decimal Amount { get; set; }

        public String Description { get; set; }

        public String DisputeMessage {get; set;}

        public Decimal CorrectedAmount { get; set; }

    }

}