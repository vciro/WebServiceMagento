using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Models
{
    public class BusinessPartnerModel
    {
        public string LicTradNum { get; set; }
        public string CardName { get; set; }
        public string CardFName { get; set; }
        public int GroupCode { get; set; }
        public int SlpCode { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public string Departamento { get; set; }
        public string Barrio { get; set; }
        public string TipoPersona { get; set; }
        public string TipoDocumento { get; set; }
        public string CamposUsuario { get; set; }
    }
}