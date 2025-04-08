namespace VaccineScheduleTracking.API_Test.Payments.VnPay.Repository
{
    public interface IPaymentRepository
    {
        Task<Models.ComboPayment> AddComboPaymentAsync(Models.ComboPayment model);
        Task<Models.Payment> AddPaymentAsync(Models.Payment model);
        Task<List<Models.ComboPayment>> GetPaymentComboByAccountId(int id);
        Task<List<Models.Payment>> GetPaymentsByAccountId(int id);
    }
}
