using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Models
{
    public class PedidoModel
    {
        public int TipoDocumento { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public bool FechaVencimiento { get; set; }
        public string IdPedido { get; set; }
        public string CedulaCliente { get; set; }
        public string NombreClienteFacturacion { get; set; }
        public string TelefonoFacturacion { get; set; }
        public string CiudadFacturacion { get; set; }
        public string DepartamentoFacturacion { get; set; }
        public string PaisFacturacion { get; set; }
        public string DireccionFacturacion { get; set; }
        public string NombreClienteEnvio { get; set; }
        public string TelefonoEnvio { get; set; }
        public string CiudadEnvio { get; set; }
        public string DepartamentoEnvio { get; set; }
        public string PaisEnvio { get; set; }
        public string DireccionEnvio { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public DateTime FechaDespacho { get; set; }
        public string Comentarios { get; set; }
        public string Observaciones { get; set; }
        public string Email { get; set; }
        public string MedioPago { get; set; }
        public double Descuento { get; set; }
        public int Ecommerce { get; set; }
        public string NumeroAutorizacion { get; set; }
        public int IdUsuario { get; set; }
        public string InfLogistica { get; set; }
        public string ComentariosDocumento { get; set; }
        public int Autorizado { get; set; }
        public bool RecogerTienda { get; set; }
        public string AgentCode { get; set; }
        public string ShipToCode { get; set; }
        public string PaytoCode { get; set; }
        public int? CondicionPago { get; set; }
        public string Referencia { get; set; }
        public bool Ocasional { get; set; }
        public string TipoCotizacion { get; set; }
        public string Foco { get; set; }
        public string CamposUsuario { get; set; }
        public string Nit { get; set; }
        public string FacturaReserva { get; set; }
        public string PendientePor { get; set; }
        public int Datafono { get; set; }
        public string CodigoCiudad { get; set; }
        public List<PedidoDetalleModel> Detalles { get; set; }

        public string ObtenerDepartamentoNombre(string Code)
        {
            string Nombre = "";
            String ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionSBO_Allers"].ToString();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                String strquery = string.Format("SELECT Name FROM OCST WHERE Code = '{0}'", Code);
                connection.Open();
                SqlCommand sqlquery = new SqlCommand(strquery, connection);
                SqlDataReader sqlreader = sqlquery.ExecuteReader();
                if (sqlreader.Read())
                {
                    Nombre = sqlreader["Name"].ToString();
                }
                connection.Close();
            }
            return Nombre;
        }

        public string GetDocNumSaluti(string DocEntry)
        {
            string DocNum = "";
            String ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionSaluti"].ToString();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT DocNum FROM ORDR WHERE DocEntry = @DocEntry";
                SqlCommand cmd = new SqlCommand(sql, connection);
                connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@DocEntry", DocEntry));
                SqlDataReader sqlreader = cmd.ExecuteReader();
                while (sqlreader.Read())
                {
                    DocNum = Convert.ToString(sqlreader["DocNum"]);
                }
                sqlreader.Close();
            }
            return DocNum;
        }

        public void InsertarWebServiceLog(string Descripcion)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spInsertarWebSericeLog", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    oSQL.Parameters.Add(new SqlParameter("@Descripcion", Descripcion));
                    oSQL.Parameters.Add(new SqlParameter("@Fecha", DateTime.Now));
                    oSQL.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}