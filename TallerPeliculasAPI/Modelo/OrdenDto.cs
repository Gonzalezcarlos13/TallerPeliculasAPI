using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TallerPeliculasAPI.Modelo
{
    public class OrdenTrabajo
    {
        public int Id { get; set; }
        public int Idorden { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public int IdEncargado { get; set; }
        public string NombreEncargado { get; set; } = string.Empty;
        public string  Sucursal { get; set; }
        public string  NotaVenta { get; set; }
        public string FechaIngreso { get; set; }
        public string HoraIngreso { get; set; }
        public string HoraEntrega { get; set; }
        public string Bodega { get; set; } = string.Empty;
        public int IdVendedor { get; set; }
        public string NombreVendedor { get; set; } = string.Empty;
        public int Estado { get; set; }
        public string EstadoOTTexto { get; set; } = string.Empty;
        public string  UsuarioModificaOT { get; set; }
        public string  IngresoOrdenCompra { get; set; }
        public string  ReferenciasDTE { get; set; }
        public string FechaRealEntregaOT { get; set; }
        public string  HoraTerminoOT { get; set; }
        public string FechaEntregaCotizacionApprox { get; set; }
        public string  Observaciones { get; set; }
        public string UsuarioCreaOT { get; set; } = string.Empty;
        public decimal AbonadoOT { get; set; }
        public int CotizacionAprobada { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DescuentoPorcentaje { get; set; }
        public decimal DescuentoMonto { get; set; }
        public decimal TotalNeto { get; set; }
        public decimal TotalIVA { get; set; }
        public decimal TotalOT { get; set; }

        // Propiedades de navegación para cuando se consulte por ID
        public List<OrdenTrabajoDetalle> Detalles { get; set; } = new List<OrdenTrabajoDetalle>();
        public List<OrdenTrabajoImagenes> Imagenes { get; set; } = new List<OrdenTrabajoImagenes>();
    }

    public class OrdenTrabajoDetalle
    {
        public int IdDetalle { get; set; }
        public int Idorden { get; set; }
        public string CodigoProducto { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal ValorNeto { get; set; }
        public decimal DescuentoPorcentaje { get; set; }
        public decimal TotalNeto { get; set; }
        public decimal ComisionPorcentaje { get; set; }
        public decimal TotalComision { get; set; }
        public bool SinRebajaDeStock { get; set; }
        public int IdTipo { get; set; }
    }

    public class OrdenTrabajoImagenes
    {
        public int IdImagen { get; set; }
        public int Idorden { get; set; }
        public string RutaImagen { get; set; } = string.Empty;
    }
}