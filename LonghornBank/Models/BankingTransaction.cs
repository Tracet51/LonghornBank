using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;

namespace LonghornBank.Models
{   
    public enum BankingTranactionType { Deposit, Withdrawl, Transfer}
    public class BankingTransaction: Transaction
    {
        public BankingTranactionType BankingTransactionType { get; set; }
    }
}