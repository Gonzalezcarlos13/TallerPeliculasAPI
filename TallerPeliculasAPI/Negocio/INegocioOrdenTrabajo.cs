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
        Task<bool> CrearOrdenTrabajoAsync(OrdenTrabajoDto orden);
        Task<bool> ActualizarOrdenTrabajoAsync(OrdenTrabajoDto orden);
        Task<bool> EliminarDetalleOTAsync(int idDetalle);
        Task<bool> CrearOrdenTrabajoAsync(object orden);
    }
    public class OrdenTrabajoNegocio : IOrdenTrabajoNegocio
    {
        private readonly IOrdenTrabajoRepository _daoOrdenTrabajo;

        public OrdenTrabajoNegocio()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MiConexionSqlServer"].ConnectionString;
            _daoOrdenTrabajo = new OrdenTrabajoRepository();
        }

        public async Task<bool> CrearOrdenTrabajoAsync(OrdenTrabajoDto orden)
        {
 
            return await _daoOrdenTrabajo.CrearOrdenTrabajoAsync(orden);
        }

        public async Task<bool> ActualizarOrdenTrabajoAsync(OrdenTrabajoDto orden)
        {
            return await _daoOrdenTrabajo.ActualizarOrdenTrabajoAsync(orden);
        }
        public async Task<bool> EliminarDetalleOTAsync(int idDetalle)
        {
            if (idDetalle <= 0)
            {
                throw new ArgumentException("El identificador del detalle no es válido.");
            }

            return await _daoOrdenTrabajo.EliminarDetalleOTAsync(idDetalle);
        }

        public Task<bool> CrearOrdenTrabajoAsync(object orden)
        {
            throw new NotImplementedException();
        }
    }
}
