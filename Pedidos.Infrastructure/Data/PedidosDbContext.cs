using Microsoft.EntityFrameworkCore;
using Pedidos.Domain.Entities;

namespace Pedidos.Infrastructure.Data;

public class PedidosDbContext : DbContext
{
    public PedidosDbContext(DbContextOptions<PedidosDbContext> options) : base(options)
    {
    }

    public DbSet<PedidoCabecera> PedidoCabecera { get; set; } = null!;
    public DbSet<PedidoDetalle> PedidoDetalle { get; set; } = null!;
    public DbSet<LogAuditoria> LogAuditoria { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuraciones de base de datos para mapear con los nombres reales de las tablas
        modelBuilder.Entity<PedidoCabecera>().ToTable("PedidoCabecera");
        modelBuilder.Entity<PedidoDetalle>().ToTable("PedidoDetalle");
        modelBuilder.Entity<LogAuditoria>().ToTable("LogAuditoria");

        // Relación Cabecera - Detalle
        modelBuilder.Entity<PedidoCabecera>()
            .HasMany(p => p.Detalles)
            .WithOne(d => d.PedidoCabecera)
            .HasForeignKey(d => d.PedidoCabeceraId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
