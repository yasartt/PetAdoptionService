using Microsoft.AspNetCore.Mvc;
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
                var theVet = await _dbContext.Veterinarians.SingleOrDefaultAsync(q => q.UserId == vetId);

                var appointmentList = await _dbContext.VetAppointments.Where(q => q.VetId == vetId).ToListAsync();

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
            var theVet = await _dbContext.Veterinarians.SingleOrDefaultAsync(q => q.UserId == vetId);

            var thePet = await _dbContext.Pets.SingleOrDefaultAsync(q => q.PetId == petId);

            if (theVet == null || thePet == null)
            {
                return false;
            }

            // check if that hour is available
            // ...

            await _dbContext.VetAppointments.AddAsync(new VetAppointment()
            {
                VetId = vetId,
                PetId = petId,
                AppointmentDate = date,
            });

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
