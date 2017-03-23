using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;

namespace LonghornBank.Models
{
    public class Saving: Account
    {
        public Decimal Balance { get; set; }

        public String Name { get; set; }

        public Decimal PendingBalance { get; set; }
    }
}