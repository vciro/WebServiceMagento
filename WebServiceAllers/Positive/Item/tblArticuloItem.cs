using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblArticuloItem
    {

        public tblArticuloItem()
        {
            IdArticulo = 0;
        }
        public long IdArticulo { get; set; }
        public string CodigoArticulo { get; set; }
        public string Nombre { get; set; }
        public string Presentacion { get; set; }
        public long IdLinea { get; set; }
        public string Linea { get; set; }
        public decimal IVACompra { get; set; }
        public decimal IVAVenta { get; set; }
        public string CodigoBarra { get; set; }
        public long IdTercero { get; set; }
        public string Tercero { get; set; }
        public long IdEmpresa { get; set; }
        public string Empresa { get; set; }
        public long IdBodega { get; set; }
        public string Bodega { get; set; }
        public bool EsInventario { get; set; }
        public decimal StockMinimo { get; set; }
        public bool Activo { get; set; }
        public bool PrecioAutomatico { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public long IdFamilia { get; set; }
        public string CodigoLargo { get; set; }
        public decimal CostoPonderado { get; set; }
        public decimal CostoPonderadoAllers { get; set; }
        public long IdUsuario { get; set; }
        public long IdUsuarioModificacion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public bool ParaEstudiante { get; set; }
    }
}