using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblDocumentoDao
    {
        private SqlConnection Conexion { get; set; }

        public tblDocumentoDao(string CadenaConexion)
        {
            Conexion = new SqlConnection(CadenaConexion);
        }

        public tblDocumentoItem ObtenerDocumento(long Id, long tipoDocumento)
        {
            tblDocumentoItem Item = new tblDocumentoItem();
            tblTipoDocumentoItem oTDItem;
            tblTipoDocumentoDao oTDDao = new tblTipoDocumentoDao(Conexion.ConnectionString);
            oTDItem = oTDDao.ObtenerTipoDocumento(tipoDocumento);
            string strSQL = string.Format(@"SELECT T0.idDocumento,
                T0.NumeroDocumento,
                T0.Fecha,
                T0.idTercero,
                T0.Telefono,
                T0.Direccion,
                T0.idCiudad,
                T0.Nombre,
                T2.Identificacion,
                T0.Observaciones,
                T0.idEmpresa,
                T0.idUsuario,
                FLOOR(T0.TotalDocumento) [TotalDocumento],
                FLOOR(T0.TotalIVA) [TotalIVA],
                FLOOR(T0.saldo) [saldo],
                ISNULL(T0.IdEstado,1)[IdEstado],
                ISNULL(T1.Estado,'Activo')[Estado],
                ISNULL(T0.Devuelta,0)[Devuelta],
                ISNULL(T0.Referencia,'')[Referencia],
                ISNULL(T0.TotalDescuento,0)[TotalDescuento],
                ISNULL(T0.TotalAntesIVA,0)[TotalAntesIVA],
                ISNULL(T0.Resolucion,'')[Resolucion],
                T3.NombreCompleto [Usuario],
                ISNULL(T0.IdSede,-1)[IdSede],
                ISNULL(T0.FechaVencimiento,T0.Fecha)[FechaVencimiento],
                ISNULL(T0.IdUnidadNegocio,1)[IdUnidadNegocio],
                ISNULL(ISNULL(T0.IdBarrio,T2.IdBarrio),0) [IdBarrio],
                ISNULL(T4.Nombre,T5.Nombre) [Barrio]
                FROM {0} T0
                LEFT JOIN tblEstadoFactura T1 ON T1.IdEstado = T0.IdEstado
                INNER JOIN tblTercero T2 ON T2.idTercero = T0.idTercero
                INNER JOIN tblUsuario T3 ON T3.idUsuario = T0.idUsuario
                LEFT JOIN tblBarrio T4 ON T4.IdBarrio = T0.IdBarrio
                LEFT JOIN tblBarrio T5 ON T5.IdBarrio = T2.IdBarrio
                WHERE idDocumento = @id;", oTDItem.TablaDocumento);
            SqlCommand oSQL = new SqlCommand(strSQL, Conexion);
            oSQL.Parameters.Add(new SqlParameter("@id", Id));
            Conexion.Open();
            try
            {
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

        private tblDocumentoItem ObtenerItem(SqlDataReader reader)
        {
            tblDocumentoItem Item = new tblDocumentoItem();
            Item.idDocumento = long.Parse(reader["idDocumento"].ToString());
            Item.NumeroDocumento = reader["NumeroDocumento"].ToString();
            Item.Fecha = DateTime.Parse(reader["Fecha"].ToString());
            Item.idTercero = long.Parse(reader["idTercero"].ToString());
            Item.Telefono = reader["Telefono"].ToString();
            Item.Direccion = reader["Direccion"].ToString();
            Item.idCiudad = short.Parse(reader["idCiudad"].ToString());
            Item.NombreTercero = reader["Nombre"].ToString();
            Item.Identificacion = reader["Identificacion"].ToString();
            Item.Observaciones = reader["Observaciones"].ToString();
            Item.idEmpresa = long.Parse(reader["idEmpresa"].ToString());
            Item.idUsuario = long.Parse(reader["idUsuario"].ToString());
            Item.TotalDocumento = decimal.Parse(reader["TotalDocumento"].ToString());
            Item.TotalIVA = decimal.Parse(reader["TotalIVA"].ToString());
            if (reader["saldo"].ToString() != "")
            {
                Item.saldo = decimal.Parse(reader["saldo"].ToString());
            }
            Item.IdEstado = long.Parse(reader["IdEstado"].ToString());
            Item.Estado = reader["Estado"].ToString();
            Item.Devuelta = decimal.Parse(reader["Devuelta"].ToString());
            Item.Referencia = reader["Referencia"].ToString();
            Item.TotalDescuento = decimal.Parse(reader["TotalDescuento"].ToString());
            Item.TotalAntesIVA = decimal.Parse(reader["TotalAntesIVA"].ToString());
            Item.Resolucion = reader["Resolucion"].ToString();
            Item.Usuario = reader["Usuario"].ToString();
            Item.IdSede = int.Parse(reader["IdSede"].ToString());
            Item.FechaVencimiento = DateTime.Parse(reader["FechaVencimiento"].ToString());
            Item.IdUnidadNegocio = int.Parse(reader["IdUnidadNegocio"].ToString());
            int IdBarrio = 0;
            if (int.TryParse(reader["IdBarrio"].ToString(), out IdBarrio) && IdBarrio > 0)
            {
                Item.IdBarrio = int.Parse(reader["IdBarrio"].ToString());
                Item.Barrio = reader["Barrio"].ToString();
            }
            return Item;
        }

        public List<string> ObtenerPedidoPorReferencia(string Referencia) {
            List<string> documentos = new List<string>();
            try
            {
                SqlCommand oSQL = new SqlCommand("spObtenerPedidoPorReferencia", Conexion);
                oSQL.Parameters.Add(new SqlParameter("@Referencia", Referencia));
                oSQL.CommandType = CommandType.StoredProcedure;
                Conexion.Open();
                SqlDataReader reader = oSQL.ExecuteReader();
                while (reader.Read()) {
                    documentos.Add(reader["NumeroDocumento"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
                if (Conexion.State == ConnectionState.Open) {
                    Conexion.Close();
                }
            }
            return documentos;
        }

        public List<string> ExistePedidoCom(string IdPedido)
        {
            List<string> result = new List<string>();
            try
            {
                SqlCommand oSQL = new SqlCommand($@"SELECT	ISNULL(Error,'') Error
                                                    FROM	[dbo].[tblPedidoComPedidos] 
                                                    WHERE	idPedidoCom IN (
                                                    SELECT idDocumento FROM [tblDocumentoCampoValor] WHERE [Nombre] = 'U_NumOrderMedishop' AND [Valor] = '{IdPedido}' AND idTipoDocumento = 23)", Conexion);
                oSQL.CommandType = CommandType.Text;
                Conexion.Open();
                var reader = oSQL.ExecuteReader();
                while (reader.Read()) {
                    result.Add(reader["Error"].ToString());
                }
                return result;
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
        }

        public long getPedidoComID(string IdPedido)
        {
            long result = 0;
            try
            {
                SqlCommand oSQL = new SqlCommand($@"SELECT idDocumento FROM [tblDocumentoCampoValor] WHERE [Nombre] = 'U_NumOrderMedishop' AND [Valor] = '{IdPedido}' AND idTipoDocumento = 23", Conexion);
                oSQL.CommandType = CommandType.Text;
                Conexion.Open();
                var reader = oSQL.ExecuteReader();
                if (reader.Read())
                {
                    result = long.Parse(decimal.Parse(reader["idDocumento"].ToString()).ToString());
                }
                return result;
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
        }

        public string GuardarTodo(tblDocumentoItem oDocItem, List<tblDetalleDocumentoItem> oListDet, tblPagoItem oPagoI, List<tblTipoPagoItem> oTipPagLis)
        {
            var errorCantidad = "";
            foreach (var d in oListDet)
            {
                if (d.Cantidad == 0)
                {
                    errorCantidad = $"{errorCantidad}<br/>El articulo {d.Codigo} - {d.Articulo} tiene cantidad 0";
                }
            }
            if (!string.IsNullOrEmpty(errorCantidad))
            {
                return $"Errores: <br/>{errorCantidad}";
            }
            Conexion.Open();
            SqlTransaction oTran;
            oTran = Conexion.BeginTransaction();
            try
            {
                SqlCommand oSQL = new SqlCommand("spGuardarDocumento", Conexion, oTran);
                oSQL.CommandType = CommandType.StoredProcedure;
                oSQL.Parameters.Add(new SqlParameter("@TipoDocumento", oDocItem.IdTipoDocumento));
                oSQL.Parameters.Add(new SqlParameter("@Fecha", oDocItem.Fecha));
                oSQL.Parameters.Add(new SqlParameter("@idTercero", oDocItem.idTercero));
                oSQL.Parameters.Add(new SqlParameter("@Telefono", oDocItem.Telefono));
                oSQL.Parameters.Add(new SqlParameter("@Direccion", oDocItem.Direccion));
                oSQL.Parameters.Add(new SqlParameter("@idCiudad", oDocItem.idCiudad));
                oSQL.Parameters.Add(new SqlParameter("@NombreTercero", oDocItem.NombreTercero));
                oSQL.Parameters.Add(new SqlParameter("@Observaciones", oDocItem.Observaciones));
                oSQL.Parameters.Add(new SqlParameter("@idEmpresa", oDocItem.idEmpresa));
                oSQL.Parameters.Add(new SqlParameter("@idUsuario", oDocItem.idUsuario));
                oSQL.Parameters.Add(new SqlParameter("@TotalDocumento", oDocItem.TotalDocumento));
                oSQL.Parameters.Add(new SqlParameter("@TotalIVA", oDocItem.TotalIVA));
                oSQL.Parameters.Add(new SqlParameter("@saldo", oDocItem.saldo));
                oSQL.Parameters.Add(new SqlParameter("@IdEstado", oDocItem.IdEstado));
                oSQL.Parameters.Add(new SqlParameter("@EnCuadre", oDocItem.EnCuadre));
                oSQL.Parameters.Add(new SqlParameter("@Devuelta", oDocItem.Devuelta));
                oSQL.Parameters.Add(new SqlParameter("@TotalDescuento", oDocItem.TotalDescuento));
                oSQL.Parameters.Add(new SqlParameter("@TotalAntesIVA", oDocItem.TotalAntesIVA));
                oSQL.Parameters.Add(new SqlParameter("@IdUsuarioAnulo", DBNull.Value));
                oSQL.Parameters.Add(new SqlParameter("@FechaAnulo", DBNull.Value));
                oSQL.Parameters.Add(new SqlParameter("@BaseTipoDocumento", oDocItem.BaseIdTipoDocumento));
                oSQL.Parameters.Add(new SqlParameter("@FechaVencimiento", oDocItem.FechaVencimiento));
                if (string.IsNullOrEmpty(oDocItem.Referencia))
                {
                    oSQL.Parameters.Add(new SqlParameter("@Referencia", DBNull.Value));
                }
                else
                {
                    oSQL.Parameters.Add(new SqlParameter("@Referencia", oDocItem.Referencia));
                }
                if (oDocItem.IdSede == -1)
                {
                    oSQL.Parameters.Add(new SqlParameter("@IdSede", DBNull.Value));
                }
                else
                {
                    oSQL.Parameters.Add(new SqlParameter("@IdSede", oDocItem.IdSede));
                }
                oSQL.Parameters.Add(new SqlParameter("@IdUnidadNegocio", oDocItem.IdUnidadNegocio));
                if (oDocItem.IdBarrio == 0)
                {
                    oSQL.Parameters.Add(new SqlParameter("@IdBarrio", DBNull.Value));
                }
                else
                {
                    oSQL.Parameters.Add(new SqlParameter("@IdBarrio", oDocItem.IdBarrio));
                }

                oSQL.Parameters.Add(new SqlParameter("@NumeroDocumento", oDocItem.NumeroDocumento));
                oSQL.Parameters.Add(new SqlParameter("@Series", oDocItem.Series));
                string Valores = oSQL.ExecuteScalar().ToString();
                if (Valores == "ERROR")
                {
                    return "El documento ya se genero por un mismo valor en menos de 1 minuto.";
                }
                else
                {
                    oDocItem.idDocumento = long.Parse(Valores.Split(',')[0]);
                    oDocItem.NumeroDocumento = Valores.Split(',')[1];
                    if (oPagoI.totalPago > 0)
                    {
                        tblPagoDao oPagoDao = new tblPagoDao(Conexion.ConnectionString);
                        List<tblPagoDetalleItem> oPagoDetList = new List<tblPagoDetalleItem>();
                        tblPagoDetalleItem oPagoDetI = new tblPagoDetalleItem();
                        oPagoDetI.idDocumento = oDocItem.idDocumento;
                        if (oPagoI.totalPago > oDocItem.TotalDocumento)
                        {
                            oPagoDetI.valorAbono = oDocItem.TotalDocumento;
                        }
                        else
                        {
                            oPagoDetI.valorAbono = oPagoI.totalPago;
                        }
                        oPagoDetList.Add(oPagoDetI);
                        if (!oPagoDao.GuardarPagoConTransaccion(oPagoI, oPagoDetList, oTipPagLis, oDocItem.IdTipoDocumento, Conexion, oTran))
                        {
                            oTran.Rollback();
                        }
                    }
                    foreach (tblDetalleDocumentoItem Detalle in oListDet)
                    {
                        SqlCommand oSQL1 = new SqlCommand("spGuardarDocumentoDetalle", Conexion, oTran);
                        oSQL1.CommandType = CommandType.StoredProcedure;
                        oSQL1.Parameters.Add(new SqlParameter("@TipoDocumento", oDocItem.IdTipoDocumento));
                        oSQL1.Parameters.Add(new SqlParameter("@idDocumento", oDocItem.idDocumento));
                        oSQL1.Parameters.Add(new SqlParameter("@NumeroLinea", Detalle.NumeroLinea));
                        oSQL1.Parameters.Add(new SqlParameter("@idArticulo", Detalle.idArticulo));
                        oSQL1.Parameters.Add(new SqlParameter("@Descripcion", Detalle.Articulo));
                        oSQL1.Parameters.Add(new SqlParameter("@Precio", Detalle.ValorUnitario));
                        oSQL1.Parameters.Add(new SqlParameter("@Impuesto", Detalle.IVA));
                        oSQL1.Parameters.Add(new SqlParameter("@Cantidad", Detalle.Cantidad));
                        oSQL1.Parameters.Add(new SqlParameter("@idBodega", Detalle.idBodega));
                        oSQL1.Parameters.Add(new SqlParameter("@Descuento", Detalle.Descuento));
                        oSQL1.Parameters.Add(new SqlParameter("@CostoPonderado", Detalle.CostoPonderado));
                        oSQL1.Parameters.Add(new SqlParameter("@CostoPonderadoAllers", Detalle.CostoPonderadoAllers));
                        oSQL1.Parameters.Add(new SqlParameter("@IdCampana", Detalle.IdCampana));
                        Detalle.idDetalleDocumento = long.Parse(oSQL1.ExecuteScalar().ToString());
                        if (oDocItem.IdTipoDocumento == 2 || oDocItem.IdTipoDocumento == 4 || oDocItem.IdTipoDocumento == 5)
                        {
                            SqlCommand oSQL2 = new SqlCommand("spCalcularCostoPonderado", Conexion, oTran);
                            oSQL2.CommandType = CommandType.StoredProcedure;
                            oSQL2.Parameters.Add(new SqlParameter("@idArticulo", Detalle.idArticulo));
                            oSQL2.Parameters.Add(new SqlParameter("@Cantidad", Detalle.Cantidad));
                            oSQL2.Parameters.Add(new SqlParameter("@ValorUnitario", Detalle.ValorUnitario));
                            if (string.IsNullOrEmpty(oDocItem.Referencia))
                            {
                                oSQL2.Parameters.Add(new SqlParameter("@NumeroDocumentoDevolucion", DBNull.Value));
                            }
                            else
                            {
                                oSQL2.Parameters.Add(new SqlParameter("@NumeroDocumentoDevolucion", oDocItem.Referencia));
                            }
                            oSQL2.ExecuteNonQuery();
                        }
                        if (oDocItem.IdTipoDocumento == 1)
                        {
                            SqlCommand oSQL3 = new SqlCommand("spGuardarCompraEstudiante", Conexion, oTran);
                            oSQL3.CommandType = CommandType.StoredProcedure;
                            oSQL3.Parameters.Add(new SqlParameter("@idArticulo", Detalle.idArticulo));
                            oSQL3.Parameters.Add(new SqlParameter("@idTercero", oDocItem.idTercero));
                            oSQL3.Parameters.Add(new SqlParameter("@DocNum", oDocItem.NumeroDocumento));
                            oSQL3.ExecuteNonQuery();
                        }
                    }
                    var exito = "Exito";
                    oTran.Commit();
                    return exito;
                }
            }
            catch (Exception ex)
            {
                oTran.Rollback();
                return ex.Message;
            }
        }
        private tblPedidoComPedidosItem ObtenerPedidoComItem(SqlDataReader reader) {
            var Item = new tblPedidoComPedidosItem();
            Item.idPedidoCom = int.Parse(reader["idPedidoCom"].ToString());
            Item.idPedido = int.Parse(reader["idPedido"].ToString());
            Item.Positive = bool.Parse(reader["Positive"].ToString());
            if (reader["WhsCode"] != DBNull.Value)
            {
                Item.WhsCode = reader["WhsCode"].ToString();
            }
            if (reader["Error"] != DBNull.Value)
            {
                Item.Error = reader["Error"].ToString();
            }
            Item.PedidoInterno = bool.Parse(reader["PedidoInterno"].ToString());
            return Item;
        }

        public tblPedidoComPedidosItem ObtenerPedidoComPedido(long idPedidoCom, string WhsCode, bool PedidoInterno) {
            var Item = new tblPedidoComPedidosItem();
            try
            {
                SqlCommand oSQL = new SqlCommand("spObtenerPedidoComPedido", Conexion);
                oSQL.CommandType = CommandType.StoredProcedure;
                oSQL.Parameters.Add(new SqlParameter("@idPedidoCom", idPedidoCom));
                oSQL.Parameters.Add(new SqlParameter("@WhsCode", WhsCode));
                oSQL.Parameters.Add(new SqlParameter("@PedidoInterno", PedidoInterno));
                Conexion.Open();
                var reader = oSQL.ExecuteReader();
                if (reader.Read()) {
                    Item = ObtenerPedidoComItem(reader);
                }
                return Item;
            }
            catch(Exception ex)
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
        }

        public List<tblPedidoComPedidosItem> ObtenerPedidoComPedidos(long idPedidoCom)
        {
            var result = new List<tblPedidoComPedidosItem>();
            try
            {
                SqlCommand oSQL = new SqlCommand("spObtenerPedidoComPedidos", Conexion);
                oSQL.CommandType = CommandType.StoredProcedure;
                oSQL.Parameters.Add(new SqlParameter("@idPedidoCom", idPedidoCom));
                Conexion.Open();
                var reader = oSQL.ExecuteReader();
                while(reader.Read())
                {
                    result.Add(ObtenerPedidoComItem(reader));
                }
                return result;
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
        }

        public bool Guardar(tblPedidoComPedidosItem Item)
        {
            var sqlString = "spInsertarPedidoComPedidos";
            try
            {
                var item_old = ObtenerPedidoComPedido(Item.idPedidoCom, Item.WhsCode, Item.PedidoInterno);
                if (item_old != null && item_old.idPedidoCom > 0) {
                    sqlString = "spActualizarPedidoComPedidos";
                }

                SqlCommand oSQL = new SqlCommand(sqlString, Conexion);
                oSQL.CommandType = CommandType.StoredProcedure;
                oSQL.Parameters.Add(new SqlParameter("@idPedidoCom", Item.idPedidoCom));
                oSQL.Parameters.Add(new SqlParameter("@idPedido", Item.idPedido));
                oSQL.Parameters.Add(new SqlParameter("@Positive", Item.Positive));
                oSQL.Parameters.Add(new SqlParameter("@WhsCode", Item.WhsCode));
                oSQL.Parameters.Add(new SqlParameter("@Error", Item.Error));
                oSQL.Parameters.Add(new SqlParameter("@PedidoInterno", Item.PedidoInterno));
                Conexion.Open();
                oSQL.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
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