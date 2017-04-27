using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace LonghornBank.Migrations
{
    public class SeedProfile
    {
        public static void SeedEmployees(AppDbContext db)
        {
            //create a user manager to add users to databases             
            UserManager<AppUser> userManager = new UserManager<AppUser>(new UserStore<AppUser>(db));
            
            //create a role manager             
            AppRoleManager roleManager = new AppRoleManager(new RoleStore<AppRole>(db));

            //create an admin role             
            String roleName = "Manager";

            //check to see if the role exists             
            if (roleManager.RoleExists(roleName) == false) //role doesn't exist
            {
                roleManager.Create(new AppRole(roleName));
            }

            

            // Check if Employee Role Exists
            if (roleManager.RoleExists("Employee") == false) //role doesn't exist
            {
                roleManager.Create(new AppRole("Employee"));
            }


            AppUser b2 = new AppUser() { UserName = "t.jacobs@longhornbank.neet", Email = "t.jacobs@longhornbank.neet", FName = "Todd", LName = "Jacobs", MiddleInitial = Convert.ToChar("L"), StreetAddress = "4564 Elm St.", City = "Houston", State = "TX", Zip = "77003", PhoneNumber = "8176593544", SSN = "222222222", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb2 = userManager.FindByName("t.jacobs@longhornbank.neet");
            if (userToAddb2 == null)
            {
                userManager.Create(b2, "society");
                db.SaveChanges();
                userToAddb2 = userManager.FindByName("t.jacobs@longhornbank.neet");
                if (userManager.IsInRole(userToAddb2.Id, "Employee") == false)
                {
                    userManager.AddToRole(userToAddb2.Id, "Employee");
                }
            }

            AppUser b3 = new AppUser() { UserName = "e.rice@longhornbank.neet", Email = "e.rice@longhornbank.neet", FName = "Eryn", LName = "Rice", MiddleInitial = Convert.ToChar("M"), StreetAddress = "3405 Rio Grande", City = "Dallas", State = "TX", Zip = "75261", PhoneNumber = "2148475583", SSN = "111111111", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb3 = userManager.FindByName("e.rice@longhornbank.neet");
            if (userToAddb3 == null)
            {
                userManager.Create(b3, "ricearoni");
                db.SaveChanges();
                userToAddb3 = userManager.FindByName("e.rice@longhornbank.neet");
                if (userManager.IsInRole(userToAddb3.Id, "Employee") == false)
                {
                    userManager.AddToRole(userToAddb3.Id, "Employee");
                }
            }

            AppUser b4 = new AppUser() { UserName = "b.ingram@longhornbank.neet", Email = "b.ingram@longhornbank.neet", FName = "Brad", LName = "Ingram", MiddleInitial = Convert.ToChar("S"), StreetAddress = "6548 La Posada Ct.", City = "Austin", State = "TX", Zip = "78705", PhoneNumber = "5126978613", SSN = "545454545", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb4 = userManager.FindByName("b.ingram@longhornbank.neet");
            if (userToAddb4 == null)
            {
                userManager.Create(b4, "ingram45");
                db.SaveChanges();
                userToAddb4 = userManager.FindByName("b.ingram@longhornbank.neet");
                if (userManager.IsInRole(userToAddb4.Id, "Employee") == false)
                {
                    userManager.AddToRole(userToAddb4.Id, "Employee");
                }
            }

            AppUser b5 = new AppUser() { UserName = "a.taylor@longhornbank.neet", Email = "a.taylor@longhornbank.neet", FName = "Allison", LName = "Taylor", MiddleInitial = Convert.ToChar("R"), StreetAddress = "467 Nueces St.", City = "Dallas", State = "TX", Zip = "75237", PhoneNumber = "2148965621", SSN = "645889563", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb5 = userManager.FindByName("a.taylor@longhornbank.neet");
            if (userToAddb5 == null)
            {
                userManager.Create(b5, "nostalgic");
                db.SaveChanges();
                userToAddb5 = userManager.FindByName("a.taylor@longhornbank.neet");
                if (userManager.IsInRole(userToAddb5.Id, "Manager") == false)
                {
                    userManager.AddToRole(userToAddb5.Id, "Manager");
                }
            }

            AppUser b6 = new AppUser() { UserName = "g.martinez@longhornbank.neet", Email = "g.martinez@longhornbank.neet", FName = "Gregory", LName = "Martinez", MiddleInitial = Convert.ToChar("R"), StreetAddress = "8295 Sunset Blvd.", City = "San Antonio", State = "TX", Zip = "78239", PhoneNumber = "2105788965", SSN = "574677829", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb6 = userManager.FindByName("g.martinez@longhornbank.neet");
            if (userToAddb6 == null)
            {
                userManager.Create(b6, "fungus");
                db.SaveChanges();
                userToAddb6 = userManager.FindByName("g.martinez@longhornbank.neet");
                if (userManager.IsInRole(userToAddb6.Id, "Employee") == false)
                {
                    userManager.AddToRole(userToAddb6.Id, "Employee");
                }
            }

            AppUser b7 = new AppUser() { UserName = "m.sheffield@longhornbank.neet", Email = "m.sheffield@longhornbank.neet", FName = "Martin", LName = "Sheffield", MiddleInitial = Convert.ToChar("J"), StreetAddress = "3886 Avenue A", City = "Austin", State = "TX", Zip = "78736", PhoneNumber = "5124678821", SSN = "334557278", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb7 = userManager.FindByName("m.sheffield@longhornbank.neet");
            if (userToAddb7 == null)
            {
                userManager.Create(b7, "longhorns");
                db.SaveChanges();
                userToAddb7 = userManager.FindByName("m.sheffield@longhornbank.neet");
                if (userManager.IsInRole(userToAddb7.Id, "Manager") == false)
                {
                    userManager.AddToRole(userToAddb7.Id, "Manager");
                }
            }

            AppUser b8 = new AppUser() { UserName = "j.macleod@longhornbank.neet", Email = "j.macleod@longhornbank.neet", FName = "Jennifer", LName = "MacLeod", MiddleInitial = Convert.ToChar("D"), StreetAddress = "2504 Far West Blvd.", City = "Austin", State = "TX", Zip = "78731", PhoneNumber = "5124653365", SSN = "886719249", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb8 = userManager.FindByName("j.macleod@longhornbank.neet");
            if (userToAddb8 == null)
            {
                userManager.Create(b8, "smitty");
                db.SaveChanges();
                userToAddb8 = userManager.FindByName("j.macleod@longhornbank.neet");
                if (userManager.IsInRole(userToAddb8.Id, "Manager") == false)
                {
                    userManager.AddToRole(userToAddb8.Id, "Manager");
                }
            }

            AppUser b9 = new AppUser() { UserName = "j.tanner@longhornbank.neet", Email = "j.tanner@longhornbank.neet", FName = "Jeremy", LName = "Tanner", MiddleInitial = Convert.ToChar("S"), StreetAddress = "4347 Almstead", City = "Austin", State = "TX", Zip = "78761", PhoneNumber = "5129457399", SSN = "888887878", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb9 = userManager.FindByName("j.tanner@longhornbank.neet");
            if (userToAddb9 == null)
            {
                userManager.Create(b9, "tanman");
                db.SaveChanges();
                userToAddb9 = userManager.FindByName("j.tanner@longhornbank.neet");
                if (userManager.IsInRole(userToAddb9.Id, "Employee") == false)
                {
                    userManager.AddToRole(userToAddb9.Id, "Employee");
                }
            }

            AppUser b10 = new AppUser() { UserName = "m.rhodes@longhornbank.neet", Email = "m.rhodes@longhornbank.neet", FName = "Megan", LName = "Rhodes", MiddleInitial = Convert.ToChar("C"), StreetAddress = "4587 Enfield Rd.", City = "San Antonio", State = "TX", Zip = "78293", PhoneNumber = "2102449976", SSN = "999990909", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb10 = userManager.FindByName("m.rhodes@longhornbank.neet");
            if (userToAddb10 == null)
            {
                userManager.Create(b10, "countryrhodes");
                db.SaveChanges();
                userToAddb10 = userManager.FindByName("m.rhodes@longhornbank.neet");
                if (userManager.IsInRole(userToAddb10.Id, "Manager") == false)
                {
                    userManager.AddToRole(userToAddb10.Id, "Manager");
                }
            }

            AppUser b11 = new AppUser() { UserName = "e.stuart@longhornbank.neet", Email = "e.stuart@longhornbank.neet", FName = "Eric", LName = "Stuart", MiddleInitial = Convert.ToChar("F"), StreetAddress = "5576 Toro Ring", City = "San Antonio", State = "TX", Zip = "78279", PhoneNumber = "2105344627", SSN = "212121212", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb11 = userManager.FindByName("e.stuart@longhornbank.neet");
            if (userToAddb11 == null)
            {
                userManager.Create(b11, "stewboy");
                db.SaveChanges();
                userToAddb11 = userManager.FindByName("e.stuart@longhornbank.neet");
                if (userManager.IsInRole(userToAddb11.Id, "Manager") == false)
                {
                    userManager.AddToRole(userToAddb11.Id, "Manager");
                }
            }

            AppUser b12 = new AppUser() { UserName = "l.chung@longhornbank.neet", Email = "l.chung@longhornbank.neet", FName = "Lisa", LName = "Chung", MiddleInitial = Convert.ToChar("N"), StreetAddress = "234 RR 12", City = "San Antonio", State = "TX", Zip = "78268", PhoneNumber = "2106983548", SSN = "333333333", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb12 = userManager.FindByName("l.chung@longhornbank.neet");
            if (userToAddb12 == null)
            {
                userManager.Create(b12, "lisssa");
                db.SaveChanges();
                userToAddb12 = userManager.FindByName("l.chung@longhornbank.neet");
                if (userManager.IsInRole(userToAddb12.Id, "Employee") == false)
                {
                    userManager.AddToRole(userToAddb12.Id, "Employee");
                }
            }

            AppUser b13 = new AppUser() { UserName = "l.swanson@longhornbank.neet", Email = "l.swanson@longhornbank.neet", FName = "Leon", LName = "Swanson", MiddleInitial = Convert.ToChar(" "), StreetAddress = "245 River Rd", City = "Austin", State = "TX", Zip = "78731", PhoneNumber = "5124748138", SSN = "444444444", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb13 = userManager.FindByName("l.swanson@longhornbank.neet");
            if (userToAddb13 == null)
            {
                userManager.Create(b13, "swansong");
                db.SaveChanges();
                userToAddb13 = userManager.FindByName("l.swanson@longhornbank.neet");
                if (userManager.IsInRole(userToAddb13.Id, "Manager") == false)
                {
                    userManager.AddToRole(userToAddb13.Id, "Manager");
                }
            }

            AppUser b14 = new AppUser() { UserName = "w.loter@longhornbank.neet", Email = "w.loter@longhornbank.neet", FName = "Wanda", LName = "Loter", MiddleInitial = Convert.ToChar("K"), StreetAddress = "3453 RR 3235", City = "Austin", State = "TX", Zip = "78732", PhoneNumber = "5124579845", SSN = "555555555", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb14 = userManager.FindByName("w.loter@longhornbank.neet");
            if (userToAddb14 == null)
            {
                userManager.Create(b14, "lottery");
                db.SaveChanges();
                userToAddb14 = userManager.FindByName("w.loter@longhornbank.neet");
                if (userManager.IsInRole(userToAddb14.Id, "Employee") == false)
                {
                    userManager.AddToRole(userToAddb14.Id, "Employee");
                }
            }

            AppUser b15 = new AppUser() { UserName = "j.white@longhornbank.neet", Email = "j.white@longhornbank.neet", FName = "Jason", LName = "White", MiddleInitial = Convert.ToChar("M"), StreetAddress = "12 Valley View", City = "Houston", State = "TX", Zip = "77045", PhoneNumber = "8174955201", SSN = "666666666", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb15 = userManager.FindByName("j.white@longhornbank.neet");
            if (userToAddb15 == null)
            {
                userManager.Create(b15, "evanescent");
                db.SaveChanges();
                userToAddb15 = userManager.FindByName("j.white@longhornbank.neet");
                if (userManager.IsInRole(userToAddb15.Id, "Manager") == false)
                {
                    userManager.AddToRole(userToAddb15.Id, "Manager");
                }
            }

            AppUser b16 = new AppUser() { UserName = "w.montgomery@longhornbank.neet", Email = "w.montgomery@longhornbank.neet", FName = "Wilda", LName = "Montgomery", MiddleInitial = Convert.ToChar("K"), StreetAddress = "210 Blanco Dr", City = "Houston", State = "TX", Zip = "77030", PhoneNumber = "8178746718", SSN = "676767676", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb16 = userManager.FindByName("w.montgomery@longhornbank.neet");
            if (userToAddb16 == null)
            {
                userManager.Create(b16, "monty3");
                db.SaveChanges();
                userToAddb16 = userManager.FindByName("w.montgomery@longhornbank.neet");
                if (userManager.IsInRole(userToAddb16.Id, "Manager") == false)
                {
                    userManager.AddToRole(userToAddb16.Id, "Manager");
                }
            }

            AppUser b17 = new AppUser() { UserName = "h.morales@longhornbank.neet", Email = "h.morales@longhornbank.neet", FName = "Hector", LName = "Morales", MiddleInitial = Convert.ToChar("N"), StreetAddress = "4501 RR 140", City = "Houston", State = "TX", Zip = "77031", PhoneNumber = "8177458615", SSN = "898989898", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb17 = userManager.FindByName("h.morales@longhornbank.neet");
            if (userToAddb17 == null)
            {
                userManager.Create(b17, "hecktour");
                db.SaveChanges();
                userToAddb17 = userManager.FindByName("h.morales@longhornbank.neet");
                if (userManager.IsInRole(userToAddb17.Id, "Employee") == false)
                {
                    userManager.AddToRole(userToAddb17.Id, "Employee");
                }
            }

            AppUser b18 = new AppUser() { UserName = "m.rankin@longhornbank.neet", Email = "m.rankin@longhornbank.neet", FName = "Mary", LName = "Rankin", MiddleInitial = Convert.ToChar("T"), StreetAddress = "340 Second St", City = "Austin", State = "TX", Zip = "78703", PhoneNumber = "5122926966", SSN = "999888777", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb18 = userManager.FindByName("m.rankin@longhornbank.neet");
            if (userToAddb18 == null)
            {
                userManager.Create(b18, "rankmary");
                db.SaveChanges();
                userToAddb18 = userManager.FindByName("m.rankin@longhornbank.neet");
                if (userManager.IsInRole(userToAddb18.Id, "Employee") == false)
                {
                    userManager.AddToRole(userToAddb18.Id, "Employee");
                }
            }

            AppUser b19 = new AppUser() { UserName = "l.walker@longhornbank.neet", Email = "l.walker@longhornbank.neet", FName = "Larry", LName = "Walker", MiddleInitial = Convert.ToChar("G"), StreetAddress = "9 Bison Circle", City = "Dallas", State = "TX", Zip = "75238", PhoneNumber = "2143125897", SSN = "323232323", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb19 = userManager.FindByName("l.walker@longhornbank.neet");
            if (userToAddb19 == null)
            {
                userManager.Create(b19, "walkamile");
                db.SaveChanges();
                userToAddb19 = userManager.FindByName("l.walker@longhornbank.neet");
                if (userManager.IsInRole(userToAddb19.Id, "Manager") == false)
                {
                    userManager.AddToRole(userToAddb19.Id, "Manager");
                }
            }

            AppUser b20 = new AppUser() { UserName = "g.chang@longhornbank.neet", Email = "g.chang@longhornbank.neet", FName = "George", LName = "Chang", MiddleInitial = Convert.ToChar("M"), StreetAddress = "9003 Joshua St", City = "San Antonio", State = "TX", Zip = "78260", PhoneNumber = "2103450925", SSN = "111222233", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb20 = userManager.FindByName("g.chang@longhornbank.neet");
            if (userToAddb20 == null)
            {
                userManager.Create(b20, "changalang");
                db.SaveChanges();
                userToAddb20 = userManager.FindByName("g.chang@longhornbank.neet");
                if (userManager.IsInRole(userToAddb20.Id, "Manager") == false)
                {
                    userManager.AddToRole(userToAddb20.Id, "Manager");
                }
            }

            AppUser b21 = new AppUser() { UserName = "g.gonzalez@longhornbank.neet", Email = "g.gonzalez@longhornbank.neet", FName = "Gwen", LName = "Gonzalez", MiddleInitial = Convert.ToChar("J"), StreetAddress = "103 Manor Rd", City = "Dallas", State = "TX", Zip = "75260", PhoneNumber = "2142345566", SSN = "499551454", DOB = Convert.ToDateTime("1/1/1800") };


            AppUser userToAddb21 = userManager.FindByName("g.gonzalez@longhornbank.neet");
            if (userToAddb21 == null)
            {
                userManager.Create(b21, "offbeat");
                db.SaveChanges();
                userToAddb21 = userManager.FindByName("g.gonzalez@longhornbank.neet");
                if (userManager.IsInRole(userToAddb21.Id, "Employee") == false)
                {
                    userManager.AddToRole(userToAddb21.Id, "Employee");
                }
            }







        }
    }
}