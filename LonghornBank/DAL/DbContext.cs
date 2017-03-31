using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using LonghornBank.Models;

namespace LonghornBank.Dal
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("MyDBConnection") { }

        // Create Checking Account to Access
        public DbSet<Checking> CheckingAccount { get; set; }

        // create Savings Account to Access
        public DbSet<Saving> SavingsAccount { get; set; }

        // Create Cusomter Account to access
        public DbSet<Customer> CustomerAccount { get; set; }

        // Create Transaction access
        public DbSet<BankingTransaction> BankingTransaction { get; set; }


    }
}