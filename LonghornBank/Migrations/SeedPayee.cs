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

            db.Payees.AddOrUpdate(
                p => p.Name,
                new Payee { Name = "City of Austin Water", PayeeType = PayeeType.Utility, StreetAddress = "113 South Congress Ave.", City = "Austin", State = "TX", Zip = "78710", PhoneNumber = "5126645558" },

                new Payee { Name = "Reliant Energy", PayeeType = PayeeType.Utility, StreetAddress = "3500 E. Interstate 10", City = "Houston", State = "TX", Zip = "77099", PhoneNumber = "7135546697" },

                new Payee { Name = "Lee Properties", PayeeType = PayeeType.Rent, StreetAddress = "2500 Salado", City = "Austin", State = "TX", Zip = "78705", PhoneNumber = "5124453312" },

                new Payee { Name = "Capital One", PayeeType = PayeeType.CreditCard, StreetAddress = "1299 Fargo Blvd.", City = "Cheyenne", State = "WY", Zip = "82001", PhoneNumber = "5302215542" },

                new Payee { Name = "Vanguard Title", PayeeType = PayeeType.Mortgage, StreetAddress = "10976 Interstate 35 South", City = "Austin", State = "TX", Zip = "78745", PhoneNumber = "5128654951" },

                new Payee { Name = "Lawn Care of Texas", PayeeType = PayeeType.Other, StreetAddress = "4473 W. 3rd Street", City = "Austin", State = "TX", Zip = "78712", PhoneNumber = "5123365247" }


            );

            db.SaveChanges();
        }
    }
}