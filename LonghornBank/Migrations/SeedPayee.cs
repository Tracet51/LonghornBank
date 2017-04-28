using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Migrations;

namespace LonghornBank.Migrations
{
    public class SeedPayee
    {
        public static void Payee(AppDbContext db)
        {
            db.BankingTransaction.AddOrUpdate(
                bt => bt.Amount,
                new BankingTransaction { BankingTransactionType = BankingTranactionType.Bonus });
        }
    }
}