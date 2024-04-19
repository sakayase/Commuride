
using CommurideModels.Models;

namespace Models {
    public class Carpool {

       
        public int Id { get; set; }
        public string AdresseLeaving { get; set; }
        public DateTime DateHourReturn { get; set; }
        public string AdressArrival { get; set; }
        public int duration { get; set; }
        public int distance { get; set; }
        public ICollection<AppUser> Users { get; } = new List<AppUser>();
        public Vehicle vehicle { get; set; }

    }
}