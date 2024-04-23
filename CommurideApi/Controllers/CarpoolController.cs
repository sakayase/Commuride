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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CarpoolController : ControllerBase
    {
        private readonly ICarpoolRepository _carpoolRepository;
        private readonly UserManager<AppUser> _userManager;

        public CarpoolController(ICarpoolRepository carpoolRepository, UserManager<AppUser> userManager)
        {
            _carpoolRepository = carpoolRepository;
            _userManager = userManager;
        }

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
                return NotFound(e);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Carpool>> PostCarpool(PostCarpoolDTO postCarpoolDTO)
        {
            try
            {
                Carpool carpool = await _carpoolRepository.PostCarpool(await GetConnectedUser(), postCarpoolDTO);
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
        public async Task<ActionResult<Carpool>> UpdateCarpool(int carpoolID, UpdateCarpoolDTO updateCarpoolDTO)
        {
            try
            {
                Carpool carpool = await _carpoolRepository.UpdateCarpool(await GetConnectedUser(), carpoolID, updateCarpoolDTO);
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
        public async Task<ActionResult> DeleteCarpool(int carpoolID)
        {
            try
            {
                await _carpoolRepository.DeleteCarpool(await GetConnectedUser(), carpoolID);
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
