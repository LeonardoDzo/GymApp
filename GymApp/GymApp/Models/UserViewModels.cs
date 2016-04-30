using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class UserViewModels
    {
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Número de Télefono")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }
        
        [Display(Name = "tipo de membresia")]
        public string tipoMembresia { get; set; }
       
        [Display(Name = "Fecha de Inicio")]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public System.DateTime fInicio { get; set; }
     
        [Display(Name = "Fecha fin")]

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime ffin { get; set; }

        [Display(Name = "Fecha Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime FechaNacimiento { get; set; }

        public string userRol { get; set; }
    }
}