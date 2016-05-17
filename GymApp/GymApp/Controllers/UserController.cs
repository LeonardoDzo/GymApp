using GymApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GymApp.Controllers
{
    [Authorize(Roles ="Administrador, Empleado")]
    public class UserController : Controller
    {
        private dbGymEntities db = new dbGymEntities();
        private static List<UserViewModels> usuarios;
        // GET: User
        public ActionResult Index(DateTime? StartDate, DateTime? EndDate, int vencimiento=0, bool Caducadas= false)
        {
            List<ICollection<AspNetUsers>> lista;
            
            if (User.IsInRole("Administrador"))
                lista = (from u in db.AspNetRoles where u.Name != "Administrador" select u.AspNetUsers).ToList();
            else
                lista = (from u in db.AspNetRoles where u.Name == "Normal" select u.AspNetUsers).ToList();

            usuarios = obtenerUsuarios(lista);
       
            var filtro = usuarios;
            double total = 0;
            int meses = 0;
            if (StartDate != null && EndDate != null)
            {
                meses = Math.Abs((StartDate.Value.Month - EndDate.Value.Month) + 
                    12 * (StartDate.Value.Year - EndDate.Value.Year));
                filtro = (from u in filtro where u.userRol == "Normal" && u.tipoMembresia != "Ninguna" 
                          && u.fInicio >= StartDate && u.fInicio <= EndDate select u).ToList();
            }
                

            if (vencimiento >0 )
                filtro = (from u in filtro where u.userRol == "Normal" && u.tipoMembresia != "Ninguna" 
                          && u.ffin >= DateTime.Now && u.ffin <= DateTime.Now.AddDays(vencimiento)
                          select u).ToList();
            if(Caducadas)
                filtro = (from u in filtro where u.userRol == "Normal" && u.tipoMembresia != "Ninguna"
                          && u.ffin <= DateTime.Now select u).ToList();

            foreach (var i in filtro)
            {
                double monto = (from u in db.Membresias where u.Nombre == i.tipoMembresia select u.Costo).FirstOrDefault();
                meses = Math.Abs((i.fInicio.Month - i.ffin.Month) + 12 * (i.fInicio.Year - i.ffin.Year));
                total += monto * meses;
            }
            ViewBag.SumaMembresias = total;
            
            return View(filtro);
        }
        public List<UserViewModels> obtenerUsuarios(List<ICollection<AspNetUsers>> users)
        {
            int var1 = 1;
            List<UserViewModels> listausuarios = new List<UserViewModels>(); 
            foreach(var item in users.ToList())
            {
                int l = item.Count();
                foreach (var i in item)
                {
                    UserViewModels usuarios = new UserViewModels();
                    usuarios.Id = i.Id;
                    usuarios.UserName = i.UserName;
                    usuarios.Email = i.Email;
                    usuarios.FirstName = i.FirstName;
                    usuarios.LastName = i.LastName;
                    usuarios.PhoneNumber = i.PhoneNumber;
                    usuarios.FechaNacimiento = i.FechaNacimiento;
                    usuarios.userRol = obtenerRol(usuarios.Id);
                    usuarios.accessControl = i.accessControl;
                    int idMem = (from u in db.UserMembresias where u.userid == i.Id select u.tipoMembresia ).FirstOrDefault();

                    if (idMem > 0)
                        usuarios.tipoMembresia = db.Membresias.Find(idMem).Nombre;
                    else
                        usuarios.tipoMembresia = "Ninguna";

                    usuarios.fInicio = (from u in db.UserMembresias where u.userid == i.Id select u.fInicio).FirstOrDefault();
                    usuarios.ffin = (from u in db.UserMembresias where u.userid == i.Id select u.ffin).FirstOrDefault();
                    listausuarios.Add(usuarios);
                    usuarios = null;
                }
                var1++;
                
            }
            
            return listausuarios;
        }
        // GET: User/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //usuarios = obtenerUsuarios((from u in db.AspNetRoles where u.Name != "Administrador" select u.AspNetUsers).ToList());
            UserViewModels usuario = (from u in usuarios where u.Id == id select u).FirstOrDefault();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
           
        }
        private string obtenerRol(string id)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var rolManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var oldUser = userManager.FindById(id);
            var oldRolId = oldUser.Roles.SingleOrDefault().RoleId;
            var oldRolName = db.AspNetRoles.SingleOrDefault(x => x.Id == oldRolId).Name;

            return oldRolName;
        }
        [Authorize(Roles = "Administrador, Empleado")]
        //GET: User/Membresia/?
        public ActionResult Membresia(string id)
        {
          
            UserMembresias membresia = db.UserMembresias.Where(x => x.userid == id).FirstOrDefault();
            if (membresia != null)
            {
                ViewBag.tipoMembresia = new SelectList(db.Membresias, "id", "Nombre", membresia.tipoMembresia);
            }else if(id!=string.Empty)
            {
                membresia = new UserMembresias();
                membresia.tipoMembresia = db.Membresias.ToList().First().id;
                membresia.userid = id;
                db.UserMembresias.Add(membresia);
                db.SaveChanges();
                ViewBag.tipoMembresia = new SelectList(db.Membresias, "id", "Nombre","Ninguna");
            }else
            {
                return RedirectToAction("Index");
            }
            
            if (membresia == null)
            {
                return HttpNotFound();
            }
            return View(membresia);
        }
        [Authorize(Roles = "Administrador, Empleado")]
        [HttpPost]
        public ActionResult Membresia(UserMembresias model)
        {
            try
            {
                var usumem = (from u in db.UserMembresias where u.userid == model.userid select u).SingleOrDefault();

                if (model != null)
                {

                    if (model.tipoMembresia == 0)
                    {

                        db.UserMembresias.Remove(usumem);

                    }
                    else
                    {
                        if (model.fInicio == null || model.ffin == null)
                        {
                            ViewBag.membresias = new SelectList((from u in db.Membresias select u.Nombre).ToList());
                            return View();
                        }
                        usumem.tipoMembresia = model.tipoMembresia;
                        usumem.fInicio = model.fInicio;
                        usumem.ffin = model.ffin;
                        db.Entry(usumem).State = EntityState.Modified;
                        if( usumem.tipoMembresia != model.tipoMembresia || (model.ffin != usumem.ffin && model.fInicio != usumem.fInicio) || model.ffin > model.fInicio)
                        {
                            Ingresos ingreso = new Ingresos();
                            ingreso.Fecha = DateTime.Now;
                            int meses = Math.Abs((model.fInicio.Month - model.ffin.Month) + 12 * (model.fInicio.Year - model.ffin.Year));
                            ingreso.Monto = db.Membresias.Where(x => x.id == model.tipoMembresia).First().Costo * meses;
                            ingreso.Nombre = "Membresia";
                            ingreso.Descripcion = "Se agrego una membresia del usuario " + db.AspNetUsers.Find(usumem.userid).FirstName+" de tipo " 
                                                + " " + db.AspNetUsers.Find(usumem.userid).LastName + " " 
                                                + (from u in db.Membresias where u.id == model.tipoMembresia select u.Nombre).First();
                            db.Ingresos.Add(ingreso);
                            ingreso = null;
                        }
                        
                    }

                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.membresias = new SelectList((from u in db.Membresias select u.Nombre).ToList());
                ViewBag.rol = new SelectList((from u in db.AspNetRoles where u.Name != "Administrador" select u.Name).ToList());
                return View();
            }
        }

        [Authorize (Roles = "Administrador")]
        // GET: User/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.membresias = new SelectList((from u in db.Membresias select u.Nombre).ToList());
            ViewBag.rol = new SelectList((from u in db.AspNetRoles where u.Name != "Administrador" select u.Name).ToList());
           
            UserViewModels usuario = (from u in usuarios where u.Id == id select u).FirstOrDefault();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(UserViewModels model)
        {
            try
            {
                var usuario = (from u in db.AspNetUsers where u.Id == model.Id select u).SingleOrDefault(); 
                var usumem = (from u in db.UserMembresias where u.userid == model.Id select u).SingleOrDefault();

                if (model != null)
                {
                    usuario.Id = model.Id;
                    usuario.Email = model.Email;
                    usuario.FirstName = model.FirstName;
                    usuario.LastName = model.LastName;
                    usuario.PhoneNumber = model.PhoneNumber;
                    usuario.FechaNacimiento = model.FechaNacimiento;

                    var rol = obtenerRol(usuario.Id);
                    ApplicationDbContext context = new ApplicationDbContext();

                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var rolManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                    if (rol!= model.userRol)
                    {
                        userManager.RemoveFromRole(usuario.Id, rol);
                        userManager.AddToRole(usuario.Id, model.userRol);
                    }


                    if (usumem != null && model.userRol=="Normal")
                    {
                        if (model.tipoMembresia == null)
                        {

                            db.UserMembresias.Remove(usumem);

                        }
                        else
                        {
                            if (model.fInicio == null || model.ffin == null)
                            {
                                ViewBag.membresias = new SelectList((from u in db.Membresias select u.Nombre).ToList());
                                ViewBag.rol = new SelectList((from u in db.AspNetRoles where u.Name != "Administrador" select u.Name).ToList());
                                return View();
                            }
                            usumem.tipoMembresia = (from u in db.Membresias where u.Nombre == model.tipoMembresia select u.id).First();
                            usumem.fInicio = model.fInicio;
                            usumem.ffin = model.ffin;
                            db.Entry(usumem).State = EntityState.Modified;
                        }
                       

                    }
                    else if(model.tipoMembresia != null && model.userRol == "Normal")
                    {
                        UserMembresias usuarioMemb = new UserMembresias();
                        usuarioMemb.userid = model.Id;
                        var temp = db.Membresias.Where(x => x.Nombre == model.tipoMembresia).FirstOrDefault();
                        usuarioMemb.tipoMembresia = temp.id;
                        temp = null;
                        if(model.fInicio == null || model.ffin == null)
                        {
                            ViewBag.membresias = new SelectList((from u in db.Membresias select u.Nombre).ToList());
                            ViewBag.rol = new SelectList((from u in db.AspNetRoles where u.Name != "Administrador" select u.Name).ToList());
                            return View();
                        }
                        usuarioMemb.fInicio = model.fInicio;
                        usuarioMemb.ffin = model.ffin;
                        db.UserMembresias.Add(usuarioMemb);
                        db.SaveChanges();
                    }
                    
                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.membresias = new SelectList((from u in db.Membresias select u.Nombre).ToList());
                ViewBag.rol = new SelectList((from u in db.AspNetRoles where u.Name != "Administrador" select u.Name).ToList());
                return View();
            }
        }

        [Authorize (Roles = "Administrador")]
        // GET: User/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //usuarios = obtenerUsuarios((from u in db.AspNetRoles where u.Name != "Administrador" select u.AspNetUsers).ToList());
            UserViewModels usuario = (from u in usuarios where u.Id == id select u).FirstOrDefault();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }
        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                AspNetUsers user = (from u in db.AspNetUsers where u.Id == id select u).SingleOrDefault();
                UserMembresias usumem = (from u in db.UserMembresias where u.userid == id select u).SingleOrDefault();
                var regis = db.Registro.Where(x => x.idUser == id);
                db.AspNetUsers.Remove(user);
                if(usumem != null)
                    db.UserMembresias.Remove(usumem);
                if (regis != null)
                {
                    foreach(var i in regis)
                    {
                        if (i.idUser == id) db.Registro.Remove(i);
                    }
                }
                  
               
                db.SaveChanges();
                return RedirectToAction("Index");
  
            }
            catch
            {
                return View();
            }
        }
    }
}
