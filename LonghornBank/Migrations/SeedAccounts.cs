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
    public class SeedAccounts
    {
        public static void Account(AppDbContext db)
        {
            var ca2query = from c in db.Users where c.Email == "Dixon@aool.com" select c;

            AppUser Customerca2 = ca2query.FirstOrDefault();

            StockAccount ca2 = new StockAccount { AccountNumber = "1000000000", Customer = Customerca2, Name = "Shan's Stock", CashBalance = 0m };

            db.StockAccount.AddOrUpdate(x => x.AccountNumber, ca2);


            var ca3query = from c in db.Users where c.Email == "willsheff@email.com" select c;

            AppUser Customerca3 = ca3query.FirstOrDefault();

            Saving ca3 = new Saving { AccountNumber = "1000000001", Customer = Customerca3, Name = "William's Savings", Balance = 40035.5m };

            db.SavingsAccount.AddOrUpdate(x => x.AccountNumber, ca3);


            var ca4query = from c in db.Users where c.Email == "smartinmartin.Martin@aool.com" select c;

            AppUser Customerca4 = ca4query.FirstOrDefault();

            Checking ca4 = new Checking { AccountNumber = "1000000002", Customer = Customerca4, Name = "Gregory's Checking", Balance = 39779.49m };

            db.CheckingAccount.AddOrUpdate(x => x.AccountNumber, ca4);


            var ca5query = from c in db.Users where c.Email == "avelasco@yaho.com" select c;

            AppUser Customerca5 = ca5query.FirstOrDefault();

            Checking ca5 = new Checking { AccountNumber = "1000000003", Customer = Customerca5, Name = "Allen's Checking", Balance = 47277.33m };

            db.CheckingAccount.AddOrUpdate(x => x.AccountNumber, ca5);


            var ca6query = from c in db.Users where c.Email == "rwood@voyager.net" select c;

            AppUser Customerca6 = ca6query.FirstOrDefault();

            Checking ca6 = new Checking { AccountNumber = "1000000004", Customer = Customerca6, Name = "Reagan's Checking", Balance = 70812.15m };

            db.CheckingAccount.AddOrUpdate(x => x.AccountNumber, ca6);


            var ca7query = from c in db.Users where c.Email == "nelson.Kelly@aool.com" select c;

            AppUser Customerca7 = ca7query.FirstOrDefault();

            Saving ca7 = new Saving { AccountNumber = "1000000005", Customer = Customerca7, Name = "Kelly's Savings", Balance = 21901.97m };

            db.SavingsAccount.AddOrUpdate(x => x.AccountNumber, ca7);


            var ca8query = from c in db.Users where c.Email == "erynrice@aool.com" select c;

            AppUser Customerca8 = ca8query.FirstOrDefault();

            Checking ca8 = new Checking { AccountNumber = "1000000006", Customer = Customerca8, Name = "Eryn's Checking", Balance = 70480.99m };

            db.CheckingAccount.AddOrUpdate(x => x.AccountNumber, ca8);


            var ca9query = from c in db.Users where c.Email == "westj@pioneer.net" select c;

            AppUser Customerca9 = ca9query.FirstOrDefault();

            Saving ca9 = new Saving { AccountNumber = "1000000007", Customer = Customerca9, Name = "Jake's Savings", Balance = 7916.4m };

            db.SavingsAccount.AddOrUpdate(x => x.AccountNumber, ca9);


            var ca10query = from c in db.Users where c.Email == "mb@aool.com" select c;

            AppUser Customerca10 = ca10query.FirstOrDefault();

            StockAccount ca10 = new StockAccount { AccountNumber = "1000000008", Customer = Customerca10, Name = "Michelle's Stock", CashBalance = 0m };

            db.StockAccount.AddOrUpdate(x => x.AccountNumber, ca10);


            var ca11query = from c in db.Users where c.Email == "jeff@ggmail.com" select c;

            AppUser Customerca11 = ca11query.FirstOrDefault();

            Saving ca11 = new Saving { AccountNumber = "1000000009", Customer = Customerca11, Name = "Jeffrey's Savings", Balance = 69576.83m };

            db.SavingsAccount.AddOrUpdate(x => x.AccountNumber, ca11);


            var ca12query = from c in db.Users where c.Email == "nelson.Kelly@aool.com" select c;

            AppUser Customerca12 = ca12query.FirstOrDefault();

            StockAccount ca12 = new StockAccount { AccountNumber = "1000000010", Customer = Customerca12, Name = "Kelly's Stock", CashBalance = 0m };

            db.StockAccount.AddOrUpdate(x => x.AccountNumber, ca12);


            var ca13query = from c in db.Users where c.Email == "erynrice@aool.com" select c;

            AppUser Customerca13 = ca13query.FirstOrDefault();

            Checking ca13 = new Checking { AccountNumber = "1000000011", Customer = Customerca13, Name = "Eryn's Checking 2", Balance = 30279.33m };

            db.CheckingAccount.AddOrUpdate(x => x.AccountNumber, ca13);


            var ca14query = from c in db.Users where c.Email == "mackcloud@pimpdaddy.com" select c;

            AppUser Customerca14 = ca14query.FirstOrDefault();

            IRA ca14 = new IRA { AccountNumber = "1000000012", Customer = Customerca14, Name = "Jennifer's IRA", Balance = 5000m };

            db.IRAAccount.AddOrUpdate(x => x.AccountNumber, ca14);


            var ca15query = from c in db.Users where c.Email == "ss34@ggmail.com" select c;

            AppUser Customerca15 = ca15query.FirstOrDefault();

            Saving ca15 = new Saving { AccountNumber = "1000000013", Customer = Customerca15, Name = "Sarah's Savings", Balance = 11958.08m };

            db.SavingsAccount.AddOrUpdate(x => x.AccountNumber, ca15);


            var ca16query = from c in db.Users where c.Email == "tanner@ggmail.com" select c;

            AppUser Customerca16 = ca16query.FirstOrDefault();

            Saving ca16 = new Saving { AccountNumber = "1000000014", Customer = Customerca16, Name = "Jeremy's Savings", Balance = 72990.47m };

            db.SavingsAccount.AddOrUpdate(x => x.AccountNumber, ca16);


            var ca17query = from c in db.Users where c.Email == "liz@ggmail.com" select c;

            AppUser Customerca17 = ca17query.FirstOrDefault();

            Saving ca17 = new Saving { AccountNumber = "1000000015", Customer = Customerca17, Name = "Elizabeth's Savings", Balance = 7417.2m };

            db.SavingsAccount.AddOrUpdate(x => x.AccountNumber, ca17);


            var ca18query = from c in db.Users where c.Email == "ra@aoo.com" select c;

            AppUser Customerca18 = ca18query.FirstOrDefault();

            IRA ca18 = new IRA { AccountNumber = "1000000016", Customer = Customerca18, Name = "Allen's IRA", Balance = 5000m };

            db.IRAAccount.AddOrUpdate(x => x.AccountNumber, ca18);


            var ca19query = from c in db.Users where c.Email == "johnsmith187@aool.com" select c;

            AppUser Customerca19 = ca19query.FirstOrDefault();

            StockAccount ca19 = new StockAccount { AccountNumber = "1000000017", Customer = Customerca19, Name = "John's Stock", CashBalance = 0m };

            db.StockAccount.AddOrUpdate(x => x.AccountNumber, ca19);


            var ca20query = from c in db.Users where c.Email == "mclarence@aool.com" select c;

            AppUser Customerca20 = ca20query.FirstOrDefault();

            Saving ca20 = new Saving { AccountNumber = "1000000018", Customer = Customerca20, Name = "Clarence's Savings", Balance = 1642.82m };

            db.SavingsAccount.AddOrUpdate(x => x.AccountNumber, ca20);


            var ca21query = from c in db.Users where c.Email == "ss34@ggmail.com" select c;

            AppUser Customerca21 = ca21query.FirstOrDefault();

            Checking ca21 = new Checking { AccountNumber = "1000000019", Customer = Customerca21, Name = "Sarah's Checking", Balance = 84421.45m };

            db.CheckingAccount.AddOrUpdate(x => x.AccountNumber, ca21);


            db.SaveChanges();
        }


    }
}