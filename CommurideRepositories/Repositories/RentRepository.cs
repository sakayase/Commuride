using CommurideModels.DbContexts;
using CommurideModels.DTOs.Carpool;
using CommurideModels.DTOs.Rent;
using CommurideModels.Exceptions;
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
        public async Task<RentDTO> GetRent(int id)
        {
            var rent = await _context.Rents
                 .Include(c => c.Vehicle)
                 .Include(c => c.User)
                 .Select(c => RentToRentDTO(c))
                 .FirstOrDefaultAsync(c => c.Id == id);
            return rent == null ? throw new NotFoundException("La location n'existe pas.") : rent;
        }

        public async Task<List<RentDTO>> GetRents(int? skip)
        {
            var rents = await _context.Rents
                 .Include(c => c.Vehicle)
                 .Include(c => c.User)
                 .Skip(skip ?? 0)
                 .Select(c => RentToRentDTO(c)).ToListAsync();
            return rents;
        }

        public async Task<Rent> PostRent(AppUser appUser, PostRentDTO postRentDTO)
        {
            Vehicle? vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == postRentDTO.VehicleId) ?? throw new NotFoundException("Le vehicule n'existe pas."); ;
            //TODO check si le vehicule lui appartient à la date de depart
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

        public async Task<Rent> UpdateRent(AppUser appUser, int rentId, UpdateRentDTO updateRentDto)
        {
            Rent? rent = await _context.Rents.FirstOrDefaultAsync(c => c.Id == rentId);
            if ((rent == null) || (rent.User.Id != appUser.Id))
            {
                throw new NotFoundException("La location n'existe pas ou ne vous appartiens pas");
            }
            if (updateRentDto.VehicleId != null)
            {
                Vehicle? vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == updateRentDto.VehicleId); //TODO Check si le vehicule est dispo
                if (vehicle == null)
                {
                    throw new NotFoundException("Le vehicule n'existe pas ou ne vous appartiens pas");
                }
                rent.Vehicle = vehicle;
            }
            rent.DateHourStart = updateRentDto.StartDate ?? rent.DateHourStart;
            rent.DateHourEnd = updateRentDto?.EndDate ?? rent.DateHourEnd;

            _context.Update(rent);
            await _context.SaveChangesAsync();
            return rent;
        }
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

        static public RentDTO RentToRentDTO(Rent rent)
        {
            return new RentDTO()
            {
                Id = rent.Id,
                DateStart = rent.DateHourStart,
                DateEnd = rent.DateHourEnd,
                AppUser = new RentAppUserDTO()
                {
                    Id = rent.User.Id,
                    Username = rent.User.UserName ?? "noname",
                },
                Vehicle = new RentVehicleDTO()
                {
                    Id = rent.Vehicle.Id,
                    Brand = rent.Vehicle.Brand,
                    Category = rent.Vehicle.Category,
                    CO2 = rent.Vehicle.CO2,
                    NbPlaces = rent.Vehicle.NbPlaces,
                    Motorization = rent.Vehicle.Motorization,
                    Model = rent.Vehicle.Model,
                    URLPhoto = rent.Vehicle.URLPhoto,
                },
            };
        }

    }
}
