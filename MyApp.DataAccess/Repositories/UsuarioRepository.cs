using Microsoft.EntityFrameworkCore;
using MyApp.DataAccess.Data;
using MyApp.DataAccess.IRepositories;
using MyApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccess.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener usuario por ID
        public Usuario ObtenerPorId(int id)
        {
            return _context.Usuarios.Include(u => u.Rol).FirstOrDefault(u => u.Id == id);
        }

        // Obtener usuario por email
        public Usuario ObtenerPorEmail(string email)
        {
            return _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefault(u => u.Email == email);
        }

        // Obtener usuario por email y contraseña
        public Usuario ObtenerPorEmailYContraseña(string email, string contraseña)
        {
            return _context.Usuarios
                .Include(u => u.Rol) // Incluir la relación con Rol
                .FirstOrDefault(u => u.Email == email && u.Contraseña == contraseña);
        }

        // Obtener todos los usuarios
        public IEnumerable<Usuario> ObtenerTodos()
        {
            return _context.Usuarios.Include(u => u.Rol).ToList();
        }

        // Crear un nuevo usuario
        public void Crear(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        // Actualizar un usuario existente
        public void Actualizar(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
        }

        // Eliminar un usuario por ID
        public void Eliminar(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }
        }
    }
}
