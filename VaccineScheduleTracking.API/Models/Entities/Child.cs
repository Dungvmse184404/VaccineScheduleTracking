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

        public int Age => CalculateAge(DateOfBirth);

        private int CalculateAge(DateOnly birthDate)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--; //chưa đến sinh nhật thì -1
            return age;
        }
    }
}
