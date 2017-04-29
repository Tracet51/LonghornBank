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
        public ActionResult Portal()
        {
            return View();
        }
        public ActionResult ViewCustomers()
        {
            return View(db.Users.ToList());
        }
        // GET: Employees
        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }
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

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

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
        public ActionResult Create([Bind(Include = "EmployeeID,FName,LName,StreetAddress,City,State,Zip,EmailAddress,Password,PhoneNumber,SSN,ActiveStatus,FiredStatus")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }
        //GET
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

                var passwordRemove = await UserManager.RemovePasswordAsync(user.Id);

                var passwordUpdate = await UserManager.AddPasswordAsync(user.Id, NewPasswordUnhashed);

                var result = await db.SaveChangesAsync();
                
                db.SaveChanges();
                return RedirectToAction("ViewCustomers");
            }
            return View(customer);
          }
        //chnge password get
        public ActionResult ChangePassword()
        {
            return View();
        }
        
        // GET: Employees/Edit/5
        public ActionResult Edit(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,FName,LName,StreetAddress,City,State,Zip,EmailAddress,Password,PhoneNumber,SSN,ActiveStatus,FiredStatus")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
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
