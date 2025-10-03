using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblDetalleDocumentoItem
    {

        public tblDetalleDocumentoItem()
        {
            idDetalleDocumento = -1;
        }

        public long idDetalleDocumento { get; set; }
        public long idDocumento { get; set; }
        public short NumeroLinea { get; set; }
        public long idArticulo { get; set; }
        public string Codigo { get; set; }
        public string Articulo { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal IVA { get; set; }
        public decimal Cantidad { get; set; }
        public long idBodega { get; set; }
        public string Bodega { get; set; }
        public decimal TotalLinea { get; set; }
        public decimal Descuento { get; set; }
        public decimal CostoPonderado { get; set; }
        public decimal ValorDescuento { get; set; }
        public decimal ValorUnitarioConDescuento { get; set; }
        public decimal ValorUnitarioConIVA { get; set; }
        public bool EsInventario { get; set; }
        public decimal CostoPonderadoAllers { get; set; }
        public int IdCampana { get; set; }
        public int TipoDescuento { get; set; }
        public int VisOrder { get; set; }
        public int LineNum { get; set; }
        public string WhsCode { get; set; }
    }
}