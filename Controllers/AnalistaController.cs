using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCreditos.Data;
using PlataformaCreditos.Models;

namespace PlataformaCreditos.Controllers;

[Authorize(Roles = "Analista")]
public class AnalistaController : Controller
{
    private readonly ApplicationDbContext _context;

    public AnalistaController(ApplicationDbContext context)
    {
        _context = context;
    }

    
    public async Task<IActionResult> Index()
    {
        var pendientes = await _context.Solicitudes
            .Include(s => s.Cliente)
            .Where(s => s.Estado == EstadoSolicitud.Pendiente)
            .ToListAsync();

        return View(pendientes);
    }

    
    [HttpPost]
    public async Task<IActionResult> Aprobar(int id)
    {
        var solicitud = await _context.Solicitudes.FindAsync(id);
        if (solicitud == null) return NotFound();

        solicitud.Estado = EstadoSolicitud.Aprobado;
        solicitud.MotivoRechazo = null;

        _context.Update(solicitud);
        await _context.SaveChangesAsync();

        TempData["Ok"] = "Solicitud aprobada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    
    [HttpPost]
    public async Task<IActionResult> Rechazar(int id, string motivo)
    {
        var solicitud = await _context.Solicitudes.FindAsync(id);
        if (solicitud == null) return NotFound();

        solicitud.Estado = EstadoSolicitud.Rechazado;
        solicitud.MotivoRechazo = motivo;

        _context.Update(solicitud);
        await _context.SaveChangesAsync();

        TempData["Ok"] = "Solicitud rechazada correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
