using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using WebServiceAllers.Positive.Dao;
using WebServiceAllers.Positive.Item;

namespace WebServiceAllers.Positive
{
    public class PositiveBiz
    {
        public string cadenaConexion = ConfigurationManager.ConnectionStrings["CadenaConexionPos"].ConnectionString;
        public string Guardar(tblDocumentoItem oDocItem, List<tblDetalleDocumentoItem> oListDet, tblPagoItem oPagoI, List<tblTipoPagoItem> oTipPagLis)
        {
            try
            {
                tblDocumentoDao oDocD = new tblDocumentoDao(cadenaConexion);
                return oDocD.GuardarTodo(oDocItem, oListDet, oPagoI, oTipPagLis);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> ObtenerPedidoPorReferencia(string Referencia) {
            tblDocumentoDao oDocD = new tblDocumentoDao(cadenaConexion);
            return oDocD.ObtenerPedidoPorReferencia(Referencia);
        }

        public tblDocumentoItem ObtenerDocumento(long Id, long tipoDocumento) {
            tblDocumentoDao oDocD = new tblDocumentoDao(cadenaConexion);
            return oDocD.ObtenerDocumento(Id, tipoDocumento);
        }

        public tblTerceroItem ObtenerTerceroPorCardCode(string CardCode, long idEmpresa) {
            tblTerceroDao oTDao = new tblTerceroDao(cadenaConexion);
            return oTDao.ObtenerTerceroPorCardCode(CardCode, idEmpresa);
        }

        public void ActualizarTerceroCardCode(long idEmpresa) {
            tblTerceroDao oTDao = new tblTerceroDao(cadenaConexion);
            oTDao.ActualizarTerceroCardCode(idEmpresa);
        }
        public bool Guardar(tblTerceroItem Item) {
            tblTerceroDao oTDao = new tblTerceroDao(cadenaConexion);
            return oTDao.Guardar(Item);
        }
        public tblCiudadItem ObtenerCiudadPorCodigo(string Codigo) {
            tblCiudadDao oCDao = new tblCiudadDao(cadenaConexion);
            return oCDao.ObtenerCiudadPorCodigo(Codigo);
        }
        public tblArticuloItem ObtenerArticuloPorCodigo(string Codigo) {
            tblArticuloDao oADao = new tblArticuloDao(cadenaConexion);
            return oADao.ObtenerArticuloPorCodigo(Codigo);
        }
        public tblBodegaItem ObtenerBodegaPorWhsCode(string WhsCode) {
            tblBodegaDao oBDao = new tblBodegaDao(cadenaConexion);
            return oBDao.ObtenerBodegaPorWhsCode(WhsCode);
        }

        #region Documento Campo Valor
        public tblDocumentoCampoValorItem ObtenerDocumentoCampoValor(short idTipoDocumento, long idDocumento, string Nombre)
        {
            tblDocumentoCampoValorDao oDCVDao = new tblDocumentoCampoValorDao(cadenaConexion);
            return oDCVDao.ObtenerDocumentoCampoValor(idTipoDocumento, idDocumento, Nombre);
        }

        public List<tblDocumentoCampoValorItem> ObtenerDocumentoCampoValorLista(short idTipoDocumento, long idDocumento)
        {
            tblDocumentoCampoValorDao oDCVDao = new tblDocumentoCampoValorDao(cadenaConexion);
            return oDCVDao.ObtenerDocumentoCampoValorLista(idTipoDocumento, idDocumento);
        }
        public bool Guardar(tblDocumentoCampoValorItem Item) {
            tblDocumentoCampoValorDao oDCVDao = new tblDocumentoCampoValorDao(cadenaConexion);
            return oDCVDao.Guardar(Item);
        }

        public bool Guardar(tblPedidoComPedidosItem Item)
        {
            tblDocumentoDao oDDao = new tblDocumentoDao(cadenaConexion);
            return oDDao.Guardar(Item);
        }

        public tblPedidoComPedidosItem ObtenerPedidoComPedido(int idPedidoCom, string WhsCode, bool PedidoInterno)
        {
            tblDocumentoDao oDDao = new tblDocumentoDao(cadenaConexion);
            return oDDao.ObtenerPedidoComPedido(idPedidoCom, WhsCode, PedidoInterno);
        }
        public List<tblPedidoComPedidosItem> ObtenerPedidoComPedidos(int idPedidoCom)
        {
            tblDocumentoDao oDDao = new tblDocumentoDao(cadenaConexion);
            return oDDao.ObtenerPedidoComPedidos(idPedidoCom);
        }

        public bool ExistePedidoCom(string IdPedido) {
            tblDocumentoDao oDDao = new tblDocumentoDao(cadenaConexion);
            bool result = true;
            var errores = oDDao.ExistePedidoCom(IdPedido);
            if (errores.Count == 0)
            {
                result = false;
            }
            else {
                foreach (var e in errores) {
                    if (!string.IsNullOrEmpty(e)) {
                        result = false;
                    }
                }
            }
            return result;
        }

        public long getPedidoComID(string IdPedido) {
            tblDocumentoDao oDDao = new tblDocumentoDao(cadenaConexion);
            return oDDao.getPedidoComID(IdPedido);
        }
        #endregion
        #region Sede
        public tblSedeItem ObtenerSedeCardCode(string CardCode)
        {
            tblSedeDao oSDao = new tblSedeDao(cadenaConexion);
            return oSDao.ObtenerSedeCardCode(CardCode);
        }

        public bool Guardar(tblDocumentoJSONItem Item)
        {
            tblDocumentoJSONDao oDDao = new tblDocumentoJSONDao(cadenaConexion);
            return oDDao.Guardar(Item);
        }
        #endregion
    }
}