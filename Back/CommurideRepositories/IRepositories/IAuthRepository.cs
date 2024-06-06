using CommurideModels.DTOs.AppUser;
using CommurideModels.Models;

namespace CommurideRepositories.IRepositories
{
    public interface IAuthRepository
    {
        public Task<AppUser> Login(LoginDTO loginDTO);
        public Task Logout();
        public Task<AppUser> Register(CreateAppUserDTO appUserDTO);
        public Task<AppUser> GetLoggedUser();
        public Task<AppUser> GetUserFromId(string id);
    }
}
