using Microsoft.AspNetCore.Identity;

namespace CommurideModels.Models
{
    public class AppUser : IdentityUser
    {
        public string PhotoURL { get; set; }
    }
}
