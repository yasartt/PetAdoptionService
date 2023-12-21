namespace pet_adoption_service.Models
{
    public class AddAppointmentDTO
    {
        public int shelterId { get; set; }
        public int petAdopterId { get; set; }
        public DateTime date { get; set;}
    }
}
