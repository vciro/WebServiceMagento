using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblCiudadDao
    {
        private SqlConnection Conexion { get; set; }

        public tblCiudadDao(string CadenaConexion)
        {
            Conexion = new SqlConnection(CadenaConexion);
        }

        public tblCiudadItem ObtenerCiudadPorCodigo(string Codigo)
        {
            tblCiudadItem Item = new tblCiudadItem();
            SqlCommand oSQL = new SqlCommand("spObtenerCiudadPorCodigo", Conexion);
            try
            {
                Conexion.Open();
                oSQL.Parameters.Add(new SqlParameter("@Codigo", Codigo));
                oSQL.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = oSQL.ExecuteReader();
                if (reader.Read())
                {
                    Item = ObtenerItem(reader);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conexion.State == System.Data.ConnectionState.Open)
                {
                    Conexion.Close();
                }
            }
            return Item;
        }

        private tblCiudadItem ObtenerItem(SqlDataReader reader)
        {
            tblCiudadItem Item = new tblCiudadItem();
            Item.idCiudad = short.Parse(reader["idCiudad"].ToString());
            Item.Codigo = reader["Codigo"].ToString();
            Item.Nombre = reader["Nombre"].ToString();
            Item.idDepartamento = short.Parse(reader["idDepartamento"].ToString());
            Item.CodDepartamento = reader["CodDepartamento"].ToString();
            return Item;
        }
    }
}