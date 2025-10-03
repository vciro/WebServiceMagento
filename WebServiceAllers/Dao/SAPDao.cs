using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WebServiceAllers.Models;

namespace WebServiceAllers.Dao
{
    public class SAPDao
    {
        string ConnectionString;

        public SAPDao()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionSaluti"].ToString();
        }
        public DocumentoItem getInvoice(string Code, string Tipo)
        {
            DocumentoItem item = new DocumentoItem();
            SqlConnection oConexion = new SqlConnection(ConnectionString);
            SqlCommand oSQL = new SqlCommand("SELECT * FROM [@FACTURAELECTRONICA] WHERE Code = @Code AND U_Tipo = @U_Tipo", oConexion);
            try
            {
                oConexion.Open();
                oSQL.CommandType = System.Data.CommandType.Text;
                oSQL.Parameters.Add(new SqlParameter("@Code", Code));
                oSQL.Parameters.Add(new SqlParameter("@U_Tipo", Tipo));
                SqlDataReader reader = oSQL.ExecuteReader();
                if (reader.Read())
                {
                    item.Code = reader["Code"].ToString();
                    item.Name = reader["Name"].ToString();
                    item.U_CreatedDate = reader["U_CreatedDate"].ToString();
                    item.U_UploadDate = reader["U_UploadDate"].ToString();
                    item.U_XmlData = reader["U_XmlData"].ToString();
                    item.U_Respuesta = reader["U_Respuesta"].ToString();
                    item.U_Estado = reader["U_Estado"].ToString();
                    item.U_DocNum = int.Parse(reader["U_DocNum"].ToString());
                    item.U_Tipo = reader["U_Tipo"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oConexion.State == System.Data.ConnectionState.Open)
                {
                    oConexion.Close();
                }
            }
            return item;
        }
        public void UpdateInvoice(DocumentoItem data)
        {
            SqlConnection oConexion = new SqlConnection(ConnectionString);
            SqlCommand oSQL = new SqlCommand("UPDATE [@FACTURAELECTRONICA] SET  U_UploadDate = GETDATE(), U_Respuesta = @U_Respuesta, U_Estado = @U_Estado, U_XmlData = @U_XmlData WHERE Code = @Code AND U_Tipo = @U_Tipo", oConexion);
            try
            {
                oConexion.Open();
                oSQL.CommandType = System.Data.CommandType.Text;
                oSQL.Parameters.Add(new SqlParameter("@Code", data.Code));
                oSQL.Parameters.Add(new SqlParameter("@U_Respuesta", data.U_Respuesta));
                oSQL.Parameters.Add(new SqlParameter("@U_Estado", data.U_Estado));
                oSQL.Parameters.Add(new SqlParameter("@U_DocNum", data.U_DocNum));
                oSQL.Parameters.Add(new SqlParameter("@U_Tipo", data.U_Tipo));
                oSQL.Parameters.Add(new SqlParameter("@U_XmlData", data.U_XmlData));
                oSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oConexion.State == System.Data.ConnectionState.Open)
                {
                    oConexion.Close();
                }
            }
        }
        public void InsertInvoice(DocumentoItem data)
        {
            SqlConnection oConexion = new SqlConnection(ConnectionString);
            SqlCommand oSQL = new SqlCommand("INSERT INTO [@FACTURAELECTRONICA] VALUES (@Code, @Name, GETDATE(), NULL, @U_XmlData,@U_Respuesta,@U_Estado, @U_DocNum, @U_Tipo)", oConexion);
            try
            {
                oConexion.Open();
                oSQL.CommandType = System.Data.CommandType.Text;
                oSQL.Parameters.Add(new SqlParameter("@Code", data.Code));
                oSQL.Parameters.Add(new SqlParameter("@Name", data.Name));
                oSQL.Parameters.Add(new SqlParameter("@U_XmlData", data.U_XmlData));
                oSQL.Parameters.Add(new SqlParameter("@U_Respuesta", data.U_Respuesta));
                oSQL.Parameters.Add(new SqlParameter("@U_Estado", data.U_Estado));
                oSQL.Parameters.Add(new SqlParameter("@U_DocNum", data.U_DocNum));
                oSQL.Parameters.Add(new SqlParameter("@U_Tipo", data.U_Tipo));
                oSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oConexion.State == System.Data.ConnectionState.Open)
                {
                    oConexion.Close();
                }
            }
        }
        public string SaveInvoice(DocumentoItem data)
        {
            string result = "";
            try
            {
                DocumentoItem dataOld = getInvoice(data.Code, data.U_Tipo);
                if (string.IsNullOrEmpty(dataOld.Code))
                {
                    InsertInvoice(data);
                }
                else
                {
                    UpdateInvoice(data);
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        public int GetTotalLineas(int DocEntry, string Type)
        {
            int Total = 0;
            string Sentencia = string.Empty;
            SqlConnection oConexion = new SqlConnection(ConnectionString);
            if (Type == "INV" || Type == "ND")
            {
                Sentencia = @"SELECT COUNT(*)
                                FROM INV1
                                WHERE DocEntry = @DocEntry AND TreeType <> 'I'";
            }
            if (Type == "NC")
            {
                Sentencia = @"SELECT COUNT(*)
                                FROM RIN1
                                WHERE DocEntry = @DocEntry AND TreeType <> 'I'";
            }
            SqlCommand oSQL = new SqlCommand(Sentencia, oConexion);
            try
            {
                oConexion.Open();
                oSQL.CommandType = System.Data.CommandType.Text;
                oSQL.Parameters.Add(new SqlParameter("@DocEntry", DocEntry));
                Total = int.Parse(oSQL.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oConexion.State == System.Data.ConnectionState.Open)
                {
                    oConexion.Close();
                }
            }
            return Total;
        }

        public bool ExistePedidoAllersSaluti(string NumOrderMedishop)
        {
            int Total = 0;
            string Sentencia = string.Empty;
            SqlConnection oConexion = new SqlConnection(ConnectionString);
            Sentencia = $"SELECT COUNT(*) FROM [{ConfigurationManager.AppSettings["CompanyDBSAP"]}].dbo.ORDR T0 WHERE T0.CardCode = 'CN2543' AND T0.U_NumOrderMedishop = @NumOrderMedishop";
            SqlCommand oSQL = new SqlCommand(Sentencia, oConexion);
            try
            {
                oConexion.Open();
                oSQL.CommandType = System.Data.CommandType.Text;
                oSQL.Parameters.Add(new SqlParameter("@NumOrderMedishop", NumOrderMedishop));
                Total = int.Parse(oSQL.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oConexion.State == System.Data.ConnectionState.Open)
                {
                    oConexion.Close();
                }
            }
            return Total>0;
        }


        public string EnLetras(string num)
        {
            string res, dec = "";
            Int64 entero;
            int decimales;
            double nro;
            try
            {
                nro = Convert.ToDouble(num);
            }
            catch
            {
                return "";
            }
            entero = Convert.ToInt64(Math.Truncate(nro));
            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
            if (decimales > 0)
            {
                dec = " CON " + decimales.ToString() + "/100";
            }
            res = toText(Convert.ToDouble(entero)) + dec;
            return res;
        }
        private string toText(double value)
        {
            string Num2Text = "";
            value = Math.Truncate(value);
            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
            }
            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
            }
            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;
        }
        public List<ImpuestoItem> GetImpuestosGlobales(int DocEntry, string Type)
        {
            List<ImpuestoItem> list = new List<ImpuestoItem>();
            SqlConnection oConexion = new SqlConnection(ConnectionString);
            string Sentencia = string.Empty;
            if (Type == "INV" || Type == "ND")
            {
                Sentencia = @"SELECT S0.Codigo,
	                            S0.ValorImpuestoRetencion,
	                            S0.BaseImponible,
	                            S0.Porcentaje,
                                0 [IsAutoRetencion]
                            FROM (SELECT '01' [Codigo],
                                SUM(T1.VatSum)[ValorImpuestoRetencion],
                                SUM(T1.LineTotal)[BaseImponible],
                                T1.VatPrcnt[Porcentaje]
                            FROM OINV T0
                            INNER JOIN INV1 T1 ON T1.DocEntry = T0.DocEntry
                            WHERE T0.DocEntry = @DocEntry
                            AND T1.ItemCode NOT IN ('91339')
                            AND T1.TaxCode NOT IN ('IVA GE05')
                            GROUP BY T1.VatPrcnt) S0
                            UNION ALL
                            SELECT S0.Codigo,
	                            S0.ValorImpuestoRetencion,
	                            S0.BaseImponible,
	                            S0.Porcentaje,
                                0 [IsAutoRetencion]
                            FROM (SELECT '22' [Codigo],
                                SUM(T1.Quantity * 50)[ValorImpuestoRetencion],
                                50 [BaseImponible],
                                0 [Porcentaje]
                            FROM OINV T0
                            INNER JOIN INV1 T1 ON T1.DocEntry = T0.DocEntry
                            WHERE T0.DocEntry = @DocEntry
                            AND T1.ItemCode = '91339'
                            GROUP BY T1.VatPrcnt) S0
                            UNION ALL
                            SELECT ISNULL(T1.U_FE_Codigo,'') [Codigo],
	                            t0.TaxbleAmnt [ValorImpuestoRetencion],
	                            t0.U_HBT_BaseRet [BaseImponible],
	                            t1.PrctBsAmnt [Porcentaje],
                                1 [IsAutoRetencion]
                            FROM INV5 t0
                            INNER JOIN OWHT t1 on t1.WTCode = t0.WTCode
                            WHERE AbsEntry = @DocEntry
                            AND t1.U_FE_Codigo NOT IN ('99')";
            }
            if (Type == "NC")
            {
                Sentencia = @"SELECT S0.Codigo,
	                            S0.ValorImpuestoRetencion,
	                            S0.BaseImponible,
	                            S0.Porcentaje,
                                0 [IsAutoRetencion]
                            FROM (SELECT '01' [Codigo],
                                SUM(T1.VatSum)[ValorImpuestoRetencion],
                                SUM(T1.LineTotal)[BaseImponible],
                                T1.VatPrcnt[Porcentaje]
                            FROM ORIN T0
                            INNER JOIN RIN1 T1 ON T1.DocEntry = T0.DocEntry
                            WHERE T0.DocEntry = @DocEntry
                            AND T1.ItemCode NOT IN ('91339')
                            AND T1.TaxCode NOT IN ('IVA GE05')
                            GROUP BY T1.VatPrcnt) S0
                            UNION ALL
                            SELECT S0.Codigo,
	                            S0.ValorImpuestoRetencion,
	                            S0.BaseImponible,
	                            S0.Porcentaje,
                                0 [IsAutoRetencion]
                            FROM (SELECT '22' [Codigo],
                                SUM(T1.Quantity * 50)[ValorImpuestoRetencion],
                                50 [BaseImponible],
                                0 [Porcentaje]
                            FROM ORIN T0
                            INNER JOIN RIN1 T1 ON T1.DocEntry = T0.DocEntry
                            WHERE T0.DocEntry = @DocEntry
                            AND T1.ItemCode = '91339'
                            GROUP BY T1.VatPrcnt) S0
                            UNION ALL
                            SELECT ISNULL(T1.U_FE_Codigo,'') [Codigo],
	                            t0.TaxbleAmnt [ValorImpuestoRetencion],
	                            t0.U_HBT_BaseRet [BaseImponible],
	                            t1.PrctBsAmnt [Porcentaje],
                                1 [IsAutoRetencion]
                            FROM RIN5 t0
                            INNER JOIN OWHT t1 on t1.WTCode = t0.WTCode
                            WHERE AbsEntry = @DocEntry
                            AND t1.U_FE_Codigo NOT IN ('99')";
            }
            SqlCommand oSQL = new SqlCommand(Sentencia, oConexion);
            try
            {
                oConexion.Open();
                oSQL.CommandType = System.Data.CommandType.Text;
                oSQL.Parameters.Add(new SqlParameter("@DocEntry", DocEntry));
                SqlDataReader reader = oSQL.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new ImpuestoItem()
                    {
                        Codigo = reader["Codigo"].ToString(),
                        ValorImpuestoRetencion = double.Parse(reader["ValorImpuestoRetencion"].ToString()),
                        BaseImponible = double.Parse(reader["BaseImponible"].ToString()),
                        Porcentaje = double.Parse(reader["Porcentaje"].ToString()),
                        IsAutoRetencion = Convert.ToBoolean(reader["IsAutoRetencion"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oConexion.State == System.Data.ConnectionState.Open)
                {
                    oConexion.Close();
                }
            }
            return list;
        }
        public string ObtenerTotalDescuento(int DocEntry, string TipoDocumento)
        {
            string Nombre = string.Empty;
            SqlConnection oConexion = new SqlConnection(ConnectionString);
            string SQL = string.Empty;
            if (TipoDocumento == "NC")
            {
                SQL = @"SELECT SUM(FLOOR(T1.[Quantity] * T1.[PriceBefDi] * T1.[DiscPrcnt]/100)) 
                FROM RIN1 T1 WHERE T1.DocEntry = @DocEntry";
            }
            else
            {
                SQL = @"SELECT SUM(FLOOR(T1.[Quantity] * T1.[PriceBefDi] * T1.[DiscPrcnt]/100)) 
                FROM INV1 T1 WHERE T1.DocEntry = @DocEntry";
            }
            SqlCommand oSQL = new SqlCommand(SQL, oConexion);
            try
            {
                oConexion.Open();
                oSQL.CommandType = System.Data.CommandType.Text;
                oSQL.Parameters.Add(new SqlParameter("@DocEntry", DocEntry));
                Nombre = oSQL.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Nombre = ex.Message;
            }
            finally
            {
                if (oConexion.State == System.Data.ConnectionState.Open)
                {
                    oConexion.Close();
                }
            }
            return Nombre;
        }
        public string ObtenerSubtotal(int DocEntry, string TipoDocumento)
        {
            string Nombre = string.Empty;
            SqlConnection oConexion = new SqlConnection(ConnectionString);
            string SQL = string.Empty;
            if (TipoDocumento == "NC")
            {
                //SQL = @"SELECT (T0.[DocTotal] + T0.[DiscSum] - T0.[VatSum] - T0.[WTSum] + T0.[TotalExpns])
                //FROM ORIN T0 WHERE T0.DocEntry = @DocEntry";

                SQL = @"SELECT FLOOR(T0.[DocTotal] + T0.[DiscSum] + SUM((T1.[Quantity] * T1.[PriceBefDi] * T1.[DiscPrcnt]/100)) - T0.[TotalExpns] - T0.[VatSum] + T0.[WTSum] )
                    FROM ORIN T0
                    INNER JOIN RIN1 T1 ON T1.DocEntry = T0.DocEntry
                    WHERE T0.DocEntry = @DocEntry
                    GROUP BY T0.[DocTotal],T0.[DiscSum],T0.[TotalExpns], T0.[VatSum], T0.[WTSum]";
            }
            else
            {
                //SQL = @"SELECT (T0.[DocTotal] + T0.[DiscSum] - T0.[VatSum] - T0.[WTSum] + T0.[TotalExpns])
                //FROM OINV T0 WHERE T0.DocEntry = @DocEntry";

                SQL = @"SELECT FLOOR(T0.[DocTotal] + T0.[DiscSum] + SUM((T1.[Quantity] * T1.[PriceBefDi] * T1.[DiscPrcnt]/100)) - T0.[TotalExpns] - T0.[VatSum] + T0.[WTSum] )
                    FROM OINV T0 
                    INNER JOIN INV1 T1 ON T1.DocEntry = T0.DocEntry
                    WHERE T0.DocEntry = @DocEntry
                    GROUP BY T0.[DocTotal],T0.[DiscSum],T0.[TotalExpns], T0.[VatSum], T0.[WTSum]";
            }
            SqlCommand oSQL = new SqlCommand(SQL, oConexion);
            try
            {
                oConexion.Open();
                oSQL.CommandType = System.Data.CommandType.Text;
                oSQL.Parameters.Add(new SqlParameter("@DocEntry", DocEntry));
                Nombre = oSQL.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Nombre = ex.Message;
            }
            finally
            {
                if (oConexion.State == System.Data.ConnectionState.Open)
                {
                    oConexion.Close();
                }
            }
            return Nombre;
        }
        public string ObtenerNumeroPedido(int DocEntry)
        {
            string DocNum;
            SqlConnection oConexion = new SqlConnection(ConnectionString);
            SqlCommand oSQL = new SqlCommand(@"IF (SELECT COUNT(*)
                                                FROM INV1 T0
                                                INNER JOIN DLN1 T1 ON T0.BaseEntry = T1.DocEntry
                                                INNER JOIN RDR1 T2 ON T1.BaseEntry = T2.DocEntry
                                                INNER JOIN ORDR T3 ON T2.DocEntry = T3.DocEntry
                                                WHERE T0.DocEntry = @DocEntry) > 0
                                        BEGIN
	                                        SELECT TOP 1
	                                        ISNULL(T3.DocNum,0)
	                                        FROM INV1 T0
	                                        INNER JOIN DLN1 T1 ON T0.BaseEntry = T1.DocEntry
	                                        INNER JOIN RDR1 T2 ON T1.BaseEntry = T2.DocEntry
	                                        INNER JOIN ORDR T3 ON T2.DocEntry = T3.DocEntry
	                                        WHERE T0.DocEntry = @DocEntry
                                        END
                                        ELSE
                                        BEGIN
	                                        SELECT ''
                                        END", oConexion);
            try
            {
                oConexion.Open();
                oSQL.CommandType = System.Data.CommandType.Text;
                oSQL.Parameters.Add(new SqlParameter("@DocEntry", DocEntry));
                DocNum = oSQL.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oConexion.State == System.Data.ConnectionState.Open)
                {
                    oConexion.Close();
                }
            }
            return DocNum;
        }
        public string ObtenerFormaPago(int GroupNum)
        {
            string FormaPago = string.Empty;
            if (GroupNum != 0)
            {
                SqlConnection oConexion = new SqlConnection(ConnectionString);
                SqlCommand oSQL = new SqlCommand("SELECT ISNULL(PymntGroup,'') FROM OCTG where GroupNum = @GroupNum", oConexion);
                try
                {
                    oConexion.Open();
                    oSQL.CommandType = System.Data.CommandType.Text;
                    oSQL.Parameters.Add(new SqlParameter("@GroupNum", GroupNum));
                    FormaPago = oSQL.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    FormaPago = ex.Message;
                }
                finally
                {
                    if (oConexion.State == System.Data.ConnectionState.Open)
                    {
                        oConexion.Close();
                    }
                }
            }
            return FormaPago;
        }
        public string ObtenerResolucionDocumento(int Serie, string TipoDocumento)
        {
            string Sentencia = string.Empty;
            if (TipoDocumento == "INV")
            {
                Sentencia = @"SELECT ISNULL(U_Resolucion,'') FROM NNM1 T0
                    INNER JOIN [@RESOLUCION] T1 ON T1.Code = T0.SeriesName
                    WHERE T0.Series = @Serie;";
                SqlConnection oConexion = new SqlConnection(ConnectionString);
                SqlCommand oSQL = new SqlCommand(Sentencia, oConexion);
                try
                {
                    oConexion.Open();
                    oSQL.CommandType = System.Data.CommandType.Text;
                    oSQL.Parameters.Add(new SqlParameter("@Serie", Serie));
                    return oSQL.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return Sentencia;
            }
        }
        public double GetTotalBaseImponible(int DocEntry, string Type)
        {
            double Total = 0;
            string Sentencia = string.Empty;
            SqlConnection oConexion = new SqlConnection(ConnectionString);
            if (Type == "INV" || Type == "ND")
            {
                Sentencia = @"SELECT FLOOR(SUM(CASE WHEN ItemCode = '91339' THEN Price ELSE (CASE WHEN TaxCode NOT IN ('IVA GE05','IVA GE09') THEN LineTotal ELSE 0 END) END))
                                FROM INV1
                                WHERE DocEntry = @DocEntry";
            }
            if (Type == "NC")
            {
                Sentencia = @"SELECT FLOOR(SUM(CASE WHEN ItemCode = '91339' THEN Price ELSE (CASE WHEN TaxCode NOT IN ('IVA GE05','IVA GE09') THEN LineTotal ELSE 0 END) END))
                                FROM RIN1
                                WHERE DocEntry = @DocEntry";
            }
            SqlCommand oSQL = new SqlCommand(Sentencia, oConexion);
            try
            {
                oConexion.Open();
                oSQL.CommandType = System.Data.CommandType.Text;
                oSQL.Parameters.Add(new SqlParameter("@DocEntry", DocEntry));
                Total = double.Parse(oSQL.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oConexion.State == System.Data.ConnectionState.Open)
                {
                    oConexion.Close();
                }
            }
            return Total;
        }
        public string ObtenerNombreUnidadMedida(string UnidadMedida)
        {
            string Nombre = string.Empty;
            if (!string.IsNullOrEmpty(UnidadMedida))
            {
                SqlConnection oConexion = new SqlConnection(ConnectionString);
                SqlCommand oSQL = new SqlCommand(@"SELECT ISNULL(Name,'') FROM [@UNIDADMEDIDA]
                WHERE Code = @UnidadMedida;", oConexion);
                try
                {
                    oConexion.Open();
                    oSQL.CommandType = System.Data.CommandType.Text;
                    oSQL.Parameters.Add(new SqlParameter("@UnidadMedida", UnidadMedida));
                    Nombre = oSQL.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    Nombre = ex.Message;
                }
                finally
                {
                    if (oConexion.State == System.Data.ConnectionState.Open)
                    {
                        oConexion.Close();
                    }
                }
            }
            return Nombre;
        }
        public double GetTotalImporteBruto(int DocEntry, string Type)
        {
            double Total = 0;
            string Sentencia = string.Empty;
            SqlConnection oConexion = new SqlConnection(ConnectionString);
            if (Type == "INV" || Type == "ND")
            {
                Sentencia = @"SELECT FLOOR(SUM(LineTotal))
                                FROM INV1
                                WHERE DocEntry = @DocEntry
                                AND ItemCode NOT IN ('91339')";
            }
            if (Type == "NC")
            {
                Sentencia = @"SELECT FLOOR(SUM(LineTotal))
                                FROM RIN1
                                WHERE DocEntry = @DocEntry
                                AND ItemCode NOT IN ('91339')";
            }
            SqlCommand oSQL = new SqlCommand(Sentencia, oConexion);
            try
            {
                oConexion.Open();
                oSQL.CommandType = System.Data.CommandType.Text;
                oSQL.Parameters.Add(new SqlParameter("@DocEntry", DocEntry));
                Total = double.Parse(oSQL.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oConexion.State == System.Data.ConnectionState.Open)
                {
                    oConexion.Close();
                }
            }
            return Total;
        }
        public string getPrefijo(int Series)
        {
            SqlConnection oConexion = new SqlConnection(ConnectionString);
            SqlCommand oSQL = new SqlCommand("SELECT T0.SeriesName FROM NNM1 T0 WHERE T0.Series = @Series", oConexion);
            try
            {
                oConexion.Open();
                oSQL.CommandType = System.Data.CommandType.Text;
                oSQL.CommandTimeout = 1000000000;
                oSQL.Parameters.Add(new SqlParameter("@Series", Series));
                return oSQL.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oConexion.State == System.Data.ConnectionState.Open)
                {
                    oConexion.Close();
                }
            }
        }
        public int ObtenerDocEntryDocumentoReferenciado(int DocEntry, string Type)
        {
            int DocEntryFac = 0;
            SqlConnection oConexion = new SqlConnection(ConnectionString);
            SqlCommand oSQL;
            if (Type == "NC")
            {
                oSQL = new SqlCommand("SELECT TOP 1 RefDocEntr FROM RIN21 WHERE DocEntry = @DocEntry AND RefObjType = '13'", oConexion);
            }
            else
            {
                oSQL = new SqlCommand("SELECT TOP 1 RefDocEntr FROM INV21 WHERE DocEntry = @DocEntry AND RefObjType = '13'", oConexion);
            }
            try
            {
                oConexion.Open();
                oSQL.CommandType = System.Data.CommandType.Text;
                oSQL.CommandTimeout = 1000000000;
                oSQL.Parameters.Add(new SqlParameter("@DocEntry", DocEntry));
                DocEntryFac = int.Parse(oSQL.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oConexion.State == System.Data.ConnectionState.Open)
                {
                    oConexion.Close();
                }
            }
            return DocEntryFac;
        }

        internal string ObtenerNombreUnidadMedida(object value)
        {
            throw new NotImplementedException();
        }
    }
}