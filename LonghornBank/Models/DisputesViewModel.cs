using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LonghornBank.Models
{
    public class DisputesViewModel
    {
        public virtual BankingTransaction DisputedTransaction { get; set; }
        public virtual AppUser Customer { get; set; }
    }
}