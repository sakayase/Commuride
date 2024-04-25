using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommurideModels.Models
{
    public class AppUser : IdentityUser
    {
        public string? PhotoURL { get; set; }
        public List<Rent>? Rents { get; set; }
        [ForeignKey("DriverId")]
        public List<Carpool>? Carpools { get; set; }
        public List<Carpool>? PassengerCarpools { get; set; }
    }
}
