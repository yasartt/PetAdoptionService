using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using pet_adoption_service.Models;
using System.Net;

namespace pet_adoption_service.Services
{
    public class ShelterService
    {
        private readonly PetAdoptionDbContext _dbContext;

        public ShelterService(PetAdoptionDbContext dbContext)
        {

            _dbContext = dbContext;

        }

        // Use GetAllPetsOfSheltersAsync instead GetPetsByShelterAsync
        [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
        public async Task<List<Pet>> GetPetsByShelterAsync(int shelterId)
        {
            string sqlCommand = "SELECT pet.* FROM pet JOIN stay ON pet.pet_id = stay.pet_id WHERE stay.shelter_id = @ShelterId AND stay.is_Current = 1";

            var pets = await _dbContext.Pets
                        .FromSqlRaw(sqlCommand, new SqlParameter("@ShelterId", shelterId))
                        .ToListAsync();

            return pets.Count > 0 ? pets : null;
        }


        public async Task<ShelterBusyHoursView> GetShelterBusyHoursAsync(int shelterId)
        {
            try
            {
                var theShelter = await _dbContext.Shelters
                                    .FromSqlRaw("SELECT * FROM shelter WHERE user_id = @ShelterId", new SqlParameter("@ShelterId", shelterId))
                                    .SingleOrDefaultAsync();


                var appointmentList = await _dbContext.ShelterAppointments
                                    .FromSqlRaw("SELECT * FROM shelter_appointments WHERE shelter_id = @ShelterId", new SqlParameter("@ShelterId", shelterId))
                                    .ToListAsync();


                return new ShelterBusyHoursView()
                {
                    Appointments = appointmentList,
                    RestrictedHours = theShelter.RestrictedHours,
                };

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<Boolean> AddAppointmentAsync(int vetId, int petId, DateTime date)
        {
            // Check if the veterinarian exists
            var vetExistsCommand = "SELECT COUNT(1) FROM veterinarian WHERE user_id = @VetId";
            var vetExists = await _dbContext.Database.ExecuteSqlRawAsync(vetExistsCommand, new SqlParameter("@VetId", vetId)) > 0;

            // Check if the pet exists
            var petExistsCommand = "SELECT COUNT(1) FROM pet WHERE pet_id = @PetId";
            var petExists = await _dbContext.Database.ExecuteSqlRawAsync(petExistsCommand, new SqlParameter("@PetId", petId)) > 0;

            if (!vetExists || !petExists)
            {
                return false;
            }

            // Insert the new appointment
            string insertCommand = "INSERT INTO vet_appointments (vet_id, pet_id, appointment_date) VALUES (@VetId, @PetId, @Date)";
            await _dbContext.Database.ExecuteSqlRawAsync(insertCommand,
                new SqlParameter("@VetId", vetId),
                new SqlParameter("@PetId", petId),
                new SqlParameter("@Date", date)
            );

            return true;
        }


        public async Task<List<Pet>> GetAllPetsOfSheltersAsync(int shelterId)
        {
            string sqlCommand = "SELECT pet.* FROM pet JOIN stay ON pet.pet_id = stay.pet_id WHERE stay.shelter_id = @ShelterId";

            var pets = await _dbContext.Pets
                        .FromSqlRaw(sqlCommand, new SqlParameter("@ShelterId", shelterId))
                        .ToListAsync();

            return pets.Count > 0 ? pets : null;
        }


        public async Task<Shelter> GetShelterByIdAsync(int shelterId)
        {
            var resultShelter = await _dbContext.Shelters
                                .FromSqlRaw("SELECT * FROM shelter WHERE user_id = @ShelterId",
                                            new SqlParameter("@ShelterId", shelterId))
                                .SingleOrDefaultAsync();

            return resultShelter;
        }

    }
}
