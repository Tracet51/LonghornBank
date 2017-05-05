using System;
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
    public class StockMarketsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: StockMarkets
        // veiw every stock
        public ActionResult Index()
        {
            return View(db.StockMarket.ToList());
        }

        // GET: StockMarkets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockMarket stockMarket = db.StockMarket.Find(id);
            if (stockMarket == null)
            {
                return HttpNotFound();
            }
            return View(stockMarket);
        }

        [Authorize(Roles ="Manager")]
        // GET: StockMarkets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StockMarkets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StockMarketID,CompanyName,Ticker,StockType,Fee")] StockMarket stockMarket)
        {
            if (ModelState.IsValid)
            {
                db.StockMarket.Add(stockMarket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(stockMarket);
        }

        // GET: StockMarkets/Edit/5
        [Authorize(Roles ="Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockMarket stockMarket = db.StockMarket.Find(id);
            if (stockMarket == null)
            {
                return HttpNotFound();
            }
            return View(stockMarket);
        }

        // POST: StockMarkets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles ="Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StockMarketID,CompanyName,Ticker,StockType,Fee,StockPrice")] StockMarket stockMarket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockMarket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stockMarket);
        }

        // GET: StockMarkets/Delete/5
        [Authorize(Roles ="Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockMarket stockMarket = db.StockMarket.Find(id);
            if (stockMarket == null)
            {
                return HttpNotFound();
            }
            return View(stockMarket);
        }

        // POST: StockMarkets/Delete/5
        [Authorize(Roles ="Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StockMarket stockMarket = db.StockMarket.Find(id);
            db.StockMarket.Remove(stockMarket);
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
