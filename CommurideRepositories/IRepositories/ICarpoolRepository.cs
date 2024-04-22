using CommurideModels.DTOs.Carpool;
using CommurideModels.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommurideRepositories.IRepositories
{
    public interface ICarpoolRepository
    {
        public Task<List<CarpoolDTO>> GetCarpools(int? skip, GetListCarpoolsDTO? getListCarpoolsDTO);
        public Task<CarpoolDTO> GetCarpool(int id);
        public Task<Carpool> PostCarpool(AppUser appUser, PostCarpoolDTO carpoolDTO);
        public Task<Carpool> UpdateCarpool(AppUser appUser,int carpoolId, UpdateCarpoolDTO carpool);
        public Task DeleteCarpool(AppUser appUser, int carpoolId);

    }
}
