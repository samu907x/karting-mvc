using KartingMvc.Models;

namespace KartingMvc.Data;


public static class RepositorioPilotos
{
    private static readonly object _candado = new();
    private static int _siguienteId = 6;

    
    public static readonly int[] PuntosPorPuesto = { 25, 18, 15, 12, 10, 8, 6, 4, 2, 1 };

    private static readonly List<Piloto> _pilotos = new()
    {
        new Piloto { Id = 1, Nombre = "Valeria Hernández Castro", Correo = "valeria@ejemplo.com", Edad = 22, Motor = "2 Tiempos", Puntos = 25 },
        new Piloto { Id = 2, Nombre = "Fernando Alonso",          Correo = "fernando@ejemplo.com", Edad = 44, Motor = "2 Tiempos", Puntos = 18 },
        new Piloto { Id = 3, Nombre = "Samuel Ramírez",           Correo = "samuel@ejemplo.com",   Edad = 14, Motor = "4 Tiempos", Puntos = 15 },
        new Piloto { Id = 4, Nombre = "Camila Ortiz",             Correo = "camila@ejemplo.com",   Edad = 10, Motor = "4 Tiempos", Puntos = 12 },
        new Piloto { Id = 5, Nombre = "Juan Pérez",               Correo = "juan@ejemplo.com",     Edad = 13, Motor = "2 Tiempos", Puntos = 10 }
    };

   
    public static List<Piloto> ObtenerTodos(string? categoria = null)
    {
        lock (_candado)
        {
            IEnumerable<Piloto> consulta = _pilotos;

            if (!string.IsNullOrEmpty(categoria) && categoria != "Todas")
            {
                consulta = consulta.Where(p => p.Categoria == categoria);
            }

            return consulta.OrderByDescending(p => p.Puntos).ThenBy(p => p.Nombre).ToList();
        }
    }

    public static Piloto? ObtenerPorId(int id)
    {
        lock (_candado)
        {
            return _pilotos.FirstOrDefault(p => p.Id == id);
        }
    }

    public static bool ExisteNombre(string nombre)
    {
        lock (_candado)
        {
            return _pilotos.Any(p => p.Nombre.Equals(nombre.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }

    public static void Agregar(Piloto piloto)
    {
        lock (_candado)
        {
            piloto.Id = _siguienteId++;
            piloto.Nombre = piloto.Nombre.Trim();
            piloto.Puntos = 0;
            _pilotos.Add(piloto);
        }
    }

    public static bool Eliminar(int id)
    {
        lock (_candado)
        {
            var piloto = _pilotos.FirstOrDefault(p => p.Id == id);
            return piloto != null && _pilotos.Remove(piloto);
        }
    }

 
    public static bool SumarPuntos(int id, int puesto)
    {
        lock (_candado)
        {
            var piloto = _pilotos.FirstOrDefault(p => p.Id == id);
            if (piloto == null || puesto < 1 || puesto > PuntosPorPuesto.Length)
            {
                return false;
            }

            piloto.Puntos += PuntosPorPuesto[puesto - 1];
            return true;
        }
    }
}
