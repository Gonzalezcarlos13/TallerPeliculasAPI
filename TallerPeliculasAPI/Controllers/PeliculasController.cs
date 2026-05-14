using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;

namespace Taller.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PeliculasController : ApiController
    {
        // Cambia el nombre del Data Source por el que aparece en tu SQL Management Studio
        string cadena = "Data Source=Gonzalecarlos13;Initial Catalog=Taller;Integrated Security=True";

        [HttpGet]
        [Route("api/peliculas")]
        public IHttpActionResult GetPeliculas(string s = null)
        {
            // SI HAY BÚSQUEDA: Vamos a la API de OMDb
            if (!string.IsNullOrEmpty(s))
            {
                string apiKey = "5ef9ae45";
                string url = $"https://www.omdbapi.com/?s={s}&apikey={apiKey}";

                using (WebClient web = new WebClient())
                {
                    var json = web.DownloadString(url);
                    dynamic datos = JsonConvert.DeserializeObject(json);
                    
                    // IMPORTANTE: Devolvemos solo la lista que está dentro de "Search"
                    return Ok(datos.Search);
                }
            }

            // SI NO HAY BÚSQUEDA: Vamos a tu SQL
            List<object> lista = new List<object>();
            using (SqlConnection conn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_peliculas", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader leer = cmd.ExecuteReader();
                while (leer.Read())
                {
                    lista.Add(new {
                        id = leer["id"],
                        Title = leer["titulo"].ToString(),
                        Year = leer["anio"].ToString(),
                        Poster = leer["poster"].ToString()
                    });
                }
            }
            return Ok(lista);
        }

        // CREATE: Para GUARDAR una película nueva
        [HttpPost]
        [Route("api/peliculas")]
        public IHttpActionResult PostPelicula(dynamic nueva)
        {
            using (SqlConnection conn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_crear_pelicula", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@titulo", (string)nueva.Title);
                cmd.Parameters.AddWithValue("@anio", (string)nueva.Year);
                cmd.Parameters.AddWithValue("@poster", (string)nueva.Poster);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return Ok("¡Película guardada!");
        }

        // DELETE: Para BORRAR una película de tu tabla
        [HttpDelete]
        [Route("api/peliculas/{id}")]
        public IHttpActionResult DeletePelicula(int id)
        {
            using (SqlConnection conn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_borrar_pelicula", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return Ok("Eliminada correctamente");
        }
    }
}