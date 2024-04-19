
using CommurideModels.Models;

namespace Models {
    public class Rent {

       
        public int Id { get; set; }
        public DateTime DateHourLeaving { get; set; }
        public DateTime DateHourReturn { get; set; }
        public string AdressArrival { get; set; }
        public Vehicle vehicle{ get; set; }
        public AppUser user{ get; set; }

    }
}