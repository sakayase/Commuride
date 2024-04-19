using CommurideModels.DTOs.AppUser;
using CommurideModels.Exceptions;
using CommurideModels.Models;
using CommurideRepositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CommurideApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(
            IAuthRepository authRepository,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _authRepository = authRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet()]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAppUser()
        {
            var appUser = await GetConnectedUser();
            return Ok($"{appUser.UserName}");
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(CreateAppUserDTO appUserDTO)
        {
            try
            {
                AppUser createdUser = await _authRepository.Register(appUserDTO);
                return Ok($"User created at ID:{createdUser.Id} !");
            }
            catch (SignInException e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                AppUser loggedUser = await _authRepository.Login(loginDTO);
                return Ok($"User logged ({loggedUser.UserName})");
            }
            catch (LoginException e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet()]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authRepository.Logout();
                return Ok("User Disconnected");
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }


        //TODO: Sortir ca dans le repo
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<AppUser> GetConnectedUser()
        {
            var appUser = await _userManager.GetUserAsync(User);
            if (appUser == null)
            {
                throw new GetConnectedUserException();
            }
            return appUser;
        }
    }
}
