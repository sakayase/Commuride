using CommurideModels.DbContexts;
using CommurideModels.Models;
using Models;

namespace CommurideTests.Helpers
{
    internal class Utilities
    {
        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            db.Vehicles.AddRange(GetSeedingVehicles());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(ApplicationDbContext db)
        {
            db.Rents.RemoveRange(db.Rents);
            db.Vehicles.RemoveRange(db.Vehicles);
            InitializeDbForTests(db);
        }

        public static List<Vehicle> GetSeedingVehicles()
        {
            return new List<Vehicle>()
            {
                new Vehicle { Id = 1, Brand = "renault", Category = Vehicle.CategoryVehicle.MiniCitadine, CO2 = 11, Model = "clio", Motorization = Vehicle.MotorizationVehicle.Hybride, NbPlaces = 5, Registration = "OK", Status = Vehicle.StatusVehicle.Service, URLPhoto = "" },
                new Vehicle { Id = 2, Brand = "mazda", Category = Vehicle.CategoryVehicle.Compacte, CO2 = 20, Model = "MX5", Motorization = Vehicle.MotorizationVehicle.Essence, NbPlaces = 2, Registration = "OK", Status = Vehicle.StatusVehicle.Service, URLPhoto = "" },
                new Vehicle { Id = 3, Brand = "renault", Category = Vehicle.CategoryVehicle.Compacte, CO2 = 21, Model = "megane", Motorization = Vehicle.MotorizationVehicle.Diesel, NbPlaces = 5, Registration = "OK", Status = Vehicle.StatusVehicle.Service, URLPhoto = "" },
            };
        }
    }
}
