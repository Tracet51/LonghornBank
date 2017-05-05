using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LonghornBank.Models;
using System.Diagnostics;
using LonghornBank.Utility;

namespace LonghornBank.Controllers
{

    public class BankingTransactionsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: BankingTransactions
        // Index Only Displays Checking 
        // Might Consider a Name Change 
        public ActionResult Index()
        {
            List<BankingTransaction> CustomerTransactions = db.BankingTransaction.ToList();
            
            // Add the customer to the view bag
            
            ViewBag.Ranges = SearchTransactions.AmountRange();
            ViewBag.Dates = SearchTransactions.DateRanges();
            ViewBag.ResultsCount = CustomerTransactions.Count;

            return View("Index", CustomerTransactions);
        }

        // GET: BankingTransactions/Details/5
        // id == BankingTransactionID
        public ActionResult Details(int? id, Int32 choice, Int32 AccountId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankingTransaction bankingTransaction = db.BankingTransaction.Find(id);
            if (bankingTransaction == null)
            {
                return HttpNotFound();
            }

            // Check to see the account related to the details
            // Checking Accounts
            if (choice == 1)
            {
                var QueryTrans = from t in db.BankingTransaction
                                 from c in t.CheckingAccount
                                 where c.CheckingID == AccountId && t.BankingTransactionType == bankingTransaction.BankingTransactionType && t.BankingTransactionID != bankingTransaction.BankingTransactionID
                                 orderby t.TransactionDate
                                 select t;
                List<BankingTransaction> TransactionsList = QueryTrans.ToList();

                // Check the length of the Transactions List
                Int32 Length = TransactionsList.Count;

                // Set the Range for return to max out at 5 
                if (Length > 5)
                {
                    Length = 5;
                }

                TransactionsList = TransactionsList.GetRange(0, Length);

                // Add Transactions and Id to the ViewBag
                ViewBag.OtherTransactions = TransactionsList;

                // Add the account ids to the ViewBag
                ViewBag.AccountId = AccountId;

                return View(bankingTransaction);
                                 
            }

            // Savings Accounts
            else if (choice == 2)
            {
                var QueryTrans = from t in db.BankingTransaction
                                 from c in t.SavingsAccount
                                 where c.SavingID == AccountId && t.BankingTransactionType == bankingTransaction.BankingTransactionType && t.BankingTransactionID != bankingTransaction.BankingTransactionID
                                 orderby t.TransactionDate
                                 select t;
                List<BankingTransaction> TransactionsList = QueryTrans.ToList();

                // Check the length of the Transactions List
                Int32 Length = TransactionsList.Count;

                // Set the Range for return to max out at 5 
                if (Length > 5)
                {
                    Length = 5;
                }

                TransactionsList = TransactionsList.GetRange(0, Length);

                // Add Transactions and Id to the ViewBag
                ViewBag.OtherTransactions = TransactionsList;

                // Add the account ids to the ViewBag
                ViewBag.AccountId = AccountId;

                return View(bankingTransaction);
            }

            // IRAs
            else if (choice == 3)
            {
                var QueryTrans = from t in db.BankingTransaction
                                 from c in t.IRAAccount
                                 where c.IRAID == AccountId && t.BankingTransactionType == bankingTransaction.BankingTransactionType && t.BankingTransactionID != bankingTransaction.BankingTransactionID
                                 orderby t.TransactionDate
                                 select t;
                List<BankingTransaction> TransactionsList = QueryTrans.ToList();

                // Check the length of the Transactions List
                Int32 Length = TransactionsList.Count;

                // Set the Range for return to max out at 5 
                if (Length > 5)
                {
                    Length = 5;
                }

                TransactionsList = TransactionsList.GetRange(0, Length);

                // Add Transactions and Id to the ViewBag
                ViewBag.OtherTransactions = TransactionsList;

                // Add the account ids to the ViewBag
                ViewBag.AccountId = AccountId;

                return View(bankingTransaction);
            }
            else if (choice == 4)
            {
                var QueryTrans = from t in db.BankingTransaction
                                 where t.StockAccount.StockAccountID == AccountId && t.BankingTransactionType == bankingTransaction.BankingTransactionType && t.BankingTransactionID != bankingTransaction.BankingTransactionID
                                 orderby t.TransactionDate
                                 select t;
                List<BankingTransaction> TransactionsList = QueryTrans.ToList();

                // Check the length of the Transactions List
                Int32 Length = TransactionsList.Count;

                // Set the Range for return to max out at 5 
                if (Length > 5)
                {
                    Length = 5;
                }

                TransactionsList = TransactionsList.GetRange(0, 5);

                // Add Transactions and Id to the ViewBag
                ViewBag.OtherTransactions = TransactionsList;

                // Add the account ids to the ViewBag
                ViewBag.AccountId = AccountId;

                return View(bankingTransaction);
            }

            else
            {
                return View("Error");
            }
        }

        // GET: BankingTransactions/Create/?id
        public ActionResult WithDrawal()
        {
            // Query the Database for the logged in user 
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

            if (customer == null)
            {
                return HttpNotFound();
            }

            // Get all of the accounts
            Tuple<SelectList, SelectList, SelectList, SelectList> AllAcounts = GetAllAccounts(customer.Id);

            if (AllAcounts.Item1.Count() == 1 && AllAcounts.Item2.Count() == 1 && AllAcounts.Item3.Count() == 1 && AllAcounts.Item4.Count() == 1)
            {
                return RedirectToAction("Create", "Home");
            }

            // Add the SelectList Tuple to the ViewBag
            ViewBag.AllAccounts = AllAcounts;

            return View();
        }

        // POST: BankingTransactions/Create
        // CheckingID = the checking account to bind to 
        // id = The Customer's Account ID
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WithDrawal([Bind(Include = "BankingTransactionID,TransactionDate,Amount,Description,BankingTransactionType")] BankingTransaction bankingTransaction, int CheckingID, int SavingID, int IraID, int StockAccountID)
        {

            var CustomerQuery = from c in db.Users
                                where c.UserName == User.Identity.Name
                                select c;


            // Get the Customer 
            AppUser customer = CustomerQuery.FirstOrDefault();

            string id = customer.Id;

            bankingTransaction.BankingTransactionType = BankingTranactionType.Withdrawl;

            bankingTransaction.TransactionDispute = DisputeStatus.NotDisputed;

            if (ModelState.IsValid)
            {
                // Check to if Checking Account
                if (CheckingID != 0)
                {
                    // Find the Selected Checking Account
                    Checking SelectedChecking = db.CheckingAccount.Find(CheckingID);

                    // Create a list of checking accounts and add the one seleceted 
                    List<Checking> NewCheckingAccounts = new List<Checking> { SelectedChecking };

                    bankingTransaction.CheckingAccount = NewCheckingAccounts;

                    //Subtracts Money to account if under amount required for approval
                    if (bankingTransaction.Amount <= SelectedChecking.Balance)
                    {
                        SelectedChecking.Overdrawn = false;
                        //TODO: Write error message for invalid WithDrawal

                        Decimal New_Balance = SelectedChecking.Balance - bankingTransaction.Amount;
                        SelectedChecking.Balance = New_Balance;
                    }
                    else
                    {
                        return View("WithDrawalError");
                    }

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");
                }

                // Check to see if Savings Account
                else if (SavingID != 0)
                {
                    // Find the Selected Checking Account
                    Saving SelectedSaving = db.SavingsAccount.Find(SavingID);

                    // Create a list of checking accounts and add the one seleceted 
                    List<Saving> NewSavingsAccounts = new List<Saving> { SelectedSaving };

                    bankingTransaction.SavingsAccount = NewSavingsAccounts;

                    //Subtracts Money to account if under amount required for approval
                    if (bankingTransaction.Amount <= SelectedSaving.Balance)
                    {
                        SelectedSaving.Overdrawn = false;
                        Decimal New_Balance = SelectedSaving.Balance - bankingTransaction.Amount;
                        SelectedSaving.Balance = New_Balance;
                    }

                    //Error for WithDrawaling too much
                    else
                    {
                        return View("WithDrawalError");
                    }

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");
                }

                else if (IraID != 0)
                {
                    DateTime Restrict1 = new DateTime(1952, 5, 5, 0, 0, 0);

                    // Find the Selected Checking Account
                    IRA SelectedIra = db.IRAAccount.Find(IraID);

                    // Create a list of checking accounts and add the one seleceted 
                    List<IRA> NewIraAccounts = new List<IRA> { SelectedIra };

                    bankingTransaction.IRAAccount = NewIraAccounts;

                    if (customer.DOB <= Restrict1)
                    {

                        if (bankingTransaction.Amount <= SelectedIra.Balance)
                        {
                            SelectedIra.Overdrawn = false;
                            Decimal New_Balance = SelectedIra.Balance - bankingTransaction.Amount;
                            SelectedIra.Balance = New_Balance;
                        }

                        else
                        {
                            //Returns error if the WithDrawal is more than bankingTransaction.Amount
                            return View("WithDrawalError");
                        }

                    }
                    else
                    {
                        if (bankingTransaction.Amount <= SelectedIra.Balance)
                        {
                            SelectedIra.Overdrawn = false;
                            if (bankingTransaction.Amount > 3000)
                            {
                                return View("UnqualifiedMax");
                            }

                            IRAViewModel IRAWithDrawal = new IRAViewModel
                            {
                                PayeeTransaction = bankingTransaction,
                                IRAAccounts = SelectedIra
                            };

                            TempData["WithDrawlIRAError"] = IRAWithDrawal;

                            return RedirectToAction("IRAWithDrawalError", IRAWithDrawal);
                        }

                        else
                        {
                            //Returns error if the WithDrawal is more than bankingTransaction.Amount
                            return View("WithDrawalError");
                        }
                    }

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");
                }

                // Check to see if Savings Account
                else if (StockAccountID != 0)
                {
                    // Find the Selected Checking Account
                    StockAccount SelectedStockAccount = db.StockAccount.Find(StockAccountID);

                    bankingTransaction.StockAccount = SelectedStockAccount;

                    //Subtracts Money to account if under amount required for approval
                    if (bankingTransaction.Amount <= SelectedStockAccount.CashBalance)
                    {
                        SelectedStockAccount.Overdrawn = false;
                        Decimal New_Balance = SelectedStockAccount.CashBalance - bankingTransaction.Amount;
                        SelectedStockAccount.CashBalance = New_Balance;
                    }

                    //Error for WithDrawaling too much
                    else
                    {
                        return View("WithDrawalError");
                    }

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");
                }
            }

            return RedirectToAction("WithDrawal");
        }

        // GET: BankingTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankingTransaction bankingTransaction = db.BankingTransaction.Find(id);
            if (bankingTransaction == null)
            {
                return HttpNotFound();
            }

            // Fill out the view model 
            DisputeTransaction Dispute = new DisputeTransaction
            {
                BankingTransactionID = bankingTransaction.BankingTransactionID,
                CustomerOpinion = bankingTransaction.Amount

            };

