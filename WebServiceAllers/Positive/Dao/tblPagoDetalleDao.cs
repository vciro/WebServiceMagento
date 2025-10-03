using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblPagoDetalleDao
    {
        private SqlConnection Conexion { get; set; }

        public tblPagoDetalleDao(string CadenaConexion)
        {
            Conexion = new SqlConnection(CadenaConexion);
        }

        public bool Guardar(tblPagoDetalleItem Item, SqlConnection Con, SqlTransaction Tran)
        {
            if (Item.idPagoDetalle > 0)
            {
                return Actualizar(Item);
            }
            else
            {
                return Insertar(Item, Con, Tran);
            }
        }

        private bool Actualizar(tblPagoDetalleItem Item)
        {
            SqlCommand oSQL = new SqlCommand("EXEC spActualizarPagoDetalle @idPago,@idDocumento,@valorAbono", Conexion);
            oSQL.Parameters.Add(new SqlParameter("@idPago", Item.idPago));
            oSQL.Parameters.Add(new SqlParameter("@idDocumento", Item.idDocumento));
            oSQL.Parameters.Add(new SqlParameter("@valorAbono", Item.valorAbono));
            try
            {
                Conexion.Open();
                oSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (Conexion.State == System.Data.ConnectionState.Open)
                {
                    Conexion.Close();
                }
            }
            return true;
        }

        private bool Insertar(tblPagoDetalleItem Item, SqlConnection Con, SqlTransaction Tran)
        {
            string SQL = "";
            if (Item.IdTipoDocumento == 1 || Item.IdTipoDocumento == 9)
            {
                SQL = "EXEC spInsertarPagoDetalle @idPago,@idDocumento,@valorAbono,@idTipoDocumento";
            }
            if (Item.IdTipoDocumento == 2)
            {
                SQL = "EXEC spInsertarPagoCompraDetalle @idPago,@idDocumento,@valorAbono,@idTipoDocumento";
            }
            SqlCommand oSQL = new SqlCommand(SQL, Con, Tran);
            oSQL.Parameters.Add(new SqlParameter("@idPago", Item.idPago));
            oSQL.Parameters.Add(new SqlParameter("@idDocumento", Item.idDocumento));
            oSQL.Parameters.Add(new SqlParameter("@valorAbono", Item.valorAbono));
            oSQL.Parameters.Add(new SqlParameter("@idTipoDocumento", Item.IdTipoDocumento));
            try
            {
                Item.idPagoDetalle = long.Parse(oSQL.ExecuteScalar().ToString());
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
            return true;
        }
    }
}