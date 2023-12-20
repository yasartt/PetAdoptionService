using Microsoft.EntityFrameworkCore;

namespace pet_adoption_service.Models
{
    public class PetAdoptionDbContext: DbContext
    {
        public PetAdoptionDbContext(DbContextOptions options) : base(options)
        {

        }

        // Parameterless constructor
        public PetAdoptionDbContext()
        {
        }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Shelter> Shelters { get; set; }
        public DbSet<PetAdopter> PetAdopters { get; set; }
        public DbSet<Veterinarian> Veterinarians { get; set; }
        public DbSet<AdoptionApplication> AdoptionApplications { get; set; }
        public DbSet<VetAppointment> VetAppointments { get; set;}
        public DbSet<ShelterAppointment> ShelterAppointments { get; set; }
        
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<PetCareInfo> PetCareInfos { get; set; }
        public DbSet<Adopt> Adopts { get; set; }
        public DbSet<Stay> Stays { get; set; }
        public DbSet<ExpertAdvice> ExpertAdvices { get; set;}

    }
}

