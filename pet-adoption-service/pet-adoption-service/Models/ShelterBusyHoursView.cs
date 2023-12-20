namespace pet_adoption_service.Models
{
    public class ShelterBusyHoursView
    {
        public List<ShelterAppointment>? Appointments { get; set; }
        public string? RestrictedHours { get; set; }
    }
}
