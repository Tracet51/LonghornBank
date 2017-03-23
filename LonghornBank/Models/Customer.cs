using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LonghornBank.Models
{
    public class Customer:Person
    {
        public Boolean ActiveStatus { get; set; }
    }
}