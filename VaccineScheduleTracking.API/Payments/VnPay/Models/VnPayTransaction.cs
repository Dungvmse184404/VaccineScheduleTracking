using System.ComponentModel.DataAnnotations;

namespace VaccineScheduleTracking.API_Test.Payments.VnPay.Models
{
    public class VnPayTransaction
    {
        [Key]
        public string TransactionId { get; set; } = null!;
        public string Token { get; set; } = null!;
        public int PaymentId { get; set; }
    }
}
