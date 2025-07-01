using MyApp.DataAccess.IRepositories;
using MyApp.Entities;

namespace MyApp.Business.Services
{
    public class PréstamoService
    {
        private readonly IPréstamoRepository _préstamoRepository;
        private readonly IArtículoRepository _artículoRepository;

        public PréstamoService(IPréstamoRepository préstamoRepository, IArtículoRepository artículoRepository)
        {
            _préstamoRepository = préstamoRepository;
            _artículoRepository = artículoRepository;
        }

        // Solicitar un préstamo (Operador)
        public void SolicitarPréstamo(int artículoId, int usuarioId)
        {
            var artículo = _artículoRepository.ObtenerPorId(artículoId);
            if (artículo == null || artículo.Estado != "Disponible")
            {   
                throw new Exception("El artículo no está disponible.");
            }

            var préstamo = new Préstamo
            {
                ArtículoId = artículoId,
                UsuarioId = usuarioId,
                FechaSolicitud = DateTime.Now,
                Estado = "Pendiente"
            };

            _préstamoRepository.Crear(préstamo);
            artículo.Estado = "Reservado";
            _artículoRepository.Actualizar(artículo);
        }

        // Aprobar un préstamo (Administrador)
        public void AprobarPréstamo(int préstamoId)
        {
            var préstamo = _préstamoRepository.ObtenerPorId(préstamoId);
            if (préstamo == null || préstamo.Estado != "Pendiente")
            {
                throw new Exception("El préstamo no puede ser aprobado.");
            }

            préstamo.Estado = "Aprobado";
            préstamo.FechaRespuesta = DateTime.Now;
            _préstamoRepository.Actualizar(préstamo);

            var artículo = _artículoRepository.ObtenerPorId(préstamo.ArtículoId);
            artículo.Estado = "En Uso";
            _artículoRepository.Actualizar(artículo);
        }

        // Rechazar un préstamo (solo administrador)
        public void RechazarPréstamo(int préstamoId)
        {
            var préstamo = _préstamoRepository.ObtenerPorId(préstamoId);
            if (préstamo == null || préstamo.Estado != "Pendiente")
            {
                throw new Exception("El préstamo no puede ser rechazado.");
            }

            préstamo.Estado = "Rechazado";
            préstamo.FechaRespuesta = DateTime.Now;

            var artículo = _artículoRepository.ObtenerPorId(préstamo.ArtículoId);
            artículo.Estado = "Disponible";
            _artículoRepository.Actualizar(artículo);

            _préstamoRepository.Actualizar(préstamo);
        }

        // Registrar devolución de un préstamo (administrador)
        public void RegistrarDevolución(int préstamoId)
        {
            var préstamo = _préstamoRepository.ObtenerPorId(préstamoId);
            if (préstamo == null || préstamo.Estado != "Aprobado")
            {
                throw new Exception("El préstamo no puede ser devuelto.");
            }

            préstamo.FechaDevolución = DateTime.Now;
            préstamo.Estado = "Devuelto";

            var artículo = _artículoRepository.ObtenerPorId(préstamo.ArtículoId);
            artículo.Estado = "Disponible";
            _artículoRepository.Actualizar(artículo);

            _préstamoRepository.Actualizar(préstamo);   
        }

        // Obtener todos los préstamos (Administrador)
        public IEnumerable<Préstamo> ObtenerTodos()
        {
            return _préstamoRepository.ObtenerTodos();
        }

        // Filtrar préstamos por estado (Administrador)
        public IEnumerable<Préstamo> FiltrarPorEstado(string estado)
        {
            return _préstamoRepository.FiltrarPorEstado(estado);
        }

        // Filtrar préstamos por usuario (Operador o Administrador)
        public IEnumerable<Préstamo> FiltrarPorUsuario(int usuarioId)
        {
            return _préstamoRepository.FiltrarPorUsuario(usuarioId);
        }

        // Obtener todos los préstamos solicitados por un usuario (solo operador)
        public IEnumerable<Préstamo> ObtenerPréstamosPorUsuario(int usuarioId)
        {
            return _préstamoRepository.ObtenerTodos().Where(p => p.UsuarioId == usuarioId);
        }
    }
}
