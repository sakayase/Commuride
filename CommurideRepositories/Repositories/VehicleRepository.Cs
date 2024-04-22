using CommurideModels.DTOs.Vehicle;
using CommurideModels.Models;
using CommurideRepositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Models;

namespace CommurideRepositories.Repositories {
    public class VehicleRepository : IVehicleRepository
    {
        public Task<Vehicle> CreateVehicle(AppUser AppUser, CreateVehicleDTO vehicleDTO)
        {
            throw new NotImplementedException();
        }

        public Task DeleteComment(AppUser AppUser, int VehicleId)
        {
            throw new NotImplementedException();
        }

        public Task<GetVehicleDTO?> Get(int VehicleId)
        {
            throw new NotImplementedException();
        }

        public Task<List<GetAllVehicleDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Vehicle> UpdateVehicle(AppUser AppUser, UpdateVehicleDTO commentDTO)
        {
            throw new NotImplementedException();
        }
    }
}