namespace pet_adoption_service.Models
{
    public class VetBusyHoursView
    {
        public List<VetAppointment>? Appointments { get; set; }
        public string? RestrictedHours { get; set; }
    }
}
