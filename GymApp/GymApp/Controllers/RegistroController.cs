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
            var registro = db.Registro.Include(x=> x.AspNetUsers);
            return View(registro.ToList());
        }
        [HttpGet]
        public ActionResult Index(string Nombre, string Apellidos)
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
            var verifica = (db.AspNetUsers.Count(x => x.accessControl == accessControUser))>=1 ? true: false;
            if (ModelState.IsValid && verifica)
            {
                Response.Write(@"<script language = 'javascript'>alert('Numero de Control de accesso Correcto, Registrado') </script>");
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
            }else
            {
                Response.Write(@"<script language = 'javascript'>alert('Numero de Control de accesso Incorrecto') </script>");
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
