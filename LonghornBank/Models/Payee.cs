using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LonghornBank.Models
{
    public enum PayeeType { CreditCard, Utility, Rent, Mortgage, Other}
    public class Payee:Person
    {
        public PayeeType PayeeType { get; set; }
    }
}