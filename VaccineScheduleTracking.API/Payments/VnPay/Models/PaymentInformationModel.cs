namespace VaccineScheduleTracking.API_Test.Payments.VnPay.Models
{
    public class PaymentInformationModel
    {
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
        public int AccountID { get; set; }
        public int AppointmentID { get; set; }
    }
}
