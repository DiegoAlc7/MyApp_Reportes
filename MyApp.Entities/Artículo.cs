using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Entities
{
    public class Artículo
    {
        public int Id { get; set; }
        public string Código { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Categoría { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Ubicación { get; set; } = string.Empty;
        public ICollection<Préstamo> Préstamos { get; set; } = new List<Préstamo>();
    }
}
