using MyApp.DataAccess.IRepositories;
using MyApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Business.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // Registrar un nuevo usuario con rol "Operador" por defecto
        public void RegistrarUsuario(string nombre, string email, string contraseña)
        {
            if (_usuarioRepository.ObtenerPorEmail(email) != null)
            {
                throw new Exception("El email ya está registrado.");
            }

            var usuario = new Usuario
            {
                Nombre = nombre,
                Email = email,
                Contraseña = BCrypt.Net.BCrypt.HashPassword(contraseña),
                RolId = 2 // Rol por defecto: Operador
            };

            _usuarioRepository.Crear(usuario);
        }

        // Autenticar usuario
        public bool AutenticarUsuario(string email, string contraseña)
        {
            var usuario = _usuarioRepository.ObtenerPorEmail(email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(contraseña, usuario.Contraseña))
            {
                return false;
            }
            return true;
        }

        // Cambiar rol de un usuario (solo administradores pueden hacerlo)
        public void CambiarRol(int usuarioId, int rolId)
        {
            var usuario = _usuarioRepository.ObtenerPorId(usuarioId);
            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            usuario.RolId = rolId;
            _usuarioRepository.Actualizar(usuario);
        }

        // Obtener usuario por email
        public Usuario ObtenerPorEmail(string email)
        {
            return _usuarioRepository.ObtenerPorEmail(email);
        }

        // Listar todos los usuarios
        public IEnumerable<Usuario> ObtenerTodos()
        {
            return _usuarioRepository.ObtenerTodos();
        }

        public Usuario ObtenerPorEmailYContraseña(string email, string contraseña)
        {
            var usuario = _usuarioRepository.ObtenerPorEmail(email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(contraseña, usuario.Contraseña))
            {
                throw new Exception("Credenciales incorrectas.");
            }
            return usuario;
        }
    }
}
