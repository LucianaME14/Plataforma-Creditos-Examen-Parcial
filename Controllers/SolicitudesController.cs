using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCreditos.Data;
using PlataformaCreditos.Models;
using System.Security.Claims;

namespace PlataformaCreditos.Controllers;

[Authorize]
public class SolicitudesController : Controller
{
    private readonly ApplicationDbContext _context;

    public SolicitudesController(ApplicationDbContext context)
    {
        _context = context;
    }

    
    public async Task<IActionResult> Index()
    {
        var solicitudes = await _context.Solicitudes
            .Include(s => s.Cliente)
            .ToListAsync();

        return View(solicitudes);
    }

    
    public async Task<IActionResult> MisSolicitudes()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.UsuarioId == userId);

        if (cliente == null) return NotFound();

        var solicitudes = await _context.Solicitudes
            .Where(s => s.ClienteId == cliente.Id)
            .ToListAsync();

        return View(solicitudes);
    }

   
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(decimal montoSolicitado)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.UsuarioId == userId);

        if (cliente == null || !cliente.Activo)
        {
            ModelState.AddModelError("", "Cliente no activo o no encontrado.");
            return View();
        }

        if (montoSolicitado <= 0)
        {
            ModelState.AddModelError("", "El monto debe ser mayor a 0.");
            return View();
        }

        var existePendiente = await _context.Solicitudes
            .AnyAsync(s => s.ClienteId == cliente.Id && s.Estado == EstadoSolicitud.Pendiente);

        if (existePendiente)
        {
            ModelState.AddModelError("", "Ya existe una solicitud pendiente.");
            return View();
        }

        if (montoSolicitado > cliente.IngresosMensuales * 10)
        {
            ModelState.AddModelError("", "El monto solicitado excede 10 veces los ingresos mensuales.");
            return View();
        }

        var solicitud = new SolicitudCredito
        {
            ClienteId = cliente.Id,
            MontoSolicitado = montoSolicitado,
            Estado = EstadoSolicitud.Pendiente,
            FechaSolicitud = DateTime.Now
        };

        _context.Solicitudes.Add(solicitud);
        await _context.SaveChangesAsync();

        TempData["Ok"] = "Solicitud registrada exitosamente.";
        return RedirectToAction(nameof(MisSolicitudes));
    }
}

