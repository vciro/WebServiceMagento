using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblDocumentoItem
    {

        public tblDocumentoItem()
        {
            idDocumento = 0;
            IdSede = -1;
        }

        public enum AccionEnum
        {
            Insertar = 1,
            Actualizar = 2,
            Eliminar = 3
        }

        public long idDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime Fecha { get; set; }
        public long idTercero { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public short idCiudad { get; set; }
        public string NombreTercero { get; set; }
        public string Identificacion { get; set; }
        public string Observaciones { get; set; }
        public long idEmpresa { get; set; }
        public long idUsuario { get; set; }
        public string Usuario { get; set; }
        public decimal TotalDocumento { get; set; }
        public decimal TotalIVA { get; set; }
        public decimal saldo { get; set; }
        public long IdEstado { get; set; }
        public string Estado { get; set; }
        public bool EnCuadre { get; set; }
        public List<tblDetalleDocumentoItem> DocumentoLineas { get; set; }
        public tblTipoDocumentoItem.TipoDocumentoEnum TipoDocumento { get; set; }
        public List<tblTipoPagoItem> FormasPago { get; set; }
        public int IdTipoDocumento { get; set; }
        public short BaseIdTipoDocumento { get; set; }
        public decimal Devuelta { get; set; }
        public string Referencia { get; set; }
        public decimal TotalDescuento { get; set; }
        public decimal TotalAntesIVA { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool Pedido { get; set; }
        public string Resolucion { get; set; }
        public int DocEntry { get; set; }
        public int IdSede { get; set; }
        public int IdUnidadNegocio { get; set; }
        public int IdBarrio { get; set; }
        public string Barrio { get; set; }
        public string CardCode { get; set; }
        public int Series { get; set; }
        public string Errores { get; set; }
    }
}