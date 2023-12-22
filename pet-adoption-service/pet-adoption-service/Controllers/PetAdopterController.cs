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

        [HttpGet("{petAdopterId}")]
        [ProducesResponseType(typeof(List<Pet>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Pet>>> GetPetsOfAdopter(int petAdopterId)
        {
            var returnList = await _petAdopterService.GetAllPetsOfAdoptersAsync(petAdopterId);
            return Ok(returnList);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]

        public async Task<ActionResult<bool>> ApplyForPet(ApplicationDTO applicationDTO)
        {
            var petAdopterId = applicationDTO.petAdopterId;
            var petId = applicationDTO.petId;

            var applicationResult = await _petAdopterService.ApplyForPetAsync(petAdopterId, petId);

            if (applicationResult == 1)
            {
                return Ok(true);
            }
            else if (applicationResult == -1)
            {
                return Conflict("Pet is not available!");
            }
            else if (applicationResult == -2)
            {
                return Conflict("You already applied for this pet!");
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet]
        public async Task<ActionResult<List<AdoptionApplication>?>> GetNewWaitingApplications()
        {
            return await _dbContext.AdoptionApplications.Include(q => q.Pet).Include(q => q.PetAdopter).Include(q => q.Shelter).
                Where(q => q.Status == 1).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdoptionApplication?>> GetAdoptionApplicationById(int id)
        {
            return await _dbContext.AdoptionApplications.Include(q => q.Pet).Include(q => q.PetAdopter).SingleOrDefaultAsync(q => q.AdoptionApplicationId == id);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdoptionApplication?>> ApproveApplication(int id)
        {
            var theApplication = await _dbContext.AdoptionApplications.SingleOrDefaultAsync(q => q.AdoptionApplicationId == id);

            if (theApplication == null)
            {
                return NotFound();
            }
            else if(theApplication.Status != 1)
            {
                return BadRequest();
            }
            else
            {
                theApplication.Status = 2;

                await _dbContext.SaveChangesAsync();

                return Ok(theApplication);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdoptionApplication?>> DeclineApplication(int id)
        {
            var theApplication = await _dbContext.AdoptionApplications.SingleOrDefaultAsync(q => q.AdoptionApplicationId == id);

            if (theApplication == null)
            {
                return NotFound();
            }
            else if (theApplication.Status != 1)
            {
                return BadRequest();
            }
            else
            {
                theApplication.Status = 0;

                await _dbContext.SaveChangesAsync();

                return Ok(theApplication);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<AdoptionApplication>?>> GetApplicationsByAdopterId(int id)
        {
            return await _dbContext.AdoptionApplications.Include(q => q.Pet).Where(q => q.PetAdopterId == id && q.Status == 1).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<AdoptionApplication>?>> GetDeclinesByAdopterId(int id)
        {
            return await _dbContext.AdoptionApplications.Include(q => q.Pet).Where(q => q.PetAdopterId == id && q.Status == 0).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<StatisticsView>> GetStatisticsView()
        {
            return await _petAdopterService.GetMostPopularIdsAsync();
        }
    }
}