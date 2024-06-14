using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Models.Vehicle;

namespace CommurideModels.DTOs.Carpool
{
    public class CarpoolDTO
    {
        [Required]
        public required int Id { get; set; }
        [Required]
        public required string AddressLeaving { get; set; }
        [Required]
        public required DateTime DateDepart { get; set; }
        [Required]
        public required string AddressArrival { get; set; }
        public int Duration { get; set; }
        public int Distance { get; set; }
        [Required]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public CarpoolAppUserDTO? Driver { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public CarpoolVehicleDTO? VehicleDTO { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public ICollection<CarpoolAppUserDTO>? Passengers { get; set; }
    }

    public class CarpoolAppUserDTO
    {
        public required string Id { get; set; }
        public string? URLPhoto { get; set; }
        public required string Username { get; set; }
    }

    public class CarpoolVehicleDTO
    {
        [Required]
        public required int Id { get; set; }
        [Required]
        public required string Brand { get; set; }
        [Required]
        public required string Model { get; set; }
        [Required]
        public required CategoryVehicle Category { get; set; }
        [Required]
        public string? URLPhoto { get; set; }
        [Required]
        public required MotorizationVehicle Motorization { get; set; }
        [Required]
        public int? CO2 { get; set; }
        [Required]
        public required int NbPlaces { get; set; }
    }
    
}
