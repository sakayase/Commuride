using Microsoft.EntityFrameworkCore;

namespace DbContexts;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
   
}