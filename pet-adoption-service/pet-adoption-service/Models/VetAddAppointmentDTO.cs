namespace pet_adoption_service.Models
{
    public class VetAddAppointmentDTO
    {

        public int vetId { get; set; }

        public int petId { get; set; }

        public DateTime randevuTarih { get; set; }

    }
}