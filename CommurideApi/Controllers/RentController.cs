using CommurideModels.DTOs.Carpool;
using CommurideModels.DTOs.Rent;
using CommurideModels.Exceptions;
using CommurideModels.Models;
using CommurideRepositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommurideApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        // GET: api/<RentController>
        private readonly IRentRepository _rentRepository;
        private readonly UserManager<AppUser> _userManager;

        public RentController(IRentRepository rentRepository, UserManager<AppUser> userManager)
        {
            _rentRepository = rentRepository;
            _userManager = userManager;
        }

        [HttpGet()]
        public async Task<ActionResult<List<RentDTO>>> GetRents(int? skip)
        {
            try
            {
                List<RentDTO>? RentDTOs = await _rentRepository.GetRents(skip: skip);
                return Ok(RentDTOs);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentDTO>> GetRent(int id)
        {
            try
            {
                RentDTO RentDTO = await _rentRepository.GetRent(id);
                return Ok(RentDTO);
            }
            catch (NotFoundException e)
            {
                return NotFound(e);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Rent>> PostRent(PostRentDTO postRentDTO)
        {
            try
            {
                Rent rent = await _rentRepository.PostRent(await GetConnectedUser(), postRentDTO);
                return Created();
            }
            catch (NotFoundException e)
            {
                return NotFound(e);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<Carpool>> UpdateRent(int rentID, UpdateRentDTO updateRentDTO)
        {
            try
            {
                Rent carpool = await _rentRepository.UpdateRent(await GetConnectedUser(), rentID, updateRentDTO);
                return Ok(carpool);
            }
            catch (NotFoundException e)
            {
                return NotFound(e);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteRent(int rentID)
        {
            try
            {
                await _rentRepository.DeleteRent(await GetConnectedUser(), rentID);
                return NoContent();
            }
            catch (NotFoundException e)
            {
                return NotFound(e);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize]
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
