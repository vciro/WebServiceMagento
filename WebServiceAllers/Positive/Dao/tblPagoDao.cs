using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblPagoDao
    {
        private SqlConnection Conexion { get; set; }

        public tblPagoDao(string CadenaConexion)
        {
            Conexion = new SqlConnection(CadenaConexion);
        }

        public bool GuardarPagoConTransaccion(tblPagoItem Item, List<tblPagoDetalleItem> detallePago, List<tblTipoPagoItem> tipoPago, long tipoDocumento, SqlConnection oCon, SqlTransaction oTran)
        {
            string SQL = "";
            if (tipoDocumento == 1 || tipoDocumento == 9)
            {
                SQL = "spInsertarPago";
            }
            if (tipoDocumento == 2)
            {
                SQL = "spInsertarPagoCompra";
            }
            SqlCommand oSQL = new SqlCommand(SQL, oCon, oTran);
            oSQL.CommandType = CommandType.StoredProcedure;
            oSQL.Parameters.Add(new SqlParameter("@idTercero", Item.idTercero));
            oSQL.Parameters.Add(new SqlParameter("@totalPago", Item.totalPago));
            oSQL.Parameters.Add(new SqlParameter("@idEmpresa", Item.idEmpresa));
            oSQL.Parameters.Add(new SqlParameter("@fechaPago", Item.fechaPago));
            oSQL.Parameters.Add(new SqlParameter("@idUsuario", Item.idUsuario));
            oSQL.Parameters.Add(new SqlParameter("@idEstado", Item.idEstado));
            oSQL.Parameters.Add(new SqlParameter("@idTipoPago", Item.idTipoPago));
            if (Item.saldoCliente != 0)
            {
                oSQL.Parameters.Add(new SqlParameter("@saldoCliente", Item.saldoCliente));
            }
            else
            {
                oSQL.Parameters.Add(new SqlParameter("@saldoCliente", DBNull.Value));
            }
            if (!string.IsNullOrEmpty(Item.Observaciones))
            {
                oSQL.Parameters.Add(new SqlParameter("@Observaciones", Item.Observaciones));
            }
            else
            {
                oSQL.Parameters.Add(new SqlParameter("@Observaciones", DBNull.Value));
            }
            oSQL.Parameters.Add(new SqlParameter("@EnCuadre", Item.EnCuadre));
            try
            {
                Item.idPago = long.Parse(oSQL.ExecuteScalar().ToString());
                if (Item.idPago > 0)
                {
                    tblPagoDetalleDao oPagDetD = new tblPagoDetalleDao(oCon.ConnectionString);
                    tblTipoPagoDao oPagTipD = new tblTipoPagoDao(oCon.ConnectionString);
                    tblDocumentoDao oDocD = new tblDocumentoDao(oCon.ConnectionString);
                    foreach (tblPagoDetalleItem oPagDetI in detallePago)
                    {
                        if (oPagDetI.valorAbono > 0)
                        {
                            oPagDetI.idPago = Item.idPago;
                            oPagDetI.IdTipoDocumento = short.Parse(tipoDocumento.ToString());
                            oPagDetD.Guardar(oPagDetI, oCon, oTran);
                        }
                        else
                        {
                            oTran.Rollback();
                            return false;
                        }
                    }
                    foreach (tblTipoPagoItem oTipPagI in tipoPago)
                    {
                        if (oTipPagI.ValorPago > 0)
                        {
                            oTipPagI.idPago = Item.idPago;
                            oPagTipD.Guardar(oTipPagI, oCon, oTran, tipoDocumento);
                        }
                        else
                        {
                            oTran.Rollback();
                            return false;
                        }
                    }
                }
                else
                {
                    oTran.Rollback();
                    return false;
                }
            }
            catch
            {
                oTran.Rollback();
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