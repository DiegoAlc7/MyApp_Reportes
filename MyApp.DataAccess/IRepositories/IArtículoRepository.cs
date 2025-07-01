using MyApp.Entities;

namespace MyApp.DataAccess.IRepositories
{
    public interface IArtículoRepository
    {
        Artículo ObtenerPorId(int id);
        Artículo ObtenerPorCódigo(string código);
        IEnumerable<Artículo> ObtenerTodos();
        IEnumerable<Artículo> FiltrarPorCategoría(string categoría);
        IEnumerable<Artículo> BuscarArtículos(string término);
        bool TienePréstamosActivos(int artículoId);
        void Crear(Artículo artículo);
        void Actualizar(Artículo artículo);
        void Eliminar(int id);
    }
}
