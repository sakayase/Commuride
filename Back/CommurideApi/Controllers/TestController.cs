using CommurideModels.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace CommurideApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        ApplicationDbContext _context;
        public TestController(ApplicationDbContext context) 
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<List<Vehicle>>> getVehicles()
        {
            return await _context.Vehicles.ToListAsync();
        } 
    }
}
