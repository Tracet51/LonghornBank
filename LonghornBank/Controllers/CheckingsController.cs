using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LonghornBank.Models;
using LonghornBank.Utility;

namespace LonghornBank.Controllers
{
    public class CheckingsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Checkings
        public ActionResult Index()
        {
            // Query the Database for the logged in user 
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;


            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }

            // Add the Customer to ViewBag to Access information 
            ViewBag.CustomerAccount = customer;

            //Select The Checking Accounts Associated with the customer 
            var CheckingAccountQuery = from ca in db.CheckingAccount
                                       where ca.Customer.Id == customer.Id
                                       select ca;
                                        

            // Create list and execute the query 
            List<Checking> CustomerChecking = CheckingAccountQuery.ToList();

            return View(CustomerChecking);
        }

        // GET: Checkings/Details/5
        // ID = checkingID
        public ActionResult Details(int? id )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checking checking = db.CheckingAccount.Find(id);
            if (checking == null)
            {
                return HttpNotFound();
            }

            // Get the List off all of the Banking Transaction For this Account 
            List<BankingTransaction> CheckingTransactions = checking.BankingTransactions.ToList();

            // Pass the List to the ViewBag
            ViewBag.CheckingTransactions = CheckingTransactions;
            ViewBag.Ranges = SearchTransactions.AmountRange();
            ViewBag.Dates = SearchTransactions.DateRanges();

            return View(checking);
        }

        public ActionResult Search(SearchViewModel TheSearch, Int32 CheckingID)
        {
            Checking checking = db.CheckingAccount.Find(CheckingID);
            List<BankingTransaction> Transactions = SearchTransactions.Search(db, TheSearch, 1 , CheckingID);

            // Add the list to the view bag
            ViewBag.CheckingTransactions = Transactions;
            ViewBag.Ranges = SearchTransactions.AmountRange();
            ViewBag.Dates = SearchTransactions.DateRanges();
            return View("Details", checking);
        }

        // GET: Checkings/Create
        public ActionResult Create()
        {

            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;


            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            if(customer.ActiveStatus == false)
            {
                return View("Frozen");
            }

            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = customer.Id;
            return View();
        }

        // POST: Checkings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CheckingID, Balance, Name")] Checking checking)
        {
            Decimal originaldeposit = checking.Balance;
            var CustomerQuery = from c in db.Users
                               where c.UserName == User.Identity.Name
                               select c;

            AppUser customer = CustomerQuery.FirstOrDefault(); 

            if (customer == null)
            {
                return HttpNotFound();
            }
            

            if (ModelState.IsValid)
            {
                // Auto incremement the number
                checking.AccountNumber = AccountNumber.AutoNumber(db);

                // change the name
                if (checking.Name == null)
                {
                    checking.Name = "Longhorn Checking";
                }

                // Associate the Customer with the checking account
                checking.Customer = customer;


                db.CheckingAccount.Add(checking);
                db.SaveChanges();

                // check to see if the deposit amount is over $5000
                ApprovedorNeedsApproval FirstDeposit;
                if (checking.Balance > 5000m)
                {
                    FirstDeposit = ApprovedorNeedsApproval.NeedsApproval;
                    //Added by Carson 5/2
                    checking.PendingBalance = checking.Balance;
                    checking.Balance = 0;
                    
                }
                else
                {
                    FirstDeposit = ApprovedorNeedsApproval.Approved;
                }
                var CheckingQuery = from ca in db.CheckingAccount
                                    where ca.AccountNumber == checking.AccountNumber
                                    select ca;

                Checking CustomerCheckingAccount = CheckingQuery.FirstOrDefault();
                List<Checking> CustomerCheckingList = new List<Checking>();
                CustomerCheckingList.Add(CustomerCheckingAccount);

                // Create a new transaction 
                BankingTransaction InitialDeposit = new BankingTransaction
                {
                    Amount = originaldeposit,
                    ApprovalStatus = FirstDeposit,
                    BankingTransactionType = BankingTranactionType.Deposit,
                    Description = "Initial Deposit to Cash Balance",
                    TransactionDate = DateTime.Today,
                    TransactionDispute = DisputeStatus.NotDisputed,
                    CheckingAccount = CustomerCheckingList
            };

            // Add the transaction to the database
            db.BankingTransaction.Add(InitialDeposit);
            db.SaveChanges();
            return RedirectToAction("Index", "Home", new { id = customer.Id});
            }
        

            return View(checking);
        }

        // GET: Checkings/Edit/5
        // id = checkingID
        public ActionResult Edit(int? id)
        {
            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;


            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            //Return frozen view if no go
            if (customer.ActiveStatus == false)
            {
                return View("Frozen");
            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checking checking = db.CheckingAccount.Find(id);
            if (checking == null)
            {
                return HttpNotFound();
            }

            return View(checking);
        }

        // POST: Checkings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CheckingID,AccountNumber,Name")] Checking checking)
        {
            if (ModelState.IsValid)
            {
                // Find the CustomerID Associated with the Account
                var CheckingCustomerQuery= from ca in db.CheckingAccount
                                            where ca.CheckingID == checking.CheckingID
                                            select ca.Customer.Id;
                                            

                // Execute the Find
                List <String> CustomerID = CheckingCustomerQuery.ToList();

                String IntCustomerID = CustomerID[0];

                db.Entry(checking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Portal", "Home");
            }
            return RedirectToAction("Index", "Checkings");
        }

        // GET: Checkings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checking checking = db.CheckingAccount.Find(id);
            if (checking == null)
            {
                return HttpNotFound();
            }
            return View(checking);
        }

        // POST: Checkings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Find the CustomerID Associated with the Account
            var CheckingCustomerQuery = from ca in db.CheckingAccount
                                        where ca.CheckingID == id
                                        select ca.Customer.Id;

            // Execute the Find
            List<String> CustomerID = CheckingCustomerQuery.ToList();

            String IntCustomerID = CustomerID[0];

            Checking checking = db.CheckingAccount.Find(id);
            db.CheckingAccount.Remove(checking);
            db.SaveChanges();
            return RedirectToAction("Portal", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
