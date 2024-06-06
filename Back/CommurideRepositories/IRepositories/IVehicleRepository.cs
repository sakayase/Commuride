using CommurideModels.Models;
using CommurideModels.DTOs.Vehicle;
using Models;

namespace CommurideRepositories.IRepositories
{
    public interface IVehicleRepository
    {
        Task<List<GetAllVehicleDTO>> GetAll();
        Task<GetVehicleDTO?> Get(int VehicleId);
        Task<GetVehicleDTO?> GetVehicleByRegistration(string registration);
        Task<List<GetVehicleDTO>> GetVehicleByBrand(string brand);
        Task<Vehicle> CreateVehicle(CreateVehicleDTO vehicleDTO);
        Task<Vehicle> UpdateVehicle(UpdateVehicleDTO vehicleDTO);
        Task DeleteVehicle( int VehicleId);
    }
}