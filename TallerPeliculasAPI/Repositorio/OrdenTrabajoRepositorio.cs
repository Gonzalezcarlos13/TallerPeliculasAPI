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
        Task<bool> ActualizarImagenOrdenTrabajoAsync(int idImagen, string rutaImagen);
        Task<bool> EliminarImagenOrdenTrabajoAsync(int idImagen);

        Task<List<OrdenTrabajo>> LeerOrdenTrabajo(int idOrden);
        Task<decimal> LeerAbonoOrdenTrabajoAsync(int idOrden);
        Task<bool> ActualizarAbonoOrdenTrabajoAsync(int idOrden, decimal abonadoOT);

        // Anula la OT identificada por la PK real dbo.OrdenTrabajo.Id.
        Task<ResultadoAnulacionOrdenTrabajo> AnularOrdenTrabajoAsync(int idReal);
    }

    public enum ResultadoAnulacionOrdenTrabajo
    {
        NoEncontrada = 0,
        Anulada = 1,
        YaAnulada = 2
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
            if (orden == null)
            {
                throw new ArgumentNullException(nameof(orden));
            }

            int idReal;
            if (!int.TryParse(Convert.ToString(orden.Idorden), out idReal) || idReal <= 0)
            {
                throw new ArgumentException("El identificador real de la Orden de Trabajo no es válido.");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        const string sqlEstadoActual = @"
SELECT Estado
FROM dbo.OrdenTrabajo WITH (UPDLOCK, HOLDLOCK)
WHERE Id = @IdReal;";

                        int estadoActual;

                        using (var consultarEstado = new SqlCommand(
                            sqlEstadoActual,
                            connection,
                            transaction
                        ))
                        {
                            consultarEstado.CommandType = CommandType.Text;
                            consultarEstado.Parameters.Add("@IdReal", SqlDbType.Int).Value = idReal;

                            object resultadoEstado = await consultarEstado.ExecuteScalarAsync();

                            if (resultadoEstado == null || resultadoEstado == DBNull.Value)
                            {
                                transaction.Rollback();
                                return false;
                            }

                            estadoActual = Convert.ToInt32(resultadoEstado);
                        }

                        const string sqlActualizar = @"
UPDATE dbo.OrdenTrabajo
SET IdCliente = @IdCliente,
    NombreCliente = @NombreCliente,
    IdEncargado = @IdEncargado,
    NombreEncargado = @NombreEncargado,
    Sucursal = @Sucursal,
    NotaVenta = @NotaVenta,
    HoraEntrega = @HoraEntrega,
    Bodega = @Bodega,
    IdVendedor = @IdVendedor,
    NombreVendedor = @NombreVendedor,
    Estado = @Estado,
    EstadoOTTexto = @EstadoOTTexto,
    UsuarioModificaOT = @UsuarioModificaOT,
    IngresoOrdenCompra = @IngresoOrdenCompra,
    ReferenciasDTE = @ReferenciasDTE,
    FechaRealEntregaOT = @FechaRealEntregaOT,
    HoraTerminoOT = @HoraTerminoOT,
    FechaEntregaCotizacionApprox = @FechaEntregaCotizacionApprox,
    Observaciones = @Observaciones,
    AbonadoOT = @AbonadoOT,
    CotizacionAprobada = @CotizacionAprobada,
    SubTotal = @SubTotal,
    DescuentoPorcentaje = @DescuentoPorcentaje,
    DescuentoMonto = @DescuentoMonto,
    TotalNeto = @TotalNeto,
    TotalIVA = @TotalIVA,
    TotalOT = @TotalOT
WHERE Id = @IdReal;";

                        using (var command = new SqlCommand(
                            sqlActualizar,
                            connection,
                            transaction
                        ))
                        {
                            command.CommandType = CommandType.Text;

                            command.Parameters.Add("@IdReal", SqlDbType.Int).Value = idReal;
                            command.Parameters.Add("@IdCliente", SqlDbType.Int).Value = Convert.ToInt32(orden.IdCliente);
                            command.Parameters.Add("@NombreCliente", SqlDbType.VarChar, 150).Value = orden.NombreCliente ?? (object)DBNull.Value;
                            command.Parameters.Add("@IdEncargado", SqlDbType.Int).Value = Convert.ToInt32(orden.IdEncargado);
                            command.Parameters.Add("@NombreEncargado", SqlDbType.VarChar, 100).Value = orden.NombreEncargado ?? (object)DBNull.Value;
                            command.Parameters.Add("@Sucursal", SqlDbType.VarChar, 100).Value = orden.Sucursal ?? (object)DBNull.Value;
                            command.Parameters.Add("@NotaVenta", SqlDbType.VarChar, 50).Value = orden.NotaVenta ?? (object)DBNull.Value;
                            command.Parameters.Add("@HoraEntrega", SqlDbType.Time).Value = string.IsNullOrWhiteSpace(Convert.ToString(orden.HoraEntrega)) ? (object)DBNull.Value : orden.HoraEntrega;
                            command.Parameters.Add("@Bodega", SqlDbType.VarChar, 50).Value = orden.Bodega ?? (object)DBNull.Value;
                            command.Parameters.Add("@IdVendedor", SqlDbType.Int).Value = Convert.ToInt32(orden.IdVendedor);
                            command.Parameters.Add("@NombreVendedor", SqlDbType.VarChar, 100).Value = orden.NombreVendedor ?? (object)DBNull.Value;
                            command.Parameters.Add("@Estado", SqlDbType.Int).Value = estadoActual;
                            command.Parameters.Add("@EstadoOTTexto", SqlDbType.VarChar, 50).Value = orden.EstadoOTTexto ?? (object)DBNull.Value;
                            command.Parameters.Add("@UsuarioModificaOT", SqlDbType.VarChar, 50).Value = orden.UsuarioModificaOT ?? (object)DBNull.Value;
                            command.Parameters.Add("@IngresoOrdenCompra", SqlDbType.VarChar, 50).Value = orden.IngresoOrdenCompra ?? (object)DBNull.Value;
                            command.Parameters.Add("@ReferenciasDTE", SqlDbType.VarChar, 100).Value = orden.ReferenciasDTE ?? (object)DBNull.Value;
                            command.Parameters.Add("@FechaRealEntregaOT", SqlDbType.Date).Value = string.IsNullOrWhiteSpace(Convert.ToString(orden.FechaRealEntregaOT)) ? (object)DBNull.Value : orden.FechaRealEntregaOT;
                            command.Parameters.Add("@HoraTerminoOT", SqlDbType.Time).Value = string.IsNullOrWhiteSpace(Convert.ToString(orden.HoraTerminoOT)) ? (object)DBNull.Value : orden.HoraTerminoOT;
                            command.Parameters.Add("@FechaEntregaCotizacionApprox", SqlDbType.Date).Value = string.IsNullOrWhiteSpace(Convert.ToString(orden.FechaEntregaCotizacionApprox)) ? (object)DBNull.Value : orden.FechaEntregaCotizacionApprox;
                            command.Parameters.Add("@Observaciones", SqlDbType.VarChar, -1).Value = orden.Observaciones ?? (object)DBNull.Value;

                            var parametroAbonado = command.Parameters.Add("@AbonadoOT", SqlDbType.Decimal);
                            parametroAbonado.Precision = 18;
                            parametroAbonado.Scale = 2;
                            parametroAbonado.Value = Convert.ToDecimal(orden.AbonadoOT);

                            command.Parameters.Add("@CotizacionAprobada", SqlDbType.Int).Value = Convert.ToInt32(orden.CotizacionAprobada);

                            var parametroSubTotal = command.Parameters.Add("@SubTotal", SqlDbType.Decimal);
                            parametroSubTotal.Precision = 18;
                            parametroSubTotal.Scale = 2;
                            parametroSubTotal.Value = Convert.ToDecimal(orden.SubTotal);

                            var parametroDescuentoPorcentaje = command.Parameters.Add("@DescuentoPorcentaje", SqlDbType.Decimal);
                            parametroDescuentoPorcentaje.Precision = 5;
                            parametroDescuentoPorcentaje.Scale = 2;
                            parametroDescuentoPorcentaje.Value = Convert.ToDecimal(orden.DescuentoPorcentaje);

                            var parametroDescuentoMonto = command.Parameters.Add("@DescuentoMonto", SqlDbType.Decimal);
                            parametroDescuentoMonto.Precision = 18;
                            parametroDescuentoMonto.Scale = 2;
                            parametroDescuentoMonto.Value = Convert.ToDecimal(orden.DescuentoMonto);

                            var parametroTotalNeto = command.Parameters.Add("@TotalNeto", SqlDbType.Decimal);
                            parametroTotalNeto.Precision = 18;
                            parametroTotalNeto.Scale = 2;
                            parametroTotalNeto.Value = Convert.ToDecimal(orden.TotalNeto);

                            var parametroTotalIVA = command.Parameters.Add("@TotalIVA", SqlDbType.Decimal);
                            parametroTotalIVA.Precision = 18;
                            parametroTotalIVA.Scale = 2;
                            parametroTotalIVA.Value = Convert.ToDecimal(orden.TotalIVA);

                            var parametroTotalOT = command.Parameters.Add("@TotalOT", SqlDbType.Decimal);
                            parametroTotalOT.Precision = 18;
                            parametroTotalOT.Scale = 2;
                            parametroTotalOT.Value = Convert.ToDecimal(orden.TotalOT);

                            int filasAfectadas = await command.ExecuteNonQueryAsync();

                            if (filasAfectadas <= 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                        }

                        throw;
                    }
                }
            }
        }

        public async Task<bool> EliminarDetalleOTAsync(int idDetalle)
        {
            if (idDetalle <= 0)
            {
                throw new ArgumentException(
                    "El identificador del detalle no es válido.",
                    nameof(idDetalle)
                );
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        const string sqlVerificar = @"
SELECT TOP 1 IdDetalle
FROM dbo.OrdenTrabajoDetalle WITH (UPDLOCK, HOLDLOCK)
WHERE IdDetalle = @IdDetalle;";

                        using (var verificarAntes = new SqlCommand(
                            sqlVerificar,
                            connection,
                            transaction
                        ))
                        {
                            verificarAntes.CommandType = CommandType.Text;
                            verificarAntes.Parameters.Add(
                                "@IdDetalle",
                                SqlDbType.Int
                            ).Value = idDetalle;

                            object existente =
                                await verificarAntes.ExecuteScalarAsync();

                            if (existente == null || existente == DBNull.Value)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }

                        using (var command = new SqlCommand(
                            "dbo.sp_EliminarDetalleOT",
                            connection,
                            transaction
                        ))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add(
                                "@IdDetalle",
                                SqlDbType.Int
                            ).Value = idDetalle;

                            await command.ExecuteNonQueryAsync();
                        }

                        using (var verificarDespues = new SqlCommand(
                            sqlVerificar,
                            connection,
                            transaction
                        ))
                        {
                            verificarDespues.CommandType = CommandType.Text;
                            verificarDespues.Parameters.Add(
                                "@IdDetalle",
                                SqlDbType.Int
                            ).Value = idDetalle;

                            object existente =
                                await verificarDespues.ExecuteScalarAsync();

                            if (existente != null && existente != DBNull.Value)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (InvalidOperationException)
                        {
                        }

                        throw;
                    }
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

        public async Task<bool> CrearImagenesOrdenTrabajoAsync(
            OrdenTrabajoImagenDto imagen)
        {
            if (imagen == null)
            {
                throw new ArgumentNullException(
                    nameof(imagen),
                    "Los datos de la imagen son requeridos."
                );
            }

            int idOrden;

            if (
                !int.TryParse(imagen.Idorden, out idOrden) ||
                idOrden <= 0
            )
            {
                throw new ArgumentException(
                    "El Idorden de la imagen no es válido."
                );
            }

            if (string.IsNullOrWhiteSpace(imagen.RutaImagen))
            {
                throw new ArgumentException(
                    "La imagen no contiene información para guardar."
                );
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(
                    "dbo.sp_CrearImagenOrdenTrabajo",
                    connection
                ))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(
                        "@Idorden",
                        SqlDbType.Int
                    ).Value = idOrden;

                    command.Parameters.Add(
                        "@RutaImagen",
                        SqlDbType.VarChar,
                        -1
                    ).Value = imagen.RutaImagen;

                    await command.ExecuteNonQueryAsync();
                }

                const string sqlVerificar = @"
SELECT TOP 1 IdImagen
FROM dbo.OrdenTrabajoImagenes
WHERE Idorden = @Idorden
  AND RutaImagen = @RutaImagen
ORDER BY IdImagen DESC;";

                using (var verificar = new SqlCommand(
                    sqlVerificar,
                    connection
                ))
                {
                    verificar.CommandType = CommandType.Text;

                    verificar.Parameters.Add(
                        "@Idorden",
                        SqlDbType.Int
                    ).Value = idOrden;

                    verificar.Parameters.Add(
                        "@RutaImagen",
                        SqlDbType.VarChar,
                        -1
                    ).Value = imagen.RutaImagen;

                    object resultado =
                        await verificar.ExecuteScalarAsync();

                    bool guardada =
                        resultado != null &&
                        resultado != DBNull.Value;

                    System.Diagnostics.Debug.WriteLine(
                        "[IMAGEN OT] IdOrden: " + idOrden
                    );

                    System.Diagnostics.Debug.WriteLine(
                        "[IMAGEN OT] Largo RutaImagen: " +
                        imagen.RutaImagen.Length
                    );

                    System.Diagnostics.Debug.WriteLine(
                        "[IMAGEN OT] Guardada en BD: " +
                        guardada
                    );

                    if (!guardada)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            "[IMAGEN OT] El procedimiento terminó, " +
                            "pero no se encontró el registro en " +
                            "dbo.OrdenTrabajoImagenes."
                        );
                    }

                    return guardada;
                }
            }
        }

        public async Task<bool> ActualizarImagenOrdenTrabajoAsync(
            int idImagen,
            string rutaImagen)
        {
            if (idImagen <= 0)
            {
                throw new ArgumentException("El IdImagen no es válido.");
            }

            if (string.IsNullOrWhiteSpace(rutaImagen))
            {
                throw new ArgumentException(
                    "La imagen no contiene información para actualizar."
                );
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"
UPDATE dbo.OrdenTrabajoImagenes
SET RutaImagen = @RutaImagen
WHERE IdImagen = @IdImagen;";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(
                        "@IdImagen",
                        SqlDbType.Int
                    ).Value = idImagen;

                    command.Parameters.Add(
                        "@RutaImagen",
                        SqlDbType.VarChar,
                        -1
                    ).Value = rutaImagen;

                    await connection.OpenAsync();

                    int filasAfectadas =
                        await command.ExecuteNonQueryAsync();

                    return filasAfectadas > 0;
                }
            }
        }


        public async Task<bool> EliminarImagenOrdenTrabajoAsync(int idImagen)
        {
            if (idImagen <= 0)
            {
                throw new ArgumentException("El IdImagen no es válido.");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"
DELETE FROM dbo.OrdenTrabajoImagenes
WHERE IdImagen = @IdImagen;";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@IdImagen", SqlDbType.Int).Value = idImagen;

                    await connection.OpenAsync();

                    int filasAfectadas = await command.ExecuteNonQueryAsync();

                    return filasAfectadas > 0;
                }
            }
        }

        public async Task<bool> ObtenerOrdenesTrabajo(OrdenTrabajoImagenDto imagen)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("dbo.sp_LeerOrdenTrabajo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@Idorden", SqlDbType.Int).Value = "";

                    await connection.OpenAsync();
                    int filasAfectadas = await command.ExecuteNonQueryAsync();
                    return filasAfectadas > 0;
                }
            }
        }

        public async Task<List<OrdenTrabajo>> LeerOrdenTrabajo(int idOrden)
        {
            var listaOrdenes = new List<OrdenTrabajo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_LeerOrdenTrabajo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Idorden", idOrden);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            listaOrdenes.Add(
                                MapearOrdenTrabajo(reader)
                            );
                        }

                        if (idOrden > 0 && listaOrdenes.Count > 0)
                        {
                            var orden = listaOrdenes[0];

                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    orden.Detalles.Add(
                                        MapearOrdenTrabajoDetalle(reader)
                                    );
                                }
                            }

                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    orden.Imagenes.Add(
                                        MapearOrdenTrabajoImagen(reader)
                                    );
                                }
                            }
                        }
                    }
                }
            }

            return listaOrdenes;
        }

        private OrdenTrabajo MapearOrdenTrabajo(SqlDataReader reader)
        {
            return new OrdenTrabajo
            {
                Id = Convert.ToInt32(reader["Id"]),
                Idorden = Convert.ToInt32(reader["Idorden"]),
                IdCliente = Convert.ToInt32(reader["IdCliente"]),
                NombreCliente = reader["NombreCliente"].ToString(),
                IdEncargado = Convert.ToInt32(reader["IdEncargado"]),
                NombreEncargado = reader["NombreEncargado"].ToString(),
                Sucursal = reader["Sucursal"].ToString(),
                NotaVenta = reader["NotaVenta"].ToString(),
                FechaIngreso = reader["FechaIngreso"].ToString(),
                HoraIngreso = reader["HoraIngreso"].ToString(),
                HoraEntrega = reader["HoraEntrega"].ToString(),
                Bodega = reader["Bodega"].ToString(),
                IdVendedor = Convert.ToInt32(reader["IdVendedor"]),
                NombreVendedor = reader["NombreVendedor"].ToString(),
                Estado = Convert.ToInt32(reader["Estado"]),
                EstadoOTTexto = reader["EstadoOTTexto"].ToString(),
                UsuarioModificaOT = reader["UsuarioModificaOT"].ToString(),
                IngresoOrdenCompra = reader["IngresoOrdenCompra"].ToString(),
                ReferenciasDTE = reader["ReferenciasDTE"].ToString(),
                FechaRealEntregaOT = reader["FechaRealEntregaOT"].ToString(),
                HoraTerminoOT = reader["HoraTerminoOT"].ToString(),
                FechaEntregaCotizacionApprox = reader["FechaEntregaCotizacionApprox"].ToString(),
                Observaciones = reader["Observaciones"].ToString(),
                UsuarioCreaOT = reader["UsuarioCreaOT"].ToString(),
                AbonadoOT = Convert.ToDecimal(reader["AbonadoOT"]),
                CotizacionAprobada = Convert.ToInt32(reader["CotizacionAprobada"]),
                SubTotal = Convert.ToDecimal(reader["SubTotal"]),
                DescuentoPorcentaje = Convert.ToDecimal(reader["DescuentoPorcentaje"]),
                DescuentoMonto = Convert.ToDecimal(reader["DescuentoMonto"]),
                TotalNeto = Convert.ToDecimal(reader["TotalNeto"]),
                TotalIVA = Convert.ToDecimal(reader["TotalIVA"]),
                TotalOT = Convert.ToDecimal(reader["TotalOT"])
            };
        }

        private OrdenTrabajoDetalle MapearOrdenTrabajoDetalle(SqlDataReader reader)
        {
            return new OrdenTrabajoDetalle
            {
                IdDetalle = Convert.ToInt32(reader["IdDetalle"]),
                Idorden = Convert.ToInt32(reader["Idorden"]),
                CodigoProducto = reader["CodigoProducto"].ToString(),
                Descripcion = reader["Descripcion"].ToString(),
                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                ValorNeto = Convert.ToDecimal(reader["ValorNeto"]),
                DescuentoPorcentaje = Convert.ToDecimal(reader["DescuentoPorcentaje"]),
                TotalNeto = Convert.ToDecimal(reader["TotalNeto"]),
                ComisionPorcentaje = Convert.ToDecimal(reader["ComisionPorcentaje"]),
                TotalComision = Convert.ToDecimal(reader["TotalComision"]),
                SinRebajaDeStock = Convert.ToBoolean(reader["SinRebajaDeStock"]),
                IdTipo = Convert.ToInt32(reader["IdTipo"])
            };
        }

        private OrdenTrabajoImagenes MapearOrdenTrabajoImagen(SqlDataReader reader)
        {
            return new OrdenTrabajoImagenes
            {
                IdImagen = Convert.ToInt32(reader["IdImagen"]),
                Idorden = Convert.ToInt32(reader["Idorden"]),
                RutaImagen = reader["RutaImagen"].ToString()
            };
        }

        public async Task<decimal> LeerAbonoOrdenTrabajoAsync(int idOrden)
        {
            if (idOrden <= 0)
            {
                throw new ArgumentException(
                    "El IdOrden no es válido."
                );
            }

            var ordenes = await LeerOrdenTrabajo(idOrden);

            if (ordenes == null || ordenes.Count == 0)
            {
                return 0m;
            }

            return ordenes[0].AbonadoOT;
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

            if (abonadoOT < 0)
            {
                throw new ArgumentException(
                    "El monto abonado no puede ser negativo."
                );
            }

            var ordenes = await LeerOrdenTrabajo(idOrden);

            if (ordenes == null || ordenes.Count == 0)
            {
                throw new ArgumentException(
                    "No se encontró la Orden de Trabajo " + idOrden + "."
                );
            }

            var ordenActual = ordenes[0];
            int idReal = ordenActual.Id;

            if (idReal <= 0)
            {
                throw new InvalidOperationException(
                    "No fue posible determinar el Id real de la Orden de Trabajo."
                );
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        const string sqlActualizar = @"
UPDATE dbo.OrdenTrabajo
SET AbonadoOT = @AbonadoOT
WHERE Id = @IdReal;";

                        int filasAfectadas;

                        using (var command = new SqlCommand(
                            sqlActualizar,
                            connection,
                            transaction
                        ))
                        {
                            command.CommandType = CommandType.Text;

                            command.Parameters.Add(
                                "@IdReal",
                                SqlDbType.Int
                            ).Value = idReal;

                            var parametroAbono = command.Parameters.Add(
                                "@AbonadoOT",
                                SqlDbType.Decimal
                            );

                            parametroAbono.Precision = 18;
                            parametroAbono.Scale = 2;
                            parametroAbono.Value = abonadoOT;

                            filasAfectadas =
                                await command.ExecuteNonQueryAsync();
                        }

                        if (filasAfectadas <= 0)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        const string sqlVerificar = @"
SELECT AbonadoOT
FROM dbo.OrdenTrabajo
WHERE Id = @IdReal;";

                        decimal valorGuardado;

                        using (var commandVerificar = new SqlCommand(
                            sqlVerificar,
                            connection,
                            transaction
                        ))
                        {
                            commandVerificar.CommandType = CommandType.Text;

                            commandVerificar.Parameters.Add(
                                "@IdReal",
                                SqlDbType.Int
                            ).Value = idReal;

                            object resultado =
                                await commandVerificar.ExecuteScalarAsync();

                            if (
                                resultado == null ||
                                resultado == DBNull.Value
                            )
                            {
                                transaction.Rollback();
                                return false;
                            }

                            valorGuardado =
                                Convert.ToDecimal(resultado);
                        }

                        if (valorGuardado != abonadoOT)
                        {
                            transaction.Rollback();

                            throw new InvalidOperationException(
                                "El valor del abono no quedó guardado correctamente."
                            );
                        }

                        transaction.Commit();

                        System.Diagnostics.Debug.WriteLine(
                            "[ABONO OT] O.T. solicitada: " + idOrden
                        );

                        System.Diagnostics.Debug.WriteLine(
                            "[ABONO OT] Id real actualizado: " + idReal
                        );

                        System.Diagnostics.Debug.WriteLine(
                            "[ABONO OT] Valor enviado: " + abonadoOT
                        );

                        System.Diagnostics.Debug.WriteLine(
                            "[ABONO OT] Valor verificado en BD: " + valorGuardado
                        );

                        return true;
                    }
                    catch
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {

                        }

                        throw;
                    }
                }
            }
        }

        public async Task<ResultadoAnulacionOrdenTrabajo> AnularOrdenTrabajoAsync(int idReal)
        {
            if (idReal <= 0)
            {
                throw new ArgumentException("El ID real de la OT debe ser mayor que cero.", nameof(idReal));
            }

            using (var conexion = new SqlConnection(_connectionString))
            {
                await conexion.OpenAsync();

                using (var transaccion = conexion.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        // Bloquear esta fila hasta el final para evitar anulaciones simultáneas.
                        const string consultarEstadoSql = @"
SELECT EstadoOTTexto
FROM dbo.OrdenTrabajo WITH (UPDLOCK, HOLDLOCK)
WHERE Id = @IdReal;";

                        string estadoActual;
                        using (var consultar = new SqlCommand(consultarEstadoSql, conexion, transaccion))
                        {
                            consultar.CommandType = CommandType.Text;
                            consultar.Parameters.Add("@IdReal", SqlDbType.Int).Value = idReal;

                            object valor = await consultar.ExecuteScalarAsync();
                            if (valor == null)
                            {
                                transaccion.Rollback();
                                return ResultadoAnulacionOrdenTrabajo.NoEncontrada;
                            }

                            estadoActual = valor == DBNull.Value
                                ? string.Empty
                                : Convert.ToString(valor).Trim();
                        }

                        if (string.Equals(estadoActual, "ANULADA", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(estadoActual, "ANULADO", StringComparison.OrdinalIgnoreCase))
                        {
                            transaccion.Commit();
                            return ResultadoAnulacionOrdenTrabajo.YaAnulada;
                        }

                        const string anularSql = @"
UPDATE dbo.OrdenTrabajo
SET EstadoOTTexto = @EstadoOTTexto
WHERE Id = @IdReal;";

                        using (var actualizar = new SqlCommand(anularSql, conexion, transaccion))
                        {
                            actualizar.CommandType = CommandType.Text;
                            actualizar.Parameters.Add("@IdReal", SqlDbType.Int).Value = idReal;
                            actualizar.Parameters.Add("@EstadoOTTexto", SqlDbType.VarChar, 50).Value = "ANULADA";
                            await actualizar.ExecuteNonQueryAsync();
                        }

                        // No depender del conteo de filas, que puede variar por NOCOUNT o triggers.
                        // Verificar el estado persistido antes de confirmar la transaccion.

                        // Verificar dentro de la misma transacción antes del commit.
                        using (var verificar = new SqlCommand(consultarEstadoSql, conexion, transaccion))
                        {
                            verificar.CommandType = CommandType.Text;
                            verificar.Parameters.Add("@IdReal", SqlDbType.Int).Value = idReal;

                            object resultado = await verificar.ExecuteScalarAsync();
                            string estadoGuardado = resultado == null || resultado == DBNull.Value
                                ? string.Empty
                                : Convert.ToString(resultado).Trim();

                            if (!string.Equals(estadoGuardado, "ANULADA", StringComparison.OrdinalIgnoreCase))
                            {
                                throw new InvalidOperationException(
                                    "No se pudo verificar el estado ANULADA en la base de datos."
                                );
                            }
                        }

                        transaccion.Commit();
                        return ResultadoAnulacionOrdenTrabajo.Anulada;
                    }
                    catch
                    {
                        // Si ya hubo rollback o commit, un segundo rollback puede fallar.
                        try
                        {
                            transaccion.Rollback();
                        }
                        catch (InvalidOperationException)
                        {
                        }
                        throw;
                    }
                }
            }
        }

    }
}
