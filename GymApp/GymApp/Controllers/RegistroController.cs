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
    [Authorize (Roles = "Administrador")]
    public class RegistroController : Controller
    {
        private dbGymEntities db = new dbGymEntities();
       
   
        [HttpGet]
        public ActionResult Index(string Nombre="", string Apellidos="")
        {
            var registro = db.Registro.Include(x => x.AspNetUsers);
            if(Nombre!="")
                registro = registro.Where(x => x.AspNetUsers.FirstName == Nombre);

            if (Apellidos!="")
                registro = registro.Where(x => x.AspNetUsers.LastName == Apellidos);


            return View(registro);
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
        public ActionResult Create(string error = "", string persona= "")
        {
            ViewBag.persona = persona;
            ViewBag.error = error;
            return View();
        }

        // POST: Registro/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int accessControUser)
        {
            var verifica = (db.AspNetUsers.Count(x => x.accessControl == accessControUser))>=1 ? true: false;
            if (ModelState.IsValid && verifica)
            {
                Response.Write(@"<script language = 'javascript'>alert('Numero de Control de accesso Correcto, Registrado') </script>");
                var register = new Registro();
                register.date = System.DateTime.Now;
                register.idUser = (from u in db.AspNetUsers where u.accessControl == accessControUser select u.Id).FirstOrDefault();
                register.accessControUser = accessControUser;
                var user = (from u in db.AspNetUsers where u.accessControl == accessControUser select u).FirstOrDefault();
                if (register.idUser!= null)
                {
                    db.Registro.Add(register);
                    db.SaveChanges();
                }
               
                return RedirectToAction("Create", new { error= "registrado", persona = user.FirstName +" " +user.LastName});
            }else
            {
                return RedirectToAction("Create", new { error = "error" });
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
