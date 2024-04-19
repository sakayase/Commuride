using System.ComponentModel.DataAnnotations;

namespace CommurideModels.DTOs.AppUser
{
    public class LoginDTO
    {
        public required string Username{ get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
