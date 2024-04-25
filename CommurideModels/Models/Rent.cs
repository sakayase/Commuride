
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommurideModels.Models;
using Microsoft.EntityFrameworkCore;

namespace Models {
    public class Rent {

        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime DateHourStart { get; set; }
        [Required]
        public DateTime DateHourEnd { get; set; }
        [Required]
        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; }
        [Required]
        public int VehicleId { get; set; }
        [Required]
        [ForeignKey("UserId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public AppUser User { get; set; }
        [Required]
        public string UserId { get; set; }

    }
}