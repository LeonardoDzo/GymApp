using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GymApp.Models;

namespace GymApp.Controllers
{
    public class sendersController : Controller
    {
        private dbGymEntities db = new dbGymEntities();

        // GET: senders
        public ActionResult Index()
        {
            return View(db.sender.ToList());
        }

        // GET: senders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sender sender = db.sender.Find(id);
            if (sender == null)
            {
                return HttpNotFound();
            }
            return View(sender);
        }

        // GET: senders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: senders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sentaccount,pass,Id")] sender sender)
        {
            if (ModelState.IsValid)
            {
                db.sender.Add(sender);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sender);
        }

        // GET: senders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sender sender = db.sender.Find(id);
            if (sender == null)
            {
                return HttpNotFound();
            }
            return View(sender);
        }

        // POST: senders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sentaccount,pass,Id")] sender sender)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sender).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sender);
        }

        // GET: senders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sender sender = db.sender.Find(id);
            if (sender == null)
            {
                return HttpNotFound();
            }
            return View(sender);
        }

        // POST: senders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sender sender = db.sender.Find(id);
            db.sender.Remove(sender);
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
