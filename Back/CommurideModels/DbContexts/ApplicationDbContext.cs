using CommurideModels.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace CommurideModels.DbContexts;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Carpool> Carpools { get; set; }
    public DbSet<Rent> Rents { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.Entity<Rent>().HasOne(r => r.User).WithMany(u => u.Rents);
        builder.Entity<Rent>().HasOne(r => r.Vehicle).WithMany(u => u.Rents);
        builder.Entity<AppUser>().HasMany(u => u.Carpools).WithOne(c => c.Driver);
        builder.Entity<Carpool>().HasMany(c => c.Passengers).WithMany(u => u.PassengerCarpools);


        string ADMIN_ID = "02174cf0–9412–4cfe - afbf - 59f706d72cf6";
        string ROLE_ID = "341743f0 - asd2–42de - afbf - 59kmkkmk72cf6";

        //seed admin role
        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Name = "Admin",
            NormalizedName = "ADMIN",
            Id = ROLE_ID,
            ConcurrencyStamp = ROLE_ID
        });

        //create user
        var appUser = new AppUser
        {
            Id = ADMIN_ID,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            PhotoURL = ""
        };

        //set user password
        PasswordHasher<AppUser> ph = new PasswordHasher<AppUser>();
        appUser.PasswordHash = ph.HashPassword(appUser, "admin");

        //seed user
        builder.Entity<AppUser>().HasData(appUser);

        builder.Entity<Vehicle>().HasData(
       new Vehicle { Id = 1, Brand = "renault", Category = Vehicle.CategoryVehicle.MiniCitadine, CO2 = 11, Model = "clio", Motorization = Vehicle.MotorizationVehicle.Hybride, NbPlaces = 5, Registration = "OK", Status = Vehicle.StatusVehicle.Service, URLPhoto = "" },
            new Vehicle { Id = 2, Brand = "mazda", Category = Vehicle.CategoryVehicle.Compacte, CO2 = 20, Model = "MX5", Motorization = Vehicle.MotorizationVehicle.Essence, NbPlaces = 2, Registration = "OK", Status = Vehicle.StatusVehicle.Service, URLPhoto = "" },
            new Vehicle { Id = 3, Brand = "renault", Category = Vehicle.CategoryVehicle.Compacte, CO2 = 21, Model = "megane", Motorization = Vehicle.MotorizationVehicle.Diesel, NbPlaces = 5, Registration = "OK", Status = Vehicle.StatusVehicle.Service, URLPhoto = "" },
            new Vehicle { Id = 4, Brand = "wolkswagen", Category = Vehicle.CategoryVehicle.CitadinePolyvalente, CO2 = 20, Model = "polo", Motorization = Vehicle.MotorizationVehicle.Diesel, NbPlaces = 5, Registration = "OK1", Status = Vehicle.StatusVehicle.HorsService, URLPhoto = "" },
            new Vehicle { Id = 5, Brand = "toyota", Category = Vehicle.CategoryVehicle.MiniCitadine, CO2 = 3, Model = "yaris", Motorization = Vehicle.MotorizationVehicle.Hybride, NbPlaces = 5, Registration = "OK", Status = Vehicle.StatusVehicle.Service, URLPhoto = "" },
            new Vehicle { Id = 6, Brand = "tesla", Category = Vehicle.CategoryVehicle.BerlineTailleM, CO2 = 0, Model = "model c", Motorization = Vehicle.MotorizationVehicle.Electrique, NbPlaces = 2, Registration = "OK1", Status = Vehicle.StatusVehicle.Service, URLPhoto = "" },
            new Vehicle { Id = 7, Brand = "bmw", Category = Vehicle.CategoryVehicle.Compacte, CO2 = 24, Model = "serie a", Motorization = Vehicle.MotorizationVehicle.Essence, NbPlaces = 5, Registration = "OK", Status = Vehicle.StatusVehicle.Service, URLPhoto = "" },
            new Vehicle { Id = 8, Brand = "renault", Category = Vehicle.CategoryVehicle.BerlineTailleM, CO2 = 20, Model = "laguna", Motorization = Vehicle.MotorizationVehicle.Ethanol, NbPlaces = 2, Registration = "OK1", Status = Vehicle.StatusVehicle.Service, URLPhoto = "" }
            );

        builder.Entity<Rent>().HasData(
            new Rent { Id = 1, DateHourEnd = DateTime.Now.AddDays(7), DateHourStart = DateTime.Now, VehicleId = 1, UserId = ADMIN_ID}
            );


        base.OnModelCreating(builder);
    }
}