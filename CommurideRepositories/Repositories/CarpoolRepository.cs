using CommurideModels.DbContexts;
using CommurideModels.DTOs.Carpool;
using CommurideModels.Exceptions;
using CommurideModels.Models;
using CommurideRepositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Models;

namespace CommurideRepositories.Repositories
{
    public class CarpoolRepository : ICarpoolRepository
    {
        private readonly ApplicationDbContext _context;
        public CarpoolRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CarpoolDTO> GetCarpool(int id)
        {
            var carpool = await _context.Carpools
                .Include(c => c.Vehicle)
                .Include(c => c.Driver)
                .Select(c => CarpoolToCarpoolDTO(c))
                .FirstOrDefaultAsync(c => c.Id == id);
            return carpool == null ? throw new NotFoundException("Le covoiturage n'existe pas.") : carpool;
        }

        public async Task<List<CarpoolDTO>> GetCarpools(int? skip, GetListCarpoolsDTO? getListCarpoolsDTO)
        {
            if (getListCarpoolsDTO == null)
            {
                return await _context.Carpools
                    .Include(c => c.Vehicle)
                    .Include(c => c.Driver)
                    .Include(c => c.Passengers)
                    .Skip(skip ?? 0)
                    .Select(c => CarpoolToCarpoolDTO(c))
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
                .Select(c => CarpoolToCarpoolDTO(c))
                .ToListAsync(); // TODO : Filtre sur la localisation, donc gestion de distance par rapport à deux address
        }

        public async Task<Carpool> UpdateCarpool(AppUser appUser, int carpoolId, UpdateCarpoolDTO updateCarpoolDTO)
        {
            Carpool? carpool = await _context.Carpools.FirstOrDefaultAsync(c => c.Id == carpoolId);
            if ((carpool == null) || (carpool.Driver.Id != appUser.Id))
            {
                throw new NotFoundException("Le covoiturage n'existe pas ou ne vous appartiens pas");
            }
            if (updateCarpoolDTO.vehicleId != null)
            {
                var vehicle = await _context.Vehicles
                    .FirstOrDefaultAsync(v => v.Id == updateCarpoolDTO.vehicleId)
                    ?? throw new NotFoundException("Le vehicule n'existe pas.");
                carpool.Vehicle = vehicle;
            }
            carpool.DateDepart = updateCarpoolDTO.dateDepart ?? carpool.DateDepart;
            _context.Update(carpool);
            await _context.SaveChangesAsync();
            return carpool;
        }

        public async Task<Carpool> PostCarpool(AppUser appUser, PostCarpoolDTO carpoolDTO)
        {
            Vehicle? vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == carpoolDTO.vehicleId) ?? throw new NotFoundException("Le vehicule n'existe pas."); ;
            //TODO check si le vehicule lui appartient à la date de depart
            Carpool carpool = new()
            {
                AddressArrival = carpoolDTO.AddressArrival,
                AddressLeaving = carpoolDTO.AddressLeaving,
                DateDepart = carpoolDTO.DateDepart,
                Distance = 0, //TODO
                Duration = 0, //TOTO
                Driver = appUser,
                Vehicle = vehicle,
            };
            var carpoolEntry = _context.Add(carpool);
            await _context.SaveChangesAsync();
            return carpoolEntry.Entity;
        }

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

        static public int CalculatePlaceLeft(Carpool carpool)
        {
            var nbPlaces = carpool.Vehicle.NbPlaces;
            var nbPassagers = carpool.Passengers.Count;
            return nbPlaces - nbPassagers;
        }

        static public CarpoolDTO CarpoolToCarpoolDTO(Carpool carpool)
        {
            return new CarpoolDTO()
            {
                AddressArrival = carpool.AddressArrival,
                AddressLeaving = carpool.AddressLeaving,
                DateDepart = carpool.DateDepart,
                Id = carpool.Id,
                Driver = carpool.Driver != null ? new CarpoolAppUserDTO()
                {
                    Id = carpool.Driver.Id,
                    Username = carpool.Driver.UserName ?? "noname",
                } : null,
                VehicleDTO = new CarpoolVehicleDTO()
                {
                    Id = carpool.Vehicle.Id,
                    Brand = carpool.Vehicle.Brand,
                    Category = carpool.Vehicle.Category,
                    CO2 = carpool.Vehicle.CO2,
                    NbPlaces = carpool.Vehicle.NbPlaces,
                    Motorization = carpool.Vehicle.Motorization,
                    Model = carpool.Vehicle.Model,
                    URLPhoto = carpool.Vehicle.URLPhoto,
                },
                Passengers = carpool.Passengers
                    .Select(u => new CarpoolAppUserDTO()
                {
                    Id = u.Id,
                    Username = u.UserName ?? "noname",
                }).ToList(),
            };
        }
    }
}
