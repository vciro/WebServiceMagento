using Newtonsoft.Json;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Http;

namespace WebServiceAllers.Controllers
{
    [AllowAnonymous]
    public class ProteccionDatosController : ApiController
    {
        /// <summary>
        /// Metodo para guardar la respuesta del cliente sobre la protección de datos.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        [Route("api/ProteccionDatos/Guardar/{Correo}/{Respuesta}/{Identificacion}/{Tipo}/{Token}")]
        [HttpGet]
        public HttpResponseMessage GuardarConfirmacion(string Correo, string Respuesta, string Identificacion, string Tipo, string Token)
        {
            Dao.ProteccionDatosDao oPro = new Dao.ProteccionDatosDao();
            string Estado = string.Empty;
            var response = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                if (oPro.ValidarRespuestaProteccionDatos(Identificacion, Token))
                {
                    string CardCode = oPro.ObtenerCardCode(Identificacion);
                    Models.ProteccionDatosModel Item = new Models.ProteccionDatosModel();
                    Item.Correo = Correo;
                    Item.Respuesta = Respuesta;
                    Item.Identificacion = Identificacion;
                    Item.Tipo = Tipo;
                    Item.IP = HttpContext.Current.Request.UserHostAddress;
                    Item.Fecha = DateTime.Now;
                    Item.Token = Token;
                    Estado = oPro.GuardarConfirmacion(Item);
                    if (string.IsNullOrEmpty(Estado) && Item.Tipo != "2")
                    {
                        Company oCompany = ObtenerCompanySAP();
                        if (oCompany.Connected)
                        {
                            int errorCode;
                            string errorDesc;
                            BusinessPartners Cliente = (BusinessPartners)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                            Cliente.GetByKey(CardCode);
                            if (!string.IsNullOrEmpty(Cliente.CardCode))
                            {
                                if (Item.Tipo == "0")
                                {
                                    Cliente.UserFields.Fields.Item("U_ProteccionDatos").Value = Item.Respuesta;
                                    Cliente.Update();
                                    oCompany.GetLastError(out errorCode, out errorDesc);
                                    if (errorCode != 0)
                                    {
                                        response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                        Estado = string.Format("ErrorCode -> {0}, ErrorDesc -> {1}", errorCode, errorDesc);
                                    }
                                    else
                                    {
                                        Estado = "Proceso realizado con éxito";
                                    }
                                }
                                if (Item.Tipo == "1")
                                {
                                    bool Validador = false;
                                    for (int i = 0; i <= Cliente.ContactEmployees.Count - 1; i++)
                                    {
                                        Cliente.ContactEmployees.SetCurrentLine(i);
                                        if (Item.Correo == Cliente.ContactEmployees.E_Mail)
                                        {
                                            Cliente.ContactEmployees.UserFields.Fields.Item("U_ProteccionDatos").Value = Item.Respuesta;
                                            Validador = true;
                                        }
                                    }
                                    if (Validador)
                                    {
                                        Cliente.Update();
                                        oCompany.GetLastError(out errorCode, out errorDesc);
                                        if (errorCode != 0)
                                        {
                                            response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                            Estado = string.Format("ErrorCode -> {0}, ErrorDesc -> {1}", errorCode, errorDesc);
                                        }
                                        else
                                        {
                                            Estado = "Proceso realizado con éxito";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Estado = "No se pudo actualizar el CN en SAP, no se pudo conectar a la compañía.";
                        }
                    }
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    Estado = "Su respuesta ya fue procesada o envío de datos incorrectos.";
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(ex.Message), Encoding.UTF8, "application/json");
                return response;
            }
            response.Content = new StringContent(JsonConvert.SerializeObject(Estado), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para realizar envío del correo de la protección de datos a un cliente
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        [Route("api/ProteccionDatos/EnviarCorreo")]
        [HttpPost]
        public HttpResponseMessage EnviarCorreo(Models.ProteccionDatosModel Item)
        {
            Dao.ProteccionDatosDao oPro = new Dao.ProteccionDatosDao();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            string Estado;
            try
            {
                int Longitud = 16;
                Guid miGuid = Guid.NewGuid();
                string Token = Convert.ToBase64String(miGuid.ToByteArray());
                Token = Token.Replace("=", "").Replace("+", "").Substring(0, Longitud);
                //string ContentHtml = File.ReadAllText("D:/Aplicaciones Allers/WebServiceAllers/WebServiceAllers/Images/html_proteccion_de_datos.html", System.Text.Encoding.GetEncoding("iso-8859-1"));
                string Html = @"<html>
                    <head>
                    <meta charset='utf-8'>
                    <title>html_proteccion_de_datos</title>
                    <link href='https://fonts.googleapis.com/css?family=Raleway:100,100i,200,200i,300,300i,400,400i,500,500i,600,600i,700,700i,800,800i,900,900i' rel='stylesheet'>
                    <style>
                        #contenedor{
                            width:600px;
                            height: 850px;
                            margin: 0px auto;
                        }
                        #banner{
                            width:600px;
                            height:299px;
                            border-bottom:1px solid #dede;
                        }
                        #contenido{
                            width:600px;
	                        height:500px;
                        }
                        #texto{
                            width:600px;
	                        height:400px;
                            text-align:justify;
                        }
                        #botones{
                            width:600px;
	                        height:100px;
                        }
                        #pie{
                            width:600px;
	                        height:49px;
	                        background-color:#f8f8f8;
	                        border-top:1px solid #af1a17;
                        }
                        #contenido p{
                            font-family:'raleway',sans-serif;
	                        font-size:16px;
	                        line-height:20px;
	                        font-weight:500;
	                        margin:0px;
	                        color:#666666;
	                        padding:0px 40px;
                        }
                        #banner_izq{
                            width:300px;
	                        height:300px;
	                        float:left;
                        }
                        #banner_der{
                            width:300px;
	                        height:300px;
	                        float:left;	
                        }
                        #banner_izq{
                            font-family:'raleway',sans-serif;
                        }
                        #banner_der{
                            font-family:'raleway',sans-serif;
                        }
                        #banner_top{
                            width:300px;
	                        height:150px;
                        }
                        #banner_down{
                            width:300px;
	                        height:150px;
                        }
                        #banner_top img{
                            float:right;
	                        margin:20px 20px 0px 0px;
                        }
                        #banner_down p{
                            text-align:center;
	                        margin:0px;
	                        float:right;
	                        padding:10px;
	                        text-transform:uppercase;
	                        font-size:16px;
	                        line-height:16px;
	                        color:#ffffff;
	                        font-weight:600;
	                        background-color:#cccccc;
                        }
                        #texto h2{
                            font-family:'raleway',sans-serif;
	                        color:#b11e17;
	                        margin:0px;
	                        padding:20px 40px;
                        }
                        #botones a{
                            text-decoration:none;
	                        float:left;
	                        color:#ffffff;
	                        font-family:'raleway',sans-serif;
	                        text-transform:uppercase;
	                        font-weight:600;
	                    }
                        #si{
                            width:120px;
	                        height:40px;
	                        background-color:#b11e17;
	                        float:left;
	                        margin-left:160px;
	                        }
                        #si p{
                            color:#ffffff;
	                        text-align:center;
	                        font-weight:700;
	                        margin:0px;
	                        padding:0px;
	                        padding-top:10px;
                        }
                        #no{
                            width:120px;
	                        height:40px;
	                        background-color:#b11e17;
	                        float:left;
	                        margin-left:40px;
	                        }
                        #no p{
                            color:#ffffff;
	                        text-align:center;
	                        font-weight:700;
	                        margin:0px;
	                        padding:0px;
	                        padding-top:10px;
                        }
                        #pie a{
                            float:left;
	                        margin-top:8px;
	                        border:none;
	                        border-style:none;
                        }
                    </style>
                    </head>
                    <body style='margin:0px; padding:0px;' >
	                    <div id='contenedor'>
		                    <div id='banner'>
			                    <div id='banner_izq'>
                                    <img src='https://www.allers.com.co/landing/wp-content/uploads/2019/03/fondo.png' style='width:500px;height:300px;'>
                                </div>
			                    <div id='banner_der'>
                                    <div id='banner_top'>
					                    <img src='https://www.allers.com.co/landing/wp-content/uploads/2019/03/logo_allers_group.png' alt='logo allers group' />
				                    </div>
				                    <div id='banner_down'>
                                        <p>¡ Queremos seguir<br/>informándote de nuestros<br/> productos y servicios !</p>
				                    </div>	
			                    </div>
		                    </div>
		                    <div id='contenido'>
                                <div id='texto'>
                                    <h2 style='color:#b11e17;'>¿Aceptas nuestra política para el tratamiento de datos personales?</h2>
				                    <p><b style='color:#b11e17;'> ALLERS S.A.</b> utiliza sus datos que fueron recolectados mediante de nuestros diferentes canales de comercialización y contacto, dando cumplimiento a lo previsto en la<b> Ley 1581 de 2012</b>, sus Decretos reglamentarios y demás normas que la complementen, por lo tanto, este mensaje y sus anexos pueden contener información confidencial, reservada o legalmente protegida y no puede ser utilizada ni divulgada por personas diferentes a su destinatario.Sus datos serán tratados conforme a nuestra<b> Política para el Tratamiento de Datos Personales</b> que se encuentra en nuestra página web<b style='color:#b11e17;'> www.allers.com.co</b>.En caso que desee la supresión de sus datos personales de nuestras bases de datos, puede escribirnos o contactarnos por este medio, en caso de no obtener respuesta negativa, será entendida como aceptación y autorización para el tratamiento de sus datos personales.</p>
			                    </div>
			                    <div id='botones'>
                                    <a href='" + string.Format("http://192.168.1.112:8094/api/ProteccionDatos/Guardar/{0}/1/{1}/{2}/{3}", Item.Correo, Item.Identificacion, Item.Tipo, Token) + @"'>
                                        <div id='si'>
                                            <p>Sí, acepto</p>
  					                    </div>
				                    </a>
				                    <a href='" + string.Format("http://192.168.1.112:8094/api/ProteccionDatos/Guardar/{0}/0/{1}/{2}/{3}", Item.Correo, Item.Identificacion, Item.Tipo, Token) + @"'>
                                        <div id='no'>
                                            <p>No acepto</p>
  					                    </div>
				                    </a>
			                    </div>
		                    </div>
		                    <div id='pie'>
                                <a style='margin-left:252px; margin-top:7px;' href='http://facebook.com/allers.sa'>
                                    <img src='https://www.allers.com.co/landing/wp-content/uploads/2019/03/logo_facebook.png' alt='logo facebook redes sociales allers group'/>
                                </a>
                                <a style='margin-left:25px; margin-top:7px;' href='https://www.linkedin.com/company/allers-group/' >
                                    <img src='https://www.allers.com.co/landing/wp-content/uploads/2019/03/logo_linkedin.png' alt='logo linkedin redes sociales allers group' />
                                </a>
                            </div>
	                    </div>
                    </body>
                </html>";
                MailMessage email = new MailMessage();
                email.To.Add(new MailAddress(Item.Correo));
                email.From = new MailAddress("sistemas@allers.com.co");
                email.Subject = "Protección de datos";
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Html, null, "text/html");
                email.AlternateViews.Add(htmlView);
                email.SubjectEncoding = Encoding.UTF8;
                email.IsBodyHtml = true;
                SmtpClient clienteSmtp = new SmtpClient("192.168.1.69", 587);
                clienteSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                clienteSmtp.UseDefaultCredentials = false;
                clienteSmtp.Credentials = new NetworkCredential("sistemas@allers.com.co", "Sistemas1.,");
                clienteSmtp.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback = delegate (
                Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                {
                    return (true);
                };
                clienteSmtp.Send(email);
                Item.Token = Token;
                oPro.GuardarRegistroToken(Item);
                Estado = "Mensaje enviado con éxito";
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(ex.Message), Encoding.UTF8, "application/json");
                return response;
            }
            response.Content = new StringContent(JsonConvert.SerializeObject(Estado), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Metodo para abrir la conexion a la empresa.
        /// </summary>
        /// <returns>La compañia de sap</returns>
        private Company ObtenerCompanySAP()
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