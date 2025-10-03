using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace WebServiceAllers.Business
{
    public class Utils
    {
        public static double ObtenerImpuestoBolsa()
        {
            return double.Parse(ConfigurationManager.AppSettings["ImpuestoBolsa"]);
        }
        public static string getToken()
        {
            return ConfigurationManager.AppSettings["Token"];
        }
        public static string getTipoIVAExcluido()
        {
            return ConfigurationManager.AppSettings["IVAExcluido"];
        }
        public static string getTipoIVAExcento()
        {
            return ConfigurationManager.AppSettings["IVAExcento"];
        }
        public static string UsingStream(object myClass)
        {
            string xmlData = "";
            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var xw = XmlWriter.Create(ms)) // Remember to stop using XmlTextWriter  
                    {
                        var serializer = new XmlSerializer(myClass.GetType());
                        serializer.Serialize(xw, myClass);
                        xw.Flush();
                        ms.Seek(0, SeekOrigin.Begin);
                        using (var sr = new StreamReader(ms, Encoding.UTF8))
                        {
                            xmlData = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch { }
            return xmlData;
        }
        public static string getLicTradNum(string value)
        {
            string result = value;
            if (value.Contains(""))
            {
                result = value.Split('-')[0];
            }
            result = result.Replace(".", "").Replace(" ", "").Replace(",", "");
            return result;
        }
        public static string GetApyKey(string key)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionSaluti"].ToString();
            SqlConnection oConexion = new SqlConnection(ConnectionString);
            SqlCommand oSQL = new SqlCommand("SELECT Name FROM [@FACTURAELECTCONF] WHERE Code = @Code", oConexion);
            try
            {
                oConexion.Open();
                oSQL.CommandType = System.Data.CommandType.Text;
                oSQL.Parameters.Add(new SqlParameter("@Code", key));
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
    }
}