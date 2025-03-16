using VaccineScheduleTracking.API_Test.Payments.VnPay.Repository;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Models;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Service;

namespace VaccineScheduleTracking.API_Test.Payment.VnPay.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IVnPayTransactionRepository vnPayTransactionRepository;

        public PaymentService(IPaymentRepository paymentRepository, IVnPayTransactionRepository vnPayTransactionRepository)
        {
            this.paymentRepository = paymentRepository;
            this.vnPayTransactionRepository = vnPayTransactionRepository;
        }
        public async Task<Payments.VnPay.Models.Payment> AddPaymentAsync(Payments.VnPay.Models.Payment model)
        {
            await paymentRepository.AddPaymentAsync(model);
            return model;
        }

        public async Task<VnPayTransaction> AddPaymentVnPayTransactionAsyn(VnPayTransaction vnPayTransaction)
        {
            return await vnPayTransactionRepository.AddTransactionAsync(vnPayTransaction);
        }

        public async Task<List<Payments.VnPay.Models.Payment>> GetPaymentsByAccountId(int id)
        {
            return await paymentRepository.GetPaymentsByAccountId(id);
        }
    }
}
