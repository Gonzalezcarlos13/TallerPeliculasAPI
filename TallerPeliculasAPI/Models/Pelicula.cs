using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TallerPeliculasAPI.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Poster { get; set; }
    }
}