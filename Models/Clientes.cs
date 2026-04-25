using System.ComponentModel.DataAnnotations;

namespace PlataformaCreditos.Models;

public class Cliente
{
    public int Id { get; set; }

    
    public string UsuarioId { get; set; } = string.Empty;

    [Range(1, double.MaxValue, ErrorMessage = "Ingresos deben ser mayores a 0")]
    public decimal IngresosMensuales { get; set; }

    public bool Activo { get; set; }

    
    public ICollection<SolicitudCredito> Solicitudes { get; set; } = new List<SolicitudCredito>();
}

