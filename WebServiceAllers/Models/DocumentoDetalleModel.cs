namespace WebServiceAllers.Models
{
    public class DocumentoDetalleModel
    {
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public string WhsCode { get; set; }
        public double Price { get; set; }
        public double Costo { get; set; }
        public double DiscountPercent { get; set; }
        public string CentroCosto { get; set; }
        public int IdCampana { get; set; }
        public int IdListaPrecio { get; set; }
    }
}