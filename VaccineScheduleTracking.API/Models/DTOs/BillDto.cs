namespace VaccineScheduleTracking.API_Test.Models.DTOs
{
    public class BillDto
    {
        public string? TransactionId { get; set; }
        public string PaymentType {     get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
