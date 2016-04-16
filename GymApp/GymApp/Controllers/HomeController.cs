using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
    }
}