using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GymApp.Models;

namespace GymApp.Controllers
{
    public class EgresosController : Controller
    {
        private dbGymEntities db = new dbGymEntities();

        // GET: Egresos
        public async Task<ActionResult> Index()
        {
            return View(await db.Egresos.ToListAsync());
        }
        [HttpPost]
        public ActionResult Index(DateTime StartDate, DateTime StartEnd)
        {
            if (StartEnd == null || StartDate == null) return View(db.Egresos.ToList());
            var ingresos = (from u in db.Egresos where u.Fecha >= StartDate && u.Fecha <= StartEnd orderby u.Fecha select u).ToList();
            return View(ingresos);
        }

        // GET: Egresos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Egresos egresos = await db.Egresos.FindAsync(id);
            if (egresos == null)
            {
                return HttpNotFound();
            }
            return View(egresos);
        }

        // GET: Egresos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Egresos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Nombre,Descripcion,Monto,Fecha")] Egresos egresos)
        {
            if (ModelState.IsValid)
            {
                db.Egresos.Add(egresos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(egresos);
        }

        // GET: Egresos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Egresos egresos = await db.Egresos.FindAsync(id);
            if (egresos == null)
            {
                return HttpNotFound();
            }
            return View(egresos);
        }

        // POST: Egresos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Nombre,Descripcion,Monto,Fecha")] Egresos egresos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(egresos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(egresos);
        }

        // GET: Egresos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Egresos egresos = await db.Egresos.FindAsync(id);
            if (egresos == null)
            {
                return HttpNotFound();
            }
            return View(egresos);
        }

        // POST: Egresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Egresos egresos = await db.Egresos.FindAsync(id);
            db.Egresos.Remove(egresos);
            await db.SaveChangesAsync();
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
