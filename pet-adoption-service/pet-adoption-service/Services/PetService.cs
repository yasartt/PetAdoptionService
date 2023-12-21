using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
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
            return await _dbContext.Pets
                    .FromSqlRaw("SELECT * FROM pet WHERE is_available = 1")
                    .ToListAsync();
        }


        public async Task<bool> AddPet(Pet pet)
        {
            try
            {
                string sqlCommand = "INSERT INTO pet (breed, age, name, status, gender, is_available, photo_id) VALUES (@Breed, @Age, @Name, @Status, @Gender, @IsAvailable, @photo_id)";

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Breed", pet.Breed ?? (object)DBNull.Value),
                    new SqlParameter("@Age", pet.Age),
                    new SqlParameter("@Name", pet.Name ?? (object)DBNull.Value),
                    new SqlParameter("@Status", pet.Status ?? (object)DBNull.Value),
                    new SqlParameter("@Gender", pet.Gender ?? (object)DBNull.Value),
                    new SqlParameter("@IsAvailable", pet.IsAvailable.HasValue ? (pet.IsAvailable.Value != 0) : (object)DBNull.Value),
                    new SqlParameter("@photo_id", pet.photo_id ?? (object)DBNull.Value)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(sqlCommand, parameters.ToArray());

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<Shelter?> GetShelterByPetIdAsync(int petId)
        {
            var query = @"
                        SELECT shelter.* 
                        FROM shelter 
                        JOIN stay ON shelter.user_id = stay.shelter_id 
                        WHERE stay.pet_id = @PetId AND stay.is_Current = 1";

            var shelter = await _dbContext.Shelters
                        .FromSqlRaw(query, new SqlParameter("@PetId", petId))
                        .SingleOrDefaultAsync();

            return shelter;
        }


        public async Task<List<HealthRecord>?> GetHealthRecordOfPetAsync(int petId)
        {
            return await _dbContext.HealthRecords
                    .FromSqlRaw("SELECT * FROM health_record WHERE pet_id = @PetId",
                        new SqlParameter("@PetId", petId))
                    .ToListAsync();
        }

        public async Task<Pet> GetPetByIdAsync(int petId)
        {
            return await _dbContext.Pets
                    .FromSqlRaw("SELECT * FROM pet WHERE pet_id = @PetId",
                        new SqlParameter("@PetId", petId))
                    .SingleOrDefaultAsync();
        }


        public async Task<List<Pet>?> FilterPetsAsync(int? minAge, int? maxAge, string? gender, string? breed)
        {
            var query = @"
                        SELECT * FROM pet 
                        WHERE (@MinAge IS NULL OR age >= @MinAge) 
                        AND (@MaxAge IS NULL OR age <= @MaxAge)
                        AND (@Gender IS NULL OR gender = @Gender) 
                        AND (@Breed IS NULL OR breed = @Breed)";

            var filteredList = await _dbContext.Pets
                        .FromSqlRaw(query,
                            new SqlParameter("@MinAge", minAge ?? (object)DBNull.Value),
                            new SqlParameter("@MaxAge", maxAge ?? (object)DBNull.Value),
                            new SqlParameter("@Gender", gender ?? (object)DBNull.Value),
                            new SqlParameter("@Breed", breed ?? (object)DBNull.Value))
                        .ToListAsync();

            return filteredList;
        }

        public async Task<List<Pet>> SearchPetsAsync(string searchTerm)
        {
            string query = @"
                            SELECT * FROM pet 
                            WHERE breed LIKE @SearchTerm 
                            OR name LIKE @SearchTerm 
                            OR status LIKE @SearchTerm
                            OR gender LIKE @SearchTerm";

            var searchTermFormatted = $"%{searchTerm}%";

            return await _dbContext.Pets
                            .FromSqlRaw(query, new SqlParameter("@SearchTerm", searchTermFormatted))
                            .ToListAsync();
        }

    }
}
