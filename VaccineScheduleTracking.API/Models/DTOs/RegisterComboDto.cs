namespace VaccineScheduleTracking.API_Test.Models.DTOs
{
    public class RegisterComboDto
    {
        public string OrderDescription { get; set; }
        public double Amount { get; set; }
        public int ComnboId { get; set; }
        public int ChildId { get; set; }
        public DateOnly StartDate { get; set; }
    }
}
