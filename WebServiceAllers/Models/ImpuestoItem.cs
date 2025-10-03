using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Models
{
    public class ImpuestoItem
    {
        public double ValorImpuestoRetencion { get; set; }
        public double BaseImponible { get; set; }
        public double Porcentaje { get; set; }
        public string Codigo { get; set; }
        public bool IsAutoRetencion { get; set; }
    }
}