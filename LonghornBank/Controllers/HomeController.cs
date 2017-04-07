﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LonghornBank.Models;

namespace LonghornBank.Controllers
{
    public class HomeController : Controller
    {
        // App Db context 
        private AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/Create 
        // Page where the newly created user chooses what account to make
        public ActionResult Create(string id)
        {
            AppUser customer = db.Users.Find(id);
            return View(customer);
        }


        // GET: Home/Portal
        public ActionResult Portal(String id)
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

            // Find the Checking Accounts Associated
            var CheckingQuery = from ca in db.CheckingAccount
                                where ca.Customer.Id == id
                                select ca;

            // Convert to a list and execute
            List <Checking> CustomerCheckings = CheckingQuery.ToList();

            // Send to the Viewbag
            ViewBag.CheckingAccounts = CustomerCheckings;

            // Find the Savings Accounts Associated 
            var SavingsQuery = from sa in db.SavingsAccount
                               where sa.Customer.Id == id
                               select sa;

            // Convert to a list and execute 
            List<Saving> CustomerSavings = SavingsQuery.ToList();
            ViewBag.SavingsAccounts = CustomerSavings;

            // Find the IRA Accounts Associated
            var IRAQuery = from IR in db.IRAAccount
                               where IR.Customer.Id == id
                               select IR;

            List<IRA> CustomerIRA = IRAQuery.ToList();
            ViewBag.IRAAccounts = CustomerIRA;

            return View(customer);
        }
    }
}