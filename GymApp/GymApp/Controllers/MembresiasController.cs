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
    [Authorize(Roles ="Administrador")]
    public class MembresiasController : Controller
    {
        private dbGymEntities db = new dbGymEntities();

        // GET: Membresias
        public ActionResult Index()
        {
            return View(db.Membresias.ToList());
        }

        // GET: Membresias/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Membresias membresias = db.Membresias.Find(id);
            if (membresias == null)
            {
                return HttpNotFound();
            }
            return View(membresias);
        }

        // GET: Membresias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Membresias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre,Costo,Turno")] Membresias membresias)
        {
            if (ModelState.IsValid)
            {
                db.Membresias.Add(membresias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(membresias);
        }

        // GET: Membresias/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Membresias membresias = db.Membresias.Find(id);
            if (membresias == null)
            {
                return HttpNotFound();
            }
            return View(membresias);
        }

        // POST: Membresias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Nombre,Costo,Turno")] Membresias membresias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(membresias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(membresias);
        }

        // GET: Membresias/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Membresias membresias = db.Membresias.Find(id);
            if (membresias == null)
            {
                return HttpNotFound();
            }
            return View(membresias);
        }

        // POST: Membresias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Membresias membresias = db.Membresias.Find(id);
            db.Membresias.Remove(membresias);
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
