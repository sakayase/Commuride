using CommurideModels.DbContexts;
using CommurideModels.DTOs.Vehicle;
using CommurideModels.Models;
using CommurideRepositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Models;

namespace CommurideRepositories.Repositories {
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context;
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
            _context.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;


        }

        public async Task DeleteVehicle( int VehicleId)
        {
            Vehicle? vehicle = await _context.Vehicles.FirstOrDefaultAsync(x => VehicleId == x.Id);
            _context.Vehicles.Remove(vehicle!);
            await _context.SaveChangesAsync();
        }

        public async Task<GetVehicleDTO?> Get(int VehicleId)
        {
            return await _context.Vehicles.Select(x => new GetVehicleDTO()
            {
                Id = x.Id,
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
            List<GetAllVehicleDTO> vehicles = await _context.Vehicles.Select(x => new GetAllVehicleDTO()
            {
                Id = x.Id,
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

        public async Task<Vehicle> UpdateVehicle(int VehicleId, UpdateVehicleDTO vehicleDTO)
        {
            Vehicle? vehicle = await _context.Vehicles.FirstOrDefaultAsync(x => x.Id == VehicleId);
            _context.Update(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }
    }
}