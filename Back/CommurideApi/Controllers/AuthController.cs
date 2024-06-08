using CommurideModels.DTOs.AppUser;
using CommurideModels.Exceptions;
using CommurideModels.Models;
using CommurideRepositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CommurideApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthRepository _authRepository;
		private readonly IConfiguration _configuration;

		public AuthController(
			IAuthRepository authRepository,
			IConfiguration configuration
			)
		{
			_authRepository = authRepository;
			_configuration = configuration;
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
				string token = GenerateToken(loggedUser.UserName);
				return Ok($"{loggedUser.UserName} {token}");
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


		[HttpGet()]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<AppUser> GetConnectedUser()
		{

			return  await _authRepository.GetLoggedUserFromContext();
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		private string GenerateToken(string userName)
		{

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["Jwt:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: this._configuration["Jwt:Issuer"],
				audience: this._configuration["Jwt:Audience"],
				claims: [
					new Claim("sub", userName)
				],
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);

		}
	}
}
