using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Models
{
    public class PagoModel
    {
        public int TipoPago { get; set; } // 1. Transferencia 2. Tarjeta 3. Contraentrega
        //public int Series { get; set; }
        public int? CreditCard { get; set; }
        public string CreditAcct { get; set; }
        public string CreditCardNumber { get; set; }
        public DateTime? CardValidUntil { get; set; }
        public double? CreditSum { get; set; }
        public string VoucherNum { get; set; }
        public string CashAccount { get; set; }
        public double? CashSum { get; set; }
        public double? TrsfrSum { get; set; }
        public string TrsfrAcct { get; set; }
        public string TrsfrRef { get; set; }
        public string CheckAccount { get; set; }
        public double? CheckSum { get; set; }
        public int? CheckNumber { get; set; }
        public string BankCode { get; set; }
        public string Ref2 { get; set; }
        public string CamposUsuario { get; set; }
        public string Comments { get; set; }
    }
}