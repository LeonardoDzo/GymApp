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
    [Authorize(Roles = "Normal")]

    public class RendimientoEjerciciosController : Controller
    {
        private dbGymEntities db = new dbGymEntities();


        // GET: RendimientoEjercicios
        public ActionResult Index()
        {
        
            var result = db.RendimientoEjercicio.Where(u => u.AspNetUsers.UserName == User.Identity.Name)
                .GroupBy(x => x.ejercicioID, (key, g) => 
                g.OrderByDescending(e => e.Id)
                .FirstOrDefault());
            // esta linea muestra todos los ejercicios por usuario
            // var rendimientoEjercicio = (from u in db.RendimientoEjercicio where u.AspNetUsers.UserName == User.Identity.Name select u); 
            return View(result.ToList());
        }

        // GET: RendimientoEjercicios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RendimientoEjercicio rendimientoEjercicio = db.RendimientoEjercicio.Find(id);
            if (rendimientoEjercicio == null)
            {
                return HttpNotFound();
            }
            return View(rendimientoEjercicio);
        }

        // GET: RendimientoEjercicios/Create
        public ActionResult Create()
        {
            ViewBag.userID = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.ejercicioID = new SelectList(db.Ejercicio, "Id", "Nombre");
            return View();
        }

        // POST: RendimientoEjercicios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        private static List<UserViewModels> usuarios;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ejercicioID,userID,PR,fecha")] RendimientoEjercicio rendimientoEjercicio)
        {


            rendimientoEjercicio.userID = (from u in db.AspNetUsers where u.UserName == User.Identity.Name select u.Id).FirstOrDefault();
            rendimientoEjercicio.fecha = DateTime.Now;


            if (ModelState.IsValid)
            {
                db.RendimientoEjercicio.Add(rendimientoEjercicio);
                db.SaveChanges();
                return RedirectToAction("Index");
            } 

         //   ViewBag.userID = new SelectList(db.AspNetUsers, "Id", "userName", rendimientoEjercicio.userID);
            ViewBag.ejercicioID = new SelectList(db.Ejercicio, "Id", "Nombre", rendimientoEjercicio.ejercicioID);
            return View(rendimientoEjercicio);
        }

        // GET: RendimientoEjercicios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RendimientoEjercicio rendimientoEjercicio = db.RendimientoEjercicio.Find(id);
            if (rendimientoEjercicio == null)
            {
                return HttpNotFound();
            }
            ViewBag.userID = new SelectList(db.AspNetUsers, "Id", "Email", rendimientoEjercicio.userID);
            ViewBag.ejercicioID = new SelectList(db.Ejercicio, "Id", "Nombre", rendimientoEjercicio.ejercicioID);
            return View(rendimientoEjercicio);
        }

        // POST: RendimientoEjercicios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ejercicioID,userID,PR,fecha")] RendimientoEjercicio rendimientoEjercicio)
        {
            rendimientoEjercicio.userID = (from u in db.AspNetUsers where u.UserName == User.Identity.Name select u.Id).FirstOrDefault();
            rendimientoEjercicio.fecha = (from u in db.RendimientoEjercicio where u.Id == rendimientoEjercicio.Id select u.fecha).FirstOrDefault();
            if (ModelState.IsValid)
            {
                db.Entry(rendimientoEjercicio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.userID = new SelectList(db.AspNetUsers, "Id", "Email", rendimientoEjercicio.userID);
            ViewBag.ejercicioID = new SelectList(db.Ejercicio, "Id", "Nombre", rendimientoEjercicio.ejercicioID);
            return View(rendimientoEjercicio);
        }

        // GET: RendimientoEjercicios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RendimientoEjercicio rendimientoEjercicio = db.RendimientoEjercicio.Find(id);
            if (rendimientoEjercicio == null)
            {
                return HttpNotFound();
            }
            return View(rendimientoEjercicio);
        }

        // POST: RendimientoEjercicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RendimientoEjercicio rendimientoEjercicio = db.RendimientoEjercicio.Find(id);
            db.RendimientoEjercicio.Remove(rendimientoEjercicio);
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
