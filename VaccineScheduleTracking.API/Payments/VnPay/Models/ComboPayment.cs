using System.ComponentModel.DataAnnotations.Schema;

namespace VaccineScheduleTracking.API_Test.Payments.VnPay.Models
{
    public class ComboPayment
    {
        public int ComboPaymentId { get; set; }
        public int AccountId { get; set; }
        public int VaccineComboId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateDate { get; set; }

        //public VnPayTransaction? VnPayTransaction { get; set; }
    }
}
