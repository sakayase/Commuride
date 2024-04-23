using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommurideModels.DTOs.Carpool
{
    public class PostCarpoolDTO
    {
        [Required]
        public required string AddressLeaving { get; set; }
        [Required]
        public required DateTime DateDepart { get; set; }
        [Required]
        public required string AddressArrival { get; set; }
        [Required]
        public required int vehicleId { get; set; }
    }
}
