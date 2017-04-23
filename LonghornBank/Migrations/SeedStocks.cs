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
    public class SeedStocks
    {
        public static void Stocks(AppDbContext db)
        {

            db.StockMarket.AddOrUpdate(s => s.Ticker,
                new StockMarket {CompanyName = "", Fee = 0, StockPrice = 0, StockType = StockType.Index_Fund, Ticker = ""},
                new StockMarket { CompanyName = "", Fee = 0, StockPrice = 0, StockType = StockType.ETF, Ticker = "" }
                );

           
        }
    }
}