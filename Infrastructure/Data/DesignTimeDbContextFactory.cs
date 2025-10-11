using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AutoTallerDbContext>
{
    public AutoTallerDbContext CreateDbContext(string[] args)
    {
        // Configuración para migraciones
        var optionsBuilder = new DbContextOptionsBuilder<AutoTallerDbContext>();
        
        // IMPORTANTE: Cambia la cadena de conexión según tu BD
        
        // Opción 1: SQL Server LocalDB (Windows)
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=AutoTallerDB;Trusted_Connection=true;TrustServerCertificate=true;"
        );
        
        // Opción 2: MySQL (descomentar si usas MySQL)
        // optionsBuilder.UseMySql(
        //     "Server=localhost;Database=autotallerdb;User=root;Password=tu_password;",
        //     ServerVersion.AutoDetect("Server=localhost;Database=autotallerdb;User=root;Password=tu_password;")
        // );
        
        // Opción 3: SQLite (descomentar si usas SQLite)
        // optionsBuilder.UseSqlite("Data Source=autotaller.db");

        return new AutoTallerDbContext(optionsBuilder.Options);
    }
}