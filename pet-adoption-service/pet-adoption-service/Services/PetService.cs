using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using pet_adoption_service.Models;

namespace pet_adoption_service.Services
{
    public interface IPetService
    {
        Task<IEnumerable<Pet>> GetAllPets();
        Task AddPet(Pet pet);
    }

    public class PetService
    {
        private readonly PetAdoptionDbContext _dbContext;

        public PetService(PetAdoptionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Pet>> GetAllPets()
        {
            return await _dbContext.Pets.Where(q => q.IsAvailable == 1).ToListAsync();
        }

        public async Task<bool> AddPet(Pet pet)
        {
            try
            {
                await _dbContext.Pets.AddAsync(pet);
                await _dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public async Task<Shelter?> GetShelterByPetIdAsync(int petId)
        {
            var stay = await _dbContext.Stays.Include(q => q.Shelter).
                SingleOrDefaultAsync(q => q.PetId == petId && q.is_Current == true);

            if (stay == null)
            {
                return null;
            }

            var shelter = stay.Shelter;
            
            return shelter;
        }

        public async Task<List<HealthRecord>?> GetHealthRecordOfPetAsync(int petId)
        {
            var healthRecords = await _dbContext.HealthRecords.Where(q => q.PetId == petId).ToListAsync();

            return healthRecords;
        }

        public async Task<Pet> GetPetByIdAsync(int petId)
        {
            return await _dbContext.Pets.SingleOrDefaultAsync(q => q.PetId == petId);
        }

        public async Task<List<Pet>?> FilterPetsAsync(int minAge, int maxAge, string gender, string breed)
        {
            var filteredList = await _dbContext.Pets.Where(q => q.Age > minAge && q.Age < maxAge && q.Gender == gender && q.Breed == breed).ToListAsync();
            return filteredList;
        }
    }
}
