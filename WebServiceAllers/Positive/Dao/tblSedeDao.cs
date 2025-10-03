using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WebServiceAllers.Positive
{
    public class tblSedeDao
    {
        private SqlConnection Conexion { get; set; }

        public tblSedeDao(string CadenaConexion)
        {
            Conexion = new SqlConnection(CadenaConexion);
        }
        public List<tblSedeItem> ObtenerSedes()
        {
            List<tblSedeItem> list = new List<tblSedeItem>();
            SqlCommand oSQL = new SqlCommand("spObtenerSedes", Conexion);
            oSQL.CommandType = CommandType.StoredProcedure;
            try
            {
                Conexion.Open();
                SqlDataReader reader = oSQL.ExecuteReader();
                while (reader.Read()) {
                    list.Add(ObtenerItem(reader));
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
            return list;
        }

        public tblSedeItem ObtenerSedeID(int IdSede)
        {
            tblSedeItem item = new tblSedeItem();
            SqlCommand oSQL = new SqlCommand("spObtenerSedeID", Conexion);
            oSQL.Parameters.Add(new SqlParameter("@IdSede",IdSede));
            oSQL.CommandType = CommandType.StoredProcedure;
            try
            {
                Conexion.Open();
                SqlDataReader reader = oSQL.ExecuteReader();
                if (reader.Read())
                {
                    item = ObtenerItem(reader);
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
            return item;
        }

        public tblSedeItem ObtenerSedeCardCode(string CardCode)
        {
            tblSedeItem item = new tblSedeItem();
            SqlCommand oSQL = new SqlCommand("spObtenerSedeByCardCode", Conexion);
            oSQL.Parameters.Add(new SqlParameter("@CardCode", CardCode));
            oSQL.CommandType = CommandType.StoredProcedure;
            try
            {
                Conexion.Open();
                SqlDataReader reader = oSQL.ExecuteReader();
                if (reader.Read())
                {
                    item = ObtenerItem(reader);
                }
                else {
                    return null;
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
            return item;
        }

        private tblSedeItem ObtenerItem(SqlDataReader reader) {
            tblSedeItem item = new tblSedeItem();
            item = (new tblSedeItem()
            {
                IdSede = int.Parse(reader["IdSede"].ToString()),
                Sede = reader["Sede"].ToString(),
                Activo = bool.Parse(reader["Activo"].ToString()),
                CentroCosto = reader["CentroCosto"].ToString(),
                unidadNegocio = int.Parse(reader["unidadNegocio"].ToString()),
                unidadNegocioCallCenter = int.Parse(reader["unidadNegocioCallCenter"].ToString())
            });
            if (reader["CardCode"] != DBNull.Value) {
                item.CardCode = reader["CardCode"].ToString();
            }
            return item;
        }
    }
}
