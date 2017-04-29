using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LonghornBank.Models;
using System.Net.Mail;
using LonghornBank.Utility;


namespace LonghornBank.Controllers
{
    public class ManagersController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Managers
        public ActionResult Index()
        {
            return View("Index");
        }
        public ActionResult BalancedPortfolioSuccess()
        {
            BalancedPortfolio.AddBounus(db);
            return View();
        }
        public static void SendEmail(String toEmailAddress, String emailSubject, String emailBody)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("DoNotReplyLonghornBank@gmail.com", "P@ssword!"),
                EnableSsl = true
            };
            String finalMessage = "The following is a message from Longhorn Bank:\n\n"+ emailBody ;
            MailAddress senderEmail = new MailAddress("DoNotReplyLonghornBank@gmail.com", "Team 7");
            MailMessage mm = new MailMessage();
            mm.Subject = "Team 7 - " + emailSubject;
            mm.Sender = senderEmail;
            mm.From = senderEmail;
            mm.To.Add(new MailAddress(toEmailAddress));
            mm.Body = finalMessage;
            client.Send(mm);
        }
        public String PrepEmail2 (BankingTransaction editedtransobject)
        {
            String emailstring = "wehaveaproblem@prepemailfunction.com";
            if (editedtransobject.CheckingAccount.FirstOrDefault() != null)
            {
                Checking CA = editedtransobject.CheckingAccount.First();
                emailstring = CA.Customer.Email.ToString();
            }
            if (editedtransobject.SavingsAccount.FirstOrDefault() != null)
            {
                Saving SA = editedtransobject.SavingsAccount.First();
                emailstring = SA.Customer.Email.ToString();
            }
            if (editedtransobject.IRAAccount.FirstOrDefault() != null)
            {
                IRA IA = editedtransobject.IRAAccount.First();
                emailstring = IA.Customer.Email.ToString();
            }
            return emailstring;
        }
        public String PrepEmail (BankingTransaction editedtransobject )
        {
            String emailstring="wehaveaproblem@prepemailfunction.com";
            if (editedtransobject.CheckingAccount.FirstOrDefault() != null)
            {
                Checking CA = editedtransobject.CheckingAccount.First();
                emailstring = CA.Customer.Email.ToString();
            }
            if (editedtransobject.SavingsAccount.FirstOrDefault() != null)
            {
                Saving SA = editedtransobject.SavingsAccount.First();
                emailstring = SA.Customer.Email.ToString();
            }
            if (editedtransobject.IRAAccount.FirstOrDefault() != null)
            {
                IRA IA = editedtransobject.IRAAccount.First();
                emailstring = IA.Customer.Email.ToString();
            }
            return emailstring;
        }
        // Post Index To revise disputed amounts
        [HttpPost]
        public ActionResult Disputes(string BankingTransactionID, string ManagerApprovedAmount)
        {
            Int32 IntBankingTransactionID = Convert.ToInt32(BankingTransactionID);
            decimal ManagerApprovedDecimal = Convert.ToDecimal(ManagerApprovedAmount);
            BankingTransaction editedtransobject = db.BankingTransaction.Find(IntBankingTransactionID);
            if (editedtransobject.CorrectedAmount == ManagerApprovedDecimal)
            {
                ViewBag.ConfirmationMessage = "You have accepted a customer's dispute for transaction ID #" + BankingTransactionID + " and the amount is now $" + ManagerApprovedDecimal;
                editedtransobject.CorrectedAmount = ManagerApprovedDecimal;
                editedtransobject.TransactionDispute = DisputeStatus.Accepted;
                //SendEmail()
                String emailsubject = "A message from Longhorn Bank";
                String emailbody = "Your dispute on transaction #" + editedtransobject.BankingTransactionID + " has been reviewed by a manager. \n This dispute has been accepted and has been revised to the amount you requested.\n We apologize for the inconvenience.\n Sincerely,\n Longhorn Bank Management";
                String emailstring = PrepEmail(editedtransobject);
                SendEmail(emailstring, emailsubject, emailbody);
                db.SaveChanges();
            }
            if (editedtransobject.CorrectedAmount != ManagerApprovedDecimal)
            {
                ViewBag.ConfirmationMessage = "You have adjusted transaction ID #" + BankingTransactionID + " and the amount is now $" + ManagerApprovedDecimal;
                editedtransobject.CorrectedAmount = ManagerApprovedDecimal;
                editedtransobject.TransactionDispute = DisputeStatus.Adjusted;
                //SendEmail()
                String emailsubject = "A message from Longhorn Bank";
                String emailbody = "Your dispute on transaction #" + editedtransobject.BankingTransactionID + " has been reviewed by a manager. \n This dispute has been adjusted and has been revised to the amount of $" +ManagerApprovedDecimal+"\n We apologize for the inconvenience.\n Sincerely,\n Longhorn Bank Management";
                String emailstring = PrepEmail(editedtransobject);
                SendEmail(emailstring, emailsubject, emailbody);
                db.SaveChanges();
            }
            if (editedtransobject.Amount== ManagerApprovedDecimal)
            {
                ViewBag.ConfirmationMessage = "You have rejected the dispute for ID #" + editedtransobject.BankingTransactionID + " and the amount is unchanged";
                editedtransobject.CorrectedAmount = ManagerApprovedDecimal;
                editedtransobject.TransactionDispute = DisputeStatus.Adjusted;
                //SendEmail()
                String emailsubject = "A message from Longhorn Bank";
                String emailbody = "Your dispute on transaction #" + editedtransobject.BankingTransactionID + " has been reviewed by a manager. \n This dispute has been rejected and will remain at the original amount of $" + ManagerApprovedDecimal + "\n We apologize for the inconvenience.\n Sincerely,\n Longhorn Bank Management";
                String emailstring = PrepEmail(editedtransobject);
                SendEmail(emailstring, emailsubject, emailbody);
                db.SaveChanges();
            }
            List<BankingTransaction> SelectedDisputes = new List<BankingTransaction>();
            SelectedDisputes = db.BankingTransaction.Where(b => b.TransactionDispute == DisputeStatus.Submitted).ToList();
            SelectedDisputes.OrderBy(b => b.TransactionDate);
            db.SaveChanges();
            return View(SelectedDisputes);
        }
        //Get 
        public ActionResult Disputes()
        {
            //var query = from b in db.BankingTransaction
            //          select b;
            //query = query.Where(b => b.TransactionDispute == DisputeStatus.Submitted);
            //List<BankingTransaction> ListOfDisputes = query.ToList();
            //return View("Disputes", ListOfDisputes);
            List<BankingTransaction> SelectedDisputes = new List<BankingTransaction>();
            SelectedDisputes = db.BankingTransaction.Where(b => b.TransactionDispute == DisputeStatus.Submitted).ToList();
            SelectedDisputes.OrderBy(b => b.TransactionDate);
            
            return View(SelectedDisputes);
        }
        public ActionResult ViewAllDisputes()
        {
            var query = from t in db.BankingTransaction
                        select t;
            query = query.Where(t => t.TransactionDispute != DisputeStatus.NotDisputed);
           List<BankingTransaction> listofdisputes = query.ToList();
            return View(listofdisputes);

        }
        [HttpPost]
       public ActionResult DisputeManagementDetail(DisputesViewModel bankingTransaction)
        {
            BankingTransaction transaction = db.BankingTransaction.Find(bankingTransaction.DisputedTransaction.BankingTransactionID);
            transaction.ManagerDisputeMessage = bankingTransaction.DisputedTransaction.ManagerDisputeMessage;
            if (bankingTransaction.DisputedTransaction.CorrectedAmount == bankingTransaction.DisputedTransaction.CustomerOpinion)
            {
                ViewBag.ConfirmationMessage = "You have accepted a customer's dispute for transaction ID #" + bankingTransaction.DisputedTransaction.BankingTransactionID + " and the amount is now $" + bankingTransaction.DisputedTransaction.CorrectedAmount;
                transaction.CorrectedAmount = bankingTransaction.DisputedTransaction.CustomerOpinion;
                transaction.TransactionDispute = DisputeStatus.Accepted;
                //SendEmail()
                String emailsubject = "A message from Longhorn Bank";
                String emailbody = "Your dispute on transaction #" + bankingTransaction.DisputedTransaction.BankingTransactionID + " has been reviewed by a manager. \n This dispute has been accepted and has been revised to the amount you requested.\n We apologize for the inconvenience.\n Sincerely,\n Longhorn Bank Management";
                String emailstring = bankingTransaction.Customer.Email;
                SendEmail(emailstring, emailsubject, emailbody);
                db.SaveChanges();
            }
            if (bankingTransaction.DisputedTransaction.CorrectedAmount != bankingTransaction.DisputedTransaction.CustomerOpinion && bankingTransaction.DisputedTransaction.CorrectedAmount!= bankingTransaction.DisputedTransaction.Amount)
            {
                ViewBag.ConfirmationMessage = "You have adjusted transaction ID #" + bankingTransaction.DisputedTransaction.BankingTransactionID + " and the amount is now $" + bankingTransaction.DisputedTransaction.CorrectedAmount;
                transaction.CorrectedAmount = bankingTransaction.DisputedTransaction.CorrectedAmount;
                transaction.TransactionDispute = DisputeStatus.Adjusted;
                //SendEmail()
                String emailsubject = "A message from Longhorn Bank";
                String emailbody = "Your dispute on transaction #" + bankingTransaction.DisputedTransaction.BankingTransactionID + " has been reviewed by a manager. \n This dispute has been adjusted and has been revised to the amount of $" + bankingTransaction.DisputedTransaction.CorrectedAmount + "\n We apologize for the inconvenience.\n Sincerely,\n Longhorn Bank Management";
                String emailstring = bankingTransaction.Customer.Email;
                SendEmail(emailstring, emailsubject, emailbody);
                db.SaveChanges();
            }
            if (bankingTransaction.DisputedTransaction.CorrectedAmount == bankingTransaction.DisputedTransaction.Amount)
            {
                ViewBag.ConfirmationMessage = "You have rejected the dispute for ID #" + bankingTransaction.DisputedTransaction.BankingTransactionID + " and the amount is unchanged";
                transaction.CorrectedAmount = bankingTransaction.DisputedTransaction.CorrectedAmount;
                bankingTransaction.DisputedTransaction.TransactionDispute = DisputeStatus.Adjusted;
                //SendEmail()
                String emailsubject = "A message from Longhorn Bank";
                String emailbody = "Your dispute on transaction #" + bankingTransaction.DisputedTransaction.BankingTransactionID + " has been reviewed by a manager. \n This dispute has been rejected and will remain at the original amount of $" + bankingTransaction.DisputedTransaction.CorrectedAmount + "\n We apologize for the inconvenience.\n Sincerely,\n Longhorn Bank Management";
                String emailstring = bankingTransaction.Customer.Email;
                SendEmail(emailstring, emailsubject, emailbody);
                db.SaveChanges();
            }
            db.SaveChanges();
            ViewBag.Confirmation = "You have successfully approved the dispute";
            return View("Index");
        }

        //GET
        public ActionResult DisputeManagementDetail(Int32 BankingTransactionID)
        {
            
            DisputesViewModel disputesviewmodel = new DisputesViewModel();
            disputesviewmodel.DisputedTransaction = db.BankingTransaction.Find(BankingTransactionID);
            
            //Begin Copied Code
            
                
                if (disputesviewmodel.DisputedTransaction.CheckingAccount.FirstOrDefault() != null)
                {
                    Checking CA = disputesviewmodel.DisputedTransaction.CheckingAccount.FirstOrDefault();
                    disputesviewmodel.Customer = CA.Customer;
                }
                if (disputesviewmodel.DisputedTransaction.SavingsAccount.FirstOrDefault() != null)
                {
                    Saving SA = disputesviewmodel.DisputedTransaction.SavingsAccount.FirstOrDefault();
                    disputesviewmodel.Customer = SA.Customer;
                }
               if (disputesviewmodel.DisputedTransaction.IRAAccount.FirstOrDefault() != null)
                {
                IRA IA = disputesviewmodel.DisputedTransaction.IRAAccount.FirstOrDefault();
                disputesviewmodel.Customer = IA.Customer;
                }
                
                //End COpied COde
            
            return View(disputesviewmodel);
        }

        //DEPO Approval GET
        public ActionResult DepositApproval()
        {
            List<BankingTransaction> SelectedDepositsOrig = new List<BankingTransaction>();
            SelectedDepositsOrig = db.BankingTransaction.Where(b=>b.ApprovalStatus == ApprovedorNeedsApproval.Approved).ToList();
            List<BankingTransaction> SelectedDeposits = new List<BankingTransaction>();

            foreach (BankingTransaction depo in SelectedDepositsOrig)
                {
                if (depo.ApprovalStatus == ApprovedorNeedsApproval.NeedsApproval) { SelectedDeposits.Add(depo); }
                }
            SelectedDeposits.OrderBy(b => b.TransactionDate);
            List<DepositApprovalViewModel> ListOfDepositApprovalViewModels = new List<DepositApprovalViewModel>();
            db.SaveChanges();
            foreach (var deposit in SelectedDeposits)
            {
                DepositApprovalViewModel depoviewmodel = new DepositApprovalViewModel();
                depoviewmodel.BankingTransaction = deposit;
                if (deposit.CheckingAccount.FirstOrDefault() != null)
                {
                    Checking CA = deposit.CheckingAccount.FirstOrDefault();

                    depoviewmodel.FName = CA.Customer.FName;
                    depoviewmodel.LName = CA.Customer.LName;
                }
                if (deposit.SavingsAccount.FirstOrDefault() != null)
                {
                    Saving SA = deposit.SavingsAccount.FirstOrDefault();

                    depoviewmodel.FName = SA.Customer.FName;
                    depoviewmodel.LName = SA.Customer.LName;
                }
                if (deposit.IRAAccount.FirstOrDefault() != null)
                {
                    IRA IA = deposit.IRAAccount.FirstOrDefault();

                    depoviewmodel.FName = IA.Customer.FName;
                    depoviewmodel.LName = IA.Customer.LName;
                }
                ListOfDepositApprovalViewModels.Add(depoviewmodel);
            }
            return View(ListOfDepositApprovalViewModels);
        }
        //DEPO Approval POST
        [HttpPost]
        public ActionResult DepositApproval(Boolean Approve, string ID)
        {
            Int32 id = Convert.ToInt32(ID);
            if (Approve)
            {
                BankingTransaction bankingtransactionobject = db.BankingTransaction.Find(id);
                bankingtransactionobject.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                ViewBag.SuccessMessage = "You have successfully approved deposits.";
                if (bankingtransactionobject.CheckingAccount.First() != null)
                {
                    Checking checking= bankingtransactionobject.CheckingAccount.First();
                    Decimal pendingbalance= checking.PendingBalance;
                    checking.PendingBalance = 0;
                    checking.Balance = pendingbalance+checking.Balance;
                    db.SaveChanges();
                }
                if (bankingtransactionobject.SavingsAccount.First() != null)
                {
                    Saving saving = bankingtransactionobject.SavingsAccount.First();
                    Decimal pendingbalance = saving.PendingBalance;
                    saving.PendingBalance = 0;
                    saving.Balance = pendingbalance + saving.Balance;
                    db.SaveChanges();
                }
                if (bankingtransactionobject.IRAAccount.First() != null)
                {
                    IRA ira = bankingtransactionobject.IRAAccount.First();
                    Decimal pendingbalance = ira.PendingBalance;
                    ira.PendingBalance = 0;
                    ira.Balance = pendingbalance + ira.Balance;
                    db.SaveChanges();
                }
                //SendEmail()
                String emailsubject = "A message from Longhorn Bank";
                String emailbody= "Your deposit of $" + bankingtransactionobject.Amount + " has been approved! You may now use your funds. \n\n Thank you for banking with Longhorn Bank";
                String emailstring= PrepEmail(bankingtransactionobject);
                SendEmail(emailstring, emailsubject, emailbody);
            }
            db.SaveChanges();
            //Begin Copied Code
            List<BankingTransaction> SelectedDepositsOrig = new List<BankingTransaction>();
            SelectedDepositsOrig = db.BankingTransaction.Where(b => b.ApprovalStatus == ApprovedorNeedsApproval.Approved).ToList();
            List<BankingTransaction> SelectedDeposits = new List<BankingTransaction>();

            foreach (BankingTransaction depo in SelectedDepositsOrig)
            {
                if (depo.ApprovalStatus == ApprovedorNeedsApproval.NeedsApproval) { SelectedDeposits.Add(depo); }
            }
            SelectedDeposits.OrderBy(b => b.TransactionDate);
            List<DepositApprovalViewModel> ListOfDepositApprovalViewModels = new List<DepositApprovalViewModel>();
            db.SaveChanges();
            foreach (var deposit in SelectedDeposits)
            {
                DepositApprovalViewModel depoviewmodel = new DepositApprovalViewModel();
                depoviewmodel.BankingTransaction = deposit;
                if (deposit.CheckingAccount.FirstOrDefault() != null)
                {
                    Checking CA = deposit.CheckingAccount.FirstOrDefault();

                    depoviewmodel.FName = CA.Customer.FName;
                    depoviewmodel.LName = CA.Customer.LName;
                }
                if (deposit.SavingsAccount.FirstOrDefault() != null)
                {
                    Saving SA = deposit.SavingsAccount.FirstOrDefault();

                    depoviewmodel.FName = SA.Customer.FName;
                    depoviewmodel.LName = SA.Customer.LName;
                }
                if (deposit.IRAAccount.FirstOrDefault() != null)
                {
                    IRA IA = deposit.IRAAccount.FirstOrDefault();

                    depoviewmodel.FName = IA.Customer.FName;
                    depoviewmodel.LName = IA.Customer.LName;
                }
                ListOfDepositApprovalViewModels.Add(depoviewmodel);
            }
            //End Copied Code
            return View(ListOfDepositApprovalViewModels);
        }
        // GET: Managers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // GET: Managers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Managers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManagerID,FName,LName,StreetAddress,City,State,Zip,EmailAddress,Password,PhoneNumber,SSN,ActiveStatus,FiredStatus")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                db.Managers.Add(manager);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manager);
        }

        // GET: Managers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ManagerID,FName,LName,StreetAddress,City,State,Zip,EmailAddress,Password,PhoneNumber,SSN,ActiveStatus,FiredStatus")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manager);
        }

        // GET: Managers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manager manager = db.Managers.Find(id);
            db.Managers.Remove(manager);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //FreezeEmployees GET
        public ActionResult FreezeEmployees()
        {
            //IEnumerable<Employee> EmployeeList = db.Employees.ToList();
            List<Employee> EmployeeList= db.Employees.ToList();
            return View(EmployeeList);
        }
        //FreezeEmployees POST
        [HttpPost]
        public ActionResult FreezeEmployees(Boolean Freeze, String EmployeeID)
        {
            String idstring = Convert.ToString(EmployeeID);
            Employee FreezingEmployee= db.Employees.Find(idstring);
            FreezingEmployee.ActiveStatus = Freeze;
            db.SaveChanges();
            ViewBag.ConfirmationMessage = "You have successfully frozen " + FreezingEmployee.FName + " " + FreezingEmployee.LName + "'s account.";
            List<Employee> EmployeeList = db.Employees.ToList();
            return View(EmployeeList);
        }
        public ActionResult FreezeCustomers()
        {
            List<AppUser> CustomerList = db.Users.ToList();
            //foreach (AppUser Customer in CustomerList.ToList())
            //{
              //  if (Customer.ActiveStatus) {CustomerList.Remove(Customer);}
            //}
            return View(CustomerList);
        }
        [HttpPost]
        public ActionResult FreezeCustomers(String CustomerID, Boolean Freeze)
        {

            var query = from fc in db.Users
                        where fc.Id == CustomerID
                        select fc;

            AppUser FreezingCustomer = query.FirstOrDefault();

            if (CustomerID == null)
            {
                RedirectToAction("Portal", "Home");
            }

            FreezingCustomer.ActiveStatus = Freeze;
            db.Entry(FreezingCustomer).State = EntityState.Modified;
            db.SaveChanges();
            if (FreezingCustomer.ActiveStatus == false) { ViewBag.ConfirmationMessage = FreezingCustomer.FName + " " + FreezingCustomer.LName + "'s account is now unfrozen. "; }
            if (FreezingCustomer.ActiveStatus) { ViewBag.ConfirmationMessage = FreezingCustomer.FName + " " + FreezingCustomer.LName + "'s account is now frozen"; }
            List<AppUser> CustomerList = db.Users.ToList();
            //foreach (AppUser Customer in CustomerList.ToList())
            //{
              //  if (Customer.ActiveStatus) { CustomerList.Remove(Customer); }
            //}
            return View(CustomerList);

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
