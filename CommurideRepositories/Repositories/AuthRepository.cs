using CommurideModels.DbContexts;
using CommurideModels.DTOs.AppUser;
using CommurideModels.Exceptions;
using CommurideModels.Models;
using CommurideRepositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CommurideRepositories.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AuthRepository(
            ApplicationDbContext dbContext,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
            )
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public Task<AppUser> GetLoggedUser()
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> GetUserFromId(string id)
        {
            return await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == id) ?? throw new NotFoundException();
        }

        public async Task<AppUser> Login(LoginDTO loginDTO)
        {
            AppUser? AppUser = await _userManager.FindByNameAsync(loginDTO.Username);
            if (AppUser == null)
            {
                throw new LoginException(message: "The user doesnt exist, or the password doesnst match.");
            }
            SignInResult result = await _signInManager.PasswordSignInAsync(AppUser, loginDTO.Password, false, false);

            if (result.Succeeded)
            {
                return AppUser;
            }
            else
            {
                throw new LoginException(message: "The user doesnt exist, or the password doesnst match.");
            }
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AppUser> Register(CreateAppUserDTO appUserDTO)
        {

            var User = new AppUser { UserName =  appUserDTO.Username};
            var result = await _userManager.CreateAsync(User, appUserDTO.Password);

            if (result.Succeeded && User != null)
            {
                return User;
            }
            else
                throw new SignInException(message: string.Join(" | ", result.Errors.Select(e => e.Description)));
        }
    }
}
