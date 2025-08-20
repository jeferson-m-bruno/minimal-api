using Microsoft.EntityFrameworkCore;
using MinimalAPI.Domain.Entities;

namespace MinimalAPI.Infra.DB;

public class MyContext : DbContext
{
    private readonly IConfiguration _configurationAppSettings;
    public DbSet<Admin> Admins { get; set; } = default!;
    public DbSet<Vehicle> Vehicles { get; set; } = default!;

    public MyContext(IConfiguration configuration)
    {
        this._configurationAppSettings = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>().HasData(
            new Admin()
            {
                Id = 1,
                Email = "administrador",
                Password = "123456",
                Perfil = "ADM"
            }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;

        var connectionString = this._configurationAppSettings.GetConnectionString("mysql")?.ToString();
        if (String.IsNullOrEmpty(connectionString))
            return;

        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}