using HBT_CalculoRetenciones;/*Sí*/
using Newtonsoft.Json;/*Sí*/
using SAPbobsCOM;/*Sí*/
using System;/*Sí*/
using System.Collections.Generic;/*Sí*/
using System.Net;/*Sí*/
using System.Net.Http;/*Sí*/
using System.Text;/*Sí*/
using System.Web;/*Sí*/
using WebServiceAllers.Business;/*Sí*/
using WebServiceAllers.Dao;/*Sí*/
using WebServiceAllers.Models;/*Sí*/
using System.Web.Http;/*Sí*/
using System.Web.Caching;/*Sí*/
using System.Configuration;/*Sí*/
using WebServiceAllers.Dispapeles;
using System.ServiceModel;
using System.Web.Http.Cors;
using System.Threading.Tasks;

namespace WebServiceAllers.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AllowAnonymous]
    public class DocumentoController : ApiController
    {
        private enum enumTipoPago
        {
            Transferencia = 1,
            Tarjeta = 2,
            Efectivo = 3,
            Cheque = 4
        }

        [Route("api/Documento/ProbarConexion")]
        [HttpPost]
        public HttpResponseMessage ProbarConexion([FromBody] PruebaModel oOrdM)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                if (string.IsNullOrEmpty(oOrdM.CardCode))
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest);
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { error = "Por favor digitar un CardCode valido" }), Encoding.UTF8, "application/json");
                }
                else
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { oOrdM }), Encoding.UTF8, "application/json");
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(new { ex.Message }), Encoding.UTF8, "application/json");
            }
            return response;
        }

        [Route("api/Documento/ClosePurchaseOrders")]
        [HttpPost]
        public HttpResponseMessage ClosePurchaseOrders([FromBody] OrdenCompraModel oOrdM)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                int errorCode = 0;
                string errorDesc = string.Empty;
                Company oCompanySaluti = new Company();
                oCompanySaluti = ObtenerCompanySAPSaluti();
                if (oCompanySaluti.Connected)
                {
                    Documents oDoc = (Documents)oCompanySaluti.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
                    oDoc.GetByKey(oOrdM.DocEntry);
                    bool ValidarCambios = false;
                    for (int i = 0; i < oDoc.Lines.Count; i++)
                    {
                        oDoc.Lines.SetCurrentLine(i);
                        foreach (OrdenCompraDetalleModel Item in oOrdM.Detalles)
                        {
                            if (oDoc.Lines.LineStatus == BoStatus.bost_Open && oDoc.Lines.VisualOrder == Item.VisOrder)
                            {
                                oDoc.Lines.LineStatus = BoStatus.bost_Close;
                                ValidarCambios = true;
                            }
                        }
                    }
                    if (ValidarCambios)
                    {
                        oDoc.Update();
                        oCompanySaluti.GetLastError(out errorCode, out errorDesc);
                        if (errorCode != 0)
                        {
                            response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                            response.Content = new StringContent(JsonConvert.SerializeObject(new { errorCode, errorDesc }), Encoding.UTF8, "application/json");
                        }
                        else
                        {
                            response.Content = new StringContent(JsonConvert.SerializeObject(new { errorCode, errorDesc }), Encoding.UTF8, "application/json");
                        }
                    }
                }
                else
                {
                    oCompanySaluti.GetLastError(out errorCode, out errorDesc);
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { errorCode, errorDesc }), Encoding.UTF8, "application/json");
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

        [Route("api/Documento/CreateSaluti")]
        [HttpPost]
        public HttpResponseMessage CreateOrder([FromBody] DocumentoModel oDocM)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                int errorCode = 0;
                string errorDesc = "";
                Company oCompanySaluti = ObtenerCompanySAPSaluti();
                if (oCompanySaluti.Connected)
                {
                    if (oCompanySaluti.InTransaction == true)
                    {
                        oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                    }
                    oCompanySaluti.StartTransaction();
                    //Se debe realizar la validación si el cliete ya existe en el SAP Saluti
                    BusinessPartners Cliente = (BusinessPartners)oCompanySaluti.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                    Cliente.GetByKey(string.Format("CN{0}", oDocM.BusinessPartner.LicTradNum));
                    if (string.IsNullOrEmpty(Cliente.CardCode))
                    {
                        Cliente.CardCode = string.Format("CN{0}", oDocM.BusinessPartner.LicTradNum);
                        Cliente.CardName = oDocM.BusinessPartner.CardName;
                        Cliente.CardType = BoCardTypes.cCustomer;
                        Cliente.GroupCode = 100;
                        Cliente.FederalTaxID = oDocM.BusinessPartner.LicTradNum;
                        Cliente.Phone1 = oDocM.BusinessPartner.Telefono;
                        Cliente.Cellular = oDocM.BusinessPartner.Celular;
                        Cliente.EmailAddress = oDocM.BusinessPartner.Email;
                        //Direccion de envio
                        Cliente.Addresses.AddressName = "Direccion Envio Saluti";
                        Cliente.Addresses.Street = oDocM.BusinessPartner.Direccion;
                        Cliente.Addresses.City = oDocM.BusinessPartner.Ciudad;
                        Cliente.Addresses.Country = oDocM.BusinessPartner.Pais;
                        Cliente.Addresses.State = oDocM.BusinessPartner.Departamento;
                        Cliente.Addresses.County = oDocM.BusinessPartner.Departamento;
                        Cliente.Addresses.Block = oDocM.BusinessPartner.Barrio;
                        Cliente.Addresses.AddressType = BoAddressType.bo_ShipTo;
                        Cliente.Addresses.Add();
                        //Direccion de pago
                        Cliente.Addresses.AddressName = "Direccion Pago Saluti";
                        Cliente.Addresses.Street = oDocM.BusinessPartner.Direccion;
                        Cliente.Addresses.City = oDocM.BusinessPartner.Ciudad;
                        Cliente.Addresses.Country = oDocM.BusinessPartner.Pais;
                        Cliente.Addresses.State = oDocM.BusinessPartner.Departamento;
                        Cliente.Addresses.County = oDocM.BusinessPartner.Departamento;
                        Cliente.Addresses.Block = oDocM.BusinessPartner.Barrio;
                        Cliente.Addresses.AddressType = BoAddressType.bo_BillTo;
                        Cliente.Addresses.Add();
                        if (oDocM.BusinessPartner.TipoDocumento == "1")
                        {
                            Cliente.UserFields.Fields.Item("U_BPCO_TDC").Value = "31";
                        }
                        else if (oDocM.BusinessPartner.TipoDocumento == "2")
                        {
                            Cliente.UserFields.Fields.Item("U_BPCO_TDC").Value = "13";
                        }
                        else
                        {
                            Cliente.UserFields.Fields.Item("U_BPCO_TDC").Value = "21";
                        }
                        Cliente.UserFields.Fields.Item("U_BPCO_TP").Value = string.Format("0{0}", oDocM.BusinessPartner.TipoPersona);
                        Cliente.Add();
                        oCompanySaluti.GetLastError(out errorCode, out errorDesc);
                        if (errorCode != 0)
                        {
                            if (oCompanySaluti.InTransaction == true)
                            {
                                oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                            response = Request.CreateResponse(HttpStatusCode.BadRequest);
                            response.Content = new StringContent(JsonConvert.SerializeObject(new { errorCode, errorDesc }), Encoding.UTF8, "application/json");
                            return response;
                        }
                        else
                        {
                            oDocM.CardCode = oCompanySaluti.GetNewObjectKey();
                        }
                    }
                    else
                    {
                        oDocM.CardCode = Cliente.CardCode;
                        if (oDocM.BusinessPartner.TipoDocumento == "1")
                        {
                            Cliente.UserFields.Fields.Item("U_BPCO_TDC").Value = "31";
                        }
                        else if (oDocM.BusinessPartner.TipoDocumento == "2")
                        {
                            Cliente.UserFields.Fields.Item("U_BPCO_TDC").Value = "13";
                        }
                        else
                        {
                            Cliente.UserFields.Fields.Item("U_BPCO_TDC").Value = "21";
                        }
                        Cliente.UserFields.Fields.Item("U_BPCO_TP").Value = string.Format("0{0}", oDocM.BusinessPartner.TipoPersona);
                        Cliente.Update();
                    }
                    if (!string.IsNullOrEmpty(oDocM.CardCode))
                    {
                        bool ValidarItemsInventario = false;
                        errorCode = 0;
                        Documents ent = (Documents)oCompanySaluti.GetBusinessObject(BoObjectTypes.oInventoryGenEntry);
                        ent.Comments = "Entrada de mercancía para facturación electrónica desde Positive.";
                        foreach (DocumentoDetalleModel Item in oDocM.Detalles)
                        {
                            Items oItem = (Items)oCompanySaluti.GetBusinessObject(BoObjectTypes.oItems);
                            oItem.GetByKey(Item.ItemCode);
                            if (oItem.InventoryItem == BoYesNoEnum.tYES)
                            {
                                ent.Lines.ItemCode = Item.ItemCode;
                                ent.Lines.Quantity = Item.Quantity;
                                ent.Lines.WarehouseCode = Item.WhsCode;
                                ent.Lines.UnitPrice = Item.Costo;
                                ent.Lines.Add();
                                ValidarItemsInventario = true;
                            }
                        }
                        ent.UserFields.Fields.Item("U_UnidadNegocio").Value = oDocM.IdUnidadNegocio.ToString();
                        ent.UserFields.Fields.Item("U_Sede").Value = oDocM.IdSede.ToString();
                        if (ValidarItemsInventario)
                        {
                            ent.Add();
                            oCompanySaluti.GetLastError(out errorCode, out errorDesc);
                        }
                        if (errorCode != 0)
                        {
                            if (oCompanySaluti.InTransaction == true)
                            {
                                oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                            response = Request.CreateResponse(HttpStatusCode.BadRequest);
                            response.Content = new StringContent(JsonConvert.SerializeObject(new { errorDesc, errorCode }), Encoding.UTF8, "application/json");
                            return response;
                        }
                        else
                        {
                            Documents doc = (Documents)oCompanySaluti.GetBusinessObject(BoObjectTypes.oInvoices);
                            HBT_CalculoRetenciones.Entities.ResultadoOperacion resultado;
                            doc.CardCode = oDocM.CardCode;
                            doc.Series = oDocM.Serie;
                            doc.DocDueDate = oDocM.DocDueDate;
                            doc.CardName = oDocM.BusinessPartner.CardName;
                            if (!string.IsNullOrEmpty(oDocM.BusinessPartner.Email))
                            {
                                doc.UserFields.Fields.Item("U_Email").Value = oDocM.BusinessPartner.Email;
                            }
                            if (!string.IsNullOrEmpty(oDocM.Numero))
                            {
                                doc.UserFields.Fields.Item("U_NumEcommerce").Value = oDocM.Numero;
                            }
                            if (!string.IsNullOrEmpty(oDocM.IdUsuarioFactura))
                            {
                                doc.UserFields.Fields.Item("U_UsuarioFact").Value = oDocM.IdUsuarioFactura;
                            }
                            if (!string.IsNullOrEmpty(oDocM.IdUsuarioOrigen))
                            {
                                doc.UserFields.Fields.Item("U_UsuarioCall").Value = oDocM.IdUsuarioOrigen;
                            }
                            doc.UserFields.Fields.Item("U_UnidadNegocio").Value = oDocM.IdUnidadNegocio.ToString();
                            doc.UserFields.Fields.Item("U_Sede").Value = oDocM.IdSede.ToString();
                            foreach (DocumentoDetalleModel Item in oDocM.Detalles)
                            {
                                doc.Lines.ItemCode = Item.ItemCode;
                                doc.Lines.Quantity = Item.Quantity;
                                doc.Lines.UnitPrice = Item.Price;
                                doc.Lines.DiscountPercent = Item.DiscountPercent;
                                doc.Lines.WarehouseCode = Item.WhsCode;
                                if (!string.IsNullOrEmpty(Item.CentroCosto))
                                {
                                    doc.Lines.COGSCostingCode = Item.CentroCosto.Substring(0, 2);
                                    doc.Lines.COGSCostingCode2 = Item.CentroCosto.Substring(0, 4);
                                    doc.Lines.COGSCostingCode3 = Item.CentroCosto;
                                }
                                doc.Lines.UserFields.Fields.Item("U_IdCampana").Value = Item.IdCampana.ToString();
                                doc.Lines.UserFields.Fields.Item("U_IdListaPrecio").Value = Item.IdListaPrecio.ToString();
                                doc.Lines.Add();
                            }
                            doc.Comments = oDocM.Comentarios;
                            resultado = CalculoRetenciones.CrearObjetoSAPAsientoTerceroValido(oCompanySaluti, doc, true, true, true, 0);
                            if (resultado.ErrorCode != 0 || resultado.ErrorCodeAsiento != 0)
                            {
                                errorCode = resultado.ErrorCode != 0 ? resultado.ErrorCode : resultado.ErrorCodeAsiento;
                                errorDesc = resultado.ErrorCode != 0 ? resultado.ErrorDescription : resultado.ErrorDescriptionAsiento;
                                if (oCompanySaluti.InTransaction == true)
                                {
                                    oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                                }
                                response = Request.CreateResponse(HttpStatusCode.BadRequest);
                                response.Content = new StringContent(JsonConvert.SerializeObject(new { errorDesc, errorCode }), Encoding.UTF8, "application/json");
                                return response;
                            }
                            else
                            {
                                doc = (Documents)resultado.Documento;
                                bool FormaPago = ValidarFormaPago(oDocM.Pago);
                                if (FormaPago)
                                {
                                    double TotalPago = 0;
                                    Payments payment = (Payments)oCompanySaluti.GetBusinessObject(BoObjectTypes.oIncomingPayments);
                                    payment.CardCode = doc.CardCode;
                                    payment.UserFields.Fields.Item("U_Pago").Value = oDocM.IdUsuarioFactura;
                                    foreach (PagoModel oPag in oDocM.Pago)
                                    {
                                        if (oPag.TipoPago == enumTipoPago.Cheque.GetHashCode())
                                        {
                                            payment.CheckAccount = oPag.CheckAccount;
                                            payment.Checks.CheckSum = oPag.CheckSum.Value;
                                            payment.Checks.CountryCode = "CO";
                                            payment.Checks.CheckNumber = oPag.CheckNumber.Value;
                                            payment.Checks.BankCode = oPag.BankCode;
                                            payment.Checks.DueDate = DateTime.Now;
                                            payment.Checks.Trnsfrable = BoYesNoEnum.tYES;
                                            TotalPago = TotalPago + oPag.CheckSum.Value;
                                        }
                                        if (oPag.TipoPago == enumTipoPago.Transferencia.GetHashCode())
                                        {
                                            payment.TransferSum = oPag.TrsfrSum.Value;
                                            payment.TransferAccount = oPag.TrsfrAcct;
                                            payment.TransferReference = oPag.TrsfrRef;
                                            payment.TransferDate = DateTime.Now;
                                            TotalPago = TotalPago + oPag.TrsfrSum.Value;
                                        }
                                        else if (oPag.TipoPago == enumTipoPago.Tarjeta.GetHashCode())
                                        {
                                            payment.CreditCards.CreditCard = oPag.CreditCard.Value;
                                            payment.CreditCards.CreditAcct = oPag.CreditAcct;
                                            payment.CreditCards.CreditCardNumber = oPag.CreditCardNumber;
                                            payment.CreditCards.CardValidUntil = DateTime.Now;//oPag.CardValidUntil.Value;
                                            payment.CreditCards.CreditSum = oPag.CreditSum.Value;
                                            payment.CreditCards.VoucherNum = oPag.VoucherNum;
                                            payment.CreditCards.CreditType = BoRcptCredTypes.cr_Regular;
                                            if (!string.IsNullOrEmpty(oPag.CamposUsuario))
                                            {
                                                foreach (string CampoUsuario in oPag.CamposUsuario.Split('|'))
                                                {
                                                    payment.CreditCards.UserFields.Fields.Item(CampoUsuario.Split('~')[0]).Value = CampoUsuario.Split('~')[1];
                                                }
                                            }
                                            TotalPago = TotalPago + oPag.CreditSum.Value;
                                            payment.CreditCards.Add();
                                        }
                                        else if (oPag.TipoPago == enumTipoPago.Efectivo.GetHashCode())
                                        {
                                            payment.CashAccount = oPag.CashAccount;
                                            payment.CashSum = oPag.CashSum.Value;
                                            TotalPago = TotalPago + oPag.CashSum.Value;
                                        }
                                    }
                                    payment.Remarks = "Pago regisrado desde Positive";
                                    payment.Invoices.DocEntry = doc.DocEntry;
                                    payment.Invoices.InvoiceType = BoRcptInvTypes.it_Invoice;
                                    payment.Invoices.SumApplied = TotalPago;
                                    payment.Invoices.Add();
                                    payment.Add();
                                    oCompanySaluti.GetLastError(out errorCode, out errorDesc);
                                    if (errorCode != 0)
                                    {
                                        if (oCompanySaluti.InTransaction == true)
                                        {
                                            oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                                        }
                                        response = Request.CreateResponse(HttpStatusCode.BadRequest);
                                        response.Content = new StringContent(JsonConvert.SerializeObject(new { errorDesc, errorCode }), Encoding.UTF8, "application/json");
                                        return response;
                                    }
                                }
                                if (oDocM.HacerNC)
                                {
                                    Documents NotaCredito = (Documents)oCompanySaluti.GetBusinessObject(BoObjectTypes.oCreditNotes);
                                    NotaCredito.CardCode = oDocM.CardCode;
                                    foreach (DocumentoDetalleModel Item in oDocM.Detalles)
                                    {
                                        NotaCredito.Lines.ItemCode = Item.ItemCode;
                                        NotaCredito.Lines.Quantity = Item.Quantity;
                                        NotaCredito.Lines.UnitPrice = Item.Price;
                                        NotaCredito.Lines.DiscountPercent = Item.DiscountPercent;
                                        NotaCredito.Lines.WarehouseCode = Item.WhsCode;
                                        NotaCredito.Lines.Add();
                                    }
                                    NotaCredito.Comments = string.Format("", doc.DocNum);
                                    resultado = CalculoRetenciones.CrearObjetoSAPAsientoTerceroValido(oCompanySaluti, NotaCredito, true, true, true, 0);
                                    if (resultado.ErrorCode != 0 || resultado.ErrorCodeAsiento != 0)
                                    {
                                        errorCode = resultado.ErrorCode != 0 ? resultado.ErrorCode : resultado.ErrorCodeAsiento;
                                        errorDesc = resultado.ErrorCode != 0 ? resultado.ErrorDescription : resultado.ErrorDescriptionAsiento;
                                        if (oCompanySaluti.InTransaction == true)
                                        {
                                            oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                                        }
                                        response = Request.CreateResponse(HttpStatusCode.BadRequest);
                                        response.Content = new StringContent(JsonConvert.SerializeObject(new { errorDesc, errorCode }), Encoding.UTF8, "application/json");
                                        return response;
                                    }
                                }
                                oCompanySaluti.EndTransaction(BoWfTransOpt.wf_Commit);
                                FacturaEletronica(doc, oCompanySaluti);
                                List<DocumentoDetalleModel> Detalles = new List<DocumentoDetalleModel>();
                                for (int i = 0; i < doc.Lines.Count; i++)
                                {
                                    DocumentoDetalleModel Det = new DocumentoDetalleModel();
                                    doc.Lines.SetCurrentLine(i);
                                    Det.ItemCode = doc.Lines.ItemCode;
                                    Det.Quantity = doc.Lines.Quantity;
                                    Det.WhsCode = doc.Lines.WarehouseCode;
                                    Det.Price = doc.Lines.UnitPrice;
                                    Det.DiscountPercent = doc.Lines.DiscountPercent;
                                    Detalles.Add(Det);
                                }
                                response = Request.CreateResponse(HttpStatusCode.Created);
                                response.Content = new StringContent(JsonConvert.SerializeObject(new { doc.DocNum, doc.DocEntry, Data = Detalles }), Encoding.UTF8, "application/json");
                                return response;
                            }
                        }
                    }
                    else
                    {
                        if (oCompanySaluti.InTransaction == true)
                        {
                            oCompanySaluti.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }
                        response = Request.CreateResponse(HttpStatusCode.BadRequest);
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { err = "No es posible obtener un CardCode." }), Encoding.UTF8, "application/json");
                        return response;
                    }
                }
                else
                {
                    oCompanySaluti.GetLastError(out errorCode, out errorDesc);
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Content = new StringContent(JsonConvert.SerializeObject(new { err = errorDesc, code = errorCode }), Encoding.UTF8, "application/json");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(new { err = ex.Message }), Encoding.UTF8, "application/json");
                return response;
            }
        }
        private bool ValidarFormaPago(List<PagoModel> oList)
        {
            try
            {
                bool Bandera = false;
                foreach (PagoModel Item in oList)
                {
                    if (Item.TipoPago != 6 && (Item.CashSum > 0 || Item.CreditSum > 0 || Item.TrsfrSum > 0 || Item.CheckSum > 0))
                    {
                        Bandera = true;
                    }
                }
                return Bandera;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private enum EstadoEnum
        {
            Pendiente = 0,
            Enviado = 1,
            Completo = 2,
            Error = 3
        }

        private void FacturaEletronica(Documents oDoc, Company oCompany)
        {
            DocumentoItem data = new DocumentoItem();
            SAPDao oSDao = new SAPDao();
            string resul = "";
            try
            {
                data.Code = oDoc.DocEntry.ToString();
                data.Name = oDoc.DocNum.ToString();
                data.U_CreatedDate = DateTime.Now.ToShortDateString();
                data.U_DocNum = oDoc.DocNum;
                data.U_Respuesta = "";
                data.U_UploadDate = "";
                data.U_Estado = EstadoEnum.Pendiente.GetHashCode().ToString();
                string TipoDocumento = "";
                string Token = Utils.getToken();
                string Usuario = Utils.GetApyKey("usuario");
                string Password = Utils.GetApyKey("password");
                int IdEmpresa = int.Parse(Utils.GetApyKey("idEmpresa"));
                felCabezaDocumento client = new felCabezaDocumento();
                client.idEmpresa = IdEmpresa;
                client.idEmpresaSpecified = true;
                client.usuario = Usuario;
                client.contrasenia = Password;
                client.token = Token;
                client.version = "5";
                if (oDoc.DocObjectCode == BoObjectTypes.oInvoices)
                {
                    if (oDoc.DocumentSubType == BoDocumentSubType.bod_DebitMemo)
                    {
                        client.tipodocumento = "3";
                        client.tiponota = Convert.ToString(oDoc.UserFields.Fields.Item("U_MotivoND").Value);
                        client.tipoOperacion = Convert.ToString(oDoc.UserFields.Fields.Item("U_TipoOP").Value);
                        TipoDocumento = "ND";
                        if (client.tipoOperacion == "30")
                        {
                            felFacturaModificada oDocRef = new felFacturaModificada();
                            Documents oFac = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oInvoices);
                            oDoc.Lines.SetCurrentLine(0);
                            if (oDoc.Lines.BaseEntry > 0)
                            {
                                if (oDoc.Lines.BaseType == 13)
                                {
                                    oFac.GetByKey(oDoc.Lines.BaseEntry);
                                }
                                else
                                {
                                    oFac.GetByKey(oSDao.ObtenerDocEntryDocumentoReferenciado(oDoc.DocEntry, TipoDocumento));
                                }
                            }
                            else
                            {
                                oFac.GetByKey(oSDao.ObtenerDocEntryDocumentoReferenciado(oDoc.DocEntry, TipoDocumento));
                            }
                            oDocRef.tipoDocumentoFacturaModificada = "1";
                            oDocRef.prefijoFacturaModificada = oSDao.getPrefijo(oFac.Series);
                            oDocRef.consecutivoFacturaModificada = oFac.DocNum.ToString();
                            oDocRef.cufeFacturaModificada = (string)oFac.UserFields.Fields.Item("U_Cufe").Value;
                            client.listaFacturasModificadas = new felFacturaModificada[1];
                            client.listaFacturasModificadas[0] = oDocRef;
                        }
                    }
                    else
                    {
                        if (oDoc.UserFields.Fields.Item("U_Contingencia").Value == "1")
                        {
                            client.tipodocumento = "5";
                            felFacturaModificada oDocRef = new felFacturaModificada();
                            oDocRef.tipoDocumentoFacturaModificada = "5";
                            oDocRef.prefijoFacturaModificada = oSDao.getPrefijo(oDoc.Series);
                            oDocRef.consecutivoFacturaModificada = oDoc.DocNum.ToString();
                            oDocRef.fechaFacturaModificada = oDoc.DocDate;
                            oDocRef.fechaFacturaModificadaSpecified = true;
                            client.listaFacturasModificadas = new felFacturaModificada[1];
                            client.listaFacturasModificadas[0] = oDocRef;
                        }
                        else
                        {
                            client.tipodocumento = "1";
                        }
                        TipoDocumento = "INV";
                        client.tipoOperacion = "10";
                    }
                }
                else
                {
                    client.tipodocumento = "2";
                    client.tiponota = Convert.ToString(oDoc.UserFields.Fields.Item("U_MotivoNC").Value);
                    TipoDocumento = "NC";
                    client.tipoOperacion = Convert.ToString(oDoc.UserFields.Fields.Item("U_TipoOP").Value);
                    if (client.tipoOperacion == "20")
                    {
                        felFacturaModificada oDocRef = new felFacturaModificada();
                        Documents oFac = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oInvoices);
                        oDoc.Lines.SetCurrentLine(0);
                        if (oDoc.Lines.BaseEntry > 0)
                        {
                            if (oDoc.Lines.BaseType == 13)
                            {
                                oFac.GetByKey(oDoc.Lines.BaseEntry);
                            }
                            else
                            {
                                oFac.GetByKey(oSDao.ObtenerDocEntryDocumentoReferenciado(oDoc.DocEntry, TipoDocumento));
                            }
                        }
                        else
                        {
                            oFac.GetByKey(oSDao.ObtenerDocEntryDocumentoReferenciado(oDoc.DocEntry, TipoDocumento));
                        }
                        oDocRef.tipoDocumentoFacturaModificada = "1";
                        oDocRef.prefijoFacturaModificada = oSDao.getPrefijo(oFac.Series);
                        oDocRef.consecutivoFacturaModificada = oFac.DocNum.ToString();
                        oDocRef.cufeFacturaModificada = (string)oFac.UserFields.Fields.Item("U_Cufe").Value;
                        client.listaFacturasModificadas = new felFacturaModificada[1];
                        client.listaFacturasModificadas[0] = oDocRef;
                    }
                }
                data.U_Tipo = TipoDocumento;
                data.U_XmlData = Utils.UsingStream(client);
                resul = oSDao.SaveInvoice(data);
                client.prefijo = oSDao.getPrefijo(oDoc.Series);
                client.consecutivo = oDoc.DocNum;
                client.fechafacturacion = oDoc.DocDate;
                client.fechafacturacionSpecified = true;
                client.codigoPlantillaPdf = 1;
                client.idErp = null;
                int TotalLineas = oSDao.GetTotalLineas(oDoc.DocEntry, TipoDocumento);
                client.cantidadLineas = TotalLineas;
                client.incoterm = null;
                client.aplicafel = "SI";
                client.centroCostos = null;
                client.descripcionCentroCostos = null;
                client.codigovendedor = null;
                client.nombrevendedor = null;
                client.sucursal = null;
                client.tipoOperacion = "10";
                //Detalles del pago
                felPagos oPago = new felPagos();
                oPago.moneda = "COP";
                oPago.totalimportebruto = oSDao.GetTotalImporteBruto(oDoc.DocEntry, TipoDocumento);
                double prueba = oDoc.Lines.PriceAfterVAT + oDoc.BaseAmount;
                oPago.totalimportebrutoSpecified = true;
                oPago.totalbaseimponible = oSDao.GetTotalBaseImponible(oDoc.DocEntry, TipoDocumento);
                oPago.totalbaseimponibleSpecified = true;
                oPago.totalbaseconimpuestos = oDoc.BaseAmount + oDoc.VatSum;
                oPago.totalbaseconimpuestosSpecified = true;
                oPago.totalfactura = oDoc.BaseAmount + oDoc.VatSum;
                oPago.totalfacturaSpecified = true;
                if (oDoc.GroupNumber == -1)
                {
                    oPago.tipocompra = 1;
                }
                else
                {
                    oPago.tipocompra = 2;
                    oPago.periododepagoa = (oDoc.DocDueDate - oDoc.DocDate).Days;
                    oPago.periododepagoaSpecified = true;
                    oPago.fechavencimiento = oDoc.DocDueDate;
                    oPago.fechavencimientoSpecified = true;
                }
                oPago.codigoMonedaCambio = "COP";
                client.pago = oPago;
                //Medios de pagos
                client.listaMediosPagos = new felMedioPago[1];
                felMedioPago oMedP = new felMedioPago();
                oMedP.medioPago = "1";
                client.listaMediosPagos[0] = oMedP;
                //Detalles del documento
                int i = 0;
                int y = 0;
                client.listaDetalle = new felDetalleDocumento[TotalLineas];
                for (i = 0; i < oDoc.Lines.Count; i++)
                {
                    oDoc.Lines.SetCurrentLine(i);
                    if (oDoc.Lines != null && oDoc.Lines.Quantity > 0 && oDoc.Lines.TreeType != BoItemTreeTypes.iIngredient)
                    {
                        felDetalleDocumento detalle = new felDetalleDocumento();
                        Items item = (Items)oCompany.GetBusinessObject(BoObjectTypes.oItems);
                        item.GetByKey(oDoc.Lines.ItemCode);
                        detalle.codigoproducto = oDoc.Lines.ItemCode;
                        detalle.tipocodigoproducto = "999";
                        detalle.nombreProducto = oDoc.Lines.ItemDescription;
                        detalle.cantidad = oDoc.Lines.Quantity;
                        detalle.cantidadSpecified = true;
                        detalle.unidadmedida = (string)item.UserFields.Fields.Item("U_UNIDADMEDIDA").Value;
                        if (oDoc.Lines.ItemCode == "91339")
                        {
                            detalle.valorunitario = 0.001;
                            detalle.preciosinimpuestos = oDoc.Lines.Quantity * 0.001;
                            detalle.preciototal = (0.001 * oDoc.Lines.Quantity) + oDoc.Lines.TaxTotal;
                        }
                        else
                        {
                            detalle.valorunitario = oDoc.Lines.UnitPrice;
                            detalle.preciosinimpuestos = oDoc.Lines.LineTotal;
                            detalle.preciototal = oDoc.Lines.LineTotal + oDoc.Lines.TaxTotal;
                        }
                        detalle.posicion = y;
                        if (oDoc.Lines.ItemCode == "91339")
                        {
                            //Lista de impuestos
                            detalle.tipoImpuesto = 1;
                            detalle.listaImpuestos = new felImpuesto[1];
                            detalle.listaImpuestos[0] = new felImpuesto()
                            {
                                codigoImpuestoRetencion = "22",
                                porcentaje = 0,
                                porcentajeSpecified = true,
                                valorImpuestoRetencion = oDoc.Lines.Quantity * Utils.ObtenerImpuestoBolsa(),
                                valorImpuestoRetencionSpecified = true,
                                baseimponible = Utils.ObtenerImpuestoBolsa(),
                                baseimponibleSpecified = true,
                                isAutoRetenido = false,
                                isAutoRetenidoSpecified = true
                            };
                        }
                        else
                        {
                            if (oDoc.Lines.TaxPercentagePerRow > 0)
                            {
                                //Lista de impuestos
                                detalle.tipoImpuesto = 1;
                                detalle.listaImpuestos = new felImpuesto[1];
                                detalle.listaImpuestos[0] = new felImpuesto()
                                {
                                    codigoImpuestoRetencion = "01",
                                    porcentaje = oDoc.Lines.TaxPercentagePerRow,
                                    porcentajeSpecified = true,
                                    valorImpuestoRetencion = oDoc.Lines.TaxTotal,
                                    valorImpuestoRetencionSpecified = true,
                                    baseimponible = oDoc.Lines.LineTotal,
                                    baseimponibleSpecified = true,
                                    isAutoRetenido = false,
                                    isAutoRetenidoSpecified = true
                                };
                            }
                            else
                            {
                                foreach (string IVA in Utils.getTipoIVAExcluido().Split(';'))
                                {
                                    if (oDoc.Lines.TaxCode == IVA)
                                    {
                                        detalle.tipoImpuesto = 2;
                                    }
                                }
                                foreach (string IVA in Utils.getTipoIVAExcento().Split(';'))
                                {
                                    if (oDoc.Lines.TaxCode == IVA)
                                    {
                                        detalle.tipoImpuesto = 3;
                                    }
                                }
                                if (detalle.tipoImpuesto != 2)
                                {
                                    detalle.listaImpuestos = new felImpuesto[1];
                                    detalle.listaImpuestos[0] = new felImpuesto()
                                    {
                                        codigoImpuestoRetencion = "01",
                                        porcentaje = oDoc.Lines.TaxPercentagePerRow,
                                        porcentajeSpecified = true,
                                        valorImpuestoRetencion = oDoc.Lines.TaxTotal,
                                        valorImpuestoRetencionSpecified = true,
                                        baseimponible = oDoc.Lines.LineTotal,
                                        baseimponibleSpecified = true,
                                        isAutoRetenido = false,
                                        isAutoRetenidoSpecified = true
                                    };
                                }
                            }
                        }
                        //Lista de descuentos
                        if (oDoc.Lines.DiscountPercent > 0)
                        {
                            detalle.listaDescuentos = new felDescuento[1];
                            detalle.listaDescuentos[0] = new felDescuento()
                            {
                                codigoDescuento = "11",
                                descuento = Math.Round(((oDoc.Lines.UnitPrice * oDoc.Lines.Quantity) * oDoc.Lines.DiscountPercent) / 100),
                                descuentoSpecified = true,
                                porcentajeDescuento = oDoc.Lines.DiscountPercent,
                                porcentajeDescuentoSpecified = true
                            };
                        }
                        detalle.aplicaMandato = "NO";
                        detalle.listaCamposAdicionales = new felCampoAdicional[1];
                        detalle.listaCamposAdicionales[0] = new felCampoAdicional()
                        {
                            nombreCampo = "UnidadMedida",
                            valorCampo = oSDao.ObtenerNombreUnidadMedida(item.UserFields.Fields.Item("U_UNIDADMEDIDA").Value),
                            seccion = 1,
                            orden = 2,
                            fecha = DateTime.Now
                        };
                        client.listaDetalle[y] = detalle;
                        y++;
                    }
                }
                //Lista de impuestos al documento
                List<ImpuestoItem> Impuestos = oSDao.GetImpuestosGlobales(oDoc.DocEntry, TipoDocumento);
                i = 0;
                if (Impuestos.Count > 0)
                {
                    client.listaImpuestos = new felImpuesto[Impuestos.Count];
                    foreach (ImpuestoItem Item in Impuestos)
                    {
                        client.listaImpuestos[i] = new felImpuesto()
                        {
                            codigoImpuestoRetencion = Item.Codigo,
                            porcentaje = Item.Porcentaje,
                            porcentajeSpecified = true,
                            valorImpuestoRetencion = Item.ValorImpuestoRetencion,
                            valorImpuestoRetencionSpecified = true,
                            baseimponible = Item.BaseImponible,
                            baseimponibleSpecified = true,
                            isAutoRetenido = Item.IsAutoRetencion,
                            isAutoRetenidoSpecified = true
                        };
                        i = i + 1;
                    }
                }
                //Detalles del cliente
                client.listaAdquirentes = new felAdquirente[1];
                felAdquirente oAdq = new felAdquirente();
                BusinessPartners oBPClient = (BusinessPartners)oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                oBPClient.GetByKey(oDoc.CardCode);
                oAdq.tipoPersona = getPersona(oBPClient.UserFields.Fields.Item(Utils.GetApyKey("campopersona")).Value);
                oAdq.nombreCompleto = oBPClient.CardName;
                oAdq.tipoIdentificacion = int.Parse((string)oBPClient.UserFields.Fields.Item(Utils.GetApyKey("campotdocumento")).Value);
                oAdq.numeroIdentificacion = Utils.getLicTradNum(oBPClient.FederalTaxID);
                if (oAdq.tipoIdentificacion == 31)
                {
                    oAdq.digitoverificacion = oBPClient.FederalTaxID.Split('-')[1];
                    oAdq.tipoobligacion = null;
                }
                oAdq.regimen = (string)oBPClient.UserFields.Fields.Item(Utils.GetApyKey("camporegimen")).Value;
                string ResFis = string.Empty;
                if (!string.IsNullOrEmpty((string)oBPClient.UserFields.Fields.Item("U_HBT_ResFis").Value))
                {
                    ResFis = (string)oBPClient.UserFields.Fields.Item("U_HBT_ResFis").Value;
                    if (!string.IsNullOrEmpty((string)oBPClient.UserFields.Fields.Item("U_HBT_ResFis1").Value))
                    {
                        ResFis = string.Format("{0};{1}", ResFis, oBPClient.UserFields.Fields.Item("U_HBT_ResFis1").Value);
                        if (!string.IsNullOrEmpty((string)oBPClient.UserFields.Fields.Item("U_HBT_ResFis2").Value))
                        {
                            ResFis = string.Format("{0};{1}", ResFis, oBPClient.UserFields.Fields.Item("U_HBT_ResFis2").Value);
                            if (!string.IsNullOrEmpty((string)oBPClient.UserFields.Fields.Item("U_HBT_ResFis3").Value))
                            {
                                ResFis = string.Format("{0};{1}", ResFis, oBPClient.UserFields.Fields.Item("U_HBT_ResFis3").Value);
                            }
                        }
                    }
                }
                oAdq.tipoobligacion = ResFis;
                if (!string.IsNullOrEmpty(oBPClient.EmailAddress))
                {
                    oAdq.email = oBPClient.EmailAddress;
                    oAdq.envioPorEmailPlataforma = "Email";
                }
                else
                {
                    client.aplicafel = "NO";
                    oAdq.envioPorEmailPlataforma = null;
                }
                oAdq.pais = "CO";
                oAdq.paisnombre = "Colombia";
                oAdq.departamento = FormateState(oDoc.AddressExtension.BillToCounty);
                oAdq.ciudad = (string)oBPClient.UserFields.Fields.Item("U_HBT_MunMed").Value;
                oAdq.direccion = oDoc.Address.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                oAdq.telefono = oBPClient.Phone1;
                client.listaAdquirentes[0] = oAdq;
                //Anticipos
                //client.listaAnticipo = new felAnticipo[1];
                //felAnticipo oAnt = new felAnticipo();
                //oAnt.anticipo = 0;
                //oAnt.anticipoSpecified = true;
                //oAnt.descripcion = null;
                //oAnt.fechaAnticipo = DateTime.Now;
                //oAnt.fechaAnticipoSpecified = true;
                //client.listaAnticipo[0] = oAnt;
                client.listaAnticipo = null;
                //Campos adicionales
                client.listaCamposAdicionales = new felCampoAdicional[9];
                if (TipoDocumento != "INV")
                {
                    client.listaCamposAdicionales[0] = new felCampoAdicional()
                    {
                        nombreCampo = "Resolucion",
                        valorCampo = "* Bienes exentos Decreto 417 del 17 de marzo de 2020",
                        seccion = 1,
                        orden = 1,
                        fecha = DateTime.Now
                    };
                }
                else
                {
                    client.listaCamposAdicionales[0] = new felCampoAdicional()
                    {
                        nombreCampo = "Resolucion",
                        valorCampo = string.Format("{0};* Bienes exentos Decreto 417 del 17 de marzo de 2020", oSDao.ObtenerResolucionDocumento(oDoc.Series, TipoDocumento)),
                        seccion = 1,
                        orden = 1,
                        fecha = DateTime.Now
                    };
                }
                client.listaCamposAdicionales[1] = new felCampoAdicional()
                {
                    nombreCampo = "Comments",
                    valorCampo = oDoc.Comments.Replace("\r\n", "").Replace("\n", "").Replace("\r", ""),
                    seccion = 1,
                    orden = 1,
                    fecha = DateTime.Now
                };
                client.listaCamposAdicionales[2] = new felCampoAdicional()
                {
                    nombreCampo = "VALOR EN LETRAS",
                    valorCampo = string.Format("{0} Pesos Colombianos.", oSDao.EnLetras(oDoc.DocTotal.ToString())),
                    seccion = 1,
                    orden = 1,
                    fecha = DateTime.Now
                };
                client.listaCamposAdicionales[3] = new felCampoAdicional()
                {
                    nombreCampo = "MedioPago",
                    valorCampo = string.Format("MedioPago: {0}", oSDao.ObtenerFormaPago(oDoc.GroupNumber)),
                    seccion = 1,
                    orden = 2,
                    fecha = DateTime.Now
                };
                client.listaCamposAdicionales[4] = new felCampoAdicional()
                {
                    nombreCampo = "PedidoEcommerce",
                    valorCampo = string.Format("PedidoEcommerce: {0}", oDoc.UserFields.Fields.Item("U_NumEcommerce").Value),
                    seccion = 1,
                    orden = 2,
                    fecha = DateTime.Now
                };
                client.listaCamposAdicionales[5] = new felCampoAdicional()
                {
                    nombreCampo = "PedidoSAP",
                    valorCampo = string.Format("PedidoSAP: {0}", oSDao.ObtenerNumeroPedido(oDoc.DocEntry)),
                    seccion = 1,
                    orden = 2,
                    fecha = DateTime.Now
                };
                string TotalDescuento = oSDao.ObtenerTotalDescuento(oDoc.DocEntry, TipoDocumento);
                client.listaCamposAdicionales[6] = new felCampoAdicional()
                {
                    nombreCampo = "TotalDescuento",
                    valorCampo = TotalDescuento,
                    seccion = 1,
                    orden = 2,
                    fecha = DateTime.Now
                };
                string ObtenerSubtotal = oSDao.ObtenerSubtotal(oDoc.DocEntry, TipoDocumento).ToString().Replace(",", ".");
                client.listaCamposAdicionales[7] = new felCampoAdicional()
                {
                    nombreCampo = "Subtotal",
                    valorCampo = ObtenerSubtotal,
                    seccion = 1,
                    orden = 2,
                    fecha = DateTime.Now
                };
                client.listaCamposAdicionales[8] = new felCampoAdicional()
                {
                    nombreCampo = "ImporteNeto",
                    valorCampo = (double.Parse(ObtenerSubtotal) - double.Parse(TotalDescuento)).ToString(),
                    seccion = 1,
                    orden = 2,
                    fecha = DateTime.Now
                };
                data.U_XmlData = Utils.UsingStream(client);
                resul = oSDao.SaveInvoice(data);

                if (string.IsNullOrEmpty(resul))
                {
                    WsEnviarDocumentoClient clientsend = new WsEnviarDocumentoClient();
                    using (new OperationContextScope(clientsend.InnerChannel))
                    {
                        felRespuestaEnvio response = clientsend.enviarDocumento(client);
                        data = oSDao.getInvoice(oDoc.DocEntry.ToString(), TipoDocumento);
                        if (!string.IsNullOrEmpty(response.cufe) || (response.estadoProceso == 1 && client.tipodocumento != "1"))
                        {
                            data.U_Estado = EstadoEnum.Completo.GetHashCode().ToString();
                            if (client.tipodocumento != "2")//NC
                            {
                                oDoc.UserFields.Fields.Item(Utils.GetApyKey("cufefield")).Value = response.cufe;
                                oDoc.UserFields.Fields.Item(Utils.GetApyKey("QRfield")).Value = response.codigoQr;
                                oDoc.Update();
                            }
                        }
                        else
                        {
                            data.U_Estado = EstadoEnum.Error.GetHashCode().ToString();
                        }
                        string Errores = response.descripcionProceso;
                        if (response.listaMensajesProceso != null)
                        {
                            bool cont = false;
                            foreach (felMensajesProceso Error in response.listaMensajesProceso)
                            {
                                if (Error.codigoMensaje != "90")
                                {
                                    cont = true;
                                    Errores = string.Format("{0} - {1}", Errores, Error.descripcionMensaje);
                                }
                            }
                            if (cont)
                            {
                                data.U_Estado = EstadoEnum.Error.GetHashCode().ToString();
                            }
                        }
                        data.U_Respuesta = Errores;
                        oSDao.SaveInvoice(data);
                    }
                    clientsend.Close();
                }
            }
            catch (Exception ex)
            {
                resul = ex.Message;
                data.U_Estado = EstadoEnum.Error.GetHashCode().ToString();
                data.U_Respuesta = resul;
                data.U_XmlData = ex.Message;
                oSDao.SaveInvoice(data);
            }
        }

        private string getPersona(object value)
        {
            throw new NotImplementedException();
        }

        private static string FormateState(string state)
        {
            var result = state;
            if (state == "100")
            {
                result = "76";
            }
            else
            {
                if (int.TryParse(state, out int dpto))
                {
                    result = dpto < 10 ? $"0{dpto}" : dpto.ToString();
                }
            }
            return result;
        }
        private static string getPersona(string value)
        {
            string result = "1";
            if (value == "01")
            {
                result = "2";
            }
            return result;
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
                else if (ConfigurationManager.AppSettings["BDServerSAP"] == "2017")
                    oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2017;
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
    }
}