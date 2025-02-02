namespace VaccineScheduleTracking.API.Models.Entities
{
    public class Child
    {
        public int ChildID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public string Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int ParentID { get; set; }
        public Parent Parent { get; set; }
    }
}
