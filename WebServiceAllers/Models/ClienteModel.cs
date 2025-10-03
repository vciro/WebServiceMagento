using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Models
{
    public class ClienteModel
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string LicTradNum { get; set; }
        public string Categoria { get; set; }
        public string Factor { get; set; }
        public string FrozenFor { get; set; }
        public string City { get; set; }
        public int GroupCode { get; set; }
        public int CondicionPago { get; set; }
        public string GroupName { get; set; }
        public string EstablecimientoComercial { get; set; }
        public string Zona { get; set; }
        public int AgentCode { get; set; }
        public string CodigoBodega { get; set; }
        public string MetaCampana { get; set; }
    }
}