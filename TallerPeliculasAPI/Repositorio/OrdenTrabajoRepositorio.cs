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
        Task<int> CrearOrdenTrabajoAsync(OrdenTrabajoDto orden);
        Task<bool> ActualizarOrdenTrabajoAsync(OrdenTrabajoDto orden);
        Task<bool> EliminarDetalleOTAsync(int idDetalle);
        Task<bool> CrearDetalleOrdenTrabajoAsync(OrdenTrabajoDetalleDto detalle);
        Task<bool> CrearImagenesOrdenTrabajoAsync(OrdenTrabajoImagenDto imagen);
    }

    public class OrdenTrabajoRepository : IOrdenTrabajoRepository
    {
        private readonly string _connectionString;

        public OrdenTrabajoRepository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MiConexionSqlServer"].ConnectionString;
            _connectionString = connectionString;
        }

        public async Task<int> CrearOrdenTrabajoAsync(OrdenTrabajoDto orden)
        {
            int result = 0;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("dbo.sp_CrearOrdenTrabajo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Ya no enviamos el parámetro @Idorden porque lo genera la BD
                        command.Parameters.Add("@IdCliente", SqlDbType.Int).Value = Convert.ToInt32(orden.IdCliente);
                        command.Parameters.Add("@NombreCliente", SqlDbType.VarChar, 150).Value = orden.NombreCliente ?? (object)DBNull.Value;
                        command.Parameters.Add("@IdEncargado", SqlDbType.Int).Value = Convert.ToInt32(orden.IdEncargado);
                        command.Parameters.Add("@NombreEncargado", SqlDbType.VarChar, 100).Value = orden.NombreEncargado ?? (object)DBNull.Value;
                        command.Parameters.Add("@Sucursal", SqlDbType.VarChar, 100).Value = orden.Sucursal ?? (object)DBNull.Value;
                        command.Parameters.Add("@NotaVenta", SqlDbType.VarChar, 50).Value = orden.NotaVenta ?? (object)DBNull.Value;
                        command.Parameters.Add("@FechaIngreso", SqlDbType.Date).Value = orden.FechaIngreso;
                        command.Parameters.Add("@HoraIngreso", SqlDbType.Time).Value = orden.HoraIngreso;
                        command.Parameters.Add("@HoraEntrega", SqlDbType.Time).Value = orden.HoraEntrega;
                        command.Parameters.Add("@Bodega", SqlDbType.VarChar, 50).Value = orden.Bodega ?? (object)DBNull.Value;
                        command.Parameters.Add("@IdVendedor", SqlDbType.Int).Value = Convert.ToInt32(orden.IdVendedor);
                        command.Parameters.Add("@NombreVendedor", SqlDbType.VarChar, 100).Value = orden.NombreVendedor ?? (object)DBNull.Value;
                        command.Parameters.Add("@Estado", SqlDbType.Int).Value = Convert.ToInt32(orden.Estado);
                        command.Parameters.Add("@EstadoOTTexto", SqlDbType.VarChar, 50).Value = orden.EstadoOTTexto ?? (object)DBNull.Value;
                        command.Parameters.Add("@UsuarioModificaOT", SqlDbType.VarChar, 50).Value = orden.UsuarioModificaOT ?? (object)DBNull.Value;
                        command.Parameters.Add("@IngresoOrdenCompra", SqlDbType.VarChar, 50).Value = orden.IngresoOrdenCompra ?? (object)DBNull.Value;
                        command.Parameters.Add("@ReferenciasDTE", SqlDbType.VarChar, 100).Value = orden.ReferenciasDTE ?? (object)DBNull.Value;
                        command.Parameters.Add("@FechaRealEntregaOT", SqlDbType.Date).Value = string.IsNullOrEmpty(orden.FechaRealEntregaOT) ? (object)DBNull.Value : orden.FechaRealEntregaOT;
                        command.Parameters.Add("@HoraTerminoOT", SqlDbType.Time).Value = string.IsNullOrEmpty(orden.HoraTerminoOT) ? (object)DBNull.Value : orden.HoraTerminoOT;
                        command.Parameters.Add("@FechaEntregaCotizacionApprox", SqlDbType.Date).Value = string.IsNullOrEmpty(orden.FechaEntregaCotizacionApprox) ? (object)DBNull.Value : orden.FechaEntregaCotizacionApprox;
                        command.Parameters.Add("@Observaciones", SqlDbType.VarChar, -1).Value = orden.Observaciones ?? (object)DBNull.Value;
                        command.Parameters.Add("@UsuarioCreaOT", SqlDbType.VarChar, 50).Value = orden.UsuarioCreaOT ?? (object)DBNull.Value;
                        command.Parameters.Add("@AbonadoOT", SqlDbType.Decimal).Value = Convert.ToDecimal(orden.AbonadoOT);
                        command.Parameters.Add("@CotizacionAprobada", SqlDbType.Int).Value = Convert.ToInt32(orden.CotizacionAprobada);
                        command.Parameters.Add("@SubTotal", SqlDbType.Decimal).Value = Convert.ToDecimal(orden.SubTotal);
                        command.Parameters.Add("@DescuentoPorcentaje", SqlDbType.Decimal).Value = Convert.ToDecimal(orden.DescuentoPorcentaje);
                        command.Parameters.Add("@DescuentoMonto", SqlDbType.Decimal).Value = Convert.ToDecimal(orden.DescuentoMonto);
                        command.Parameters.Add("@TotalNeto", SqlDbType.Decimal).Value = Convert.ToDecimal(orden.TotalNeto);
                        command.Parameters.Add("@TotalIVA", SqlDbType.Decimal).Value = Convert.ToDecimal(orden.TotalIVA);
                        command.Parameters.Add("@TotalOT", SqlDbType.Decimal).Value = Convert.ToDecimal(orden.TotalOT);

                        await connection.OpenAsync();

                        // ExecuteScalar ejecuta la consulta y retorna el objeto del SELECT SCOPE_IDENTITY()
                        var scalarResult = await command.ExecuteScalarAsync();

                        if (scalarResult != null && scalarResult != DBNull.Value)
                        {
                            result = Convert.ToInt32(scalarResult);
                        }
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
        public async Task<bool> CrearDetalleOrdenTrabajoAsync(OrdenTrabajoDetalleDto detalle)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("dbo.sp_CrearDetalleOrdenTrabajo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@Idorden", SqlDbType.Int).Value = Convert.ToInt32(detalle.Idorden);
                        command.Parameters.Add("@Codigo", SqlDbType.VarChar, 50).Value = detalle.Codigo ?? (object)DBNull.Value;
                        command.Parameters.Add("@Descripcion", SqlDbType.VarChar, 250).Value = detalle.Descripcion ?? (object)DBNull.Value;
                        command.Parameters.Add("@Cantidad", SqlDbType.Decimal).Value = Convert.ToDecimal(detalle.Cantidad);
                        command.Parameters.Add("@ValorNeto", SqlDbType.Decimal).Value = Convert.ToDecimal(detalle.ValorUnitario);
                        command.Parameters.Add("@DescuentoPorcentaje", SqlDbType.Decimal).Value = Convert.ToDecimal(detalle.DescuentoPorcentaje);
                        command.Parameters.Add("@TotalNeto", SqlDbType.Decimal).Value = Convert.ToDecimal(detalle.Cantidad) * Convert.ToDecimal(detalle.ValorUnitario);
                        command.Parameters.Add("@ComisionPorcentaje", SqlDbType.Decimal).Value = Convert.ToDecimal("0");
                        command.Parameters.Add("@TotalComision", SqlDbType.Decimal).Value = Convert.ToDecimal("0");
                        command.Parameters.Add("@SinRebajaDeStock", SqlDbType.Decimal).Value = Convert.ToDecimal("1");
                        command.Parameters.Add("@IdTipo", SqlDbType.Int).Value = Convert.ToInt32(detalle.IdTipo);

                        await connection.OpenAsync();
                        int filasAfectadas = await command.ExecuteNonQueryAsync();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                string error = string.Format("{0},{1}", ex.Message, ex.StackTrace);
                return false;
            }

        }
        public async Task<bool> CrearImagenesOrdenTrabajoAsync(OrdenTrabajoImagenDto imagen)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("dbo.sp_CrearImagenOrdenTrabajo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@Idorden", SqlDbType.Int).Value = Convert.ToInt32(imagen.Idorden);
                    command.Parameters.Add("@RutaImagen", SqlDbType.VarChar, -1).Value = imagen.RutaImagen ?? (object)DBNull.Value;

                    await connection.OpenAsync();
                    int filasAfectadas = await command.ExecuteNonQueryAsync();
                    return filasAfectadas > 0;
                }
            }
        }
    }
}
