using SAPbobsCOM;
using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;

namespace WebServiceAllers.Conection
{
    public class Conexion
    {
        /// <summary>
        /// Metodo para abrir la conexion a la empresa.
        /// </summary>
        /// <returns>La compañia de sap</returns>
        public Company ObtenerCompanySAP()
        {
            var oCompanyAllers = ConfigurationManager.AppSettings["oCompanyAllers"];
            Company oCompany;
            if (HttpRuntime.Cache[oCompanyAllers] == null)
            {
                oCompany = new Company();
                oCompany.Server = ConfigurationManager.AppSettings["hostSAP"];
                oCompany.LicenseServer = ConfigurationManager.AppSettings["LicSAP"];
                oCompany.CompanyDB = ConfigurationManager.AppSettings["CompanyDBSAP"];
                oCompany.UserName = ConfigurationManager.AppSettings["usrSAP"];
                oCompany.Password = ConfigurationManager.AppSettings["pwdSAP"];
                oCompany.UseTrusted = false;
                oCompany.DbUserName = ConfigurationManager.AppSettings["DbUserNameSAP"];
                oCompany.DbPassword = ConfigurationManager.AppSettings["DbPasswordSAP"];
                if (ConfigurationManager.AppSettings["BDServerSAP"] == "2005")
                    oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2005;
                else if (ConfigurationManager.AppSettings["BDServerSAP"] == "2008")
                    oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2008;
                else
                    oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2017;
                HttpRuntime.Cache.Add(oCompanyAllers, oCompany, null, DateTime.Now.AddDays(5), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
            else
            {
                oCompany = (Company)HttpRuntime.Cache[oCompanyAllers];
            }
            if (!oCompany.Connected)
            {
                oCompany.Connect();
            }
            return oCompany;
        }

    }
}