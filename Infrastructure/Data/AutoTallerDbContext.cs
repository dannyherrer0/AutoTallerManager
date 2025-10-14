
using Domain.Entities;  
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AutoTallerDbContext : DbContext
{
    public AutoTallerDbContext(DbContextOptions<AutoTallerDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Vehiculo> Vehiculos { get; set; }
    public DbSet<OrdenServicio> OrdenesServicio { get; set; }
    public DbSet<Repuesto> Repuestos { get; set; }
    public DbSet<DetalleOrden> DetallesOrden { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Factura> Facturas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Usuario>()
        .Property(u => u.UsuarioId)
        .HasColumnName("UsuarioId");
        modelBuilder.Entity<BaseEntity>().Property<DateTime>("FechaCreacion")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<BaseEntity>().Property<DateTime>("FechaActualizacion")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAddOrUpdate();


    }
}