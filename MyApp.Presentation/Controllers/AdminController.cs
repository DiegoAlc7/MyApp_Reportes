using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Business.Services;

namespace MyApp.Presentation.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public AdminController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // Página principal del administrador
        public IActionResult Index()
        {
            return View();
        }

        // Cambiar el rol de un usuario
        [HttpPost]
        public IActionResult CambiarRol(int usuarioId, int rolId)
        {
            try
            {
                _usuarioService.CambiarRol(usuarioId, rolId);
                ViewBag.Mensaje = "Rol cambiado exitosamente.";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return RedirectToAction("ListaUsuarios");
        }

        // Listar todos los usuarios
        public IActionResult ListaUsuarios()
        {
            var usuarios = _usuarioService.ObtenerTodos();
            return View(usuarios);
        }
    }
}
