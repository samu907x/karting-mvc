using Microsoft.AspNetCore.Mvc;
using KartingMvc.Data;
using KartingMvc.Models;

namespace KartingMvc.Controllers;


public class PilotosController : Controller
{
   
    public IActionResult Index(string? categoria)
    {
        var pilotos = RepositorioPilotos.ObtenerTodos(categoria);
        ViewBag.Categoria = string.IsNullOrEmpty(categoria) ? "Todas" : categoria;
        return View(pilotos);
    }

  
    public IActionResult Details(int id)
    {
        var piloto = RepositorioPilotos.ObtenerPorId(id);
        if (piloto == null)
        {
            return NotFound();
        }
        return View(piloto);
    }

   
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

   
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Piloto piloto)
    {
     
        if (!string.IsNullOrWhiteSpace(piloto.Nombre) && RepositorioPilotos.ExisteNombre(piloto.Nombre))
        {
            ModelState.AddModelError(nameof(Piloto.Nombre), "Ya existe un piloto con ese nombre.");
        }

       
        if (!ModelState.IsValid)
        {
            return View(piloto);
        }

        RepositorioPilotos.Agregar(piloto);
        TempData["Mensaje"] = $"{piloto.Nombre} quedó inscrito en la categoría {piloto.Categoria}.";
        return RedirectToAction(nameof(Index));
    }

  
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var piloto = RepositorioPilotos.ObtenerPorId(id);
        if (piloto != null && RepositorioPilotos.Eliminar(id))
        {
            TempData["Mensaje"] = $"{piloto.Nombre} fue retirado del campeonato.";
        }
        return RedirectToAction(nameof(Index));
    }

   
    [HttpGet]
    public IActionResult Resultado()
    {
        return View(CrearModeloResultado(new ResultadoViewModel()));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Resultado(ResultadoViewModel modelo)
    {
        var piloto = RepositorioPilotos.ObtenerPorId(modelo.PilotoId);
        if (piloto == null)
        {
            ModelState.AddModelError(nameof(modelo.PilotoId), "El piloto seleccionado no existe.");
        }

        if (!ModelState.IsValid)
        {
            return View(CrearModeloResultado(modelo));
        }

        RepositorioPilotos.SumarPuntos(modelo.PilotoId, modelo.Puesto);
        int puntos = RepositorioPilotos.PuntosPorPuesto[modelo.Puesto - 1];
        TempData["Mensaje"] = $"{piloto!.Nombre} suma {puntos} puntos por su puesto {modelo.Puesto}.";
        return RedirectToAction(nameof(Index));
    }

    
    private static ResultadoViewModel CrearModeloResultado(ResultadoViewModel modelo)
    {
        modelo.Pilotos = RepositorioPilotos.ObtenerTodos();
        modelo.PuntosPorPuesto = RepositorioPilotos.PuntosPorPuesto;
        return modelo;
    }
}
