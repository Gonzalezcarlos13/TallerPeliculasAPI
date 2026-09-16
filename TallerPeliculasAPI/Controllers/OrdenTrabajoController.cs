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


                if (idot>0)
                {
                    return Ok(new { Id= idot, exito = true, mensaje = "Orden de trabajo creada correctamente." });
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
    }
}