using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCreditos.Data;
using PlataformaCreditos.Models;

namespace PlataformaCreditos.Controllers;

[Authorize]
public class ClientesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ClientesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var clientes = await _context.Clientes.ToListAsync();
        return View(clientes);
    }

    public async Task<IActionResult> Details(int id)
    {
        var cliente = await _context.Clientes
            .Include(c => c.Solicitudes)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null) return NotFound();
        return View(cliente);
    }
}
