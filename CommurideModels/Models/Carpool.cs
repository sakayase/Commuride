
using System.ComponentModel.DataAnnotations;
using CommurideModels.Models;

namespace Models {
    public class Carpool {

        [Required]
        public int Id { get; set; }
        [Required]
        public string AdresseLeaving { get; set; }
        [Required]
        public DateTime DateHourReturn { get; set; }
        [Required]
        public string AdressArrival { get; set; }
        public int duration { get; set; }
        public int distance { get; set; }
        [Required]
        public AppUser user { get; set; }
        [Required]
        public Vehicle vehicle { get; set; }
        public ICollection<AppUser> Users { get; } = new List<AppUser>();
    }
}