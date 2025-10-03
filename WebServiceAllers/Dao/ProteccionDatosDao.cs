using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Dao
{
    public class ProteccionDatosDao
    {
        public string ObtenerCardCode(string Identificacion)
        {
            string CardCode = string.Empty;
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionSP"].ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerCardCodePorIdentificacion", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    oSQL.Parameters.Add(new SqlParameter("@Identificacion", Identificacion));
                    connection.Open();
                    CardCode = oSQL.ExecuteScalar().ToString();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return CardCode;
        }

        public bool ValidarRespuestaProteccionDatos(string Identificacion, string Token)
        {
            bool Respuesta = false;
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionSP"].ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spValidarRespuestaProteccionDatos", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    oSQL.Parameters.Add(new SqlParameter("@Identificacion", Identificacion));
                    oSQL.Parameters.Add(new SqlParameter("@Token", Token));
                    connection.Open();
                    if (oSQL.ExecuteScalar().ToString() == "1")
                    {
                        Respuesta = true;
                    }
                    connection.Close();
                }
            }
            catch
            {
                return false;
            }
            return Respuesta;
        }

        /// <summary>
        /// Método para guardar la confirmación del cliente de la protección de datos.
        /// </summary>
        /// <returns>Lista de objetos de productos</returns>
        public string GuardarConfirmacion(Models.ProteccionDatosModel Item)
        {
            string Mensaje = string.Empty;
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionSP"].ToString();
            List<Models.ProductoModel> Lista = new List<Models.ProductoModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGuardarProteccionDatos", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    oSQL.Parameters.Add(new SqlParameter("@IP", Item.IP));
                    oSQL.Parameters.Add(new SqlParameter("@Correo", Item.Correo));
                    oSQL.Parameters.Add(new SqlParameter("@Respuesta", Item.Respuesta));
                    oSQL.Parameters.Add(new SqlParameter("@Fecha", Item.Fecha));
                    oSQL.Parameters.Add(new SqlParameter("@Identificacion", Item.Identificacion));
                    oSQL.Parameters.Add(new SqlParameter("@Tipo", Item.Tipo));
                    oSQL.Parameters.Add(new SqlParameter("@Token", Item.Token));
                    connection.Open();
                    if (oSQL.ExecuteNonQuery() == 0)
                    {
                        Mensaje = "No se pudo guardar el registro.";
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return Mensaje;
        }

        /// <summary>
        /// Método para guardar la confirmación del cliente de la protección de datos.
        /// </summary>
        /// <returns>Lista de objetos de productos</returns>
        public void GuardarRegistroToken(Models.ProteccionDatosModel Item)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionSP"].ToString();
            List<Models.ProductoModel> Lista = new List<Models.ProductoModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGuardarRegistroToken", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    oSQL.Parameters.Add(new SqlParameter("@Identificacion", Item.Identificacion));
                    oSQL.Parameters.Add(new SqlParameter("@Token", Item.Token));
                    connection.Open();
                    oSQL.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}