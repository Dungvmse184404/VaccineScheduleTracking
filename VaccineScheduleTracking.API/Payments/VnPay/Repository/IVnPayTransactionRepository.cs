using VaccineScheduleTracking.API_Test.Payments.VnPay.Models;

namespace VaccineScheduleTracking.API_Test.Payments.VnPay.Repository
{
    public interface IVnPayTransactionRepository
    {
        Task<VnPayTransaction?> GetTransactionByPymentIdAsync(int paymentId);
        Task<VnPayTransaction?> GetTransactionByComboPaymentIdAsync(int comboPaymentId);
        Task<VnPayTransaction> AddTransactionAsync(VnPayTransaction vnPayTransaction);
        Task<VnPayTransaction> GetTransactionForAccountAsync(int targetId, string paymentType);
    }
}
