
using System.ComponentModel.DataAnnotations;
using CommurideModels.Models;

namespace Models {
    public class Rent {

       [Required]
        public int Id { get; set; }
        [Required]
        public DateTime DateHourStart { get; set; }
        [Required]
        public DateTime DateHourEnd { get; set; }
        [Required]
        public Vehicle Vehicle { get; set; }
        [Required]
        public AppUser User { get; set; }

    }
}