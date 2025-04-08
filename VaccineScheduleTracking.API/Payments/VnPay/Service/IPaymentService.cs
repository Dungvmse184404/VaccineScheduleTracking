using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Models;

namespace VaccineScheduleTracking.API_Test.Payments.VnPay.Service
{
    public interface IPaymentService
    {
        Task<List<BillDto>> GetAllBillByAccountIdAsync(int accountId);
        Task<Models.ComboPayment> AddComboPaymentAsync(Models.ComboPayment model);
        Task<Models.Payment> AddPaymentAsync(Models.Payment model);
        Task<VnPayTransaction> AddPaymentVnPayTransactionAsyn(VnPayTransaction vnPayTransaction);
        Task<List<Models.ComboPayment>> GetPaymentComboByAccountId(int id);
        Task<List<Models.Payment>> GetPaymentsByAccountId(int id);
    }
}
