using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using LonghornBank.Models;

namespace LonghornBank.Models
{
    public class IRA
    {
        public Int32 IRAID { get; set; }

        [Display(Name = "Account Balance")]
        [Required(ErrorMessage = "An Account Balance is Required")]
        public Decimal Balance { get; set; }

        [Display(Name = "Account Balance")]
        [Required(ErrorMessage = "An Account Balance is Required")]
        public String Name { get; set; }

        [Display(Name = "Deposits made this year toward contribution limit")]
        [Required(ErrorMessage = "A Running Total of Deposits is Required")]
        public Decimal RunningTotal { get; set; }

        [Required(ErrorMessage = "A Withdrawl Limit is Required")]
        [Display(Name = "Withdrawl Limit")]
        public  Decimal MaxWithdrawl { get; set; }

        [Required(ErrorMessage = "A Pending Balance is Required")]
        [Display(Name = "Pending Balance")]
        public Decimal PendingBalance { get; set; }
    }
}