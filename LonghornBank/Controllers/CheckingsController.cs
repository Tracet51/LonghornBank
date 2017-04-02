﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LonghornBank.Dal;
using LonghornBank.Models;

namespace LonghornBank.Controllers
{
    public class CheckingsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Checkings
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.CustomerAccount.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            // Add the Customer to ViewBag to Access information 
            ViewBag.CustomerAccount = customer;

            // Select The Checking Accounts Associated with the customer 
            var CheckingAccountQuery = from ca in db.CheckingAccount
                                       where ca.Customer.CustomerID == customer.CustomerID
                                       select ca;

            // Create list and execute the query 
            List<Checking> CustomerChecking = CheckingAccountQuery.ToList();

            return View(CustomerChecking);
        }

        // GET: Checkings/Details/5
        // ID = checkingID
        public ActionResult Details(int? id)
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

            return View(checking);
        }

        // GET: Checkings/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.CustomerAccount.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = id;
            return View();
        }

        // POST: Checkings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CheckingID, Balance, Name, AccountNumber, Customer_CustomerID")] Checking checking, int? CustomerID)
        {
            
            if (CustomerID == null)
            {
                return HttpNotFound();
            }

            Customer customer = db.CustomerAccount.Find(CustomerID);
            if (customer == null)
            {
                return HttpNotFound();
            }
            

            if (ModelState.IsValid)
            {
                // Pass in the Customer ID
                checking.Customer = customer;

                db.CheckingAccount.Add(checking);
                db.SaveChanges();
                return RedirectToAction("Portal", "Home", new { id = 1});
            }

            return View(checking);
        }

        // GET: Checkings/Edit/5
        public ActionResult Edit(int? id)
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
                                            select ca.Customer.CustomerID;
                                            

                // Execute the Find
                List <Int32> CustomerID = CheckingCustomerQuery.ToList();

                Int32 IntCustomerID = CustomerID[0];

                db.Entry(checking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Portal", "Home", new { id = IntCustomerID});
            }
            return RedirectToAction("Index", "Checkings", new { id = checking.CheckingID });
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
                                        select ca.Customer.CustomerID;

            // Execute the Find
            List<Int32> CustomerID = CheckingCustomerQuery.ToList();

            Int32 IntCustomerID = CustomerID[0];

            Checking checking = db.CheckingAccount.Find(id);
            db.CheckingAccount.Remove(checking);
            db.SaveChanges();
            return RedirectToAction("Portal", "Home", new { id = IntCustomerID});
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
