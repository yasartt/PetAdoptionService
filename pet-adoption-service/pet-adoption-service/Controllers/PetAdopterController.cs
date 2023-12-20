using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pet_adoption_service.Models;
using pet_adoption_service.Services;
using System.Net;
using System.Threading.Tasks;

namespace pet_adoption_service.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PetAdopterController : ControllerBase
    {
        private readonly PetAdopterService _petAdopterService;
        private readonly PetAdoptionDbContext _dbContext;

        public PetAdopterController(PetAdopterService petAdopterService, PetAdoptionDbContext dbContext)
        {
            _petAdopterService = petAdopterService;
            _dbContext = dbContext;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PetAdopter), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PetAdopter>> RegisterAdopter(PetAdopter newAdopter)
        {
            if (await _dbContext.PetAdopters.AnyAsync(q => q.Username == newAdopter.Username))
            {
                return Conflict("Username is already in use");
            }

            var newAdopterResult = await _petAdopterService.AddAdopter(newAdopter);
            if (newAdopterResult != null)
            {
                return Ok(newAdopterResult);
            }
            return BadRequest("Unable to register pet adopter");
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PetAdopter>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<PetAdopter>>> GetAllAdopters()
        {
            var adopters = await _petAdopterService.GetAllAdopters();
            return Ok(adopters);
        }
        
        [HttpGet("petAdopterId")]
        [ProducesResponseType(typeof(List<Pet>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Pet>>> GetPetsOfAdopter(int petAdopterId)
        {
            return await _petAdopterService.GetAllPetsOfAdoptersAsync(petAdopterId);
        }

    }
}