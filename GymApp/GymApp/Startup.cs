using System;
using Microsoft.Owin;
using Owin;
using GymApp.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

[assembly: OwinStartupAttribute(typeof(GymApp.Startup))]
namespace GymApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var rolManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!rolManager.RoleExists("Administrador"))
            {
                var role = new IdentityRole();
                role.Name = "Administrador";
                rolManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "Admin@gmail.com";
                user.FirstName = "Admin";
                user.LastName = "Nuevo";
                user.Email = "leo.dd@hotmail.es";

                string userPWD = "Admin 123";
                var chkuser = userManager.Create(user, userPWD);
                if(chkuser.Succeeded)
                {
                    var reulst1 = userManager.AddToRole(user.Id, "Administrador");
                    
                }
                var user2 = new ApplicationUser();
                user2.UserName = "DiegoNavarro";
                user2.FirstName = "Diego";
                user2.LastName = "Navarro";
                user2.Email = "Navarro.diego@gmail.com";

                string userPWD2 = "DiegoNavarro7!";
                var chkuser2 = userManager.Create(user2, userPWD2);
                if (chkuser2.Succeeded)
                {
                    var reulst2 = userManager.AddToRole(user2.Id, "Administrador");

                }
            }


            if (!rolManager.RoleExists("Empleado"))
            {
                var role = new IdentityRole();
                role.Name = "Empleado";
                rolManager.Create(role);
            }

            if (!rolManager.RoleExists("Normal"))
            {
                var role = new IdentityRole();
                role.Name = "Normal";
                rolManager.Create(role);
            }
        }
    }
}
