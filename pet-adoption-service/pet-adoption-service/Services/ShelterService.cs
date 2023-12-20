using Microsoft.AspNetCore.Mvc;
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

        [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
        public async Task<List<Pet>> GetPetsByShelterAsync(int shelterId)
        {
            //var petList = await _dbContext.Pets.Where(q => q.ShelterId == shelterId).ToListAsync();
            

            return null;
        }

        public async Task<ShelterBusyHoursView> GetShelterBusyHoursAsync(int shelterId)
        {
            try
            {
                var theShelter = await _dbContext.Shelters.SingleOrDefaultAsync(q => q.UserId == shelterId);

                var appointmentList = await _dbContext.ShelterAppointments.Where(q => q.ShelterId == shelterId).ToListAsync();

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
            var theVet = await _dbContext.Veterinarians.SingleOrDefaultAsync(q => q.UserId == vetId);

            var thePet = await _dbContext.Pets.SingleOrDefaultAsync(q => q.PetId == petId);

            if (theVet == null || thePet == null)
            {
                return false;
            }

            // check if that hour is available

            await _dbContext.VetAppointments.AddAsync(new VetAppointment()
            {
                VetId = vetId,
                PetId = petId,
                AppointmentDate = date,
            });

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Pet>> GetAllPetsOfSheltersAsync(int shelterId)
        {
            var petList = await _dbContext.Stays.Where(q => q.ShelterId == shelterId).Include(q => q.Pet).Select(q => q.Pet).ToListAsync();

            if (petList.Count > 0)
            {
                return petList;
            }
            else
            {
                return null;
            }

        }

        public async Task<Shelter> GetShelterByIdAsync(int shelterId)
        {
            var resultShelter = await _dbContext.Shelters.SingleOrDefaultAsync(q => q.UserId == shelterId);

            return resultShelter;
        }

    }
}
