using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;
using System.ComponentModel.DataAnnotations;

namespace LonghornBank.Models
{
    public class IRA
    {
        [Key]
        public Int32 IRAID { get; set; }

        [Display(Name = "Account Balance")]
        [Required(ErrorMessage = "An Account Balance is Required")]
        public Decimal Balance { get; set; }

        [Display(Name = "Account Number")]
        public String AccountNumber { get; set; }

        [Display(Name = "Account Name")]
        //[Required(ErrorMessage = "An Account Name is Required")]
        public String Name { get; set; }

        [Display(Name = "Deposits made this year toward contribution limit")]
        //[Required(ErrorMessage = "A Running Total of Deposits is Required")]
        public Decimal RunningTotal { get; set; }

        //[Required(ErrorMessage = "A Withdrawl Limit is Required")]
        [Display(Name = "Withdrawl Limit")]
        public  Decimal MaxWithdrawl { get; set; }

        //[Required(ErrorMessage = "A Pending Balance is Required")]
        [Display(Name = "Pending Balance")]
        public Decimal PendingBalance { get; set; }

        [Display(Name = "Fee")]
        public Decimal Fee { get; set; }

        public Boolean Overdrawn { get; set; }


        // Navigation

        // IRA can have 1 user
        
        public virtual AppUser Customer { get; set; }

        // IRA can have many transactions 
        public virtual List<BankingTransaction> BankingTransactions { get; set; }


    }
}