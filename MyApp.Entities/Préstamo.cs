using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Entities
{
    public class Préstamo
    {
        public int Id { get; set; }
        public int ArtículoId { get; set; }
        public Artículo? Artículo { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public DateTime? FechaDevolución { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