            return View(Dispute);
        }

        // POST: BankingTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BankingTransactionID,TransactionDispute, DisputeMessage, CustomerOpinion, CorrectedAmount")] DisputeTransaction Dispute)
        {
            if (ModelState.IsValid)
            {
                // Get the transations 
                BankingTransaction bankingTransaction = db.BankingTransaction.Find(Dispute.BankingTransactionID);

                // set the fields 
                bankingTransaction.CustomerOpinion = Dispute.CustomerOpinion;
                bankingTransaction.DisputeMessage = Dispute.TransactionDispute.ToString() + ": " + Dispute.DisputeMessage;
                bankingTransaction.TransactionDispute = DisputeStatus.Submitted;

                db.Entry(bankingTransaction).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("DisputeConfirmation");
            }

            return View(Dispute);
        }

        // GET: BankingTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankingTransaction bankingTransaction = db.BankingTransaction.Find(id);
            if (bankingTransaction == null)
            {
                return HttpNotFound();
            }
            return View(bankingTransaction);
        }

        // POST: BankingTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankingTransaction bankingTransaction = db.BankingTransaction.Find(id);
            db.BankingTransaction.Remove(bankingTransaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Portal/Checkings/Deposit/5
        // Where 5 is the CheckingID
        public ActionResult Deposit() //int? id
        {
            var CustomerQuery = from c in db.Users
                                where c.Email == User.Identity.Name
                                select c;

            AppUser Customer = CustomerQuery.FirstOrDefault();

            //Return frozen view if no go
            if (Customer.ActiveStatus == false)
            {
                return View("Frozen");
            }

            // Get all of the accounts
            Tuple<SelectList, SelectList, SelectList, SelectList> AllAcounts = GetAllAccounts(Customer.Id);

            if (AllAcounts.Item1.Count() == 1 && AllAcounts.Item2.Count() == 1 && AllAcounts.Item3.Count() == 1 && AllAcounts.Item4.Count() == 1)
            {
                return RedirectToAction("Create", "Home");
            }

            // Add the SelectList Tuple to the ViewBag
            ViewBag.AllAccounts = AllAcounts;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposit([Bind(Include = "BankingTransactionID,TransactionDate,Amount,Description")] BankingTransaction bankingTransaction, Int32 CheckingID, Int32 SavingID, Int32 IraID, Int32 StockAccountID)
        {
            var CustomerQuery = from c in db.Users
                                where c.Email == User.Identity.Name
                                select c;

            AppUser Customer = CustomerQuery.FirstOrDefault();

            if (CheckingID != 0)
            {
                // Find the selected Checking Account
                Checking SelectedChecking = db.CheckingAccount.Find(CheckingID);
                List<Checking> CheckingList = new List<Checking>();
                CheckingList.Add(SelectedChecking);

                // Associate the transaction with the checking account
                bankingTransaction.CheckingAccount = CheckingList;

                // Check to see if the model state if valid
                bankingTransaction.BankingTransactionType = BankingTranactionType.Deposit;

                //Sets status to default of not disputed
                bankingTransaction.TransactionDispute = DisputeStatus.NotDisputed;

                if (bankingTransaction.Amount <= 5000m)
                {
                    SelectedChecking.Overdrawn = true;
                    Decimal New_Balance = SelectedChecking.Balance + bankingTransaction.Amount;
                    SelectedChecking.Balance = New_Balance;

                    bankingTransaction.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                }
                else
                {
                    SelectedChecking.Overdrawn = false;
                    Decimal PendingBalance = SelectedChecking.PendingBalance + bankingTransaction.Amount;
                    SelectedChecking.PendingBalance = PendingBalance;

                    // make the transaction not approved 
                    bankingTransaction.ApprovalStatus = ApprovedorNeedsApproval.NeedsApproval;
                }

                // Repopulate the dropdown options
                var CheckingQuery = from c in db.CheckingAccount
                                    where c.Customer.Id == SelectedChecking.Customer.Id
                                    select c;

                List<Checking> CustomerChecking = CheckingQuery.ToList();

                // Convert into a select list 
                SelectList CheckingSelectList = new SelectList(CustomerChecking, "CheckingID", "Name");
                ViewBag.CheckingAccounts = CheckingSelectList;

                db.BankingTransaction.Add(bankingTransaction);
                db.Entry(SelectedChecking).State = EntityState.Modified;
                db.SaveChanges();

                // Add transaction type to ViewBag
                ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                // Redirect 
                return View("TransactionConfirmation");
            }

            // Check to see if Savings Account
            else if (SavingID != 0)
            {
                // Find the selected saving Account
                Saving SelectedSaving = db.SavingsAccount.Find(SavingID);
                List<Saving> SavingList = new List<Saving>();
                SavingList.Add(SelectedSaving);

                // Associate the transaction with the saving account
                bankingTransaction.SavingsAccount = SavingList;

                // Check to see if the model state if valid
                bankingTransaction.BankingTransactionType = BankingTranactionType.Deposit;

                //Sets status to default of not disputed
                bankingTransaction.TransactionDispute = DisputeStatus.NotDisputed;

                if (bankingTransaction.Amount <= 5000)
                {
                    SelectedSaving.Overdrawn = false;
                    Decimal New_Balance = SelectedSaving.Balance + bankingTransaction.Amount;
                    SelectedSaving.Balance = New_Balance;

                    // make the transaction not approved 
                    bankingTransaction.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                }
                else
                {
                    Decimal PendingBalance = SelectedSaving.PendingBalance + bankingTransaction.Amount;
                    SelectedSaving.PendingBalance = PendingBalance;

                    bankingTransaction.ApprovalStatus = ApprovedorNeedsApproval.NeedsApproval;
                }

                // Repopulate the dropdown options
                var SavingQuery = from c in db.SavingsAccount
                                  where c.Customer.Id == SelectedSaving.Customer.Id
                                  select c;

                List<Saving> CustomerSaving = SavingQuery.ToList();

                // Convert into a select list 
                SelectList SavingSelectList = new SelectList(CustomerSaving, "SavingID", "Name");
                ViewBag.SavingAccounts = SavingSelectList;

                db.BankingTransaction.Add(bankingTransaction);
                db.Entry(SelectedSaving).State = EntityState.Modified;
                db.SaveChanges();

                // Add transaction type to ViewBag
                ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                // Redirect 
                return View("TransactionConfirmation");
            }
            //Check to see if depositing into IRRA
            else if (IraID != 0)
            {
                // Find the Selected IRA Account
                IRA SelectedIra = db.IRAAccount.Find(IraID);

                // Create a list of IRA accounts and add the one seleceted 
                List<IRA> NewIraAccounts = new List<IRA> { SelectedIra };

                bankingTransaction.IRAAccount = NewIraAccounts;

                //Sets status to default of not disputed
                bankingTransaction.TransactionDispute = DisputeStatus.NotDisputed;

                if (SelectedIra.RunningTotal == 5000)
                {
                    return View("NoMoreDeposits");
                }

                //Adds money to account if under 5000
                if (bankingTransaction.Amount + SelectedIra.RunningTotal <= 5000)
                {
                    SelectedIra.Overdrawn = false;
                    SelectedIra.RunningTotal = SelectedIra.RunningTotal + bankingTransaction.Amount;
                    Decimal New_Balance = SelectedIra.Balance + bankingTransaction.Amount;
                    SelectedIra.Balance = New_Balance;
                }

                //Adds to pending balance if over 5000
                else
                {
                    return RedirectToAction("IRAError", new { Description = bankingTransaction.Description, Date = bankingTransaction.TransactionDate, Amount = bankingTransaction.Amount, IRAID = IraID, CID = CheckingID, SID = SavingID, StAID = StockAccountID, btID = bankingTransaction.BankingTransactionID, type = bankingTransaction.BankingTransactionType });
                }


                // Add to database
                db.BankingTransaction.Add(bankingTransaction);
                db.Entry(SelectedIra).State = EntityState.Modified;
                db.SaveChanges();

                // Add transaction type to ViewBag
                ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                // Redirect 
                return View("TransactionConfirmation");
            }
            else if (StockAccountID != 0)
            {
                // Get the stock account
                StockAccount CustomerStockPortfolio = db.StockAccount.Find(StockAccountID);

                // Set the status 
                bankingTransaction.TransactionDispute = DisputeStatus.NotDisputed;
                bankingTransaction.BankingTransactionType = BankingTranactionType.Deposit;
                
                // Check for Deposit over $5000
                if (bankingTransaction.Amount > 5000m)
                {
                    CustomerStockPortfolio.Overdrawn = false;
                    // Needs to be approved
                    bankingTransaction.ApprovalStatus = ApprovedorNeedsApproval.NeedsApproval;

                    // Set the pending balance
                    CustomerStockPortfolio.PendingBalance += bankingTransaction.Amount;

                }

                else
                {
                    // Does not need approval
                    bankingTransaction.ApprovalStatus = ApprovedorNeedsApproval.NeedsApproval;

                    // Set the cash balance 
                    CustomerStockPortfolio.CashBalance += bankingTransaction.Amount;
                }

                // Relate the transaction to the stock account 
                bankingTransaction.StockAccount = CustomerStockPortfolio;

                // add the information to the database
                db.BankingTransaction.Add(bankingTransaction);
                db.Entry(CustomerStockPortfolio).State = EntityState.Modified;
                

                db.SaveChanges();

                // Add transaction type to ViewBag
                ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                // Redirect 
                return View("TransactionConfirmation");
            }

            return View(bankingTransaction);
        }

        public ActionResult Transfer()
        {
            
            var CustomerQuery = from c in db.Users
                                where c.Email == User.Identity.Name
                                select c;

            AppUser Customer = CustomerQuery.FirstOrDefault();

            //Return frozen view if no go
            if (Customer.ActiveStatus == false)
            {
                return View("Frozen");
            }

            // Populate a list of Checking Accounts
            var CheckingQuery = from ca in db.CheckingAccount
                                where ca.Customer.Id == Customer.Id
                                select ca;

            // Create a list of accounts of execute
            List<Checking> CheckingAccounts = CheckingQuery.ToList();
            foreach(var account in CheckingAccounts)
            {
                account.AccountNumber = "XXXXXX" + account.AccountNumber.Substring(6);
                account.AccountDisplay = account.AccountNumber + " | " + account.Name + " | " + account.Balance;
            }
            // Create a None Options 
            Checking SelectNone = new Checking() { CheckingID = 0, AccountNumber = "XXXXXX0000", Balance = 0, Name = "None" };
            SelectNone.AccountDisplay =  "XXXXXX" + SelectNone.AccountNumber.Substring(6);
            SelectNone.AccountDisplay = SelectNone.AccountNumber + " " + SelectNone.Name + " " + SelectNone.Balance;
            CheckingAccounts.Add(SelectNone);

            // Convert the List into a select list 
            SelectList CheckingSelectList = new SelectList(CheckingAccounts.OrderBy(a => a.CheckingID), "CheckingID", "AccountDisplay");

            // Populate a list of Savings Accounts
            var SavingsQuery = from sa in db.SavingsAccount
                               where sa.Customer.Id == Customer.Id
                               select sa;

            // Create a list of accounts of execute
            List<Saving> SavingsAccounts = SavingsQuery.ToList();
            foreach (Saving account in SavingsAccounts)
            {
                account.AccountNumber = "XXXXXX" + account.AccountNumber.Substring(6);
                account.AccountDisplay = account.AccountNumber + " | " + account.Name + " | " + account.Balance;
            }

            // Create a None Options 
            Saving SelectNoneSavings = new Saving() { SavingID = 0, AccountNumber = "1000000000000", Balance = 0, Name = "None" };
            SelectNoneSavings.AccountNumber = "XXXXXX" + SelectNoneSavings.AccountNumber.Substring(6);
            SelectNoneSavings.AccountDisplay = SelectNoneSavings.AccountNumber + " " + SelectNoneSavings.Name + " " + SelectNoneSavings.Balance;
            SavingsAccounts.Add(SelectNoneSavings);

            // Convert the List into a select list 
            SelectList SavingsSelectList = new SelectList(SavingsAccounts.OrderBy(a => a.SavingID), "SavingID", "AccountDisplay");

            // Grab the IRA account
            var StockAccountQuery = from stock in db.StockAccount
                                    where stock.Customer.Id == Customer.Id
                                    select stock;

            // Get the IRA account
            List<StockAccount> StockAccounts = StockAccountQuery.ToList();

            foreach (StockAccount account in StockAccounts)
            {
                account.AccountNumber = "XXXXXX" + account.AccountNumber.Substring(6);
                account.AccountDisplay = account.AccountNumber + " | " + account.Name + " | " + account.CashBalance;
            }

            // Create a None Options 
            StockAccount SelectNoStockAccount = new StockAccount() { StockAccountID = 0, AccountNumber = "1000000000000", CashBalance = 0, Name = "None" };
            SelectNoStockAccount.AccountNumber = "XXXXXX" + SelectNoStockAccount.AccountNumber.Substring(6);
            SelectNoStockAccount.AccountDisplay = SelectNoStockAccount.AccountNumber + " " + SelectNoStockAccount.Name + " " + SelectNoStockAccount.CashBalance;
            StockAccounts.Add(SelectNoStockAccount);

            // Convert the List into a select list 
            SelectList StockSelectList = new SelectList(StockAccounts.OrderBy(a => a.StockAccountID), "StockAccountID", "AccountDisplay");

            // Grab the IRA account
            var IraQuery = from ira in db.IRAAccount
                           where ira.Customer.Id == Customer.Id
                           select ira;

            // Get the IRA account
            List<IRA> IRAAccounts = IraQuery.ToList();

            foreach (IRA account in IRAAccounts)
            {
                account.AccountNumber = "XXXXXX" + account.AccountNumber.Substring(6);
                account.AccountDisplay = account.AccountNumber + " | " + account.Name + " | " + account.Balance;
            }

            // Create a None Options 
            IRA SelectNoIRA = new IRA() { IRAID = 0, AccountNumber = "1000000000000", Balance = 0, Name = "None" };
            SelectNoIRA.AccountNumber = "XXXXXX" + SelectNoIRA.AccountNumber.Substring(6);
            SelectNoIRA.AccountDisplay = SelectNoIRA.AccountNumber + " " + SelectNoIRA.Name + " " + SelectNoIRA.Balance;
            IRAAccounts.Add(SelectNoIRA);

            // Convert the List into a select list 
            SelectList IRASelectList = new SelectList(IRAAccounts.OrderBy(a => a.IRAID), "IRAID", "AccountDisplay");

            // Add the Accounts to the Tuple of Accounts 
            Tuple<SelectList, SelectList, SelectList, SelectList> Accounts = new Tuple<SelectList, SelectList, SelectList, SelectList>(CheckingSelectList, SavingsSelectList, StockSelectList, IRASelectList);

            if (Accounts.Item1.Count() == 1 && Accounts.Item2.Count() == 1 && Accounts.Item3.Count() == 1 && Accounts.Item4.Count() == 1)
            {
                return RedirectToAction("Create", "Home");
            }

            // Add the SelectList Tuple to the ViewBag
            ViewBag.AllAccounts = Accounts;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transfer([Bind(Include = "BankingTransactionID,TransactionDate,Amount,Description")] BankingTransaction bankingTransaction, Int32 CheckingID, Int32 SavingID, Int32 IraID, Int32 StockAccountID, int CheckingIDTrans, int SavingIDTrans, int IRAIDTrans, int StockAccountIDTrans)
        {
            var CustomerQuery = from c in db.Users
                                where c.Email == User.Identity.Name
                                select c;
            AppUser Customer = CustomerQuery.FirstOrDefault();

            bankingTransaction.TransactionDispute = DisputeStatus.NotDisputed;

            bankingTransaction.BankingTransactionType = BankingTranactionType.Transfer;

            Decimal actual = bankingTransaction.Amount;

            // Transfer from account checking
            if (CheckingID != 0)
            {

                // Find the Selected Checking Account
                Checking SelectedChecking = db.CheckingAccount.Find(CheckingID);

                // Create a list of checking accounts and add the one seleceted 
                List<Checking> NewCheckingAccounts = new List<Checking> { SelectedChecking };

                // Check to see if the Transfer is to Another Checking
                if (CheckingIDTrans != 0)
                {
                    // Find the Selected Checking Account
                    Checking CheckingTrans = db.CheckingAccount.Find(CheckingIDTrans);

                    List<Checking> NewCheckingTransfer = new List<Checking> { CheckingTrans };

                    // Add the Transfer to the Checking List 
                    NewCheckingTransfer.Add(CheckingTrans);

                    // Create a new association of the acccounts
                    bankingTransaction.CheckingAccount = NewCheckingTransfer;

                    bankingTransaction.Description = "Transfer from " + SelectedChecking.AccountNumber;

                    //Take money from checking
                    if (bankingTransaction.Amount <= SelectedChecking.Balance + 50 && SelectedChecking.Balance >= 0)
                    {
                        SelectedChecking.Overdrawn = true;
                        Decimal New_Balance = SelectedChecking.Balance - bankingTransaction.Amount;
                        SelectedChecking.Balance = New_Balance;

                        Decimal New_Transfer_Balance = CheckingTrans.Balance + bankingTransaction.Amount;
                        CheckingTrans.Balance = New_Transfer_Balance;

                        if (New_Transfer_Balance < 0 && New_Transfer_Balance >= -50)
                        {
                            // Create a new banking transaction 
                            BankingTransaction Fee = new BankingTransaction();

                            // Take out the fee
                            SelectedChecking.Balance -= 30;

                            // Create the bankings transaction 
                            Fee.Description = "OverDrawnFee";
                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                            Fee.Amount = 30;
                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                            Fee.CheckingAccount = NewCheckingAccounts;

                            // Add the fee to the database
                            db.BankingTransaction.Add(Fee);

                            db.SaveChanges();

                            // Send the email 
                            String Body = SelectedChecking.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedChecking.Balance.ToString();
                            LonghornBank.Utility.Email.PasswordEmail(Customer.Email, "Overdrawn Account", Body);
                        }


                        bankingTransaction.Amount = actual;


                    }

                    else
                    {
                        return View("WithDrawalError");
                    }

                    // Create a new association of the acccounts
                    bankingTransaction.CheckingAccount = NewCheckingAccounts;
                    bankingTransaction.Description = "Transfer to " + CheckingTrans.AccountNumber;

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.Entry(SelectedChecking).State = EntityState.Modified;
                    db.Entry(CheckingTrans).State = EntityState.Modified;

                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");
                }

                // If Transfering to Savings Account
                else if (SavingIDTrans != 0)
                {
                    // Find the Selected Checking Account
                    Saving SavingsTrans = db.SavingsAccount.Find(SavingIDTrans);

                    // Create a list of checking accounts and add the one seleceted 
                    List<Saving> NewSavingsAccounts = new List<Saving> { SavingsTrans };

                    //Adds money to account
                    if (bankingTransaction.Amount <= SelectedChecking.Balance + 50 && SelectedChecking.Balance >= 0)
                    {
                        SelectedChecking.Overdrawn = true;
                        Decimal New_Balance = SavingsTrans.Balance + bankingTransaction.Amount;
                        SavingsTrans.Balance = New_Balance;

                        Decimal New_Transfer_Balance = SelectedChecking.Balance - bankingTransaction.Amount;
                        SelectedChecking.Balance = New_Transfer_Balance;


                        if (New_Transfer_Balance < 0 && New_Transfer_Balance >= -50)
                        {
                            // Create a new banking transaction 
                            BankingTransaction Fee = new BankingTransaction();

                            // Take out the fee
                            SelectedChecking.Balance -= 30;

                            // Create the bankings transaction 
                            Fee.Description = "OverDrawnFee";
                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                            Fee.Amount = 30;
                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                            Fee.CheckingAccount = NewCheckingAccounts;

                            // Add the fee to the database
                            db.BankingTransaction.Add(Fee);


                            db.SaveChanges();

                            // Send the email 
                            String Body = SelectedChecking.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedChecking.Balance.ToString();
                            LonghornBank.Utility.Email.PasswordEmail(Customer.Email, "Overdrawn Account", Body);

                        }
                        bankingTransaction.Amount = actual;

                    }
                    else
                    {
                        return View("WithDrawalError");
                    }

                    // Add the Savings Account
                    bankingTransaction.SavingsAccount = NewSavingsAccounts;

                    bankingTransaction.Description = "Transfer from " + SelectedChecking.AccountNumber;
                    db.BankingTransaction.Add(bankingTransaction);

                    // Create a new association of the acccounts
                    bankingTransaction.CheckingAccount = NewCheckingAccounts;
                    bankingTransaction.Description = "Transfer to " + SavingsTrans.AccountNumber;


                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.Entry(SelectedChecking).State = EntityState.Modified;
                    db.Entry(SavingsTrans).State = EntityState.Modified;
                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");

                }

                else if (IRAIDTrans != 0)
                {
                    // Find the Selected Checking Account
                    IRA IRATrans = db.IRAAccount.Find(IRAIDTrans);

                    // Create a list of checking accounts and add the one seleceted 
                    List<IRA> NewIRAAccounts = new List<IRA> { IRATrans };

                    // Add the Savings Account
                    bankingTransaction.IRAAccount = NewIRAAccounts;

                    if (IRATrans.RunningTotal == 5000)
                    {
                        return View("NoMoreDeposits");
                    }

                    

                    //Adds money to account
                    if (bankingTransaction.Amount <= SelectedChecking.Balance + 50 && SelectedChecking.Balance >= 0)
                    {
                        SelectedChecking.Overdrawn = true;
                        Decimal New_Balance = IRATrans.RunningTotal + bankingTransaction.Amount;

                        if (New_Balance > 5000)
                        {
                            return RedirectToAction("IRAError", new { Description = bankingTransaction.Description, Date = bankingTransaction.TransactionDate, Amount = bankingTransaction.Amount, IRAID = IRAIDTrans, CID = CheckingID, SID = SavingID, StAID = StockAccountID, btID = bankingTransaction.BankingTransactionID, type = bankingTransaction.BankingTransactionType });
                        }

                        IRATrans.Balance += bankingTransaction.Amount;

                        Decimal New_Transfer_Balance = SelectedChecking.Balance - bankingTransaction.Amount;

                        SelectedChecking.Balance = New_Transfer_Balance;

                        if (New_Transfer_Balance < 0 && New_Transfer_Balance >= -50)
                        {
                            // Create a new banking transaction 
                            BankingTransaction Fee = new BankingTransaction();

                            // Take out the fee
                            SelectedChecking.Balance -= 30;

                            // Create the bankings transaction 
                            Fee.Description = "OverDrawnFee";
                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                            Fee.Amount = 30;
                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                            Fee.CheckingAccount = NewCheckingAccounts;

                            // Add the fee to the database
                            db.BankingTransaction.Add(Fee);
                            db.SaveChanges();

                            // Send the email 
                            String Body = SelectedChecking.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedChecking.Balance.ToString();
                            LonghornBank.Utility.Email.PasswordEmail(Customer.Email, "Overdrawn Account", Body);
                        }

                        bankingTransaction.Amount = actual;

                    }
                    else
                    {
                        return View("WithDrawalError");
                    }
                    bankingTransaction.IRAAccount = NewIRAAccounts;

                    bankingTransaction.BankingTransactionType = BankingTranactionType.Transfer;

                    bankingTransaction.Description = "Transfer from " + SelectedChecking.AccountNumber;

                    db.BankingTransaction.Add(bankingTransaction);

                    // Create a new association of the acccounts
                    bankingTransaction.CheckingAccount = NewCheckingAccounts;
                    bankingTransaction.Description = "Transfer to " + IRATrans.AccountNumber;

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.Entry(SelectedChecking).State = EntityState.Modified;
                    db.Entry(IRATrans).State = EntityState.Modified;
                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");

                }

                else if (StockAccountIDTrans != 0)
                {
                    // Find the Selected Checking Account
                    StockAccount StockAccountTrans = db.StockAccount.Find(StockAccountIDTrans);

                    // Add the Savings Account
                    bankingTransaction.StockAccount = StockAccountTrans;

                    //Adds money to account
                    if (bankingTransaction.Amount <= SelectedChecking.Balance + 50 && SelectedChecking.Balance >= 0)
                    {
                        SelectedChecking.Overdrawn = true;
                        Decimal New_Balance = StockAccountTrans.CashBalance + bankingTransaction.Amount;
                        StockAccountTrans.CashBalance = New_Balance;

                        Decimal New_Transfer_Balance = SelectedChecking.Balance - bankingTransaction.Amount;
                        SelectedChecking.Balance = New_Transfer_Balance;

                        if (New_Transfer_Balance < 0 && New_Transfer_Balance >= -50)
                        {
                            // Create a new banking transaction 
                            BankingTransaction Fee = new BankingTransaction();

                            // Take out the fee
                            SelectedChecking.Balance -= 30;

                            // Create the bankings transaction 
                            Fee.Description = "OverDrawnFee";
                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                            Fee.Amount = 30;
                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                            Fee.CheckingAccount = NewCheckingAccounts;

                            // Add the fee to the database
                            db.BankingTransaction.Add(Fee);

                            db.SaveChanges();

                            // Send the email 
                            String Body = SelectedChecking.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedChecking.Balance.ToString();
                            LonghornBank.Utility.Email.PasswordEmail(Customer.Email, "Overdrawn Account", Body);
                        }
                        
                        bankingTransaction.Amount = actual;

                    }
                    else
                    {
                        return View("WithDrawalError");
                    }
                    // Add the Savings Account
                    bankingTransaction.StockAccount = StockAccountTrans;

                    bankingTransaction.BankingTransactionType = BankingTranactionType.Transfer;

                    bankingTransaction.Description = "Transfer from " + SelectedChecking.AccountNumber;

                    db.BankingTransaction.Add(bankingTransaction);


                    // Create a new association of the acccounts
                    bankingTransaction.CheckingAccount = NewCheckingAccounts;
                    bankingTransaction.Description = "Transfer to " + StockAccountTrans.AccountNumber;

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.Entry(SelectedChecking).State = EntityState.Modified;
                    db.Entry(StockAccountTrans).State = EntityState.Modified;
                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");

                }

                // Redirect 
                return RedirectToAction("Index", "BankingTransactions", new { id = Customer.Id });
            }

            // If Transfering from a savings account 
            else if (SavingID != 0)
            {
                // Find the Selected Checking Account
                Saving SelectedSavings = db.SavingsAccount.Find(SavingID);

                // Create a list of checking accounts and add the one seleceted 
                List<Saving> NewSavingsAccounts = new List<Saving> { SelectedSavings };

                // If transfering to a checking account
                if (CheckingIDTrans != 0)
                {
                    // Find the Selected Checking Account
                    Checking CheckingTrans = db.CheckingAccount.Find(CheckingIDTrans);

                    // Create a list of checking accounts and add the one seleceted 
                    List<Checking> NewCheckingAccounts = new List<Checking> { CheckingTrans };

                    // Add the Transfer to the Checking List 
                    NewCheckingAccounts.Add(CheckingTrans);

                    // Create a new association of the acccounts
                    bankingTransaction.CheckingAccount = NewCheckingAccounts;

                    bankingTransaction.Description = "Transfer from " + SelectedSavings.AccountNumber;

                    //Adds money from savings to checking account
                    if (bankingTransaction.Amount <= SelectedSavings.Balance + 50 && SelectedSavings.Balance >= 0)
                    {
                        SelectedSavings.Overdrawn = false;
                        //Adds money to transferred account
                        Decimal New_Balance = CheckingTrans.Balance + bankingTransaction.Amount;
                        CheckingTrans.Balance = New_Balance;

                        //Takes away money from account being withdrawn from 
                        Decimal New_Transfer_Balance = SelectedSavings.Balance - bankingTransaction.Amount;
                        SelectedSavings.Balance = New_Transfer_Balance;

                        if (New_Transfer_Balance < 0 && New_Transfer_Balance >= -80)
                        {
                            // Create a new banking transaction 
                            BankingTransaction Fee = new BankingTransaction();

                            // Take out the fee
                            SelectedSavings.Balance -= 30;

                            // Create the bankings transaction 
                            Fee.Description = "OverDrawnFee";
                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                            Fee.Amount = 30;
                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                            Fee.SavingsAccount = NewSavingsAccounts;

                            // Add the fee to the database
                            db.BankingTransaction.Add(Fee);
                            db.SaveChanges();

                            // Send the email 
                            String Body = SelectedSavings.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedSavings.Balance.ToString();
                            LonghornBank.Utility.Email.PasswordEmail(Customer.Email, "Overdrawn Account", Body);
                        }

                        bankingTransaction.Amount = actual;
                    }
                    else
                    {
                        return View("WithDrawalError");
                    }
                    db.BankingTransaction.Add(bankingTransaction);



                    // Add the Checking Account
                    bankingTransaction.SavingsAccount = NewSavingsAccounts;
                    bankingTransaction.Description = "Transfer to " + CheckingTrans.AccountNumber;

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.Entry(SelectedSavings).State = EntityState.Modified;
                    db.Entry(CheckingTrans).State = EntityState.Modified;
                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");
                }

                // If Transfering to a Savings Account 
                else if (SavingIDTrans != 0)
                {
                    // Find the Selected saving Account
                    Saving SavingsTrans = db.SavingsAccount.Find(SavingIDTrans);

                    // Create a list of checking accounts and add the one seleceted 
                    List<Saving> NewTransferAccounts = new List<Saving> { SelectedSavings };

                    NewTransferAccounts.Add(SavingsTrans);

                    // Associate with Savings Account 
                    bankingTransaction.SavingsAccount = NewTransferAccounts;

                    bankingTransaction.Description = "Transfer from " + SelectedSavings.AccountNumber;

                    //Adds money from savings to checking account
                    if (bankingTransaction.Amount <= SelectedSavings.Balance + 50 && SelectedSavings.Balance >= 0)
                    {
                        SelectedSavings.Overdrawn = false;
                        //Adds money to transferred account
                        Decimal New_Balance = SavingsTrans.Balance + bankingTransaction.Amount;
                        SavingsTrans.Balance = New_Balance;

                        //Takes away money from account being withdrawn from 
                        Decimal New_Transfer_Balance = SelectedSavings.Balance - bankingTransaction.Amount;
                        SelectedSavings.Balance = New_Transfer_Balance;

                        if (New_Transfer_Balance < 0 && New_Transfer_Balance >= -50)
                        {
                            // Create a new banking transaction 
                            BankingTransaction Fee = new BankingTransaction();

                            // Take out the fee
                            SelectedSavings.Balance -= 30;

                            // Create the bankings transaction 
                            Fee.Description = "OverDrawnFee";
                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                            Fee.Amount = 30;
                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                            Fee.SavingsAccount = NewSavingsAccounts;

                            // Add the fee to the database
                            db.BankingTransaction.Add(Fee);
                            db.SaveChanges();

                            // Send the email 
                            String Body = SelectedSavings.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedSavings.Balance.ToString();
                            LonghornBank.Utility.Email.PasswordEmail(Customer.Email, "Overdrawn Account", Body);
                        }

                        bankingTransaction.Amount = actual;
                    }
                    else
                    {
                        return View("WithDrawalError");
                    }
                    db.BankingTransaction.Add(bankingTransaction);

                    // Add the Checking Account
                    bankingTransaction.SavingsAccount = NewSavingsAccounts;
                    bankingTransaction.Description = "Transfer to " + SavingsTrans.AccountNumber;

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.Entry(SelectedSavings).State = EntityState.Modified;
                    db.Entry(SavingsTrans).State = EntityState.Modified;
                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");
                }

                else if (IRAIDTrans != 0)
                {
                    // Find the Selected Checking Account
                    IRA IRATrans = db.IRAAccount.Find(IRAIDTrans);

                    // Create a list of checking accounts and add the one seleceted 
                    List<IRA> NewIRAAccounts = new List<IRA> { IRATrans };

                    if (IRATrans.RunningTotal == 5000)
                    {
                        return View("NoMoreDeposits");
                    }

                    // Add the Savings Account
                    bankingTransaction.IRAAccount = NewIRAAccounts;

                    bankingTransaction.Description = "Transfer from " + SelectedSavings.AccountNumber;

                    //Adds money to account
                    if (bankingTransaction.Amount <= SelectedSavings.Balance + 50 && SelectedSavings.Balance >= 0)
                    {
                        SelectedSavings.Overdrawn = false;
                        Decimal New_Balance = IRATrans.RunningTotal + bankingTransaction.Amount;

                        if (New_Balance > 5000)
                        {
                            IRAViewModel DepositIRAError = new IRAViewModel
                            {
                                CustomerProfile = Customer,
                                SavingAccounts = SelectedSavings,
                                IRAAccounts = IRATrans,
                                PayeeTransaction = bankingTransaction
                            };
                            return RedirectToAction("IRAError", new { Description = bankingTransaction.Description, Date = bankingTransaction.TransactionDate, Amount = bankingTransaction.Amount, IRAID = IRAIDTrans, CID = CheckingID, SID = SavingID, StAID = StockAccountID, btID = bankingTransaction.BankingTransactionID, type = bankingTransaction.BankingTransactionType, TEAct = IRATrans.AccountNumber, TFAct = SelectedSavings.AccountNumber });
                        }

                        IRATrans.Balance += bankingTransaction.Amount;

                        Decimal New_Transfer_Balance = SelectedSavings.Balance - bankingTransaction.Amount;

                        SelectedSavings.Balance = New_Transfer_Balance;

                        if (New_Transfer_Balance < 0 && New_Transfer_Balance >= -50)
                        {
                            // Create a new banking transaction 
                            BankingTransaction Fee = new BankingTransaction();

                            // Take out the fee
                            SelectedSavings.Balance -= 30;

                            // Create the bankings transaction 
                            Fee.Description = "OverDrawnFee";
                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                            Fee.Amount = 30;
                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                            Fee.SavingsAccount = NewSavingsAccounts;

                            // Add the fee to the database
                            db.BankingTransaction.Add(Fee);
                            db.SaveChanges();

                            // Send the email 
                            String Body = SelectedSavings.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedSavings.Balance.ToString();
                            LonghornBank.Utility.Email.PasswordEmail(Customer.Email, "Overdrawn Account", Body);
                        }
                        
                        // Set the amount
                        bankingTransaction.Amount = actual;
                    }
                    else
                    {
                        return View("WithDrawalError");
                    }
                    db.BankingTransaction.Add(bankingTransaction);

                    // Add the Checking Account
                    bankingTransaction.SavingsAccount = NewSavingsAccounts;
                    bankingTransaction.Description = "Transfer to " + IRATrans.AccountNumber;

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.Entry(SelectedSavings).State = EntityState.Modified;
                    db.Entry(IRATrans).State = EntityState.Modified;
                    db.SaveChanges();

                    // Redirect 
                    return RedirectToAction("Index", "BankingTransactions", new { id = Customer.Id });
                }

                else if (StockAccountIDTrans != 0)
                {
                    // Find the Selected Checking Account
                    StockAccount StockAccountTrans = db.StockAccount.Find(StockAccountIDTrans);

                    // Add the Checking Account
                    bankingTransaction.StockAccount = StockAccountTrans;

                    bankingTransaction.Description = "Transfer from " + SelectedSavings.AccountNumber;

                    //Adds money to account
                    if (bankingTransaction.Amount <= SelectedSavings.Balance + 50 && SelectedSavings.Balance >= 0)
                    {
                        SelectedSavings.Overdrawn = false;
                        Decimal New_Balance = StockAccountTrans.CashBalance + bankingTransaction.Amount;
                        StockAccountTrans.CashBalance = New_Balance;

                        Decimal New_Transfer_Balance = SelectedSavings.Balance - bankingTransaction.Amount;
                        SelectedSavings.Balance = New_Transfer_Balance;

                        if (New_Transfer_Balance < 0 && New_Transfer_Balance >= -50)
                        {
                            // Create a new banking transaction 
                            BankingTransaction Fee = new BankingTransaction();

                            // Take out the fee
                            SelectedSavings.Balance -= 30;

                            // Create the bankings transaction 
                            Fee.Description = "OverDrawnFee";
                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                            Fee.Amount = 30;
                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                            Fee.SavingsAccount = NewSavingsAccounts;

                            // Add the fee to the database
                            db.BankingTransaction.Add(Fee);
                            db.SaveChanges();

                            // Send the email 
                            String Body = SelectedSavings.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedSavings.Balance.ToString();
                            LonghornBank.Utility.Email.PasswordEmail(Customer.Email, "Overdrawn Account", Body);
                        }

                        // Set the amount
                        bankingTransaction.Amount = actual;

                    }

                    else
                    {
                        return View("WithDrawalError");
                    }
                    db.BankingTransaction.Add(bankingTransaction);
                    // Add the Savings Account
                    bankingTransaction.SavingsAccount = NewSavingsAccounts;
                    bankingTransaction.Description = "Transfer to " + StockAccountTrans.AccountNumber;

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.Entry(SelectedSavings).State = EntityState.Modified;
                    db.Entry(StockAccountTrans).State = EntityState.Modified;
                    db.SaveChanges();


                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");
                }

                // Redirect 
                return RedirectToAction("Index", "BankingTransactions", new { id = Customer.Id });
            }


            else if (IraID != 0)
            {
                DateTime Restrict1 = new DateTime(1952, 5, 5, 0, 0, 0);

                // Find the Selected Checking Account
                IRA SelectedIRA = db.IRAAccount.Find(IraID);

                // Create a list of checking accounts and add the one seleceted 
                List<IRA> NewIRAAccounts = new List<IRA> { SelectedIRA };

                // If transfering to a checking account
                if (CheckingIDTrans != 0)
                {
                    // Find the Selected Checking Account
                    Checking CheckingTrans = db.CheckingAccount.Find(CheckingIDTrans);

                    // Create a list of checking accounts and add the one seleceted 
                    List<Checking> NewCheckingAccounts = new List<Checking> { CheckingTrans };

                    // Add the Transfer to the Checking List 
                    NewCheckingAccounts.Add(CheckingTrans);

                    // Create a new association of the acccounts
                    bankingTransaction.CheckingAccount = NewCheckingAccounts;

                    bankingTransaction.Description = "Transfer From " + SelectedIRA;
                    //Adds money from savings to checking account
                    if (bankingTransaction.Amount <= SelectedIRA.Balance + 50 && SelectedIRA.Balance >= 0)
                    {
                        SelectedIRA.Overdrawn = false;
                        //Adds money to transferred account
                        Decimal New_Balance = CheckingTrans.Balance + bankingTransaction.Amount;
                        CheckingTrans.Balance = New_Balance;

                        if (Customer.DOB <= Restrict1)
                        {
                            Decimal Transfer = SelectedIRA.Balance - bankingTransaction.Amount;
                            SelectedIRA.Balance = Transfer;

                            if (Transfer < 0 && Transfer >= -50)
                            {
                                // Create a new banking transaction 
                                BankingTransaction Fee = new BankingTransaction();

                                // Take out the fee
                                SelectedIRA.Balance -= 30;

                                // Create the bankings transaction 
                                Fee.Description = "OverDrawnFee";
                                Fee.BankingTransactionType = BankingTranactionType.Fee;
                                Fee.Amount = 30;
                                Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                                Fee.TransactionDate = bankingTransaction.TransactionDate;
                                Fee.TransactionDispute = DisputeStatus.NotDisputed;
                                Fee.IRAAccount = NewIRAAccounts;

                                // Add the fee to the database
                                db.BankingTransaction.Add(Fee);
                                db.SaveChanges();

                                // Send the email 
                                String Body = SelectedIRA.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedIRA.Balance.ToString();
                                LonghornBank.Utility.Email.PasswordEmail(Customer.Email, "Overdrawn Account", Body);
                            }

                            // Set the amount
                            bankingTransaction.Amount = actual;
                        }
                        else
                        {
                            if (bankingTransaction.Amount <= SelectedIRA.Balance + 50)
                            {
                                if (bankingTransaction.Amount > 3000)
                                {
                                    return View("UnqualifiedMax");
                                }

                                IRAViewModel IRAWithDrawal = new IRAViewModel
                                {
                                    PayeeTransaction = bankingTransaction,
                                    IRAAccounts = SelectedIRA,
                                    CheckingAccounts = CheckingTrans
                                };
                                return RedirectToAction("IRAWithDrawalError", new { Description = bankingTransaction.Description, Date = bankingTransaction.TransactionDate, Amount = bankingTransaction.Amount, IRAID = IraID, CID = CheckingIDTrans, SID = SavingIDTrans, StAID = StockAccountIDTrans, btID = bankingTransaction.BankingTransactionID, type = bankingTransaction.BankingTransactionType });
                            }
                        }
                    }

                    else
                    {
                        return View("WithDrawalError");
                    }
                    db.BankingTransaction.Add(bankingTransaction);

                    // Associate with Savings Account 
                    bankingTransaction.IRAAccount = NewIRAAccounts;
                    bankingTransaction.Description = "Transfer to " + CheckingTrans.AccountNumber;

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.Entry(SelectedIRA).State = EntityState.Modified;
                    db.Entry(CheckingTrans).State = EntityState.Modified;
                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");


                }

                // If transfering to a checking account
                if (SavingIDTrans != 0)
                {
                    // Find the Selected Checking Account
                    Saving SavingTrans = db.SavingsAccount.Find(SavingIDTrans);

                    // Create a list of checking accounts and add the one seleceted 
                    List<Saving> NewSavingAccounts = new List<Saving> { SavingTrans };

                    // Add the Transfer to the Checking List 
                    NewSavingAccounts.Add(SavingTrans);

                    // Create a new association of the acccounts
                    bankingTransaction.SavingsAccount = NewSavingAccounts;

                    bankingTransaction.Description = "Transfer From " + SelectedIRA;

                    //Adds money from savings to checking account
                    if (bankingTransaction.Amount <= SelectedIRA.Balance + 50 && SelectedIRA.Balance >= 0)
                    {
                        SelectedIRA.Overdrawn = false;
                        //Adds money to transferred account
                        Decimal New_Balance = SavingTrans.Balance + bankingTransaction.Amount;
                        SavingTrans.Balance = New_Balance;

                        if (Customer.DOB <= Restrict1)
                        {
                            if (bankingTransaction.Amount <= SelectedIRA.Balance + 50 && SelectedIRA.Balance >= 0)
                            {
                                Decimal Transfer = SelectedIRA.Balance - bankingTransaction.Amount;
                                SelectedIRA.Balance = Transfer;

                                if ( Transfer < 0 && Transfer >= -50)
                                {
                                    // Create a new banking transaction 
                                    BankingTransaction Fee = new BankingTransaction();

                                    // Take out the fee
                                    SelectedIRA.Balance -= 30;

                                    // Create the bankings transaction 
                                    Fee.Description = "OverDrawnFee";
                                    Fee.BankingTransactionType = BankingTranactionType.Fee;
                                    Fee.Amount = 30;
                                    Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                                    Fee.TransactionDate = bankingTransaction.TransactionDate;
                                    Fee.TransactionDispute = DisputeStatus.NotDisputed;
                                    Fee.IRAAccount = NewIRAAccounts;

                                    // Add the fee to the database
                                    db.BankingTransaction.Add(Fee);
                                    db.SaveChanges();

                                    // Send the email 
                                    String Body = SelectedIRA.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedIRA.Balance.ToString();
                                    LonghornBank.Utility.Email.PasswordEmail(Customer.Email, "Overdrawn Account", Body);
                                }

                                // Set the amount
                                bankingTransaction.Amount = actual;

                            }
                            else
                            {
                                //Returns error if the WithDrawal is more than bankingTransaction.Amount
                                return View("IRAError");
                            }

                        }
                        else
                        {
                            if (bankingTransaction.Amount <= SelectedIRA.Balance + 50 && SelectedIRA.Balance >= 0)
                            {

                                if (bankingTransaction.Amount > 3000)
                                {
                                    return View("UnqualifiedMax");
                                }

                                IRAViewModel IRAWithDrawal = new IRAViewModel
                                {
                                    PayeeTransaction = bankingTransaction,
                                    IRAAccounts = SelectedIRA,
                                    SavingAccounts = SavingTrans
                                };

                                return RedirectToAction("IRAWithDrawalError", new { Description = bankingTransaction.Description, Date = bankingTransaction.TransactionDate, Amount = bankingTransaction.Amount, IRAID = IraID, CID = CheckingIDTrans, SID = SavingIDTrans, StAID = StockAccountIDTrans, btID = bankingTransaction.BankingTransactionID, type = bankingTransaction.BankingTransactionType });
                            }
                        }
                    }

                    else
                    {
                        return View("WithDrawalError");
                    }
                    db.BankingTransaction.Add(bankingTransaction);

                    bankingTransaction.IRAAccount = NewIRAAccounts;
                    bankingTransaction.Description = "Transfer to " + SavingTrans.AccountNumber;

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.Entry(SelectedIRA).State = EntityState.Modified;
                    db.Entry(SavingTrans).State = EntityState.Modified;
                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");


                }

                if (StockAccountIDTrans != 0)
                {
                    // Find the Selected Checking Account
                    StockAccount StockAccountTrans = db.StockAccount.Find(StockAccountIDTrans);

                    // Create a new association of the acccounts
                    bankingTransaction.StockAccount = StockAccountTrans;

                    bankingTransaction.Description = "Transfer From " + SelectedIRA;

                    //Adds money from savings to checking account
                    if (bankingTransaction.Amount <= SelectedIRA.Balance + 50 && SelectedIRA.Balance >= 0)
                    {
                        SelectedIRA.Overdrawn = false;
                        //Adds money to transferred account
                        Decimal New_Balance = StockAccountTrans.CashBalance + bankingTransaction.Amount;
                        StockAccountTrans.CashBalance = New_Balance;

                        if (Customer.DOB <= Restrict1)
                        {
                            if (bankingTransaction.Amount <= SelectedIRA.Balance + 50 && SelectedIRA.Balance >= 0)
                            {
                                Decimal Transfer = SelectedIRA.Balance - bankingTransaction.Amount;
                                SelectedIRA.Balance = Transfer;

                                if (Transfer < 0 && SelectedIRA.Balance - Transfer >= -50)
                                {
                                    // Create a new banking transaction 
                                    BankingTransaction Fee = new BankingTransaction();

                                    // Take out the fee
                                    SelectedIRA.Balance -= 30;

                                    // Create the bankings transaction 
                                    Fee.Description = "OverDrawnFee";
                                    Fee.BankingTransactionType = BankingTranactionType.Fee;
                                    Fee.Amount = 30;
                                    Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                                    Fee.TransactionDate = bankingTransaction.TransactionDate;
                                    Fee.TransactionDispute = DisputeStatus.NotDisputed;
                                    Fee.IRAAccount = NewIRAAccounts;

                                    // Add the fee to the database
                                    db.BankingTransaction.Add(Fee);
                                    db.SaveChanges();

                                    // Send the email 
                                    String Body = SelectedIRA.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedIRA.Balance.ToString();
                                    LonghornBank.Utility.Email.PasswordEmail(Customer.Email, "Overdrawn Account", Body);
                                }

                                // Set the amount
                                bankingTransaction.Amount = actual;

                            }
                            else
                            {
                                //Returns error if the WithDrawal is more than bankingTransaction.Amount
                                return View("IRAError", new { Description = bankingTransaction.Description, Date = bankingTransaction.TransactionDate, Amount = bankingTransaction.Amount, IRAID = IraID, CID = CheckingIDTrans, SID = SavingIDTrans, StAID = StockAccountIDTrans, btID = bankingTransaction.BankingTransactionID, type = bankingTransaction.BankingTransactionType } );
                            }

                        }
                        else
                        {
                            if (bankingTransaction.Amount <= SelectedIRA.Balance + 50 && SelectedIRA.Balance >= 0)
                            {
                                if (bankingTransaction.Amount > 3000)
                                {
                                    return View("UnqualifiedMax");
                                }

                                IRAViewModel IRAWithDrawal = new IRAViewModel
                                {
                                    PayeeTransaction = bankingTransaction,
                                    IRAAccounts = SelectedIRA,
                                    StockAccounts = StockAccountTrans
                                };
                                return RedirectToAction("IRAWithDrawalError", new { Description = bankingTransaction.Description, Date = bankingTransaction.TransactionDate, Amount = bankingTransaction.Amount, IRAID = IraID, CID = CheckingIDTrans, SID = SavingIDTrans, StAID = StockAccountIDTrans, btID = bankingTransaction.BankingTransactionID, type = bankingTransaction.BankingTransactionType });
                            }
                        }
                    }

                    else
                    {
                        return View("WithDrawalError");
                    }
                    db.BankingTransaction.Add(bankingTransaction);

                    bankingTransaction.IRAAccount = NewIRAAccounts;
                    bankingTransaction.Description = "Transfer to " + StockAccountTrans.AccountNumber;

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.Entry(SelectedIRA).State = EntityState.Modified;
                    db.Entry(StockAccountTrans).State = EntityState.Modified;
                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");


                }

                // Redirect 
                return RedirectToAction("Index", "BankingTransactions", new { id = Customer.Id });

            }

            // If Transfering from a savings account 
            else if (StockAccountID != 0)
            {
                // Find the Selected Checking Account
                StockAccount SelectedStockAccount = db.StockAccount.Find(StockAccountID);


                // If transfering to a checking account
                if (CheckingIDTrans != 0)
                {
                    // Find the Selected Checking Account
                    Checking CheckingTrans = db.CheckingAccount.Find(CheckingIDTrans);

                    // Create a list of checking accounts and add the one seleceted 
                    List<Checking> NewCheckingAccounts = new List<Checking> { CheckingTrans };

                    // Add the Transfer to the Checking List 
                    NewCheckingAccounts.Add(CheckingTrans);

                    // Create a new association of the acccounts
                    bankingTransaction.CheckingAccount = NewCheckingAccounts;

                    bankingTransaction.Description = "Transfer From " + SelectedStockAccount;

                    //Adds money from savings to checking account
                    if (bankingTransaction.Amount <= SelectedStockAccount.CashBalance + 50m && SelectedStockAccount.CashBalance >= 0)
                    {
                        SelectedStockAccount.Overdrawn = false;
                        Decimal New_Balance = CheckingTrans.Balance + bankingTransaction.Amount;
                        CheckingTrans.Balance = New_Balance;

                        //Takes away money from account being withdrawn from 
                        Decimal New_Transfer_Balance = SelectedStockAccount.CashBalance - bankingTransaction.Amount;
                        SelectedStockAccount.CashBalance = New_Transfer_Balance;

                        if (New_Transfer_Balance < 0 && New_Transfer_Balance >= -50)
                        {
                            // Create a new banking transaction 
                            BankingTransaction Fee = new BankingTransaction();

                            // Take out the fee
                            SelectedStockAccount.CashBalance -= 30;

                            // Create the bankings transaction 
                            Fee.Description = "OverDrawnFee";
                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                            Fee.Amount = 30;
                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                            Fee.StockAccount = SelectedStockAccount;

                            // Add the fee to the database
                            db.BankingTransaction.Add(Fee);
                            db.SaveChanges();

                            // Send the email 
                            String Body = SelectedStockAccount.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedStockAccount.CashBalance.ToString();
                            LonghornBank.Utility.Email.PasswordEmail(Customer.Email, "Overdrawn Account", Body);
                        }

                        // Set the amount
                        bankingTransaction.Amount = actual;
                    }
                    else
                    {
                        return View("WithDrawalError");
                    }
                    db.BankingTransaction.Add(bankingTransaction);

                    // Associate with Savings Account 
                    bankingTransaction.StockAccount = SelectedStockAccount;
                    bankingTransaction.Description = "Transfer to " + CheckingTrans.AccountNumber;

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.Entry(SelectedStockAccount).State = EntityState.Modified;
                    db.Entry(CheckingTrans).State = EntityState.Modified;
                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");
                }

                // If Transfering to a Savings Account 
                else if (SavingIDTrans != 0)
                {
                    SelectedStockAccount.Overdrawn = false;
                    // Find the Selected saving Account
                    Saving SavingsTrans = db.SavingsAccount.Find(SavingIDTrans);

                    // Create a list of checking accounts and add the one seleceted 
                    List<Saving> NewSavingAccounts = new List<Saving> { SavingsTrans };

                    // Add to the Savings Account List
                    NewSavingAccounts.Add(SavingsTrans);

                    // Associate with Savings Account 
                    bankingTransaction.SavingsAccount = NewSavingAccounts;

                    bankingTransaction.Description = "Transfer From " + SelectedStockAccount;

                    //Adds money from savings to checking account
                    if (bankingTransaction.Amount <= SelectedStockAccount.CashBalance + 50 && SelectedStockAccount.CashBalance >= 0)
                    {
                        //Adds money to transferred account
                        Decimal New_Balance = SavingsTrans.Balance + bankingTransaction.Amount;
                        SavingsTrans.Balance = New_Balance;

                        //Takes away money from account being withdrawn from 
                        Decimal New_Transfer_Balance = SelectedStockAccount.CashBalance - bankingTransaction.Amount;
                        SelectedStockAccount.CashBalance = New_Transfer_Balance;


                        if (New_Transfer_Balance < 0 && New_Transfer_Balance >= -50)
                        {
                            // Create a new banking transaction 
                            BankingTransaction Fee = new BankingTransaction();

                            // Take out the fee
                            SelectedStockAccount.CashBalance -= 30;

                            // Create the bankings transaction 
                            Fee.Description = "OverDrawnFee";
                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                            Fee.Amount = 30;
                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                            Fee.StockAccount = SelectedStockAccount;

                            // Add the fee to the database
                            db.BankingTransaction.Add(Fee);
                            db.SaveChanges();

                            // Send the email 
                            String Body = SelectedStockAccount.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedStockAccount.CashBalance.ToString();
                            LonghornBank.Utility.Email.PasswordEmail(Customer.Email, "Overdrawn Account", Body);
                        }

                        // Set the amount
                        bankingTransaction.Amount = actual;
                    }
                    else
                    {
                        return View("WithDrawalError");
                    }
                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);

                    // Associate with Savings Account 
                    bankingTransaction.StockAccount = SelectedStockAccount;
                    bankingTransaction.Description = "Transfer to " + SavingsTrans.AccountNumber;

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.Entry(SelectedStockAccount).State = EntityState.Modified;
                    db.Entry(SavingsTrans).State = EntityState.Modified;
                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");
                }

                else if (IRAIDTrans != 0)
                {
                    SelectedStockAccount.Overdrawn = false;
                    // Find the Selected Checking Account
                    IRA IRATrans = db.IRAAccount.Find(IRAIDTrans);

                    // Create a list of checking accounts and add the one seleceted 
                    List<IRA> NewIRAAccounts = new List<IRA> { IRATrans };

                    if (IRATrans.RunningTotal == 5000)
                    {
                        return View("NoMoreDeposits");
                    }

                    // Add the Savings Account
                    bankingTransaction.IRAAccount = NewIRAAccounts;

                    bankingTransaction.Description = "Transfer From " + SelectedStockAccount;

                    //Adds money to account
                    if (bankingTransaction.Amount <= SelectedStockAccount.CashBalance + 50 && SelectedStockAccount.CashBalance >= 0)
                    {
                        decimal New = bankingTransaction.Amount;
                        Decimal New_Balance = IRATrans.RunningTotal + bankingTransaction.Amount;

                        if (New_Balance > 5000)
                        {
                            IRAViewModel DepositIRAError = new IRAViewModel
                            {
                                CustomerProfile = Customer,
                                StockAccounts = SelectedStockAccount,
                                IRAAccounts = IRATrans,
                                PayeeTransaction = bankingTransaction
                            };

                            return RedirectToAction("IRAError", new { Description = bankingTransaction.Description, Date = bankingTransaction.TransactionDate, Amount = bankingTransaction.Amount, IRAID = IRAIDTrans, CID = CheckingID, SID = SavingID, StAID = StockAccountID, btID = bankingTransaction.BankingTransactionID, type = bankingTransaction.BankingTransactionType });
                        }

                        IRATrans.Balance += New;

                        Decimal New_Transfer_Balance = SelectedStockAccount.CashBalance - bankingTransaction.Amount;

                        SelectedStockAccount.CashBalance = New_Transfer_Balance;
                        if (New_Transfer_Balance < 0 && New_Transfer_Balance >= -50)
                        {
                            // Create a new banking transaction 
                            BankingTransaction Fee = new BankingTransaction();

                            // Take out the fee
                            SelectedStockAccount.CashBalance -= 30;

                            // Create the bankings transaction 
                            Fee.Description = "OverDrawnFee";
                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                            Fee.Amount = 30;
                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                            Fee.StockAccount = SelectedStockAccount;

                            // Add the fee to the database
                            db.BankingTransaction.Add(Fee);
                            db.SaveChanges();

                            // Send the email 
                            String Body = SelectedStockAccount.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedStockAccount.CashBalance.ToString();
                            LonghornBank.Utility.Email.PasswordEmail(Customer.Email, "Overdrawn Account", Body);
                        }

                        // Set the amount
                        bankingTransaction.Amount = actual;

                    }
                    else
                    {
                        return View("WithDrawalError");
                    }
                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);

                    // Associate with Savings Account 
                    bankingTransaction.StockAccount = SelectedStockAccount;
                    bankingTransaction.Description = "Transfer to " + IRATrans.AccountNumber;

                    // Add to database
                    db.BankingTransaction.Add(bankingTransaction);
                    db.Entry(SelectedStockAccount).State = EntityState.Modified;
                    db.Entry(IRATrans).State = EntityState.Modified;
                    db.SaveChanges();

                    // Add transaction type to ViewBag
                    ViewBag.TransactionType = bankingTransaction.BankingTransactionType.ToString();

                    // Redirect 
                    return View("TransactionConfirmation");
                }
            }
            return View(bankingTransaction);
        }


        // Get all of the customer's account 
        // id == customer's id
        public Tuple<SelectList, SelectList, SelectList, SelectList> GetAllAccounts(string id)
        {
            // Populate a list of Checking Accounts
            var CheckingQuery = from ca in db.CheckingAccount
                                where ca.Customer.Id == id
                                select ca;

            // Create a list of accounts of execute
            List<Checking> CheckingAccounts = CheckingQuery.ToList();

            // Create a None Options 
            Checking SelectNone = new Checking() { CheckingID = 0, AccountNumber = "1000000000000", Balance = 0, Name = "None" };
            CheckingAccounts.Add(SelectNone);

            foreach (Checking account in CheckingAccounts)
            {
                account.AccountNumber = "XXXXXX" + account.AccountNumber.Substring(6);
            }

            // Convert the List into a select list 
            SelectList CheckingSelectList = new SelectList(CheckingAccounts.OrderBy(a => a.CheckingID), "CheckingID", "Name");

            // Populate a list of Savings Accounts
            var SavingsQuery = from sa in db.SavingsAccount
                               where sa.Customer.Id == id
                               select sa;

            // Create a list of accounts of execute
            List<Saving> SavingsAccounts = SavingsQuery.ToList();

            // Create a None Options 
            Saving SelectNoneSavings = new Saving() { SavingID = 0, AccountNumber = "1000000000000", Balance = 0, Name = "None" };
            SavingsAccounts.Add(SelectNoneSavings);

            // Convert the List into a select list 
            SelectList SavingsSelectList = new SelectList(SavingsAccounts.OrderBy(a => a.SavingID), "SavingID", "Name");

            // Grab the IRA account
            var StockAccountQuery = from stock in db.StockAccount
                                    where stock.Customer.Id == id
                                    select stock;

            // Get the IRA account
            List<StockAccount> StockAccounts = StockAccountQuery.ToList();

            // Create a None Options 
            StockAccount SelectNoStockAccount = new StockAccount() { StockAccountID = 0, AccountNumber = "1000000000000", CashBalance = 0, Name = "None" };
            StockAccounts.Add(SelectNoStockAccount);

            // Convert the List into a select list 
            SelectList StockSelectList = new SelectList(StockAccounts.OrderBy(a => a.StockAccountID), "StockAccountID", "Name");

            // Grab the IRA account
            var IraQuery = from ira in db.IRAAccount
                           where ira.Customer.Id == id
                           select ira;

            // Get the IRA account
            List<IRA> IRAAccounts = IraQuery.ToList();

            // Create a None Options 
            IRA SelectNoIRA = new IRA() { IRAID = 0, AccountNumber = "1000000000000", Balance = 0, Name = "None" };
            IRAAccounts.Add(SelectNoIRA);

            // Convert the List into a select list 
            SelectList IRASelectList = new SelectList(IRAAccounts.OrderBy(a => a.IRAID), "IRAID", "Name");

            // Add the Accounts to the Tuple of Accounts 
            Tuple<SelectList, SelectList, SelectList, SelectList> Accounts = new Tuple<SelectList, SelectList, SelectList, SelectList>(CheckingSelectList, SavingsSelectList, StockSelectList, IRASelectList);

            return Accounts;

        }

        public ActionResult IRAError(string Description, DateTime Date, Decimal Amount, int IRAID, int CID, int SID, int StAID, int btID, BankingTranactionType type)
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
            // Find the Selected Checking Account
            Checking SelectedChecking = db.CheckingAccount.Find(CID);

            Saving SelectedSaving = db.SavingsAccount.Find(SID);

            StockAccount SelectedStockAccount = db.StockAccount.Find(StAID);

            IRA SelectedIRA = db.IRAAccount.Find(IRAID);

            BankingTransaction bankingtransaction = new BankingTransaction();

            bankingtransaction.TransactionDate = Date;

            bankingtransaction.Amount = Amount;

            bankingtransaction.Description = Description;

            bankingtransaction.BankingTransactionType = type;

            IRAViewModel ErrorAction = new IRAViewModel
            {
                CheckingAccounts = SelectedChecking,
                SavingAccounts = SelectedSaving,
                StockAccounts = SelectedStockAccount,
                IRAAccounts = SelectedIRA,
                PayeeTransaction = bankingtransaction,
                CustomerProfile = customer
            };


            return View(ErrorAction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IRAError(string submit, IRAViewModel ErrorActionIRA)
        {

           

            if (ErrorActionIRA.CustomerProfile.Id == null)
            {
                return RedirectToAction("Portal", "Home");
            }

            BankingTransaction bankingTransaction = new BankingTransaction();
            bankingTransaction.TransactionDispute = DisputeStatus.NotDisputed;
            bankingTransaction.Description = ErrorActionIRA.PayeeTransaction.Description;
            bankingTransaction.TransactionDate = ErrorActionIRA.PayeeTransaction.TransactionDate;
            bankingTransaction.BankingTransactionID = ErrorActionIRA.PayeeTransaction.BankingTransactionID;
            bankingTransaction.BankingTransactionType = ErrorActionIRA.PayeeTransaction.BankingTransactionType;
            bankingTransaction.Amount = ErrorActionIRA.PayeeTransaction.Amount;

            Decimal actual = bankingTransaction.Amount;
            // Check to see if Deposit 
            if (bankingTransaction.BankingTransactionType == BankingTranactionType.Deposit)
            {
                if (ErrorActionIRA.IRAAccounts.IRAID != 0)
                {
                    // Find the Selected Checking Account
                    IRA SelectedIra = db.IRAAccount.Find(ErrorActionIRA.IRAAccounts.IRAID);

                    // Create a list of checking accounts and add the one seleceted 
                    List<IRA> NewIraAccounts = new List<IRA> { SelectedIra };

                    bankingTransaction.IRAAccount = NewIraAccounts;

                    switch (submit)
                    {
                        case "Automatic":
                            Decimal New_Balance = SelectedIra.Balance + (5000 - SelectedIra.RunningTotal);
                            bankingTransaction.Amount = New_Balance;
                            SelectedIra.RunningTotal = New_Balance;
                            SelectedIra.Balance = 0 + New_Balance;
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            break;

                        case "User":
                            if (bankingTransaction.Amount + SelectedIra.RunningTotal <= 5000)
                            {
                                SelectedIra.RunningTotal = bankingTransaction.Amount + SelectedIra.RunningTotal;
                                Decimal New_Balance2 = SelectedIra.Balance + bankingTransaction.Amount;
                                SelectedIra.Balance = New_Balance2;
                                db.BankingTransaction.Add(bankingTransaction);
                                db.SaveChanges();
                            }

                            else
                            {
                                return RedirectToAction("IRAError", "BankingTransactions", new { Description = ErrorActionIRA.PayeeTransaction.Description,  Date = ErrorActionIRA.PayeeTransaction.TransactionDate, Amount = ErrorActionIRA.PayeeTransaction.Amount, IRAID = ErrorActionIRA.IRAAccounts.IRAID, CID = ErrorActionIRA.CheckingAccounts.CheckingID, SID = ErrorActionIRA.SavingAccounts.SavingID, StAID = ErrorActionIRA.StockAccounts.StockAccountID, btID = ErrorActionIRA.PayeeTransaction.BankingTransactionID, type =ErrorActionIRA.PayeeTransaction.BankingTransactionType });
                            }
                            break;
                    }
                }

            }

            if (bankingTransaction.BankingTransactionType == BankingTranactionType.Transfer)
            {
                if (ErrorActionIRA.IRAAccounts.IRAID != 0)
                {
                    // Find the Selected Checking Account
                    IRA SelectedIra = db.IRAAccount.Find(ErrorActionIRA.IRAAccounts.IRAID);

                    String IRAact = SelectedIra.AccountNumber;

                    // Create a list of checking accounts and add the one seleceted 
                    List<IRA> NewIraAccounts = new List<IRA> { SelectedIra };

                    switch (submit)
                    {
                        case "Automatic":

                            Decimal New_Balance = SelectedIra.Balance + (5000 - ErrorActionIRA.IRAAccounts.RunningTotal);

                            bankingTransaction.Amount = New_Balance;

                            if (ErrorActionIRA.SavingAccounts.SavingID != 0)
                            {
                                // Find the Selected Checking Account
                                Saving SelectedSaving = db.SavingsAccount.Find(ErrorActionIRA.SavingAccounts.SavingID);

                                String SaAct = SelectedSaving.AccountNumber;

                                // Create a list of checking accounts and add the one seleceted 
                                List<Saving> NewSavingAccounts = new List<Saving> { SelectedSaving };

                                bankingTransaction.SavingsAccount = NewSavingAccounts;

                                Decimal New = (5000 - SelectedIra.RunningTotal);

                                if (SelectedSaving.Balance - bankingTransaction.Amount <= SelectedSaving.Balance)
                                {
                                    SelectedSaving.Overdrawn = false;
                                    SelectedSaving.Balance -= New;
                                    if (bankingTransaction.Amount <= SelectedSaving.Balance + 50 && SelectedSaving.Balance >= 0)
                                    {

                                        if (SelectedSaving.Balance - bankingTransaction.Amount < 0 && SelectedSaving.Balance - bankingTransaction.Amount >= -50)
                                        {
                                            // Create a new banking transaction 
                                            BankingTransaction Fee = new BankingTransaction();

                                            // Take out the fee
                                            SelectedSaving.Balance -= 30;

                                            // Create the bankings transaction 
                                            Fee.Description = "OverDrawnFee";
                                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                                            Fee.Amount = 30;
                                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                                            Fee.SavingsAccount = NewSavingAccounts;

                                            // Add the fee to the database
                                            db.BankingTransaction.Add(Fee);
                                            db.SaveChanges();

                                            // Send the email 
                                            String Body = SelectedSaving.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedSaving.Balance.ToString();
                                            LonghornBank.Utility.Email.PasswordEmail(ErrorActionIRA.CustomerProfile.Email, "Overdrawn Account", Body);

                                        }
                                    }
                                }
                                else
                                {
                                    return View("WithDrawalError");
                                }
                                SelectedIra.RunningTotal = 5000;
                                SelectedIra.Balance += New;
                                bankingTransaction.Amount = New;
                                bankingTransaction.Description = "Transfer to " + IRAact;
                                bankingTransaction.BankingTransactionType = BankingTranactionType.Transfer;

                                BankingTransaction IRA = new BankingTransaction();
                                IRA.Amount = (5000 - SelectedIra.RunningTotal);
                                IRA.IRAAccount = NewIraAccounts;
                                IRA.Description = "Transfer from " + SaAct;
                                IRA.TransactionDate = bankingTransaction.TransactionDate;
                                IRA.BankingTransactionType = BankingTranactionType.Transfer;
                                db.BankingTransaction.Add(IRA);
                                db.SaveChanges();

                            }

                            else if (ErrorActionIRA.CheckingAccounts.CheckingID != 0)
                            {
                                Checking SelectedChecking = db.CheckingAccount.Find(ErrorActionIRA.CheckingAccounts.CheckingID);

                                // Create a list of checking accounts and add the one seleceted 
                                List<Checking> NewCheckingAccounts = new List<Checking> { SelectedChecking };

                                String Cact = SelectedChecking.AccountNumber;

                                bankingTransaction.CheckingAccount = NewCheckingAccounts;

                                Decimal New = (5000 - SelectedIra.RunningTotal);
                                if (SelectedChecking.Balance - bankingTransaction.Amount <= SelectedChecking.Balance)
                                {
                                    SelectedChecking.Overdrawn = false;
                                    SelectedChecking.Balance -= New;
                                    if (bankingTransaction.Amount <= SelectedChecking.Balance + 50 && SelectedChecking.Balance >= 0)
                                    {

                                        if (SelectedChecking.Balance - bankingTransaction.Amount < 0 && SelectedChecking.Balance - bankingTransaction.Amount >= -50)
                                        {
                                            // Create a new banking transaction 
                                            BankingTransaction Fee = new BankingTransaction();

                                            // Take out the fee
                                            SelectedChecking.Balance -= 30;

                                            // Create the bankings transaction 
                                            Fee.Description = "OverDrawnFee";
                                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                                            Fee.Amount = 30;
                                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                                            Fee.CheckingAccount = NewCheckingAccounts;

                                            // Add the fee to the database
                                            db.BankingTransaction.Add(Fee);
                                            db.SaveChanges();

                                            // Send the email 
                                            String Body = SelectedChecking.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedChecking.Balance.ToString();
                                            LonghornBank.Utility.Email.PasswordEmail(ErrorActionIRA.CustomerProfile.Email, "Overdrawn Account", Body);
                                        }
                                    }
                                }
                                else
                                {
                                    return View("WithDrawalError");
                                }
                                bankingTransaction.Amount = New;
                                SelectedIra.RunningTotal = 5000;
                                SelectedIra.Balance += New;
                                bankingTransaction.Description = "Transfer to " + IRAact;
                                bankingTransaction.BankingTransactionType = BankingTranactionType.Transfer;

                                db.BankingTransaction.Add(bankingTransaction);
                                db.SaveChanges();

                                BankingTransaction IRA = new BankingTransaction();
                                IRA.Amount = New;
                                IRA.IRAAccount = NewIraAccounts;
                                IRA.Description = "Transfer from " + Cact;
                                IRA.TransactionDate = bankingTransaction.TransactionDate;
                                IRA.BankingTransactionType = BankingTranactionType.Transfer;
                                db.BankingTransaction.Add(IRA);
                                db.SaveChanges();


                            }

                            else if (ErrorActionIRA.StockAccounts.StockAccountID != 0)
                            {
                                StockAccount SelectedStockAccount = db.StockAccount.Find(ErrorActionIRA.StockAccounts.StockAccountID);

                                String STact = SelectedStockAccount.AccountNumber;

                                bankingTransaction.StockAccount = SelectedStockAccount;

                                Decimal New = (5000 - SelectedIra.RunningTotal);
                                if (SelectedStockAccount.CashBalance - bankingTransaction.Amount <= SelectedStockAccount.CashBalance)
                                {
                                    SelectedStockAccount.Overdrawn = true;
                                    SelectedStockAccount.CashBalance -= New;
                                    if (bankingTransaction.Amount <= SelectedStockAccount.CashBalance + 50 && SelectedStockAccount.CashBalance >= 0)
                                    {
                                        if (SelectedStockAccount.CashBalance - bankingTransaction.Amount < 0 && SelectedStockAccount.CashBalance - bankingTransaction.Amount >= -50)
                                        {
                                            // Create a new banking transaction 
                                            BankingTransaction Fee = new BankingTransaction();

                                            // Take out the fee
                                            SelectedStockAccount.CashBalance -= 30;

                                            // Create the bankings transaction 
                                            Fee.Description = "OverDrawnFee";
                                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                                            Fee.Amount = 30;
                                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                                            Fee.StockAccount = SelectedStockAccount;

                                            // Add the fee to the database
                                            db.BankingTransaction.Add(Fee);
                                            db.SaveChanges();

                                            // Send the email 
                                            String Body = SelectedStockAccount.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedStockAccount.CashBalance.ToString();
                                            LonghornBank.Utility.Email.PasswordEmail(ErrorActionIRA.CustomerProfile.Email, "Overdrawn Account", Body);
                                        }
                                    }
                                }
                                else
                                {
                                    return View("WithDrawalError");
                                }
                                SelectedIra.RunningTotal = 5000;
                                SelectedIra.Balance += New;
                                bankingTransaction.Amount = New;
                                bankingTransaction.Description = "Transfer to " + IRAact;
                                bankingTransaction.BankingTransactionType = BankingTranactionType.Transfer;

                                BankingTransaction IRA = new BankingTransaction();
                                IRA.Amount = New;
                                IRA.IRAAccount = NewIraAccounts;
                                IRA.Description = "Transfer from " + STact;
                                IRA.TransactionDate = bankingTransaction.TransactionDate;
                                IRA.BankingTransactionType = BankingTranactionType.Transfer;
                                db.BankingTransaction.Add(IRA);
                                db.SaveChanges();
                            }
                            break;

                        case "User":

                            if (SelectedIra.RunningTotal == 5000)
                            {
                                return View("NoMoreDeposits");
                            }

                            if (bankingTransaction.Amount + SelectedIra.RunningTotal <= 5000)
                            {
                                SelectedIra.RunningTotal = SelectedIra.RunningTotal + bankingTransaction.Amount;
                                Decimal New_Balance2 = bankingTransaction.Amount;
                                SelectedIra.Balance += New_Balance2;

                                if (ErrorActionIRA.SavingAccounts.SavingID != 0)
                                {
                                    // Find the Selected Checking Account
                                    Saving SelectedSaving = db.SavingsAccount.Find(ErrorActionIRA.SavingAccounts.SavingID);

                                    // Create a list of checking accounts and add the one seleceted 
                                    List<Saving> NewSavingAccounts = new List<Saving> { SelectedSaving };

                                    String SaAct = SelectedSaving.AccountNumber;

                                    bankingTransaction.SavingsAccount = NewSavingAccounts;

                                    bankingTransaction.Amount = New_Balance2;

                                    if (bankingTransaction.Amount <= SelectedSaving.Balance + 50 && SelectedSaving.Balance >= 0)
                                    {
                                        SelectedSaving.Overdrawn = false;
                                        if (SelectedSaving.Balance - bankingTransaction.Amount < 0 && SelectedSaving.Balance - bankingTransaction.Amount >= -50)
                                        {
                                            SelectedSaving.Balance -= New_Balance2;

                                            // Create a new banking transaction 
                                            BankingTransaction Fee = new BankingTransaction();

                                            // Take out the fee
                                            SelectedSaving.Balance -= 30;

                                            // Create the bankings transaction 
                                            Fee.Description = "OverDrawnFee";
                                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                                            Fee.Amount = 30;
                                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                                            Fee.SavingsAccount = NewSavingAccounts;

                                            // Add the fee to the database
                                            db.BankingTransaction.Add(Fee);
                                            db.SaveChanges();

                                            // Send the email 
                                            String Body = SelectedSaving.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedSaving.Balance.ToString();
                                            LonghornBank.Utility.Email.PasswordEmail(ErrorActionIRA.CustomerProfile.Email, "Overdrawn Account", Body);
                                        }
                                        else
                                        {
                                            SelectedSaving.Balance -= New_Balance2;
                                        }
                                    }
                                    else
                                    {
                                        return View("WithDrawalError");
                                    }

                                    bankingTransaction.Description = "Transfer to " + IRAact;
                                    bankingTransaction.BankingTransactionType = BankingTranactionType.Transfer;
                                    bankingTransaction.Amount = New_Balance2;
                                    db.BankingTransaction.Add(bankingTransaction);
                                    db.SaveChanges();


                                    BankingTransaction IRA = new BankingTransaction();
                                    IRA.IRAAccount = NewIraAccounts;
                                    IRA.Description = "Transfer from " + SaAct;
                                    IRA.Amount = bankingTransaction.Amount;
                                    IRA.BankingTransactionType = BankingTranactionType.Transfer;
                                    IRA.TransactionDate = bankingTransaction.TransactionDate;
                                    db.BankingTransaction.Add(IRA);
                                    db.SaveChanges();

                                }

                                else if (ErrorActionIRA.CheckingAccounts.CheckingID != 0)
                                {
                                    Checking SelectedChecking = db.CheckingAccount.Find(ErrorActionIRA.CheckingAccounts.CheckingID);

                                    // Create a list of checking accounts and add the one seleceted 
                                    List<Checking> NewCheckingAccounts = new List<Checking> { SelectedChecking };

                                    String Cact = SelectedChecking.AccountNumber;

                                    bankingTransaction.CheckingAccount = NewCheckingAccounts;

                                    bankingTransaction.Amount = New_Balance2;

                                    if (bankingTransaction.Amount <= SelectedChecking.Balance + 50 && SelectedChecking.Balance >= 0)
                                    {
                                        SelectedChecking.Overdrawn = false;
                                        if (SelectedChecking.Balance - bankingTransaction.Amount < 0 && SelectedChecking.Balance - bankingTransaction.Amount >= -50)
                                        {
                                            SelectedChecking.Balance -= New_Balance2;

                                            // Create a new banking transaction 
                                            BankingTransaction Fee = new BankingTransaction();

                                            // Take out the fee
                                            SelectedChecking.Balance -= 30;

                                            // Create the bankings transaction 
                                            Fee.Description = "OverDrawnFee";
                                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                                            Fee.Amount = 30;
                                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                                            Fee.CheckingAccount = NewCheckingAccounts;

                                            // Add the fee to the database
                                            db.BankingTransaction.Add(Fee);
                                            db.SaveChanges();

                                            // Send the email 
                                            String Body = SelectedChecking.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedChecking.Balance.ToString();
                                            LonghornBank.Utility.Email.PasswordEmail(ErrorActionIRA.CustomerProfile.Email, "Overdrawn Account", Body);
                                        }
                                        else
                                        {
                                            SelectedChecking.Balance -= New_Balance2;
                                        }
                                    }
                                    else
                                    {
                                        return View("WithDrawalError");
                                    }

                                    bankingTransaction.Description = "Transfer to " + IRAact;
                                    bankingTransaction.BankingTransactionType = BankingTranactionType.Transfer;
                                    bankingTransaction.Amount = New_Balance2;
                                    db.BankingTransaction.Add(bankingTransaction);
                                    db.SaveChanges();

                                    BankingTransaction IRA = new BankingTransaction();
                                    IRA.IRAAccount = NewIraAccounts;
                                    IRA.Description = "Transfer from " + Cact;
                                    IRA.Amount = bankingTransaction.Amount;
                                    IRA.BankingTransactionType = BankingTranactionType.Transfer;
                                    IRA.TransactionDate = bankingTransaction.TransactionDate;
                                    db.BankingTransaction.Add(IRA);
                                    db.SaveChanges();

                                }

                                else if (ErrorActionIRA.StockAccounts.StockAccountID != 0)
                                {
                                    StockAccount SelectedStockAccount = db.StockAccount.Find(ErrorActionIRA.StockAccounts.StockAccountID);

                                    String STact = SelectedStockAccount.AccountNumber;

                                    bankingTransaction.StockAccount = SelectedStockAccount;

                                    bankingTransaction.Amount = New_Balance2;

                                    
                                    if (bankingTransaction.Amount <= SelectedStockAccount.CashBalance + 50 && SelectedStockAccount.CashBalance >= 0)
                                    {
                                        SelectedStockAccount.Overdrawn = false;
                                        if (SelectedStockAccount.CashBalance - bankingTransaction.Amount < 0 && SelectedStockAccount.CashBalance - bankingTransaction.Amount >= -50)
                                        {
                                            SelectedStockAccount.CashBalance -= New_Balance2;
                                            // Create a new banking transaction 
                                            BankingTransaction Fee = new BankingTransaction();

                                            // Take out the fee
                                            SelectedStockAccount.CashBalance -= 30;

                                            // Create the bankings transaction 
                                            Fee.Description = "OverDrawnFee";
                                            Fee.BankingTransactionType = BankingTranactionType.Fee;
                                            Fee.Amount = 30;
                                            Fee.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                                            Fee.TransactionDate = bankingTransaction.TransactionDate;
                                            Fee.TransactionDispute = DisputeStatus.NotDisputed;
                                            Fee.StockAccount = SelectedStockAccount;

                                            // Add the fee to the database
                                            db.BankingTransaction.Add(Fee);
                                            db.SaveChanges();

                                            // Send the email 
                                            String Body = SelectedStockAccount.Name.ToString() + " : has been overdrawn and you have been charged a $30 fee. Your current account balance is $" + SelectedStockAccount.CashBalance.ToString();
                                            LonghornBank.Utility.Email.PasswordEmail(ErrorActionIRA.CustomerProfile.Email, "Overdrawn Account", Body);
                                        }
                                        else
                                        {
                                            SelectedStockAccount.CashBalance -= New_Balance2;
                                        }
                                    }
                                    else
                                    {
                                        return View("WithDrawalError");
                                    }

                                    bankingTransaction.Description = "Transfer to " + IRAact;
                                    bankingTransaction.BankingTransactionType = BankingTranactionType.Transfer;
                                    bankingTransaction.Amount = New_Balance2;
                                    db.BankingTransaction.Add(bankingTransaction);
                                    db.SaveChanges();


                                    BankingTransaction IRA = new BankingTransaction();
                                    IRA.IRAAccount = NewIraAccounts;
                                    IRA.Description = "Transfer from " + STact;
                                    IRA.Amount = bankingTransaction.Amount;
                                    IRA.BankingTransactionType = BankingTranactionType.Transfer;
                                    IRA.TransactionDate = bankingTransaction.TransactionDate;
                                    db.BankingTransaction.Add(IRA);
                                    db.SaveChanges();
                                }
                            }

                            else
                            {
                                return RedirectToAction("IRAError", new { Description = bankingTransaction.Description, Date = bankingTransaction.TransactionDate, Amount = bankingTransaction.Amount, IRAID = ErrorActionIRA.IRAAccounts.IRAID, CID = ErrorActionIRA.CheckingAccounts.CheckingID, SID = ErrorActionIRA.SavingAccounts.SavingID, StAID = ErrorActionIRA.StockAccounts.StockAccountID, btID = bankingTransaction.BankingTransactionID, type = bankingTransaction.BankingTransactionType });
                            }
                            break;
                    }
                }

            }
            // Redirect 
            return RedirectToAction("Home", "Portal", new { id = ErrorActionIRA.CustomerProfile.Id });
        }

        public ActionResult IRAWithDrawalError(string Description, DateTime Date, Decimal Amount, int IRAID, int CID, int SID, int StAID, int btID, BankingTranactionType type)
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
            // Find the Selected Checking Account
            Checking SelectedChecking = db.CheckingAccount.Find(CID);

            Saving SelectedSaving = db.SavingsAccount.Find(SID);

            StockAccount SelectedStockAccount = db.StockAccount.Find(StAID);

            IRA SelectedIRA = db.IRAAccount.Find(IRAID);

            BankingTransaction bankingtransaction = new BankingTransaction();

            bankingtransaction.TransactionDate = Date;

            bankingtransaction.Amount = Amount;

            bankingtransaction.Description = Description;

            bankingtransaction.BankingTransactionType = type;

            IRAViewModel ErrorAction = new IRAViewModel
            {
                CheckingAccounts = SelectedChecking,
                SavingAccounts = SelectedSaving,
                StockAccounts = SelectedStockAccount,
                IRAAccounts = SelectedIRA,
                PayeeTransaction = bankingtransaction,
                CustomerProfile = customer
            };


            return View(ErrorAction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IRAWithDrawalError(IRAViewModel ErrorAction, string submit)
        {
            
            BankingTransaction bankingTransaction = new BankingTransaction();
            bankingTransaction.TransactionDispute = DisputeStatus.NotDisputed;

            if (ErrorAction.CustomerProfile == null)
            {
                return RedirectToAction("Portal", "Home");
            }

            bankingTransaction.Description = ErrorAction.PayeeTransaction.Description;
            bankingTransaction.TransactionDate = ErrorAction.PayeeTransaction.TransactionDate;
            bankingTransaction.BankingTransactionID = ErrorAction.PayeeTransaction.BankingTransactionID;
            bankingTransaction.BankingTransactionType = ErrorAction.PayeeTransaction.BankingTransactionType;
            bankingTransaction.Amount = ErrorAction.PayeeTransaction.Amount;

            // Check to see if Deposit 
            if (bankingTransaction.BankingTransactionType == BankingTranactionType.Withdrawl)
            {
                if (ErrorAction.IRAAccounts.IRAID != 0)
                {
                    // Find the Selected Checking Account
                    IRA SelectedIra = db.IRAAccount.Find(ErrorAction.IRAAccounts.IRAID);

                    // Create a list of checking accounts and add the one seleceted 
                    List<IRA> NewIraAccounts = new List<IRA> { SelectedIra };

                    bankingTransaction.IRAAccount = NewIraAccounts;

                    switch (submit)
                    {
                        case "Add":
                            Decimal New_Balance = SelectedIra.Balance - ErrorAction.PayeeTransaction.Amount;
                            SelectedIra.Balance = New_Balance;
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            Decimal Fee = SelectedIra.Balance - 30;
                            SelectedIra.Balance = Fee;
                            bankingTransaction.Description = "Unqualified WithDrawal fee";
                            bankingTransaction.TransactionDate = ErrorAction.PayeeTransaction.TransactionDate;
                            bankingTransaction.BankingTransactionID = ErrorAction.PayeeTransaction.BankingTransactionID;
                            bankingTransaction.BankingTransactionType = BankingTranactionType.Fee;
                            bankingTransaction.Amount = 30;
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            break;

                        case "Include":
                            Decimal New_Balance2 = SelectedIra.Balance - ErrorAction.PayeeTransaction.Amount - 30;
                            SelectedIra.Balance = New_Balance2;
                            bankingTransaction.Amount = ErrorAction.PayeeTransaction.Amount - 30;
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            Decimal Fee2 = SelectedIra.Balance - 30;
                            bankingTransaction.Description = "Unqualified WithDrawal fee";
                            bankingTransaction.TransactionDate = ErrorAction.PayeeTransaction.TransactionDate;
                            bankingTransaction.BankingTransactionID = ErrorAction.PayeeTransaction.BankingTransactionID;
                            bankingTransaction.BankingTransactionType = BankingTranactionType.Fee;
                            bankingTransaction.Amount = 30;
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();
                            break;
                    }
                }
            }

            if (ErrorAction.PayeeTransaction.BankingTransactionType == BankingTranactionType.Transfer)
            {
                // Find the Selected Checking Account
                IRA SelectedIra = db.IRAAccount.Find(ErrorAction.IRAAccounts.IRAID);

                String IRAact = SelectedIra.AccountNumber;

                // Create a list of checking accounts and add the one seleceted 
                List<IRA> NewIraAccounts = new List<IRA> { SelectedIra };

                bankingTransaction.IRAAccount = NewIraAccounts;

                switch (submit)
                {
                    case "Add":
                        Decimal actual = bankingTransaction.Amount;
                        if (ErrorAction.SavingAccounts.SavingID != 0)
                        {
                            // Find the Selected Checking Account
                            Saving SelectedSaving = db.SavingsAccount.Find(ErrorAction.SavingAccounts.SavingID);

                            // Create a list of checking accounts and add the one seleceted 
                            List<Saving> NewSavingAccounts = new List<Saving> { SelectedSaving };

                            String SaAct = SelectedSaving.AccountNumber;
                            if (SelectedIra.Balance - bankingTransaction.Amount <= SelectedIra.Balance)
                            {
                                SelectedIra.Overdrawn = false;
                                if (SelectedIra.Balance - bankingTransaction.Amount < 0 && SelectedIra.Balance - bankingTransaction.Amount >= -50)
                                {

                                    if (SelectedIra.Balance - bankingTransaction.Amount < 0 && SelectedIra.Balance - bankingTransaction.Amount >= -50)
                                    {
                                        SelectedIra.Balance -= 30;
                                        bankingTransaction.Description = "OverDrawnFee";
                                        bankingTransaction.BankingTransactionType = BankingTranactionType.Fee;
                                        bankingTransaction.Amount = 30;
                                        db.BankingTransaction.Add(bankingTransaction);
                                        db.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                return View("WithDrawalError");
                            }
                            SelectedIra.Balance -= actual;
                            bankingTransaction.Description = "Transfer to " + SaAct;
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            Decimal Transfer_Balance = SelectedSaving.Balance + actual;
                            bankingTransaction.SavingsAccount = NewSavingAccounts;
                            SelectedSaving.Balance = Transfer_Balance;
                            BankingTransaction Saving = new BankingTransaction();
                            Saving.Amount = actual;
                            Saving.TransactionDate = bankingTransaction.TransactionDate;
                            Saving.BankingTransactionType = BankingTranactionType.Transfer;
                            Saving.Description = "Transfer from " + IRAact;

                            db.BankingTransaction.Add(Saving);
                            db.SaveChanges();

                            BankingTransaction IRA = new BankingTransaction();
                            Decimal Fee1 = SelectedIra.Balance - 30;
                            SelectedIra.Balance = Fee1;
                            IRA.IRAAccount = NewIraAccounts;
                            IRA.Description = "Unqualified WithDrawal fee";
                            IRA.TransactionDate = ErrorAction.PayeeTransaction.TransactionDate;
                            IRA.BankingTransactionType = BankingTranactionType.Fee;
                            IRA.Amount = 30;
                            db.BankingTransaction.Add(IRA);
                            db.SaveChanges();
                        }

                        else if (ErrorAction.CheckingAccounts.CheckingID != 0)
                        {
                            Checking SelectedChecking = db.CheckingAccount.Find(ErrorAction.CheckingAccounts.CheckingID);

                            // Create a list of checking accounts and add the one seleceted 
                            List<Checking> NewCheckingAccounts = new List<Checking> { SelectedChecking };

                            String Cact = SelectedChecking.AccountNumber;

                            if (SelectedIra.Balance - bankingTransaction.Amount <= SelectedIra.Balance)
                            {
                                SelectedIra.Overdrawn = false;
                                if (SelectedIra.Balance - bankingTransaction.Amount < 0 && SelectedIra.Balance - bankingTransaction.Amount >= -50)
                                {

                                    if (SelectedIra.Balance - bankingTransaction.Amount < 0 && SelectedIra.Balance - bankingTransaction.Amount >= -50)
                                    {
                                        SelectedIra.Balance -= 30;
                                        bankingTransaction.Description = "OverDrawnFee";
                                        bankingTransaction.BankingTransactionType = BankingTranactionType.Fee;
                                        bankingTransaction.Amount = 30;
                                        db.BankingTransaction.Add(bankingTransaction);
                                        db.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                return View("WithDrawalError");
                            }
                            SelectedIra.Balance -= actual;
                            bankingTransaction.Description = "Transfer to " + Cact;
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            BankingTransaction Checking = new BankingTransaction();
                            bankingTransaction.CheckingAccount = NewCheckingAccounts;
                            Decimal Transfer_Balance = SelectedChecking.Balance + bankingTransaction.Amount;
                            SelectedChecking.Balance = Transfer_Balance;
                            Checking.BankingTransactionType = BankingTranactionType.Transfer;
                            Checking.Amount = actual;
                            Checking.TransactionDate = bankingTransaction.TransactionDate;
                            Checking.Description = "Transfer from " + IRAact;

                            db.BankingTransaction.Add(Checking);
                            db.SaveChanges();

                            BankingTransaction IRA = new BankingTransaction();
                            Decimal Fee1 = SelectedIra.Balance - 30;
                            SelectedIra.Balance = Fee1;
                            IRA.IRAAccount = NewIraAccounts;
                            IRA.Description = "Unqualified WithDrawal fee";
                            IRA.TransactionDate = ErrorAction.PayeeTransaction.TransactionDate;
                            IRA.BankingTransactionType = BankingTranactionType.Fee;
                            IRA.Amount = 30;
                            db.BankingTransaction.Add(IRA);
                            db.SaveChanges();
                        }

                        else if (ErrorAction.StockAccounts.StockAccountID != 0)
                        {
                            StockAccount SelectedStockAccount = db.StockAccount.Find(ErrorAction.StockAccounts.StockAccountID);

                            String STact = SelectedStockAccount.AccountNumber;

                            if (SelectedIra.Balance - bankingTransaction.Amount <= SelectedIra.Balance)
                            {
                                SelectedIra.Overdrawn = false;
                                if (SelectedIra.Balance - bankingTransaction.Amount < 0 && SelectedIra.Balance - bankingTransaction.Amount >= -50)
                                {

                                    if (SelectedIra.Balance - bankingTransaction.Amount < 0 && SelectedIra.Balance - bankingTransaction.Amount >= -50)
                                    {
                                        SelectedIra.Balance -= 30;
                                        bankingTransaction.Description = "OverDrawnFee";
                                        bankingTransaction.BankingTransactionType = BankingTranactionType.Fee;
                                        bankingTransaction.Amount = 30;
                                        db.BankingTransaction.Add(bankingTransaction);
                                        db.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                return View("WithDrawalError");
                            }
                            bankingTransaction.Amount = actual;
                            SelectedIra.Balance -= actual;
                            bankingTransaction.Description = "Transfer to " + STact;
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            Decimal Transfer_Balance = SelectedStockAccount.CashBalance + actual;
                            SelectedStockAccount.CashBalance = Transfer_Balance;

                            BankingTransaction Stock = new BankingTransaction();
                            bankingTransaction.StockAccount = SelectedStockAccount;
                            Stock.Amount = actual;
                            Stock.TransactionDate = bankingTransaction.TransactionDate;
                            Stock.BankingTransactionType = BankingTranactionType.Transfer;
                            Stock.Description = "Transfer from " + IRAact;

                            db.BankingTransaction.Add(Stock);
                            db.SaveChanges();

                            BankingTransaction IRA = new BankingTransaction();
                            Decimal Fee1 = SelectedIra.Balance - 30;
                            SelectedIra.Balance = Fee1;
                            IRA.IRAAccount = NewIraAccounts;
                            IRA.Description = "Unqualified WithDrawal fee";
                            IRA.TransactionDate = ErrorAction.PayeeTransaction.TransactionDate;
                            IRA.BankingTransactionType = BankingTranactionType.Fee;
                            IRA.Amount = 30;
                            db.BankingTransaction.Add(IRA);
                            db.SaveChanges();
                        }
                        break;

                    case "Include":

                        Decimal actual1= bankingTransaction.Amount - 30;

                        if (ErrorAction.CheckingAccounts.CheckingID != 0)
                        {
                            Checking SelectedChecking = db.CheckingAccount.Find(ErrorAction.CheckingAccounts.CheckingID);

                            // Create a list of checking accounts and add the one seleceted 
                            List<Checking> NewCheckingAccounts = new List<Checking> { SelectedChecking };

                            String Cact = SelectedChecking.AccountNumber;
                            if (SelectedIra.Balance - bankingTransaction.Amount <= SelectedIra.Balance)
                            {
                                SelectedIra.Overdrawn = false;
                                if (SelectedIra.Balance - bankingTransaction.Amount < 0 && SelectedIra.Balance - bankingTransaction.Amount >= -50)
                                {
                                    if (SelectedIra.Balance - bankingTransaction.Amount < 0 && SelectedIra.Balance - bankingTransaction.Amount >= -50)
                                    {
                                        SelectedIra.Balance -= 30;
                                        bankingTransaction.Description = "OverDrawnFee";
                                        bankingTransaction.BankingTransactionType = BankingTranactionType.Fee;
                                        bankingTransaction.Amount = 30;
                                        db.BankingTransaction.Add(bankingTransaction);
                                        db.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                return View("WithDrawalError");
                            }
                            SelectedIra.Balance -= actual1;
                            bankingTransaction.Amount = actual1;
                            bankingTransaction.Description = "Transfer to " + Cact;
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            BankingTransaction Checking = new BankingTransaction();
                            bankingTransaction.CheckingAccount = NewCheckingAccounts;
                            Decimal Transfer_Balance = SelectedChecking.Balance + actual1;
                            SelectedChecking.Balance = Transfer_Balance;
                            Checking.Amount = actual1;
                            Checking.TransactionDate = bankingTransaction.TransactionDate;
                            Checking.BankingTransactionType = BankingTranactionType.Transfer;
                            Checking.Description = "Transfer from " + IRAact;

                            db.BankingTransaction.Add(Checking);
                            db.SaveChanges();

                            BankingTransaction IRA = new BankingTransaction();
                            Decimal Fee = SelectedIra.Balance - 30;
                            SelectedIra.Balance = Fee;
                            IRA.IRAAccount = NewIraAccounts;
                            IRA.Description = "Unqualified WithDrawal fee";
                            IRA.TransactionDate = ErrorAction.PayeeTransaction.TransactionDate;
                            IRA.BankingTransactionType = BankingTranactionType.Fee;
                            IRA.Amount = 30;
                            db.BankingTransaction.Add(IRA);
                            db.SaveChanges();
                        }

                        else if (ErrorAction.SavingAccounts.SavingID != 0)
                        {
                            Saving SelectedSaving = db.SavingsAccount.Find(ErrorAction.SavingAccounts.SavingID);

                            // Create a list of checking accounts and add the one seleceted 
                            List<Saving> NewSavingAccounts = new List<Saving> { SelectedSaving };

                            String Sact = SelectedSaving.AccountNumber;

                            if (SelectedIra.Balance - bankingTransaction.Amount <= SelectedIra.Balance)
                            {
                                SelectedIra.Overdrawn = false;
                                if (SelectedIra.Balance - bankingTransaction.Amount < 0 && SelectedIra.Balance - bankingTransaction.Amount >= -50)
                                { 

                                    if (SelectedIra.Balance - bankingTransaction.Amount < 0 && SelectedIra.Balance - bankingTransaction.Amount >= -50)
                                    {
                                        SelectedIra.Balance -= 30;
                                        bankingTransaction.Description = "OverDrawnFee";
                                        bankingTransaction.BankingTransactionType = BankingTranactionType.Fee;
                                        bankingTransaction.Amount = 30;
                                        db.BankingTransaction.Add(bankingTransaction);
                                        db.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                return View("WithDrawalError");
                            }
                            SelectedIra.Balance -= actual1;
                            bankingTransaction.Description = "Transfer to " + Sact;
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            BankingTransaction Saving = new BankingTransaction();
                            Decimal Transfer_Balance = SelectedSaving.Balance + actual1;
                            bankingTransaction.SavingsAccount = NewSavingAccounts;
                            SelectedSaving.Balance = Transfer_Balance;
                            Saving.Amount = actual1;
                            Saving.TransactionDate = bankingTransaction.TransactionDate;
                            Saving.TransactionDate = bankingTransaction.TransactionDate;
                            Saving.Description = "Transfer from " + IRAact;

                            db.BankingTransaction.Add(Saving);
                            db.SaveChanges();

                            BankingTransaction IRA = new BankingTransaction();
                            Decimal Fee = SelectedIra.Balance - 30;
                            SelectedIra.Balance = Fee;
                            IRA.IRAAccount = NewIraAccounts;
                            IRA.Description = "Unqualified WithDrawal fee";
                            IRA.TransactionDate = ErrorAction.PayeeTransaction.TransactionDate;
                            IRA.BankingTransactionType = BankingTranactionType.Fee;
                            IRA.Amount = 30;
                            db.BankingTransaction.Add(IRA);
                            db.SaveChanges();
                        }

                        else if (ErrorAction.StockAccounts.StockAccountID != 0)
                        {
                            StockAccount SelectedStockAccount = db.StockAccount.Find(ErrorAction.StockAccounts.StockAccountID);

                            String STact = SelectedStockAccount.AccountNumber;

                            if (SelectedIra.Balance - bankingTransaction.Amount <= SelectedIra.Balance)
                            {
                                SelectedIra.Overdrawn = false;
                                if (SelectedIra.Balance - bankingTransaction.Amount < 0 && SelectedIra.Balance - bankingTransaction.Amount >= -50)
                                {

                                    if (SelectedIra.Balance - bankingTransaction.Amount < 0 && SelectedIra.Balance - bankingTransaction.Amount >= -50)
                                    {
                                        SelectedIra.Balance -= 30;
                                        bankingTransaction.Description = "OverDrawnFee";
                                        bankingTransaction.BankingTransactionType = BankingTranactionType.Fee;
                                        bankingTransaction.Amount = 30;
                                        db.BankingTransaction.Add(bankingTransaction);
                                        db.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                return View("WithDrawalError");
                            }
                            SelectedIra.Balance -= actual1;
                            bankingTransaction.Description = "Transfer to " + STact;
                            db.BankingTransaction.Add(bankingTransaction);
                            db.SaveChanges();

                            BankingTransaction Stock = new BankingTransaction();
                            Decimal Transfer_Balance = SelectedStockAccount.CashBalance + actual1;
                            SelectedStockAccount.CashBalance = Transfer_Balance;
                            bankingTransaction.StockAccount = SelectedStockAccount;
                            Stock.Amount = actual1;
                            Stock.TransactionDate = bankingTransaction.TransactionDate;
                            Stock.BankingTransactionType = BankingTranactionType.Transfer;
                            Stock.Description = "Transfer from " + IRAact;

                            BankingTransaction IRA = new BankingTransaction();
                            Decimal Fee = SelectedIra.Balance - 30;
                            SelectedIra.Balance = Fee;
                            IRA.IRAAccount = NewIraAccounts;
                            IRA.Description = "Unqualified WithDrawal fee";
                            IRA.TransactionDate = ErrorAction.PayeeTransaction.TransactionDate;
                            IRA.BankingTransactionType = BankingTranactionType.Fee;
                            IRA.Amount = 30;
                            db.BankingTransaction.Add(IRA);
                            db.SaveChanges();

                            db.BankingTransaction.Add(Stock);
                            db.SaveChanges();
                        }

                        break;
                }
            }

            return RedirectToAction("Portal", "Home", new { id = ErrorAction.CustomerProfile.Id });

        }

        public ActionResult DisputeConfirmation()
        {
            return View();
        }
        

        //Detailed Search function
        public ActionResult SearchResults(SearchViewModel TheSearch)
        {
            List<BankingTransaction> Transactions = SearchTransactions.Search(db, TheSearch, 0, 0);

            ViewBag.ResultsCount = Transactions.Count;
            ViewBag.Ranges = SearchTransactions.AmountRange();
            ViewBag.Dates = SearchTransactions.DateRanges();
            return View("Index", Transactions);
        }
        

        public SelectList GetAllBankingTypesList()
        {
            List<BankingTranactionType> BBT = Enum.GetValues(typeof(BankingTranactionType)).Cast<BankingTranactionType>().ToList();
            SelectList BBTSelectList = new SelectList(BBT, new BankingTranactionType());
            ViewBag.AllBankingTypes = BBTSelectList;
            return BBTSelectList;
        }

        public SelectList AmountRange()
        {
            // Create a list of ranges
            List<Ranges> RangesList = new List<Ranges>();

            Ranges None = new Ranges { Name = "None", RangeID = 0 };
            Ranges _100 = new Ranges { Name = "$0-100", RangeID = 1 };
            Ranges _100_200 = new Ranges { Name = "$100-200", RangeID = 2 };
            Ranges _200_300 = new Ranges { Name = "$200-300", RangeID = 3 };
            Ranges _300 = new Ranges { Name = "$300+", RangeID = 4 };
            Ranges Custom = new Ranges { Name = "Custom", RangeID = 5 };

            // Add to the list
            RangesList.Add(None);
            RangesList.Add(_100);
            RangesList.Add(_100_200);
            RangesList.Add(_200_300);
            RangesList.Add(_300);
            RangesList.Add(Custom);
            

            // Convert to Select List

            SelectList RangesSelect = new SelectList(RangesList, "RangeID", "Name");

            return RangesSelect;
        }

        public SelectList DateRanges()
        {
            // Create a list for Dates
            List<RangesDate> RangeDates = new List<RangesDate>();

            RangesDate All = new RangesDate { Name = "All Available", RangeID = 0 };
            RangesDate Last15 = new RangesDate { Name = "Last 15 Days", RangeID = 1 };
            RangesDate Last30 = new RangesDate { Name = "Last 30 Days", RangeID = 2 };
            RangesDate Last60 = new RangesDate { Name = "Last 60 Days", RangeID = 3 };
            RangesDate Custom = new RangesDate { Name = "Custom", RangeID = 4 };

            // add to the list 
            RangeDates.Add(All);
            RangeDates.Add(Last15);
            RangeDates.Add(Last30);
            RangeDates.Add(Last60);
            RangeDates.Add(Custom);

            // create a select list 
            SelectList DateSelect = new SelectList(RangeDates, "RangeID", "Name");

            return DateSelect;
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
