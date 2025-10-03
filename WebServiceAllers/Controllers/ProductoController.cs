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
using WebServiceAllers.Conection;

namespace WebServiceAllers.Controllers
{
    [AllowAnonymous]
    public class ProductoController : ApiController
    {
        
        /// <summary>
        /// Método para actualizar cantidad de reservas
        /// </summary>
        /// <returns></returns>
        /// GET: api/Producto
        [Route("api/Producto/UpdateItemReserva")]
        [HttpPost]
        public HttpResponseMessage UpdateItemReserva([FromBody] List<ItemUpdateModel> ItemsUpdate)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                int errorCode = 0;
                string errorDesc = "";
                Conexion con = new Conexion();
                Company oCompany = con.ObtenerCompanySAP();
                Items item;
                item = (Items)oCompany.GetBusinessObject(BoObjectTypes.oItems);
                foreach (ItemUpdateModel itemUpdate in ItemsUpdate)
                {
                    item.GetByKey(itemUpdate.ItemCode);
                    item.UserFields.Fields.Item("U_Reserva").Value = itemUpdate.Reserva;
                    item.Update();
                }
                oCompany.GetLastError(out errorCode, out errorDesc);
                if (errorCode != 0)
                {
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = errorDesc }), Encoding.UTF8, "application/json");
                    GC.Collect();
                    return response;
                }
                else
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para obtener los productos en formato Json
        /// </summary>
        /// <returns>Json con la estructura de lista de los productos</returns>
        /// GET: api/Producto
        [Route("api/Producto/{Todos}")]
        [HttpGet]
        public HttpResponseMessage GetProducts(bool Todos)
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            DataTable Lista;
            try
            {
                Lista = oProD.ObtenerProductos(Todos);
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
        /// Método para obtener los artículos solicitados.
        /// </summary>
        /// <returns>Lista de los artículos solicitados</returns>
        [Route("api/Producto/Solicitados")]
        [HttpGet]
        public HttpResponseMessage ObtenerProductosSolicitados()
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            DataTable Lista;
            try
            {
                Lista = oProD.ObtenerProductosSolicitados();
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
        /// Método para obtener los productos Saluti en formato Json
        /// </summary>
        /// <returns>Json con la estructura de lista de los productos Saluti</returns>
        /// GET: api/Producto
        [Route("api/Producto/ProductoSaluti/{Todos}")]
        [HttpGet]
        public HttpResponseMessage GetProductsSaluti(bool Todos)
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            DataTable Lista;
            try
            {
                Lista = oProD.ObtenerProductosSalutiCom(Todos);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(Lista), Encoding.UTF8, "application/json");
            return response;
        }

        [Route("api/Producto/ProductoSalutiPaginado/{Todos}/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage GetProductsSalutiPaginado(int Todos, int CantidadInicio, int CantidadFija)
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            DataTable Lista;
            try
            {
                Lista = oProD.ObtenerProductosSalutiComPaginado(Todos, CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los productos Saluti en formato Json
        /// </summary>
        /// <returns>Json con la estructura de lista de los productos Saluti</returns>
        [Route("api/Producto/SpecialPrices/{Todos}/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage SpecialPrices(bool Todos, int CantidadInicio, int CantidadFija)
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            DataTable Lista;
            try
            {
                Lista = oProD.SpecialPrices(Todos, CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener la lista de las familias de los artículos de Allers
        /// </summary>
        /// <returns>Json con la estructura de la lista de las familias de los artículos de Allers</returns>
        [Route("api/Producto/GetFamiliasArticulosAllers/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage GetFamiliasArticulosAllers(int CantidadInicio, int CantidadFija)
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            DataTable Lista;
            try
            {
                Lista = oProD.GetFamiliasArticulosAllers(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los productos Saluti en formato Json
        /// </summary>
        /// <returns>Json con la estructura de lista de los productos Saluti</returns>
        /// GET: api/Producto
        [Route("api/Producto/ProductoStock/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage GetProductsStock(int CantidadInicio, int CantidadFija)
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            DataTable Lista;
            try
            {
                Lista = oProD.ObtenerProductosStock(CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los productos Saluti en formato Json
        /// </summary>
        /// <returns>Json con la estructura de lista de los productos Saluti</returns>
        /// GET: api/Producto
        [Route("api/Producto/ProductoSalutiPrice")]
        [HttpGet]
        public HttpResponseMessage GetProductsSalutiPrice()
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            DataTable Lista;
            try
            {
                Lista = oProD.ObtenerProductosSalutiPrice();
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(Lista), Encoding.UTF8, "application/json");
            return response;
        }
        [Route("api/Producto/ProductoSalutiPricePaginado/{CantidadInicio}/{CantidadFija}/{Fecha}")]
        [HttpGet]
        public HttpResponseMessage GetProductsSalutiPrice(int CantidadInicio, int CantidadFija, DateTime Fecha)
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            DataTable Lista;
            try
            {
                Lista = oProD.ObtenerProductosSalutiPricePaginado(CantidadInicio, CantidadFija, Fecha);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los productos Saluti en formato Json
        /// </summary>
        /// <returns>Json con la estructura de lista de los productos Saluti</returns>
        /// GET: api/Producto
        [Route("api/Producto/ProductoStockFast/")]
        [HttpGet]
        public HttpResponseMessage GetProductsStockFast()
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            DataTable Lista;
            try
            {
                Lista = oProD.ObtenerProductosStockFast();
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los productos Saluti en formato Json
        /// </summary>
        /// <returns>Json con la estructura de lista de los productos Saluti</returns>
        /// GET: api/Producto
        [Route("api/Producto/ProductoStockFastBodegas/")]
        [HttpGet]
        public HttpResponseMessage ProductoStockFastBodegas()
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            List<ProductoStockModel> Lista;
            try
            {
                Lista = oProD.ObtenerProductosStockFastBodegas();
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Count, data = Lista }), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener los productos de Allers
        /// </summary>
        /// <param name="Todos"></param>
        /// <returns>Json con la estructura de los productos de Allers</returns>
        [Route("api/Producto/AllersProductos/{Todos}/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage GetAllersProductos(bool Todos, int CantidadInicio, int CantidadFija)
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            DataTable Lista;
            try
            {
                Lista = oProD.ObtenerAllersProductos(Todos, CantidadInicio, CantidadFija);
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
        /// Método para obtener el stock de los productos de Allers
        /// </summary>
        /// <param name="Todos"></param>
        /// <returns>Json con la estructura de los productos de Allers</returns>
        [Route("api/Producto/ObtenerStockArticulosAllers/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage ObtenerStockArticulosAllers(int CantidadInicio, int CantidadFija)
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            DataTable Lista;
            try
            {
                Lista = oProD.ObtenerStockArticulosAllers(CantidadInicio, CantidadFija);
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
        /// Método para obtener el stock de los productos de Saluti
        /// </summary>
        /// <param name="Todos"></param>
        /// <returns>Json con la estructura de los productos de Saluti</returns>
        [Route("api/Producto/ObtenerStockArticulosSaluti/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage ObtenerStockArticulosSaluti(int CantidadInicio, int CantidadFija)
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            DataTable Lista;
            try
            {
                Lista = oProD.ObtenerStockArticulosSaluti(CantidadInicio, CantidadFija);
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
        /// Método para obtener los productos en formato Json
        /// </summary>
        /// <returns>Json con la estructura de lista de los productos</returns>
        /// GET: api/Producto
        [Route("api/Producto/AcuerdosGlobales")]
        public HttpResponseMessage GetAcuerdosGlobales()
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            DataTable Lista;
            try
            {
                Lista = oProD.ObtenerAcuerdosGlobales();
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
        /// Método para obtener los productos de Allers creados el ultimo día
        /// </summary>
        /// <returns>Json con la estructura de los productos de Allers creados</returns>
        [Route("api/Producto/ObtenerProductosRapido")]
        [HttpGet]
        public HttpResponseMessage ObtenerProductosRapido()
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            DataTable Lista;
            try
            {
                Lista = oProD.ObtenerProductosRapido();
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Rows.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }

        [Route("api/Producto/ObtenerProductoStock/{Todos}/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage ObtenerProductoStock(int Todos, int CantidadInicio, int CantidadFija)
        {
            Dao.ProductoDao oProD = new Dao.ProductoDao();
            List<ProductoStockModel> Lista;
            try
            {
                Lista = oProD.ObtenerProductoStock(Todos, CantidadInicio, CantidadFija);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { total = Lista.Count, data = Lista, status = HttpStatusCode.BadRequest.GetHashCode() }), Encoding.UTF8, "application/json");
            return response;
        }
    }
}