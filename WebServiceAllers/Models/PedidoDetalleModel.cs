using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Models
{
    public class PedidoDetalleModel
    {
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public string WhsCode { get; set; }
        public double Price { get; set; }
        public double DiscountPercent { get; set; }
        public int? LineNum { get; set; }
        public int? VisOrder { get; set; }
        public int? BaseEntry { get; set; }
        public int? BaseLine { get; set; }
        public string CamposUsuario { get; set; }
        public int Eliminar { get; set; }
    }
}