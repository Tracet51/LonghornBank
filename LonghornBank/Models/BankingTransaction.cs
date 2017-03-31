using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;
using System.ComponentModel.DataAnnotations;

namespace LonghornBank.Models
{
    public enum DisputeStatus { Submitted, Accepted, Rejected, Adjusted }
    public enum BankingTranactionType { Deposit, Withdrawl, Transfer}
    public class BankingTransaction
    {
        public Int32 BankingTransactionID { get; set; }

        [Required(ErrorMessage = "Transaction Dispute Status is Required")]
        [Display(Name = "Transaction Dispute Status")]
        public DisputeStatus TransactionDispute { get; set; }

        [Required(ErrorMessage = "Transaction Date is Required")]
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Transaction Amount is Required")]
        [Display(Name = "Transaction Amount")]
        public Decimal Amount { get; set; }

        [Required(ErrorMessage = "Transaction Description is Required")]
        [Display(Name = "Transaction Description")]
        public String Description { get; set; }

        [Required(ErrorMessage = "Dispute Message is Required")]
        [Display(Name = "Dispute Message")]
        public String DisputeMessage { get; set; }

        [Required(ErrorMessage = "Disputed Amount is Required")]
        [Display(Name = "Disputed Amount")]
        public Decimal CustomerOpinion { get; set; }


        [Required(ErrorMessage = "Corrected Transaction Amount is Required")]
        [Display(Name = "Corrected Transaction Amount")]
        public Decimal CorrectedAmount { get; set; }

        [Display(Name = "Transaction Type")]
        [Required(ErrorMessage = "Transaction Type is Required")]
        public BankingTranactionType BankingTransactionType { get; set; }

        // jayden wuz hur!
    }
}