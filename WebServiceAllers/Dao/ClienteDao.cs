using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Dao
{
    public class ClienteDao
    {
        /// <summary>
        /// Método para obtener la lista de clientes.
        /// </summary>
        /// <returns>Lista de objetos de clientes</returns>
        public List<Models.ClienteModel> ObtenerClientes(bool Todos)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            List<Models.ClienteModel> Lista = new List<Models.ClienteModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetAllersCustomers", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    oSQL.Parameters.Add(new SqlParameter("@Todos", Todos));
                    connection.Open();
                    SqlDataReader sqlreader = oSQL.ExecuteReader();
                    while (sqlreader.Read())
                    {
                        Lista.Add(ObtenerItem(sqlreader));
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Lista;
        }
        /// <summary>
        /// Método para obtener la lista de clientes.
        /// </summary>
        /// <returns>Lista de objetos de clientes</returns>
        public DataTable ObtenerClientesRapidos()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            List<Models.ClienteModel> Lista = new List<Models.ClienteModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerClientesRapidos", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        /// <summary>
        /// Metodo para obtener un objeto tipo cliente.
        /// </summary>
        /// <param name="sqlreader"></param>
        /// <returns>Objeto tipo cliente</returns>
        private Models.ClienteModel ObtenerItem(SqlDataReader sqlreader)
        {
            Models.ClienteModel Item = new Models.ClienteModel();
            Item.CardCode = sqlreader["CardCode"].ToString();
            Item.CardName = sqlreader["CardName"].ToString();
            Item.LicTradNum = sqlreader["LicTradNum"].ToString();
            Item.Categoria = sqlreader["Categoria"].ToString();
            Item.Factor = sqlreader["Factor"].ToString();
            Item.FrozenFor = sqlreader["FrozenFor"].ToString();
            Item.City = sqlreader["City"].ToString();
            Item.GroupCode = int.Parse(sqlreader["Grupo"].ToString());
            Item.CondicionPago = int.Parse(sqlreader["CondicionPago"].ToString());
            Item.GroupName = sqlreader["GroupName"].ToString();
            Item.EstablecimientoComercial = sqlreader["CardFName"].ToString();
            Item.Zona = sqlreader["U_ZONA"].ToString();
            Item.AgentCode = int.Parse(sqlreader["AgentCode"].ToString());
            Item.CodigoBodega = sqlreader["U_COD_BODEGA"].ToString();
            Item.MetaCampana = sqlreader["U_MetaCampana"].ToString();
            return Item;
        }
        public DataTable ObtenerDireccionesClientes(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerDireccionesClientes", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable ObtenerDireccionesClientesRapido()
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerDireccionesClientesRapido", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable ObtenerDireccionesPorCliente(string CardCode)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerDireccionesPorCliente", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CardCode", CardCode));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable GruposClientes(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetGruposClientes", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable AllersDepartamentos(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerDepartamentos", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable AllersCiudades(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerCiudades", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable ObtenerHorasEntrega(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerHorasEntrega", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable ObtenerZonasReparto(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerZonasReparto", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable ObtenerEnviarRemisionPor(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerEnviarRemisionPor", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable ObtenerListaPrecioSaluti(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionPos"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerListaPrecioLista", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@IdEmpresa", 1));
                    //oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    //oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable ObtenerGruposClienteSaluti(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionPos"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerGrupoClienteLista", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@idEmpresa", 1));
                    oSQL.Parameters.Add(new SqlParameter("@Texto", string.Empty));
                    //oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    //oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable ObtenerZonasAllers(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetAllersZonas", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable ObtenerTipoCliente(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spTipoCliente", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable CondicionesPagos(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetCondicionesPago", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable ObtenerClientesSaluti(bool Todos, int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionPos"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerClientesSaluti", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    oSQL.Parameters.Add(new SqlParameter("@Todos", Todos));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    connection.Open();
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
        public DataTable ObtenerAllersClientes(bool Todos, int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetAllersCustomers", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@Todos", Todos));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable AllersCarteraClientes(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spAllersCarteraClientes", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable SpecialPricesSaluti(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetSpecialPricesSaluti", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable ObtenerPreciosListaPrecio(int PriceList, int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerPreciosListaPrecio", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@PriceList", PriceList));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable ObtenerPreciosListaPrecioRapido(int PriceList)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerPreciosListaPrecioRapido", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@PriceList", PriceList));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable AllersResponsables(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerResponsablesSap", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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
        public DataTable AllersContactosCilentes(int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetAllersContacts", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
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