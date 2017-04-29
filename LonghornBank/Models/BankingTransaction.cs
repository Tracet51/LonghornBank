using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;
using System.ComponentModel.DataAnnotations;

namespace LonghornBank.Models
{
    public enum DisputeStatus { Submitted, Accepted, Rejected, Adjusted }
    public enum BankingTranactionType { Deposit, Withdrawl, Transfer, Fee, Bonus, None}

    public enum SortingOption { TransIDAsc, TransIDDec, TransTypeAsc, TransTypeDec, TransDescriptionAsc, TransDescriptionDec, TransAmountAsc, TransAmountDec, TransDateAsc, TransDateDec}

    public class BankingTransaction
    {
        public Int32 BankingTransactionID { get; set; }

        [Required(ErrorMessage = "Transaction Dispute Status is Required")]
        [Display(Name = "Transaction Dispute Status")]
        public DisputeStatus TransactionDispute { get; set; }

        [Required(ErrorMessage = "Transaction Date is Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Transaction Amount is Required")]
        [Display(Name = "Transaction Amount")]
        public Decimal Amount { get; set; }

        [Required(ErrorMessage = "Transaction Description is Required")]
        [Display(Name = "Transaction Description")]
        public String Description { get; set; }

        [Display(Name = "Dispute Message")]
        public String DisputeMessage { get; set; }

        //[Required(ErrorMessage = "Disputed Amount is Required")]
        [Display(Name = "Disputed Amount")]
        public Decimal CustomerOpinion { get; set; }


        //[Required(ErrorMessage = "Corrected Transaction Amount is Required")]
        [Display(Name = "Corrected Transaction Amount")]
        public Decimal CorrectedAmount { get; set; }

        [Display(Name = "Transaction Type")]
        [Required(ErrorMessage = "Transaction Type is Required")]
        public BankingTranactionType BankingTransactionType { get; set; }

        // Many to Many: A Transaction and Belong to Multiple Checking Accounts
        public virtual List<Checking> CheckingAccount { get; set; }

        // Many to Many: A Transaction can belong to multiple Savings Accounts
        public virtual List<Saving> SavingsAccount { get; set; }

        // 1 to Many: A transaction can belong to 1 stock account
        public virtual StockAccount StockAccount { get; set; }

        // Many to Many: a transaction can belong to multiple IRA accounts
        public virtual List<IRA> IRAAccount { get; set; }

        // A Transaction can belong to 1 trade
        public virtual Trade Trade { get; set; }
    }
}