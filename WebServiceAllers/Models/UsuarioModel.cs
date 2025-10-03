using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Models
{
    public class UsuarioModel
    {
        public string Nombre { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool Activo { get; set; }
    }
}