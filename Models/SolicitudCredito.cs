using System.ComponentModel.DataAnnotations;

namespace PlataformaCreditos.Models;

public enum EstadoSolicitud
{
    Pendiente = 0,
    Aprobado = 1,
    Rechazado = 2
}

public class SolicitudCredito
{
    public int Id { get; set; }

    
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = "Monto solicitado debe ser mayor a 0")]
    public decimal MontoSolicitado { get; set; }

    public DateTime FechaSolicitud { get; set; } = DateTime.Now;

    public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Pendiente;

    public string? MotivoRechazo { get; set; }
}


