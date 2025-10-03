using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Models
{
    public class ProductoStockModel
    {
        public string ItemCode { get; set; }
        public string WhsCode { get; set; }
        public decimal Stock { get; set; }
        public string FrozenFor { get; set; }
    }
}