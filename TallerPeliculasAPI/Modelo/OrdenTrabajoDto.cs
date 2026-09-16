using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TallerPeliculasAPI.Modelo
{
    public class OrdenTrabajoDto
    {
        public string Idorden { get; set; }
        public string IdCliente { get; set; }  
        public string NombreCliente { get; set; }
        public string IdEncargado { get; set; }
        public string NombreEncargado { get; set; }
        public string Sucursal { get; set; }
        public string NotaVenta { get; set; }
        public string FechaIngreso { get; set; }
        public string HoraIngreso { get; set; }
        public string HoraEntrega { get; set; }
        public string Bodega { get; set; }
        public string IdVendedor { get; set; }
        public string NombreVendedor { get; set; }
        public string Estado { get; set; }
        public string EstadoOTTexto { get; set; }
        public string UsuarioModificaOT { get; set; }
        public string IngresoOrdenCompra { get; set; }
        public string ReferenciasDTE { get; set; }
        public string FechaRealEntregaOT { get; set; }
        public string HoraTerminoOT { get; set; }
        public string FechaEntregaCotizacionApprox { get; set; }
        public string Observaciones { get; set; }
        public string UsuarioCreaOT { get; set; }
        public string AbonadoOT { get; set; }
        public string CotizacionAprobada { get; set; }
        public string SubTotal { get; set; }
        public string DescuentoPorcentaje { get; set; }
        public string DescuentoMonto { get; set; }
        public string TotalNeto { get; set; }
        public string TotalIVA { get; set; }
        public string TotalOT { get; set; }
       
    }

    public class OrdenTrabajoCompletaDto
    {
        public OrdenTrabajoDto Cabecera { get; set; }
        public List<OrdenTrabajoDetalleDto> Detalles { get; set; } = new List<OrdenTrabajoDetalleDto>();
        public List<OrdenTrabajoImagenDto> Imagenes { get; set; } = new List<OrdenTrabajoImagenDto>();
    }

    public class OrdenTrabajoDetalleDto
    {
        public string IdDetalle { get; set; }
        public string Idorden { get; set; }
        public string IdTipo { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Cantidad { get; set; }
        public string ValorUnitario { get; set; }
        public string DescuentoPorcentaje { get; set; }
        public string SubTotal { get; set; }


       
    }

    public class OrdenTrabajoImagenDto
    {
        public string IdImagen { get; set; }
        public string Idorden { get; set; }
        public string RutaImagen { get; set; }
    }
}