using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Business.Services;
using MyApp.Entities;
using System.Security.Claims;

namespace MyApp.Presentation.Controllers
{
    [AllowAnonymous] // Permite acceso sin autenticación
    public class LoginController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public LoginController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // Mostrar formulario de inicio de sesión
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // Procesar inicio de sesión
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string contraseña)
        {
            try
            {
                // Autenticar al usuario
                var usuario = _usuarioService.ObtenerPorEmailYContraseña(email, contraseña);

                if (usuario == null)
                {
                    ViewBag.Error = "Credenciales incorrectas.";
                    return View();
                }

                if (usuario.Rol == null)
                {
                    ViewBag.Error = "El usuario no tiene un rol asignado.";
                    return View();
                }

                // Crear claims para el usuario
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Crear la cookie de autenticación
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                // Redirigir a la página principal
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error inesperado: {ex.Message}";
                return View();
            }
        }

        // Cerrar sesión
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}