using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblBodegaItem
    {

        public tblBodegaItem()
        {
            IdBodega = 0;
        }

        public long IdBodega { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public long idEmpresa { get; set; }
        public string WhsCode { get; set; }
        public int idSede { get; set; }
    }
}