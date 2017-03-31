using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;
using System.ComponentModel.DataAnnotations;

namespace LonghornBank.Models
{
    public class Checking
    {
        public Int32 CheckingID { get; set; }

        [Display(Name = "Account Balance")]
        public Decimal Balance { get; set; }

        [Display(Name = "Pending Balance")]
        [Required(ErrorMessage = "Pending Balance is Required")]
        public Decimal PendingBalance { get; set; }

        [Display(Name = "Account Name")]
        [Required(ErrorMessage = "Account Name is Required")]
        public String Name { get; set; }
    }
}