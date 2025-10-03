using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Models
{
    public class DocumentoItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string U_CreatedDate { get; set; }
        public string U_UploadDate { get; set; }
        public string U_XmlData { get; set; }
        public string U_Respuesta { get; set; }
        public string U_Estado { get; set; }
        public int U_DocNum { get; set; }
        public string U_Tipo { get; set; }
    }
}