using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Models
{
    public class OrdenCompraModel
    {
        public int DocEntry { get; set; }
        public List<OrdenCompraDetalleModel> Detalles { get; set; }
    }
}