
using CommurideModels.DTOs.Vehicle;
using CommurideModels.Exceptions;
using CommurideModels.Models;
using CommurideRepositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace CommurideApi.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly UserManager<AppUser> _userManager;

        public VehicleController(IVehicleRepository vehicleRepository, UserManager<AppUser> userManager)
        {
            this._vehicleRepository = vehicleRepository;
            this._userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetAllVehicleDTO>>> GetAllVehicles()
        {
            return await _vehicleRepository.GetAll();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GetVehicleDTO>>> GetVehicle(int id)
        {
            GetVehicleDTO? vehicleDTO = await _vehicleRepository.Get(id);
            if (vehicleDTO == null) { return NotFound(); }
            return Ok(vehicleDTO);
        }

        [HttpPut("{id}")]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Vehicle>> UpdateVehicle(int id, UpdateVehicleDTO vehicleDTO)
        {
            Vehicle vehicle = await _vehicleRepository.UpdateVehicle(id, vehicleDTO);
            return Ok(vehicle);
        }

        [HttpPost]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Vehicle>> PostVehicle(CreateVehicleDTO vehicleDTO)
        {
            Vehicle vehicle= await _vehicleRepository.CreateVehicle(vehicleDTO);
            return CreatedAtAction("GetVehicle", new {id = vehicle.Id}, vehicle);
        }

        [HttpDelete("Delete/{id}")]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            await _vehicleRepository.DeleteVehicle(id);
            return NoContent();
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