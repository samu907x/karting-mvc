using System.ComponentModel.DataAnnotations;

namespace KartingMvc.Models;


public class Piloto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(60, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 60 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Escribe un correo válido.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La edad es obligatoria.")]
    [Range(7, 60, ErrorMessage = "La edad debe estar entre 7 y 60 años.")]
    public int? Edad { get; set; }

    [Required(ErrorMessage = "Selecciona el tipo de motor.")]
    public string Motor { get; set; } = string.Empty;

    public int Puntos { get; set; }

  
    [Range(typeof(bool), "true", "true", ErrorMessage = "Debes aceptar el reglamento.")]
    public bool AceptaReglamento { get; set; }

  
    public string Categoria => Edad switch
    {
        >= 7 and <= 11 => "Infantil",
        >= 12 and <= 15 => "Juvenil",
        >= 16 => "Pro / Libre",
        _ => "Sin categoría"
    };
}
