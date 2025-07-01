using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Business.Services;

namespace MyApp.Presentation.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(string nombre, string email, string contraseña)
        {
            try
            {
                _usuarioService.RegistrarUsuario(nombre, email, contraseña);
                ViewBag.Mensaje = "Usuario registrado exitosamente.";
                return RedirectToAction("ListaUsuarios");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        public IActionResult AsignarRol(int usuarioId, int rolId)
        {
            try
            {
                _usuarioService.CambiarRol(usuarioId, rolId);
                ViewBag.Mensaje = "Rol asignado exitosamente.";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return RedirectToAction("ListaUsuarios");
        }

        public IActionResult ListaUsuarios()
        {
            var usuarios = _usuarioService.ObtenerTodos();
            return View(usuarios);
        }
    }
}
