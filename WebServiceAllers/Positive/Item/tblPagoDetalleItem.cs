using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblPagoDetalleItem
    {
        public long idPagoDetalle { get; set; }
        public long idPago { get; set; }
        public long idDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public decimal valorAbono { get; set; }
        public short IdTipoDocumento { get; set; }
    }
}