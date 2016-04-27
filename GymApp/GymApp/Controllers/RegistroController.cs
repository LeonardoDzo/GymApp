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
    public class RegistroController : Controller
    {
        private dbGymEntities db = new dbGymEntities();

        // GET: Registro
        public ActionResult Index()
        {
            var registro = db.AspNetUsers;
            return View(registro.ToList());
        }

        // GET: Registro/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var registro = (from u in db.AspNetUsers where u.accessControl == id select u).First();
            if (registro == null)
            {
                return HttpNotFound();
            }
            return View(registro);
        }

        // GET: Registro/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Registro/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int accessControUser)
        {
            if (ModelState.IsValid)
            {
                var register = new Registro();
                register.date = System.DateTime.Now;
                register.idUser = (from u in db.AspNetUsers where u.accessControl == accessControUser select u.Id).FirstOrDefault();
                register.accessControUser = accessControUser;
                if(register.idUser!= null)
                {
                    db.Registro.Add(register);
                    db.SaveChanges();
                }
                
                return RedirectToAction("Create");
            }

            
            return View();
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
