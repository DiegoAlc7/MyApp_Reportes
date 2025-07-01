using MyApp.DataAccess.Data;
using MyApp.DataAccess.IRepositories;
using MyApp.Entities;

namespace MyApp.DataAccess.Repositories
{
    public class ArtículoRepository : IArtículoRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtículoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener artículo por ID
        public Artículo ObtenerPorId(int id)
        {
            return _context.Artículos.FirstOrDefault(a => a.Id == id);
        }

        // Obtener artículo por código
        public Artículo ObtenerPorCódigo(string código)
        {
            return _context.Artículos.FirstOrDefault(a => a.Código == código);
        }

        // Obtener todos los artículos
        public IEnumerable<Artículo> ObtenerTodos()
        {
            return _context.Artículos.ToList();
        }

        // Filtrar artículos por categoría
        public IEnumerable<Artículo> FiltrarPorCategoría(string categoría)
        {
            return _context.Artículos.Where(a => a.Categoría == categoría).ToList();
        }

        // Buscar artículos por nombre o código
        public IEnumerable<Artículo> BuscarArtículos(string término)
        {
            return _context.Artículos
                .Where(a => a.Nombre.Contains(término) || a.Código.Contains(término))
                .ToList();
        }

        // Verificar si un artículo tiene préstamos activos
        public bool TienePréstamosActivos(int artículoId)
        {
            return _context.Préstamos.Any(p => p.ArtículoId == artículoId && p.Estado != "Devuelto");
        }

        // Crear un nuevo artículo
        public void Crear(Artículo artículo)
        {
            _context.Artículos.Add(artículo);
            _context.SaveChanges();
        }

        // Actualizar un artículo existente
        public void Actualizar(Artículo artículo)
        {
            _context.Artículos.Update(artículo);
            _context.SaveChanges();
        }

        // Eliminar un artículo por ID
        public void Eliminar(int id)
        {
            var artículo = _context.Artículos.Find(id);
            if (artículo != null)
            {
                _context.Artículos.Remove(artículo);
                _context.SaveChanges();
            }
        }
    }
}
