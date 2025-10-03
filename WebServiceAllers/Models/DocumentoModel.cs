using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Models
{
    public class DocumentoModel
    {
        public string Numero { get; set; }
        public string CardCode { get; set; }
        public DateTime DocDueDate { get; set; }
        public string Comentarios { get; set; }
        public string CamposUsuario { get; set; }
        public int Serie { get; set; }
        public string CiudadS { get; set; }
        public string StateS { get; set; }
        public string DireccionS { get; set; }
        public string CountryS { get; set; }
        public bool HacerNC { get; set; }
        public string IdUsuarioFactura { get; set; }
        public string IdUsuarioOrigen { get; set; }
        public int IdUnidadNegocio { get; set; }
        public int IdSede { get; set; }
        public BusinessPartnerModel BusinessPartner { get; set; }
        public List<DocumentoDetalleModel> Detalles { get; set; }
        public List<PagoModel> Pago { get; set; }
    }
}