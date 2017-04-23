using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LonghornBank.Models
{
    public class StockAccount
    {
        private AppDbContext db = new AppDbContext();

        public Int32 StockAccountID { get; set; }

        [Display(Name = "Cash Balance")]
        [Required(ErrorMessage = "A Cash Balance is Required")]
        public Decimal CashBalance { get; set; }

        [Display(Name = "Account Name")]
        [Required(ErrorMessage = "An Account Name is Required")]
        public String Name { get; set; }

        private String strAccountNumber;

        [Display(Name = "Account Number")]
        public String AccountNumber
        {
            get
            {
                return strAccountNumber;
            }

            private set
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
                strAccountNumber = MaxAccNum.ToString();
                
            }
        }

        private Decimal _decStockBalance;


        [Required(ErrorMessage = "A Stock Value is Required")]
        [Display(Name = "Stock Value")]
        public Decimal StockBalance
        {
            get
            {
                if (this.Trades != null)
                {
                    if (this.Trades.Count() != 0)
                    {
                        foreach (var t in this.Trades.Where(i => i.TradeType == TradeType.Buy))
                        {
                            _decStockBalance += (t.Quantity * t.PricePerShare);

                        }
                    }
                    
                }

                else
                {
                    _decStockBalance = 0;
                }

                return _decStockBalance;
            }
        }

        [Required(ErrorMessage = "A Trading Fee is Required")]
        [Display(Name = "Trading Fee")]
        public Decimal TradingFee { get; set; }

        private Decimal decGains;

        [Display(Name = "Unrealized Gains")]
        public Decimal Gains
        {
            get
            {
                if (this.Trades != null)
                {
                    if (this.Trades.Count() != 0)
                    {
                        foreach (var t in this.Trades)
                        {
                            Decimal TradeValue = (t.Quantity * t.PricePerShare);

                            Decimal CurrentValue = (t.Quantity * db.StockMarket.Find(t.StockMarket.StockMarketID).StockPrice);

                            decGains += (CurrentValue - TradeValue);

                        }
                    }

                }

                else
                {
                    decGains = 0;
                }

                return decGains;
            }
        }

        [Display(Name = "Balanced Portfolio Bonus Balance")]
        public Boolean Bounses { get; set; }

        // A stock account can belong to 1 person 
        public virtual AppUser Customer { get; set; }

        // A stock account can have many trades
        public virtual List<Trade> Trades { get; set; }

        // Stock account can have many transactions 
        public virtual List<BankingTransaction> BankingTransaction { get; set; }
    }
}