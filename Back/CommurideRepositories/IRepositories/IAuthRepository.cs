using CommurideModels.DTOs.AppUser;
using CommurideModels.Models;
using System.IdentityModel.Tokens.Jwt;

namespace CommurideRepositories.IRepositories
{
    public interface IAuthRepository
    {
        public Task<AppUser> Login(LoginDTO loginDTO);
        public Task Logout();
        public Task<AppUser> Register(CreateAppUserDTO appUserDTO);
        public Task<AppUser> GetLoggedUserFromContext();
        public Task<AppUser> GetUserFromId(string id);
    }
}
