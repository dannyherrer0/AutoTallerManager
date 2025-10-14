
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data;

public class AutoTallerDbContext : DbContext
{
    public AutoTallerDbContext(DbContextOptions<AutoTallerDbContext> options)
        : base(options)
    {
    }
    public DbSet<Auditoria> Auditorias { get; set; }
    public DbSet<RateLimitConfig> RateLimitConfigs { get; set; }


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
        modelBuilder.Entity<DetalleOrden>()
            .Property(d => d.Subtotal)
            .HasComputedColumnSql("Cantidad * PrecioUnitario", stored: true);
        modelBuilder.Entity<Factura>()
        .Property(f => f.FormaPago)
        .HasConversion<string>();

        modelBuilder.Entity<OrdenServicio>()
            .Property(o => o.TipoServicio)
            .HasConversion<string>();

        modelBuilder.Entity<OrdenServicio>()
            .Property(o => o.Estado)
            .HasConversion<string>();


        modelBuilder.Entity<DetalleOrden>()
            .Property(d => d.Subtotal)
            .HasComputedColumnSql("Cantidad * PrecioUnitario", stored: true);
        modelBuilder.Entity<OrdenServicio>()
            .Property(o => o.TipoServicio)
            .HasConversion<string>();

        modelBuilder.Entity<OrdenServicio>()
            .Property(o => o.Estado)
            .HasConversion<string>();

        modelBuilder.Entity<Factura>()
            .Property(f => f.FormaPago)
            .HasConversion<string>();

        modelBuilder.Entity<Factura>()
            .Property(f => f.Estado)
            .HasConversion<string>();

        
        modelBuilder.Entity<Repuesto>()
    .ToTable(t =>
    {
        t.HasCheckConstraint("CK_Repuesto_Precio_NonNeg", "PrecioUnitario >= 0");
        t.HasCheckConstraint("CK_Repuesto_Cantidad_NonNeg", "CantidadStock >= 0");
    });

    modelBuilder.Entity<OrdenServicio>()
        .ToTable(t =>
        {
            t.HasCheckConstraint("CK_Orden_CostoManoObra_NonNeg", "CostoManoObra >= 0");
        });

    modelBuilder.Entity<Factura>()
        .ToTable(t =>
        {
            t.HasCheckConstraint("CK_Factura_MontoTotal_NonNeg", "MontoTotal >= 0");
        });

        modelBuilder.Entity<Usuario>().HasIndex(u => u.Correo).HasDatabaseName("idx_correo");
        modelBuilder.Entity<Cliente>().HasIndex(c => c.Nombre).HasDatabaseName("idx_nombre");
        modelBuilder.Entity<Vehiculo>().HasIndex(v => v.VIN).HasDatabaseName("idx_vin");


    }
}