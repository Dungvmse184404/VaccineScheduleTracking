using VaccineScheduleTracking.API_Test.Payments.VnPay.Repository;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Models;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Service;

namespace VaccineScheduleTracking.API_Test.Payment.VnPay.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }
        public async Task<Payments.VnPay.Models.Payment> AddPaymentAsync(Payments.VnPay.Models.Payment model)
        {
            await paymentRepository.AddPaymentAsync(model);
            return model;
        }
    }
}
