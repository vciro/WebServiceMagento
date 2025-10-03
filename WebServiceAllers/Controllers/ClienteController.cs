using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using WebServiceAllers.Models;

namespace WebServiceAllers.Controllers
{
    [AllowAnonymous]
    public class ClienteController : ApiController
    {
        /// <summary>
        /// Método para crear un contacto en SAP desde la BI
        /// </summary>
        /// <param name="oPedM"></param>
        /// <returns></returns>
        [Route("api/Cliente/UpdateContacto")]
        [HttpPost]
        public HttpResponseMessage UpdateContacto([FromBody] ContactoModel oConM)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                string Num = string.Empty;
                int errorCode;
                bool Validador = false;
                string errorDesc = string.Empty;
                Company oCompany = ObtenerCompanySAP();
                SAPbobsCOM.BusinessPartners cliente;
                cliente = (SAPbobsCOM.BusinessPartners)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                cliente.GetByKey(oConM.CardCode);
                for (int i = 0; i <= cliente.ContactEmployees.Count - 1; i++)
                {
                    cliente.ContactEmployees.SetCurrentLine(i);
                    if (cliente.ContactEmployees.Name == oConM.Area)
                    {
                        cliente.ContactEmployees.Active = BoYesNoEnum.tYES;
                        cliente.ContactEmployees.Name = oConM.Area;
                        cliente.ContactEmployees.FirstName = oConM.Nombre;
                        cliente.ContactEmployees.LastName = oConM.Apellido;
                        cliente.ContactEmployees.Position = oConM.Responsable;
                        cliente.ContactEmployees.Profession = oConM.Cargo;
                        cliente.ContactEmployees.MobilePhone = oConM.Telefono;
                        cliente.ContactEmployees.Phone1 = oConM.Telefono;
                        cliente.ContactEmployees.E_Mail = oConM.Correo;
                        cliente.ContactEmployees.Address = oConM.Direccion;
                        Validador = true;
                    }
                }
                if (Validador)
                {
                    cliente.Update();
                    oCompany.GetLastError(out errorCode, out errorDesc);
                    if (errorCode == 0)
                    {
                        Num = oConM.CardCode;
                    }
                    else
                    {
                        Num = string.Format("No se pudo actualizar el contacto. {0} - {1}", errorCode, errorDesc);
                    }
                }
                else
                {
                    Num = "El contacto no existe en el cliente.";
                }
                response.Content = new StringContent(JsonConvert.SerializeObject(Num), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(ex.Message), Encoding.UTF8, "application/json");
            }
            return response;
        }

        /// <summary>
        /// Método para crear un contacto en SAP desde la BI
        /// </summary>
        /// <param name="oPedM"></param>
        /// <returns></returns>
        [Route("api/Cliente/CrearContacto")]
        [HttpPost]
        public HttpResponseMessage CrearContacto([FromBody] ContactoModel oConM)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                string Num = string.Empty;
                int errorCode;
                string errorDesc = string.Empty;
                Company oCompany = ObtenerCompanySAP();
                SAPbobsCOM.BusinessPartners cliente;
                cliente = (SAPbobsCOM.BusinessPartners)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                cliente.GetByKey(oConM.CardCode);
                cliente.ContactEmployees.Add();
                cliente.ContactEmployees.Active = BoYesNoEnum.tYES;
                cliente.ContactEmployees.Name = oConM.Area;
                cliente.ContactEmployees.FirstName = oConM.Nombre;
                cliente.ContactEmployees.LastName = oConM.Apellido;
                cliente.ContactEmployees.Position = oConM.Responsable;
                cliente.ContactEmployees.Profession = oConM.Cargo;
                cliente.ContactEmployees.MobilePhone = oConM.Telefono;
                cliente.ContactEmployees.Phone1 = oConM.Telefono;
                cliente.ContactEmployees.E_Mail = oConM.Correo;
                cliente.ContactEmployees.Address = oConM.Direccion;
                cliente.Update();
                oCompany.GetLastError(out errorCode, out errorDesc);
                if (errorCode == 0)
                {
                    Num = oConM.CardCode;
                }
                else
                {
                    Num = string.Format("No se pudo crear el contacto. {0} - {1}", errorCode, errorDesc);
                }
                response.Content = new StringContent(JsonConvert.SerializeObject(Num), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(ex.Message), Encoding.UTF8, "application/json");
            }
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
                    oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2019;
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

        /// <summary>
        /// Método para obtener los clientes en formato Json
        /// </summary>
        /// <returns>Json con la estructura de lista de los clientes</returns>
        /// GET: api/Cliente
        [Route("api/Cliente/{Todos}")]
        public HttpResponseMessage GetCustomers(bool Todos)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            List<Models.ClienteModel> Lista = new List<Models.ClienteModel>();
            try
            {
                Lista = oCliD.ObtenerClientes(Todos);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(Lista), Encoding.UTF8, "application/json");
            return response;
        }
        /// <summary>
        /// Método para obtener los clientes en formato Json
        /// </summary>
        /// <returns>Json con la estructura de lista de los clientes</returns>
        /// GET: api/Cliente
        [Route("api/Cliente/ObtenerClientesRapido")]
        [HttpGet]
        public HttpResponseMessage ObtenerClientesRapidos()
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.ObtenerClientesRapidos();
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(Lista), Encoding.UTF8, "application/json");
            return response;
        }
        /// <summary>
        /// Método para obtener las direcciones de los clientes en formato Json
        /// </summary>
        /// <returns>Json con la estructura de lista de las direcciones de los clientes</returns>
        [Route("api/Cliente/Direcciones/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage GetDireccionesClientes(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.ObtenerDireccionesClientes(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }
        /// <summary>
        /// Método para obtener las direcciones de los clientes en formato Json
        /// </summary>
        /// <returns>Json con la estructura de lista de las direcciones de los clientes</returns>
        [Route("api/Cliente/DireccionesRapido")]
        [HttpGet]
        public HttpResponseMessage ObtenerDireccionesClientesRapido()
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.ObtenerDireccionesClientesRapido();
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }
        /// <summary>
        /// Método para obtener las direcciones de un cliente en formato Json
        /// </summary>
        /// <returns>Json con la estructura de lista de las direcciones del ciente</returns>
        [Route("api/Cliente/DireccionesPorCliente/{CardCode}")]
        [HttpGet]
        public HttpResponseMessage ObtenerDireccionesPorCliente(string CardCode)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.ObtenerDireccionesPorCliente(CardCode);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }
        /// <summary>
        /// Método para obtener las condiciones de pago en Allers
        /// </summary>
        /// <returns>Json con la estructura de  las condiciones de pago en Allers</returns>
        [Route("api/Cliente/CondicionesPagos/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage CondicionesPagos(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.CondicionesPagos(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los grupos de clientes en Allers
        /// </summary>
        /// <returns>Json con la estructura de los grupos de clientes en Allers</returns>
        [Route("api/Cliente/GruposClientes/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage GruposClientes(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.GruposClientes(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los departamentos en Allers
        /// </summary>
        /// <returns>Json con la estructura de los departamentos en Allers</returns>
        [Route("api/Cliente/AllersDepartamentos/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage AllersDepartamentos(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.AllersDepartamentos(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los tipos de clientes en Allers
        /// </summary>
        /// <returns>Json con la estructura de los tipos de clientes en Allers</returns>
        [Route("api/Cliente/ObtenerTipoCliente/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage ObtenerTipoCliente(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.ObtenerTipoCliente(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los Ciudades en Allers
        /// </summary>
        /// <returns>Json con la estructura de los Ciudades en Allers</returns>
        [Route("api/Cliente/AllersCiudades/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage AllersCiudades(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.AllersCiudades(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener las horas de entrega en Allers
        /// </summary>
        /// <returns>Json con la estructura de las horas de entrega en Allers</returns>
        [Route("api/Cliente/ObtenerHorasEntrega/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage ObtenerHorasEntrega(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.ObtenerHorasEntrega(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener las zonas de reparto en Allers
        /// </summary>
        /// <returns>Json con la estructura de las horas de entrega en Allers</returns>
        [Route("api/Cliente/ObtenerZonasReparto/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage ObtenerZonasReparto(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.ObtenerZonasReparto(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener las opciones de enviar remisión por en Allers
        /// </summary>
        /// <returns>Json con la estructura de las opciones de enviar remisión por en Allers</returns>
        [Route("api/Cliente/ObtenerEnviarRemisionPor/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage ObtenerEnviarRemisionPor(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.ObtenerEnviarRemisionPor(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener las listas de precios en Saluti
        /// </summary>
        /// <returns>Json con la estructura de las opciones de enviar remisión por en Allers</returns>
        [Route("api/Cliente/ObtenerListaPrecioSaluti/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage ObtenerListaPrecioSaluti(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.ObtenerListaPrecioSaluti(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los grupos de clientes en Saluti
        /// </summary>
        /// <returns>Json con la estructura de las opciones de enviar remisión por en Allers</returns>
        [Route("api/Cliente/ObtenerGruposClienteSaluti/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage ObtenerGruposClienteSaluti(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.ObtenerGruposClienteSaluti(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener las zanos en Allers
        /// </summary>
        /// <returns>Json con la estructura de las zanos en Allers</returns>
        [Route("api/Cliente/ObtenerZonasAllers/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage ObtenerZonasAllers(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.ObtenerZonasAllers(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los clientes de Saluti
        /// </summary>
        /// <returns>Json con la estructura de los clientes de Saluti</returns>
        [Route("api/Cliente/Saluti/{Todos}/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage GetClientesSaluti(bool Todos, int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.ObtenerClientesSaluti(Todos, CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los clientes de Allers
        /// </summary>
        /// <returns>Json con la estructura de lista de los clientes Allers</returns>
        [Route("api/Cliente/Allers/{Todos}/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage GetAllersClientes(bool Todos, int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.ObtenerAllersClientes(Todos, CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener la cartera de los clientes de Allers
        /// </summary>
        /// <returns>Json con la estructura de la cartera de los clientes de Allers</returns>
        [Route("api/Cliente/AllersCarteraClientes/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage AllersCarteraClientes(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.AllersCarteraClientes(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los precios especiales de Saluti
        /// </summary>
        /// <returns>Json con la estructura de los precios especiales de Saluti</returns>
        [Route("api/Cliente/SpecialPricesSaluti/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage SpecialPricesSaluti(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.SpecialPricesSaluti(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los precios de una lista de precios en Allers
        /// </summary>
        /// <returns>Json con la estructura de los precios de una lista de precios en Allers</returns>
        [Route("api/Cliente/ObtenerPreciosListaPrecio/{PriceList}/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage ObtenerPreciosListaPrecio(int PriceList, int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.ObtenerPreciosListaPrecio(PriceList, CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los precios de una lista de precios en Allers
        /// </summary>
        /// <returns>Json con la estructura de los precios de una lista de precios en Allers</returns>
        [Route("api/Cliente/ObtenerPreciosListaPrecioRapido/{PriceList}")]
        [HttpGet]
        public HttpResponseMessage ObtenerPreciosListaPrecioRapido(int PriceList)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.ObtenerPreciosListaPrecioRapido(PriceList);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los responsables en Allers
        /// </summary>
        /// <returns>Json con la estructura de los responsables en Allers</returns>
        [Route("api/Cliente/AllersResponsables/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage AllersResponsables(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.AllersResponsables(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los responsables en Allers
        /// </summary>
        /// <returns>Json con la estructura de los responsables en Allers</returns>
        [Route("api/Cliente/Contactos/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage AllersContactosCilentes(int CantidadInicio, int CantidadFija)
        {
            Dao.ClienteDao oCliD = new Dao.ClienteDao();
            DataTable Lista;
            try
            {
                Lista = oCliD.AllersContactosCilentes(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }
    }
}