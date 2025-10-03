using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Models
{
    public class ContactoModel
    {
        public string CardCode { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Responsable { get; set; }
        public string Area { get; set; }
        public string Cargo { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
    }
}