using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;

namespace LonghornBank.Models
{
    public class Saving
    {
        public Int32 SavingID { get; set; }

        [Display(Name = "Account Balance")]
        [Required(ErrorMessage = "Account Balance is Required")]
        public Decimal Balance { get; set; }

        [Display(Name = "Account Name")]
        [Required(ErrorMessage = "Account Name is Required")]
        public String Name { get; set; }

        [Display(Name = "Pending Balance")]
        [Required(ErrorMessage = "Pending Balance is Required")]

        public Decimal PendingBalance { get; set; }
    }
}