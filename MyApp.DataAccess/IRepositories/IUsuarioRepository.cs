using MyApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccess.IRepositories
{
    public interface IUsuarioRepository
    {
        Usuario ObtenerPorId(int id);
        Usuario ObtenerPorEmail(string email);
        Usuario ObtenerPorEmailYContraseña(string email, string contraseña);
        IEnumerable<Usuario> ObtenerTodos();
        void Crear(Usuario usuario);
        void Actualizar(Usuario usuario);
        void Eliminar(int id);
    }
}
