using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblDocumentoCampoValorDao
    {
        private SqlConnection Conexion { get; set; }

        public tblDocumentoCampoValorDao(string CadenaConexion)
        {
            Conexion = new SqlConnection(CadenaConexion);
        }
        public tblDocumentoCampoValorItem ObtenerDocumentoCampoValor(short idTipoDocumento, long idDocumento, string Nombre)
        {
            tblDocumentoCampoValorItem Item = new tblDocumentoCampoValorItem();
            try
            {
                SqlCommand oSQL = new SqlCommand("spObtenerDocumentoCampoValor", Conexion);
                oSQL.CommandType = CommandType.StoredProcedure;
                oSQL.Parameters.Add(new SqlParameter("@idDocumento", idDocumento));
                oSQL.Parameters.Add(new SqlParameter("@idTipoDocumento", idTipoDocumento));
                oSQL.Parameters.Add(new SqlParameter("@Nombre", Nombre));
                Conexion.Open();
                SqlDataReader reader = oSQL.ExecuteReader();
                if (reader.Read())
                {
                    Item.idTipoDocumento = short.Parse(reader["idTipoDocumento"].ToString());
                    Item.idDocumento = long.Parse(reader["idDocumento"].ToString());
                    Item.Nombre = reader["Nombre"].ToString();
                    Item.Valor = reader["Valor"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conexion.State == ConnectionState.Open)
                {
                    Conexion.Close();
                }
            }
            return Item;
        }

        public List<tblDocumentoCampoValorItem> ObtenerDocumentoCampoValorLista(short idTipoDocumento, long idDocumento)
        {
            List<tblDocumentoCampoValorItem> lista = new List<tblDocumentoCampoValorItem>();
            try
            {
                SqlCommand oSQL = new SqlCommand("spObtenerDocumentoCampoValorLista", Conexion);
                oSQL.CommandType = CommandType.StoredProcedure;
                oSQL.Parameters.Add(new SqlParameter("@idDocumento", idDocumento));
                oSQL.Parameters.Add(new SqlParameter("@idTipoDocumento", idTipoDocumento));
                Conexion.Open();
                SqlDataReader reader = oSQL.ExecuteReader();
                if (reader.Read())
                {
                    tblDocumentoCampoValorItem Item = new tblDocumentoCampoValorItem();
                    Item.idTipoDocumento = short.Parse(reader["idTipoDocumento"].ToString());
                    Item.idDocumento = long.Parse(reader["idDocumento"].ToString());
                    Item.Nombre = reader["Nombre"].ToString();
                    Item.Valor = reader["Valor"].ToString();
                    lista.Add(Item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conexion.State == ConnectionState.Open)
                {
                    Conexion.Close();
                }
            }
            return lista;
        }

        private bool Insertar(tblDocumentoCampoValorItem Item)
        {
            try
            {
                SqlCommand oSQL = new SqlCommand("spInsertarDocumentoCampoValor", Conexion);
                oSQL.CommandType = CommandType.StoredProcedure;
                oSQL.Parameters.Add(new SqlParameter("@idDocumento", Item.idDocumento));
                oSQL.Parameters.Add(new SqlParameter("@idTipoDocumento", Item.idTipoDocumento));
                oSQL.Parameters.Add(new SqlParameter("@Nombre", Item.Nombre));
                oSQL.Parameters.Add(new SqlParameter("@Valor", Item.Valor));
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

        private bool Actualizar(tblDocumentoCampoValorItem Item)
        {
            try
            {
                SqlCommand oSQL = new SqlCommand("spActualizarDocumentoCampoValor", Conexion);
                oSQL.CommandType = CommandType.StoredProcedure;
                oSQL.Parameters.Add(new SqlParameter("@idDocumento", Item.idDocumento));
                oSQL.Parameters.Add(new SqlParameter("@idTipoDocumento", Item.idTipoDocumento));
                oSQL.Parameters.Add(new SqlParameter("@Nombre", Item.Nombre));
                oSQL.Parameters.Add(new SqlParameter("@Valor", Item.Valor));
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

        public bool Guardar(tblDocumentoCampoValorItem Item)
        {
            tblDocumentoCampoValorItem OldItem = ObtenerDocumentoCampoValor(Item.idTipoDocumento, Item.idDocumento, Item.Nombre);
            if (OldItem == null || OldItem.idDocumento == 0)
            {
                return Insertar(Item);
            }
            else
            {
                return Actualizar(Item);
            }
        }
    }
}