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
        [Route("Actualizar")]
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
    }
}