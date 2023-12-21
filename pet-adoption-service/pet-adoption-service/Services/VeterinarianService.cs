using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using pet_adoption_service.Models;
using System.Net;

namespace pet_adoption_service.Services
{
    public class VeterinarianService
    {
        private readonly PetAdoptionDbContext _dbContext;

        public VeterinarianService(PetAdoptionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<VetBusyHoursView> GetVetBusyHoursAsync(int vetId)
        {
            try
            {
                string vetSql = "SELECT * FROM veterinarian WHERE user_id = @UserId";
                var theVet = await _dbContext.Veterinarians
                              .FromSqlRaw(vetSql, new SqlParameter("@UserId", vetId))
                              .SingleOrDefaultAsync();

                string appointmentSql = "SELECT * FROM vet_appointments WHERE vet_id = @UserId";
                var appointmentList = await _dbContext.VetAppointments
                                      .FromSqlRaw(appointmentSql, new SqlParameter("@UserId", vetId))
                                      .ToListAsync();

                return new VetBusyHoursView()
                {
                    Appointments = appointmentList,
                    RestrictedHours = theVet.RestrictedHours,
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<Boolean> AddAppointmentAsync(int vetId, int petId, DateTime date)
        {
            string vetSql = "SELECT * FROM veterinarian WHERE user_id = @UserId";
            var theVet = await _dbContext.Veterinarians
                          .FromSqlRaw(vetSql, new SqlParameter("@UserId", vetId))
                          .SingleOrDefaultAsync();

            string petSql = "SELECT * FROM pet WHERE pet_id = @PetId";
            var thePet = await _dbContext.Pets
                          .FromSqlRaw(petSql, new SqlParameter("@PetId", petId))
                          .SingleOrDefaultAsync();

            if (theVet == null || thePet == null)
            {
                return false;
            }

            // Check if that hour is available
            string checkAvailabilitySql = "SELECT COUNT(1) FROM vet_appointments WHERE vet_id = @UserId AND appointment_date = @Date";
            var count = await _dbContext.Database.ExecuteSqlRawAsync(checkAvailabilitySql,
                new SqlParameter("@UserId", vetId),
                new SqlParameter("@Date", date)
            );

            if (count > 0)
            {
                // If count is greater than 0, it means there's already an appointment at that time
                return false;
            }

            // Hour is available, insert the new appointment
            string insertAppointmentSql = "INSERT INTO vet_appointments (vet_id, pet_id, appointment_date) VALUES (@UserId, @PetId, @Date)";
            await _dbContext.Database.ExecuteSqlRawAsync(insertAppointmentSql,
                new SqlParameter("@UserId", vetId),
                new SqlParameter("@PetId", petId),
                new SqlParameter("@Date", date)
            );

            return true;
        }

    }
}
