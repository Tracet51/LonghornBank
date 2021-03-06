﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;
using System.ComponentModel.DataAnnotations;

namespace LonghornBank.Models
{
    public class Checking
    {
        public Int32 CheckingID { get; set; }

        [Display(Name ="Account Number")]
        public String AccountNumber { get; set; }

        [Display(Name = "Account Balance")]
        public Decimal Balance { get; set; }

        [Display(Name = "Pending Balance")]
        // [Required(ErrorMessage = "Pending Balance is Required")]
        public Decimal PendingBalance { get; set; }

        [Display(Name = "Account Name")]
        public String Name { get; set; }

        public Boolean Overdrawn { get; set; }

        // create connection to customer account 
        public virtual AppUser Customer { get; set; }

        // Create Many to Many Relationship w/ Transaction 
        public virtual List<BankingTransaction> BankingTransactions { get; set; }
        public String AccountDisplay { get; set; }
    }
}