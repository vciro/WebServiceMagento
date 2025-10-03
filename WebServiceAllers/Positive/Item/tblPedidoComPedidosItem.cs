using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblPedidoComPedidosItem
    {
        public long idPedidoCom { get; set; }
        public long idPedido { get; set; }
        public bool Positive { get; set; }
        public string WhsCode { get; set; }
        public string Error { get; set; }
        public bool PedidoInterno { get; set; }
    }
}