using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Business.Services;
using MyApp.Entities;
using System.Linq;

namespace MyApp.Presentation.Controllers
{
    [Authorize]
    public class PréstamoController : Controller
    {
        private readonly PréstamoService _préstamoService;
        private readonly ArtículoService _artículoService;

        public PréstamoController(PréstamoService préstamoService, ArtículoService artículoService)
        {
            _préstamoService = préstamoService;
            _artículoService = artículoService;
        }

        // Solicitar un préstamo (Operador) - GET
        [HttpGet]
        [Authorize(Roles = "Operador")]
        public IActionResult Solicitar()
        {
            try
            {
                // Obtener todos los artículos disponibles
                var artículosDisponibles = _artículoService.ObtenerTodos()?.Where(a => a.Estado == "Disponible").ToList();

                // Si no hay artículos disponibles, pasa una lista vacía en lugar de null
                if (artículosDisponibles == null)
                {
                    artículosDisponibles = new List<Artículo>();
                }

                return View(artículosDisponibles);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // Solicitar un préstamo (Operador) - POST
        [HttpPost]
        [Authorize(Roles = "Operador")]
        public IActionResult Solicitar(int artículoId)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim == null || string.IsNullOrEmpty(claim.Value) || !int.TryParse(claim.Value, out var usuarioId))
            {
                TempData["Error"] = "No se pudo identificar al usuario autenticado.";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                _préstamoService.SolicitarPréstamo(artículoId, usuarioId);
                TempData["Mensaje"] = "Préstamo solicitado exitosamente.";
                return RedirectToAction("Historial");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException?.Message ?? ex.Message;
                return View();
            }
        }

        // Ver historial de préstamos (Operador)
        [Authorize(Roles = "Operador")]
        public IActionResult Historial()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim == null || string.IsNullOrEmpty(claim.Value) || !int.TryParse(claim.Value, out var usuarioId))
            {
                TempData["Error"] = "No se pudo identificar al usuario autenticado.";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var historial = _préstamoService.FiltrarPorUsuario(usuarioId);
                return View(historial);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // Aprobar un préstamo (Administrador)
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Aprobar(int préstamoId)
        {
            try
            {
                _préstamoService.AprobarPréstamo(préstamoId);
                TempData["Mensaje"] = "Préstamo aprobado exitosamente.";
                return RedirectToAction("ListaPréstamos");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("ListaPréstamos");
            }
        }

        // Rechazar un préstamo (Administrador)
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Rechazar(int préstamoId)
        {
            try
            {
                _préstamoService.RechazarPréstamo(préstamoId);
                TempData["Mensaje"] = "Préstamo rechazado exitosamente.";
                return RedirectToAction("ListaPréstamos");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("ListaPréstamos");
            }
        }

        // Registrar devolución de un préstamo (Administrador)
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Devolver(int préstamoId)
        {
            try
            {
                _préstamoService.RegistrarDevolución(préstamoId);
                TempData["Mensaje"] = "Préstamo devuelto exitosamente.";
                return RedirectToAction("ListaPréstamos");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("ListaPréstamos");
            }
        }

        // Listar préstamos con filtros (Administrador)
        [Authorize(Roles = "Administrador")]
        public IActionResult ListaPréstamos(string usuario = null, string artículo = null, string estado = null)
        {
            try
            {
                // Obtener todos los préstamos
                var préstamos = _préstamoService.ObtenerTodos().AsQueryable();

                // Aplicar filtros opcionales
                if (!string.IsNullOrEmpty(usuario))
                    préstamos = préstamos.Where(p => p.Usuario.Nombre.Contains(usuario, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(artículo))
                    préstamos = préstamos.Where(p => p.Artículo.Nombre.Contains(artículo, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(estado))
                    préstamos = préstamos.Where(p => p.Estado.Equals(estado, StringComparison.OrdinalIgnoreCase));

                return View(préstamos.ToList());
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // Ver préstamos pendientes (Administrador)
        [Authorize(Roles = "Administrador")]
        public IActionResult PréstamosPendientes()
        {
            try
            {
                var préstamosPendientes = _préstamoService.FiltrarPorEstado("Pendiente");
                return View(préstamosPendientes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}