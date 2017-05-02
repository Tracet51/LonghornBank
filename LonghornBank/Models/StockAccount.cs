using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace LonghornBank.Models
{
    public class StockAccount
    {
        private AppDbContext db = new AppDbContext();

        public Int32 StockAccountID { get; set; }
        [Display(Name= "Approved or Needs Approval")]
        public ApprovedorNeedsApproval ApprovalStatus { get; set; }


        [Display(Name = "Cash Balance")]
        [Required(ErrorMessage = "A Cash Balance is Required")]
        [Column("CashBalance")]
        public Decimal CashBalance { get; set; }

        [Display(Name = "Account Name")]
        [Column("Name")]
        public String Name { get; set; }

        [Display(Name = "Account Number")]
        [Column("AccountNumber")]
        public String AccountNumber { get; set; }

        [Display(Name ="Pending Balance")]
        public Decimal PendingBalance { get; set; }


        private Decimal _decStockBalance;


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

        [Display(Name = "Trading Fee")]
        [Column("TradingFee")]
        public Decimal TradingFee { get; set; }

        private Decimal decGains;

        [Display(Name = "Unrealized Gains")]
        [Column("Gains")]
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

        [Display(Name = "Balanced Portfolio?")]
        [Column("Bounses")]
        public Boolean Balanced { get; set; }

        // A stock account can belong to 1 person 
        public virtual AppUser Customer { get; set; }

        // A stock account can have many trades
        public virtual List<Trade> Trades { get; set; }

        // Stock account can have many transactions 
        public virtual List<BankingTransaction> BankingTransaction { get; set; }
    }
}