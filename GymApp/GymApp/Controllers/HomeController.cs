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
        [Authorize(Roles ="Normal")]
        public ActionResult IndexUser()
        {
            return View();
        }
        [Authorize(Roles = "Normal")]
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
            var diarestante = x.Days;

            if ((diarestante) < 0){
                ViewBag.restante = "Membresía Vencida";
                Response.Write("<script text/javascript>alert('Membresía Vencida')</script>");

            }
            else {
                if (diarestante <= 7)
                {
                 

                    Response.Write("<script text/javascript>alert('Te quedan "+ diarestante+" dias restantes ')</script>");
                }
                ViewBag.restante = diarestante + "Día(s) Restante";
            }
            return PartialView(usuario);
            
        }
        
        [Authorize(Roles = "Normal")]
        public ActionResult _sectionRutina()
        {
            var currentime = DateTime.Now.Date;
            Rutina rutinadiaria = (from u in db.Rutina
                                   where (DbFunctions.TruncateTime(u.fini) == currentime)
                                   select u).FirstOrDefault();
       
            return PartialView(rutinadiaria);

        }
        private static List<Aviso> avisos;

        [Authorize(Roles = "Normal")]
        public ActionResult _sectionAvisos()
        {

            return PartialView(db.Aviso.Where(x=> x.ffin >= DateTime.Now));

        }


        [Authorize(Roles = "Normal")]
        public ActionResult _sectionRendimientos()
        {

            var result = db.RendimientoEjercicio.Where(u => u.AspNetUsers.UserName == User.Identity.Name)
                .GroupBy(x => x.ejercicioID, (key, g) =>
                g.OrderByDescending(e => e.Id)
                .FirstOrDefault());
            // esta linea muestra todos los ejercicios por usuario
            // var rendimientoEjercicio = (from u in db.RendimientoEjercicio where u.AspNetUsers.UserName == User.Identity.Name select u); 
            ViewBag.ejercicioID = new SelectList(db.Ejercicio, "Id", "Nombre");

            return PartialView(result.ToList());

        }

    }
}