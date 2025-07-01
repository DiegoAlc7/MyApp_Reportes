using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Presentation.Controllers
{
    [Authorize(Roles = "Operador")]
    public class OperadorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
