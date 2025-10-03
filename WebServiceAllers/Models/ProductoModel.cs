using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Models
{
    public class ProductoModel
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionLarga { get; set; }
        public decimal Price { get; set; }
        public decimal PrecioRegulado { get; set; }
        public decimal Stock { get; set; }
        public string TaxCodeAR { get; set; }
        public string SalUnitMsr { get; set; }
        public string Presentacion { get; set; }
        public string FrozenFor { get; set; }
        public string Familia { get; set; }
        public string GrupoFamilia { get; set; }
        public string LineaArticulo { get; set; }
        public string TipoPrecio { get; set; }
        public string Controlado { get; set; }
        public string Refrigerado { get; set; }
        public DateTime FechaAdmision { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CantidadSolicitada { get; set; }
        public int PriceList { get; set; }
        public string ArticuloMedishop { get; set; }
        public string InvntItem { get; set; }
        public string SellItem { get; set; }
        public string PrchseItem { get; set; }
        public string Estado { get; set; }
    }
}