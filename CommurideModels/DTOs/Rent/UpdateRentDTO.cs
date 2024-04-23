using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommurideModels.DTOs.Rent
{
    public class UpdateRentDTO
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? VehicleId { get; set; }
    }
}
