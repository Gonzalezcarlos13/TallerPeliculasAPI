using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TallerPeliculasAPI.Modelo;

namespace TallerPeliculasAPI.Repositorio
{
    public interface IOrdenTrabajoRepository
    {
        Task<bool> CrearOrdenTrabajoAsync(OrdenTrabajoDto orden);
        Task<bool> ActualizarOrdenTrabajoAsync(OrdenTrabajoDto orden);
        Task<bool> EliminarDetalleOTAsync(int idDetalle);
    }

    public class OrdenTrabajoRepository : IOrdenTrabajoRepository
    {
        private readonly string _connectionString;

        public OrdenTrabajoRepository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MiConexionSqlServer"].ConnectionString;
            _connectionString = connectionString;
        }

        public async Task<bool> CrearOrdenTrabajoAsync(OrdenTrabajoDto orden)
        {
            bool result = false;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("dbo.sp_CrearOrdenTrabajo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@Idorden", SqlDbType.Int).Value = orden.Idorden;
                        command.Parameters.Add("@IdCliente", SqlDbType.Int).Value = orden.IdCliente;
                        command.Parameters.Add("@NombreCliente", SqlDbType.VarChar, 150).Value = orden.NombreCliente ?? (object)DBNull.Value;
                        command.Parameters.Add("@IdEncargado", SqlDbType.Int).Value = orden.IdEncargado;
                        command.Parameters.Add("@NombreEncargado", SqlDbType.VarChar, 100).Value = orden.NombreEncargado ?? (object)DBNull.Value;
                        command.Parameters.Add("@Sucursal", SqlDbType.VarChar, 100).Value = orden.Sucursal ?? (object)DBNull.Value;
                        command.Parameters.Add("@NotaVenta", SqlDbType.VarChar, 50).Value = orden.NotaVenta ?? (object)DBNull.Value;
                        command.Parameters.Add("@FechaIngreso", SqlDbType.Date).Value = orden.FechaIngreso;
                        command.Parameters.Add("@HoraIngreso", SqlDbType.Time).Value = orden.HoraIngreso;
                        command.Parameters.Add("@HoraEntrega", SqlDbType.Time).Value = orden.HoraEntrega;
                        command.Parameters.Add("@Bodega", SqlDbType.VarChar, 50).Value = orden.Bodega ?? (object)DBNull.Value;
                        command.Parameters.Add("@IdVendedor", SqlDbType.Int).Value = orden.IdVendedor;
                        command.Parameters.Add("@NombreVendedor", SqlDbType.VarChar, 100).Value = orden.NombreVendedor ?? (object)DBNull.Value;
                        command.Parameters.Add("@Estado", SqlDbType.Int).Value = orden.Estado;
                        command.Parameters.Add("@EstadoOTTexto", SqlDbType.VarChar, 50).Value = orden.EstadoOTTexto ?? (object)DBNull.Value;
                        command.Parameters.Add("@UsuarioModificaOT", SqlDbType.VarChar, 50).Value = orden.UsuarioModificaOT ?? (object)DBNull.Value;
                        command.Parameters.Add("@IngresoOrdenCompra", SqlDbType.VarChar, 50).Value = orden.IngresoOrdenCompra ?? (object)DBNull.Value;
                        command.Parameters.Add("@ReferenciasDTE", SqlDbType.VarChar, 100).Value = orden.ReferenciasDTE ?? (object)DBNull.Value;
                        command.Parameters.Add("@FechaRealEntregaOT", SqlDbType.Date).Value = orden.FechaRealEntregaOT ?? (object)DBNull.Value;
                        command.Parameters.Add("@HoraTerminoOT", SqlDbType.Time).Value = orden.HoraTerminoOT ?? (object)DBNull.Value;
                        command.Parameters.Add("@FechaEntregaCotizacionApprox", SqlDbType.Date).Value = orden.FechaEntregaCotizacionApprox ?? (object)DBNull.Value;
                        command.Parameters.Add("@Observaciones", SqlDbType.VarChar, -1).Value = orden.Observaciones ?? (object)DBNull.Value;
                        command.Parameters.Add("@UsuarioCreaOT", SqlDbType.VarChar, 50).Value = orden.UsuarioCreaOT ?? (object)DBNull.Value;
                        command.Parameters.Add("@AbonadoOT", SqlDbType.Decimal).Value = orden.AbonadoOT;
                        command.Parameters.Add("@CotizacionAprobada", SqlDbType.Int).Value = orden.CotizacionAprobada;
                        command.Parameters.Add("@SubTotal", SqlDbType.Decimal).Value = orden.SubTotal;
                        command.Parameters.Add("@DescuentoPorcentaje", SqlDbType.Decimal).Value = orden.DescuentoPorcentaje;
                        command.Parameters.Add("@DescuentoMonto", SqlDbType.Decimal).Value = orden.DescuentoMonto;
                        command.Parameters.Add("@TotalNeto", SqlDbType.Decimal).Value = orden.TotalNeto;
                        command.Parameters.Add("@TotalIVA", SqlDbType.Decimal).Value = orden.TotalIVA;
                        command.Parameters.Add("@TotalOT", SqlDbType.Decimal).Value = orden.TotalOT;

                        await connection.OpenAsync();
                        int filasAfectadas = await command.ExecuteNonQueryAsync();

                        result =  filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                string error = string.Format("{0},{1}", ex.Message.ToString(), ex.StackTrace.ToString());
            }

