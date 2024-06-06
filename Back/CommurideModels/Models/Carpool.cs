
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommurideModels.Models;
using Microsoft.EntityFrameworkCore;

namespace Models {
    public class Carpool {

        [Required]
        public int Id { get; set; }
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
        [ForeignKey("DriverId")]
        public AppUser Driver { get; set; }
        [Required]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required Vehicle Vehicle { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public ICollection<AppUser> Passengers { get; set; } = new List<AppUser>();
    }
}