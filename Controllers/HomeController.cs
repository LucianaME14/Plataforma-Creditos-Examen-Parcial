using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace PlataformaCreditos.Controllers;

public class HomeController : Controller
{
    // Página principal
    [AllowAnonymous]
    public IActionResult Index()
    {
        
        HttpContext.Session.SetString("UsuarioActivo", User.Identity?.Name ?? "Invitado");

        
        ViewBag.Usuario = HttpContext.Session.GetString("UsuarioActivo");

        return View();
    }

    
    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    
    [AllowAnonymous]
    public IActionResult Error()
    {
        return View();
    }
}

