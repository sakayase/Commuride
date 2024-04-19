using CommurideModels.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace CommurideModels.DbContexts;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Carpool> Carpools { get; set; }
    public DbSet<Rent> Rents { get; set; }

}