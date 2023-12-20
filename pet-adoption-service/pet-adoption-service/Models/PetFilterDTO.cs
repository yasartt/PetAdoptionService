namespace pet_adoption_service.Models
{
    public class PetFilterDTO
    {
        public int minAge { get; set; }
        public int maxAge { get; set; }
        public string sex { get; set; }
        public string breed { get; set; }
    }
}
