﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GymApp.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbGymEntities : DbContext
    {
        public dbGymEntities()
            : base("name=dbGymEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Egresos> Egresos { get; set; }
        public virtual DbSet<Ingresos> Ingresos { get; set; }
        public virtual DbSet<Membresias> Membresias { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Registro> Registro { get; set; }
        public virtual DbSet<UserMembresias> UserMembresias { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
    }
}
