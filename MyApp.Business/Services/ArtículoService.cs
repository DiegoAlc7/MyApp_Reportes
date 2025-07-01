using MyApp.DataAccess.IRepositories;
using MyApp.Entities;

namespace MyApp.Business.Services
{
    public class ArtículoService
    {
        private readonly IArtículoRepository _artículoRepository;

        public ArtículoService(IArtículoRepository artículoRepository)
        {
            _artículoRepository = artículoRepository;
        }

        // Crear un nuevo artículo
        public void CrearArtículo(string código, string nombre, string categoría, string estado, string ubicación)
        {
            if (_artículoRepository.ObtenerPorCódigo(código) != null)
            {
                throw new Exception("El código del artículo ya existe.");
            }

            var artículo = new Artículo
            {
                Código = código,
                Nombre = nombre,
                Categoría = categoría,
                Estado = estado,
                Ubicación = ubicación
            };

            _artículoRepository.Crear(artículo);
        }

        // Actualizar un artículo (excepto el código)
        public void ActualizarArtículo(int id, string código, string nombre, string categoría, string estado, string ubicación)
        {
            var artículo = _artículoRepository.ObtenerPorId(id);
            if (artículo == null)
            {
                throw new Exception("Artículo no encontrado.");
            }

            artículo.Nombre = nombre;
            artículo.Código = código;
            artículo.Categoría = categoría;
            artículo.Estado = estado;
            artículo.Ubicación = ubicación;

            _artículoRepository.Actualizar(artículo);
        }

        // Eliminar un artículo solo si no tiene préstamos activos
        public void EliminarArtículo(int id)
        {
            var artículo = _artículoRepository.ObtenerPorId(id);
            if (artículo == null)
            {
                throw new Exception("Artículo no encontrado.");
            }

            // Verificar si el artículo tiene préstamos activos
            if (_artículoRepository.TienePréstamosActivos(id))
            {
                throw new Exception("No se puede eliminar un artículo con préstamos activos.");
            }

            _artículoRepository.Eliminar(id);
        }

        // Obtener todos los artículos
        public IEnumerable<Artículo> ObtenerTodos()
        {
            return _artículoRepository.ObtenerTodos();
        }

        // Filtrar artículos por categoría
        public IEnumerable<Artículo> FiltrarPorCategoría(string categoría)
        {
            return _artículoRepository.FiltrarPorCategoría(categoría);
        }

        // Buscar artículos por nombre o código
        public IEnumerable<Artículo> BuscarArtículos(string término)
        {
            return _artículoRepository.BuscarArtículos(término);
        }
        public Artículo ObtenerPorId(int id)
        {
            return _artículoRepository.ObtenerPorId(id);
        }
    }
}
