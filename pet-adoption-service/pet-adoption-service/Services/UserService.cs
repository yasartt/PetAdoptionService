using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using pet_adoption_service.Models;
using System.Net;

namespace pet_adoption_service.Services
{
    public class UserService
    {
        private readonly PetAdoptionDbContext _dbContext;

        public UserService(PetAdoptionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Veterinarian> AddVetAsync(Veterinarian vet)
        {
            string sqlCommand = "INSERT INTO veterinarian (location, name, specialization, username, password) VALUES (@Location, @Name, @Specialization, @Username, @Password)";

            await _dbContext.Database.ExecuteSqlRawAsync(sqlCommand,
                new SqlParameter("@Location", vet.Location),
                new SqlParameter("@Name", vet.Name),
                new SqlParameter("@Specialization", vet.Specialization),
                new SqlParameter("@Username", vet.Username),
                new SqlParameter("@Password", vet.Password)
            );

            return vet;
        }

        [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
        public async Task<PetAdopter> AddAdopterAsync(PetAdopter adopter)
        {
            string sqlCommand = "INSERT INTO pet_adopter (age, name, address, username, password) VALUES (@Age, @Name, @Address, @Username, @Password)";

            await _dbContext.Database.ExecuteSqlRawAsync(sqlCommand,
                new SqlParameter("@Age", adopter.Age),
                new SqlParameter("@Name", adopter.Name),
                new SqlParameter("@Address", adopter.Address),
                new SqlParameter("@Username", adopter.Username),
                new SqlParameter("@Password", adopter.Password)
            );

            return adopter;
        }

        [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
        public async Task<Shelter> AddShelterAsync(Shelter shelter)
        {
            string sqlCommand = "INSERT INTO shelter (name, address, username, password, restricted_hours) VALUES (@Name, @Address, @Username, @Password, @RestrictedHours)";

            await _dbContext.Database.ExecuteSqlRawAsync(sqlCommand,
                new SqlParameter("@Name", shelter.Name),
                new SqlParameter("@Address", shelter.Address),
                new SqlParameter("@Username", shelter.Username),
                new SqlParameter("@Password", shelter.Password),
                new SqlParameter("@RestrictedHours", shelter.RestrictedHours)
            );

            return shelter;
        }

        public async Task<SessionView> LoginUserAsync(string userType, string userName, string password)
        {
            string sqlCommand = "";
            object user = null;

            switch (userType)
            {
                case "Adopter":
                    sqlCommand = "SELECT * FROM pet_adopter WHERE username = @Username AND password = @Password";
                    user = await _dbContext.PetAdopters.FromSqlRaw(sqlCommand,
                            new SqlParameter("@Username", userName),
                            new SqlParameter("@Password", password))
                            .SingleOrDefaultAsync();
                    break;
                case "Shelter":
                    sqlCommand = "SELECT * FROM shelter WHERE username = @Username AND password = @Password";
                    user = await _dbContext.Shelters.FromSqlRaw(sqlCommand,
                            new SqlParameter("@Username", userName),
                            new SqlParameter("@Password", password))
                            .SingleOrDefaultAsync();
                    break;
                case "Veterinarian":
                    sqlCommand = "SELECT * FROM veterinarian WHERE username = @Username AND password = @Password";
                    user = await _dbContext.Veterinarians.FromSqlRaw(sqlCommand,
                            new SqlParameter("@Username", userName),
                            new SqlParameter("@Password", password))
                            .SingleOrDefaultAsync();
                    break;
                case "Admin":
                    sqlCommand = "SELECT * FROM admin WHERE username = @Username AND password = @Password";
                    user = await _dbContext.Admin.FromSqlRaw(sqlCommand,
                            new SqlParameter("@Username", userName),
                            new SqlParameter("@Password", password))
                            .SingleOrDefaultAsync();
                    break;
            }

            if (user == null)
            {
                return null;
            }
            else
            {
                return new SessionView()
                {
                    user = user
                };
            }
        }

    }
}

/*
 * public async Task<SessionView> LoginUserAsync(string userType, string userName, string password)
        {
            if (userType == "Adopter")
            {
                var usernameVarMi = await _dbContext.PetAdopters.SingleOrDefaultAsync(q => q.Username == userName);

                if (usernameVarMi == null)
                {
                    return null;
                }
                else if (usernameVarMi.Password != password)
                {
                    //return passwordHatali 
                }
                else
                {
                    return new SessionView()
                    {
                        user = usernameVarMi
                    };
                }

            return null;

        }
            else if (userType == "Shelter")
            {
                var usernameVarMi = await _dbContext.Shelters.SingleOrDefaultAsync(q => q.Username == userName);
                if (usernameVarMi == null)
                {
                    return null;
                }
                else if (usernameVarMi.Password != password)
                {
                    //return passwordHatali 
                }
                else
                {
                    return new SessionView()
                    {
                        user = usernameVarMi
                    }
                }
    }
    return null;
            }
            else if(userType == "Veterinarian")
            {
                var usernameVarMi = await _dbContext.Veterinarians.SingleOrDefaultAsync(q => q.Username == userName);
                if (usernameVarMi == null)
                {
                    return null;
                }
                else if (usernameVarMi.Password != password)
                {
                    //return passwordHatali 
                }
                else
                {
        return new SessionView()
        {
            user = usernameVarMi;
    };
}
            }
            else
            {
    // admini unuttuk
    return null;

}

        }*/
