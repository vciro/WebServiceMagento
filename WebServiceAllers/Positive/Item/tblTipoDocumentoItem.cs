using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblTipoDocumentoItem
    {

        public enum TipoDocumentoEnum
        {
            venta = 1,
            compra = 2,
            cotizacion = 3,
            notaCreditoVenta = 4,
            entradaMercancia = 5,
            salidaMercancia = 6,
            notaCreditoCompra = 7,
            trasladoMercancia = 8,
            separadoMercancia = 9,
            pedido = 10,
            trasladoSalida = 11,
            trasladoEntrada = 12,
            ordenCompra = 13,
            PedidoCom = 23
        }

        public short idTipoDocumento { get; set; }
        public string Nombre { get; set; }
        public string TablaDocumento { get; set; }
        public string TablaDetalle { get; set; }
        public string idTipoSocioNegocio { get; set; }
        public bool AfectaInventario { get; set; }
        public short SentidoInventario { get; set; }
        public bool ManejaListaPrecios { get; set; }
        public string TablaNumeracion { get; set; }
    }
}