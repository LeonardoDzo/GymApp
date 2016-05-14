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
    public class AvisosController : Controller
    {
        private dbGymEntities db = new dbGymEntities();

        // GET: Avisos
        public ActionResult Index()
        {
            return View(db.Aviso.ToList());
        }

        // GET: Avisos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aviso aviso = db.Aviso.Find(id);
            if (aviso == null)
            {
                return HttpNotFound();
            }
            return View(aviso);
        }

        // GET: Avisos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Avisos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Aviso1,ffin")] Aviso aviso)
        {
            if (ModelState.IsValid)
            {
                db.Aviso.Add(aviso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aviso);
        }

        // GET: Avisos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aviso aviso = db.Aviso.Find(id);
            if (aviso == null)
            {
                return HttpNotFound();
            }
            return View(aviso);
        }

        // POST: Avisos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Aviso1,ffin")] Aviso aviso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aviso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aviso);
        }

        // GET: Avisos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aviso aviso = db.Aviso.Find(id);
            if (aviso == null)
            {
                return HttpNotFound();
            }
            return View(aviso);
        }

        // POST: Avisos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Aviso aviso = db.Aviso.Find(id);
            db.Aviso.Remove(aviso);
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
