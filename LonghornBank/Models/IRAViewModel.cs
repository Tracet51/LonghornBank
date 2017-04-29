using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LonghornBank.Models
{
    public class IRAViewModel
    {
        public AppUser CustomerProfile { get; set;}

        public Checking CheckingAccounts { get; set; }

        public Saving SavingAccounts { get; set; }

        public IRA IRAAccounts { get; set; }

        public StockAccount StockAccounts { get; set; }

        public BankingTransaction PayeeTransaction { get; set; }
    }
}