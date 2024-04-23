using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.Vehicle;

namespace CommurideModels.DTOs.Rent
{
    public class RentDTO
    {
        public int Id { get; set; }
        public RentVehicleDTO Vehicle { get; set; }
        public RentAppUserDTO AppUser { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }

    public class RentAppUserDTO
    {
        public required string Id { get; set; }
        public string? URLPhoto { get; set; }
        public required string Username { get; set; }
    }

    public class RentVehicleDTO
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
        public required MotorisationVehicle Motorization { get; set; }
        [Required]
        public int? CO2 { get; set; }
        [Required]
        public required int NbPlaces { get; set; }
    }
}
