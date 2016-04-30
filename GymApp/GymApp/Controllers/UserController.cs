using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using GymApp.Models;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GymApp.Controllers
{
    [Authorize(Roles ="Administrador, Empleado")]
    public class UserController : Controller
    {
        private dbGymEntities db = new dbGymEntities();
        private static List<UserViewModels> usuarios;
        // GET: User
        public ActionResult Index()
        {
            List<ICollection<AspNetUsers>> lista;
            if (User.IsInRole("Administrador"))
            {
                lista = (from u in db.AspNetRoles where u.Name != "Administrador" select u.AspNetUsers).ToList();

            }else
            {
                lista = (from u in db.AspNetRoles where u.Name == "Normal" select u.AspNetUsers).ToList();
                
            }
           
            usuarios = obtenerUsuarios(lista);
            return View(usuarios);
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
                    int idMem = (from u in db.UserMembresias where u.userid == i.Id select u.tipoMembresia ).FirstOrDefault();
                    usuarios.tipoMembresia = db.Membresias.Find(idMem).Nombre;
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


                    if (usumem != null && model.tipoMembresia!="Ninguna")
                    {
                        usumem.tipoMembresia = (from u in db.Membresias where u.Nombre == model.tipoMembresia select u.id).First();
                        usumem.fInicio = model.fInicio;
                        usumem.ffin = model.ffin;
                        db.Entry(usumem).State = EntityState.Modified;

                    }
                    else if(model.tipoMembresia != "Ninguna")
                    {
                        UserMembresias usuarioMemb = new UserMembresias();
                        usuarioMemb.userid = model.Id;
                        var temp = db.Membresias.Where(x => x.Nombre == model.tipoMembresia).FirstOrDefault();
                        usuarioMemb.tipoMembresia = temp.id;
                        temp = null;
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
