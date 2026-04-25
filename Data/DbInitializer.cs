using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlataformaCreditos.Models;

namespace PlataformaCreditos.Data;

public static class DbInitializer
{
    public static async Task Seed(IServiceProvider services)
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        
        if (!context.Clientes.Any())
        {
            var cliente1 = new Cliente { UsuarioId = "user1", IngresosMensuales = 2000, Activo = true };
            var cliente2 = new Cliente { UsuarioId = "user2", IngresosMensuales = 3000, Activo = true };

            context.Clientes.AddRange(cliente1, cliente2);

            context.Solicitudes.Add(new SolicitudCredito 
            { 
                Cliente = cliente1, 
                MontoSolicitado = 4000, 
                Estado = EstadoSolicitud.Pendiente 
            });

            context.Solicitudes.Add(new SolicitudCredito 
            { 
                Cliente = cliente2, 
                MontoSolicitado = 5000, 
                Estado = EstadoSolicitud.Aprobado 
            });

            await context.SaveChangesAsync();
        }

        
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync("Analista"))
            await roleManager.CreateAsync(new IdentityRole("Analista"));

        var analista = new ApplicationUser 
        { 
            UserName = "analista@demo.com", 
            Email = "analista@demo.com" 
        };

        if (await userManager.FindByEmailAsync(analista.Email) == null)
        {
            await userManager.CreateAsync(analista, "Analista123!");
            await userManager.AddToRoleAsync(analista, "Analista");
        }
    }
}


