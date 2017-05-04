using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace LonghornBank.Models
{
    public class TransactionViewModel
    {
    }

    public enum DisputeType {Delete, Adjust}

    public class DisputeTransaction
    {
        public Int32 BankingTransactionID { get; set; }

        [Required(ErrorMessage = "Transaction Dispute Status is Required")]
        [Display(Name = "Transaction Dispute Status")]
        public DisputeType TransactionDispute { get; set; }
        

        [Display(Name = "Dispute Message")]
        public String DisputeMessage { get; set; }
        

        //[Required(ErrorMessage = "Disputed Amount is Required")]
        [Display(Name = "Disputed Amount")]
        public Decimal CustomerOpinion { get; set; }

    }
}