using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblPagoItem
    {

        public tblPagoItem()
        {
            idPago = -1;
        }

        public long idPago { get; set; }
        public long idTercero { get; set; }
        public string Tercero { get; set; }
        public decimal totalPago { get; set; }
        public long idEmpresa { get; set; }
        public DateTime fechaPago { get; set; }
        public long idUsuario { get; set; }
        public short idEstado { get; set; }
        public string Observaciones { get; set; }
        public int idTipoPago { get; set; }
        public decimal saldoCliente { get; set; }
        public bool EnCuadre { get; set; }
    }
}