using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceAllers.Positive
{
    public class tblTerceroItem
    {

        public tblTerceroItem()
        {
            IdTercero = 0;
            oListDirecciones = new List<tblTerceroDireccion>();
            oListCelulares = new List<tblTerceroCelular>();
            oListCorreo = new List<tblTerceroCorreo>();
        }

        public long IdTercero { get; set; }
        public short idTipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Mail { get; set; }
        public string Direccion { get; set; }
        public short idCiudad { get; set; }
        public string Ciudad { get; set; }
        public string Departamento { get; set; }
        public string idDepartamento { get; set; }
        public long idEmpresa { get; set; }
        public string Empresa { get; set; }
        public string TipoTercero { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public long IdListaPrecio { get; set; }
        public long idGrupoCliente { get; set; }
        public string GrupoCliente { get; set; }
        public bool Generico { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Observaciones { get; set; }
        public bool Activo { get; set; }
        public bool ProteccionDatos { get; set; }
        public long IdUsuario { get; set; }
        public int FrecuenciaCompra { get; set; }
        public short TipoPersona { get; set; }
        public string EstablecimientoComercial { get; set; }
        public int IdBarrio { get; set; }
        public string Barrio { get; set; }
        public long IdUsuarioModificacion { get; set; }
        public List<tblTerceroDireccion> oListDirecciones { get; set; }
        public List<tblTerceroCelular> oListCelulares { get; set; }
        public List<tblTerceroCorreo> oListCorreo { get; set; }
        public int InformacionComercial { get; set; }
        public int TipoFactura { get; set; }

        public string CardCode { get; set; }
        public string U_BPCO_RTC { get; set; } /*Regimen Tributario <Listas de opciones>*/
        public string U_BPCO_TDC { get; set; } /*Tipo Documento <Listas de opciones>*/
        public string U_HBT_Nombres { get; set; } /*Nombres*/
        public string U_HBT_Apellido1 { get; set; } /*Primer apellido*/
        public string U_HBT_Apellido2 { get; set; } /*Segundo apellido*/
        public string U_HBT_RegTrib { get; set; } /*Regimen tributario<Listas de opciones>*/
        public string U_HBT_TipDoc { get; set; } /*Tipo Documento<Listas de opciones>.*/
        public string U_HBT_ActEco { get; set; } /*Actividad economica<Listas de opciones>*/
        public string U_HBT_MunMed { get; set; } /*Municipio<Listas de opciones>*/
        public string U_HBT_TipEnt { get; set; } /*Tipo Entidad <Listas de opciones>*/
        public string U_HBT_TipExt { get; set; } /*Tipo Extranjero<Listas de opciones>.*/
        public string U_HBT_RegFis { get; set; } /*Regimen Fiscal<Listas de opciones>.*/
        public string U_HBT_InfoTrib { get; set; } /*Información Tributaria<Listas de opciones>*/
        public string U_HBT_ResFis { get; set; } /* Responsabilidad Fiscal<Listas de opciones>*/
    }
}