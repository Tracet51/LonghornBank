using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace LonghornBank.Utility
{
    public static class BalancedPortfolio
    {
        // Determines if the portfolio is balance

        public static void CheckBalanced(AppDbContext db, AppUser Customer)
        {
            // get the users stock account 
            // Get the customer Savings account 
            var SAQ = from sa in db.StockAccount
                      where sa.Customer.Id == Customer.Id
                      select sa;

            StockAccount CustomerStockAccount = SAQ.FirstOrDefault();

            // Holding variables to counts the number of each type of stock 
            Int32 ordinary = 0;
            Int32 index = 0;
            Int32 mutual = 0;

            // loop through the stocks and check the types
            foreach (Trade t in CustomerStockAccount.Trades)
            {
                if (t.StockMarket.StockType == StockType.Ordinary)
                {
                    ordinary += 1;
                }
                else if(t.StockMarket.StockType == StockType.Index_Fund)
                {
                    index += 1;
                }
                else if (t.StockMarket.StockType == StockType.Mutual_Fund)
                {
                    mutual += 1;
                }
            }

            // check to see if balanced
            if (ordinary == 2 && index == 1 && mutual == 1)
            {
                CustomerStockAccount.Balanced = true;
            }
            else
            {
                CustomerStockAccount.Balanced = false;
            }

            // update the stock account
            db.Entry(CustomerStockAccount).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
    }
}