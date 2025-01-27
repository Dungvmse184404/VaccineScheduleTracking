namespace VaccineScheduleTracking.API.Models.Entities
{
    public class Parent
    {
        public int ParentID { get; set; }
        public int AccountID { get; set; }
        public Account Account { get; set; }
    }
}
