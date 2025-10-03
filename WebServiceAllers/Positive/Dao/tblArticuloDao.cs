using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblArticuloDao
    {
        private SqlConnection Conexion { get; set; }

        public tblArticuloDao(string CadenaConexion)
        {
            Conexion = new SqlConnection(CadenaConexion);
        }

        public tblArticuloItem ObtenerArticuloPorCodigo(string Codigo)
        {
            tblArticuloItem Item = new tblArticuloItem();
            SqlCommand oSQL = new SqlCommand("spObtenerArticuloPorCodigoEmpresa", Conexion);
            oSQL.CommandType = CommandType.StoredProcedure;
            try
            {
                Conexion.Open();
                oSQL.Parameters.Add(new SqlParameter("@Codigo", Codigo));
                oSQL.Parameters.Add(new SqlParameter("@idEmpresa", 1));
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

        private tblArticuloItem ObtenerItem(SqlDataReader reader)
        {
            tblArticuloItem Item = new tblArticuloItem();
            Item.IdArticulo = long.Parse(reader["idArticulo"].ToString());
            Item.CodigoArticulo = reader["CodigoArticulo"].ToString();
            Item.Nombre = reader["Nombre"].ToString();
            Item.Presentacion = reader["Presentacion"].ToString();
            Item.IdLinea = long.Parse(reader["idLinea"].ToString());
            Item.Linea = reader["Linea"].ToString();
            Item.IVAVenta = decimal.Parse(reader["IVA"].ToString());
            Item.IVACompra = decimal.Parse(reader["IVACompra"].ToString());
            Item.CodigoBarra = reader["CodigoBarra"].ToString();
            Item.IdTercero = long.Parse(reader["idTercero"].ToString());
            Item.Tercero = reader["Tercero"].ToString();
            Item.IdEmpresa = long.Parse(reader["idEmpresa"].ToString());
            Item.Empresa = reader["Empresa"].ToString();
            Item.IdBodega = long.Parse(reader["idBodega"].ToString());
            Item.Bodega = reader["Bodega"].ToString();
            Item.EsInventario = bool.Parse(reader["EsInventario"].ToString());
            Item.Cantidad = decimal.Parse(reader["Cantidad"].ToString());
            Item.StockMinimo = decimal.Parse(reader["StockMinimo"].ToString());
            Item.PrecioAutomatico = bool.Parse(reader["PrecioAutomatico"].ToString());
            Item.Activo = bool.Parse(reader["Activo"].ToString());
            Item.CodigoLargo = reader["CodigoLargo"].ToString();
            Item.IdFamilia = long.Parse(reader["IdFamilia"].ToString());
            Item.CostoPonderado = decimal.Parse(reader["CostoPonderado"].ToString());
            Item.IdUsuario = long.Parse(reader["IdUsuario"].ToString());
            Item.IdUsuarioModificacion = long.Parse(reader["IdUsuarioModificacion"].ToString());
            Item.CostoPonderadoAllers = decimal.Parse(reader["CostoPonderadoAllers"].ToString());
            Item.ParaEstudiante = bool.Parse(reader["ParaEstudiante"].ToString());
            return Item;
        }
    }
}