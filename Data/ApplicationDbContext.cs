using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlataformaCreditos.Models;

namespace PlataformaCreditos.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<SolicitudCredito> Solicitudes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        
        builder.Entity<SolicitudCredito>()
            .HasCheckConstraint("CK_MontoSolicitado", "MontoSolicitado > 0");

        builder.Entity<Cliente>()
            .HasCheckConstraint("CK_IngresosMensuales", "IngresosMensuales > 0");

        
        builder.Entity<SolicitudCredito>()
            .HasIndex(s => new { s.ClienteId, s.Estado })
            .IsUnique()
            .HasFilter("[Estado] = 0"); 
    }
}