            return result;
        }

        public async Task<bool> ActualizarOrdenTrabajoAsync(OrdenTrabajoDto orden)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("dbo.sp_ActualizarOrdenTrabajo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@Idorden", SqlDbType.Int).Value = orden.Idorden;
                    command.Parameters.Add("@IdCliente", SqlDbType.Int).Value = orden.IdCliente;
                    command.Parameters.Add("@NombreCliente", SqlDbType.VarChar, 150).Value = orden.NombreCliente ?? (object)DBNull.Value;
                    command.Parameters.Add("@IdEncargado", SqlDbType.Int).Value = orden.IdEncargado;
                    command.Parameters.Add("@NombreEncargado", SqlDbType.VarChar, 100).Value = orden.NombreEncargado ?? (object)DBNull.Value;
                    command.Parameters.Add("@Sucursal", SqlDbType.VarChar, 100).Value = orden.Sucursal ?? (object)DBNull.Value;
                    command.Parameters.Add("@NotaVenta", SqlDbType.VarChar, 50).Value = orden.NotaVenta ?? (object)DBNull.Value;
                    command.Parameters.Add("@HoraEntrega", SqlDbType.Time).Value = orden.HoraEntrega;
                    command.Parameters.Add("@Bodega", SqlDbType.VarChar, 50).Value = orden.Bodega ?? (object)DBNull.Value;
                    command.Parameters.Add("@IdVendedor", SqlDbType.Int).Value = orden.IdVendedor;
                    command.Parameters.Add("@NombreVendedor", SqlDbType.VarChar, 100).Value = orden.NombreVendedor ?? (object)DBNull.Value;
                    command.Parameters.Add("@Estado", SqlDbType.Int).Value = orden.Estado;
                    command.Parameters.Add("@EstadoOTTexto", SqlDbType.VarChar, 50).Value = orden.EstadoOTTexto ?? (object)DBNull.Value;
                    command.Parameters.Add("@UsuarioModificaOT", SqlDbType.VarChar, 50).Value = orden.UsuarioModificaOT ?? (object)DBNull.Value;
                    command.Parameters.Add("@IngresoOrdenCompra", SqlDbType.VarChar, 50).Value = orden.IngresoOrdenCompra ?? (object)DBNull.Value;
                    command.Parameters.Add("@ReferenciasDTE", SqlDbType.VarChar, 100).Value = orden.ReferenciasDTE ?? (object)DBNull.Value;
                    command.Parameters.Add("@FechaRealEntregaOT", SqlDbType.Date).Value = orden.FechaRealEntregaOT ?? (object)DBNull.Value;
                    command.Parameters.Add("@HoraTerminoOT", SqlDbType.Time).Value = orden.HoraTerminoOT ?? (object)DBNull.Value;
                    command.Parameters.Add("@FechaEntregaCotizacionApprox", SqlDbType.Date).Value = orden.FechaEntregaCotizacionApprox ?? (object)DBNull.Value;
                    command.Parameters.Add("@Observaciones", SqlDbType.VarChar, -1).Value = orden.Observaciones ?? (object)DBNull.Value;
                    command.Parameters.Add("@AbonadoOT", SqlDbType.Decimal).Value = orden.AbonadoOT;
                    command.Parameters.Add("@CotizacionAprobada", SqlDbType.Int).Value = orden.CotizacionAprobada;
                    command.Parameters.Add("@SubTotal", SqlDbType.Decimal).Value = orden.SubTotal;
                    command.Parameters.Add("@DescuentoPorcentaje", SqlDbType.Decimal).Value = orden.DescuentoPorcentaje;
                    command.Parameters.Add("@DescuentoMonto", SqlDbType.Decimal).Value = orden.DescuentoMonto;
                    command.Parameters.Add("@TotalNeto", SqlDbType.Decimal).Value = orden.TotalNeto;
                    command.Parameters.Add("@TotalIVA", SqlDbType.Decimal).Value = orden.TotalIVA;
                    command.Parameters.Add("@TotalOT", SqlDbType.Decimal).Value = orden.TotalOT;

                    await connection.OpenAsync();
                    int filasAfectadas = await command.ExecuteNonQueryAsync();

                    return filasAfectadas > 0;
                }
            }
        }
        public async Task<bool> EliminarDetalleOTAsync(int idDetalle)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("dbo.sp_EliminarDetalleOT", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IdDetalle", SqlDbType.Int).Value = idDetalle;

                    await connection.OpenAsync();
                    int filasAfectadas = await command.ExecuteNonQueryAsync();

                    return filasAfectadas > 0;
                }
            }
        }
    }
}
