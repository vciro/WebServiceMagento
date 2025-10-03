using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblCiudadItem
    {
        public short idCiudad { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public short idDepartamento { get; set; }
        public string CodDepartamento { get; set; }
    }
}