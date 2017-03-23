using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LonghornBank.Models
{
    public class Person
    {

        // ADD DATA ANNOTATIONS!!

        public Int32 CustomerID { get; set; }

        public String FName { get; set; }

        public String LName { get; set; }

        public Char? MiddleInitial { get;  set;}

        public String StreetAddress { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        public String Zip { get; set; }

        public String EmailAddress { get; set; }

        public String Password { get; set; }

        public String PhoneNumber { get; set; }

        public DateTime DOB { get; set; }

        public Boolean Active { get; set; }


    }
}