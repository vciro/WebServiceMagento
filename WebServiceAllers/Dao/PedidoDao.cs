using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Dao
{
    public class PedidoDao
    {
        /// <summary>
        /// Método para obtener la lista de productos con el stock
        /// </summary>
        /// <returns>Lista de objetos de productos con el stock</returns>
        public string getDocNumByNumOrderMedishop(string IdPedido)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerPedidoPorIdBI", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    oSQL.Parameters.Add(new SqlParameter("@NumOrderMedishop", IdPedido));
                    connection.Open();
                    return oSQL.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para obtener la lista de productos con el stock
        /// </summary>
        /// <returns>Lista de objetos de productos con el stock</returns>
        public string ValidarPedidoDespachado(string IdPedido)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spValidarPedidoDespachado", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    oSQL.Parameters.Add(new SqlParameter("@DocNum", IdPedido));
                    connection.Open();
                    return oSQL.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ObtenerCotizacionesAbiertas(bool Todos, int CantidadInicio, int CantidadFija, string CardCode = "")
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerCotizacionesAbiertas", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@Todos", Todos));
                    if (string.IsNullOrEmpty(CardCode))
                    {
                        oSQL.Parameters.Add(new SqlParameter("@CardCode", DBNull.Value));
                    }
                    else
                    {
                        oSQL.Parameters.Add(new SqlParameter("@CardCode", CardCode));
                    }
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsDatos = new DataSet();
                    adapter.Fill(dsDatos, "Datos");
                    DataTable dtDatos = dsDatos.Tables[0];
                    connection.Close();
                    return dtDatos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable HistoricoPedidosPorClientesSaluti(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionPOS"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerHistoricoPedidosSaluti", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsDatos = new DataSet();
                    adapter.Fill(dsDatos, "Datos");
                    DataTable dtDatos = dsDatos.Tables[0];
                    connection.Close();
                    return dtDatos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable HistoricoPedidosPorClientes(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerHistoricoPedidos", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsDatos = new DataSet();
                    adapter.Fill(dsDatos, "Datos");
                    DataTable dtDatos = dsDatos.Tables[0];
                    connection.Close();
                    return dtDatos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ObtenerPedidosEstadoSaluti(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionPOS"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerPedidosEstados", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsDatos = new DataSet();
                    adapter.Fill(dsDatos, "Datos");
                    DataTable dtDatos = dsDatos.Tables[0];
                    connection.Close();
                    return dtDatos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable ObtenerTrackingPedido(int DocEntry)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerTrackingPedido", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@DocEntry", DocEntry));
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsDatos = new DataSet();
                    adapter.Fill(dsDatos, "Datos");
                    DataTable dtDatos = dsDatos.Tables[0];
                    connection.Close();
                    return dtDatos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetOrdersStatus()
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetOrderStatus", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsDatos = new DataSet();
                    adapter.Fill(dsDatos, "Datos");
                    DataTable dtDatos = dsDatos.Tables[0];
                    connection.Close();
                    return dtDatos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}