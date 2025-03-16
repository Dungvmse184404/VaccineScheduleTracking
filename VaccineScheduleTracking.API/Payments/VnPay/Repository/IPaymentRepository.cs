namespace VaccineScheduleTracking.API_Test.Payments.VnPay.Repository
{
    public interface IPaymentRepository
    {
        Task<Models.Payment> AddPaymentAsync(Models.Payment model);
        Task<List<Models.Payment>> GetPaymentsByAccountId(int id);
    }
}
