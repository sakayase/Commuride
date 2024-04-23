using CommurideModels.DTOs.Carpool;
using CommurideModels.DTOs.Rent;
using CommurideModels.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommurideRepositories.IRepositories
{
    public interface IRentRepository
    {
        public Task<List<RentDTO>> GetRents(int? skip);
        public Task<RentDTO> GetRent(int id);
        public Task<Rent> PostRent(AppUser appUser, PostRentDTO postRentDto);
        public Task<Rent> UpdateRent(AppUser appUser, int rentId, UpdateRentDTO carpool);
        public Task DeleteRent(AppUser appUser, int rentId);
    }
}
