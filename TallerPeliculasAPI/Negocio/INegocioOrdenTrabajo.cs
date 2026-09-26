using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TallerPeliculasAPI.Modelo;
using TallerPeliculasAPI.Repositorio;

namespace TallerPeliculasAPI.Negocio
{
    public interface IOrdenTrabajoNegocio
    {
        Task<int> CrearOrdenTrabajoAsync(OrdenTrabajoDto orden);
        Task<bool> ActualizarOrdenTrabajoAsync(OrdenTrabajoDto orden);
        Task<bool> EliminarDetalleOTAsync(int idDetalle);
        Task<bool> CrearDetalleOrdenTrabajoAsync(OrdenTrabajoDetalleDto item);
        Task<bool> CrearImagenesOrdenTrabajoAsync(OrdenTrabajoImagenDto item);
        Task<bool> ActualizarImagenOrdenTrabajoAsync(int idImagen, string rutaImagen);
        Task<List<OrdenTrabajo>> LeerOrdenTrabajo(int idOrden);

        // ==========================================================
        // ABONO ORDEN DE TRABAJO
        // ==========================================================
        Task<decimal> LeerAbonoOrdenTrabajoAsync(int idOrden);
        Task<bool> ActualizarAbonoOrdenTrabajoAsync(
            int idOrden,
            decimal abonadoOT
        );
        Task<bool> EliminarImagenOrdenTrabajoAsync(int idImagen);
    }

    public class OrdenTrabajoNegocio : IOrdenTrabajoNegocio
    {
        private readonly IOrdenTrabajoRepository _daoOrdenTrabajo;

        public OrdenTrabajoNegocio()
        {
            string connectionString =
                ConfigurationManager
                    .ConnectionStrings["MiConexionSqlServer"]
                    .ConnectionString;

            _daoOrdenTrabajo = new OrdenTrabajoRepository();
        }

        public async Task<int> CrearOrdenTrabajoAsync(
            OrdenTrabajoDto orden
        )
        {
            return await _daoOrdenTrabajo
                .CrearOrdenTrabajoAsync(orden);
        }

        public async Task<bool> ActualizarOrdenTrabajoAsync(
            OrdenTrabajoDto orden
        )
        {
            return await _daoOrdenTrabajo
                .ActualizarOrdenTrabajoAsync(orden);
        }

        public async Task<bool> EliminarDetalleOTAsync(
            int idDetalle
        )
        {
            if (idDetalle <= 0)
            {
                throw new ArgumentException(
                    "El identificador del detalle no es válido."
                );
            }

            return await _daoOrdenTrabajo
                .EliminarDetalleOTAsync(idDetalle);
        }

        public async Task<bool> CrearDetalleOrdenTrabajoAsync(
            OrdenTrabajoDetalleDto detalle
        )
        {
            return await _daoOrdenTrabajo
                .CrearDetalleOrdenTrabajoAsync(detalle);
        }

        public async Task<bool> CrearImagenesOrdenTrabajoAsync(
            OrdenTrabajoImagenDto imagen
        )
        {
            return await _daoOrdenTrabajo
                .CrearImagenesOrdenTrabajoAsync(imagen);
        }

        public async Task<List<OrdenTrabajo>> LeerOrdenTrabajo(
            int idOrden
        )
        {
            return await _daoOrdenTrabajo
                .LeerOrdenTrabajo(idOrden);
        }

        public async Task<decimal> LeerAbonoOrdenTrabajoAsync(
            int idOrden
        )
        {
            if (idOrden <= 0)
            {
                throw new ArgumentException(
                    "El IdOrden no es válido."
                );
            }

            return await _daoOrdenTrabajo
                .LeerAbonoOrdenTrabajoAsync(idOrden);
        }

        public async Task<bool> ActualizarAbonoOrdenTrabajoAsync(
            int idOrden,
            decimal abonadoOT
        )
        {
            if (idOrden <= 0)
            {
                throw new ArgumentException(
                    "El IdOrden no es válido."
                );
            }

            // Permitimos 0 para poder limpiar/corregir el abono.
            if (abonadoOT < 0)
            {
                throw new ArgumentException(
                    "El monto abonado no puede ser negativo."
                );
            }

            return await _daoOrdenTrabajo
                .ActualizarAbonoOrdenTrabajoAsync(
                    idOrden,
                    abonadoOT
                );
        }

        public async Task<bool> ActualizarImagenOrdenTrabajoAsync(
            int idImagen,
            string rutaImagen
        )
        {
            if (idImagen <= 0)
            {
                throw new ArgumentException(
                    "El IdImagen no es válido."
                );
            }

            if (string.IsNullOrWhiteSpace(rutaImagen))
            {
                throw new ArgumentException(
                    "La imagen no contiene información para actualizar."
                );
            }

            return await _daoOrdenTrabajo
                .ActualizarImagenOrdenTrabajoAsync(
                    idImagen,
                    rutaImagen
                );
        }


        public async Task<bool> EliminarImagenOrdenTrabajoAsync(
            int idImagen
        )
        {
            if (idImagen <= 0)
            {
                throw new ArgumentException(
                    "El IdImagen no es válido."
                );
            }

            return await _daoOrdenTrabajo
                .EliminarImagenOrdenTrabajoAsync(idImagen);
        }
    }
}
