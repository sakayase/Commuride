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

        public async Task<Vehicle> UpdateVehicle(UpdateVehicleDTO vehicleDTO)
        {
            Vehicle? vehicle = await _context.Vehicles.FirstOrDefaultAsync(x => x.Id == vehicleDTO.Id);
            if (vehicle == null)
            {
                throw new Exception("The vehicle does not exist.");
            }
            if (vehicleDTO.Id != null)
            {
                vehicle.Id = vehicleDTO.Id;
            }
            if (vehicleDTO.Registration != null)
            {
                vehicle.Registration = vehicleDTO.Registration;
            }
            if (vehicleDTO.Brand != null)
            {
                vehicle.Brand = vehicleDTO.Brand;
            }
            if (vehicleDTO.Model != null)
            {
                vehicle.Model = vehicleDTO.Model;
            }
            if (vehicleDTO.Category != null)
            {
                vehicle.Category = vehicleDTO.Category;
            }
            if (vehicleDTO.URLPhoto != null)
            {
                vehicle.URLPhoto = vehicleDTO.URLPhoto;
            }
            if (vehicleDTO.CO2 != null)
            {
                vehicle.CO2 = vehicleDTO.CO2;
            }
            if (vehicleDTO.Motorization != null)
            {
                vehicle.Motorization = vehicleDTO.Motorization;
            }
            if (vehicleDTO.Status != null)
            {
                vehicle.Status = vehicleDTO.Status;
            }
            if (vehicleDTO.NbPlaces != null)
            {
                vehicle.NbPlaces = vehicleDTO.NbPlaces;
            }
            _context.Entry(vehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return vehicle;
        }
    }
}