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
    /// <summary>
    /// Controller for the rents
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        // GET: api/<RentController>
        private readonly IRentRepository _rentRepository;
		private readonly IAuthRepository _authRepository;

		/// <summary>
		/// Controller contructor
		/// </summary>
		/// <param name="rentRepository">Rent repository</param>
		/// <param name="authRepository">Auth repository</param>
		public RentController(
            IRentRepository rentRepository,
			IAuthRepository authRepository
			)
        {
            _rentRepository = rentRepository;
            _authRepository = authRepository;
        }

        /// <summary>
        /// Gets all rents
        /// </summary>
        /// <param name="skip">nb to skip</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets rents of the connected user
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Authorize]
        public async Task<ActionResult<List<RentDTO>>> GetUserRents()
        {
            try
            {
				var user = await _authRepository.GetLoggedUserFromContext();

                List<RentDTO>? RentDTOs = await _rentRepository.GetUserRents(user);
                return Ok(RentDTOs);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Get the specified rent
        /// </summary>
        /// <param name="id">rentID</param>
        /// <returns></returns>
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
                return NotFound(e.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Post a rent, has to be logged in
        /// </summary>
        /// <param name="postRentDTO">rent data</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Rent>> PostRent(PostRentDTO postRentDTO)
        {
            try
            {
				var user = await _authRepository.GetLoggedUserFromContext();

				Rent rent = await _rentRepository.PostRent(user, postRentDTO);
                return CreatedAtAction("GetRent", new { id = rent.Id }, rent);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Update specified rent, has to be logged in
        /// </summary>
        /// <param name="rentID">rent to update</param>
        /// <param name="updateRentDTO">new rent data</param>
        /// <returns></returns>
        [HttpPut("{rentID}")]
        [Authorize]
        public async Task<ActionResult<Carpool>> UpdateRent(int rentID, UpdateRentDTO updateRentDTO)
        {
            try
            {
				var user = await _authRepository.GetLoggedUserFromContext();

				Rent carpool = await _rentRepository.UpdateRent(user, rentID, updateRentDTO);
                return Ok(carpool);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// delete specified rent, has to be logged in
        /// </summary>
        /// <param name="rentID"></param>
        /// <returns></returns>
        [HttpDelete("{rentID}")]
        [Authorize]
        public async Task<ActionResult> DeleteRent(int rentID)
        {
            try
            {
                var user = await _authRepository.GetLoggedUserFromContext();

                await _rentRepository.DeleteRent(user, rentID);
                return NoContent();
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
