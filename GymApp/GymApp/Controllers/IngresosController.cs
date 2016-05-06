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
    [Authorize (Roles = "Administrador")]
    public class IngresosController : Controller
    {
        private dbGymEntities db = new dbGymEntities();

        // GET: Ingresos
        public ActionResult Index()
        { 
            return View( db.Ingresos.ToList());
        }
        [HttpGet]
        public ActionResult Index(System.DateTime? StartDate, System.DateTime? StartEnd, string Tipo="")
        {
            ViewBag.tipo = new SelectList((from u in db.Categoria select u.Nombre).ToList());
            var ingresos = db.Ingresos.ToList();
            if (Tipo != "")
                ingresos = ingresos.Where(x => x.Nombre == Tipo).OrderBy(x=> x.Fecha).ToList() ;
           
               
            try
            {
                ViewBag.totalingresos = (ingresos.Sum(c => c.Monto));
                if (StartDate == null || StartDate == null) return View(ingresos);
                ingresos = ingresos.Where(x => x.Fecha >= StartDate && x.Fecha <= StartEnd).OrderBy(x => x.Fecha).ToList();
                ViewBag.totalingresos = (ingresos.Sum(c => c.Monto));
                return View(ingresos);
            }
            catch (Exception e)
            {
                return View(ingresos);
            }
           
            
        }

        // GET: Ingresos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingresos ingresos = await db.Ingresos.FindAsync(id);
            if (ingresos == null)
            {
                return HttpNotFound();
            }
            return View(ingresos);
        }

        // GET: Ingresos/Create
        public ActionResult Create()
        {
            ViewBag.tipo = new SelectList((from u in db.Categoria select u.Nombre).ToList());
            return View();
        }

        // POST: Ingresos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Nombre,Descripcion,Monto,Fecha")] Ingresos ingresos)
        {
            ViewBag.tipo = new SelectList((from u in db.Categoria select u.Nombre).ToList());
            if (ModelState.IsValid)
            {
                ingresos.Fecha = System.DateTime.Now;
                db.Ingresos.Add(ingresos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(ingresos);
        }

        // GET: Ingresos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.tipo = new SelectList((from u in db.Categoria select u.Nombre).ToList());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingresos ingresos = await db.Ingresos.FindAsync(id);
            if (ingresos == null)
            {
                return HttpNotFound();
            }
            return View(ingresos);
        }

        // POST: Ingresos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Nombre,Descripcion,Monto,Fecha")] Ingresos ingresos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ingresos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ingresos);
        }

        // GET: Ingresos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingresos ingresos = await db.Ingresos.FindAsync(id);
            if (ingresos == null)
            {
                return HttpNotFound();
            }
            return View(ingresos);
        }

        // POST: Ingresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Ingresos ingresos = await db.Ingresos.FindAsync(id);
            db.Ingresos.Remove(ingresos);
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
