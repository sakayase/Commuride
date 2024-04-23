
using System.ComponentModel.DataAnnotations;
using CommurideModels.Models;

namespace Models {
    public class Rent {

        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime DateHourLeaving { get; set; }
        [Required]
        public DateTime DateHourReturn { get; set; }
        [Required]
        public string AdressArrival { get; set; }
        [Required]
        public Vehicle vehicle{ get; set; }
        [Required]
        public AppUser user{ get; set; }

    }
}