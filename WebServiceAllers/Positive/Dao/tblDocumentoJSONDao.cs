using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebServiceAllers.Positive.Item;

namespace WebServiceAllers.Positive.Dao
{
    public class tblDocumentoJSONDao
    {
        private SqlConnection Conexion { get; set; }

        public tblDocumentoJSONDao(string CadenaConexion)
        {
            Conexion = new SqlConnection(CadenaConexion);
        }
        public bool Guardar(tblDocumentoJSONItem Item)
        {
            try
            {
                SqlCommand oSQL = new SqlCommand("spInsertarDocumentoJSON", Conexion);
                oSQL.CommandType = CommandType.StoredProcedure;
                oSQL.Parameters.Add(new SqlParameter("@idDocumento", Item.idDocumento));
                oSQL.Parameters.Add(new SqlParameter("@data", Item.data));
                oSQL.Parameters.Add(new SqlParameter("@idTipoDocumento", Item.idTipoDocumento));
                Conexion.Open();
                oSQL.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (Conexion.State == ConnectionState.Open)
                {
                    Conexion.Close();
                }
            }
        }
    }
}