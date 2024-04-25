
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GetAllVehicleDTO>>> GetAllVehicles()
        {
            return await _vehicleRepository.GetAll();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetVehicleDTO>> GetVehicle(int id)
        {
            var vehicles = await _vehicleRepository.Get(id);
            if (vehicles == null)
            {
                return NotFound("Vehicle not found.");
            }
            return Ok(vehicles);
        }

        [HttpGet("{registration}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetVehicleDTO>> GetVehicleByRegistration (string registration)
        { 
            var vehicles = await _vehicleRepository.GetVehicleByRegistration(registration);
            if (vehicles == null)
            {
                return NotFound("Vehicle not found.");
            }
            return Ok(vehicles);
        }

        [HttpGet("{brand}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GetVehicleDTO>>> GetVehicleByBrand (string brand)
        { 
            return await _vehicleRepository.GetVehicleByBrand(brand);
        }

        [HttpPut("{id}")]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVehicle(int id, UpdateVehicleDTO vehicleDTO)
        {
            if (vehicleDTO == null)
            {
                return BadRequest("Vehicle is null");
            }

            var vehicleToUpdate = await _vehicleRepository.Get(id);
            if(vehicleToUpdate == null)
            {
                return NotFound("Vehicle counldn't be found");
            }

            await _vehicleRepository.UpdateVehicle( vehicleDTO);

            return NoContent();
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
    }
}