//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class sender
    {
        [Display(Name = "Correo")]
        public string sentaccount { get; set; }
        [Required(ErrorMessage = "Contraseña Requerida")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string pass { get; set; }
        public int Id { get; set; }
    }
}
