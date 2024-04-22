using CommurideModels.Models;
using CommurideModels.DTOs.Vehicle;
using Models;

namespace CommurideRepositories.IRepositories
{
    public interface IVehicleRepository
    {
        Task<List<GetAllVehicleDTO>> GetAll();
        Task<GetVehicleDTO?> Get(int VehicleId);
        Task<Vehicle> CreateVehicle( CreateVehicleDTO vehicleDTO);
        Task<Vehicle> UpdateVehicle( UpdateVehicleDTO commentDTO);
        Task DeleteVehicle( int VehicleId);
    }
}