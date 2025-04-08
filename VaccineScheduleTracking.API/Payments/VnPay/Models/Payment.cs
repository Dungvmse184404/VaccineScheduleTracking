namespace VaccineScheduleTracking.API_Test.Payments.VnPay.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int AccountId { get; set; }
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateDate { get; set; }
        //public string TransactionId { get; set; }

        //public VnPayTransaction? VnPayTransaction { get; set; }
    }
}
