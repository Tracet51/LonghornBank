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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;


namespace LonghornBank.Controllers
{
    public class ManagersController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private AppUserManager _userManager;

        public ManagersController()
        {
        }

        public ManagersController(AppUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

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
        //Get for Fire Employees
        public ActionResult FireEmployees()
        {
            var QueryRole = from ri in db.Roles
                            where ri.Name == "Employee"
                            select ri.Id;

            String RoleId = QueryRole.FirstOrDefault();

            List<AppUser> Employees = db.Users
                .Where(x => x.Roles.Select(y => y.RoleId).Contains(RoleId))
                .ToList();

            return View(Employees);
        }
        //Post for Fire Employees
        [HttpPost]
        public ActionResult FireEmployees(String EmployeeID, Boolean Fire)
        {
            if (Fire)
            {
                AppUser employee = db.Users.Find(EmployeeID);
                employee.FiredStatus = true;
                // Update the account 
                //db.Entry(employee).State = EntityState.Modified;
                // Update the Database
                //var update = UserManager.Update(employee);

                var roleRemove = UserManager.RemoveFromRole(employee.Id, "Employee");

                var roleAdd = UserManager.AddToRole(employee.Id, "Fired");

                // Save the databse and set the view
                db.SaveChanges();
                ViewBag.SuccessMessage = "You have successfully fired an employee.";
            }
            if (Fire == false)
            {
                AppUser employee = db.Users.Find(EmployeeID);
                employee.FiredStatus = false;

                // Update the database 
                //var update = UserManager.Update(employee);

                var roleRemove = UserManager.RemoveFromRole(employee.Id, "Fired");

                var roleAdd = UserManager.AddToRole(employee.Id, "Employee");

                // Save the changes and update the database
                db.SaveChanges();
                ViewBag.SuccessMessage = "You have successfully rehired an employee.";
            }

            var QueryRole = from ri in db.Roles
                            where ri.Name == "Employee"
                            select ri.Id;

            String RoleId = QueryRole.FirstOrDefault();

            List<AppUser> Employees = db.Users
               .Where(x => x.Roles.Select(y => y.RoleId).Contains(RoleId))
               .ToList();

            return View(Employees);
        }
        //GET for approve stock accounts
        public ActionResult ApproveStockAccounts()
        {
            var query = from a in db.StockAccount
                        select a;
            query = query.Where(a => a.ApprovalStatus == ApprovedorNeedsApproval.NeedsApproval);
            List<StockAccount> stockaccountlist = query.ToList();
            return View(stockaccountlist);
        }
        //POST for approve 
        [HttpPost]
        public ActionResult ApproveStockAccounts(Int32 StockAccountID, Boolean Approve)
        {
            ViewBag.SuccessMessage = "Your approval failed.";
            if (Approve)
            {
                StockAccount stockaccount = db.StockAccount.Find(StockAccountID);
                stockaccount.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                ViewBag.SuccessMessage = "You have successfully approved a stock account";
                db.SaveChanges();
            }
            var query = from a in db.StockAccount
                        select a;
            query = query.Where(a => a.ApprovalStatus == ApprovedorNeedsApproval.NeedsApproval);
            List<StockAccount> stockaccountlist = query.ToList();
            return View(stockaccountlist);
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
            SelectedDepositsOrig = db.BankingTransaction.Where(b=>b.ApprovalStatus == ApprovedorNeedsApproval.NeedsApproval).ToList();
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
        public ActionResult DepositApproval(Int32 BankingTransactionID)
        {
            Int32 id =Convert.ToInt32(BankingTransactionID);
            if (true)
            {
                BankingTransaction bankingtransactionobject = db.BankingTransaction.Find(id);
                bankingtransactionobject.ApprovalStatus = ApprovedorNeedsApproval.Approved;
                ViewBag.SuccessMessage = "You have successfully approved deposits.";
                if (bankingtransactionobject.CheckingAccount.FirstOrDefault() != null)
                {
                    Checking checking= bankingtransactionobject.CheckingAccount.First();
                    Decimal pendingbalance= checking.PendingBalance;
                    checking.PendingBalance = pendingbalance-bankingtransactionobject.Amount;
                    checking.Balance = bankingtransactionobject.Amount+checking.Balance;
                    db.SaveChanges();
                }
                if (bankingtransactionobject.SavingsAccount.FirstOrDefault() != null)
                {
                    Saving saving = bankingtransactionobject.SavingsAccount.First();
                    Decimal pendingbalance = saving.PendingBalance;
                    saving.PendingBalance = pendingbalance - bankingtransactionobject.Amount;
                    saving.Balance = bankingtransactionobject.Amount + saving.Balance;
                    db.SaveChanges();
                }
                if (bankingtransactionobject.IRAAccount.FirstOrDefault() != null)
                {
                    IRA ira = bankingtransactionobject.IRAAccount.First();
                    Decimal pendingbalance = ira.PendingBalance;
                    ira.PendingBalance = pendingbalance - bankingtransactionobject.Amount;
                    ira.Balance = bankingtransactionobject.Amount + ira.Balance;
                    db.SaveChanges();
                }
                //SendEmail()
                String emailsubject = "A message from Longhorn Bank";
                String emailbody= "Your deposit of $" + bankingtransactionobject.Amount + " has been approved! You may now use your funds. \n\n Thank you for banking with Longhorn Bank";
                String emailstring= PrepEmail(bankingtransactionobject);
                SendEmail(emailstring, emailsubject, emailbody);
            }
            db.SaveChanges();

            //Begin Copied Get 
            List<BankingTransaction> SelectedDepositsOrig = new List<BankingTransaction>();
            SelectedDepositsOrig = db.BankingTransaction.Where(b => b.ApprovalStatus == ApprovedorNeedsApproval.NeedsApproval).ToList();
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
            return RedirectToAction("DepositApproval", "Managers");
            //End Copied Get
        }
        // GET: Managers/Details/
        public ActionResult Details()
        {

            var QueryManger = from m in db.Users
                              where m.UserName == User.Identity.Name
                              select m;
            AppUser manager = QueryManger.FirstOrDefault();

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


        // GET: Managers/Edit/5
        public ActionResult Edit()
        {
            var QueryManger = from m in db.Users
                              where m.UserName == User.Identity.Name
                              select m;
            AppUser manager = QueryManger.FirstOrDefault();

            if (manager == null)
            {
                return HttpNotFound();
            }

            // Build out the view model 
            Models.ManagerEditManager TheEdit = new ManagerEditManager
            {
                City = manager.City,
                PhoneNumber = manager.PhoneNumber,
                State = manager.State,
                StreetAddress = manager.StreetAddress,
                Zip = manager.Zip
            };

            return View(TheEdit);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ManagerEditManager TheEdit)
        {
            // get the manager 
            AppUser Manager = await UserManager.FindByNameAsync(User.Identity.Name);

            if (Manager == null)
            {
                return HttpNotFound();
            }

            // Update the Manager 
            Manager.City = TheEdit.City;
            Manager.PhoneNumber = TheEdit.PhoneNumber;
            Manager.StreetAddress = TheEdit.StreetAddress;
            Manager.Zip = TheEdit.Zip;
            Manager.State = TheEdit.State;

            // Update the Database 
            var upate = await UserManager.UpdateAsync(Manager);

            db.SaveChanges();

            return View("Index", "Managers");
        }


        //FreezeEmployees GET
        public ActionResult FreezeEmployees()
        {
            var QueryRole = from ri in db.Roles
                            where ri.Name == "Employee"
                            select ri.Id;

            String RoleId = QueryRole.FirstOrDefault();

            List<AppUser> Employees = db.Users
                .Where(x => x.Roles.Select(y => y.RoleId).Contains(RoleId))
                .ToList();

            return View(Employees);
        }
        //FreezeEmployees POST
        [HttpPost]
        public ActionResult FreezeEmployees(Boolean Freeze, String EmployeeID)
        {
            String idstring = Convert.ToString(EmployeeID);
            AppUser FreezingEmployee= db.Users.Find(idstring);

            // Set the Status
            FreezingEmployee.ActiveStatus = Freeze;

            // Update the account 
            db.Entry(FreezingEmployee).State = EntityState.Modified;

            // Save the changes and set the confirmation message
            db.SaveChanges();
            ViewBag.ConfirmationMessage = "You have successfully frozen " + FreezingEmployee.FName + " " + FreezingEmployee.LName + "'s account.";


            var QueryRole = from ri in db.Roles
                            where ri.Name == "Employee"
                            select ri.Id;

            String RoleId = QueryRole.FirstOrDefault();

            List<AppUser> Employees = db.Users
                .Where(x => x.Roles.Select(y => y.RoleId).Contains(RoleId))
                .ToList();

            return View(Employees);
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

        // GET: Managers/AllEmployees
        public ActionResult DisplayAllEmployees()
        {
            var roleQuery = from ri in db.Roles
                         where ri.Name == "Employee"
                         select ri.Id;

             String RoleId = roleQuery.FirstOrDefault();

            List<AppUser> Employees = db.Users
                .Where(x => x.Roles.Select(y => y.RoleId).Contains(RoleId))
                .ToList();

            return View(Employees);
        }

        // GET: Mangagers/EditEmployees/5
        public ActionResult EditEmployee(string id)
        {
            if (id == null)
            {
                return View("Index", "Managers");
            }
            // Get the employee 
            AppUser Employee = db.Users.Find(id);

            if (Employee == null)
            {
                return View("Index", "Managers");
            }
            Boolean Status;

            if (UserManager.IsInRole(Employee.Id, "Manager"))
            {
                Status = true;
            }
            else
            {
                Status = false;
            }

            ViewBag.Status = Status;

            return View(Employee);
        }

        // POST: Managers/EditEmployee/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEmployee(AppUser employee, Boolean RoleChange)
        {
            // Get the employee
            var Employee = await UserManager.FindByNameAsync(employee.UserName);

            // Update the Employee
            Employee.City = employee.City;
            Employee.PhoneNumber = employee.PhoneNumber;
            Employee.State = employee.State;
            Employee.StreetAddress = employee.StreetAddress;
            Employee.Zip = employee.Zip;

            // Comitt the update 
            var update = await UserManager.UpdateAsync(Employee);

            if (RoleChange == true)
            {
                Boolean InMangerRole = await (UserManager.IsInRoleAsync(employee.Id, "Manager"));
                if (!InMangerRole)
                {
                    var role = await UserManager.AddToRoleAsync(employee.Id, "Manager");
                }
            }
            else
            {
                Boolean InMangerRole =  await (UserManager.IsInRoleAsync(employee.Id, "Manager"));
                if (InMangerRole == true)
                {
                    var role = await UserManager.RemoveFromRoleAsync(employee.Id, "Manager");
                }
            }

            return RedirectToAction("DisplayAllEmployees", "Managers");
        }

        // GET: Managers/ChangeEmployeePassword
        public ActionResult ChangeEmployeePassword(String id)
        {
            // Get the employee 
            AppUser Employee = db.Users.Find(id);

            // Set the View Model
            ChangeEmployeePassword CEP = new Models.ChangeEmployeePassword
            {
                EmployeeID = Employee.Id,
                EmployeeFName = Employee.FName,
                EmployeeLName = Employee.LName
            };

            return View(CEP);
        }

        // POST: Managers/ChangeEmployeePassword
        // Allows the manager to change an employee password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmployeePassword(ChangeEmployeePassword TheChange)
        {
            // Get the employee
            AppUser Employee = db.Users.Find(TheChange.EmployeeID);

            // Check to see if the passwords are the same 
            if (TheChange.Password != TheChange.ConfirmPassword)
            {
                return View(TheChange);
            }

            var Remove = await UserManager.RemovePasswordAsync(Employee.Id);

            var UpdatePassword = await UserManager.AddPasswordAsync(Employee.Id, TheChange.Password);

            await db.SaveChangesAsync();

            return RedirectToAction("Index", "Managers");
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
