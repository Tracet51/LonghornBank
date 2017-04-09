namespace LonghornBank.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using LonghornBank.Models;
    using LonghornBank.Controllers;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<LonghornBank.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        public static AppDbContext db = new AppDbContext();

        protected override void Seed(LonghornBank.Models.AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var hasher = new PasswordHasher();


            AppUser a2 = new AppUser { Email = "cbaker@freezing.co.uk", PasswordHash = hasher.HashPassword("hello"), FName = "Christopher", LName = "Baker", MiddleInitial = Convert.ToChar("L"), StreetAddress = "1245 Lake Austin Blvd.", City = "Austin", State = "TX", Zip = "78733", PhoneNumber = "5125571146", DOB = Convert.ToDateTime("1991-02-07 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a2);


            AppUser a3 = new AppUser { Email = "mb@aool.com", PasswordHash = hasher.HashPassword("banquet"), FName = "Michelle", LName = "Banks", MiddleInitial = Convert.ToChar("None"), StreetAddress = "1300 Tall Pine Lane", City = "San Antonio", State = "TX", Zip = "78261", PhoneNumber = "2102678873", DOB = Convert.ToDateTime("1990-06-23 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a3);


            AppUser a4 = new AppUser { Email = "fd@aool.com", PasswordHash = hasher.HashPassword("666666"), FName = "Franco", LName = "Broccolo", MiddleInitial = Convert.ToChar("V"), StreetAddress = "62 Browning Rd", City = "Houston", State = "TX", Zip = "77019", PhoneNumber = "8175659699", DOB = Convert.ToDateTime("1986-05-06 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a4);


            AppUser a5 = new AppUser { Email = "wendy@ggmail.com", PasswordHash = hasher.HashPassword("texas"), FName = "Wendy", LName = "Chang", MiddleInitial = Convert.ToChar("L"), StreetAddress = "202 Bellmont Hall", City = "Austin", State = "TX", Zip = "78713", PhoneNumber = "5125943222", DOB = Convert.ToDateTime("1964-12-21 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a5);


            AppUser a6 = new AppUser { Email = "limchou@yaho.com", PasswordHash = hasher.HashPassword("austin"), FName = "Lim", LName = "Chou", MiddleInitial = Convert.ToChar("None"), StreetAddress = "1600 Teresa Lane", City = "San Antonio", State = "TX", Zip = "78266", PhoneNumber = "2107724599", DOB = Convert.ToDateTime("1950-06-14 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a6);


            AppUser a7 = new AppUser { Email = "Dixon@aool.com", PasswordHash = hasher.HashPassword("mailbox"), FName = "Shan", LName = "Dixon", MiddleInitial = Convert.ToChar("D"), StreetAddress = "234 Holston Circle", City = "Dallas", State = "TX", Zip = "75208", PhoneNumber = "2142643255", DOB = Convert.ToDateTime("1930-05-09 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a7);


            AppUser a8 = new AppUser { Email = "louann@ggmail.com", PasswordHash = hasher.HashPassword("aggies"), FName = "Lou Ann", LName = "Feeley", MiddleInitial = Convert.ToChar("K"), StreetAddress = "600 S 8th Street W", City = "Houston", State = "TX", Zip = "77010", PhoneNumber = "8172556749", DOB = Convert.ToDateTime("1930-02-24 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a8);


            AppUser a9 = new AppUser { Email = "tfreeley@minntonka.ci.state.mn.us", PasswordHash = hasher.HashPassword("raiders"), FName = "Tesa", LName = "Freeley", MiddleInitial = Convert.ToChar("P"), StreetAddress = "4448 Fairview Ave.", City = "Houston", State = "TX", Zip = "77009", PhoneNumber = "8173255687", DOB = Convert.ToDateTime("1935-09-01 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a9);


            AppUser a10 = new AppUser { Email = "mgar@aool.com", PasswordHash = hasher.HashPassword("mustangs"), FName = "Margaret", LName = "Garcia", MiddleInitial = Convert.ToChar("L"), StreetAddress = "594 Longview", City = "Houston", State = "TX", Zip = "77003", PhoneNumber = "8176593544", DOB = Convert.ToDateTime("1990-07-03 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a10);


            AppUser a11 = new AppUser { Email = "chaley@thug.com", PasswordHash = hasher.HashPassword("mydog"), FName = "Charles", LName = "Haley", MiddleInitial = Convert.ToChar("E"), StreetAddress = "One Cowboy Pkwy", City = "Dallas", State = "TX", Zip = "75261", PhoneNumber = "2148475583", DOB = Convert.ToDateTime("1985-09-17 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a11);


            AppUser a12 = new AppUser { Email = "jeff@ggmail.com", PasswordHash = hasher.HashPassword("jeffh"), FName = "Jeffrey", LName = "Hampton", MiddleInitial = Convert.ToChar("T"), StreetAddress = "337 38th St.", City = "Austin", State = "TX", Zip = "78705", PhoneNumber = "5126978613", DOB = Convert.ToDateTime("1995-01-23 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a12);


            AppUser a13 = new AppUser { Email = "wjhearniii@umch.edu", PasswordHash = hasher.HashPassword("logicon"), FName = "John", LName = "Hearn", MiddleInitial = Convert.ToChar("B"), StreetAddress = "4225 North First", City = "Dallas", State = "TX", Zip = "75237", PhoneNumber = "2148965621", DOB = Convert.ToDateTime("1994-01-08 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a13);


            AppUser a14 = new AppUser { Email = "hicks43@ggmail.com", PasswordHash = hasher.HashPassword("doofus"), FName = "Anthony", LName = "Hicks", MiddleInitial = Convert.ToChar("J"), StreetAddress = "32 NE Garden Ln., Ste 910", City = "San Antonio", State = "TX", Zip = "78239", PhoneNumber = "2105788965", DOB = Convert.ToDateTime("1990-10-06 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a14);


            AppUser a15 = new AppUser { Email = "bradsingram@mall.utexas.edu", PasswordHash = hasher.HashPassword("mother"), FName = "Brad", LName = "Ingram", MiddleInitial = Convert.ToChar("S"), StreetAddress = "6548 La Posada Ct.", City = "Austin", State = "TX", Zip = "78736", PhoneNumber = "5124678821", DOB = Convert.ToDateTime("1984-04-12 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a15);


            AppUser a16 = new AppUser { Email = "mother.Ingram@aool.com", PasswordHash = hasher.HashPassword("whimsical"), FName = "Todd", LName = "Jacobs", MiddleInitial = Convert.ToChar("L"), StreetAddress = "4564 Elm St.", City = "Austin", State = "TX", Zip = "78731", PhoneNumber = "5124653365", DOB = Convert.ToDateTime("1983-04-04 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a16);


            AppUser a17 = new AppUser { Email = "victoria@aool.com", PasswordHash = hasher.HashPassword("nothing"), FName = "Victoria", LName = "Lawrence", MiddleInitial = Convert.ToChar("M"), StreetAddress = "6639 Butterfly Ln.", City = "Austin", State = "TX", Zip = "78761", PhoneNumber = "5129457399", DOB = Convert.ToDateTime("1961-02-03 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a17);


            AppUser a18 = new AppUser { Email = "lineback@flush.net", PasswordHash = hasher.HashPassword("GoodFellow"), FName = "Erik", LName = "Lineback", MiddleInitial = Convert.ToChar("W"), StreetAddress = "1300 Netherland St", City = "San Antonio", State = "TX", Zip = "78293", PhoneNumber = "2102449976", DOB = Convert.ToDateTime("1946-09-03 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a18);


            AppUser a19 = new AppUser { Email = "elowe@netscrape.net", PasswordHash = hasher.HashPassword("Elbow"), FName = "Ernest", LName = "Lowe", MiddleInitial = Convert.ToChar("S"), StreetAddress = "3201 Pine Drive", City = "San Antonio", State = "TX", Zip = "78279", PhoneNumber = "2105344627", DOB = Convert.ToDateTime("1992-02-07 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a19);


            AppUser a20 = new AppUser { Email = "luce_chuck@ggmail.com", PasswordHash = hasher.HashPassword("LuceyDucey"), FName = "Chuck", LName = "Luce", MiddleInitial = Convert.ToChar("B"), StreetAddress = "2345 Rolling Clouds", City = "San Antonio", State = "TX", Zip = "78268", PhoneNumber = "2106983548", DOB = Convert.ToDateTime("1942-10-25 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a20);


            AppUser a21 = new AppUser { Email = "mackcloud@pimpdaddy.com", PasswordHash = hasher.HashPassword("cloudyday"), FName = "Jennifer", LName = "MacLeod", MiddleInitial = Convert.ToChar("D"), StreetAddress = "2504 Far West Blvd.", City = "Austin", State = "TX", Zip = "78731", PhoneNumber = "5124748138", DOB = Convert.ToDateTime("1965-08-06 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a21);


            AppUser a22 = new AppUser { Email = "liz@ggmail.com", PasswordHash = hasher.HashPassword("emarkbark"), FName = "Elizabeth", LName = "Markham", MiddleInitial = Convert.ToChar("P"), StreetAddress = "7861 Chevy Chase", City = "Austin", State = "TX", Zip = "78732", PhoneNumber = "5124579845", DOB = Convert.ToDateTime("1959-04-13 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a22);


            AppUser a23 = new AppUser { Email = "mclarence@aool.com", PasswordHash = hasher.HashPassword("smartinmartin"), FName = "Clarence", LName = "Martin", MiddleInitial = Convert.ToChar("A"), StreetAddress = "87 Alcedo St.", City = "Houston", State = "TX", Zip = "77045", PhoneNumber = "8174955201", DOB = Convert.ToDateTime("1990-01-06 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a23);


            AppUser a24 = new AppUser { Email = "smartinmartin.Martin@aool.com", PasswordHash = hasher.HashPassword("grego"), FName = "Gregory", LName = "Martinez", MiddleInitial = Convert.ToChar("R"), StreetAddress = "8295 Sunset Blvd.", City = "Houston", State = "TX", Zip = "77030", PhoneNumber = "8178746718", DOB = Convert.ToDateTime("1987-10-09 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a24);


            AppUser a25 = new AppUser { Email = "cmiller@mapster.com", PasswordHash = hasher.HashPassword("chucky33"), FName = "Charles", LName = "Miller", MiddleInitial = Convert.ToChar("R"), StreetAddress = "8962 Main St.", City = "Houston", State = "TX", Zip = "77031", PhoneNumber = "8177458615", DOB = Convert.ToDateTime("1984-07-21 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a25);


            AppUser a26 = new AppUser { Email = "nelson.Kelly@aool.com", PasswordHash = hasher.HashPassword("orange"), FName = "Kelly", LName = "Nelson", MiddleInitial = Convert.ToChar("T"), StreetAddress = "2601 Red River", City = "Austin", State = "TX", Zip = "78703", PhoneNumber = "5122926966", DOB = Convert.ToDateTime("1956-07-04 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a26);


            AppUser a27 = new AppUser { Email = "jojoe@ggmail.com", PasswordHash = hasher.HashPassword("victorious"), FName = "Joe", LName = "Nguyen", MiddleInitial = Convert.ToChar("C"), StreetAddress = "1249 4th SW St.", City = "Dallas", State = "TX", Zip = "75238", PhoneNumber = "2143125897", DOB = Convert.ToDateTime("1963-01-29 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a27);


            AppUser a28 = new AppUser { Email = "orielly@foxnets.com", PasswordHash = hasher.HashPassword("billyboy"), FName = "Bill", LName = "O'Reilly", MiddleInitial = Convert.ToChar("T"), StreetAddress = "8800 Gringo Drive", City = "San Antonio", State = "TX", Zip = "78260", PhoneNumber = "2103450925", DOB = Convert.ToDateTime("1983-01-07 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a28);


            AppUser a29 = new AppUser { Email = "or@aool.com", PasswordHash = hasher.HashPassword("radicalone"), FName = "Anka", LName = "Radkovich", MiddleInitial = Convert.ToChar("L"), StreetAddress = "1300 Elliott Pl", City = "Dallas", State = "TX", Zip = "75260", PhoneNumber = "2142345566", DOB = Convert.ToDateTime("1980-03-31 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a29);


            AppUser a30 = new AppUser { Email = "megrhodes@freezing.co.uk", PasswordHash = hasher.HashPassword("gohorns"), FName = "Megan", LName = "Rhodes", MiddleInitial = Convert.ToChar("C"), StreetAddress = "4587 Enfield Rd.", City = "Austin", State = "TX", Zip = "78707", PhoneNumber = "5123744746", DOB = Convert.ToDateTime("1944-08-12 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a30);


            AppUser a31 = new AppUser { Email = "erynrice@aool.com", PasswordHash = hasher.HashPassword("iloveme"), FName = "Eryn", LName = "Rice", MiddleInitial = Convert.ToChar("M"), StreetAddress = "3405 Rio Grande", City = "Austin", State = "TX", Zip = "78705", PhoneNumber = "5123876657", DOB = Convert.ToDateTime("1934-08-02 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a31);


            AppUser a32 = new AppUser { Email = "jorge@hootmail.com", PasswordHash = hasher.HashPassword("greedy"), FName = "Jorge", LName = "Rodriguez", MiddleInitial = Convert.ToChar("None"), StreetAddress = "6788 Cotter Street", City = "Houston", State = "TX", Zip = "77057", PhoneNumber = "8178904374", DOB = Convert.ToDateTime("1989-08-11 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a32);


            AppUser a33 = new AppUser { Email = "ra@aoo.com", PasswordHash = hasher.HashPassword("familiar"), FName = "Allen", LName = "Rogers", MiddleInitial = Convert.ToChar("B"), StreetAddress = "4965 Oak Hill", City = "Austin", State = "TX", Zip = "78732", PhoneNumber = "5128752943", DOB = Convert.ToDateTime("1967-08-27 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a33);


            AppUser a34 = new AppUser { Email = "st-jean@home.com", PasswordHash = hasher.HashPassword("historical"), FName = "Olivier", LName = "Saint-Jean", MiddleInitial = Convert.ToChar("M"), StreetAddress = "255 Toncray Dr.", City = "San Antonio", State = "TX", Zip = "78292", PhoneNumber = "2104145678", DOB = Convert.ToDateTime("1950-07-08 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a34);


            AppUser a35 = new AppUser { Email = "ss34@ggmail.com", PasswordHash = hasher.HashPassword("guiltless"), FName = "Sarah", LName = "Saunders", MiddleInitial = Convert.ToChar("J"), StreetAddress = "332 Avenue C", City = "Austin", State = "TX", Zip = "78705", PhoneNumber = "5123497810", DOB = Convert.ToDateTime("1977-10-29 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a35);


            AppUser a36 = new AppUser { Email = "willsheff@email.com", PasswordHash = hasher.HashPassword("frequent"), FName = "William", LName = "Sewell", MiddleInitial = Convert.ToChar("T"), StreetAddress = "2365 51st St.", City = "Austin", State = "TX", Zip = "78709", PhoneNumber = "5124510084", DOB = Convert.ToDateTime("1941-04-21 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a36);


            AppUser a37 = new AppUser { Email = "sheff44@ggmail.com", PasswordHash = hasher.HashPassword("history"), FName = "Martin", LName = "Sheffield", MiddleInitial = Convert.ToChar("J"), StreetAddress = "3886 Avenue A", City = "Austin", State = "TX", Zip = "78705", PhoneNumber = "5125479167", DOB = Convert.ToDateTime("1937-11-10 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a37);


            AppUser a38 = new AppUser { Email = "johnsmith187@aool.com", PasswordHash = hasher.HashPassword("squirrel"), FName = "John", LName = "Smith", MiddleInitial = Convert.ToChar("A"), StreetAddress = "23 Hidden Forge Dr.", City = "San Antonio", State = "TX", Zip = "78280", PhoneNumber = "2108321888", DOB = Convert.ToDateTime("1954-10-26 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a38);


            AppUser a39 = new AppUser { Email = "dustroud@mail.com", PasswordHash = hasher.HashPassword("snakes"), FName = "Dustin", LName = "Stroud", MiddleInitial = Convert.ToChar("P"), StreetAddress = "1212 Rita Rd", City = "Dallas", State = "TX", Zip = "75221", PhoneNumber = "2142346667", DOB = Convert.ToDateTime("1932-09-01 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a39);


            AppUser a40 = new AppUser { Email = "ericstuart@aool.com", PasswordHash = hasher.HashPassword("loaf"), FName = "Eric", LName = "Stuart", MiddleInitial = Convert.ToChar("D"), StreetAddress = "5576 Toro Ring", City = "Austin", State = "TX", Zip = "78746", PhoneNumber = "5128178335", DOB = Convert.ToDateTime("1930-12-28 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a40);


            AppUser a41 = new AppUser { Email = "peterstump@hootmail.com", PasswordHash = hasher.HashPassword("rhythm"), FName = "Peter", LName = "Stump", MiddleInitial = Convert.ToChar("L"), StreetAddress = "1300 Kellen Circle", City = "Houston", State = "TX", Zip = "77018", PhoneNumber = "8174560903", DOB = Convert.ToDateTime("1989-08-13 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a41);


            AppUser a42 = new AppUser { Email = "tanner@ggmail.com", PasswordHash = hasher.HashPassword("kindly"), FName = "Jeremy", LName = "Tanner", MiddleInitial = Convert.ToChar("S"), StreetAddress = "4347 Almstead", City = "Houston", State = "TX", Zip = "77044", PhoneNumber = "8174590929", DOB = Convert.ToDateTime("1982-05-21 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a42);


            AppUser a43 = new AppUser { Email = "taylordjay@aool.com", PasswordHash = hasher.HashPassword("instrument"), FName = "Allison", LName = "Taylor", MiddleInitial = Convert.ToChar("R"), StreetAddress = "467 Nueces St.", City = "Austin", State = "TX", Zip = "78705", PhoneNumber = "5124748452", DOB = Convert.ToDateTime("1960-01-08 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a43);


            AppUser a44 = new AppUser { Email = "TayTaylor@aool.com", PasswordHash = hasher.HashPassword("deep"), FName = "Rachel", LName = "Taylor", MiddleInitial = Convert.ToChar("K"), StreetAddress = "345 Longview Dr.", City = "Austin", State = "TX", Zip = "78705", PhoneNumber = "5124512631", DOB = Convert.ToDateTime("1975-07-27 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a44);


            AppUser a45 = new AppUser { Email = "teefrank@hootmail.com", PasswordHash = hasher.HashPassword("rest"), FName = "Frank", LName = "Tee", MiddleInitial = Convert.ToChar("J"), StreetAddress = "5590 Lavell Dr", City = "Houston", State = "TX", Zip = "77004", PhoneNumber = "8178765543", DOB = Convert.ToDateTime("1968-04-06 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a45);


            AppUser a46 = new AppUser { Email = "tuck33@ggmail.com", PasswordHash = hasher.HashPassword("approval"), FName = "Clent", LName = "Tucker", MiddleInitial = Convert.ToChar("J"), StreetAddress = "312 Main St.", City = "Dallas", State = "TX", Zip = "75315", PhoneNumber = "2148471154", DOB = Convert.ToDateTime("1978-05-19 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a46);


            AppUser a47 = new AppUser { Email = "avelasco@yaho.com", PasswordHash = hasher.HashPassword("decorate"), FName = "Allen", LName = "Velasco", MiddleInitial = Convert.ToChar("G"), StreetAddress = "679 W. 4th", City = "Dallas", State = "TX", Zip = "75207", PhoneNumber = "2143985638", DOB = Convert.ToDateTime("1963-10-06 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a47);


            AppUser a48 = new AppUser { Email = "westj@pioneer.net", PasswordHash = hasher.HashPassword("geese"), FName = "Jake", LName = "West", MiddleInitial = Convert.ToChar("T"), StreetAddress = "RR 3287", City = "Dallas", State = "TX", Zip = "75323", PhoneNumber = "2148475244", DOB = Convert.ToDateTime("1993-10-14 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a48);


            AppUser a49 = new AppUser { Email = "louielouie@aool.com", PasswordHash = hasher.HashPassword("sturdy"), FName = "Louis", LName = "Winthorpe", MiddleInitial = Convert.ToChar("L"), StreetAddress = "2500 Padre Blvd", City = "Dallas", State = "TX", Zip = "75220", PhoneNumber = "2145650098", DOB = Convert.ToDateTime("1952-05-31 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a49);


            AppUser a50 = new AppUser { Email = "rwood@voyager.net", PasswordHash = hasher.HashPassword("decorous"), FName = "Reagan", LName = "Wood", MiddleInitial = Convert.ToChar("B"), StreetAddress = "447 Westlake Dr.", City = "Austin", State = "TX", Zip = "78746", PhoneNumber = "5124545242", DOB = Convert.ToDateTime("1992-04-24 00:00:00") };

            db.Users.AddOrUpdate(u => u.Email, a50);



        }

    }
}
