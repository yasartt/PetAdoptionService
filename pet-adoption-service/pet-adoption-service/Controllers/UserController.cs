using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pet_adoption_service.Models;
using pet_adoption_service.Services;
using System.Net;

namespace pet_adoption_service.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly PetAdoptionDbContext _dbContext;

        public UserController(UserService userService, PetAdoptionDbContext dbContext)
        {
             _userService = userService;
            _dbContext = dbContext;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Veterinarian), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Veterinarian>> RegisterVet(Veterinarian newVet)
        {
            if (await _dbContext.Veterinarians.AnyAsync(q => q.Username == newVet.Username))
            {
                return Conflict("Kullanıcı adı kullanımda");
            }

            var newVetResult = await _userService.AddVetAsync(newVet);
            if (newVetResult != null)
            {
                return Ok(newVetResult);
            }
            return BadRequest();
        }

        [HttpPost]
        [ProducesResponseType(typeof(PetAdopter), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PetAdopter>> RegisterAdopter(PetAdopter newAdopter)
        {
            if (await _dbContext.PetAdopters.AnyAsync(q => q.Username == newAdopter.Username))
            {
                return Conflict("Kullanıcı adı kullanımda");
            }

            var newAdopterResult = await _userService.AddAdopterAsync(newAdopter);
            if (newAdopterResult != null)
            {
                return Ok(newAdopterResult);
            }
            return BadRequest();
        }

        [HttpPost]
        [ProducesResponseType(typeof(Shelter), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Shelter>> RegisterShelter(Shelter shelter)
        {
            if (await _dbContext.Shelters.AnyAsync(q => q.Username == shelter.Username))
            {
                return Conflict("Kullanıcı adı kullanımda");
            }

            var newAdopterResult = await _userService.AddShelterAsync(shelter);
            if (newAdopterResult != null)
            {
                return Ok(newAdopterResult);
            }
            return BadRequest();
        }

        [HttpGet]

        public async Task<ActionResult<Veterinarian>> GetAllUsers()
        {
            return await _dbContext.Veterinarians.SingleOrDefaultAsync(q => q.UserId == 1);
        }


        [HttpGet("{userType}/{username}/{password}")]
        [ProducesResponseType(typeof(SessionView), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<SessionView>> LoginUser(string userType, string userName, string password)
        {
            var loginResult = await _userService.LoginUserAsync(userType, userName, password);

            return Ok(loginResult);
        }

    }
}
