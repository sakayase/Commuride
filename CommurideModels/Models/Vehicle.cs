
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommurideModels.DTOs.Vehicle;
using CommurideModels.Models;

namespace Models
{
    public class Vehicle
    {

        public enum CategoryVehicle
        {
            MicroUrbaine,
            MiniCitadine,
            CitadinePolyvalente,
            Compacte,
            BerlineTailleS,
            BerlineTailleM,
            BerlineTailleL,
            SUVPickUpTT
        }

        public enum MotorizationVehicle
        {
            Diesel,
            Essence,
            Ethanol,
            Hybride,
            Electrique
        }

        public enum StatusVehicle
        {
            Service,
            HorsService,
            Reparation
        }

        [Required]
        public int Id { get; set; }
        [Required]
        public required string Registration { get; set; }
        [Required]
        public required string Brand { get; set; }
        [Required]
        public required string Model { get; set; }
        [Required]
        public CategoryVehicle Category { get; set; }
        [Required]
        public string? URLPhoto { get; set; }
        [Required]
        public MotorizationVehicle Motorization { get; set; }
        [Required]
        public int CO2 { get; set; }
        [Required]
        public StatusVehicle Status{ get; set; }
        [Required]
        public int NbPlaces { get; set; }
        [ForeignKey("UserId")]
        public AppUser? User { get; set; }
        [ForeignKey("RentId")]
        public ICollection<Rent>? Rents { get; } = new List<Rent>();

    }
}