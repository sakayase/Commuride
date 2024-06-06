using CommurideModels.DbContexts;
using CommurideModels.DTOs.Carpool;
using CommurideModels.DTOs.Rent;
using CommurideModels.Exceptions;
using CommurideModels.Helpers;
using CommurideModels.Models;
using CommurideRepositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Models;

namespace CommurideRepositories.Repositories
{
    public class RentRepository : IRentRepository
    {
        private readonly ApplicationDbContext _context;
        public RentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Get a rent with specified id
        /// </summary>
        /// <param name="id">id of the rent to return</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<RentDTO> GetRent(int id)
        {
            var rent = await _context.Rents
                 .Include(c => c.Vehicle)
                 .Include(c => c.User)
                 .Select(c => Helpers.RentToRentDTO(c))
                 .FirstOrDefaultAsync(c => c.Id == id);
            return rent == null ? throw new NotFoundException("La location n'existe pas.") : rent;
        }

        /// <summary>
        /// Get All rents
        /// </summary>
        /// <param name="skip">nb items to skip</param>
        /// <returns></returns>
        public async Task<List<RentDTO>> GetRents(int? skip)
        {
            var rents = await _context.Rents
                 .Include(c => c.Vehicle)
                 .Include(c => c.User)
                 .Skip(skip ?? 0)
                 .Select(c => Helpers.RentToRentDTO(c)).ToListAsync();
            return rents;
        }

        /// <summary>
        /// Post a rent that belongs to the specified user
        /// </summary>
        /// <param name="appUser"></param>
        /// <param name="postRentDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<Rent> PostRent(AppUser appUser, PostRentDTO postRentDTO)
        {
            if (postRentDTO.StartDate >= postRentDTO.EndDate) 
            {
                throw new Exception("La date de départ doit etre avant la date de fin");
            }
            Vehicle? vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => (v.Id == postRentDTO.VehicleId && v.Status == Vehicle.StatusVehicle.Service)) 
                ?? throw new NotFoundException("Le vehicule n'existe pas.");
            //Check if vehicle is available at queried dates
            if (!await IsVehicleAvailable(vehicle, postRentDTO.StartDate, postRentDTO.EndDate))
            {
                throw new Exception("Le vehicule n'est pas disponible à ces dates.");
            }
            Rent rent = new()
            {
                DateHourStart = postRentDTO.StartDate,
                DateHourEnd = postRentDTO.EndDate,
                User = appUser,
                Vehicle = vehicle,
            };
            var rentEntry = _context.Add(rent);
            await _context.SaveChangesAsync();
            return rentEntry.Entity;
        }

        /// <summary>
        /// Update rent if it exists, and it belongs to the specified appUser
        /// </summary>
        /// <param name="appUser">Current appuser</param>
        /// <param name="rentId">Id of the rent to modify</param>
        /// <param name="updateRentDto">Objects with the modifiable data</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<Rent> UpdateRent(AppUser appUser, int rentId, UpdateRentDTO updateRentDto)
        {
            Rent? rent = await _context.Rents.Include(r => r.Vehicle).FirstOrDefaultAsync(r => r.Id == rentId);
            if ((rent == null) || (rent.User.Id != appUser.Id))
            {
                throw new NotFoundException("La location n'existe pas ou ne vous appartiens pas");
            }
            if (updateRentDto.VehicleId != null)
            {
                Vehicle vehicle = await _context.Vehicles
                    .FirstOrDefaultAsync(v => v.Id == updateRentDto.VehicleId) 
                    ?? throw new NotFoundException("Le vehicule n'existe pas ou ne vous appartiens pas");

                rent.Vehicle = vehicle;
            }
            rent.DateHourStart = updateRentDto.StartDate ?? rent.DateHourStart;
            rent.DateHourEnd = updateRentDto?.EndDate ?? rent.DateHourEnd;
            if (rent.DateHourStart >= rent.DateHourEnd)
            {
                throw new Exception("La date de départ doit etre avant la date de fin");
            }
            //Check if vehicle is available (updated before if it is a new vehicle) is available at new dates (ommiting the current rent)
            if (!await IsVehicleAvailable(rent.Vehicle, rent.DateHourStart, rent.DateHourEnd, rentId))
            {
                throw new Exception("Le vehicule n'est pas disponible à ces dates.");
            }

            _context.Update(rent);
            await _context.SaveChangesAsync();
            return rent;
        }

        /// <summary>
        /// Delete a rent if it belongs to the specified appUser
        /// </summary>
        /// <param name="appUser">Current user</param>
        /// <param name="rentId">Id of the rend to delete</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task DeleteRent(AppUser appUser, int rentId)
        {
            Rent? rent = await _context.Rents.FirstOrDefaultAsync(c => c.Id == rentId);
            if ((rent == null) || (rent.User.Id != appUser.Id))
            {
                throw new NotFoundException("Le covoiturage n'existe pas ou ne vous appartiens pas");
            }
            _context.Remove(rent);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Check if the vehicle is available between specified dates (able to ignore a rent if you specify its id, in order to modify the rent)
        /// </summary>
        /// <param name="vehicle">Vehicle to check</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="rentId">id of the rent to ignore</param>
        /// <returns></returns>
        public async Task<bool> IsVehicleAvailable(Vehicle vehicle, DateTime startDate, DateTime endDate, int? rentId = null) 
        {
            var Rents = await _context.Rents
                .Where(r => r.Vehicle.Id == vehicle.Id)
                .ToListAsync();
            var FilteredRents = Rents.Where(r => !Helpers.IsRentOutsideDateTime(r, startDate, endDate)).ToList();
            if (rentId != null)
            {
                FilteredRents = FilteredRents.Where(r => r.Id != rentId).ToList();
            }
            if ((FilteredRents == null) || (FilteredRents.Count == 0))
            {
                return true;
            }
            return false;
        }

        public async Task<List<RentDTO>> GetUserRents(AppUser appUser) 
        {
            return await _context.Rents
                .Where(r => r.User.Id == appUser.Id)
                .Select(r => Helpers.RentToRentDTO(r))
                .ToListAsync();
        }

        public async Task<List<RentDTO>> GetUserRentsFromPeriodAndVehicleId(AppUser appUser, DateTime startDate, DateTime endDate, int vehicleId)
        {
            var Rents = await _context.Rents
                .Where(r => r.User.Id == appUser.Id)
                .Where(r => r.Vehicle.Id == vehicleId)
                .ToListAsync();
            return Rents
                .Where(r => Helpers.IsRentInDateTime(r, startDate, endDate))
                .Select(r => Helpers.RentToRentDTO(r))
                .ToList();
        }
    }
}
