using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LonghornBank.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class AppUser : IdentityUser
    {

        //TODO: Put any additional fields that you need for your user here
        //For instance
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "A First Name is Required")]
        public String FName { get; set; }

        [Required(ErrorMessage = "A Last Name is Required")]
        [Display(Name = "Last Name")]
        public String LName { get; set; }

        [Display(Name = "Middle Initial")]
        public Char? MiddleInitial { get; set; }

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

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "DOB is Required")]
        [Display(Name = "Birthdate")]
        public DateTime DOB { get; set; }


        [Display(Name = "Active Status")]
        [Required(ErrorMessage = "An Active Status is Required")]
        public Boolean ActiveStatus { get; set; }

        // Navigational Properties 

        public virtual List<Checking> CheckingAccounts { get; set; }

        public virtual List<Saving> SavingAccounts { get; set; }

        public virtual List<IRA> IRAAccounts { get; set; }


        //This method allows you to create a new user
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    //TODO: Here's your db context for the project.  All of your db sets should go in here
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        //TODO:  Add dbsets here, for instance there's one for books
        //Remember, Identity adds a db set for users, so you shouldn't add that one - you will get an error

        // Create Checking Account to Access
        public DbSet<Checking> CheckingAccount { get; set; }

        // create Savings Account to Access
        public DbSet<Saving> SavingsAccount { get; set; }

        // Create Transaction access
        public DbSet<BankingTransaction> BankingTransaction { get; set; }

        // Create IRA Access
        public DbSet<IRA> IRAAccount { get; set; }

        //TODO: Make sure that your connection string name is correct here.
        public AppDbContext()
            : base("MyDBConnection", throwIfV1Schema: false)
        {
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        public DbSet<AppRole> AppRoles { get; set; }

        public System.Data.Entity.DbSet<LonghornBank.Models.Manager> Managers { get; set; }
    }
}