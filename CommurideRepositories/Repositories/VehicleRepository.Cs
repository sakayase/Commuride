using CommurideModels.DbContexts;
using CommurideModels.DTOs.Vehicle;
using CommurideModels.Models;
using CommurideRepositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Models;

namespace CommurideRepositories.Repositories {
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VehicleRepository(ApplicationDbContext context)
        {
            this._dbContext = context;
        }

        public async Task<Vehicle> CreateVehicle( CreateVehicleDTO vehicleDTO)
        {
            Vehicle? vehicle = new() {
                Id = vehicleDTO.VehicleId,
                Registration = vehicleDTO.Registration,
                Brand = vehicleDTO.Brand,
                Model = vehicleDTO.Model,
                Category = vehicleDTO.Category,
                URLPhoto =  vehicleDTO.URLPhoto,
                Motorization = vehicleDTO.Motorization,
                CO2 =   vehicleDTO.CO2,
                Status = vehicleDTO.Status,
                NbPlaces = vehicleDTO.NbPlaces
            };
            _dbContext.Add(vehicle);
            await _dbContext.SaveChangesAsync();
            return vehicle;


        }

        public async Task DeleteVehicle( int VehicleId)
        {
            Vehicle? vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(x => VehicleId == x.Id);
            _dbContext.Vehicles.Remove(vehicle!);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<GetVehicleDTO?> Get(int VehicleId)
        {
            return await _dbContext.Vehicles.Select(x => new GetVehicleDTO()
            {
                Registration = x.Registration,
                Brand = x.Brand,
                Model = x.Model,
                Category = x.Category,
                URLPhoto =  x.URLPhoto,
                Motorization = x.Motorization,
                CO2 =   x.CO2,
                Status = x.Status,
                NbPlaces = x.NbPlaces
            }).FirstOrDefaultAsync();
        }

        public async Task<List<GetAllVehicleDTO>> GetAll()
        {
            List<GetAllVehicleDTO> vehicles = await _dbContext.Vehicles.Select(x => new GetAllVehicleDTO()
            {
                Registration = x.Registration,
                Brand = x.Brand,
                Model = x.Model,
                Category = x.Category,
                URLPhoto =  x.URLPhoto,
                Motorization = x.Motorization,
                CO2 =   x.CO2,
                Status = x.Status,
                NbPlaces = x.NbPlaces
            }).ToListAsync();
            return vehicles;
        }

        public async Task<Vehicle> UpdateVehicle( UpdateVehicleDTO vehicleDTO)
        {
            Vehicle? vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(x => x.Id == vehicleDTO.VehicleId);
            _dbContext.Update(vehicle);
            await _dbContext.SaveChangesAsync();
            return vehicle;
        }
    }
}