using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using LonghornBank.Models;

namespace LonghornBank.Dal
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("MyDBConnection") { }

        public DbSet<Checking> CheckingAccount { get; set; }


    }
}