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

        public async Task<Boolean> AddAppointmentAsync(int shelterId, int petAdopterId, DateTime date)
        {
            // Check if the shelter exists
            var shelterExistsCommand = "SELECT COUNT(1) FROM shelter WHERE user_id = @ShelterId";
            var shelterExists = await _dbContext.Database.ExecuteSqlRawAsync(shelterExistsCommand, new SqlParameter("@ShelterId", shelterId)) > 0;

            // Check if the pet adopter exists
            var adopterExistsCommand = "SELECT COUNT(1) FROM pet_adopter WHERE user_id = @PetAdopterId";
            var adopterExists = await _dbContext.Database.ExecuteSqlRawAsync(adopterExistsCommand, new SqlParameter("@PetAdopterId", petAdopterId)) > 0;

            if (!shelterExists || !adopterExists)
            {
                return false;
            }

            // Insert the new appointment
            string insertCommand = "INSERT INTO shelter_appointments (shelter_id, pet_adopter_id, appointment_date) VALUES (@ShelterId, @PetAdopterId, @Date)";
            await _dbContext.Database.ExecuteSqlRawAsync(insertCommand,
                new SqlParameter("@ShelterId", shelterId),
                new SqlParameter("@PetAdopterId", petAdopterId),
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
