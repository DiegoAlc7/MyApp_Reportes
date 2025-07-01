using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
        public int RolId { get; set; }
        public Rol? Rol { get; set; }
        public ICollection<Préstamo> Préstamos { get; set; } = new List<Préstamo>();
    }
}
