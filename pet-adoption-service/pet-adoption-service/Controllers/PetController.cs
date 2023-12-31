﻿using Microsoft.AspNetCore.Mvc;
using pet_adoption_service.Models;
using pet_adoption_service.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace pet_adoption_service.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private readonly PetService _petService;
        private readonly PetAdoptionDbContext _dbContext;

        public PetController(PetService petService, PetAdoptionDbContext dbContext)
        {
            _petService = petService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Pet>>> GetAllPets()
        {
            var pets = await _petService.GetAllPets();
            return Ok(pets);
        }

        [HttpPost]
        public async Task<ActionResult> AddPet([FromBody] AddPetDTO newPet)
        {
            var pet = new Pet()
            {
                Breed = newPet.Breed,
                Age = newPet.Age,
                IsAvailable = newPet.IsAvailable,
                Gender = newPet.Gender,
                photo_id = newPet.photo_id,
                Name = newPet.Name,
                Status = newPet.Status,
            };

            if (pet == null)
            {
                return BadRequest("Pet data is null");
            }

            await _petService.AddPet(pet, newPet.shelterId);

            return CreatedAtAction(nameof(GetAllPets), new { id = pet.PetId }, pet);
        }

        [HttpGet("{petId}")]
        [ProducesResponseType(typeof(Pet), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Pet>> GetPetById(int petId)
        {

            var thePet = await _petService.GetPetByIdAsync(petId);

            return Ok(thePet);
        }

        [HttpGet("{petId}")]
        [ProducesResponseType(typeof(Shelter), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Shelter>>? GetShelterByPetId(int petId)
        {
            var resultingShelter = await _petService.GetShelterByPetIdAsync(petId);

            return Ok(resultingShelter);
        }

        [HttpGet("{petId}")]
        [ProducesResponseType(typeof(List<HealthRecord>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<HealthRecord>>?> GetHealthRecordOfPet(int petId)
        {
            var healthRecords = await _petService.GetHealthRecordOfPetAsync(petId);

            return Ok(healthRecords);
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<Pet>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Pet>?>> FilterPets(PetFilterDTO filterParams)
        {
            if(filterParams == null)
            {
                return null;
            }

            var minAge = filterParams.minAge;
            var maxAge = filterParams.maxAge;
            var gender = filterParams.sex;
            var breed = filterParams.breed;

            return await _petService.FilterPetsAsync( minAge,  maxAge,  gender, breed);
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<List<Pet>>> SearchPets(string searchTerm)
        {
            var pets = await _petService.SearchPetsAsync(searchTerm);
            if (pets == null || !pets.Any())
            {
                return NotFound("No pets found matching the search term.");
            }

            return Ok(pets);
        }

    }
}