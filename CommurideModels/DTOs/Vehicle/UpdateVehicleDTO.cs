using System.ComponentModel.DataAnnotations;
using static Models.Vehicle;

namespace CommurideModels.DTOs.Vehicle
{
    public class UpdateVehicleDTO
    {
        public string? Registration { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public CategoryVehicle? Category { get; set; }
        public string? URLPhoto { get; set; }
        public int? CO2 { get; set; }
        public MotorizationVehicle? Motorization { get; set; }
        public StatusVehicle? Status{ get; set; }
        public int? NbPlaces { get; set; }
    }
}
