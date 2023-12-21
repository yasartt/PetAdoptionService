﻿using System.Collections.Generic;
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


    }
}