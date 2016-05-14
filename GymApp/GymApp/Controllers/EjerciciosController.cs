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
    [Authorize(Roles = "Administrador, Empleado")]

    public class EjerciciosController : Controller
    {
        private dbGymEntities db = new dbGymEntities();

        // GET: Ejercicios
        public ActionResult Index()
        {
            return View(db.Ejercicio.ToList());
        }

        // GET: Ejercicios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ejercicio ejercicio = db.Ejercicio.Find(id);
            if (ejercicio == null)
            {
                return HttpNotFound();
            }
            return View(ejercicio);
        }

        // GET: Ejercicios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ejercicios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre")] Ejercicio ejercicio)
        {
            if (ModelState.IsValid)
            {
                db.Ejercicio.Add(ejercicio);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e) {
                    Response.Write("<script text/javascript>alert('Ejercicio Repetido')</script>");
                    return View(ejercicio);
                }

            }

            return View(ejercicio);
        }

        // GET: Ejercicios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ejercicio ejercicio = db.Ejercicio.Find(id);
            if (ejercicio == null)
            {
                return HttpNotFound();
            }
            return View(ejercicio);
        }

        // POST: Ejercicios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre")] Ejercicio ejercicio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ejercicio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ejercicio);
        }

        // GET: Ejercicios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ejercicio ejercicio = db.Ejercicio.Find(id);
            if (ejercicio == null)
            {
                return HttpNotFound();
            }
            return View(ejercicio);
        }

        // POST: Ejercicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ejercicio ejercicio = db.Ejercicio.Find(id);
            db.Ejercicio.Remove(ejercicio);
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
