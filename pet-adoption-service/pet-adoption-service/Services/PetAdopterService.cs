using System.Data.SqlClient;
using System.Configuration
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using pet_adoption_service.Models;


namespace pet_adoption_service.Services
{
    public class PetAdopterService
    {
        private readonly PetAdoptionDbContext _dbContext;

        public PetAdopterService(PetAdoptionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PetAdopter> AddAdopter(PetAdopter newAdopter)
        {
            string sqlCommand = "INSERT INTO pet_adopter (username, password, name, address, age) VALUES (@Username, @Password, @Name, @Address, @Age)";

            await _dbContext.Database.ExecuteSqlRawAsync(sqlCommand,
                new SqlParameter("@Username", newAdopter.Username),
                new SqlParameter("@Password", newAdopter.Password),
                new SqlParameter("@Name", newAdopter.Name),
                new SqlParameter("@Address", newAdopter.Address),
                new SqlParameter("@Age", newAdopter.Age)
            );

            return newAdopter;
        }

        public async Task<long> ApplyForPetAsync(int petAdopterId, int petId)
        {
            var petShelter = await _dbContext.Stays.SingleOrDefaultAsync(q => q.PetId == petId);
            var thePet = await _dbContext.Pets.SingleOrDefaultAsync(q => q.PetId == petId);
            var theAdopter = await _dbContext.PetAdopters.SingleOrDefaultAsync(q => q.UserId == petAdopterId);
            var buAdopterBasvuruYaptiMi = await _dbContext.AdoptionApplications.SingleOrDefaultAsync(q => q.PetAdopterId == petAdopterId && q.PetId == petId);

            if (thePet == null || petShelter == null || theAdopter == null)
            {
                return -3;
            }
            else if(thePet.IsAvailable == 0)
            {
                return -1;
            }
            else if (buAdopterBasvuruYaptiMi != null)
            {
                return -2;
            }

            await _dbContext.AdoptionApplications.AddAsync(new AdoptionApplication()
            {
                PetAdopterId = petAdopterId,
                PetId = petId,
                ShelterId = petShelter.ShelterId,
                ApplicationDate = DateTime.Now,
                Status = 1,
            });
            await _dbContext.SaveChangesAsync();

            return 1;
        }

        public async Task<List<PetAdopter>> GetAllAdopters()
        {
            var adopters = await _dbContext.PetAdopters
                              .FromSqlRaw("SELECT * FROM pet_adopter")
                              .ToListAsync();
            return adopters;
        }


        public async Task<List<Pet>> GetAllPetsOfAdoptersAsync(int petAdopterId)
        {
            string sqlCommand = "SELECT pet.* FROM pet JOIN adopt ON pet.pet_id = adopt.pet_id WHERE adopt.user_id = @PetAdopterId";

            var pets = await _dbContext.Pets
                        .FromSqlRaw(sqlCommand, new SqlParameter("@PetAdopterId", petAdopterId))
                        .ToListAsync();

            return pets.Count > 0 ? pets : null;
        }

        public async Task<(int MostPopularShelterId, int MostPopularVetId)> GetMostPopularIdsAsync()
        {
            string connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
            int mostPopularShelterId = -1;
            int mostPopularVetId = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Query for the most popular shelter
                string mostPopularShelterQuery = @"
                                            SELECT TOP 1 shelter_id, COUNT(*) as AppointmentCount
                                            FROM shelter_appointments
                                            GROUP BY shelter_id
                                            ORDER BY AppointmentCount DESC";

                using (SqlCommand command = new SqlCommand(mostPopularShelterQuery, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            mostPopularShelterId = reader.GetInt32(0); // shelter_id is the first column
                        }
                    }
                }

                // Query for the most popular vet
                string mostPopularVetQuery = @"
                                            SELECT TOP 1 vet_id, COUNT(*) as AppointmentCount
                                            FROM vet_appointments
                                            GROUP BY vet_id
                                            ORDER BY AppointmentCount DESC";

                using (SqlCommand command = new SqlCommand(mostPopularVetQuery, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            mostPopularVetId = reader.GetInt32(0); // vet_id is the first column
                        }
                    }
                }
            }

            return (mostPopularShelterId, mostPopularVetId);
        }

    }

}