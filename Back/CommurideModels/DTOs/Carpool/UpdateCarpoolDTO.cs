using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommurideModels.DTOs.Carpool
{
    public class UpdateCarpoolDTO
    {
        public DateTime? dateDepart {  get; set; }
        public int? vehicleId { get; set; }
    }
}
