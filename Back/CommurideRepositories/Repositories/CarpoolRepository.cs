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
    public class CarpoolRepository : ICarpoolRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IRentRepository _rentRepository;
        public CarpoolRepository(ApplicationDbContext context, IRentRepository rentRepository)
        {
            _context = context;
            _rentRepository = rentRepository;
        }
        
        /// <summary>
        /// Get a carpool with specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">thrown if carpool not found</exception>
        public async Task<CarpoolDTO> GetCarpool(int id)
        {
            var carpool = await _context.Carpools
                .Include(c => c.Vehicle)
                .Include(c => c.Driver)
                .Select(c => Helpers.CarpoolToCarpoolDTO(c))
                .FirstOrDefaultAsync(c => c.Id == id);
            return carpool == null ? throw new NotFoundException("Le covoiturage n'existe pas.") : carpool;
        }

        /// <summary>
        /// Get a list of carpools matching the parameters in getListCarpoolsDTO
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="getListCarpoolsDTO">search parameters</param>
        /// <returns></returns>
        public async Task<List<CarpoolDTO>> GetCarpools(int? skip, GetListCarpoolsDTO? getListCarpoolsDTO)
        {
            if (getListCarpoolsDTO == null)
            {
                return await _context.Carpools
                    .Include(c => c.Vehicle)
                    .Include(c => c.Driver)
                    .Include(c => c.Passengers)
                    .Skip(skip ?? 0)
                    .Select(c => Helpers.CarpoolToCarpoolDTO(c))
                    .ToListAsync();
            }
            return await _context.Carpools
                .Include(c => c.Vehicle)
                .Include(c => c.Driver)
                .Include(c => c.Passengers)
                // Si nbPassenger pas specifié dans le filtre, verifier avec 0
                .Where(c => (c.Vehicle.NbPlaces - c.Passengers.Count()) >= (getListCarpoolsDTO.numberPassenger ?? 0))
                // Si dateDepart pas specifié dans le filtre, verifier avec maintenant
                .Where(c => c.DateDepart > (getListCarpoolsDTO.dateDepart ?? DateTime.Now))
                .Select(c => Helpers.CarpoolToCarpoolDTO(c))
                .ToListAsync(); // TODO : Filtre sur la localisation, donc gestion de distance par rapport à deux address
        }

        /// <summary>
        /// Update a carpool belonging to the provided appUser
        /// </summary>
        /// <param name="appUser">current user</param>
        /// <param name="carpoolId"></param>
        /// <param name="updateCarpoolDTO">fields to update</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<Carpool> UpdateCarpool(AppUser appUser, int carpoolId, UpdateCarpoolDTO updateCarpoolDTO)
        {

            Carpool? carpool = await _context.Carpools.FirstOrDefaultAsync(c => c.Id == carpoolId);
            if ((carpool == null) || (carpool.Driver.Id != appUser.Id))
            {
                throw new NotFoundException("Le covoiturage n'existe pas ou ne vous appartiens pas");
            }
            if (updateCarpoolDTO.vehicleId != null)
            {
                Vehicle? vehicle = await GetVehicleIfAvailable(appUser, updateCarpoolDTO.dateDepart ?? carpool.DateDepart, updateCarpoolDTO.dateDepart ?? carpool.DateDepart, updateCarpoolDTO.vehicleId ?? default);
                carpool.Vehicle = vehicle;
            }
            carpool.DateDepart = updateCarpoolDTO.dateDepart ?? carpool.DateDepart;
            _context.Update(carpool);
            await _context.SaveChangesAsync();
            return carpool;
        }

        /// <summary>
        /// Post a carpool
        /// </summary>
        /// <param name="appUser"></param>
        /// <param name="carpoolDTO"></param>
        /// <returns></returns>
        public async Task<Carpool> PostCarpool(AppUser appUser, PostCarpoolDTO carpoolDTO)
        {

            Vehicle? vehicle = await GetVehicleIfAvailable(appUser, carpoolDTO.DateDepart, carpoolDTO.DateDepart, carpoolDTO.vehicleId);

            Carpool carpool = new()
            {
                AddressArrival = carpoolDTO.AddressArrival,
                AddressLeaving = carpoolDTO.AddressLeaving,
                DateDepart = carpoolDTO.DateDepart,
                Distance = carpoolDTO.Distance ?? 0, //TODO
                Duration = carpoolDTO.Duration ?? 0, //TOTO
                Driver = appUser,
                Vehicle = vehicle,
                Passengers = [appUser],
            };
            var carpoolEntry = _context.Add(carpool);
            await _context.SaveChangesAsync();
            return carpoolEntry.Entity;
        }

        /// <summary>
        /// Delete a carpool that belongs to the provided appUser
        /// </summary>
        /// <param name="appUser"></param>
        /// <param name="carpoolId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task DeleteCarpool(AppUser appUser, int carpoolId)
        {
            Carpool? carpool = await _context.Carpools.FirstOrDefaultAsync(c => c.Id == carpoolId);
            if ((carpool == null) || (carpool.Driver.Id != appUser.Id))
            {
                throw new NotFoundException("Le covoiturage n'existe pas ou ne vous appartiens pas");
            }
            _context.Remove(carpool);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// returns a vehicle with the provided vehicleId if it belongs to the appuser from startDate to endDate, 
        /// if no vehicle matches throws a notfoundexception
        /// </summary>
        /// <param name="appUser"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<Vehicle> GetVehicleIfAvailable(AppUser appUser, DateTime startDate, DateTime endDate, int vehicleId)
        {
            Vehicle? vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == vehicleId)
                ?? throw new NotFoundException("Le vehicule n'existe pas.");

            List<RentDTO>? Rents = await _rentRepository.GetUserRentsFromPeriodAndVehicleId(appUser, startDate, endDate, vehicleId);

            if (Rents.Count == 0) throw new NotFoundException("Le vehicule ne vous appartiens pas à cette date.");

            return vehicle;
        }
    }
}
