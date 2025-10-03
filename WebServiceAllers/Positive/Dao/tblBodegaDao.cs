using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblBodegaDao
    {
        private SqlConnection Conexion { get; set; }

        public tblBodegaDao(string CadenaConexion)
        {
            Conexion = new SqlConnection(CadenaConexion);
        }

        public tblBodegaItem ObtenerBodegaPorWhsCode(string WhsCode)
        {
            tblBodegaItem Item = new tblBodegaItem();
            SqlCommand oSQL = new SqlCommand("spObtenerBodegaPorWhsCode", Conexion);
            oSQL.CommandType = CommandType.StoredProcedure;
            try
            {
                Conexion.Open();
                oSQL.Parameters.Add(new SqlParameter("@WhsCode", WhsCode));
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

        private tblBodegaItem ObtenerItem(SqlDataReader reader)
        {
            tblBodegaItem Item = new tblBodegaItem();
            Item.IdBodega = long.Parse(reader["idBodega"].ToString());
            Item.Descripcion = reader["Descripcion"].ToString();
            Item.Direccion = reader["Direccion"].ToString();
            Item.idEmpresa = long.Parse(reader["idEmpresa"].ToString());
            Item.WhsCode = reader["WhsCode"].ToString();
            if (reader["idSede"] != DBNull.Value)
            {
                Item.idSede = int.Parse(reader["idSede"].ToString());
            }
            return Item;
        }
    }
}