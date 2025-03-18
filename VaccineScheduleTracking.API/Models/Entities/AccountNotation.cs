using Microsoft.AspNetCore.Http.HttpResults;

namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class AccountNotation
    {
        public int AccountNotationID { get; set; }
        public int AccountID { get; set; }
        public DateTime CreateDate { get; set; }
        public string Notation { get; set; }
        public bool Processed { get; set; }

    }
}
