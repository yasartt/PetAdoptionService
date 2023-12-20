using Microsoft.AspNetCore.Mvc;
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


        [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
        public async Task<Veterinarian> AddVetAsync(Veterinarian vet)
        {


            var newVeterinarian = new Veterinarian
            {
                Location = vet.Location,
                Name = vet.Name,
                Specialization = vet.Specialization,
                Username = vet.Username,
                Password = vet.Password,
            };

            await _dbContext.Veterinarians.AddAsync(newVeterinarian);
            await _dbContext.SaveChangesAsync();

            return newVeterinarian;
        }

        [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
        public async Task<PetAdopter> AddAdopterAsync(PetAdopter adopter)
        {


            var newAdopter = new PetAdopter
            {
                Age = adopter.Age,
                Name = adopter.Name,
                Address = adopter.Address,
                Username = adopter.Username,
                Password = adopter.Password,
            };

            await _dbContext.PetAdopters.AddAsync(newAdopter);
            await _dbContext.SaveChangesAsync();

            return newAdopter;
        }

        [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
        public async Task<Shelter> AddShelterAsync(Shelter shelter)
        {


            var newShelter = new Shelter
            {
                Name = shelter.Name,
                Address = shelter.Address,
                Username = shelter.Username,
                Password = shelter.Password,
                RestrictedHours = "",
            };

            await _dbContext.Shelters.AddAsync(newShelter);
            await _dbContext.SaveChangesAsync();

            return newShelter;
        }

        public async Task<SessionView> LoginUserAsync(string userType, string userName, string password)
        {
            if(userType == "Adopter")
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
            else if(userType == "Shelter")
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
                    };
                }
                return null;

            }
            else if (userType == "Veterinarian")
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
                        user = usernameVarMi
                    };
                }
                return null;

            }

            return null;

            // admin unuttuk
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
