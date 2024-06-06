using CommurideModels.DbContexts;
using CommurideTests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace CommurideTests
{
    public class CommurideWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        //Random db name in order to have a clean database with each test class
        private readonly string _dbName = Guid.NewGuid().ToString();
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
            builder.ConfigureServices(services =>
            {

                // Remove the dbcontext
                var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ApplicationDbContext));
                if (context != null)
                {
                    services.Remove(context);
                    var options = services.Where(r => r.ServiceType == typeof(DbContextOptions)
                        || r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>)).ToArray();
                    foreach (var option in options)
                    {
                        services.Remove(option);
                    }
                }

                // Add a new registration for ApplicationDbContext with an in-memory database
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    // Provide a unique name for your in-memory database
                    options.UseInMemoryDatabase(_dbName);
                    options.EnableSensitiveDataLogging();
                });

                // Seed test data to test db
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var db = sp.GetRequiredService<ApplicationDbContext>();
                    db.Database.EnsureCreated();
                    try
                    {
                        Utilities.InitializeDbForTests(db);
                    } catch (Exception ex) 
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            });
        }
    }
}

