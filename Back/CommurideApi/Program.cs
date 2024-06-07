using System.Reflection;
using System.Text.Json.Serialization;
using CommurideModels.DbContexts;
using CommurideModels.Models;
using CommurideRepositories.IRepositories;
using CommurideRepositories.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>( options =>
{
    options.EnableSensitiveDataLogging(); // DEBUG
    var connectionString = builder.Configuration.GetConnectionString("CommurideConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsConfigDev", policy =>
    {
/*        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
*/       policy.SetIsOriginAllowed(origin => {
            Console.WriteLine(origin);
            Uri uri = new UriBuilder(origin).Uri;
            return uri.IsLoopback;
        }).AllowCredentials().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
});

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddIdentityApiEndpoints<AppUser>(o =>
{
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequireUppercase = false;
    o.Password.RequiredLength = 4;
    o.Password.RequiredUniqueChars = 1;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<ICarpoolRepository, CarpoolRepository>();
builder.Services.AddScoped<IRentRepository, RentRepository>();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Commuride API",
        Description = "API for the Commuride Application",
        Contact = new OpenApiContact
        {
            Name = "Simon Ponitzki",
            Url = new Uri("mailto:simon.ponitzki@gmail.com"),
        },
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsConfigDev");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


//Expose Program to Tests
public partial class Program { }
