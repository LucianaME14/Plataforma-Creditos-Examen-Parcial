using Microsoft.AspNetCore.Identity;

namespace PlataformaCreditos.Models;

public class ApplicationUser : IdentityUser
{
    public string? NombreCompleto { get; set; }
}

