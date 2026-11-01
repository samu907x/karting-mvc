using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace KartingMvc.Models;


public class ResultadoViewModel
{
    [Required(ErrorMessage = "Selecciona un piloto.")]
    public int PilotoId { get; set; }

    [Range(1, 10, ErrorMessage = "El puesto debe estar entre 1 y 10.")]
    public int Puesto { get; set; } = 1;

  
    [ValidateNever]
    public List<Piloto> Pilotos { get; set; } = new();

    [ValidateNever]
    public int[] PuntosPorPuesto { get; set; } = Array.Empty<int>();
}
