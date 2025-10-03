using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Models
{
    public class ProductoSalutiModel
    {
        public string ItemCode { get; set; }
        public string NombreMedishop { get; set; }
        public decimal Stock { get; set; }
        public decimal Tax { get; set; }
        public decimal Price { get; set; }
        public string Marca { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionLarga { get; set; }
        public string Categorizacion { get; set; }
    }
}