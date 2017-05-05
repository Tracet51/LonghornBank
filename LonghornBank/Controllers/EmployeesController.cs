using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LonghornBank.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

namespace LonghornBank.Controllers
{
    public class EmployeesController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private AppUserManager _userManager;

        public EmployeesController()
        {
        }

        public EmployeesController(AppUserManager userManager, ApplicationSignInManager signInManager)
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
        [Authorize(Roles ="Employee,Manager")]
        
        public ActionResult Portal()
        {
            var Query = from e in db.Users
                        where e.UserName == User.Identity.Name
                        select e;

            AppUser Employee = Query.FirstOrDefault();

            if (Employee.FiredStatus == true)
            {
                return View("Fired");
            }

            return View();
        }
        [Authorize(Roles = "Employee,Manager")]
        public ActionResult ViewCustomers()
        {
            return View(db.Users.ToList());
        }
        [Authorize(Roles = "Employee,Manager")]
        public ActionResult CustomerDetails(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser customer = db.Users.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Employees/Details/
        // Show the employee the details of his profile
        [Authorize(Roles = "Employee,Manager")]
        public ActionResult Details()
        {
            var QueryEmployee = from e in db.Users
                                where e.UserName == User.Identity.Name
                                select e;

            AppUser employee = QueryEmployee.FirstOrDefault();

            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }
        [Authorize(Roles = "Employee,Manager")]
        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee,Manager")]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeID,FName,LName,StreetAddress,City,State,Zip,EmailAddress,Password,PhoneNumber,SSN,ActiveStatus,FiredStatus")] Employee employee)
        {
            if (ModelState.IsValid || !ModelState.IsValid)
            {
                ViewBag.Confirmation = "You have successfully created an employee.";
                var user = new AppUser
                {
                    UserName = employee.EmailAddress,
                    Email = employee.EmailAddress,
                    FName = employee.FName,
                    LName = employee.LName,
                    City = employee.City,
                    DOB = Convert.ToDateTime("1/1/1800"),
                    MiddleInitial = employee.MiddleInitial,
                    State = employee.State,
                    StreetAddress = employee.StreetAddress,
                    Zip = employee.Zip,
                    PhoneNumber = employee.PhoneNumber,
                    FiredStatus = false,
                    ActiveStatus = true
                };

                // Create the user
                var result = await UserManager.CreateAsync(user, employee.Password);

                // Add the user to the employee role 
                await UserManager.AddToRoleAsync(user.Id, "Employee");

                db.SaveChanges();
                return RedirectToAction("Index", "Managers");
            }

            return View(employee);
        }
        //GET
        [Authorize(Roles = "Employee,Manager")]
        public ActionResult EditCustomers(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser customer = db.Users.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //EDIT Customers POST
        [HttpPost]
        [Authorize(Roles = "Employee,Manager")]
        public async Task<ActionResult> EditCustomers([Bind(Include = "Id,FName,LName,StreetAddress,City,State,Zip,Email,PhoneNumber,DOB,ActiveStatus")] AppUser customer, String NewPasswordUnhashed)
        {
            
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(customer.Id);

                // Set all the property 
                user.City = customer.City;
                user.DOB = customer.DOB;
                user.FName = customer.FName;
                user.LName = customer.LName;
                user.MiddleInitial = customer.MiddleInitial;
                user.PhoneNumber = customer.PhoneNumber;
                user.ActiveStatus = customer.ActiveStatus;
                user.StreetAddress = customer.StreetAddress;
                user.Zip = customer.Zip;

                var update = await UserManager.UpdateAsync(user);

                if (NewPasswordUnhashed != null)
                {
                    var passwordRemove = await UserManager.RemovePasswordAsync(user.Id);

                    var passwordUpdate = await UserManager.AddPasswordAsync(user.Id, NewPasswordUnhashed);
                }
                
                var result = await db.SaveChangesAsync();
                
                db.SaveChanges();
                return RedirectToAction("ViewCustomers");
            }
            return View(customer);
          }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Employee,Manager")]
        public ActionResult Edit()
        {
            var employeequery = from e in db.Users
                               where e.UserName == User.Identity.Name
                               select e;

            AppUser Employee = employeequery.FirstOrDefault();

            if (Employee == null)
            {
                return HttpNotFound();
            }

            // set the view model 
            EmployeeEditEmployee EditEmployee = new EmployeeEditEmployee
            {
                City = Employee.City,
                PhoneNumber = Employee.PhoneNumber,
                State = Employee.State,
                StreetAddress = Employee.StreetAddress,
                Zip = Employee.Zip
            };
            return View();
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee,Manager")]
        public async Task<ActionResult> Edit(EmployeeEditEmployee employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            // Get the Employee 
            var Employee = await UserManager.FindByNameAsync(User.Identity.Name);

            // Update the Employee
            Employee.City = employee.City;
            Employee.PhoneNumber = employee.PhoneNumber;
            Employee.State = employee.State;
            Employee.StreetAddress = employee.StreetAddress;
            Employee.Zip = employee.Zip;

            // Update the employee 
            var update = await UserManager.UpdateAsync(Employee);

            return RedirectToAction("Portal", "Employees");
        }

        // GET: Employees/ChangePassword
        // return page to edit the employee password
        [Authorize(Roles = "Employee,Manager")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: Employees/ChangePassword
        // Post method to change the password 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee,Manager")]
        public async Task<ActionResult> ChangePassword(EmployeeChangePassword NewPassword)
        {
            // Check to make sure the passwords are the same
            if (NewPassword.Password != NewPassword.ConfirmPassword)
            {
                return View();
            }

            // Get the user 
            var Employee = await UserManager.FindAsync(User.Identity.Name, NewPassword.OldPassword);


            // If the employee password does not match
            if (Employee == null)
            {
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View(NewPassword);
            }

            // Remove the old password
            var PasswordRemove = await UserManager.RemovePasswordAsync(Employee.Id);

            // update the new password
            var PasswordUpdate = await UserManager.AddPasswordAsync(Employee.Id, NewPassword.Password);

            // Check to see if successful 
            if (PasswordUpdate.Succeeded)
            {
                return RedirectToAction("Portal", "Employees");
            }

            // If the password failed
            return View();
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
