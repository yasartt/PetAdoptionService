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
            var newAppointment = new VetAppointment()
            {
                VetId = vetId,
                PetId = petId,
                AppointmentDate = date,
            };

            await _dbContext.VetAppointments.AddAsync(newAppointment);
            await _dbContext.SaveChangesAsync();

            return true;
        }

    }
}
