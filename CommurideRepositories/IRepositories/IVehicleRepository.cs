using CommurideModels.Models;
using CommurideModels.DTOs.Vehicle;
using Models;

namespace CommurideRepositories.IRepositories
{
    public interface IVehicleRepository
    {
        Task<List<GetAllVehicleDTO>> GetAll();
        Task<GetVehicleDTO?> Get(int VehicleId);
        Task<Vehicle> CreateVehicle(AppUser AppUser, CreateVehicleDTO vehicleDTO);
        Task<Vehicle> UpdateVehicle(AppUser AppUser, UpdateVehicleDTO commentDTO);
        Task DeleteComment(AppUser AppUser, int VehicleId);
    }
}