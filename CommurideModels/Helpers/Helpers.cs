using CommurideModels.DTOs.AppUser;
using CommurideModels.DTOs.Carpool;
using CommurideModels.DTOs.Rent;
using CommurideModels.DTOs.Vehicle;
using Models;

namespace CommurideModels.Helpers
{
    public static class Helpers
    {
        /// <summary>
        /// Convert a rent entity to a rent DTO
        /// </summary>
        /// <param name="rent"></param>
        /// <returns></returns>
        static public RentDTO RentToRentDTO(Rent rent)
        {
            return new RentDTO()
            {
                Id = rent.Id,
                DateStart = rent.DateHourStart,
                DateEnd = rent.DateHourEnd,
                AppUser = rent.User != null ? new AppUserDTO()
                {
                    Id = rent.User.Id,
                    Username = rent.User.UserName ?? "noname",
                } : null,
                Vehicle = rent.Vehicle != null ? new GetVehicleDTO()
                {
                    Id = rent.Vehicle.Id,
                    Brand = rent.Vehicle.Brand,
                    Category = rent.Vehicle.Category,
                    CO2 = rent.Vehicle.CO2,
                    NbPlaces = rent.Vehicle.NbPlaces,
                    Motorization = rent.Vehicle.Motorization,
                    Model = rent.Vehicle.Model,
                    URLPhoto = rent.Vehicle.URLPhoto,
                    Registration = rent.Vehicle.Registration,
                    Status = rent.Vehicle.Status,
                } : null,
            };
        }
        /// <summary>
        /// Convert a carpool entity to a carpool DTO
        /// </summary>
        /// <param name="carpool"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Check if a rent period is outside the period between startDate and endDate
        /// </summary>
        /// <param name="rent">Rent to check</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        static public bool IsRentOutsideDateTime(Rent rent, DateTime startDate, DateTime endDate)
        {
            var result = ((rent.DateHourStart < startDate) && (rent.DateHourEnd < startDate)) || ((rent.DateHourStart > endDate) && (rent.DateHourEnd > endDate));
            return result;
        }

        /// <summary>
        /// Check if a period (startDate to endDate) is inside the rent period
        /// </summary>
        /// <param name="rent">Rent to check</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        static public bool IsRentInDateTime(Rent rent, DateTime startDate, DateTime endDate)
        {
            var result = (rent.DateHourStart <= startDate) && (rent.DateHourEnd >= endDate);
            return result;
        }

        /// <summary>
        /// Calculate the remaining places in a carpool
        /// </summary>
        /// <param name="carpool"></param>
        /// <returns></returns>
        static public int CalculatePlaceLeft(Carpool carpool)
        {
            var nbPlaces = carpool.Vehicle.NbPlaces;
            var nbPassagers = carpool.Passengers.Count;
            return nbPlaces - nbPassagers;
        }
    }
}
