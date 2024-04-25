using CommurideModels.DTOs.AppUser;
using CommurideModels.DTOs.Vehicle;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.Vehicle;

namespace CommurideModels.DTOs.Rent
{
    public class RentDTO
    {
        public int Id { get; set; }
        public GetVehicleDTO? Vehicle { get; set; }
        public AppUserDTO? AppUser { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
