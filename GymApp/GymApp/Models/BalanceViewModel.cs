using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymApp.Models
{
    public class BalanceViewModel
    {
        public int id { get; set; } = -100000000;
        public string Nombre { get; set; } = "";
        public string Monto { get; set; } = "";
        public string Fecha { get; set; } = "";
    }
}