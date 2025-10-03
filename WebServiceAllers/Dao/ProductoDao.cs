using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebServiceAllers.Models;

namespace WebServiceAllers.Dao
{
    public class ProductoDao
    {
        public DataTable ObtenerProductos(bool Todos)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetAllersProducts", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    oSQL.Parameters.Add(new SqlParameter("@Todos", Todos));
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsArticulo = new DataSet();
                    adapter.Fill(dsArticulo, "Articulos");
                    DataTable dtArticulo = dsArticulo.Tables[0];
                    connection.Close();
                    return dtArticulo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Método para obtener los artículos solicitados.
        /// </summary>
        /// <returns>Lista de los artículos solicitados.</returns>
        public DataTable ObtenerProductosSolicitados()
        { 
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerArticulosSolicitados", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsArticulo = new DataSet();
                    adapter.Fill(dsArticulo, "Articulos");
                    DataTable dtArticulo = dsArticulo.Tables[0];
                    connection.Close();
                    return dtArticulo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para obtener la lista de productos de Saluti.com.
        /// </summary>
        /// <returns>Lista de objetos de productos</returns>
        public DataTable ObtenerProductosSalutiCom(bool Todos)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetArticulosSalutiSAP", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    oSQL.Parameters.Add(new SqlParameter("@Todos", Todos));
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsArticulo = new DataSet();
                    adapter.Fill(dsArticulo, "Articulos");
                    DataTable dtArticulo = dsArticulo.Tables[0];
                    connection.Close();
                    return dtArticulo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable ObtenerProductosSalutiComPaginado(int Todos, int CantidadInicio, int CantidadFija)
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetArticulosSalutiSAPaginado", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    oSQL.Parameters.Add(new SqlParameter("@Todos", Todos));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsArticulo = new DataSet();
                    adapter.Fill(dsArticulo, "Articulos");
                    DataTable dtArticulo = dsArticulo.Tables[0];
                    connection.Close();
                    return dtArticulo;
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
        public DataTable ObtenerProductosStock(int CantidadInicio, int CantidadFija)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetProductsStock", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsArticulo = new DataSet();
                    adapter.Fill(dsArticulo, "Articulos");
                    DataTable dtArticulo = dsArticulo.Tables[0];
                    connection.Close();
                    return dtArticulo;
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
        public DataTable SpecialPrices(bool Todos, int CantidadInicio, int CantidadFija)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetSpecialPrices", connection);
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

        /// <summary>
        /// Método para obtener la lista de las familias de los artículos de Allers
        /// </summary>
        /// <returns>Lista de objetos de la lista de las familias de los artículos de Allers</returns>
        public DataTable GetFamiliasArticulosAllers(int CantidadInicio, int CantidadFija)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetFamiliasArticulosAllers", connection);
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

        /// <summary>
        /// Método para obtener la lista de productos con el stock
        /// </summary>
        /// <returns>Lista de objetos de productos con el stock</returns>
        public DataTable ObtenerProductosSalutiPrice()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetArticulosSalutiPrice", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsArticulo = new DataSet();
                    adapter.Fill(dsArticulo, "Articulos");
                    DataTable dtArticulo = dsArticulo.Tables[0];
                    connection.Close();
                    return dtArticulo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable ObtenerProductosSalutiPricePaginado(int CantidadInicio, int CantidadFija, DateTime Fecha)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetArticulosSalutiPricePaginado", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.Parameters.Add(new SqlParameter("@Fecha", Fecha));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsArticulo = new DataSet();
                    adapter.Fill(dsArticulo, "Articulos");
                    DataTable dtArticulo = dsArticulo.Tables[0];
                    connection.Close();
                    return dtArticulo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProductoStockModel> ObtenerProductoStock(int Todos, int CantidadInicio, int CantidadFija)
        {
            List<ProductoStockModel> list = new List<ProductoStockModel>();
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand oSQL = new SqlCommand("spObtenerProductoStock", connection);
            oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
            oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
            oSQL.Parameters.Add(new SqlParameter("@Todos", Todos));
            oSQL.CommandType = CommandType.StoredProcedure;
            connection.Open();
            try
            {
                SqlDataReader reader = oSQL.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new ProductoStockModel()
                    {
                        ItemCode = reader["ItemCode"].ToString(),
                        WhsCode = reader["WhsCode"].ToString(),
                        Stock = decimal.Parse(reader["Stock"].ToString())
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
                if (connection.State == ConnectionState.Open) {
                    connection.Close();
                }
            }
        }

        public SedeModel ObtenerSedeByWhsDefault(string WhsDefault)
        {
            SedeModel item = new SedeModel();
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionPos"].ToString();
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand oSQL = new SqlCommand("spObtenerSedeByWhsDefault", connection);
            oSQL.Parameters.Add(new SqlParameter("@WhsDefault", WhsDefault));
            oSQL.CommandType = CommandType.StoredProcedure;
            connection.Open();
            try
            {
                SqlDataReader reader = oSQL.ExecuteReader();
                while (reader.Read())
                {
                    item = new SedeModel()
                    {
                        IdSede = int.Parse(reader["IdSede"].ToString()),
                        Sede = reader["Sede"].ToString()
                    };
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Método para obtener la lista de productos con el stock
        /// </summary>
        /// <returns>Lista de objetos de productos con el stock</returns>
        public DataTable ObtenerProductosStockFast()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerStockArticulosAllersConMovimientos", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsArticulo = new DataSet();
                    adapter.Fill(dsArticulo, "Articulos");
                    DataTable dtArticulo = dsArticulo.Tables[0];
                    connection.Close();
                    return dtArticulo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProductoStockModel> ObtenerProductosStockFastBodegas()
        {
            List<ProductoStockModel> list = new List<ProductoStockModel>();
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand oSQL = new SqlCommand("spObtenerStockArticulosSalutiConMovimientos", connection);
            oSQL.CommandType = CommandType.StoredProcedure;
            connection.Open();
            try
            {
                SqlDataReader reader = oSQL.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new ProductoStockModel()
                    {
                        ItemCode = reader["ItemCode"].ToString(),
                        WhsCode = reader["WhsCode"].ToString(),
                        Stock = decimal.Parse(reader["Stock"].ToString())
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public DataTable ObtenerAllersProductos(bool Todos, int CantidadInicio, int CantidadFija)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spGetAllersProducts", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@Todos", Todos));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsArticulo = new DataSet();
                    adapter.Fill(dsArticulo, "Articulos");
                    DataTable dtArticulo = dsArticulo.Tables[0];
                    connection.Close();
                    return dtArticulo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ObtenerStockArticulosAllers(int CantidadInicio, int CantidadFija)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerStockArticulosAllers", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsArticulo = new DataSet();
                    adapter.Fill(dsArticulo, "Articulos");
                    DataTable dtArticulo = dsArticulo.Tables[0];
                    connection.Close();
                    return dtArticulo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ObtenerStockArticulosSaluti(int CantidadInicio, int CantidadFija)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionPOS"].ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerStockArticulosSaluti", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.Parameters.Add(new SqlParameter("@CantidadInicio", CantidadInicio));
                    oSQL.Parameters.Add(new SqlParameter("@CantidadFija", CantidadFija));
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsArticulo = new DataSet();
                    adapter.Fill(dsArticulo, "Articulos");
                    DataTable dtArticulo = dsArticulo.Tables[0];
                    connection.Close();
                    return dtArticulo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Método para obtener la lista de productos.
        /// </summary>
        /// <returns>Lista de objetos de productos</returns>
        public DataTable ObtenerAcuerdosGlobales()
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerAcuerdosGlobales", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsArticulo = new DataSet();
                    adapter.Fill(dsArticulo, "Articulos");
                    DataTable dtArticulo = dsArticulo.Tables[0];
                    connection.Close();
                    return dtArticulo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable ObtenerProductosRapido()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexion"].ToString();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand oSQL = new SqlCommand("spObtenerProductosRapido", connection);
                    oSQL.CommandType = CommandType.StoredProcedure;
                    oSQL.CommandTimeout = 100000;
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(oSQL);
                    DataSet dsArticulo = new DataSet();
                    adapter.Fill(dsArticulo, "Articulos");
                    DataTable dtArticulo = dsArticulo.Tables[0];
                    connection.Close();
                    return dtArticulo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}