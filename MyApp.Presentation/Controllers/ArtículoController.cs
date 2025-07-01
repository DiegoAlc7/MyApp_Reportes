using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Business.Services;

namespace MyApp.Presentation.Controllers
{
    public class ArtículoController : Controller
    {
        private readonly ArtículoService _artículoService;

        public ArtículoController(ArtículoService artículoService)
        {
            _artículoService = artículoService;
        }

        // Crear un nuevo artículo
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear(string código, string nombre, string categoría, string estado, string ubicación)
        {
            try
            {
                _artículoService.CrearArtículo(código, nombre, categoría, estado, ubicación);
                ViewBag.Mensaje = "Artículo creado exitosamente.";
                return RedirectToAction("ListaArtículos");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        // Editar un artículo existente
        public IActionResult Editar(int id)
        {
            var artículo = _artículoService.ObtenerPorId(id);
            if (artículo == null)
            {
                return NotFound("El articulo no fue encontrado.");
            }
            return View(artículo);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id, string código, string nombre, string categoría, string estado, string ubicación)
        {
            try
            {

                _artículoService.ActualizarArtículo(id, código, nombre, categoría, estado, ubicación);
                ViewBag.Mensaje = "Artículo actualizado exitosamente.";
                return RedirectToAction("ListaArtículos");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                var articulo = _artículoService.ObtenerPorId(id);
                return View(articulo);
            }
        }

        // Eliminar un artículo
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                _artículoService.EliminarArtículo(id);
                ViewBag.Mensaje = "Artículo eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return RedirectToAction("ListaArtículos");
        }

        // Listar artículos con paginación y filtros
        public IActionResult ListaArtículos(string? categoría = null, string? estado = null, string? búsqueda = null, int página = 1)
        {
            var artículos = _artículoService.ObtenerTodos();

            // Filtrado
            if (!string.IsNullOrEmpty(categoría))
                artículos = artículos.Where(a => a.Categoría == categoría);

            if (!string.IsNullOrEmpty(estado))
                artículos = artículos.Where(a => a.Estado == estado);

            if (!string.IsNullOrEmpty(búsqueda))
                artículos = artículos.Where(a => a.Nombre.Contains(búsqueda) || a.Código.Contains(búsqueda));

            // Paginación
            int elementosPorPágina = 10;
            var artículosPaginados = artículos.Skip((página - 1) * elementosPorPágina).Take(elementosPorPágina).ToList();

            ViewBag.PáginaActual = página;
            ViewBag.TotalPáginas = (int)Math.Ceiling(artículos.Count() / (double)elementosPorPágina);

            return View(artículosPaginados);
        }
    }
}
