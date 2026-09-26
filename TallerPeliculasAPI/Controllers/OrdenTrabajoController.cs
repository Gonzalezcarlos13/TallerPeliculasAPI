using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using TallerPeliculasAPI.Modelo;
using TallerPeliculasAPI.Negocio;

namespace TallerPeliculasAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OrdenTrabajoController : ApiController
    {
        private readonly IOrdenTrabajoNegocio _negocio;

        public OrdenTrabajoController()
        {
            _negocio = new OrdenTrabajoNegocio();
        }

        #region Crear

        [HttpPost]
        [Route("api/OrdenTrabajo/Crear")]
        public async Task<IHttpActionResult> Crear([FromBody] OrdenTrabajoCompletaDto orden)
        {
            if (orden == null)
            {
                return BadRequest("Los datos de la orden de trabajo son requeridos.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int idot = await _negocio.CrearOrdenTrabajoAsync(orden.Cabecera);


                if (orden.Detalles != null)
                {
                    foreach (var item in orden.Detalles)
                    {
                        item.Idorden = idot.ToString();
                        await _negocio.CrearDetalleOrdenTrabajoAsync(item);
                    }
                }

                if (orden.Imagenes != null)
                {
                    foreach (var item in orden.Imagenes)
                    {
                        item.Idorden = idot.ToString();
                        await _negocio.CrearImagenesOrdenTrabajoAsync(item);
                    }
                }


                if (idot > 0)
                {
                    return Ok(new { Id = idot, exito = true, mensaje = "Orden de trabajo creada correctamente." });
                }
                else
                {
                    return InternalServerError(new Exception("No se pudo completar el registro de la orden de trabajo."));
                }
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        #endregion

        #region Actualizar
        [HttpPost]
        [Route("api/OrdenTrabajo/Actualizar")]
        public async Task<IHttpActionResult> Actualizar([FromBody] OrdenTrabajoDto orden)
        {
            if (orden == null)
            {
                return BadRequest("Los datos de la orden de trabajo son requeridos.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool resultado = await _negocio.ActualizarOrdenTrabajoAsync(orden);

                if (resultado)
                {
                    return Ok(new { exito = true, mensaje = "Orden de trabajo creada correctamente." });
                }
                else
                {
                    return InternalServerError(new Exception("No se pudo completar el registro de la orden de trabajo."));
                }
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        #endregion

        [HttpDelete]
        [Route("eliminar-detalle/{idDetalle:int}")]
        public async Task<IHttpActionResult> EliminarDetalle(int idDetalle)
        {
            try
            {
                bool resultado = await _negocio.EliminarDetalleOTAsync(idDetalle);

                if (resultado)
                {
                    return Ok(new { exito = true, mensaje = "Detalle de orden de trabajo eliminado correctamente." });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #region Actualizar
        [HttpGet]
        [Route("api/OrdenTrabajo/Leer/{idorden}")]
        public async Task<IHttpActionResult> LeerOrdenesTrabajo(string idorden)
        {

            try
            {
                if (string.IsNullOrEmpty(idorden))
                {
                    return BadRequest("El ID de la orden es requerido.");
                }

                //idorden ="37";
                var listado = await _negocio.LeerOrdenTrabajo(int.Parse(idorden));
                return Ok(listado);
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        #endregion
        #region CrearDetalle

        [HttpPost]
        [ActionName("CrearDetalleOrdenTrabajo")]
        [Route("api/OrdenTrabajo/CrearDetalleOrdenTrabajo")]
        public async Task<IHttpActionResult> CrearDetalleOrdenTrabajo(
            [FromBody] OrdenTrabajoCompletaDto orden)
        {
            if (orden == null)
            {
                return BadRequest("Los datos del detalle son requeridos.");
            }

            if (orden.Detalles == null || orden.Detalles.Count == 0)
            {
                return BadRequest("No se recibieron detalles para guardar.");
            }

            try
            {
                int guardados = 0;

                foreach (var item in orden.Detalles)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.Idorden))
                    {
                        return BadRequest("El Idorden es requerido.");
                    }

                    int idOrden;

                    if (!int.TryParse(item.Idorden, out idOrden) || idOrden <= 0)
                    {
                        return BadRequest("El Idorden no es válido.");
                    }

                    await _negocio.CrearDetalleOrdenTrabajoAsync(item);

                    guardados++;
                }

                if (guardados == 0)
                {
                    return BadRequest("No existen detalles válidos para guardar.");
                }

                return Ok(new
                {
                    exito = true,
                    cantidad = guardados,
                    mensaje = "Productos agregados correctamente."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion
        #region AbonoOT

        public class AbonoOrdenTrabajoRequest
        {
            public int IdOrden { get; set; }
            public decimal AbonadoOT { get; set; }
        }


        // ================================================
        // COMPROBAR QUE LA API NUEVA ESTÁ PUBLICADA
        // ================================================
        [HttpGet]
        [Route("api/OrdenTrabajo/PingAbono")]
        public IHttpActionResult PingAbono()
        {
            return Ok(new
            {
                exito = true,
                mensaje = "ABONO_API_OK"
            });
        }


        // ================================================
        // ACTUALIZAR / GUARDAR ABONO
        // ================================================
        [HttpPost]
        [Route("api/OrdenTrabajo/ActualizarAbono")]
        public async Task<IHttpActionResult> ActualizarAbono(
            [FromBody] AbonoOrdenTrabajoRequest datos)
        {
            if (datos == null)
            {
                return BadRequest(
                    "Los datos del abono son requeridos."
                );
            }

            if (datos.IdOrden <= 0)
            {
                return BadRequest(
                    "El IdOrden no es válido."
                );
            }

            if (datos.AbonadoOT < 0)
            {
                return BadRequest(
                    "El abono no puede ser negativo."
                );
            }

            try
            {
                bool resultado =
                    await _negocio.ActualizarAbonoOrdenTrabajoAsync(
                        datos.IdOrden,
                        datos.AbonadoOT
                    );

                if (!resultado)
                {
                    return BadRequest(
                        "No fue posible actualizar el abono."
                    );
                }

                return Ok(new
                {
                    exito = true,
                    IdOrden = datos.IdOrden,
                    AbonadoOT = datos.AbonadoOT,
                    mensaje = "Abono guardado correctamente."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // ================================================
        // LEER ABONO GUARDADO
        // ================================================
        [HttpGet]
        [Route("api/OrdenTrabajo/LeerAbono/{idOrden:int}")]
        public async Task<IHttpActionResult> LeerAbono(
            int idOrden)
        {
            if (idOrden <= 0)
            {
                return BadRequest(
                    "El IdOrden no es válido."
                );
            }

            try
            {
                decimal abonado =
                    await _negocio.LeerAbonoOrdenTrabajoAsync(
                        idOrden
                    );

                return Ok(new
                {
                    exito = true,
                    IdOrden = idOrden,
                    AbonadoOT = abonado
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        // ==========================================================
        // IMÁGENES O.T.
        // Permite guardar imágenes en una O.T. YA EXISTENTE.
        // ==========================================================
        #region ImagenesOT

        [HttpPost]
        [Route("api/OrdenTrabajo/CrearImagenesOrdenTrabajoAsync")]
        public async Task<IHttpActionResult> CrearImagenesOrdenTrabajoAsync(
            [FromBody] OrdenTrabajoImagenDto imagen)
        {
            if (imagen == null)
            {
                return BadRequest(
                    "Los datos de la imagen son requeridos."
                );
            }

            if (string.IsNullOrWhiteSpace(imagen.Idorden))
            {
                return BadRequest(
                    "El Idorden de la imagen es requerido."
                );
            }

            int idOrden;

            if (
                !int.TryParse(imagen.Idorden, out idOrden) ||
                idOrden <= 0
            )
            {
                return BadRequest(
                    "El Idorden de la imagen no es válido."
                );
            }

            if (string.IsNullOrWhiteSpace(imagen.RutaImagen))
            {
                return BadRequest(
                    "La imagen no contiene información para guardar."
                );
            }

            try
            {
                bool resultado =
                    await _negocio.CrearImagenesOrdenTrabajoAsync(
                        imagen
                    );

                if (!resultado)
                {
                    return InternalServerError(
                        new Exception(
                            "No fue posible guardar la imagen."
                        )
                    );
                }

                return Ok(new
                {
                    exito = true,
                    IdOrden = idOrden,
                    mensaje = "Imagen guardada correctamente."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ==========================================================
        // ELIMINAR IMAGEN DE UNA O.T.
        // ==========================================================
        [HttpPost]
        [Route("api/OrdenTrabajo/ActualizarImagen")]
        public async Task<IHttpActionResult> ActualizarImagen(
            [FromBody] OrdenTrabajoImagenDto imagen)
        {
            if (imagen == null)
            {
                return BadRequest(
                    "Los datos de la imagen son requeridos."
                );
            }

            int idImagen;

            try
            {
                idImagen = Convert.ToInt32(imagen.IdImagen);
            }
            catch
            {
                return BadRequest(
                    "El IdImagen no es válido."
                );
            }

            return await ActualizarImagenInterna(
                idImagen,
                imagen
            );
        }

        [HttpPost]
        [Route("api/OrdenTrabajo/ActualizarImagen/{idImagen:int}")]
        public async Task<IHttpActionResult> ActualizarImagenPorId(
            int idImagen,
            [FromBody] OrdenTrabajoImagenDto imagen)
        {
            return await ActualizarImagenInterna(
                idImagen,
                imagen
            );
        }

        private async Task<IHttpActionResult> ActualizarImagenInterna(
            int idImagen,
            OrdenTrabajoImagenDto imagen)
        {
            if (idImagen <= 0)
            {
                return BadRequest(
                    "El IdImagen no es válido."
                );
            }

            if (imagen == null)
            {
                return BadRequest(
                    "Los datos de la imagen son requeridos."
                );
            }

            if (string.IsNullOrWhiteSpace(imagen.RutaImagen))
            {
                return BadRequest(
                    "La imagen no contiene información para actualizar."
                );
            }

            try
            {
                bool resultado =
                    await _negocio
                        .ActualizarImagenOrdenTrabajoAsync(
                            idImagen,
                            imagen.RutaImagen
                        );

                if (!resultado)
                {
                    return NotFound();
                }

                return Ok(new
                {
                    exito = true,
                    IdImagen = idImagen,
                    mensaje = "Imagen actualizada correctamente."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/OrdenTrabajo/EliminarImagen/{idImagen:int}")]
        public async Task<IHttpActionResult> EliminarImagen(
            int idImagen)
        {
            if (idImagen <= 0)
            {
                return BadRequest(
                    "El IdImagen no es válido."
                );
            }

            try
            {
                bool resultado =
                    await _negocio.EliminarImagenOrdenTrabajoAsync(
                        idImagen
                    );

                if (!resultado)
                {
                    return NotFound();
                }

                return Ok(new
                {
                    exito = true,
                    IdImagen = idImagen,
                    mensaje = "Imagen eliminada correctamente."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion
    }
}