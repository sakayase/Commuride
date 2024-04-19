
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

        public int Id { get; set; }
        public string Registration { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public CategoryVehicle Category { get; set; }
        public string URLPhoto { get; set; }
        public MotorisationVehicle Motorization { get; set; }
        public int CO2 { get; set; }
        public StatutVehicle status{ get; set; }
        public int NbPlaces { get; set; }
        public AppUser user { get; set; }

    }
}