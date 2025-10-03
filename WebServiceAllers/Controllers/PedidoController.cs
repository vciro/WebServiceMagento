using Newtonsoft.Json;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using WebServiceAllers.Dao;
using WebServiceAllers.Models;
using WebServiceAllers.Positive;
using WebServiceAllers.Positive.Item;

namespace WebServiceAllers.Controllers
{
    [AllowAnonymous]
    public class PedidoController : ApiController
    {
        /// <summary>
        /// Método para validar si el pedido fue despachado
        /// </summary>
        /// <param name="IdPedido"></param>
        /// <returns>Retorna el estado del pedido</returns>
        [Route("api/Pedido/PedidoDespachado/{IdPedido}")]
        [HttpGet]
        public HttpResponseMessage GetValidarPedidoDespachado(string IdPedido)
        {
            Dao.PedidoDao oPed = new Dao.PedidoDao();
            string Estado;
            try
            {
                Estado = oPed.ValidarPedidoDespachado(IdPedido);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(Estado), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Método para obtener las cotizaciones abiertas
        /// </summary>
        /// <param name="Todos"></param>
        /// <param name="CardCode"></param>
        /// <returns>Cotizaciones abiertas</returns>
        [Route("api/Cotizaciones/ObtenerCotizacionesAbiertas/{Todos}/{CantidadInicio}/{CantidadFija}/{CardCode?}")]
        [HttpGet]
        public HttpResponseMessage GetCotizacionesAbiertas(bool Todos, int CantidadInicio, int CantidadFija, string CardCode = "")
        {
            Dao.PedidoDao oPed = new Dao.PedidoDao();
            DataTable Lista;
            try
            {
                Lista = oPed.ObtenerCotizacionesAbiertas(Todos, CantidadInicio, CantidadFija, CardCode);
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
        /// Método para obtener el historico de pedidos por clientes Saluti
        /// </summary>
        /// <returns>Historico de pedidos por clientes Saluti</returns>
        [Route("api/Pedido/HistoricoPedidosPorClientesSaluti/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage GetHistoricoPedidosPorClientesSaluti(int CantidadInicio, int CantidadFija)
        {
            Dao.PedidoDao oPed = new Dao.PedidoDao();
            DataTable Lista;
            try
            {
                Lista = oPed.HistoricoPedidosPorClientesSaluti(CantidadInicio, CantidadFija);
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
        /// Método para obtener el historico de pedidos por clientes
        /// </summary>
        /// <returns>Historico de pedidos por clientes</returns>
        [Route("api/Pedido/HistoricoPedidosPorClientes/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage GetHistoricoPedidosPorClientes(int CantidadInicio, int CantidadFija)
        {
            Dao.PedidoDao oPed = new Dao.PedidoDao();
            DataTable Lista;
            try
            {
                Lista = oPed.HistoricoPedidosPorClientes(CantidadInicio, CantidadFija);
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
        /// Método para crear un pedido en SAP
        /// </summary>
        /// <param name="oPedM"></param>
        /// <returns></returns>
        //[Route("api/Pedido/CreateDocument")]
        //[HttpPost]
        //public HttpResponseMessage CreateDocument([FromBody] PedidoModel oPedM)
        //{
        //    var response = Request.CreateResponse(HttpStatusCode.OK);
        //    try
        //    {
        //        Company oCompany = ObtenerCompanySAP();
        //        int errorCode = 0;
        //        string errorDesc = "";
        //        if (oCompany.Connected)
        //        {
        //            Documents doc;
        //            if (oPedM.TipoDocumento == 17)
        //            {
        //                doc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oOrders);
        //                if (!string.IsNullOrEmpty(oPedM.AgentCode))
        //                {
        //                    doc.AgentCode = oPedM.AgentCode;
        //                }
        //                if (oPedM.Descuento != 0)
        //                {
        //                    doc.DiscountPercent = oPedM.Descuento;
        //                }
        //                if (!string.IsNullOrEmpty(oPedM.ShipToCode))
        //                {
        //                    doc.ShipToCode = oPedM.ShipToCode;
        //                }
        //                if (!string.IsNullOrEmpty(oPedM.PaytoCode))
        //                {
        //                    doc.PayToCode = oPedM.PaytoCode;
        //                }
        //                if (!string.IsNullOrEmpty(oPedM.NombreClienteFacturacion))
        //                {
        //                    doc.CardName = oPedM.NombreClienteFacturacion;
        //                }
        //                if (!string.IsNullOrEmpty(oPedM.Referencia))
        //                {
        //                    doc.NumAtCard = oPedM.Referencia;
        //                }
        //                if (oPedM.CondicionPago != 0)
        //                {
        //                    doc.PaymentGroupCode = oPedM.CondicionPago.Value;
        //                }
        //                if (!string.IsNullOrEmpty(oPedM.CiudadEnvio))
        //                {
        //                    doc.TaxExtension.CityS = oPedM.CiudadEnvio;
        //                }
        //                if (!string.IsNullOrEmpty(oPedM.DepartamentoEnvio))
        //                {
        //                    doc.TaxExtension.CountryS = "CO";
        //                    doc.TaxExtension.StateS = oPedM.DepartamentoEnvio;
        //                }
        //                if (!string.IsNullOrEmpty(oPedM.DireccionEnvio))
        //                {
        //                    doc.TaxExtension.StreetS = oPedM.DireccionEnvio;
        //                }
        //                if (!string.IsNullOrEmpty(oPedM.CiudadFacturacion))
        //                {
        //                    doc.TaxExtension.CityB = oPedM.CiudadFacturacion;
        //                }
        //                if (!string.IsNullOrEmpty(oPedM.DepartamentoFacturacion))
        //                {
        //                    doc.TaxExtension.CountryB = "CO";
        //                    doc.TaxExtension.StateB = oPedM.DepartamentoFacturacion;
        //                }
        //                if (!string.IsNullOrEmpty(oPedM.DireccionFacturacion))
        //                {
        //                    doc.TaxExtension.StreetB = oPedM.DireccionFacturacion;
        //                }
        //                doc.Confirmed = BoYesNoEnum.tNO;
        //            }
        //            else
        //            {
        //                doc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oQuotations);
        //                if (oPedM.Ocasional)
        //                {
        //                    doc.AddressExtension.ShipToStreet = oPedM.DireccionEnvio;
        //                    doc.AddressExtension.ShipToCountry = "CO";
        //                    doc.AddressExtension.ShipToState = oPedM.DepartamentoEnvio;
        //                    doc.AddressExtension.ShipToCity = oPedM.CiudadEnvio;
        //                }
        //            }
        //            doc.CardCode = oPedM.CardCode;
        //            if (!string.IsNullOrEmpty(oPedM.CardName))
        //            {
        //                doc.CardName = oPedM.CardName;
        //            }
        //            doc.DocDueDate = oPedM.FechaDespacho;
        //            doc.DocumentsOwner = oPedM.IdUsuario;
        //            doc.Comments = oPedM.Comentarios;
        //            if (!String.IsNullOrEmpty(oPedM.CamposUsuario))
        //            {
        //                foreach (string CampoUsuario in oPedM.CamposUsuario.Split('|'))
        //                {
        //                    doc.UserFields.Fields.Item(CampoUsuario.Split('~')[0]).Value = CampoUsuario.Split('~')[1];
        //                }
        //            }
        //            foreach (PedidoDetalleModel Item in oPedM.Detalles)
        //            {
        //                doc.Lines.ItemCode = Item.ItemCode;
        //                doc.Lines.Quantity = Item.Quantity;
        //                doc.Lines.UnitPrice = Item.Price;
        //                doc.Lines.DiscountPercent = Item.DiscountPercent;
        //                if (Item.BaseEntry != null)
        //                {
        //                    doc.Lines.BaseType = 23;
        //                    doc.Lines.BaseEntry = Item.BaseEntry.Value;
        //                    doc.Lines.BaseLine = Item.BaseLine.Value;
        //                }
        //                if (oPedM.TipoDocumento == 17)
        //                {
        //                    doc.Lines.WarehouseCode = Item.WhsCode;
        //                }
        //                if (!String.IsNullOrEmpty(Item.CamposUsuario))
        //                {
        //                    foreach (string CampoUsuario in Item.CamposUsuario.Split('|'))
        //                    {
        //                        doc.Lines.UserFields.Fields.Item(CampoUsuario.Split('~')[0]).Value = CampoUsuario.Split('~')[1];
        //                    }
        //                }
        //                doc.Lines.Add();
        //            }
        //            PositiveBiz oPBiz = new PositiveBiz();
        //            var oSItem = oPBiz.ObtenerSedeCardCode(oPedM.CardCode);
        //            if (oSItem != null)
        //            {
        //                if (oCompany.InTransaction)
        //                {
        //                    oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
        //                }
        //                oCompany.StartTransaction();
        //                doc.Add();

        //                oCompany.GetLastError(out errorCode, out errorDesc);
        //                if (errorCode != 0 || errorDesc.Contains("(10)"))
        //                {
        //                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
        //                    response.Content = new StringContent(JsonConvert.SerializeObject(new { err = errorDesc }), Encoding.UTF8, "application/json");
        //                }
        //                else
        //                {
        //                    int DocEntry = int.Parse(oCompany.GetNewObjectKey());
        //                    if (DocEntry == 0)
        //                    {
        //                        GC.Collect();
        //                        response = Request.CreateResponse(HttpStatusCode.InternalServerError);
        //                        response.Content = new StringContent(JsonConvert.SerializeObject(new { error = "Error al crear el documento, enviar de nuevo" }), Encoding.UTF8, "application/json");
        //                        return response;
        //                    }
        //                    else {
        //                        doc.GetByKey(DocEntry);
        //                        if (doc.CardCode != oPedM.CardCode)
        //                        {
        //                            GC.Collect();
        //                            response = Request.CreateResponse(HttpStatusCode.InternalServerError);
        //                            response.Content = new StringContent(JsonConvert.SerializeObject(new { error = "Error al crear el documento, enviar de nuevo" }), Encoding.UTF8, "application/json");
        //                            return response;
        //                        }
        //                        else {
        //                            Company oCompanySaluti = ObtenerCompanySAPSaluti();
        //                            if (oCompanySaluti.Connected)
        //                            {
        //                                Documents oOrdC = (Documents)oCompanySaluti.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
        //                                oOrdC.CardCode = "PN890412352";
        //                                oOrdC.Comments = "Pedido realizado desde iSearch";
        //                                oOrdC.Confirmed = BoYesNoEnum.tYES;
        //                                oOrdC.UserFields.Fields.Item("U_NumEcommerce").Value = doc.DocEntry.ToString();
        //                                oOrdC.UserFields.Fields.Item("U_Sede").Value = oSItem.IdSede.ToString();
        //                                foreach (PedidoDetalleModel Item in oPedM.Detalles)
        //                                {
        //                                    oOrdC.Lines.ItemCode = Item.ItemCode;
        //                                    oOrdC.Lines.Quantity = Item.Quantity;
        //                                    oOrdC.Lines.UnitPrice = Item.Price;
        //                                    oOrdC.Lines.DiscountPercent = Item.DiscountPercent;
        //                                    oOrdC.Lines.WarehouseCode = "07";
        //                                    oOrdC.Lines.Add();
        //                                }
        //                                oOrdC.Add();
        //                                oCompanySaluti.GetLastError(out errorCode, out errorDesc);
        //                                if (errorCode != 0)
        //                                {
        //                                    if (oCompany.InTransaction)
        //                                    {
        //                                        oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
        //                                    }
        //                                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
        //                                    response.Content = new StringContent(JsonConvert.SerializeObject(new { err = errorDesc }), Encoding.UTF8, "application/json");
        //                                }
        //                                else
        //                                {
        //                                    List<PedidoDetalleModel> Detalles = new List<PedidoDetalleModel>();
        //                                    for (int i = 0; i < doc.Lines.Count; i++)
        //                                    {
        //                                        PedidoDetalleModel Det = new PedidoDetalleModel();
        //                                        doc.Lines.SetCurrentLine(i);
        //                                        Det.ItemCode = doc.Lines.ItemCode;
        //                                        Det.LineNum = doc.Lines.LineNum;
        //                                        Det.VisOrder = doc.Lines.VisualOrder;
        //                                        Det.BaseLine = doc.Lines.BaseLine;
        //                                        Detalles.Add(Det);
        //                                    }
        //                                    if (oCompany.InTransaction)
        //                                    {
        //                                        oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
        //                                    }
        //                                    response = Request.CreateResponse(HttpStatusCode.OK);
        //                                    response.Content = new StringContent(JsonConvert.SerializeObject(new { doc.DocNum, doc.DocEntry, Data = Detalles }), Encoding.UTF8, "application/json");
        //                                }
        //                            }
        //                            else
        //                            {
        //                                oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
        //                                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
        //                                oCompanySaluti.GetLastError(out errorCode, out errorDesc);
        //                                response.Content = new StringContent(JsonConvert.SerializeObject(new { err = string.Format("No se puedo conectar a SAP MAIHOEHE - {0}", errorDesc) }), Encoding.UTF8, "application/json");
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                doc.Add();
        //                oCompany.GetLastError(out errorCode, out errorDesc);
        //                if (errorCode != 0)
        //                {
        //                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
        //                    response.Content = new StringContent(JsonConvert.SerializeObject(new { err = errorDesc }), Encoding.UTF8, "application/json");
        //                }
        //                else
        //                {
        //                    int DocEntry = int.Parse(oCompany.GetNewObjectKey());
        //                    if (DocEntry == 0)
        //                    {
        //                        GC.Collect();
        //                        response = Request.CreateResponse(HttpStatusCode.InternalServerError);
        //                        response.Content = new StringContent(JsonConvert.SerializeObject(new { error = "Error al crear el documento, enviar de nuevo" }), Encoding.UTF8, "application/json");
        //                        return response;
        //                    }
        //                    else {
        //                        doc.GetByKey(DocEntry);
        //                        if (doc.CardCode != oPedM.CardCode)
        //                        {
        //                            GC.Collect();
        //                            response = Request.CreateResponse(HttpStatusCode.InternalServerError);
        //                            response.Content = new StringContent(JsonConvert.SerializeObject(new { error = "Error al crear el documento, enviar de nuevo" }), Encoding.UTF8, "application/json");
        //                            return response;
        //                        }
        //                        else {
        //                            doc.GetByKey(int.Parse(oCompany.GetNewObjectKey()));
        //                            List<PedidoDetalleModel> Detalles = new List<PedidoDetalleModel>();
        //                            for (int i = 0; i < doc.Lines.Count; i++)
        //                            {
        //                                PedidoDetalleModel Det = new PedidoDetalleModel();
        //                                doc.Lines.SetCurrentLine(i);
        //                                Det.ItemCode = doc.Lines.ItemCode;
        //                                Det.LineNum = doc.Lines.LineNum;
        //                                Det.VisOrder = doc.Lines.VisualOrder;
        //                                Det.BaseLine = doc.Lines.BaseLine;
        //                                Detalles.Add(Det);
        //                            }
        //                            response = Request.CreateResponse(HttpStatusCode.OK);
        //                            response.Content = new StringContent(JsonConvert.SerializeObject(new { doc.DocNum, doc.DocEntry, Data = Detalles }), Encoding.UTF8, "application/json");
        //                        }
        //                    }
        //                }
        //            }
        //            oCompany.GetLastError(out errorCode, out errorDesc);
        //        }
        //        else
        //        {
        //            response = Request.CreateResponse(HttpStatusCode.InternalServerError);
        //            oCompany.GetLastError(out errorCode, out errorDesc);
        //            response.Content = new StringContent(JsonConvert.SerializeObject(new { err = string.Format("No se puedo conectar a SAP ALLERS - {0}", errorDesc) }), Encoding.UTF8, "application/json");
        //        }
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response = Request.CreateResponse(HttpStatusCode.InternalServerError);
        //        response.Content = new StringContent(JsonConvert.SerializeObject(new { err = ex.Message }), Encoding.UTF8, "application/json");
        //        return response;
        //    }
        //}


        [Route("api/Pedido/CreateDocument")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateDocument([FromBody] PedidoModel oPedM)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                Company oCompany = await OpenSAP();
                int errorCode = 0;
                string errorDesc = "";
                if (oCompany.Connected)
                {
                    Documents doc;
                    if (oPedM.TipoDocumento == 17)
                    {
                        doc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oOrders);
                        if (!string.IsNullOrEmpty(oPedM.AgentCode))
                        {
                            doc.AgentCode = oPedM.AgentCode;
                        }
                        if (oPedM.Descuento != 0)
                        {
                            doc.DiscountPercent = oPedM.Descuento;
                        }
                        if (!string.IsNullOrEmpty(oPedM.ShipToCode))
                        {
                            doc.ShipToCode = oPedM.ShipToCode;
                        }
                        if (!string.IsNullOrEmpty(oPedM.PaytoCode))
                        {
                            doc.PayToCode = oPedM.PaytoCode;
                        }
                        if (!string.IsNullOrEmpty(oPedM.NombreClienteFacturacion))
                        {
                            doc.CardName = oPedM.NombreClienteFacturacion;
                        }
                        if (!string.IsNullOrEmpty(oPedM.Referencia))
                        {
                            doc.NumAtCard = oPedM.Referencia;
                        }
                        if (oPedM.CondicionPago != 0)
                        {
                            doc.PaymentGroupCode = oPedM.CondicionPago.Value;
                        }
                        if (!string.IsNullOrEmpty(oPedM.CiudadEnvio))
                        {
                            doc.TaxExtension.CityS = oPedM.CiudadEnvio;
                        }
                        if (!string.IsNullOrEmpty(oPedM.DepartamentoEnvio))
                        {
                            doc.TaxExtension.CountryS = "CO";
                            doc.TaxExtension.StateS = oPedM.DepartamentoEnvio;
                        }
                        if (!string.IsNullOrEmpty(oPedM.DireccionEnvio))
                        {
                            doc.TaxExtension.StreetS = oPedM.DireccionEnvio;
                        }
                        if (!string.IsNullOrEmpty(oPedM.CiudadFacturacion))
                        {
                            doc.TaxExtension.CityB = oPedM.CiudadFacturacion;
                        }
                        if (!string.IsNullOrEmpty(oPedM.DepartamentoFacturacion))
                        {
                            doc.TaxExtension.CountryB = "CO";
                            doc.TaxExtension.StateB = oPedM.DepartamentoFacturacion;
                        }
                        if (!string.IsNullOrEmpty(oPedM.DireccionFacturacion))
                        {
                            doc.TaxExtension.StreetB = oPedM.DireccionFacturacion;
                        }
                        doc.Confirmed = BoYesNoEnum.tNO;
                    }
                    else
                    {
                        doc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oQuotations);
                        if (oPedM.Ocasional)
                        {
                            doc.AddressExtension.ShipToStreet = oPedM.DireccionEnvio;
                            doc.AddressExtension.ShipToCountry = "CO";
                            doc.AddressExtension.ShipToState = oPedM.DepartamentoEnvio;
                            doc.AddressExtension.ShipToCity = oPedM.CiudadEnvio;
                        }
                    }
                    doc.CardCode = oPedM.CardCode;
                    if (!string.IsNullOrEmpty(oPedM.CardName))
                    {
                        doc.CardName = oPedM.CardName;
                    }
                    doc.DocDueDate = oPedM.FechaDespacho;
                    doc.DocumentsOwner = oPedM.IdUsuario;
                    doc.Comments = oPedM.Comentarios;
                    //doc.UserFields.Fields.Item("U_NumOrderMedishop").Value = oPedM.IdPedido;
                    if (!String.IsNullOrEmpty(oPedM.CamposUsuario))
                    {
                        foreach (string CampoUsuario in oPedM.CamposUsuario.Split('|'))
                        {
                            doc.UserFields.Fields.Item(CampoUsuario.Split('~')[0]).Value = CampoUsuario.Split('~')[1];
                        }
                    }
                    foreach (PedidoDetalleModel Item in oPedM.Detalles)
                    {
                        doc.Lines.ItemCode = Item.ItemCode;
                        doc.Lines.Quantity = Item.Quantity;
                        doc.Lines.UnitPrice = Item.Price;
                        doc.Lines.DiscountPercent = Item.DiscountPercent;
                        if (Item.BaseEntry != null)
                        {
                            doc.Lines.BaseType = 23;
                            doc.Lines.BaseEntry = Item.BaseEntry.Value;
                            doc.Lines.BaseLine = Item.BaseLine.Value;
                        }
                        if (oPedM.TipoDocumento == 17)
                        {
                            doc.Lines.WarehouseCode = Item.WhsCode;
                        }
                        if (!String.IsNullOrEmpty(Item.CamposUsuario))
                        {
                            foreach (string CampoUsuario in Item.CamposUsuario.Split('|'))
                            {
                                doc.Lines.UserFields.Fields.Item(CampoUsuario.Split('~')[0]).Value = CampoUsuario.Split('~')[1];
                            }
                        }
                        doc.Lines.Add();
                    }
                    if (oPedM.CardCode == "CN10810" || oPedM.CardCode == "CN3364" || oPedM.CardCode == "CN9635" || oPedM.CardCode == "CN11490")
                    {
                        PositiveBiz oPBiz = new PositiveBiz();
                        var oSItem = oPBiz.ObtenerSedeCardCode(oPedM.CardCode);
                        if (oSItem != null)
                        {
                            if (oCompany.InTransaction)
                            {
                                oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                            doc.Add();
                            oCompany.GetLastError(out errorCode, out errorDesc);
                            if (errorCode != 0 || errorDesc.Contains("(10)"))
                            {
                                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                response.Content = new StringContent(JsonConvert.SerializeObject(new { err = errorDesc }), Encoding.UTF8, "application/json");
                            }
                            else
                            {
                                int DocEntry = int.Parse(oCompany.GetNewObjectKey());
                                if (DocEntry == 0)
                                {
                                    GC.Collect();
                                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = "Error al crear el documento, enviar de nuevo" }), Encoding.UTF8, "application/json");
                                    return response;
                                }
                                else
                                {
                                    oCompany.StartTransaction();
                                    doc.GetByKey(DocEntry);
                                    //if (doc.CardCode != oPedM.CardCode)
                                    //{
                                    //    GC.Collect();
                                    //    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                    //    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = "Error al crear el documento, enviar de nuevo" }), Encoding.UTF8, "application/json");
                                    //    return response;
                                    //}
                                    //else
                                    //{
                                    Company oCompanySaluti = await ObtenerCompanySAPSalutiAsync();
                                    if (oCompanySaluti.Connected)
                                    {
                                        Documents oOrdC = (Documents)oCompanySaluti.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
                                        oOrdC.CardCode = "PN890412352";
                                        oOrdC.Comments = "Pedido realizado desde iSearch Vieja Version";
                                        oOrdC.Confirmed = BoYesNoEnum.tYES;
                                        oOrdC.UserFields.Fields.Item("U_NumEcommerce").Value = doc.DocEntry.ToString();
                                        oOrdC.UserFields.Fields.Item("U_Sede").Value = oSItem.IdSede.ToString();
                                        foreach (PedidoDetalleModel Item in oPedM.Detalles)
                                        {
                                            oOrdC.Lines.ItemCode = Item.ItemCode;
                                            oOrdC.Lines.Quantity = Item.Quantity;
                                            oOrdC.Lines.UnitPrice = Item.Price;
                                            oOrdC.Lines.DiscountPercent = Item.DiscountPercent;
                                            oOrdC.Lines.WarehouseCode = "07";
                                            oOrdC.Lines.Add();
                                        }
                                        oOrdC.Add();
                                        oCompanySaluti.GetLastError(out errorCode, out errorDesc);
                                        if (errorCode != 0)
                                        {
                                            if (oCompany.InTransaction)
                                            {
                                                oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                            }
                                            response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                            response.Content = new StringContent(JsonConvert.SerializeObject(new { err = errorDesc }), Encoding.UTF8, "application/json");
                                        }
                                        else if (int.Parse(oCompany.GetNewObjectKey()) == 0)
                                        {
                                            GC.Collect();
                                            response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                            response.Content = new StringContent(JsonConvert.SerializeObject(new { error = errorDesc }), Encoding.UTF8, "application/json");
                                            return response;
                                        }
                                        else
                                        {
                                            oOrdC.GetByKey(int.Parse(oCompany.GetNewObjectKey()));
                                            if (oOrdC != null)
                                            {
                                                List<PedidoDetalleModel> Detalles = new List<PedidoDetalleModel>();
                                                for (int i = 0; i < doc.Lines.Count; i++)
                                                {
                                                    PedidoDetalleModel Det = new PedidoDetalleModel();
                                                    doc.Lines.SetCurrentLine(i);
                                                    Det.ItemCode = doc.Lines.ItemCode;
                                                    Det.LineNum = doc.Lines.LineNum;
                                                    Det.VisOrder = doc.Lines.VisualOrder;
                                                    Det.BaseLine = doc.Lines.BaseLine;
                                                    Detalles.Add(Det);
                                                }
                                                if (oCompany.InTransaction)
                                                {
                                                    oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                                                }
                                                response = Request.CreateResponse(HttpStatusCode.OK);
                                                response.Content = new StringContent(JsonConvert.SerializeObject(new { doc.DocNum, doc.DocEntry, Data = Detalles }), Encoding.UTF8, "application/json");
                                            }
                                            else
                                            {
                                                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oOrdC);
                                                GC.Collect();
                                                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                                response.Content = new StringContent(JsonConvert.SerializeObject(new { error = "Error al crear el documento, enviar información de nuevo - SAP B1" }), Encoding.UTF8, "application/json");
                                                return response;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                        response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                        oCompanySaluti.GetLastError(out errorCode, out errorDesc);
                                        response.Content = new StringContent(JsonConvert.SerializeObject(new { err = string.Format("No se puedo conectar a SAP MAIHOEHE - {0}", errorDesc) }), Encoding.UTF8, "application/json");
                                    }
                                }
                            }
                        }
                    }

                    else
                    {
                        doc.Add();
                        oCompany.GetLastError(out errorCode, out errorDesc);
                        if (errorCode != 0)
                        {
                            response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                            response.Content = new StringContent(JsonConvert.SerializeObject(new { err = errorDesc }), Encoding.UTF8, "application/json");
                        }
                        else
                        {
                            int DocEntry = int.Parse(oCompany.GetNewObjectKey());
                            if (DocEntry == 0)
                            {
                                GC.Collect();
                                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                response.Content = new StringContent(JsonConvert.SerializeObject(new { error = "Error al crear el documento, enviar de nuevo" }), Encoding.UTF8, "application/json");
                                return response;
                            }
                            else
                            {
                                doc.GetByKey(DocEntry);
                                if (doc.CardCode != oPedM.CardCode)
                                {
                                    GC.Collect();
                                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = "Error al crear el documento, enviar de nuevo" }), Encoding.UTF8, "application/json");
                                    return response;
                                }
                                else
                                {
                                    doc.GetByKey(int.Parse(oCompany.GetNewObjectKey()));
                                    List<PedidoDetalleModel> Detalles = new List<PedidoDetalleModel>();
                                    for (int i = 0; i < doc.Lines.Count; i++)
                                    {
                                        PedidoDetalleModel Det = new PedidoDetalleModel();
                                        doc.Lines.SetCurrentLine(i);
                                        Det.ItemCode = doc.Lines.ItemCode;
                                        Det.LineNum = doc.Lines.LineNum;
                                        Det.VisOrder = doc.Lines.VisualOrder;
                                        Det.BaseLine = doc.Lines.BaseLine;
                                        Detalles.Add(Det);
                                    }
                                    response = Request.CreateResponse(HttpStatusCode.OK);
                                    response.Content = new StringContent(JsonConvert.SerializeObject(new { doc.DocNum, doc.DocEntry, Data = Detalles }), Encoding.UTF8, "application/json");
                                }
                            }
                        }
                    }
                    oCompany.GetLastError(out errorCode, out errorDesc);
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    oCompany.GetLastError(out errorCode, out errorDesc);
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { err = string.Format("No se puedo conectar a SAP ALLERS - {0}", errorDesc) }), Encoding.UTF8, "application/json");
                }
                return response;
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(new { err = ex.Message }), Encoding.UTF8, "application/json");
                return response;
            }
        }

        [Route("api/Pedido/ComprobarConexionSAP/{SAPB1}")]
        [HttpGet]
        public async Task<HttpResponseMessage> ComprobarSAP(int SAPB1)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);

            try
            {
                Company oCompany = new Company();
                if (SAPB1 == 1)
                {
                    oCompany = await OpenSAP();
                }
                else
                {
                    oCompany = await ObtenerCompanySAPSalutiAsync();
                }

                if (oCompany != null)
                {
                    if (oCompany.Connected)
                    {
                        if (SAPB1 == 1)
                        {
                            response.Content = new StringContent(JsonConvert.SerializeObject("Conexión exitosa a SAP Allers"), Encoding.UTF8, "application/json");
                        }
                        else
                        {
                            response.Content = new StringContent(JsonConvert.SerializeObject("Conexión exitosa a SAP MAIHOEHE"), Encoding.UTF8, "application/json");
                        }

                        //oCompany.Disconnect();
                    }
                    else
                    {
                        int errorCode;
                        string errorDesc;
                        oCompany.GetLastError(out errorCode, out errorDesc);
                        HttpRuntime.Cache.Remove("oCompany");
                        response.Content = new StringContent(JsonConvert.SerializeObject(string.Format("Conexión fallida a sap. ErrorCode:{0} Desc:{1}", errorCode, errorDesc)), Encoding.UTF8, "application/json");
                    }
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = "Excepción no controlada de conexión a SAP" }), Encoding.UTF8, "application/json");

                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(ex.Message), Encoding.UTF8, "application/json");

            }
            GC.Collect();
            return response;
        }

        /// <summary>
        /// Método para crear un pedido en SAP
        /// </summary>
        /// <param name="oPedM"></param>
        /// <returns></returns>
        [Route("api/Pedido/UpdateDocument")]
        [HttpPost]
        public HttpResponseMessage UpdateDocument([FromBody] PedidoModel oPedM)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                Company oCompany = ObtenerCompanySAP();
                int errorCode = 0;
                string errorDesc = "";
                if (oCompany.Connected)
                {
                    Documents doc;
                    if (oPedM.TipoDocumento == 17)
                    {
                        doc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oOrders);
                        doc.GetByKey(oPedM.DocEntry);
                        if (!string.IsNullOrEmpty(oPedM.AgentCode))
                        {
                            doc.AgentCode = oPedM.AgentCode;
                        }
                        if (oPedM.Descuento != 0)
                        {
                            doc.DiscountPercent = oPedM.Descuento;
                        }
                        if (!string.IsNullOrEmpty(oPedM.ShipToCode))
                        {
                            doc.ShipToCode = oPedM.ShipToCode;
                        }
                        if (!string.IsNullOrEmpty(oPedM.PaytoCode))
                        {
                            doc.PayToCode = oPedM.PaytoCode;
                        }
                        if (!string.IsNullOrEmpty(oPedM.NombreClienteFacturacion))
                        {
                            doc.CardName = oPedM.NombreClienteFacturacion;
                        }
                        if (!string.IsNullOrEmpty(oPedM.Referencia))
                        {
                            doc.NumAtCard = oPedM.Referencia;
                        }
                        if (oPedM.CondicionPago != 0)
                        {
                            doc.PaymentGroupCode = oPedM.CondicionPago.Value;
                        }
                        if (!string.IsNullOrEmpty(oPedM.CiudadEnvio))
                        {
                            doc.TaxExtension.CityS = oPedM.CiudadEnvio;
                        }
                        if (!string.IsNullOrEmpty(oPedM.DepartamentoEnvio))
                        {
                            doc.TaxExtension.CountryS = "CO";
                            doc.TaxExtension.StateS = oPedM.DepartamentoEnvio;
                        }
                        if (!string.IsNullOrEmpty(oPedM.DireccionEnvio))
                        {
                            doc.TaxExtension.StreetS = oPedM.DireccionEnvio;
                        }
                        if (!string.IsNullOrEmpty(oPedM.CiudadFacturacion))
                        {
                            doc.TaxExtension.CityB = oPedM.CiudadFacturacion;
                        }
                        if (!string.IsNullOrEmpty(oPedM.DepartamentoFacturacion))
                        {
                            doc.TaxExtension.CountryB = "CO";
                            doc.TaxExtension.StateB = oPedM.DepartamentoFacturacion;
                        }
                        if (!string.IsNullOrEmpty(oPedM.DireccionFacturacion))
                        {
                            doc.TaxExtension.StreetB = oPedM.DireccionFacturacion;
                        }
                    }
                    else
                    {
                        doc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oQuotations);
                        doc.GetByKey(oPedM.DocEntry);
                        if (oPedM.Ocasional)
                        {
                            doc.CardName = oPedM.CardName;
                            doc.AddressExtension.ShipToStreet = oPedM.DireccionEnvio;
                            doc.AddressExtension.ShipToCountry = "CO";
                            doc.AddressExtension.ShipToState = oPedM.DepartamentoEnvio;
                            doc.AddressExtension.ShipToCity = oPedM.CiudadEnvio;
                        }
                    }
                    doc.DocDueDate = oPedM.FechaDespacho;
                    doc.DocumentsOwner = oPedM.IdUsuario;
                    doc.Comments = oPedM.Comentarios;
                    if (!String.IsNullOrEmpty(oPedM.CamposUsuario))
                    {
                        foreach (string CampoUsuario in oPedM.CamposUsuario.Split('|'))
                        {
                            doc.UserFields.Fields.Item(CampoUsuario.Split('~')[0]).Value = CampoUsuario.Split('~')[1];
                        }
                    }
                    foreach (PedidoDetalleModel Item in oPedM.Detalles)
                    {
                        if (Item.VisOrder != null)
                        {
                            doc.Lines.SetCurrentLine(Item.VisOrder.Value);
                            if (Item.Eliminar == 0)
                            {
                                doc.Lines.ItemCode = Item.ItemCode;
                                doc.Lines.Quantity = Item.Quantity;
                                doc.Lines.UnitPrice = Item.Price;
                                doc.Lines.DiscountPercent = Item.DiscountPercent;
                                if (Item.BaseEntry != null)
                                {
                                    doc.Lines.BaseType = 23;
                                    doc.Lines.BaseEntry = Item.BaseEntry.Value;
                                    doc.Lines.BaseLine = Item.BaseLine.Value;
                                }
                                if (oPedM.TipoDocumento == 17)
                                {
                                    doc.Lines.WarehouseCode = Item.WhsCode;
                                }
                                if (!String.IsNullOrEmpty(Item.CamposUsuario))
                                {
                                    foreach (string CampoUsuario in Item.CamposUsuario.Split('|'))
                                    {
                                        doc.Lines.UserFields.Fields.Item(CampoUsuario.Split('~')[0]).Value = CampoUsuario.Split('~')[1];
                                    }
                                }
                                doc.Lines.Add();
                            }
                            else
                            {
                                doc.Lines.Delete();
                            }
                        }
                        else
                        {
                            doc.Lines.ItemCode = Item.ItemCode;
                            doc.Lines.Quantity = Item.Quantity;
                            doc.Lines.UnitPrice = Item.Price;
                            doc.Lines.DiscountPercent = Item.DiscountPercent;
                            if (Item.BaseEntry != null)
                            {
                                doc.Lines.BaseType = 23;
                                doc.Lines.BaseEntry = Item.BaseEntry.Value;
                                doc.Lines.BaseLine = Item.BaseLine.Value;
                            }
                            if (oPedM.TipoDocumento == 17)
                            {
                                doc.Lines.WarehouseCode = Item.WhsCode;
                            }
                            if (!String.IsNullOrEmpty(Item.CamposUsuario))
                            {
                                foreach (string CampoUsuario in Item.CamposUsuario.Split('|'))
                                {
                                    doc.Lines.UserFields.Fields.Item(CampoUsuario.Split('~')[0]).Value = CampoUsuario.Split('~')[1];
                                }
                            }
                            doc.Lines.Add();
                        }
                    }
                    doc.Update();
                    oCompany.GetLastError(out errorCode, out errorDesc);
                    if (errorCode != 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { err = errorDesc }), Encoding.UTF8, "application/json");
                    }
                    else
                    {
                        doc.GetByKey(int.Parse(oCompany.GetNewObjectKey()));
                        List<PedidoDetalleModel> Detalles = new List<PedidoDetalleModel>();
                        for (int i = 0; i < doc.Lines.Count; i++)
                        {
                            PedidoDetalleModel Det = new PedidoDetalleModel();
                            doc.Lines.SetCurrentLine(i);
                            Det.ItemCode = doc.Lines.ItemCode;
                            Det.LineNum = doc.Lines.LineNum;
                            Det.VisOrder = doc.Lines.VisualOrder;
                            Det.BaseLine = doc.Lines.BaseLine;
                            Detalles.Add(Det);
                        }
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { doc.DocNum, doc.DocEntry, Data = Detalles }), Encoding.UTF8, "application/json");
                    }
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    oCompany.GetLastError(out errorCode, out errorDesc);
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { err = string.Format("No se puedo conectar a SAP - {0}", errorDesc) }), Encoding.UTF8, "application/json");
                }
                return response;
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(new { err = ex.Message }), Encoding.UTF8, "application/json");
                return response;
            }
        }

        /// <summary>
        /// Método para crear una orden de compra de Saluti.com.co
        /// </summary>
        /// <param name="oPedM"></param>
        /// <returns></returns>
        [Route("api/Pedido/CreateOrder")]
        [HttpPost]
        public HttpResponseMessage CreateOrder([FromBody] PedidoModel oPedM)
        {
            //var response = Request.CreateResponse(HttpStatusCode.OK);
            //response.Content = new StringContent(JsonConvert.SerializeObject(new { err = oPedM }), Encoding.UTF8, "application/json");
            //return response;

            if (oPedM.IdPedido.Length < 12)
            {
                bool errorFlag = false;
                string numeroPedido = "";
                string NumPedidoSaluti = "";
                string docEntry = "";
                //string DocEntrySaluti = "";
                var response = Request.CreateResponse(HttpStatusCode.OK);
                Company oCompanySaluti = ObtenerCompanySAPSaluti();
                Company oCompany = ObtenerCompanySAP();
                try
                {
                    string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionSBO_Allers"].ToString();
                    int errorCode = 0;
                    string errorDesc = "";
                    string Mensaje = string.Empty;
                    if (oCompanySaluti.Connected)
                    {
                        if (oCompanySaluti.InTransaction == true)
                        {
                            oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }
                        //Se debe realizar la validación si el cliete ya existe en el SAP Saluti
                        BusinessPartners Cliente = (BusinessPartners)oCompanySaluti.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                        Cliente.GetByKey(string.Format("CN{0}", oPedM.CedulaCliente));
                        if (string.IsNullOrEmpty(Cliente.CardCode))
                        {
                            Cliente.CardCode = string.Format("CN{0}", oPedM.CedulaCliente);
                            Cliente.CardName = oPedM.NombreClienteFacturacion;
                            Cliente.CardType = BoCardTypes.cCustomer;
                            Cliente.GroupCode = 100;
                            Cliente.FederalTaxID = oPedM.CedulaCliente;
                            Cliente.Phone1 = oPedM.TelefonoFacturacion;
                            Cliente.Phone2 = oPedM.TelefonoEnvio;
                            Cliente.EmailAddress = oPedM.Email;
                            //Direccion de envio
                            Cliente.Addresses.AddressName = "Direccion Envio Saluti.com";
                            Cliente.Addresses.Street = oPedM.DireccionEnvio;
                            Cliente.Addresses.City = oPedM.CiudadEnvio;
                            Cliente.Addresses.Country = oPedM.PaisEnvio;
                            Cliente.Addresses.State = oPedM.DepartamentoEnvio;
                            Cliente.Addresses.County = oPedM.DepartamentoEnvio;
                            Cliente.Addresses.AddressType = BoAddressType.bo_ShipTo;
                            Cliente.Addresses.Add();
                            //Direccion de pago
                            Cliente.Addresses.AddressName = "Direccion Pago Saluti.com";
                            Cliente.Addresses.Street = oPedM.DireccionFacturacion;
                            Cliente.Addresses.City = oPedM.CiudadFacturacion;
                            Cliente.Addresses.Country = oPedM.PaisFacturacion;
                            Cliente.Addresses.State = oPedM.DepartamentoFacturacion;
                            Cliente.Addresses.County = oPedM.DepartamentoFacturacion;
                            Cliente.Addresses.AddressType = BoAddressType.bo_BillTo;
                            Cliente.Addresses.Add();
                            Cliente.Add();
                            oCompanySaluti.GetLastError(out errorCode, out errorDesc);
                            if (errorCode != 0)
                            {
                                errorFlag = true;
                                Mensaje = string.Format("ErrorCode -> {0}, ErrorDesc -> {1}", errorCode, errorDesc);
                            }
                            else
                            {
                                oPedM.CardCode = oCompanySaluti.GetNewObjectKey();
                                oCompanySaluti.StartTransaction();
                            }
                        }
                        else
                        {
                            oPedM.CardCode = Cliente.CardCode;
                        }
                        if (!errorFlag)
                        {
                            //Crear pedido SAP Saluti
                            Documents doc = (Documents)oCompanySaluti.GetBusinessObject(BoObjectTypes.oOrders);
                            doc.CardCode = oPedM.CardCode;
                            doc.DocDueDate = oPedM.FechaDespacho;
                            doc.CardName = oPedM.NombreClienteFacturacion;
                            doc.DiscountPercent = oPedM.Descuento;
                            doc.Confirmed = BoYesNoEnum.tYES;
                            doc.UserFields.Fields.Item("U_NumEcommerce").Value = oPedM.IdPedido;
                            if (!string.IsNullOrEmpty(oPedM.MedioPago))
                            {
                                if (oPedM.Datafono == 1)
                                {
                                    doc.UserFields.Fields.Item("U_MedioPago").Value = string.Format("{0} - Con Datafono", oPedM.MedioPago);
                                }
                                else
                                {
                                    doc.UserFields.Fields.Item("U_MedioPago").Value = oPedM.MedioPago;
                                }
                            }
                            if (oPedM.IdUsuario != 0)
                            {
                                doc.UserFields.Fields.Item("U_UsuarioCall").Value = oPedM.IdUsuario.ToString();
                            }
                            if (!string.IsNullOrEmpty(oPedM.Email))
                            {
                                doc.UserFields.Fields.Item("U_Email").Value = oPedM.Email;
                            }
                            doc.UserFields.Fields.Item("U_Sede").Value = "1";
                            doc.UserFields.Fields.Item("U_UnidadNegocio").Value = "6";
                            foreach (PedidoDetalleModel Item in oPedM.Detalles)
                            {
                                doc.Lines.ItemCode = Item.ItemCode;
                                doc.Lines.Quantity = Item.Quantity;
                                doc.Lines.UnitPrice = Item.Price;
                                doc.Lines.DiscountPercent = Item.DiscountPercent;
                                doc.Lines.WarehouseCode = "06";
                                doc.Lines.COGSCostingCode = "62";
                                doc.Lines.COGSCostingCode2 = "6215";
                                doc.Lines.COGSCostingCode3 = "621505";
                                doc.Lines.Add();
                            }
                            doc.Comments = oPedM.Comentarios + " Cliente para envío:" + oPedM.NombreClienteEnvio;
                            doc.TaxExtension.CityS = oPedM.CiudadEnvio;
                            doc.TaxExtension.StreetS = oPedM.DireccionEnvio;
                            doc.TaxExtension.CountryS = oPedM.PaisEnvio;
                            doc.TaxExtension.StateS = oPedM.DepartamentoEnvio;
                            doc.TaxExtension.CityB = oPedM.CiudadFacturacion;
                            doc.TaxExtension.StateB = oPedM.DepartamentoFacturacion;
                            doc.TaxExtension.StreetB = oPedM.DireccionFacturacion;
                            doc.TaxExtension.CountryB = oPedM.PaisFacturacion;
                            doc.Add();
                            oCompanySaluti.GetLastError(out errorCode, out errorDesc);
                            if (errorCode != 0)
                            {
                                errorFlag = true;
                                if (errorDesc.Split(' ')[0] == "(100)")
                                {
                                    Mensaje = errorDesc.Split(' ')[1];
                                    response = Request.CreateResponse(HttpStatusCode.OK);
                                    if (oCompanySaluti.InTransaction == true)
                                    {
                                        oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                                    }
                                }
                                else
                                {
                                    Mensaje = string.Format("ErrorCode -> {0}, ErrorDesc -> {1}", errorCode, errorDesc);
                                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                    if (oCompanySaluti.InTransaction == true)
                                    {
                                        oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                                    }
                                }
                            }
                            else
                            {
                                doc.GetByKey(int.Parse(oCompanySaluti.GetNewObjectKey()));
                                if (string.IsNullOrEmpty(oPedM.IdPedido))
                                {
                                    doc.UserFields.Fields.Item("U_NumEcommerce").Value = doc.DocNum;
                                    doc.Update();
                                    oCompanySaluti.GetLastError(out errorCode, out errorDesc);
                                    if (errorCode != 0)
                                    {
                                        errorFlag = true;
                                        Mensaje = string.Format("ErrorCode -> {0}, ErrorDesc -> {1}", errorCode, errorDesc);
                                        response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                        if (oCompanySaluti.InTransaction == true)
                                        {
                                            oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                                        }
                                    }
                                    else
                                    {
                                        NumPedidoSaluti = doc.DocNum.ToString();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        errorFlag = true;
                    }
                    if (!errorFlag)
                    {
                        if (oCompany.Connected)
                        {
                            DateTime HoraCali = DateTime.Parse(string.Format("{0} {1}", DateTime.Now.ToShortDateString(), ConfigurationManager.AppSettings["HoraCali"]));
                            DateTime HoraOtros = DateTime.Parse(string.Format("{0} {1}", DateTime.Now.ToShortDateString(), ConfigurationManager.AppSettings["HoraOtros"]));
                            Documents doc = (Documents)oCompany.GetBusinessObject((BoObjectTypes)17);
                            doc.CardCode = "CN2543";
                            doc.DocDueDate = oPedM.FechaDespacho;
                            doc.CardName = oPedM.NombreClienteFacturacion;
                            if (oPedM.Ecommerce == 1)
                            {
                                doc.GroupNumber = -1;
                            }
                            if (string.IsNullOrEmpty(oPedM.IdPedido))
                            {
                                doc.UserFields.Fields.Item("U_NumOrderMedishop").Value = NumPedidoSaluti;
                            }
                            else
                            {
                                doc.UserFields.Fields.Item("U_NumOrderMedishop").Value = oPedM.IdPedido;
                            }
                            if (!string.IsNullOrEmpty(oPedM.NumeroAutorizacion))
                            {
                                doc.UserFields.Fields.Item("U_PedidoTR").Value = oPedM.NumeroAutorizacion;
                            }
                            if (!string.IsNullOrEmpty(oPedM.InfLogistica))
                            {
                                doc.UserFields.Fields.Item("U_InfoLogistica").Value = oPedM.InfLogistica;
                            }
                            doc.UserFields.Fields.Item("U_NIT").Value = oPedM.CedulaCliente;
                            doc.UserFields.Fields.Item("U_Ecommerce").Value = oPedM.Ecommerce.ToString();
                            if (oPedM.IdUsuario != 0)
                            {
                                doc.UserFields.Fields.Item("U_UsuarioCall").Value = oPedM.IdUsuario.ToString();
                            }
                            if (oPedM.RecogerTienda)
                            {
                                doc.UserFields.Fields.Item("U_H_ENTREGA").Value = "03";
                            }
                            else
                            {
                                if (oPedM.CiudadEnvio == "Cali")
                                {
                                    if (DateTime.Now <= HoraCali)
                                    {
                                        doc.UserFields.Fields.Item("U_H_ENTREGA").Value = "02";
                                    }
                                    else
                                    {
                                        doc.UserFields.Fields.Item("U_H_ENTREGA").Value = "01";
                                    }
                                }
                                else
                                {
                                    if (DateTime.Now <= HoraOtros)
                                    {
                                        doc.UserFields.Fields.Item("U_H_ENTREGA").Value = "02";
                                    }
                                    else
                                    {
                                        doc.UserFields.Fields.Item("U_H_ENTREGA").Value = "01";
                                    }
                                }
                            }
                            doc.UserFields.Fields.Item("U_RAZONSOCIAL").Value = oPedM.NombreClienteFacturacion;
                            doc.UserFields.Fields.Item("U_AutorizaPedido").Value = "S";
                            doc.UserFields.Fields.Item("U_AutorizaTotal").Value = "S";
                            doc.UserFields.Fields.Item("U_Ciudad").Value = oPedM.CiudadEnvio;
                            doc.UserFields.Fields.Item("U_DireccionCliente").Value = oPedM.DireccionEnvio;
                            doc.UserFields.Fields.Item("U_TelClienteMedishop").Value = oPedM.TelefonoEnvio;
                            doc.UserFields.Fields.Item("U_Observacion").Value = oPedM.Observaciones;
                            if (!string.IsNullOrEmpty(oPedM.ComentariosDocumento))
                            {
                                doc.UserFields.Fields.Item("U_Comentarios").Value = oPedM.ComentariosDocumento;
                            }
                            doc.UserFields.Fields.Item("U_REMISIONPO").Value = "4";
                            if (oPedM.Autorizado == 1)
                            {
                                doc.Confirmed = BoYesNoEnum.tYES;
                                doc.UserFields.Fields.Item("U_Workflow").Value = "1";
                            }
                            else
                            {
                                doc.UserFields.Fields.Item("U_Workflow").Value = "0";
                            }
                            if (!string.IsNullOrEmpty(oPedM.Email))
                            {
                                doc.UserFields.Fields.Item("U_Mail").Value = oPedM.Email;
                            }
                            if (oPedM.FechaVencimiento)
                            {
                                doc.UserFields.Fields.Item("U_AutFechaVen").Value = "Y";
                            }
                            if (!string.IsNullOrEmpty(oPedM.MedioPago))
                            {
                                if (oPedM.Datafono == 1)
                                {
                                    doc.UserFields.Fields.Item("U_MedioPago").Value = string.Format("{0} - Con Datafono", oPedM.MedioPago);
                                }
                                else
                                {
                                    doc.UserFields.Fields.Item("U_MedioPago").Value = oPedM.MedioPago;
                                }
                            }
                            foreach (PedidoDetalleModel Item in oPedM.Detalles)
                            {
                                Items item = (Items)oCompanySaluti.GetBusinessObject(BoObjectTypes.oItems);
                                item.GetByKey(Item.ItemCode);
                                if (item.InventoryItem == BoYesNoEnum.tYES)
                                {
                                    doc.Lines.ItemCode = Item.ItemCode;
                                    doc.Lines.Quantity = Convert.ToInt32(Item.Quantity);
                                    doc.Lines.WarehouseCode = "08";
                                    if (oPedM.FechaVencimiento)
                                    {
                                        doc.Lines.UserFields.Fields.Item("U_AutorizaFecVen").Value = "S";
                                    }
                                    doc.Lines.Add();
                                }
                            }
                            doc.Comments = oPedM.Comentarios + " Cliente para envío:" + oPedM.NombreClienteEnvio;
                            doc.TaxExtension.CityS = oPedM.CiudadEnvio;
                            doc.TaxExtension.StreetS = oPedM.DireccionEnvio;
                            doc.TaxExtension.CountryS = oPedM.PaisEnvio;
                            doc.TaxExtension.StateS = oPedM.DepartamentoEnvio;
                            doc.TaxExtension.CountyS = oPedM.DepartamentoEnvio;
                            doc.Add();
                            oCompany.GetLastError(out errorCode, out errorDesc);
                            if (errorCode != 0)
                            {
                                errorFlag = true;
                                NumPedidoSaluti = string.Format("ErrorCode -> {0}, ErrorDesc -> {1}", errorCode, errorDesc);
                                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                if (oCompanySaluti.InTransaction == true)
                                {
                                    oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                                }
                            }
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                            if (oCompanySaluti.Connected)
                            {
                                oCompany.GetLastError(out errorCode, out errorDesc);
                                NumPedidoSaluti = errorCode + "," + errorDesc;
                                if (oCompanySaluti.InTransaction == true)
                                {
                                    oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                                }
                            }
                            else
                            {
                                errorFlag = true;
                                NumPedidoSaluti = "No se pudo conectar a las compañias";
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Mensaje))
                        {
                            oCompanySaluti.GetLastError(out errorCode, out errorDesc);
                            Mensaje = string.Format("ErrorCode -> {0}, ErrorDesc -> {1}", errorCode, errorDesc);
                        }
                        NumPedidoSaluti = Mensaje;
                        if (oCompanySaluti.InTransaction == true)
                        {
                            oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }
                    }
                    if (oCompanySaluti.InTransaction == true)
                    {
                        oCompanySaluti.EndTransaction(BoWfTransOpt.wf_Commit);
                    }
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { err = NumPedidoSaluti }), Encoding.UTF8, "application/json");
                    return response;
                }
                catch (Exception ex)
                {
                    if (oCompanySaluti.InTransaction == true)
                    {
                        oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                    }
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { err = ex.Message }), Encoding.UTF8, "application/json");
                    return response;
                }
            }
            else
            {
                var serverErrorResponse = Request.CreateResponse(HttpStatusCode.InternalServerError);
                serverErrorResponse.Content = new StringContent(JsonConvert.SerializeObject(new { err = "El campo IdPedido debe ser menor o igual a 11 caracteres." }), Encoding.UTF8, "application/json");
                return serverErrorResponse;
            }
        }

        /// <summary>
        /// Método para crear una orden de compra de Saluti.com.co
        /// </summary>
        /// <param name="oPedM"></param>
        /// <returns></returns>
        [Route("api/Pedido/CreateOrderByWareHouse")]
        [HttpPost]
        public HttpResponseMessage CreateOrderByWareHouse([FromBody] PedidoModel oPedM)
        {
            //var response = Request.CreateResponse(HttpStatusCode.OK);
            //response.Content = new StringContent(JsonConvert.SerializeObject(new { err = oPedM }), Encoding.UTF8, "application/json");
            //return response;

            if (oPedM.IdPedido.Length < 12)
            {
                PositiveBiz oPBiz = new PositiveBiz();
                if (!oPBiz.ExistePedidoCom(oPedM.IdPedido))
                {
                    bool errorFlag = false;
                    string NumPedidoSaluti = "";
                    string errorMenssage = "";
                    var PedidosPositive = new List<string>();
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    Company oCompanySaluti = ObtenerCompanySAPSaluti();
                    Company oCompany = ObtenerCompanySAP();
                    int idDocumentoCom = 0;
                    try
                    {
                        string ConnectionString = ConfigurationManager.ConnectionStrings["CadenaConexionSBO_Allers"].ToString();
                        int errorCode = 0;
                        string errorDesc = "";
                        //string Mensaje = string.Empty;
                        bool crearPedidoAllers = false;
                        if (oCompanySaluti.Connected)
                        {
                            if (oCompanySaluti.InTransaction == true)
                            {
                                oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                            //Se debe realizar la validación si el cliete ya existe en el SAP Saluti
                            BusinessPartners Cliente = (BusinessPartners)oCompanySaluti.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                            Cliente.GetByKey(string.Format("CN{0}", oPedM.CedulaCliente));
                            if (string.IsNullOrEmpty(Cliente.CardCode))
                            {
                                Cliente.CardCode = string.Format("CN{0}", oPedM.CedulaCliente);
                                Cliente.CardName = oPedM.NombreClienteFacturacion;
                                Cliente.CardType = BoCardTypes.cCustomer;
                                Cliente.GroupCode = 100;
                                Cliente.FederalTaxID = oPedM.CedulaCliente;
                                Cliente.Phone1 = oPedM.TelefonoFacturacion;
                                Cliente.Phone2 = oPedM.TelefonoEnvio;
                                Cliente.EmailAddress = oPedM.Email;
                                //Direccion de envio
                                Cliente.Addresses.AddressName = "Direccion Envio Saluti.com";
                                Cliente.Addresses.Street = oPedM.DireccionEnvio;
                                Cliente.Addresses.City = oPedM.CiudadEnvio;
                                Cliente.Addresses.Country = oPedM.PaisEnvio;
                                Cliente.Addresses.State = oPedM.DepartamentoEnvio;
                                Cliente.Addresses.County = oPedM.DepartamentoEnvio;
                                Cliente.Addresses.AddressType = BoAddressType.bo_ShipTo;
                                Cliente.Addresses.Add();
                                //Direccion de pago
                                Cliente.Addresses.AddressName = "Direccion Pago Saluti.com";
                                Cliente.Addresses.Street = oPedM.DireccionFacturacion;
                                Cliente.Addresses.City = oPedM.CiudadFacturacion;
                                Cliente.Addresses.Country = oPedM.PaisFacturacion;
                                Cliente.Addresses.State = oPedM.DepartamentoFacturacion;
                                Cliente.Addresses.County = oPedM.DepartamentoFacturacion;
                                Cliente.Addresses.AddressType = BoAddressType.bo_BillTo;
                                Cliente.Addresses.Add();
                                Cliente.Add();
                                oCompanySaluti.GetLastError(out errorCode, out errorDesc);
                                if (errorCode != 0)
                                {
                                    errorFlag = true;
                                    errorMenssage = string.Format("ErrorCode -> {0}, ErrorDesc -> {1}", errorCode, errorDesc);
                                }
                                else
                                {
                                    oPedM.CardCode = oCompanySaluti.GetNewObjectKey();
                                    oCompanySaluti.StartTransaction();
                                }
                            }
                            else
                            {
                                oPedM.CardCode = Cliente.CardCode;
                            }
                            if (!errorFlag)
                            {
                                string msgPositiveCom = CrearPedidoPositiveCom(oPedM, oPedM.Detalles);
                                int.TryParse(msgPositiveCom, out idDocumentoCom);
                                if (idDocumentoCom > 0)
                                {
                                    oPBiz.Guardar(new tblDocumentoJSONItem()
                                    {
                                        idDocumento = idDocumentoCom,
                                        data = JsonConvert.SerializeObject(oPedM),
                                        idTipoDocumento = WebServiceAllers.Positive.tblTipoDocumentoItem.TipoDocumentoEnum.PedidoCom.GetHashCode()
                                    });
                                    List<string> bodegas = new List<string>();
                                    foreach (var d in oPedM.Detalles)
                                    {
                                        bool existe = false;
                                        foreach (var b in bodegas)
                                        {
                                            if (d.WhsCode == b)
                                            {
                                                existe = true;
                                            }
                                        }
                                        if (!existe)
                                        {
                                            bodegas.Add(d.WhsCode);
                                        }
                                    }

                                    var documentosPositive = oPBiz.ObtenerPedidoPorReferencia(oPedM.IdPedido);
                                    string localError = "";
                                    foreach (var b in bodegas)
                                    {
                                        var Detalles = new List<PedidoDetalleModel>();
                                        foreach (var d in oPedM.Detalles)
                                        {
                                            if (b == d.WhsCode)
                                            {
                                                Detalles.Add(d);
                                            }
                                        }
                                        if (b == "06")
                                        {
                                            crearPedidoAllers = true;
                                            var pedido = oPBiz.ObtenerPedidoComPedido(idDocumentoCom, b, false);
                                            if (pedido == null || pedido.idPedidoCom == 0 || !string.IsNullOrEmpty(pedido.Error))
                                            {
                                                string Message = CrearPedidoSaluti(oPedM, Detalles, oCompanySaluti, b);
                                                int.TryParse(Message, out int DocEntry);
                                                if (DocEntry == 0)
                                                {
                                                    errorFlag = false;
                                                    errorMenssage = $"{errorMenssage}{Environment.NewLine}{Message}";
                                                    localError = Message;
                                                }
                                                else
                                                {
                                                    localError = "";
                                                }
                                                NumPedidoSaluti = DocEntry.ToString();
                                                oPBiz.Guardar(new tblPedidoComPedidosItem()
                                                {
                                                    idPedidoCom = idDocumentoCom,
                                                    idPedido = long.Parse(NumPedidoSaluti),
                                                    Positive = false,
                                                    WhsCode = b,
                                                    Error = localError,
                                                    PedidoInterno = false
                                                });
                                            }
                                            else
                                            {
                                                NumPedidoSaluti = pedido.idPedido.ToString();
                                            }
                                        }
                                        else
                                        {
                                            var pedido = oPBiz.ObtenerPedidoComPedido(idDocumentoCom, b, false);
                                            if (pedido == null || pedido.idPedidoCom == 0 || !string.IsNullOrEmpty(pedido.Error))
                                            {
                                                string Message = CrearPedidoPositive(oPedM, Detalles);
                                                int.TryParse(Message, out int idDocumento);
                                                if (idDocumento == 0)
                                                {
                                                    errorFlag = true;
                                                    errorMenssage = $"{errorMenssage}{Environment.NewLine}{Message}";
                                                    localError = Message;
                                                }
                                                else
                                                {
                                                    localError = "";
                                                    var p = oPBiz.ObtenerDocumento(idDocumento, WebServiceAllers.Positive.tblTipoDocumentoItem.TipoDocumentoEnum.pedido.GetHashCode());
                                                    PedidosPositive.Add(p.NumeroDocumento);
                                                }
                                                oPBiz.Guardar(new tblPedidoComPedidosItem()
                                                {
                                                    idPedidoCom = idDocumentoCom,
                                                    idPedido = idDocumento,
                                                    Positive = true,
                                                    WhsCode = b,
                                                    Error = localError,
                                                    PedidoInterno = false
                                                });
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    errorFlag = true;
                                    errorMenssage = $"{errorMenssage}{Environment.NewLine}{msgPositiveCom}";
                                }
                            }
                        }
                        else
                        {
                            errorFlag = true;
                            errorMenssage = "No existe una conexión a SAP Saluti";
                        }
                        if (!errorFlag)
                        {
                            if (oCompany.Connected)
                            {
                                if (!string.IsNullOrEmpty(oPedM.IdPedido))
                                {
                                    NumPedidoSaluti = oPedM.IdPedido;
                                }
                                var pedido = oPBiz.ObtenerPedidoComPedido(idDocumentoCom, "06", true);

                                if (crearPedidoAllers && (pedido == null || pedido.idPedidoCom == 0 || !string.IsNullOrEmpty(pedido.Error)))
                                {
                                    DateTime HoraCali = DateTime.Parse(string.Format("{0} {1}", DateTime.Now.ToShortDateString(), ConfigurationManager.AppSettings["HoraCali"]));
                                    DateTime HoraOtros = DateTime.Parse(string.Format("{0} {1}", DateTime.Now.ToShortDateString(), ConfigurationManager.AppSettings["HoraOtros"]));
                                    Documents doc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oOrders);
                                    doc.CardCode = "CN2543";
                                    doc.DocDueDate = oPedM.FechaDespacho;
                                    doc.CardName = oPedM.NombreClienteFacturacion;
                                    if (oPedM.Ecommerce == 1)
                                    {
                                        doc.GroupNumber = -1;
                                    }
                                    if (string.IsNullOrEmpty(oPedM.IdPedido))
                                    {
                                        doc.UserFields.Fields.Item("U_NumOrderMedishop").Value = NumPedidoSaluti;
                                    }
                                    else
                                    {
                                        doc.UserFields.Fields.Item("U_NumOrderMedishop").Value = oPedM.IdPedido;
                                    }
                                    if (!string.IsNullOrEmpty(oPedM.NumeroAutorizacion))
                                    {
                                        doc.UserFields.Fields.Item("U_PedidoTR").Value = oPedM.NumeroAutorizacion;
                                    }
                                    if (!string.IsNullOrEmpty(oPedM.InfLogistica))
                                    {
                                        doc.UserFields.Fields.Item("U_InfoLogistica").Value = oPedM.InfLogistica;
                                    }
                                    doc.UserFields.Fields.Item("U_NIT").Value = oPedM.CedulaCliente;
                                    doc.UserFields.Fields.Item("U_Ecommerce").Value = oPedM.Ecommerce.ToString();
                                    if (oPedM.IdUsuario != 0)
                                    {
                                        doc.UserFields.Fields.Item("U_UsuarioCall").Value = oPedM.IdUsuario.ToString();
                                    }
                                    if (oPedM.RecogerTienda)
                                    {
                                        doc.UserFields.Fields.Item("U_H_ENTREGA").Value = "03";
                                    }
                                    else
                                    {
                                        if (oPedM.CiudadEnvio == "Cali")
                                        {
                                            if (DateTime.Now <= HoraCali)
                                            {
                                                doc.UserFields.Fields.Item("U_H_ENTREGA").Value = "02";
                                            }
                                            else
                                            {
                                                doc.UserFields.Fields.Item("U_H_ENTREGA").Value = "01";
                                            }
                                        }
                                        else
                                        {
                                            if (DateTime.Now <= HoraOtros)
                                            {
                                                doc.UserFields.Fields.Item("U_H_ENTREGA").Value = "02";
                                            }
                                            else
                                            {
                                                doc.UserFields.Fields.Item("U_H_ENTREGA").Value = "01";
                                            }
                                        }
                                    }
                                    doc.UserFields.Fields.Item("U_RAZONSOCIAL").Value = oPedM.NombreClienteFacturacion;
                                    doc.UserFields.Fields.Item("U_AutorizaPedido").Value = "S";
                                    doc.UserFields.Fields.Item("U_AutorizaTotal").Value = "S";
                                    doc.UserFields.Fields.Item("U_Ciudad").Value = oPedM.CiudadEnvio;
                                    doc.UserFields.Fields.Item("U_DireccionCliente").Value = oPedM.DireccionEnvio;
                                    doc.UserFields.Fields.Item("U_TelClienteMedishop").Value = oPedM.TelefonoEnvio;
                                    doc.UserFields.Fields.Item("U_Observacion").Value = oPedM.Observaciones;
                                    if (!string.IsNullOrEmpty(oPedM.ComentariosDocumento))
                                    {
                                        doc.UserFields.Fields.Item("U_Comentarios").Value = oPedM.ComentariosDocumento;
                                    }
                                    doc.UserFields.Fields.Item("U_REMISIONPO").Value = "4";
                                    if (oPedM.Autorizado == 1)
                                    {
                                        doc.Confirmed = BoYesNoEnum.tYES;
                                        doc.UserFields.Fields.Item("U_Workflow").Value = "1";
                                    }
                                    else
                                    {
                                        doc.UserFields.Fields.Item("U_Workflow").Value = "0";
                                    }
                                    if (!string.IsNullOrEmpty(oPedM.Email))
                                    {
                                        doc.UserFields.Fields.Item("U_Mail").Value = oPedM.Email;
                                    }
                                    if (oPedM.FechaVencimiento)
                                    {
                                        doc.UserFields.Fields.Item("U_AutFechaVen").Value = "Y";
                                    }
                                    if (!string.IsNullOrEmpty(oPedM.MedioPago))
                                    {
                                        if (oPedM.Datafono == 1)
                                        {
                                            doc.UserFields.Fields.Item("U_MedioPago").Value = string.Format("{0} - Con Datafono", oPedM.MedioPago);
                                        }
                                        else
                                        {
                                            doc.UserFields.Fields.Item("U_MedioPago").Value = oPedM.MedioPago;
                                        }
                                    }
                                    crearPedidoAllers = false;
                                    foreach (PedidoDetalleModel Item in oPedM.Detalles)
                                    {
                                        if (Item.WhsCode == "06")
                                        {
                                            Items item = (Items)oCompanySaluti.GetBusinessObject(BoObjectTypes.oItems);
                                            item.GetByKey(Item.ItemCode);
                                            if (item.InventoryItem == BoYesNoEnum.tYES)
                                            {
                                                doc.Lines.ItemCode = Item.ItemCode;
                                                doc.Lines.Quantity = Convert.ToInt32(Item.Quantity);
                                                doc.Lines.WarehouseCode = "08";
                                                if (oPedM.FechaVencimiento)
                                                {
                                                    doc.Lines.UserFields.Fields.Item("U_AutorizaFecVen").Value = "S";
                                                }
                                                crearPedidoAllers = true;
                                                doc.Lines.Add();
                                            }
                                        }
                                    }
                                    doc.Comments = oPedM.Comentarios + " Cliente para envío:" + oPedM.NombreClienteEnvio;
                                    doc.TaxExtension.CityS = oPedM.CiudadEnvio;
                                    doc.TaxExtension.StreetS = oPedM.DireccionEnvio;
                                    doc.TaxExtension.CountryS = oPedM.PaisEnvio;
                                    doc.TaxExtension.StateS = oPedM.DepartamentoEnvio;
                                    doc.TaxExtension.CountyS = oPedM.DepartamentoEnvio;
                                    if (crearPedidoAllers)
                                    {
                                        doc.Add();
                                        oCompany.GetLastError(out errorCode, out errorDesc);
                                        if (errorCode != 0)
                                        {
                                            errorFlag = true;
                                            errorMenssage = string.Format("ErrorCode -> {0}, ErrorDesc -> {1}", errorCode, errorDesc);
                                            response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                            if (oCompanySaluti.InTransaction == true)
                                            {
                                                oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                                            }
                                            oPBiz.Guardar(new tblPedidoComPedidosItem()
                                            {
                                                idPedidoCom = idDocumentoCom,
                                                idPedido = 0,
                                                Positive = false,
                                                WhsCode = "06",
                                                Error = errorMenssage,
                                                PedidoInterno = true
                                            });

                                        }
                                        else
                                        {
                                            var sDocEntry = oCompany.GetNewObjectKey();
                                            oPBiz.Guardar(new tblPedidoComPedidosItem()
                                            {
                                                idPedidoCom = idDocumentoCom,
                                                idPedido = long.Parse(sDocEntry),
                                                Positive = false,
                                                WhsCode = "06",
                                                Error = errorMenssage,
                                                PedidoInterno = true
                                            });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                                if (oCompanySaluti.Connected)
                                {
                                    oCompany.GetLastError(out errorCode, out errorDesc);
                                    errorMenssage = errorCode + "," + errorDesc;
                                    if (oCompanySaluti.InTransaction == true)
                                    {
                                        oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                                    }
                                }
                                else
                                {
                                    errorFlag = true;
                                    errorMenssage = "No se pudo conectar a las compañias";
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(errorMenssage))
                            {
                                oCompanySaluti.GetLastError(out errorCode, out errorDesc);
                                errorMenssage = string.Format("ErrorCode -> {0}, ErrorDesc -> {1}", errorCode, errorDesc);
                            }
                            if (oCompanySaluti.InTransaction == true)
                            {
                                oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                        }
                        if (oCompanySaluti.InTransaction == true)
                        {
                            oCompanySaluti.EndTransaction(BoWfTransOpt.wf_Commit);
                        }
                        var pedidosCreados = oPBiz.ObtenerPedidoComPedidos(idDocumentoCom);
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { response = new RespuestaCom(string.IsNullOrEmpty(errorMenssage), NumPedidoSaluti, pedidosCreados) }), Encoding.UTF8, "application/json");
                        return response;
                    }
                    catch (Exception ex)
                    {
                        if (oCompanySaluti.InTransaction == true)
                        {
                            oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }
                        response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { response = new RespuestaCom(false, ex.Message, new List<tblPedidoComPedidosItem>()) }), Encoding.UTF8, "application/json");
                        return response;
                    }
                }
                else
                {
                    var serverErrorResponse = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    serverErrorResponse.Content = new StringContent(JsonConvert.SerializeObject(new { err = $"El Pedido {oPedM.IdPedido} ya esta creado" }), Encoding.UTF8, "application/json");
                    return serverErrorResponse;
                }
            }
            else
            {
                var serverErrorResponse = Request.CreateResponse(HttpStatusCode.InternalServerError);
                serverErrorResponse.Content = new StringContent(JsonConvert.SerializeObject(new { err = "El campo IdPedido debe ser menor o igual a 11 caracteres." }), Encoding.UTF8, "application/json");
                return serverErrorResponse;
            }
        }


        private string CrearPedidoPositive(PedidoModel oPedM, List<PedidoDetalleModel> Detalles)
        {
            string Message = "";
            try
            {
                PositiveBiz oPBiz = new PositiveBiz();
                const long IdEmpresa = 1;
                const long IdUsuarioSistemas = 2;
                short IdCiudad = 1003;
                if (!string.IsNullOrEmpty(oPedM.CodigoCiudad))
                {
                    tblCiudadItem oCItem = oPBiz.ObtenerCiudadPorCodigo(oPedM.CodigoCiudad);
                    if (oCItem != null && oCItem.idCiudad > 0)
                    {
                        IdCiudad = oCItem.idCiudad;
                    }
                }
                tblDocumentoItem oDocItem = new tblDocumentoItem();
                oPBiz.ActualizarTerceroCardCode(IdEmpresa);
                tblTerceroItem oTItem = oPBiz.ObtenerTerceroPorCardCode(oPedM.CardCode, IdEmpresa);
                if (oTItem == null || oTItem.IdTercero == 0)
                {
                    oTItem = new tblTerceroItem();
                    oTItem.TipoPersona = 1;
                    oTItem.Identificacion = oPedM.CedulaCliente.Trim();
                    oTItem.idTipoIdentificacion = 1;
                    oTItem.Nombre = oPedM.NombreClienteFacturacion;
                    oTItem.Mail = oPedM.Email;
                    oTItem.Telefono = oPedM.TelefonoFacturacion;
                    oTItem.Celular = oPedM.TelefonoEnvio;
                    oTItem.Direccion = oPedM.DireccionFacturacion.Trim().Replace("\"", "");
                    oTItem.idCiudad = IdCiudad;
                    oTItem.idEmpresa = IdEmpresa;
                    oTItem.TipoTercero = "C";
                    oTItem.FechaNacimiento = DateTime.Now;
                    oTItem.IdListaPrecio = 0;
                    oTItem.idGrupoCliente = 100;
                    oTItem.FechaModificacion = DateTime.Now;
                    oTItem.Observaciones = "";
                    oTItem.Activo = true;
                    oTItem.ProteccionDatos = true;
                    oTItem.FrecuenciaCompra = 0;
                    oTItem.EstablecimientoComercial = oPedM.NombreClienteFacturacion;
                    oTItem.IdUsuarioModificacion = IdUsuarioSistemas;
                    oTItem.oListDirecciones.Clear();
                    if (!string.IsNullOrEmpty(oPedM.DireccionFacturacion))
                    {
                        oTItem.oListDirecciones.Add(new tblTerceroDireccion()
                        {
                            Direccion = $"Facturación {oPedM.DireccionFacturacion}"
                        });
                    }

                    if (!string.IsNullOrEmpty(oPedM.DireccionEnvio))
                    {
                        oTItem.oListDirecciones.Add(new tblTerceroDireccion()
                        {
                            Direccion = $"Envio {oPedM.DireccionEnvio}"
                        });
                    }

                    if (!string.IsNullOrEmpty(oPedM.TelefonoFacturacion))
                    {
                        oTItem.oListCelulares.Add(new tblTerceroCelular()
                        {
                            Celular = oPedM.TelefonoFacturacion
                        });
                    }

                    if (!string.IsNullOrEmpty(oPedM.Email))
                    {
                        oTItem.oListCorreo.Add(new tblTerceroCorreo()
                        {
                            Correo = oPedM.Email
                        });
                    }
                    oTItem.FechaCreacion = DateTime.Now;
                    oTItem.CardCode = oPedM.CardCode;
                    oTItem.IdUsuario = IdUsuarioSistemas;
                    oPBiz.Guardar(oTItem);
                }
                if (oTItem.IdTercero == 0)
                {
                    return "Error al crear el Cliente en Positive";
                }
                else
                {
                    oDocItem.Fecha = DateTime.Now;
                    oDocItem.idTercero = oTItem.IdTercero;
                    oDocItem.Telefono = oPedM.TelefonoFacturacion;
                    oDocItem.Direccion = oPedM.DireccionFacturacion;
                    oDocItem.idCiudad = IdCiudad;
                    oDocItem.NombreTercero = oPedM.NombreClienteFacturacion;
                    oDocItem.Observaciones = oPedM.ComentariosDocumento;
                    oDocItem.idEmpresa = IdEmpresa;
                    oDocItem.idUsuario = IdUsuarioSistemas;
                    //oDocItem.TotalDocumento = ;
                    //oDocItem.TotalIVA = decimal.Parse(txtTotalIVA.Text, NumberStyles.Currency);
                    //oDocItem.saldo = oDocItem.TotalDocumento;
                    oDocItem.IdTipoDocumento = tblTipoDocumentoItem.TipoDocumentoEnum.pedido.GetHashCode();
                    oDocItem.EnCuadre = false;
                    oDocItem.IdEstado = 1; //1 --> Abierto
                    oDocItem.FechaVencimiento = DateTime.Now.AddDays(30);
                    oDocItem.IdUnidadNegocio = 6; //Saluti.com
                    oDocItem.Referencia = oPedM.IdPedido;
                    List<tblDetalleDocumentoItem> oListDet = new List<tblDetalleDocumentoItem>();
                    short NumeroLinea = 1;
                    foreach (var d in Detalles)
                    {
                        decimal valorIVA = 0;
                        decimal valorDescuento = 0;
                        tblArticuloItem oAItem = oPBiz.ObtenerArticuloPorCodigo(d.ItemCode);
                        tblBodegaItem oBItem = oPBiz.ObtenerBodegaPorWhsCode(d.WhsCode);
                        oDocItem.IdSede = oBItem.idSede;
                        oListDet.Add(new tblDetalleDocumentoItem()
                        {
                            NumeroLinea = NumeroLinea,
                            idArticulo = oAItem.IdArticulo,
                            Codigo = oAItem.CodigoArticulo,
                            Articulo = oAItem.Nombre,
                            ValorUnitario = (decimal)d.Price,
                            IVA = oAItem.IVAVenta,
                            Cantidad = (decimal)d.Quantity,
                            Descuento = (decimal)d.DiscountPercent,
                            idBodega = oBItem.IdBodega,
                            CostoPonderado = oAItem.CostoPonderado,
                            EsInventario = oAItem.EsInventario,
                            CostoPonderadoAllers = oAItem.CostoPonderadoAllers
                        });
                        valorDescuento = ((decimal)d.Price * ((decimal)d.DiscountPercent) / 100) * (decimal)d.Quantity;
                        valorIVA = (((decimal)d.Price - ((decimal)d.Price * ((decimal)d.DiscountPercent) / 100)) * oAItem.IVAVenta / 100) * (decimal)d.Quantity;
                        oDocItem.TotalDocumento = oDocItem.TotalDocumento + (decimal)(d.Price * d.Quantity) + valorIVA - valorDescuento;
                        oDocItem.TotalAntesIVA = oDocItem.TotalAntesIVA + (decimal)(d.Price * d.Quantity) - valorDescuento;
                        oDocItem.TotalIVA = oDocItem.TotalIVA + valorIVA;
                        oDocItem.TotalDescuento = oDocItem.TotalDescuento + valorDescuento;
                        NumeroLinea++;
                    }
                    oDocItem.saldo = oDocItem.TotalDocumento;
                    oPBiz.Guardar(oDocItem, oListDet, new tblPagoItem(), new List<tblTipoPagoItem>());
                    Message = oDocItem.idDocumento.ToString();
                    if (Message == "0")
                    {
                        return "Error al guardar el documento en Positive - Comunicarse con Aventi SAS";
                    }
                    else
                    {
                        var U_H_ENTREGA = "";
                        DateTime HoraCali = DateTime.Parse(string.Format("{0} {1}", DateTime.Now.ToShortDateString(), ConfigurationManager.AppSettings["HoraCali"]));
                        DateTime HoraOtros = DateTime.Parse(string.Format("{0} {1}", DateTime.Now.ToShortDateString(), ConfigurationManager.AppSettings["HoraOtros"]));
                        if (oPedM.RecogerTienda)
                        {
                            U_H_ENTREGA = "03"; //Hora de Entrega --> Vienen
                        }
                        else
                        {
                            if (oPedM.CiudadEnvio == "Cali")
                            {
                                if (DateTime.Now <= HoraCali)
                                {
                                    U_H_ENTREGA = "02"; //Hora de Entrega --> PM - Domicilios
                                }
                                else
                                {
                                    U_H_ENTREGA = "01"; //Hora de Entrega --> AM - Domicilios
                                }
                            }
                            else
                            {
                                U_H_ENTREGA = "05"; //Hora de Entrega --> Lejos
                            }
                        }

                        oPBiz.Guardar(new tblDocumentoCampoValorItem()
                        {
                            idDocumento = oDocItem.idDocumento,
                            idTipoDocumento = (short)tblTipoDocumentoItem.TipoDocumentoEnum.pedido.GetHashCode(),
                            Nombre = "U_H_ENTREGA",
                            Valor = U_H_ENTREGA
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return Message;
        }

        private string CrearPedidoPositiveCom(PedidoModel oPedM, List<PedidoDetalleModel> Detalles)
        {
            string Message = "";
            try
            {
                PositiveBiz oPBiz = new PositiveBiz();

                var result = oPBiz.getPedidoComID(oPedM.IdPedido);
                if (result > 0)
                {
                    return result.ToString();
                }
                else
                {
                    const long IdEmpresa = 1;
                    const long IdUsuarioSistemas = 2;
                    short IdCiudad = 1003;
                    if (!string.IsNullOrEmpty(oPedM.CodigoCiudad))
                    {
                        tblCiudadItem oCItem = oPBiz.ObtenerCiudadPorCodigo(oPedM.CodigoCiudad);
                        if (oCItem != null && oCItem.idCiudad > 0)
                        {
                            IdCiudad = oCItem.idCiudad;
                        }
                    }
                    tblDocumentoItem oDocItem = new tblDocumentoItem();
                    oPBiz.ActualizarTerceroCardCode(IdEmpresa);
                    tblTerceroItem oTItem = oPBiz.ObtenerTerceroPorCardCode(oPedM.CardCode, IdEmpresa);
                    if (oTItem == null || oTItem.IdTercero == 0)
                    {
                        oTItem = new tblTerceroItem();
                        oTItem.TipoPersona = 1;
                        oTItem.Identificacion = oPedM.CedulaCliente.Trim();
                        oTItem.idTipoIdentificacion = 1;
                        oTItem.Nombre = oPedM.NombreClienteFacturacion;
                        oTItem.Mail = oPedM.Email;
                        oTItem.Telefono = oPedM.TelefonoFacturacion;
                        oTItem.Celular = oPedM.TelefonoEnvio;
                        oTItem.Direccion = oPedM.DireccionFacturacion.Trim().Replace("\"", "");
                        oTItem.idCiudad = IdCiudad;
                        oTItem.idEmpresa = IdEmpresa;
                        oTItem.TipoTercero = "C";
                        oTItem.FechaNacimiento = DateTime.Now;
                        oTItem.IdListaPrecio = 0;
                        oTItem.idGrupoCliente = 100;
                        oTItem.FechaModificacion = DateTime.Now;
                        oTItem.Observaciones = "";
                        oTItem.Activo = true;
                        oTItem.ProteccionDatos = true;
                        oTItem.FrecuenciaCompra = 0;
                        oTItem.EstablecimientoComercial = oPedM.NombreClienteFacturacion;
                        oTItem.IdUsuarioModificacion = IdUsuarioSistemas;
                        oTItem.oListDirecciones.Clear();
                        if (!string.IsNullOrEmpty(oPedM.DireccionFacturacion))
                        {
                            oTItem.oListDirecciones.Add(new tblTerceroDireccion()
                            {
                                Direccion = $"Facturación {oPedM.DireccionFacturacion}"
                            });
                        }

                        if (!string.IsNullOrEmpty(oPedM.DireccionEnvio))
                        {
                            oTItem.oListDirecciones.Add(new tblTerceroDireccion()
                            {
                                Direccion = $"Envio {oPedM.DireccionEnvio}"
                            });
                        }

                        if (!string.IsNullOrEmpty(oPedM.TelefonoFacturacion))
                        {
                            oTItem.oListCelulares.Add(new tblTerceroCelular()
                            {
                                Celular = oPedM.TelefonoFacturacion
                            });
                        }

                        if (!string.IsNullOrEmpty(oPedM.Email))
                        {
                            oTItem.oListCorreo.Add(new tblTerceroCorreo()
                            {
                                Correo = oPedM.Email
                            });
                        }
                        oTItem.FechaCreacion = DateTime.Now;
                        oTItem.CardCode = oPedM.CardCode;
                        oTItem.IdUsuario = IdUsuarioSistemas;
                        oPBiz.Guardar(oTItem);
                    }
                    if (oTItem.IdTercero == 0)
                    {
                        return "Error al crear el Cliente en Positive";
                    }
                    else
                    {
                        oDocItem.Fecha = DateTime.Now;
                        oDocItem.idTercero = oTItem.IdTercero;
                        oDocItem.Telefono = oPedM.TelefonoFacturacion;
                        oDocItem.Direccion = oPedM.DireccionFacturacion;
                        oDocItem.idCiudad = IdCiudad;
                        oDocItem.NombreTercero = oPedM.NombreClienteFacturacion;
                        oDocItem.Observaciones = oPedM.ComentariosDocumento;
                        oDocItem.idEmpresa = IdEmpresa;
                        oDocItem.idUsuario = IdUsuarioSistemas;
                        //oDocItem.TotalDocumento = ;
                        //oDocItem.TotalIVA = decimal.Parse(txtTotalIVA.Text, NumberStyles.Currency);
                        //oDocItem.saldo = oDocItem.TotalDocumento;
                        oDocItem.IdTipoDocumento = tblTipoDocumentoItem.TipoDocumentoEnum.PedidoCom.GetHashCode();
                        oDocItem.EnCuadre = false;
                        oDocItem.IdEstado = 3; //3 --> Preliminar, 1--> Abierto
                        oDocItem.FechaVencimiento = DateTime.Now.AddDays(30);
                        oDocItem.IdUnidadNegocio = 6; //Saluti.com
                        oDocItem.Referencia = "Magento";
                        List<tblDetalleDocumentoItem> oListDet = new List<tblDetalleDocumentoItem>();
                        short NumeroLinea = 1;
                        foreach (var d in Detalles)
                        {
                            decimal valorIVA = 0;
                            decimal valorDescuento = 0;
                            tblArticuloItem oAItem = oPBiz.ObtenerArticuloPorCodigo(d.ItemCode);
                            tblBodegaItem oBItem = oPBiz.ObtenerBodegaPorWhsCode(d.WhsCode);
                            oDocItem.IdSede = oBItem.idSede;
                            oListDet.Add(new tblDetalleDocumentoItem()
                            {
                                NumeroLinea = NumeroLinea,
                                idArticulo = oAItem.IdArticulo,
                                Codigo = oAItem.CodigoArticulo,
                                Articulo = oAItem.Nombre,
                                ValorUnitario = (decimal)d.Price,
                                IVA = oAItem.IVAVenta,
                                Cantidad = (decimal)d.Quantity,
                                Descuento = (decimal)d.DiscountPercent,
                                idBodega = oBItem.IdBodega,
                                CostoPonderado = oAItem.CostoPonderado,
                                EsInventario = oAItem.EsInventario,
                                CostoPonderadoAllers = oAItem.CostoPonderadoAllers
                            });
                            valorDescuento = ((decimal)d.Price * ((decimal)d.DiscountPercent) / 100) * (decimal)d.Quantity;
                            valorIVA = (((decimal)d.Price - ((decimal)d.Price * ((decimal)d.DiscountPercent) / 100)) * oAItem.IVAVenta / 100) * (decimal)d.Quantity;
                            oDocItem.TotalDocumento = oDocItem.TotalDocumento + (decimal)(d.Price * d.Quantity) + valorIVA - valorDescuento;
                            oDocItem.TotalAntesIVA = oDocItem.TotalAntesIVA + (decimal)(d.Price * d.Quantity) - valorDescuento;
                            oDocItem.TotalIVA = oDocItem.TotalIVA + valorIVA;
                            oDocItem.TotalDescuento = oDocItem.TotalDescuento + valorDescuento;
                            NumeroLinea++;
                        }
                        oDocItem.saldo = oDocItem.TotalDocumento;
                        var error = oPBiz.Guardar(oDocItem, oListDet, new tblPagoItem(), new List<tblTipoPagoItem>());
                        if (error == "Exito")
                        {
                            Message = oDocItem.idDocumento.ToString();
                            if (Message == "0")
                            {
                                return "Error al guardar el documento en Positive - Comunicarse con Aventi SAS";
                            }
                            else
                            {
                                var U_H_ENTREGA = "";
                                DateTime HoraCali = DateTime.Parse(string.Format("{0} {1}", DateTime.Now.ToShortDateString(), ConfigurationManager.AppSettings["HoraCali"]));
                                DateTime HoraOtros = DateTime.Parse(string.Format("{0} {1}", DateTime.Now.ToShortDateString(), ConfigurationManager.AppSettings["HoraOtros"]));
                                if (oPedM.RecogerTienda)
                                {
                                    U_H_ENTREGA = "03"; //Hora de Entrega --> Vienen
                                }
                                else
                                {
                                    if (oPedM.CiudadEnvio == "Cali")
                                    {
                                        if (DateTime.Now <= HoraCali)
                                        {
                                            U_H_ENTREGA = "02"; //Hora de Entrega --> PM - Domicilios
                                        }
                                        else
                                        {
                                            U_H_ENTREGA = "01"; //Hora de Entrega --> AM - Domicilios
                                        }
                                    }
                                    else
                                    {
                                        U_H_ENTREGA = "05"; //Hora de Entrega --> Lejos
                                    }
                                }

                                oPBiz.Guardar(new tblDocumentoCampoValorItem()
                                {
                                    idDocumento = oDocItem.idDocumento,
                                    idTipoDocumento = (short)tblTipoDocumentoItem.TipoDocumentoEnum.PedidoCom.GetHashCode(),
                                    Nombre = "U_H_ENTREGA",
                                    Valor = U_H_ENTREGA
                                });

                                oPBiz.Guardar(new tblDocumentoCampoValorItem()
                                {
                                    idDocumento = oDocItem.idDocumento,
                                    idTipoDocumento = (short)tblTipoDocumentoItem.TipoDocumentoEnum.PedidoCom.GetHashCode(),
                                    Nombre = "U_NumOrderMedishop",
                                    Valor = oPedM.IdPedido
                                });
                            }
                        }
                        else
                        {
                            Message = error;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return Message;
        }

        private string CrearPedidoSaluti(PedidoModel oPedM, List<PedidoDetalleModel> Detalles, Company oCompanySaluti, string WhsCode)
        {
            //Crear pedido SAP Saluti
            string Message = "";
            Documents doc = (Documents)oCompanySaluti.GetBusinessObject(BoObjectTypes.oOrders);
            doc.CardCode = oPedM.CardCode;
            doc.DocDueDate = oPedM.FechaDespacho;
            doc.CardName = oPedM.NombreClienteFacturacion;
            doc.DiscountPercent = oPedM.Descuento;
            doc.Confirmed = BoYesNoEnum.tYES;
            doc.UserFields.Fields.Item("U_NumEcommerce").Value = oPedM.IdPedido;
            if (!string.IsNullOrEmpty(oPedM.MedioPago))
            {
                if (oPedM.Datafono == 1)
                {
                    doc.UserFields.Fields.Item("U_MedioPago").Value = string.Format("{0} - Con Datafono", oPedM.MedioPago);
                }
                else
                {
                    doc.UserFields.Fields.Item("U_MedioPago").Value = oPedM.MedioPago;
                }
            }
            if (oPedM.IdUsuario != 0)
            {
                doc.UserFields.Fields.Item("U_UsuarioCall").Value = oPedM.IdUsuario.ToString();
            }
            if (!string.IsNullOrEmpty(oPedM.Email))
            {
                doc.UserFields.Fields.Item("U_Email").Value = oPedM.Email;
            }
            ProductoDao oPDao = new ProductoDao();
            SedeModel oSModel = oPDao.ObtenerSedeByWhsDefault(WhsCode);

            doc.UserFields.Fields.Item("U_Sede").Value = oSModel.IdSede.ToString();
            doc.UserFields.Fields.Item("U_UnidadNegocio").Value = "6";
            foreach (PedidoDetalleModel Item in Detalles)
            {
                doc.Lines.ItemCode = Item.ItemCode;
                doc.Lines.Quantity = Item.Quantity;
                doc.Lines.UnitPrice = Item.Price;
                doc.Lines.DiscountPercent = Item.DiscountPercent;
                doc.Lines.WarehouseCode = Item.WhsCode;
                doc.Lines.COGSCostingCode = "62";
                doc.Lines.COGSCostingCode2 = "6215";
                doc.Lines.COGSCostingCode3 = "621505";
                doc.Lines.Add();
            }
            doc.Comments = oPedM.Comentarios + " Cliente para envío:" + oPedM.NombreClienteEnvio;
            doc.TaxExtension.CityS = oPedM.CiudadEnvio;
            doc.TaxExtension.StreetS = oPedM.DireccionEnvio;
            doc.TaxExtension.CountryS = oPedM.PaisEnvio;
            doc.TaxExtension.StateS = oPedM.DepartamentoEnvio;
            doc.TaxExtension.CityB = oPedM.CiudadFacturacion;
            doc.TaxExtension.StateB = oPedM.DepartamentoFacturacion;
            doc.TaxExtension.StreetB = oPedM.DireccionFacturacion;
            doc.TaxExtension.CountryB = oPedM.PaisFacturacion;
            doc.Add();
            int errorCode = 0;
            string errorDesc = "";
            oCompanySaluti.GetLastError(out errorCode, out errorDesc);
            if (errorCode != 0)
            {
                if (errorDesc.Split(' ')[0] == "(100)")
                {
                    Message = errorDesc.Split(' ')[1];
                }
                else
                {
                    Message = errorDesc;
                }
            }
            else
            {
                doc.GetByKey(int.Parse(oCompanySaluti.GetNewObjectKey()));
                if (!string.IsNullOrEmpty(oPedM.IdPedido))
                {
                    doc.UserFields.Fields.Item("U_NumEcommerce").Value = oPedM.IdPedido;
                    doc.Update();
                }
                Message = doc.DocNum.ToString();
            }
            return Message;
        }

        //Metodo nuevo
        [Route("api/Pedido/EstadoPedidos")]
        [HttpGet]
        public HttpResponseMessage GetEstadoPedidos()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);

            Dao.PedidoDao oPedD = new Dao.PedidoDao();
            DataTable Lista;
            try
            {
                Lista = oPedD.GetOrdersStatus();
                response.Content = new StringContent(JsonConvert.SerializeObject(Lista), Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(ex.Message), Encoding.UTF8, "application/json");
            }
            return response;
        }
        //Fin metodo nuevo

        /// <summary>
        /// Método para crear una cotización
        /// </summary>
        /// <param name="oPedM"></param>
        /// <returns></returns>
        [Route("api/Pedido/CreateQuote")]
        [HttpPost]
        public HttpResponseMessage CreateQuote([FromBody] PedidoModel oPedM)
        {
            //var response = Request.CreateResponse(HttpStatusCode.OK);
            //response.Content = new StringContent(JsonConvert.SerializeObject(new { err = oPedM }), Encoding.UTF8, "application/json");
            //return response;
            string numeroPedido = "";
            var response = Request.CreateResponse(HttpStatusCode.OK);
            Company oCompany = ObtenerCompanySAP();
            try
            {
                Documents doc = oCompany.GetBusinessObject(BoObjectTypes.oQuotations) as Documents;
                doc.CardCode = oPedM.CardCode;
                doc.DocDueDate = oPedM.FechaDespacho;
                if (oPedM.Ocasional)
                {
                    doc.CardName = oPedM.CardName;
                    doc.AddressExtension.ShipToStreet = oPedM.DireccionEnvio;
                    doc.AddressExtension.ShipToCountry = "CO";
                    doc.AddressExtension.ShipToState = oPedM.DepartamentoEnvio;
                    doc.AddressExtension.ShipToCity = oPedM.CiudadEnvio;
                    doc.UserFields.Fields.Item("U_NIT").Value = oPedM.CedulaCliente;
                    doc.UserFields.Fields.Item("U_RAZONSOCIAL").Value = oPedM.CardName;
                    doc.UserFields.Fields.Item("U_TelefonoCliente").Value = oPedM.TelefonoEnvio;
                    doc.UserFields.Fields.Item("U_DireccionCliente").Value = oPedM.DireccionEnvio;
                    doc.UserFields.Fields.Item("U_Mail").Value = oPedM.Email;
                }
                doc.UserFields.Fields.Item("U_NumOrderMedishop").Value = oPedM.IdPedido;
                doc.UserFields.Fields.Item("U_TipoCotizacion").Value = oPedM.TipoCotizacion;
                doc.UserFields.Fields.Item("U_BusqInteligente").Value = "1";
                if (!string.IsNullOrEmpty(oPedM.Foco))
                {
                    doc.UserFields.Fields.Item("U_Foco").Value = oPedM.Foco;
                }
                doc.Comments = oPedM.Comentarios;
                doc.DocumentsOwner = oPedM.IdUsuario;
                foreach (PedidoDetalleModel Item in oPedM.Detalles)
                {
                    doc.Lines.ItemCode = Item.ItemCode;
                    doc.Lines.Quantity = Item.Quantity;
                    doc.Lines.UnitPrice = Item.Price;
                    doc.Lines.DiscountPercent = Item.DiscountPercent;
                    doc.Lines.Add();
                }
                doc.Add();
                int errorCode = 0;
                string errorDesc = "";
                oCompany.GetLastError(out errorCode, out errorDesc);
                if (errorCode != 0)
                    numeroPedido = string.Format("ErrorCode -> {0}, ErrorDesc -> {1}", errorCode, errorDesc);
                else
                {
                    oCompany.GetNewObjectCode(out errorDesc);
                    doc.GetByKey(int.Parse(oCompany.GetNewObjectKey()));
                    numeroPedido = doc.DocNum.ToString();
                }
                response.Content = new StringContent(JsonConvert.SerializeObject(new { err = numeroPedido }), Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(new { err = ex.Message }), Encoding.UTF8, "application/json");
                return response;
            }
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
                else if (ConfigurationManager.AppSettings["BDServerSAP"] == "2017")
                    oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2017;
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
        public async Task<Company> OpenSAP()
        {
            return await Task.Run(() =>
            {
                Company oCompany;
                try
                {
                    if (HttpRuntime.Cache["oCompanyIsearchs"] == null)
                    {
                        oCompany = new Company
                        {
                            Server = ConfigurationManager.AppSettings["hostSAP"],
                            LicenseServer = ConfigurationManager.AppSettings["LicSAP"],
                            CompanyDB = ConfigurationManager.AppSettings["CompanyDBSAP"],
                            UserName = ConfigurationManager.AppSettings["usrSAP"],
                            Password = ConfigurationManager.AppSettings["pwdSAP"],
                            DbUserName = ConfigurationManager.AppSettings["DbUserNameSAP"],
                            DbPassword = ConfigurationManager.AppSettings["DbPasswordSAP"],
                            UseTrusted = false,
                            language = BoSuppLangs.ln_Spanish_La
                        };

                        // Configure database server type
                        switch (ConfigurationManager.AppSettings["BDServerSAP"])
                        {
                            case "2005":
                                oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2005;
                                break;
                            case "2008":
                                oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2008;
                                break;
                            case "2012":
                                oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2012;
                                break;
                            case "2014":
                                oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2014;
                                break;
                            case "2016":
                                oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2016;
                                break;
                            case "2017":
                                oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2017;
                                break;
                            case "2019":
                                oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2019;
                                break;
                            default:
                                oCompany.DbServerType = BoDataServerTypes.dst_HANADB;
                                break;
                        }

                        HttpRuntime.Cache.Add("oCompanyIsearchs", oCompany, null, DateTime.Now.AddDays(15), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    }
                    else
                    {
                        oCompany = (Company)HttpRuntime.Cache["oCompanyIsearchs"];
                    }

                    if (!oCompany.Connected)
                    {
                        oCompany.Connect();
                    }

                    return oCompany;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        private Company ObtenerCompanySAPSaluti()
        {

            var oCompanySaluti = ConfigurationManager.AppSettings["oCompanySaluti"];
            Company oCompany;
            if (HttpRuntime.Cache[oCompanySaluti] == null)
            {
                oCompany = new Company();
                oCompany.Server = ConfigurationManager.AppSettings["hostSAP"];
                oCompany.LicenseServer = ConfigurationManager.AppSettings["LicSAP"];
                oCompany.CompanyDB = ConfigurationManager.AppSettings["CompanyDBSAPSaluti"];
                oCompany.UserName = ConfigurationManager.AppSettings["usrSAP"];
                oCompany.Password = ConfigurationManager.AppSettings["pwdSAPSaluti"];
                oCompany.UseTrusted = false;
                oCompany.DbUserName = ConfigurationManager.AppSettings["DbUserNameSAP"];
                oCompany.DbPassword = ConfigurationManager.AppSettings["DbPasswordSAP"];
                if (ConfigurationManager.AppSettings["BDServerSAP"] == "2005")
                    oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2005;
                else if (ConfigurationManager.AppSettings["BDServerSAP"] == "2008")
                    oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2008;
                else if (ConfigurationManager.AppSettings["BDServerSAP"] == "2012")
                    oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2012;
                else
                    oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2019;
                HttpRuntime.Cache.Add(oCompanySaluti, oCompany, null, DateTime.Now.AddDays(5), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
            else
            {
                oCompany = (Company)HttpRuntime.Cache[oCompanySaluti];
            }
            if (!oCompany.Connected)
            {
                oCompany.Connect();
            }
            return oCompany;
        }

        private async Task<Company> ObtenerCompanySAPSalutiAsync()
        {
            return await Task.Run(() =>
            {
                Company oCompany;
                try
                {
                    if (HttpRuntime.Cache["oCompanySaluti"] == null)
                    {
                        oCompany = new Company();
                        oCompany.Server = ConfigurationManager.AppSettings["hostSAP"];
                        oCompany.LicenseServer = ConfigurationManager.AppSettings["LicSAP"];
                        oCompany.CompanyDB = ConfigurationManager.AppSettings["CompanyDBSAPSaluti"];
                        oCompany.UserName = ConfigurationManager.AppSettings["usrSAP"];
                        oCompany.Password = ConfigurationManager.AppSettings["pwdSAPSaluti"];
                        oCompany.UseTrusted = false;
                        oCompany.DbUserName = ConfigurationManager.AppSettings["DbUserNameSAP"];
                        oCompany.DbPassword = ConfigurationManager.AppSettings["DbPasswordSAP"];
                        if (ConfigurationManager.AppSettings["BDServerSAP"] == "2005")
                            oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2005;
                        else if (ConfigurationManager.AppSettings["BDServerSAP"] == "2008")
                            oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2008;
                        else if (ConfigurationManager.AppSettings["BDServerSAP"] == "2012")
                            oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2012;
                        else
                            oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2019;
                        HttpRuntime.Cache.Add("oCompanySaluti", oCompany, null, DateTime.Now.AddDays(5), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    }
                    else
                    {
                        oCompany = (Company)HttpRuntime.Cache["oCompanySaluti"];
                    }
                    if (!oCompany.Connected)
                    {
                        oCompany.Connect();
                    }
                    return oCompany;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        /// <summary>
        /// Método para obtener el historico de pedidos por clientes
        /// </summary>
        /// <returns>Historico de pedidos por clientes</returns>
        [Route("api/Pedido/ObtenerPedidosEstadoSaluti/{CantidadInicio}/{CantidadFija}")]
        [HttpGet]
        public HttpResponseMessage ObtenerPedidosEstadoSaluti(int CantidadInicio, int CantidadFija)
        {
            Dao.PedidoDao oPed = new Dao.PedidoDao();
            DataTable Lista;
            try
            {
                Lista = oPed.ObtenerPedidosEstadoSaluti(CantidadInicio, CantidadFija);
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
        /// Método para obtener el historico de pedidos por clientes
        /// </summary>
        /// <returns>Historico de pedidos por clientes</returns>
        [Route("api/Pedido/ObtenerTrackingPedido/{DocEntry}")]
        [HttpGet]
        public HttpResponseMessage ObtenerTrackingPedido(int DocEntry)
        {
            Dao.PedidoDao oPed = new Dao.PedidoDao();
            DataTable Lista;
            try
            {
                Lista = oPed.ObtenerTrackingPedido(DocEntry);
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

    public class RespuestaCom
    {
        public bool Completo { get; set; }
        public string NumPedidoSaluti { get; set; }

        public List<tblPedidoComPedidosItem> PedidosCreados { get; set; }

        public RespuestaCom(bool Completo, string NumPedidoSaluti, List<tblPedidoComPedidosItem> PedidosCreados)
        {
            this.Completo = Completo;
            this.NumPedidoSaluti = NumPedidoSaluti;
            this.PedidosCreados = PedidosCreados;
        }
    }
}