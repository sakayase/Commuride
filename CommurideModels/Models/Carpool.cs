
using System.ComponentModel.DataAnnotations;
using CommurideModels.Models;
using Microsoft.EntityFrameworkCore;

namespace Models {
    public class Carpool {

        [Required]
        public int Id { get; set; }
        [Required]
        public string AddressLeaving { get; set; }
        [Required]
        public DateTime DateDepart { get; set; }
        [Required]
        public string AddressArrival { get; set; }
        public int Duration { get; set; }
        public int Distance { get; set; }
        [Required]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public AppUser Driver { get; set; }
        [Required]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Vehicle Vehicle { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public ICollection<AppUser> Passengers { get; } = new List<AppUser>();
    }
}