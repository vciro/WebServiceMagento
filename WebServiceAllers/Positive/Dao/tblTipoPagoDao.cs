using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblTipoPagoDao
    {
        private SqlConnection Conexion { get; set; }

        public tblTipoPagoDao(string CadenaConexion)
        {
            Conexion = new SqlConnection(CadenaConexion);
        }

        public bool Guardar(tblTipoPagoItem Item, SqlConnection Con, SqlTransaction Tran, long TipoDocumento)
        {
            if (Item.idTipoPago > 0)
            {
                return Actualizar(Item);
            }
            else
            {
                return Insertar(Item, Con, Tran, TipoDocumento);
            }
        }

        private bool Insertar(tblTipoPagoItem Item, SqlConnection Con, SqlTransaction Tran, long TipoDocumento)
        {
            string SQL = "";
            if (TipoDocumento == 1 || TipoDocumento == 9)
            {
                SQL = "EXEC spInsertarTipoPago @idPago,@ValorPago,@idFormaPago,@voucher,@idTipoTarjetaCredito";
            }
            if (TipoDocumento == 2)
            {
                SQL = "EXEC spInsertarTipoPagoCompra @idPago,@ValorPago,@idFormaPago,@voucher";
            }
            SqlCommand oSQL = new SqlCommand(SQL, Con, Tran);
            oSQL.Parameters.Add(new SqlParameter("@idPago", Item.idPago));
            oSQL.Parameters.Add(new SqlParameter("@ValorPago", Item.ValorPago));
            oSQL.Parameters.Add(new SqlParameter("@idFormaPago", Item.idFormaPago));
            if (string.IsNullOrEmpty(Item.voucher))
            {
                oSQL.Parameters.Add(new SqlParameter("@voucher", DBNull.Value));
            }
            else
            {
                oSQL.Parameters.Add(new SqlParameter("@voucher", Item.voucher));
            }
            if (Item.idTipoTarjetaCredito == 0)
            {
                oSQL.Parameters.Add(new SqlParameter("@idTipoTarjetaCredito", DBNull.Value));
            }
            else
            {
                oSQL.Parameters.Add(new SqlParameter("@idTipoTarjetaCredito", Item.idTipoTarjetaCredito));
            }
            try
            {
                Item.idTipoPago = long.Parse(oSQL.ExecuteScalar().ToString());
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

        private bool Actualizar(tblTipoPagoItem Item)
        {
            SqlCommand oSQL = new SqlCommand("EXEC spActualizarTipoPago @idTipoPago,@idPago,@ValorPago,@idFormaPago,@voucher", Conexion);
            oSQL.Parameters.Add(new SqlParameter("@idTipoPago", Item.idTipoPago));
            oSQL.Parameters.Add(new SqlParameter("@idPago", Item.idPago));
            oSQL.Parameters.Add(new SqlParameter("@ValorPago", Item.ValorPago));
            oSQL.Parameters.Add(new SqlParameter("@idFormaPago", Item.idFormaPago));
            oSQL.Parameters.Add(new SqlParameter("@voucher", Item.voucher));
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
    }
}