using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommurideModels.DTOs.Carpool
{
    public class GetListCarpoolsDTO
    {
        public string? addressLeaving { get; set; }
        public string? addressArrival { get; set; }
        public int? numberPassenger { get; set; }
        public DateTime? dateDepart { get; set; }
    }
}
