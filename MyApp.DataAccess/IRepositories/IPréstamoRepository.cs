using MyApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccess.IRepositories
{
    public interface IPréstamoRepository
    {
        Préstamo ObtenerPorId(int id);
        IEnumerable<Préstamo> ObtenerTodos();
        IEnumerable<Préstamo> FiltrarPorEstado(string estado);
        IEnumerable<Préstamo> FiltrarPorUsuario(int usuarioId);
        void Crear(Préstamo préstamo);
        void Actualizar(Préstamo préstamo);
        void Eliminar(int id);
    }
}
