using Microsoft.AspNetCore.Mvc;
using KartingMvc.Data;
using KartingMvc.Models;

namespace KartingMvc.Controllers;

// CONTROLADOR: recibe las solicitudes, trabaja con el modelo y elige la vista.
public class PilotosController : Controller
{
    // GET: /Pilotos?categoria=Juvenil  -> lista de pilotos (con filtro opcional)
    public IActionResult Index(string? categoria)
    {
        var pilotos = RepositorioPilotos.ObtenerTodos(categoria);
        ViewBag.Categoria = string.IsNullOrEmpty(categoria) ? "Todas" : categoria;
        return View(pilotos);
    }

    // GET: /Pilotos/Details/3  -> detalle de un piloto
    public IActionResult Details(int id)
    {
        var piloto = RepositorioPilotos.ObtenerPorId(id);
        if (piloto == null)
        {
            return NotFound();
        }
        return View(piloto);
    }

    // GET: /Pilotos/Create  -> muestra el formulario vacío
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Pilotos/Create  -> recibe el formulario y lo procesa
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Piloto piloto)
    {
        // Regla propia: no repetir nombres
        if (!string.IsNullOrWhiteSpace(piloto.Nombre) && RepositorioPilotos.ExisteNombre(piloto.Nombre))
        {
            ModelState.AddModelError(nameof(Piloto.Nombre), "Ya existe un piloto con ese nombre.");
        }

        // Si hay errores, se vuelve a mostrar el formulario con los mensajes
        if (!ModelState.IsValid)
        {
            return View(piloto);
        }

        RepositorioPilotos.Agregar(piloto);
        TempData["Mensaje"] = $"{piloto.Nombre} quedó inscrito en la categoría {piloto.Categoria}.";
        return RedirectToAction(nameof(Index));
    }

    // POST: /Pilotos/Delete/3  -> quita un piloto
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

    // GET: /Pilotos/Resultado  -> formulario para registrar el resultado de una carrera
    [HttpGet]
    public IActionResult Resultado()
    {
        return View(CrearModeloResultado(new ResultadoViewModel()));
    }

    // POST: /Pilotos/Resultado  -> calcula y suma los puntos
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

    // Llena las listas que necesita la vista del formulario de resultados
    private static ResultadoViewModel CrearModeloResultado(ResultadoViewModel modelo)
    {
        modelo.Pilotos = RepositorioPilotos.ObtenerTodos();
        modelo.PuntosPorPuesto = RepositorioPilotos.PuntosPorPuesto;
        return modelo;
    }
}
