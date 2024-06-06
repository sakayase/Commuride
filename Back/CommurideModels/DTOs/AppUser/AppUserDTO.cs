using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommurideModels.DTOs.AppUser
{
    public class AppUserDTO
    {
        public required string Id { get; set; }
        public string? URLPhoto { get; set; }
        public required string Username { get; set; }
    }
}
