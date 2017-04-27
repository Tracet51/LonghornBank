using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace LonghornBank.Migrations
{
    public class SeedDataCustomers
    {
        public static void SeedCustomers(AppDbContext db)
        {
            //create a user manager to add users to databases             
            UserManager<AppUser> userManager = new UserManager<AppUser>(new UserStore<AppUser>(db));

            //create a role manager             
            AppRoleManager roleManager = new AppRoleManager(new RoleStore<AppRole>(db));

            //create an admin role             
            String roleName = "Customer";

            //check to see if the role exists             
            if (roleManager.RoleExists(roleName) == false) //role doesn't exist
            {
                roleManager.Create(new AppRole(roleName));
            }


            AppUser a2 = new AppUser() { UserName = "cbaker@freezing.co.uk", Email = "cbaker@freezing.co.uk", FName = "Christopher", LName = "Baker", MiddleInitial = Convert.ToChar("L"), StreetAddress = "1245 Lake Austin Blvd.", City = "Austin", State = "TX", Zip = "78733", PhoneNumber = "5125571146", DOB = Convert.ToDateTime("1991-02-07 00:00:00"), ActiveStatus = true };


            AppUser userToAdda2 = userManager.FindByName("cbaker@freezing.co.uk");
            if (userToAdda2 == null)
            {
                userManager.Create(a2, "gazing");
                userToAdda2 = userManager.FindByName("cbaker@freezing.co.uk");
                if (userManager.IsInRole(userToAdda2.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda2.Id, "Customer");
                }
            }

            AppUser a3 = new AppUser() { UserName = "mb@aool.com", Email = "mb@aool.com", FName = "Michelle", LName = "Banks", MiddleInitial = Convert.ToChar(" "), StreetAddress = "1300 Tall Pine Lane", City = "San Antonio", State = "TX", Zip = "78261", PhoneNumber = "2102678873", DOB = Convert.ToDateTime("1990-06-23 00:00:00"), ActiveStatus = true };


            AppUser userToAdda3 = userManager.FindByName("mb@aool.com");
            if (userToAdda3 == null)
            {
                userManager.Create(a3, "banquet");
                userToAdda3 = userManager.FindByName("mb@aool.com");
                if (userManager.IsInRole(userToAdda3.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda3.Id, "Customer");
                }
            }

            AppUser a4 = new AppUser() { UserName = "fd@aool.com", Email = "fd@aool.com", FName = "Franco", LName = "Broccolo", MiddleInitial = Convert.ToChar("V"), StreetAddress = "62 Browning Rd", City = "Houston", State = "TX", Zip = "77019", PhoneNumber = "8175659699", DOB = Convert.ToDateTime("1986-05-06 00:00:00"), ActiveStatus = true };


            AppUser userToAdda4 = userManager.FindByName("fd@aool.com");
            if (userToAdda4 == null)
            {
                userManager.Create(a4, "666666");
                userToAdda4 = userManager.FindByName("fd@aool.com");
                if (userManager.IsInRole(userToAdda4.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda4.Id, "Customer");
                }
            }

            AppUser a5 = new AppUser() { UserName = "wendy@ggmail.com", Email = "wendy@ggmail.com", FName = "Wendy", LName = "Chang", MiddleInitial = Convert.ToChar("L"), StreetAddress = "202 Bellmont Hall", City = "Austin", State = "TX", Zip = "78713", PhoneNumber = "5125943222", DOB = Convert.ToDateTime("1964-12-21 00:00:00"), ActiveStatus = true };


            AppUser userToAdda5 = userManager.FindByName("wendy@ggmail.com");
            if (userToAdda5 == null)
            {
                userManager.Create(a5, "clover");
                userToAdda5 = userManager.FindByName("wendy@ggmail.com");
                if (userManager.IsInRole(userToAdda5.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda5.Id, "Customer");
                }
            }

            AppUser a6 = new AppUser() { UserName = "limchou@yaho.com", Email = "limchou@yaho.com", FName = "Lim", LName = "Chou", MiddleInitial = Convert.ToChar(" "), StreetAddress = "1600 Teresa Lane", City = "San Antonio", State = "TX", Zip = "78266", PhoneNumber = "2107724599", DOB = Convert.ToDateTime("1950-06-14 00:00:00"), ActiveStatus = true };


            AppUser userToAdda6 = userManager.FindByName("limchou@yaho.com");
            if (userToAdda6 == null)
            {
                userManager.Create(a6, "austin");
                userToAdda6 = userManager.FindByName("limchou@yaho.com");
                if (userManager.IsInRole(userToAdda6.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda6.Id, "Customer");
                }
            }

            AppUser a7 = new AppUser() { UserName = "Dixon@aool.com", Email = "Dixon@aool.com", FName = "Shan", LName = "Dixon", MiddleInitial = Convert.ToChar("D"), StreetAddress = "234 Holston Circle", City = "Dallas", State = "TX", Zip = "75208", PhoneNumber = "2142643255", DOB = Convert.ToDateTime("1930-05-09 00:00:00"), ActiveStatus = true };


            AppUser userToAdda7 = userManager.FindByName("Dixon@aool.com");
            if (userToAdda7 == null)
            {
                userManager.Create(a7, "mailbox");
                userToAdda7 = userManager.FindByName("Dixon@aool.com");
                if (userManager.IsInRole(userToAdda7.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda7.Id, "Customer");
                }
            }

            AppUser a8 = new AppUser() { UserName = "louann@ggmail.com", Email = "louann@ggmail.com", FName = "Lou Ann", LName = "Feeley", MiddleInitial = Convert.ToChar("K"), StreetAddress = "600 S 8th Street W", City = "Houston", State = "TX", Zip = "77010", PhoneNumber = "8172556749", DOB = Convert.ToDateTime("1930-02-24 00:00:00"), ActiveStatus = true };


            AppUser userToAdda8 = userManager.FindByName("louann@ggmail.com");
            if (userToAdda8 == null)
            {
                userManager.Create(a8, "aggies");
                userToAdda8 = userManager.FindByName("louann@ggmail.com");
                if (userManager.IsInRole(userToAdda8.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda8.Id, "Customer");
                }
            }

            AppUser a9 = new AppUser() { UserName = "tfreeley@minntonka.ci.state.mn.us", Email = "tfreeley@minntonka.ci.state.mn.us", FName = "Tesa", LName = "Freeley", MiddleInitial = Convert.ToChar("P"), StreetAddress = "4448 Fairview Ave.", City = "Houston", State = "TX", Zip = "77009", PhoneNumber = "8173255687", DOB = Convert.ToDateTime("1935-09-01 00:00:00"), ActiveStatus = true };


            AppUser userToAdda9 = userManager.FindByName("tfreeley@minntonka.ci.state.mn.us");
            if (userToAdda9 == null)
            {
                userManager.Create(a9, "raiders");
                userToAdda9 = userManager.FindByName("tfreeley@minntonka.ci.state.mn.us");
                if (userManager.IsInRole(userToAdda9.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda9.Id, "Customer");
                }
            }

            AppUser a10 = new AppUser() { UserName = "mgar@aool.com", Email = "mgar@aool.com", FName = "Margaret", LName = "Garcia", MiddleInitial = Convert.ToChar("L"), StreetAddress = "594 Longview", City = "Houston", State = "TX", Zip = "77003", PhoneNumber = "8176593544", DOB = Convert.ToDateTime("1990-07-03 00:00:00"), ActiveStatus = true };


            AppUser userToAdda10 = userManager.FindByName("mgar@aool.com");
            if (userToAdda10 == null)
            {
                userManager.Create(a10, "mustangs");
                userToAdda10 = userManager.FindByName("mgar@aool.com");
                if (userManager.IsInRole(userToAdda10.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda10.Id, "Customer");
                }
            }

            AppUser a11 = new AppUser() { UserName = "chaley@thug.com", Email = "chaley@thug.com", FName = "Charles", LName = "Haley", MiddleInitial = Convert.ToChar("E"), StreetAddress = "One Cowboy Pkwy", City = "Dallas", State = "TX", Zip = "75261", PhoneNumber = "2148475583", DOB = Convert.ToDateTime("1985-09-17 00:00:00"), ActiveStatus = true };


            AppUser userToAdda11 = userManager.FindByName("chaley@thug.com");
            if (userToAdda11 == null)
            {
                userManager.Create(a11, "region");
                userToAdda11 = userManager.FindByName("chaley@thug.com");
                if (userManager.IsInRole(userToAdda11.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda11.Id, "Customer");
                }
            }

            AppUser a12 = new AppUser() { UserName = "jeff@ggmail.com", Email = "jeff@ggmail.com", FName = "Jeffrey", LName = "Hampton", MiddleInitial = Convert.ToChar("T"), StreetAddress = "337 38th St.", City = "Austin", State = "TX", Zip = "78705", PhoneNumber = "5126978613", DOB = Convert.ToDateTime("1995-01-23 00:00:00"), ActiveStatus = true };


            AppUser userToAdda12 = userManager.FindByName("jeff@ggmail.com");
            if (userToAdda12 == null)
            {
                userManager.Create(a12, "hungry");
                userToAdda12 = userManager.FindByName("jeff@ggmail.com");
                if (userManager.IsInRole(userToAdda12.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda12.Id, "Customer");
                }
            }

            AppUser a13 = new AppUser() { UserName = "wjhearniii@umch.edu", Email = "wjhearniii@umch.edu", FName = "John", LName = "Hearn", MiddleInitial = Convert.ToChar("B"), StreetAddress = "4225 North First", City = "Dallas", State = "TX", Zip = "75237", PhoneNumber = "2148965621", DOB = Convert.ToDateTime("1994-01-08 00:00:00"), ActiveStatus = true };


            AppUser userToAdda13 = userManager.FindByName("wjhearniii@umch.edu");
            if (userToAdda13 == null)
            {
                userManager.Create(a13, "logicon");
                userToAdda13 = userManager.FindByName("wjhearniii@umch.edu");
                if (userManager.IsInRole(userToAdda13.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda13.Id, "Customer");
                }
            }

            AppUser a14 = new AppUser() { UserName = "hicks43@ggmail.com", Email = "hicks43@ggmail.com", FName = "Anthony", LName = "Hicks", MiddleInitial = Convert.ToChar("J"), StreetAddress = "32 NE Garden Ln., Ste 910", City = "San Antonio", State = "TX", Zip = "78239", PhoneNumber = "2105788965", DOB = Convert.ToDateTime("1990-10-06 00:00:00"), ActiveStatus = true };


            AppUser userToAdda14 = userManager.FindByName("hicks43@ggmail.com");
            if (userToAdda14 == null)
            {
                userManager.Create(a14, "doofus");
                userToAdda14 = userManager.FindByName("hicks43@ggmail.com");
                if (userManager.IsInRole(userToAdda14.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda14.Id, "Customer");
                }
            }

            AppUser a15 = new AppUser() { UserName = "bradsingram@mall.utexas.edu", Email = "bradsingram@mall.utexas.edu", FName = "Brad", LName = "Ingram", MiddleInitial = Convert.ToChar("S"), StreetAddress = "6548 La Posada Ct.", City = "Austin", State = "TX", Zip = "78736", PhoneNumber = "5124678821", DOB = Convert.ToDateTime("1984-04-12 00:00:00"), ActiveStatus = true };


            AppUser userToAdda15 = userManager.FindByName("bradsingram@mall.utexas.edu");
            if (userToAdda15 == null)
            {
                userManager.Create(a15, "mother");
                userToAdda15 = userManager.FindByName("bradsingram@mall.utexas.edu");
                if (userManager.IsInRole(userToAdda15.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda15.Id, "Customer");
                }
            }

            AppUser a16 = new AppUser() { UserName = "mother.Ingram@aool.com", Email = "mother.Ingram@aool.com", FName = "Todd", LName = "Jacobs", MiddleInitial = Convert.ToChar("L"), StreetAddress = "4564 Elm St.", City = "Austin", State = "TX", Zip = "78731", PhoneNumber = "5124653365", DOB = Convert.ToDateTime("1983-04-04 00:00:00"), ActiveStatus = true };


            AppUser userToAdda16 = userManager.FindByName("mother.Ingram@aool.com");
            if (userToAdda16 == null)
            {
                userManager.Create(a16, "whimsical");
                userToAdda16 = userManager.FindByName("mother.Ingram@aool.com");
                if (userManager.IsInRole(userToAdda16.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda16.Id, "Customer");
                }
            }

            AppUser a17 = new AppUser() { UserName = "victoria@aool.com", Email = "victoria@aool.com", FName = "Victoria", LName = "Lawrence", MiddleInitial = Convert.ToChar("M"), StreetAddress = "6639 Butterfly Ln.", City = "Austin", State = "TX", Zip = "78761", PhoneNumber = "5129457399", DOB = Convert.ToDateTime("1961-02-03 00:00:00"), ActiveStatus = true };


            AppUser userToAdda17 = userManager.FindByName("victoria@aool.com");
            if (userToAdda17 == null)
            {
                userManager.Create(a17, "nothing");
                userToAdda17 = userManager.FindByName("victoria@aool.com");
                if (userManager.IsInRole(userToAdda17.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda17.Id, "Customer");
                }
            }

            AppUser a18 = new AppUser() { UserName = "lineback@flush.net", Email = "lineback@flush.net", FName = "Erik", LName = "Lineback", MiddleInitial = Convert.ToChar("W"), StreetAddress = "1300 Netherland St", City = "San Antonio", State = "TX", Zip = "78293", PhoneNumber = "2102449976", DOB = Convert.ToDateTime("1946-09-03 00:00:00"), ActiveStatus = true };


            AppUser userToAdda18 = userManager.FindByName("lineback@flush.net");
            if (userToAdda18 == null)
            {
                userManager.Create(a18, "GoodFellow");
                userToAdda18 = userManager.FindByName("lineback@flush.net");
                if (userManager.IsInRole(userToAdda18.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda18.Id, "Customer");
                }
            }

            AppUser a19 = new AppUser() { UserName = "elowe@netscrape.net", Email = "elowe@netscrape.net", FName = "Ernest", LName = "Lowe", MiddleInitial = Convert.ToChar("S"), StreetAddress = "3201 Pine Drive", City = "San Antonio", State = "TX", Zip = "78279", PhoneNumber = "2105344627", DOB = Convert.ToDateTime("1992-02-07 00:00:00"), ActiveStatus = true };


            AppUser userToAdda19 = userManager.FindByName("elowe@netscrape.net");
            if (userToAdda19 == null)
            {
                userManager.Create(a19, "impede");
                userToAdda19 = userManager.FindByName("elowe@netscrape.net");
                if (userManager.IsInRole(userToAdda19.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda19.Id, "Customer");
                }
            }

            AppUser a20 = new AppUser() { UserName = "luce_chuck@ggmail.com", Email = "luce_chuck@ggmail.com", FName = "Chuck", LName = "Luce", MiddleInitial = Convert.ToChar("B"), StreetAddress = "2345 Rolling Clouds", City = "San Antonio", State = "TX", Zip = "78268", PhoneNumber = "2106983548", DOB = Convert.ToDateTime("1942-10-25 00:00:00"), ActiveStatus = true };


            AppUser userToAdda20 = userManager.FindByName("luce_chuck@ggmail.com");
            if (userToAdda20 == null)
            {
                userManager.Create(a20, "LuceyDucey");
                userToAdda20 = userManager.FindByName("luce_chuck@ggmail.com");
                if (userManager.IsInRole(userToAdda20.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda20.Id, "Customer");
                }
            }

            AppUser a21 = new AppUser() { UserName = "mackcloud@pimpdaddy.com", Email = "mackcloud@pimpdaddy.com", FName = "Jennifer", LName = "MacLeod", MiddleInitial = Convert.ToChar("D"), StreetAddress = "2504 Far West Blvd.", City = "Austin", State = "TX", Zip = "78731", PhoneNumber = "5124748138", DOB = Convert.ToDateTime("1965-08-06 00:00:00"), ActiveStatus = true };


            AppUser userToAdda21 = userManager.FindByName("mackcloud@pimpdaddy.com");
            if (userToAdda21 == null)
            {
                userManager.Create(a21, "cloudyday");
                userToAdda21 = userManager.FindByName("mackcloud@pimpdaddy.com");
                if (userManager.IsInRole(userToAdda21.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda21.Id, "Customer");
                }
            }

            AppUser a22 = new AppUser() { UserName = "liz@ggmail.com", Email = "liz@ggmail.com", FName = "Elizabeth", LName = "Markham", MiddleInitial = Convert.ToChar("P"), StreetAddress = "7861 Chevy Chase", City = "Austin", State = "TX", Zip = "78732", PhoneNumber = "5124579845", DOB = Convert.ToDateTime("1959-04-13 00:00:00"), ActiveStatus = true };


            AppUser userToAdda22 = userManager.FindByName("liz@ggmail.com");
            if (userToAdda22 == null)
            {
                userManager.Create(a22, "emarkbark");
                userToAdda22 = userManager.FindByName("liz@ggmail.com");
                if (userManager.IsInRole(userToAdda22.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda22.Id, "Customer");
                }
            }

            AppUser a23 = new AppUser() { UserName = "mclarence@aool.com", Email = "mclarence@aool.com", FName = "Clarence", LName = "Martin", MiddleInitial = Convert.ToChar("A"), StreetAddress = "87 Alcedo St.", City = "Houston", State = "TX", Zip = "77045", PhoneNumber = "8174955201", DOB = Convert.ToDateTime("1990-01-06 00:00:00"), ActiveStatus = true };


            AppUser userToAdda23 = userManager.FindByName("mclarence@aool.com");
            if (userToAdda23 == null)
            {
                userManager.Create(a23, "smartinmartin");
                userToAdda23 = userManager.FindByName("mclarence@aool.com");
                if (userManager.IsInRole(userToAdda23.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda23.Id, "Customer");
                }
            }

            AppUser a24 = new AppUser() { UserName = "smartinmartin.Martin@aool.com", Email = "smartinmartin.Martin@aool.com", FName = "Gregory", LName = "Martinez", MiddleInitial = Convert.ToChar("R"), StreetAddress = "8295 Sunset Blvd.", City = "Houston", State = "TX", Zip = "77030", PhoneNumber = "8178746718", DOB = Convert.ToDateTime("1987-10-09 00:00:00"), ActiveStatus = true };


            AppUser userToAdda24 = userManager.FindByName("smartinmartin.Martin@aool.com");
            if (userToAdda24 == null)
            {
                userManager.Create(a24, "looter");
                userToAdda24 = userManager.FindByName("smartinmartin.Martin@aool.com");
                if (userManager.IsInRole(userToAdda24.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda24.Id, "Customer");
                }
            }

            AppUser a25 = new AppUser() { UserName = "cmiller@mapster.com", Email = "cmiller@mapster.com", FName = "Charles", LName = "Miller", MiddleInitial = Convert.ToChar("R"), StreetAddress = "8962 Main St.", City = "Houston", State = "TX", Zip = "77031", PhoneNumber = "8177458615", DOB = Convert.ToDateTime("1984-07-21 00:00:00"), ActiveStatus = true };


            AppUser userToAdda25 = userManager.FindByName("cmiller@mapster.com");
            if (userToAdda25 == null)
            {
                userManager.Create(a25, "chucky33");
                userToAdda25 = userManager.FindByName("cmiller@mapster.com");
                if (userManager.IsInRole(userToAdda25.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda25.Id, "Customer");
                }
            }

            AppUser a26 = new AppUser() { UserName = "nelson.Kelly@aool.com", Email = "nelson.Kelly@aool.com", FName = "Kelly", LName = "Nelson", MiddleInitial = Convert.ToChar("T"), StreetAddress = "2601 Red River", City = "Austin", State = "TX", Zip = "78703", PhoneNumber = "5122926966", DOB = Convert.ToDateTime("1956-07-04 00:00:00"), ActiveStatus = true };


            AppUser userToAdda26 = userManager.FindByName("nelson.Kelly@aool.com");
            if (userToAdda26 == null)
            {
                userManager.Create(a26, "orange");
                userToAdda26 = userManager.FindByName("nelson.Kelly@aool.com");
                if (userManager.IsInRole(userToAdda26.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda26.Id, "Customer");
                }
            }

            AppUser a27 = new AppUser() { UserName = "jojoe@ggmail.com", Email = "jojoe@ggmail.com", FName = "Joe", LName = "Nguyen", MiddleInitial = Convert.ToChar("C"), StreetAddress = "1249 4th SW St.", City = "Dallas", State = "TX", Zip = "75238", PhoneNumber = "2143125897", DOB = Convert.ToDateTime("1963-01-29 00:00:00"), ActiveStatus = true };


            AppUser userToAdda27 = userManager.FindByName("jojoe@ggmail.com");
            if (userToAdda27 == null)
            {
                userManager.Create(a27, "victorious");
                userToAdda27 = userManager.FindByName("jojoe@ggmail.com");
                if (userManager.IsInRole(userToAdda27.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda27.Id, "Customer");
                }
            }

            AppUser a28 = new AppUser() { UserName = "orielly@foxnets.com", Email = "orielly@foxnets.com", FName = "Bill", LName = "O'Reilly", MiddleInitial = Convert.ToChar("T"), StreetAddress = "8800 Gringo Drive", City = "San Antonio", State = "TX", Zip = "78260", PhoneNumber = "2103450925", DOB = Convert.ToDateTime("1983-01-07 00:00:00"), ActiveStatus = true };


            AppUser userToAdda28 = userManager.FindByName("orielly@foxnets.com");
            if (userToAdda28 == null)
            {
                userManager.Create(a28, "billyboy");
                userToAdda28 = userManager.FindByName("orielly@foxnets.com");
                if (userManager.IsInRole(userToAdda28.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda28.Id, "Customer");
                }
            }

            AppUser a29 = new AppUser() { UserName = "or@aool.com", Email = "or@aool.com", FName = "Anka", LName = "Radkovich", MiddleInitial = Convert.ToChar("L"), StreetAddress = "1300 Elliott Pl", City = "Dallas", State = "TX", Zip = "75260", PhoneNumber = "2142345566", DOB = Convert.ToDateTime("1980-03-31 00:00:00"), ActiveStatus = true };


            AppUser userToAdda29 = userManager.FindByName("or@aool.com");
            if (userToAdda29 == null)
            {
                userManager.Create(a29, "radicalone");
                userToAdda29 = userManager.FindByName("or@aool.com");
                if (userManager.IsInRole(userToAdda29.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda29.Id, "Customer");
                }
            }

            AppUser a30 = new AppUser() { UserName = "megrhodes@freezing.co.uk", Email = "megrhodes@freezing.co.uk", FName = "Megan", LName = "Rhodes", MiddleInitial = Convert.ToChar("C"), StreetAddress = "4587 Enfield Rd.", City = "Austin", State = "TX", Zip = "78707", PhoneNumber = "5123744746", DOB = Convert.ToDateTime("1944-08-12 00:00:00"), ActiveStatus = true };


            AppUser userToAdda30 = userManager.FindByName("megrhodes@freezing.co.uk");
            if (userToAdda30 == null)
            {
                userManager.Create(a30, "gohorns");
                userToAdda30 = userManager.FindByName("megrhodes@freezing.co.uk");
                if (userManager.IsInRole(userToAdda30.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda30.Id, "Customer");
                }
            }

            AppUser a31 = new AppUser() { UserName = "erynrice@aool.com", Email = "erynrice@aool.com", FName = "Eryn", LName = "Rice", MiddleInitial = Convert.ToChar("M"), StreetAddress = "3405 Rio Grande", City = "Austin", State = "TX", Zip = "78705", PhoneNumber = "5123876657", DOB = Convert.ToDateTime("1934-08-02 00:00:00"), ActiveStatus = true };


            AppUser userToAdda31 = userManager.FindByName("erynrice@aool.com");
            if (userToAdda31 == null)
            {
                userManager.Create(a31, "iloveme");
                userToAdda31 = userManager.FindByName("erynrice@aool.com");
                if (userManager.IsInRole(userToAdda31.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda31.Id, "Customer");
                }
            }

            AppUser a32 = new AppUser() { UserName = "jorge@hootmail.com", Email = "jorge@hootmail.com", FName = "Jorge", LName = "Rodriguez", MiddleInitial = Convert.ToChar(" "), StreetAddress = "6788 Cotter Street", City = "Houston", State = "TX", Zip = "77057", PhoneNumber = "8178904374", DOB = Convert.ToDateTime("1989-08-11 00:00:00"), ActiveStatus = true };


            AppUser userToAdda32 = userManager.FindByName("jorge@hootmail.com");
            if (userToAdda32 == null)
            {
                userManager.Create(a32, "greedy");
                userToAdda32 = userManager.FindByName("jorge@hootmail.com");
                if (userManager.IsInRole(userToAdda32.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda32.Id, "Customer");
                }
            }

            AppUser a33 = new AppUser() { UserName = "ra@aoo.com", Email = "ra@aoo.com", FName = "Allen", LName = "Rogers", MiddleInitial = Convert.ToChar("B"), StreetAddress = "4965 Oak Hill", City = "Austin", State = "TX", Zip = "78732", PhoneNumber = "5128752943", DOB = Convert.ToDateTime("1967-08-27 00:00:00"), ActiveStatus = true };


            AppUser userToAdda33 = userManager.FindByName("ra@aoo.com");
            if (userToAdda33 == null)
            {
                userManager.Create(a33, "familiar");
                userToAdda33 = userManager.FindByName("ra@aoo.com");
                if (userManager.IsInRole(userToAdda33.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda33.Id, "Customer");
                }
            }

            AppUser a34 = new AppUser() { UserName = "stjean@home.com", Email = "stjean@home.com", FName = "Olivier", LName = "Saint-Jean", MiddleInitial = Convert.ToChar("M"), StreetAddress = "255 Toncray Dr.", City = "San Antonio", State = "TX", Zip = "78292", PhoneNumber = "2104145678", DOB = Convert.ToDateTime("1950-07-08 00:00:00"), ActiveStatus = true };


            AppUser userToAdda34 = userManager.FindByName("stjean@home.com");
            if (userToAdda34 == null)
            {
                userManager.Create(a34, "historical");
                userToAdda34 = userManager.FindByName("stjean@home.com");
                if (userManager.IsInRole(userToAdda34.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda34.Id, "Customer");
                }
            }

            AppUser a35 = new AppUser() { UserName = "ss34@ggmail.com", Email = "ss34@ggmail.com", FName = "Sarah", LName = "Saunders", MiddleInitial = Convert.ToChar("J"), StreetAddress = "332 Avenue C", City = "Austin", State = "TX", Zip = "78705", PhoneNumber = "5123497810", DOB = Convert.ToDateTime("1977-10-29 00:00:00"), ActiveStatus = true };


            AppUser userToAdda35 = userManager.FindByName("ss34@ggmail.com");
            if (userToAdda35 == null)
            {
                userManager.Create(a35, "guiltless");
                userToAdda35 = userManager.FindByName("ss34@ggmail.com");
                if (userManager.IsInRole(userToAdda35.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda35.Id, "Customer");
                }
            }

            AppUser a36 = new AppUser() { UserName = "willsheff@email.com", Email = "willsheff@email.com", FName = "William", LName = "Sewell", MiddleInitial = Convert.ToChar("T"), StreetAddress = "2365 51st St.", City = "Austin", State = "TX", Zip = "78709", PhoneNumber = "5124510084", DOB = Convert.ToDateTime("1941-04-21 00:00:00"), ActiveStatus = true };


            AppUser userToAdda36 = userManager.FindByName("willsheff@email.com");
            if (userToAdda36 == null)
            {
                userManager.Create(a36, "frequent");
                userToAdda36 = userManager.FindByName("willsheff@email.com");
                if (userManager.IsInRole(userToAdda36.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda36.Id, "Customer");
                }
            }

            AppUser a37 = new AppUser() { UserName = "sheff44@ggmail.com", Email = "sheff44@ggmail.com", FName = "Martin", LName = "Sheffield", MiddleInitial = Convert.ToChar("J"), StreetAddress = "3886 Avenue A", City = "Austin", State = "TX", Zip = "78705", PhoneNumber = "5125479167", DOB = Convert.ToDateTime("1937-11-10 00:00:00"), ActiveStatus = true };


            AppUser userToAdda37 = userManager.FindByName("sheff44@ggmail.com");
            if (userToAdda37 == null)
            {
                userManager.Create(a37, "history");
                userToAdda37 = userManager.FindByName("sheff44@ggmail.com");
                if (userManager.IsInRole(userToAdda37.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda37.Id, "Customer");
                }
            }

            AppUser a38 = new AppUser() { UserName = "johnsmith187@aool.com", Email = "johnsmith187@aool.com", FName = "John", LName = "Smith", MiddleInitial = Convert.ToChar("A"), StreetAddress = "23 Hidden Forge Dr.", City = "San Antonio", State = "TX", Zip = "78280", PhoneNumber = "2108321888", DOB = Convert.ToDateTime("1954-10-26 00:00:00"), ActiveStatus = true };


            AppUser userToAdda38 = userManager.FindByName("johnsmith187@aool.com");
            if (userToAdda38 == null)
            {
                userManager.Create(a38, "squirrel");
                userToAdda38 = userManager.FindByName("johnsmith187@aool.com");
                if (userManager.IsInRole(userToAdda38.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda38.Id, "Customer");
                }
            }

            AppUser a39 = new AppUser() { UserName = "dustroud@mail.com", Email = "dustroud@mail.com", FName = "Dustin", LName = "Stroud", MiddleInitial = Convert.ToChar("P"), StreetAddress = "1212 Rita Rd", City = "Dallas", State = "TX", Zip = "75221", PhoneNumber = "2142346667", DOB = Convert.ToDateTime("1932-09-01 00:00:00"), ActiveStatus = true };


            AppUser userToAdda39 = userManager.FindByName("dustroud@mail.com");
            if (userToAdda39 == null)
            {
                userManager.Create(a39, "snakes");
                userToAdda39 = userManager.FindByName("dustroud@mail.com");
                if (userManager.IsInRole(userToAdda39.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda39.Id, "Customer");
                }
            }

            AppUser a40 = new AppUser() { UserName = "ericstuart@aool.com", Email = "ericstuart@aool.com", FName = "Eric", LName = "Stuart", MiddleInitial = Convert.ToChar("D"), StreetAddress = "5576 Toro Ring", City = "Austin", State = "TX", Zip = "78746", PhoneNumber = "5128178335", DOB = Convert.ToDateTime("1930-12-28 00:00:00"), ActiveStatus = true };


            AppUser userToAdda40 = userManager.FindByName("ericstuart@aool.com");
            if (userToAdda40 == null)
            {
                userManager.Create(a40, "landus");
                userToAdda40 = userManager.FindByName("ericstuart@aool.com");
                if (userManager.IsInRole(userToAdda40.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda40.Id, "Customer");
                }
            }

            AppUser a41 = new AppUser() { UserName = "peterstump@hootmail.com", Email = "peterstump@hootmail.com", FName = "Peter", LName = "Stump", MiddleInitial = Convert.ToChar("L"), StreetAddress = "1300 Kellen Circle", City = "Houston", State = "TX", Zip = "77018", PhoneNumber = "8174560903", DOB = Convert.ToDateTime("1989-08-13 00:00:00"), ActiveStatus = true };


            AppUser userToAdda41 = userManager.FindByName("peterstump@hootmail.com");
            if (userToAdda41 == null)
            {
                userManager.Create(a41, "rhythm");
                userToAdda41 = userManager.FindByName("peterstump@hootmail.com");
                if (userManager.IsInRole(userToAdda41.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda41.Id, "Customer");
                }
            }

            AppUser a42 = new AppUser() { UserName = "tanner@ggmail.com", Email = "tanner@ggmail.com", FName = "Jeremy", LName = "Tanner", MiddleInitial = Convert.ToChar("S"), StreetAddress = "4347 Almstead", City = "Houston", State = "TX", Zip = "77044", PhoneNumber = "8174590929", DOB = Convert.ToDateTime("1982-05-21 00:00:00"), ActiveStatus = true };


            AppUser userToAdda42 = userManager.FindByName("tanner@ggmail.com");
            if (userToAdda42 == null)
            {
                userManager.Create(a42, "kindly");
                userToAdda42 = userManager.FindByName("tanner@ggmail.com");
                if (userManager.IsInRole(userToAdda42.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda42.Id, "Customer");
                }
            }

            AppUser a43 = new AppUser() { UserName = "taylordjay@aool.com", Email = "taylordjay@aool.com", FName = "Allison", LName = "Taylor", MiddleInitial = Convert.ToChar("R"), StreetAddress = "467 Nueces St.", City = "Austin", State = "TX", Zip = "78705", PhoneNumber = "5124748452", DOB = Convert.ToDateTime("1960-01-08 00:00:00"), ActiveStatus = true };


            AppUser userToAdda43 = userManager.FindByName("taylordjay@aool.com");
            if (userToAdda43 == null)
            {
                userManager.Create(a43, "instrument");
                userToAdda43 = userManager.FindByName("taylordjay@aool.com");
                if (userManager.IsInRole(userToAdda43.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda43.Id, "Customer");
                }
            }

            AppUser a44 = new AppUser() { UserName = "TayTaylor@aool.com", Email = "TayTaylor@aool.com", FName = "Rachel", LName = "Taylor", MiddleInitial = Convert.ToChar("K"), StreetAddress = "345 Longview Dr.", City = "Austin", State = "TX", Zip = "78705", PhoneNumber = "5124512631", DOB = Convert.ToDateTime("1975-07-27 00:00:00"), ActiveStatus = true };


            AppUser userToAdda44 = userManager.FindByName("TayTaylor@aool.com");
            if (userToAdda44 == null)
            {
                userManager.Create(a44, "arched");
                userToAdda44 = userManager.FindByName("TayTaylor@aool.com");
                if (userManager.IsInRole(userToAdda44.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda44.Id, "Customer");
                }
            }

            AppUser a45 = new AppUser() { UserName = "teefrank@hootmail.com", Email = "teefrank@hootmail.com", FName = "Frank", LName = "Tee", MiddleInitial = Convert.ToChar("J"), StreetAddress = "5590 Lavell Dr", City = "Houston", State = "TX", Zip = "77004", PhoneNumber = "8178765543", DOB = Convert.ToDateTime("1968-04-06 00:00:00"), ActiveStatus = true };


            AppUser userToAdda45 = userManager.FindByName("teefrank@hootmail.com");
            if (userToAdda45 == null)
            {
                userManager.Create(a45, "median");
                userToAdda45 = userManager.FindByName("teefrank@hootmail.com");
                if (userManager.IsInRole(userToAdda45.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda45.Id, "Customer");
                }
            }

            AppUser a46 = new AppUser() { UserName = "tuck33@ggmail.com", Email = "tuck33@ggmail.com", FName = "Clent", LName = "Tucker", MiddleInitial = Convert.ToChar("J"), StreetAddress = "312 Main St.", City = "Dallas", State = "TX", Zip = "75315", PhoneNumber = "2148471154", DOB = Convert.ToDateTime("1978-05-19 00:00:00"), ActiveStatus = true };


            AppUser userToAdda46 = userManager.FindByName("tuck33@ggmail.com");
            if (userToAdda46 == null)
            {
                userManager.Create(a46, "approval");
                userToAdda46 = userManager.FindByName("tuck33@ggmail.com");
                if (userManager.IsInRole(userToAdda46.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda46.Id, "Customer");
                }
            }

            AppUser a47 = new AppUser() { UserName = "avelasco@yaho.com", Email = "avelasco@yaho.com", FName = "Allen", LName = "Velasco", MiddleInitial = Convert.ToChar("G"), StreetAddress = "679 W. 4th", City = "Dallas", State = "TX", Zip = "75207", PhoneNumber = "2143985638", DOB = Convert.ToDateTime("1963-10-06 00:00:00"), ActiveStatus = true };


            AppUser userToAdda47 = userManager.FindByName("avelasco@yaho.com");
            if (userToAdda47 == null)
            {
                userManager.Create(a47, "decorate");
                userToAdda47 = userManager.FindByName("avelasco@yaho.com");
                if (userManager.IsInRole(userToAdda47.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda47.Id, "Customer");
                }
            }

            AppUser a48 = new AppUser() { UserName = "westj@pioneer.net", Email = "westj@pioneer.net", FName = "Jake", LName = "West", MiddleInitial = Convert.ToChar("T"), StreetAddress = "RR 3287", City = "Dallas", State = "TX", Zip = "75323", PhoneNumber = "2148475244", DOB = Convert.ToDateTime("1993-10-14 00:00:00"), ActiveStatus = true };


            AppUser userToAdda48 = userManager.FindByName("westj@pioneer.net");
            if (userToAdda48 == null)
            {
                userManager.Create(a48, "grover");
                userToAdda48 = userManager.FindByName("westj@pioneer.net");
                if (userManager.IsInRole(userToAdda48.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda48.Id, "Customer");
                }
            }

            AppUser a49 = new AppUser() { UserName = "louielouie@aool.com", Email = "louielouie@aool.com", FName = "Louis", LName = "Winthorpe", MiddleInitial = Convert.ToChar("L"), StreetAddress = "2500 Padre Blvd", City = "Dallas", State = "TX", Zip = "75220", PhoneNumber = "2145650098", DOB = Convert.ToDateTime("1952-05-31 00:00:00"), ActiveStatus = true };


            AppUser userToAdda49 = userManager.FindByName("louielouie@aool.com");
            if (userToAdda49 == null)
            {
                userManager.Create(a49, "sturdy");
                userToAdda49 = userManager.FindByName("louielouie@aool.com");
                if (userManager.IsInRole(userToAdda49.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda49.Id, "Customer");
                }
            }

            AppUser a50 = new AppUser() { UserName = "rwood@voyager.net", Email = "rwood@voyager.net", FName = "Reagan", LName = "Wood", MiddleInitial = Convert.ToChar("B"), StreetAddress = "447 Westlake Dr.", City = "Austin", State = "TX", Zip = "78746", PhoneNumber = "5124545242", DOB = Convert.ToDateTime("1992-04-24 00:00:00"), ActiveStatus = true };


            AppUser userToAdda50 = userManager.FindByName("rwood@voyager.net");
            if (userToAdda50 == null)
            {
                userManager.Create(a50, "decorous");
                userToAdda50 = userManager.FindByName("rwood@voyager.net");
                if (userManager.IsInRole(userToAdda50.Id, "Customer") == false)
                {
                    userManager.AddToRole(userToAdda50.Id, "Customer");
                }
            }




        }

    }
}