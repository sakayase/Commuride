using CommurideModels.DTOs.Rent;
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

        /// <summary>
        /// Check if a rent period is outside the period between startDate and endDate
        /// </summary>
        /// <param name="rent">Rent to check</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        static public bool IsRentOutsideDateTime(Rent rent, DateTime startDate, DateTime endDate)
        {
            Console.WriteLine($"{rent.DateHourStart} < {startDate} : {rent.DateHourStart < startDate}");
            Console.WriteLine($"{rent.DateHourEnd} < {startDate} : {rent.DateHourEnd < startDate}");
            Console.WriteLine($"{rent.DateHourStart} > {endDate} : {rent.DateHourStart > endDate}");
            Console.WriteLine($"{rent.DateHourEnd} > {endDate} : {rent.DateHourEnd > endDate}");
            var result = ((rent.DateHourStart < startDate) && (rent.DateHourEnd < startDate)) || ((rent.DateHourStart > endDate) && (rent.DateHourEnd > endDate));
            Console.WriteLine(result);
            return result;
        }
    }
}
