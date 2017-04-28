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
                new StockMarket { CompanyName = "Alphabet Inc.", Fee = 25, StockPrice = 0, StockType = StockType.Ordinary, Ticker = "GOOG" },

                new StockMarket { CompanyName = "Apple Inc.", Fee = 40, StockPrice = 0, StockType = StockType.Ordinary, Ticker = "AAPL" },

                new StockMarket { CompanyName = "Amazon.com Inc.", Fee = 15, StockPrice = 0, StockType = StockType.Ordinary, Ticker = "AMZN" },

                new StockMarket { CompanyName = "Southwest Airlines", Fee = 35, StockPrice = 0, StockType = StockType.Ordinary, Ticker = "LUV" },

                new StockMarket { CompanyName = "Texas Instruments", Fee = 15, StockPrice = 0, StockType = StockType.Ordinary, Ticker = "TXN" },

                new StockMarket { CompanyName = "The Hershey Company", Fee = 25, StockPrice = 0, StockType = StockType.Ordinary, Ticker = "HSY" },

                new StockMarket { CompanyName = "Visa Inc.", Fee = 10, StockPrice = 0, StockType = StockType.Ordinary, Ticker = "V" },

                new StockMarket { CompanyName = "Nike", Fee = 30, StockPrice = 0, StockType = StockType.Ordinary, Ticker = "NKE" },

                new StockMarket { CompanyName = "Vanguard Emerging Markets ETF", Fee = 20, StockPrice = 0, StockType = StockType.ETF, Ticker = "VWO" },

                new StockMarket { CompanyName = "Corn", Fee = 10, StockPrice = 0, StockType = StockType.Future_Share, Ticker = "CORN" },

                new StockMarket { CompanyName = "Old Mutual Asset Allocation Balanced Fund", Fee = 10, StockPrice = 0, StockType = StockType.Mutual_Fund, Ticker = "OMBCX" },

                new StockMarket { CompanyName = "Ford Motor Company", Fee = 10, StockPrice = 0, StockType = StockType.Ordinary, Ticker = "F" },

                new StockMarket { CompanyName = "Bank of America Corporation", Fee = 10, StockPrice = 0, StockType = StockType.Ordinary, Ticker = "BAC" },

                new StockMarket { CompanyName = "Vanguard REIT ETF", Fee = 15, StockPrice = 0, StockType = StockType.ETF, Ticker = "VNQ" },

                new StockMarket { CompanyName = "Nasdaq Index Fund", Fee = 20, StockPrice = 0, StockType = StockType.Index_Fund, Ticker = "NSDQ" },

                new StockMarket { CompanyName = "CarMax, Inc.", Fee = 15, StockPrice = 0, StockType = StockType.Ordinary, Ticker = "KMX" },

                new StockMarket { CompanyName = "Dow Jones Industrial Average Index Fund", Fee = 25, StockPrice = 0, StockType = StockType.Index_Fund, Ticker = "DIA" },

                new StockMarket { CompanyName = "S&P 500 Index Fund", Fee = 25, StockPrice = 0, StockType = StockType.Index_Fund, Ticker = "SPY" },

                new StockMarket { CompanyName = "Franklin Resources, Inc.", Fee = 25, StockPrice = 0, StockType = StockType.Ordinary, Ticker = "BEN" },

                new StockMarket { CompanyName = "Pacific Advisors Small Cap Value Fund", Fee = 15, StockPrice = 0, StockType = StockType.Mutual_Fund, Ticker = "PGSCX" }


                );

            db.SaveChanges();
           
        }
    }
}