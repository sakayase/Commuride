using System.ComponentModel.DataAnnotations;
using static Models.Vehicle;

namespace CommurideModels.DTOs.Vehicle
{
    public class UpdateVehicleDTO
    {
        public required string Brand { get; set; }
        public required string Model { get; set; }
        public required CategoryVehicle Category { get; set; }
        public required string URLPhoto { get; set; }
        public required int CO2 { get; set; }
        public required StatutVehicle status{ get; set; }
    }
}
