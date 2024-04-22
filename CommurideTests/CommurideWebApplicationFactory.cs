using CommurideModels.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace CommurideTests
{
    public class CommurideWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
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
                    options.UseInMemoryDatabase("CommurideTestDB");
                });
            });
        }

        /*public new HttpClient CreateClient()
        {
            var cookieContainer = new CookieContainer();
            var httpClientHandler = new HttpClientHandler()
            {
                CookieContainer = cookieContainer
            };

            var client = new HttpClient(httpClientHandler);
            client
            return client;
        } */
    }
}

