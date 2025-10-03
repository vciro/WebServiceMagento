using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblTerceroDao
    {
        private SqlConnection Conexion { get; set; }

        public tblTerceroDao(string CadenaConexion)
        {
            Conexion = new SqlConnection(CadenaConexion);
        }

        public tblTerceroItem ObtenerTercero(long Id, long IdEmpresa)
        {
            tblTerceroItem Item = new tblTerceroItem();
            SqlCommand oSQL = new SqlCommand("spObtenerTercero", Conexion);
            oSQL.CommandType = CommandType.StoredProcedure;
            try
            {
                if (Conexion.State == ConnectionState.Closed) { 
                    Conexion.Open();
                }
                oSQL.Parameters.Add(new SqlParameter("@id", Id));
                oSQL.Parameters.Add(new SqlParameter("@IdEmpresa", IdEmpresa));
                SqlDataReader reader = oSQL.ExecuteReader();
                if (reader.Read())
                {
                    Item = ObtenerItem(reader);
                }
                reader.Close();

                SqlCommand oSQLSAP = new SqlCommand("spObtenerTerceroSAP", Conexion);
                var CardCode = "";
                if (Item.Identificacion.Contains("-"))
                {
                    CardCode = $"CN{Item.Identificacion.Split('-')[0].Trim()}";
                }
                else
                {
                    CardCode = $"CN{Item.Identificacion.Trim()}";
                }
                oSQLSAP.CommandType = CommandType.StoredProcedure;
                oSQLSAP.Parameters.Add(new SqlParameter("@CardCode", CardCode));
                SqlDataReader readerSAP = oSQLSAP.ExecuteReader();
                if (readerSAP.Read())
                {
                    Item.U_BPCO_RTC = readerSAP["U_BPCO_RTC"].ToString();
                    Item.U_BPCO_TDC = readerSAP["U_BPCO_TDC"].ToString();
                    Item.U_HBT_TipDoc = readerSAP["U_HBT_TipDoc"].ToString();
                    Item.U_HBT_Nombres = readerSAP["U_HBT_Nombres"].ToString();
                    Item.U_HBT_Apellido1 = readerSAP["U_HBT_Apellido1"].ToString();
                    Item.U_HBT_Apellido2 = readerSAP["U_HBT_Apellido2"].ToString();
                    Item.U_HBT_ActEco = readerSAP["U_HBT_ActEco"].ToString();
                    Item.U_HBT_TipEnt = readerSAP["U_HBT_TipEnt"].ToString();
                    Item.U_HBT_TipExt = readerSAP["U_HBT_TipExt"].ToString();
                    Item.U_HBT_RegFis = readerSAP["U_HBT_RegFis"].ToString();
                    Item.U_HBT_InfoTrib = readerSAP["U_HBT_InfoTrib"].ToString();
                    Item.U_HBT_ResFis = readerSAP["U_HBT_ResFis"].ToString();
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

        public tblTerceroItem ObtenerTerceroPorCardCode(string CardCode, long idEmpresa)
        {
            tblTerceroItem Item = new tblTerceroItem();
            SqlCommand oSQL = new SqlCommand("spObtenerTerceroPorCardCode", Conexion);
            try
            {
                Conexion.Open();
                oSQL.CommandType = CommandType.StoredProcedure;
                oSQL.Parameters.Add(new SqlParameter("@CardCode", CardCode));
                oSQL.Parameters.Add(new SqlParameter("@idEmpresa", idEmpresa));
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

        public void ActualizarTerceroCardCode(long idEmpresa)
        {
            List<long> terceros  = new List<long>();
            SqlCommand oSQL = new SqlCommand("spObtenerTercerosCardCodeSync", Conexion);
            try
            {
                Conexion.Open();
                oSQL.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = oSQL.ExecuteReader()) {
                    while (reader.Read())
                    {
                        terceros.Add(long.Parse(reader["idTercero"].ToString()));
                    }
                }
                foreach (long t in terceros) {
                    tblTerceroItem oTItem = ObtenerTercero(t, idEmpresa);
                    if (Conexion.State == ConnectionState.Closed)
                    {
                        Conexion.Open();
                    }
                    if (oTItem != null && oTItem.IdTercero > 0) {
                        var CardCode = $"CN{oTItem.Identificacion}";
                        if (oTItem.Identificacion.Contains('-')) {
                            CardCode = $"CN{oTItem.Identificacion.Split('-')[0]}";
                        }
                        SqlCommand oSQLUpdate = new SqlCommand("spActualizarTercerosCardCode", Conexion);
                        oSQLUpdate.Parameters.Add(new SqlParameter("@idTercero", t));
                        oSQLUpdate.Parameters.Add(new SqlParameter("@CardCode", CardCode));
                        oSQLUpdate.CommandType = CommandType.StoredProcedure;
                        oSQLUpdate.ExecuteNonQuery();
                    }
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
        }

        private tblTerceroItem ObtenerItem(SqlDataReader reader)
        {
            tblTerceroItem Item = new tblTerceroItem();
            Item.IdTercero = long.Parse(reader["idTercero"].ToString());
            Item.idTipoIdentificacion = short.Parse(reader["idTipoIdentificacion"].ToString());
            Item.Identificacion = reader["Identificacion"].ToString();
            Item.Nombre = reader["Nombre"].ToString();
            Item.Telefono = reader["Telefono"].ToString();
            Item.Celular = reader["Celular"].ToString();
            Item.Mail = reader["Mail"].ToString();
            Item.Direccion = reader["Direccion"].ToString();
            Item.idCiudad = short.Parse(reader["idCiudad"].ToString());
            Item.Ciudad = reader["Ciudad"].ToString();
            Item.idEmpresa = long.Parse(reader["idEmpresa"].ToString());
            Item.Empresa = reader["Empresa"].ToString();
            Item.TipoTercero = reader["TipoTercero"].ToString();
            if (reader["FechaNacimiento"].ToString() != "")
            {
                Item.FechaNacimiento = DateTime.Parse(reader["FechaNacimiento"].ToString());
            }
            if (reader["IdListaPrecio"].ToString() != "")
            {
                Item.IdListaPrecio = long.Parse(reader["IdListaPrecio"].ToString());
            }
            Item.idGrupoCliente = long.Parse(reader["idGrupoCliente"].ToString());
            Item.GrupoCliente = reader["GrupoCliente"].ToString();
            Item.Generico = bool.Parse(reader["Generico"].ToString());
            Item.Observaciones = reader["Observaciones"].ToString();
            Item.Activo = bool.Parse(reader["Activo"].ToString());
            Item.ProteccionDatos = bool.Parse(reader["ProteccionDatos"].ToString());
            Item.TipoPersona = short.Parse(reader["TipoPersona"].ToString());
            Item.FrecuenciaCompra = int.Parse(reader["FrecuenciaCompra"].ToString());
            Item.IdBarrio = int.Parse(reader["IdBarrio"].ToString());
            Item.Barrio = reader["Barrio"].ToString();
            Item.InformacionComercial = int.Parse(reader["InformacionComercial"].ToString());
            Item.TipoFactura = int.Parse(reader["TipoFactura"].ToString());
            return Item;
        }

        private bool Insertar(tblTerceroItem Item)
        {
            Conexion.Open();
            SqlTransaction oTran;
            oTran = Conexion.BeginTransaction();
            SqlCommand oSQL = new SqlCommand("spInsertarTercero", Conexion, oTran);
            oSQL.CommandType = System.Data.CommandType.StoredProcedure;
            oSQL.Parameters.Add(new SqlParameter("@idTipoIdentificacion", Item.idTipoIdentificacion));
            oSQL.Parameters.Add(new SqlParameter("@Identificacion", Item.Identificacion));
            oSQL.Parameters.Add(new SqlParameter("@Nombre", Item.Nombre));
            oSQL.Parameters.Add(new SqlParameter("@Telefono", Item.Telefono));
            oSQL.Parameters.Add(new SqlParameter("@Celular", Item.Celular));
            oSQL.Parameters.Add(new SqlParameter("@Mail", Item.Mail));
            oSQL.Parameters.Add(new SqlParameter("@Direccion", Item.Direccion));
            oSQL.Parameters.Add(new SqlParameter("@idCiudad", Item.idCiudad));
            oSQL.Parameters.Add(new SqlParameter("@idEmpresa", Item.idEmpresa));
            oSQL.Parameters.Add(new SqlParameter("@TipoTercero", Item.TipoTercero));
            if (Item.FechaNacimiento != null && Item.FechaNacimiento != DateTime.MinValue)
            {
                oSQL.Parameters.Add(new SqlParameter("@FechaNacimiento", Item.FechaNacimiento));
            }
            else
            {
                oSQL.Parameters.Add(new SqlParameter("@FechaNacimiento", DBNull.Value));
            }
            if (Item.IdListaPrecio != 0)
            {
                oSQL.Parameters.Add(new SqlParameter("@IdListaPrecio", Item.IdListaPrecio));
            }
            else
            {
                oSQL.Parameters.Add(new SqlParameter("@IdListaPrecio", DBNull.Value));
            }
            if (Item.idGrupoCliente != 0)
            {
                oSQL.Parameters.Add(new SqlParameter("@idGrupoCliente", Item.idGrupoCliente));
            }
            else
            {
                oSQL.Parameters.Add(new SqlParameter("@idGrupoCliente", DBNull.Value));
            }
            oSQL.Parameters.Add(new SqlParameter("@FechaCreacion", Item.FechaCreacion));
            if (string.IsNullOrEmpty(Item.Observaciones))
            {
                oSQL.Parameters.Add(new SqlParameter("@Observaciones", DBNull.Value));
            }
            else
            {
                oSQL.Parameters.Add(new SqlParameter("@Observaciones", Item.Observaciones));
            }
            oSQL.Parameters.Add(new SqlParameter("@Activo", Item.Activo));
            oSQL.Parameters.Add(new SqlParameter("@ProteccionDatos", Item.ProteccionDatos));
            oSQL.Parameters.Add(new SqlParameter("@IdUsuario", Item.IdUsuario));
            oSQL.Parameters.Add(new SqlParameter("@TipoPersona", Item.TipoPersona));
            oSQL.Parameters.Add(new SqlParameter("@FrecuenciaCompra", Item.FrecuenciaCompra));
            if (string.IsNullOrEmpty(Item.EstablecimientoComercial))
            {
                oSQL.Parameters.Add(new SqlParameter("@EstablecimientoComercial", DBNull.Value));
            }
            else
            {
                oSQL.Parameters.Add(new SqlParameter("@EstablecimientoComercial", Item.EstablecimientoComercial));
            }
            oSQL.Parameters.Add(new SqlParameter("@IdBarrio", Item.IdBarrio));
            oSQL.Parameters.Add(new SqlParameter("@InformacionComercial", Item.InformacionComercial));
            oSQL.Parameters.Add(new SqlParameter("@TipoFactura", Item.TipoFactura));
            try
            {
                Item.IdTercero = long.Parse(oSQL.ExecuteScalar().ToString());
                SqlCommand oSQL1 = new SqlCommand("spEliminarDatosTercero", Conexion, oTran);
                oSQL1.CommandType = CommandType.StoredProcedure;
                oSQL1.Parameters.Add(new SqlParameter("@IdTercero", Item.IdTercero));
                oSQL1.ExecuteNonQuery();
                foreach (tblTerceroDireccion Dir in Item.oListDirecciones)
                {
                    SqlCommand oSQL2 = new SqlCommand("spInsertarDireccionTercero", Conexion, oTran);
                    oSQL2.CommandType = CommandType.StoredProcedure;
                    oSQL2.Parameters.Add(new SqlParameter("@IdTercero", Item.IdTercero));
                    oSQL2.Parameters.Add(new SqlParameter("@Direccion", Dir.Direccion));
                    oSQL2.ExecuteNonQuery();
                }
                foreach (tblTerceroCelular Cel in Item.oListCelulares)
                {
                    SqlCommand oSQL3 = new SqlCommand("spInsertarCelularTercero", Conexion, oTran);
                    oSQL3.CommandType = CommandType.StoredProcedure;
                    oSQL3.Parameters.Add(new SqlParameter("@IdTercero", Item.IdTercero));
                    oSQL3.Parameters.Add(new SqlParameter("@Celular", Cel.Celular));
                    oSQL3.ExecuteNonQuery();
                }
                foreach (tblTerceroCorreo Cor in Item.oListCorreo)
                {
                    SqlCommand oSQL4 = new SqlCommand("spInsertarCorreoTercero", Conexion, oTran);
                    oSQL4.CommandType = CommandType.StoredProcedure;
                    oSQL4.Parameters.Add(new SqlParameter("@IdTercero", Item.IdTercero));
                    oSQL4.Parameters.Add(new SqlParameter("@Correo", Cor.Correo));
                    oSQL4.ExecuteNonQuery();
                }
                oTran.Commit();
            }
            catch (Exception ex)
            {
                oTran.Rollback();
                return false;
            }
            finally
            {
                if (Conexion.State == ConnectionState.Open)
                {
                    Conexion.Close();
                }
            }
            return true;
        }

        private bool Actualizar(tblTerceroItem Item)
        {
            Conexion.Open();
            SqlTransaction oTran;
            oTran = Conexion.BeginTransaction();
            SqlCommand oSQL = new SqlCommand("spActualizarTercero", Conexion, oTran);
            oSQL.CommandType = CommandType.StoredProcedure;
            oSQL.Parameters.Add(new SqlParameter("@idTercero", Item.IdTercero));
            oSQL.Parameters.Add(new SqlParameter("@idTipoIdentificacion", Item.idTipoIdentificacion));
            oSQL.Parameters.Add(new SqlParameter("@Identificacion", Item.Identificacion));
            oSQL.Parameters.Add(new SqlParameter("@Nombre", Item.Nombre));
            oSQL.Parameters.Add(new SqlParameter("@Telefono", Item.Telefono));
            oSQL.Parameters.Add(new SqlParameter("@Celular", Item.Celular));
            oSQL.Parameters.Add(new SqlParameter("@Mail", Item.Mail));
            oSQL.Parameters.Add(new SqlParameter("@Direccion", Item.Direccion));
            oSQL.Parameters.Add(new SqlParameter("@idCiudad", Item.idCiudad));
            oSQL.Parameters.Add(new SqlParameter("@idEmpresa", Item.idEmpresa));
            oSQL.Parameters.Add(new SqlParameter("@TipoTercero", Item.TipoTercero));
            if (Item.FechaNacimiento != null && Item.FechaNacimiento != DateTime.MinValue)
            {
                oSQL.Parameters.Add(new SqlParameter("@FechaNacimiento", Item.FechaNacimiento));
            }
            else
            {
                oSQL.Parameters.Add(new SqlParameter("@FechaNacimiento", DBNull.Value));
            }
            if (Item.IdListaPrecio != 0)
            {
                oSQL.Parameters.Add(new SqlParameter("@IdListaPrecio", Item.IdListaPrecio));
            }
            else
            {
                oSQL.Parameters.Add(new SqlParameter("@IdListaPrecio", DBNull.Value));
            }
            if (Item.idGrupoCliente != 0)
            {
                oSQL.Parameters.Add(new SqlParameter("@idGrupoCliente", Item.idGrupoCliente));
            }
            else
            {
                oSQL.Parameters.Add(new SqlParameter("@idGrupoCliente", DBNull.Value));
            }
            oSQL.Parameters.Add(new SqlParameter("@FechaModificacion", Item.FechaModificacion));
            if (string.IsNullOrEmpty(Item.Observaciones))
            {
                oSQL.Parameters.Add(new SqlParameter("@Observaciones", DBNull.Value));
            }
            else
            {
                oSQL.Parameters.Add(new SqlParameter("@Observaciones", Item.Observaciones));
            }
            oSQL.Parameters.Add(new SqlParameter("@Activo", Item.Activo));
            oSQL.Parameters.Add(new SqlParameter("@ProteccionDatos", Item.ProteccionDatos));
            oSQL.Parameters.Add(new SqlParameter("@TipoPersona", Item.TipoPersona));
            oSQL.Parameters.Add(new SqlParameter("@FrecuenciaCompra", Item.FrecuenciaCompra));
            if (string.IsNullOrEmpty(Item.EstablecimientoComercial))
            {
                oSQL.Parameters.Add(new SqlParameter("@EstablecimientoComercial", DBNull.Value));
            }
            else
            {
                oSQL.Parameters.Add(new SqlParameter("@EstablecimientoComercial", Item.EstablecimientoComercial));
            }
            oSQL.Parameters.Add(new SqlParameter("@IdBarrio", Item.IdBarrio));
            oSQL.Parameters.Add(new SqlParameter("@IdUsuarioModificacion", Item.IdUsuarioModificacion));
            oSQL.Parameters.Add(new SqlParameter("@InformacionComercial", Item.InformacionComercial));
            oSQL.Parameters.Add(new SqlParameter("@TipoFactura", Item.TipoFactura));
            try
            {
                oSQL.ExecuteNonQuery();
                SqlCommand oSQL1 = new SqlCommand("spEliminarDatosTercero", Conexion, oTran);
                oSQL1.CommandType = System.Data.CommandType.StoredProcedure;
                oSQL1.Parameters.Add(new SqlParameter("@IdTercero", Item.IdTercero));
                oSQL1.ExecuteNonQuery();
                foreach (tblTerceroDireccion Dir in Item.oListDirecciones)
                {
                    SqlCommand oSQL2 = new SqlCommand("spInsertarDireccionTercero", Conexion, oTran);
                    oSQL2.CommandType = System.Data.CommandType.StoredProcedure;
                    oSQL2.Parameters.Add(new SqlParameter("@IdTercero", Item.IdTercero));
                    oSQL2.Parameters.Add(new SqlParameter("@Direccion", Dir.Direccion));
                    oSQL2.ExecuteNonQuery();
                }
                foreach (tblTerceroCelular Cel in Item.oListCelulares)
                {
                    SqlCommand oSQL3 = new SqlCommand("spInsertarCelularTercero", Conexion, oTran);
                    oSQL3.CommandType = System.Data.CommandType.StoredProcedure;
                    oSQL3.Parameters.Add(new SqlParameter("@IdTercero", Item.IdTercero));
                    oSQL3.Parameters.Add(new SqlParameter("@Celular", Cel.Celular));
                    oSQL3.ExecuteNonQuery();
                }
                foreach (tblTerceroCorreo Cor in Item.oListCorreo)
                {
                    SqlCommand oSQL4 = new SqlCommand("spInsertarCorreoTercero", Conexion, oTran);
                    oSQL4.CommandType = System.Data.CommandType.StoredProcedure;
                    oSQL4.Parameters.Add(new SqlParameter("@IdTercero", Item.IdTercero));
                    oSQL4.Parameters.Add(new SqlParameter("@Correo", Cor.Correo));
                    oSQL4.ExecuteNonQuery();
                }
                oTran.Commit();
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

        public bool Guardar(tblTerceroItem Item)
        {
            if (Item.IdTercero > 0)
            {
                return Actualizar(Item);
            }
            else
            {
                return Insertar(Item);
            }
        }
    }
}