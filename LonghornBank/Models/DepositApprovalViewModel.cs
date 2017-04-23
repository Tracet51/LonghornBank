using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LonghornBank.Models
{
    public class DepositApprovalViewModel
    {
        public BankingTransaction BankingTransaction { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
    }
}