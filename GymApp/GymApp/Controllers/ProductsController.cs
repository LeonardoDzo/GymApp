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
    public class ProductsController : Controller
    {
        private dbGymEntities db = new dbGymEntities();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            return View(await db.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = await db.Products.FindAsync(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Name,Price,Quantity,Description")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(products);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(products);
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = await db.Products.FindAsync(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Name,Price,Quantity,Description")] Products products, int Agregar)
        {
            if (ModelState.IsValid && Agregar>=0)
            {
                var pro = products;
                pro.Quantity += Agregar;
                db.Entry(pro).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = await db.Products.FindAsync(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Products products = await db.Products.FindAsync(id);
            db.Products.Remove(products);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Products/Venta
        public async Task<ActionResult> Venta(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = await db.Products.FindAsync(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Venta([Bind(Include = "id,Name,Price,Quantity,Description")] Products products)
        {
            Ingresos nuevo = new Ingresos();
           
            int cant = (from u in db.Products where u.id == products.id select u.Quantity).First();
            if (ModelState.IsValid && products.Quantity <= cant && products.Quantity > 0)
            {
                nuevo.Nombre = "Producto";
                nuevo.Monto = products.Quantity * products.Price;
                nuevo.Descripcion = products.Name+ products.Description+ "por la cantidad de: " + products.Quantity;
                nuevo.Fecha = System.DateTime.Now;
                
                db.Ingresos.Add(nuevo);
                db.SaveChanges();

                modificaProducto(products.id, products.Quantity);

                return RedirectToAction("Index");
            }
            return View(products);
        }
        public void modificaProducto(int id,int cant)
        {
            var producto = (from u in db.Products where u.id == id select u).First();
            producto.Quantity = producto.Quantity - cant;
            db.Entry(producto).State = EntityState.Modified;
            db.SaveChanges();
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
