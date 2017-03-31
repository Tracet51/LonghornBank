using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LonghornBank.Models
{
    public enum PayeeType { CreditCard, Utility, Rent, Mortgage, Other}
    public class Payee
    {
        public Int32 PayeeID { get; set; }

        [Required(ErrorMessage = "A Name is Required")]
        [Display(Name = "Name")]
        public String Name { get; set; }

        [Required(ErrorMessage = "A Street Address is Required")]
        [Display(Name = "Street Address")]
        public String StreetAddress { get; set; }

        [Required(ErrorMessage = "City is Required")]
        [Display(Name = "City")]
        public String City { get; set; }

        [Required(ErrorMessage = "State is Required")]
        [Display(Name = "State")]
        public String State { get; set; }

        [Display(Name = "Zip Code")]
        [Required(ErrorMessage = "Zip Code is Required")]
        public String Zip { get; set; }


        [Required(ErrorMessage = "Phone Number is Required")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public String PhoneNumber { get; set; }

        [Display(Name = "Payee Type")]
        [Required(ErrorMessage = "A Payee Type is Required")]
        public PayeeType PayeeType { get; set; }
    }
}