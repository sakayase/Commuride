using CommurideModels.DTOs.Carpool;
using CommurideModels.Exceptions;
using CommurideModels.Models;
using CommurideRepositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace CommurideApi.Controllers
{
    /// <summary>
    /// Controller of carpools
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CarpoolController : ControllerBase
    {
        private readonly ICarpoolRepository _carpoolRepository;
		private readonly IAuthRepository _authRepository;
		/// <summary>
		/// Constructor of carpool controller
		/// </summary>
		/// <param name="carpoolRepository"></param>
		/// <param name="authRepository"></param>
		public CarpoolController(
            ICarpoolRepository carpoolRepository,
			IAuthRepository authRepository
			)
		{
            _carpoolRepository = carpoolRepository;
            _authRepository = authRepository;
        }

        /// <summary>
        /// List all carpools matching the parameters
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="addressArrival"></param>
        /// <param name="addressLeaving"></param>
        /// <param name="dateDepart">minimal date of departure</param>
        /// <param name="numberPassenger">minimal numb of passenger seat left</param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<ActionResult<List<CarpoolDTO>>> GetCarpools(int? skip, string? addressArrival, string? addressLeaving, DateTime dateDepart, int numberPassenger)
        {
            try
            {
                GetListCarpoolsDTO getListCarpoolsDTO = new () 
                { 
                    addressArrival = addressArrival, 
                    addressLeaving = addressLeaving, 
                    dateDepart = dateDepart, 
                    numberPassenger = numberPassenger 
                };
                List<CarpoolDTO>? CarpoolDTOs = await _carpoolRepository.GetCarpools(skip: skip, getListCarpoolsDTO);
                return Ok(CarpoolDTOs);
            } catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Get a carpool by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CarpoolDTO>> GetCarpool(int id)
        {
            try
            {
                CarpoolDTO carpoolDTO = await _carpoolRepository.GetCarpool(id);
                return Ok(carpoolDTO);
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
        /// Post a carpool, have to be logged in 
        /// </summary>
        /// <param name="postCarpoolDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Carpool>> PostCarpool(PostCarpoolDTO postCarpoolDTO)
        {
            try
            {
				var user = await _authRepository.GetLoggedUserFromContext();

				Carpool carpool = await _carpoolRepository.PostCarpool(user, postCarpoolDTO);
                return CreatedAtAction("GetCarpool", new { id = carpool.Id }, carpool);
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
        /// Update a carpool that belongs to the connected user, have to be logged in
        /// </summary>
        /// <param name="carpoolID"></param>
        /// <param name="updateCarpoolDTO"></param>
        /// <returns></returns>
        [HttpPut("{carpoolID}")]
        [Authorize]
        public async Task<ActionResult<Carpool>> UpdateCarpool(int carpoolID, UpdateCarpoolDTO updateCarpoolDTO)
        {
            try
            {
				var user = await _authRepository.GetLoggedUserFromContext();

				Carpool carpool = await _carpoolRepository.UpdateCarpool(user, carpoolID, updateCarpoolDTO);
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
        /// Delete a carpool that belongs to the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteCarpool(int id)
        {
            try
            {
				var user = await _authRepository.GetLoggedUserFromContext();

				await _carpoolRepository.DeleteCarpool(user, id);
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
