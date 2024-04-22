
using System.ComponentModel.DataAnnotations;
using CommurideModels.Models;

namespace Models {
    public class Vehicle {

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

        public enum MotorisationVehicle
        {
            Diesel,
            Essence,
            Ethanol,
            Hybride,
            Electrique
        }

        public enum StatutVehicle
        {
            Service,
            HorsService,
        }

        [Required]
        public int Id { get; set; }
        [Required]
        public string Registration { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public CategoryVehicle Category { get; set; }
        [Required]
        public string URLPhoto { get; set; }
        [Required]
        public MotorisationVehicle Motorization { get; set; }
        [Required]
        public int CO2 { get; set; }
        [Required]
        public StatutVehicle Status{ get; set; }
        [Required]
        public int NbPlaces { get; set; }
        public AppUser User { get; set; }

    }
}