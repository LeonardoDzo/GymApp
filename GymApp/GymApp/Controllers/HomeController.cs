using GymApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymApp.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            if (User.IsInRole("Normal")){
                return View("IndexUser");
            }else  return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Gimnasio que utiliza un sistema de acondicionamiento físico y \n" +
               " fuerza basado en ejercicios constantemente variados, ejecutados a alta intensidad. ";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Kinetic Cross Training";
            
            return View();
        }

        private dbGymEntities db = new dbGymEntities();
        private static List<UserViewModels> usuarios;
        public ActionResult IndexUser()
        {
            return View();
        }

        public ActionResult _sectionUser()
        {
            UserController uC = new UserController();
            List<ICollection<AspNetUsers>> lista;

            lista = (from u in db.AspNetRoles where u.Name == "Normal" select u.AspNetUsers).ToList();


            usuarios = uC.obtenerUsuarios(lista);
            var username = User.Identity.Name;
            
            
            UserViewModels usuario = (from u in usuarios where  u.UserName == username select u).FirstOrDefault();
            ViewBag.ffn = usuario.ffin.ToShortDateString();
            var ffin = usuario.ffin.ToShortDateString();
            var today = DateTime.Now.ToShortDateString();
            TimeSpan x = DateTime.Parse(ffin) - DateTime.Parse(today);
            if ((x.Days) < 0){
                ViewBag.restante = "Membresía Vencida";
            }else { 
            ViewBag.restante = x.Days + "Día(s) Restante";
            }
            return PartialView(usuario);
            
        }
        // private static List<Rutina> rutinadiaria;
        [Authorize(Roles = "Normal")]
        public ActionResult _sectionRutina()
        {
            //var currenttime = DbFunctions.TruncateTime(DateTime.Now.Date);
            var currentime = DateTime.Now.Date;
            Rutina rutinadiaria = (from u in db.Rutina
                                   where (DbFunctions.TruncateTime(u.fini) == currentime)
                                   select u).FirstOrDefault();
            //   Rutina rutinadiaria = (from u in db.Rutina where u.fini.Value.ToShortDateString()== DateTime.Today.ToShortDateString() select u).FirstOrDefault();
            //db.Rutina.Where(x => DbFunctions.TruncateTime(x.fini) == DateTime.Now)
            //   var lista = db.EjercicioRutina.Where(x => x.Fecha.Date == DateTime.Now.Date);
            return PartialView(rutinadiaria);

        }
        private static List<Aviso> avisos;

        [Authorize(Roles = "Normal")]
        public ActionResult _sectionAvisos()
        {
          //  avisos = (from u in db.Aviso select u).ToList();

            //avisos = (from u in db.Aviso where u.ffin >= DateTime.Now select u).ToList();

            return PartialView(db.Aviso.Where(x=> x.ffin >= DateTime.Now));

        }

    }
}