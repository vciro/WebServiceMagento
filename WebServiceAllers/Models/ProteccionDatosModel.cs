using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Models
{
    public class ProteccionDatosModel
    {
        public long Id { get; set; }
        public string IP { get; set; }
        public string Correo { get; set; }
        public string Respuesta { get; set; }
        public DateTime Fecha { get; set; }
        public string Identificacion { get; set; }
        public string Tipo { get; set; } //0. Dato maestro 1. Persona de contacto 2. ISO
        public string Token { get; set; }
    }
}