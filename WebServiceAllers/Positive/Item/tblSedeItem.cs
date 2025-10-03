using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceAllers.Positive
{
    public class tblSedeItem
    {
        public int IdSede { get; set; }
        public string Sede { get; set; }
        public bool Activo { get; set; }
        public string CentroCosto { get; set; }
        public int unidadNegocio { get; set; }
        public int unidadNegocioCallCenter { get; set; }
        public string CardCode { get; set; }
    }
}
