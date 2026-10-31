using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KartingMvc.Models;

namespace KartingMvc.Controllers;

// CONTROLADOR de la página de inicio
public class HomeController : Controller
{
    // GET: /  o  /Home/Index  -> devuelve la vista Views/Home/Index.cshtml
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
