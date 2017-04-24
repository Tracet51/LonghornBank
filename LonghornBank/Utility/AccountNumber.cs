using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace LonghornBank.Utility
{
    public static class AccountNumber
    {

        public static String AutoNumber(AppDbContext db)
        {

            // Create a list to hold all of the account number 
            List<Decimal> AccountNumList = new List<Decimal>();

            // Find the account with the largest account number
            var SPQ = from sp in db.StockAccount
                      where sp.AccountNumber != null
                      select sp;

            SPQ = SPQ.OrderByDescending(s => s.StockAccountID);

            StockAccount SP = SPQ.FirstOrDefault();

            if (SP != null)
            {
                Decimal SPAN = Convert.ToDecimal(SP.AccountNumber);
                AccountNumList.Add(SPAN);
            }


            var CAQ = from ca in db.CheckingAccount
                      select ca;

            CAQ = CAQ.OrderByDescending(c => c.CheckingID);

            Checking CA = CAQ.FirstOrDefault();

            if (CA != null)
            {
                Decimal CAN = Convert.ToDecimal(CA.AccountNumber);
                AccountNumList.Add(CAN);
            }

            var SAQ = from sa in db.SavingsAccount
                      select sa;

            SAQ = SAQ.OrderByDescending(s => s.SavingID);

            Saving SA = SAQ.FirstOrDefault();

            if (SA != null)
            {
                Decimal SAN = Convert.ToDecimal(SA.AccountNumber);
                AccountNumList.Add(SAN);
            }

            var IQ = from ira in db.IRAAccount
                     select ira;

            IQ = IQ.OrderByDescending(s => s.IRAID);

            IRA I = IQ.FirstOrDefault();

            if (I != null)
            {
                Decimal IN = Convert.ToDecimal(I.AccountNumber);
                AccountNumList.Add(IN);
            }

            // Variable to hold the max
            Decimal MaxAccNum = 0;

            // Loop through each Account Number and find the biggest one
            foreach (Decimal AccNum in AccountNumList)
            {
                if (AccNum > MaxAccNum)
                {
                    MaxAccNum = AccNum;
                }
            }

            MaxAccNum += 1;
            return MaxAccNum.ToString();
        }
    }
}