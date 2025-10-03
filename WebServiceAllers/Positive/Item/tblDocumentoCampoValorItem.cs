using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblDocumentoCampoValorItem
    {
        public short idTipoDocumento { get; set; }
        public long idDocumento { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }
}