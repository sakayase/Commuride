using System.ComponentModel.DataAnnotations;

namespace CommurideModels.DTOs.AppUser
{
    public class CreateAppUserDTO
    {
        public required string Username { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password2", ErrorMessage = "Le mot de passe doit être identique")]
        public required string Password { get; set; }
        public required string Password2 { get; set; }
        public required DateTime BirthDate { get; set; }
    }
}
