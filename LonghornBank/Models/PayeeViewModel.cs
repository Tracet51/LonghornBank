using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LonghornBank.Models
{
    public class PayeeViewModel
    {
        // Get the User
        public AppUser UserCustomerProfile { get; set; }

        // The Checking Account Assoicated
        public SelectList CheckingAccounts { get; set; }

        // The Savings Accounts Assoicated
        public SelectList SavingsAccounts { get; set; }

        // The PayeeAccount Associated
        public List<Payee> PayeeAccount { get; set; }

        public int CheckingID { get; set; }

        public int SavingID { get; set; }

        //The transaction Details
        public BankingTransaction PayeeTransaction { get; set; }

        public String Description { get; set; }
    }

    public class PayPayee
    {
        // Get the User
        public AppUser UserCustomerProfile { get; set; }

        // The Checking Account Assoicated
        public Checking CheckingAccounts { get; set; }

        // The Savings Accounts Assoicated
        public Saving SavingsAccount { get; set; }

        // The PayeeAccount Associated
        public Payee PayeeAccount { get; set; }

        //The transaction Details
        public BankingTransaction PayeeTransaction { get; set; }
    }
}