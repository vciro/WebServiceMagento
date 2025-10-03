using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblTipoDocumentoDao
    {
        private SqlConnection Conexion { get; set; }
        public tblTipoDocumentoDao(string CadenaConexion)
        {
            Conexion = new SqlConnection(CadenaConexion);
        }

        public tblTipoDocumentoItem ObtenerTipoDocumento(long Id)
        {
            tblTipoDocumentoItem Item = new tblTipoDocumentoItem();
            SqlCommand oSQL = new SqlCommand("spObtenerTipoDocumento", Conexion);
            try
            {
                Conexion.Open();
                oSQL.Parameters.Add(new SqlParameter("@id", Id));
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

        private tblTipoDocumentoItem ObtenerItem(SqlDataReader reader)
        {
            tblTipoDocumentoItem Item = new tblTipoDocumentoItem();
            Item.idTipoDocumento = short.Parse(reader["idTipoDocumento"].ToString());
            Item.Nombre = reader["Nombre"].ToString();
            Item.TablaDocumento = reader["TablaDocumento"].ToString();
            Item.TablaDetalle = reader["TablaDetalle"].ToString();
            Item.idTipoSocioNegocio = reader["idTipoSocioNegocio"].ToString();
            Item.AfectaInventario = bool.Parse(reader["AfectaInventario"].ToString());
            Item.SentidoInventario = short.Parse(reader["SentidoInventario"].ToString());
            Item.ManejaListaPrecios = bool.Parse(reader["ManejaListaPrecios"].ToString());
            Item.TablaNumeracion = reader["TablaNumeracion"].ToString();
            return Item;
        }
    }
}