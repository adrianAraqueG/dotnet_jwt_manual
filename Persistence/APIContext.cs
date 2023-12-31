using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Configurations;

namespace Persistence;

/*
 * APIContext - Contexto de la base de datos para la aplicación.
 * 
 * Este contexto representa una sesión con la base de datos, permitiendo realizar operaciones CRUD en las entidades.
 * 
 * Propiedades:
 * - Users: Representa la tabla `Users` en la base de datos.
 * - Vehicles: Representa la tabla `Vehicles` en la base de datos.
 * - Roles: Representa la tabla `Roles` en la base de datos.
 * 
 * Configuraciones:
 * El método `OnModelCreating` se utiliza para configurar el modelo de la base de datos. Aquí se pueden definir las relaciones, claves primarias, 
 * índices, y otros detalles de mapeo entre las entidades y la base de datos. En este caso estamos trayendo esas configuraciones desde otro lugar,
 * así no tenemos que definir esas propiedades directamente aquí
 */


public class APIContext : DbContext 
{   
    public APIContext(DbContextOptions options): base (options){}

    /* Incluyendo las entidades */
    public DbSet<User> Users {get; set;}
    public DbSet<Vehicle> Vehicles {get; set;}
    public DbSet<Role> Roles {get; set;}
    public DbSet<RefreshToken> RefreshTokens {get; set;}
    public DbSet<Blacklist> Blacklist {get; set;}

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new VehicleConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new BlacklistConfiguration());
        
    }
}
