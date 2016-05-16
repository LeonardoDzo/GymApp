using System.Linq;
using System.Web.Mvc;
using System.Data;
using GymApp.Models;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using HiQPdf;
using System.Collections.Generic;

namespace GymApp.Controllers
{
    public class BalanceController : Controller
    {
        private dbGymEntities db = new dbGymEntities();
        private static List<Balance> lista;
        // GET: Balance
        public ActionResult Index()
        {
            return View();
        }
        private bool existeCategoria(string categoria, string tipo)
        {
            foreach(var i in lista)
            {
                if (categoria == i.nombre && tipo == i.tipo)
                    return false;
            }
            return true;
        }
        public ActionResult _balanceIngresos(System.DateTime? StartDate, System.DateTime? EndDate)
        {
            lista = null;
              lista = new List<Balance>();
              var ingresos = db.Ingresos.ToList();
              if(StartDate!= null && EndDate!= null) ingresos = ingresos.Where(x => x.Fecha >= StartDate && x.Fecha <= EndDate).ToList();

              foreach (var i in ingresos)
              {
                Balance balance = new Balance();
                if (existeCategoria(i.Nombre, "Ingreso"))
                {
                    balance.tipo = "Ingreso";
                    balance.nombre = i.Nombre;
                    balance.total = ingresos.Where(x => x.Nombre == i.Nombre).Sum(x => x.Monto);
                    lista.Add(balance);
                    balance = null;
                }
              }
              ViewBag.Total = lista.Where(x => x.tipo == "Ingreso").Sum(x => x.total);
            return PartialView(lista.Where(x=>x.tipo=="Ingreso"));

        }

        public ActionResult _balanceEgresos(System.DateTime? StartDate, System.DateTime? EndDate)
        {
            lista = null;
            lista = new List<Balance>();
            var egresos = db.Egresos.ToList();
            var ingresos = db.Ingresos.ToList();
            if (StartDate != null && EndDate != null) {
                egresos = db.Egresos.Where(x => x.Fecha >= StartDate && x.Fecha <= EndDate).ToList();
                ingresos = db.Ingresos.Where(x => x.Fecha >= StartDate && x.Fecha <= EndDate).ToList();
            }

            foreach (var i in egresos)
            {
                Balance balance = new Balance();
                if (existeCategoria(i.Nombre, "Egreso"))
                {
                    balance.tipo = "Egreso";
                    balance.nombre = i.Nombre;
                    balance.total = egresos.Where(x => x.Nombre == i.Nombre).Sum(x => x.Monto);
                    lista.Add(balance);
                    balance = null;
                }
            }
            ViewBag.Total = lista.Where(x => x.tipo == "Egreso").Sum(x => x.total);
            ViewBag.Total2 = ingresos.Sum(x => x.Monto)
                           - egresos.Sum(x => x.Monto);
            return PartialView(lista.Where(x => x.tipo == "Egreso"));

        }

        public ActionResult _Ingresos(System.DateTime? StartDate, System.DateTime? EndDate)
        {
            var ingresos = db.Ingresos.ToList();
            ViewBag.Total = ingresos.Sum(x => x.Monto);
            if (StartDate == null || EndDate == null) return View(ingresos);
            ingresos = ingresos.Where(x => x.Fecha >= StartDate && x.Fecha <= EndDate).ToList();
          
            ViewBag.Total = ingresos.Sum(x => x.Monto);
            return PartialView(ingresos);
        }
        public ActionResult _Egresos(System.DateTime? StartDate, System.DateTime? EndDate)
        {
            var egresos = db.Egresos.ToList();
            ViewBag.Total = egresos.Sum(x => x.Monto);
            ViewBag.Total2 = db.Ingresos.Sum(x => x.Monto)
                            - egresos.Sum(x => x.Monto);
            if (StartDate == null || EndDate == null) return View(egresos);
            egresos = egresos.Where(x => x.Fecha >= StartDate && x.Fecha <= EndDate).ToList();

            ViewBag.Total = egresos.Sum(x => x.Monto);
            ViewBag.Total2 = db.Ingresos.Where(x => x.Fecha >= StartDate && x.Fecha <= EndDate).Sum(x => x.Monto)
                            - egresos.Sum(x => x.Monto);
            return PartialView(egresos);
        }
      
     
    }
}