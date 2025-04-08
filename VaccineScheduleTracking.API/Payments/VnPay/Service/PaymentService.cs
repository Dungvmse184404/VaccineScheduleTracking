using VaccineScheduleTracking.API_Test.Payments.VnPay.Repository;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Models;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Service;
using VaccineScheduleTracking.API_Test.Models.DTOs;
using MimeKit.Tnef;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

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

        public async Task<ComboPayment> AddComboPaymentAsync(ComboPayment model)
        {
            await paymentRepository.AddComboPaymentAsync(model);
            return model;
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

        public async Task<List<ComboPayment>> GetPaymentComboByAccountId(int id)
        {
            return await paymentRepository.GetPaymentComboByAccountId(id);
        }

        public async Task<List<Payments.VnPay.Models.Payment>> GetPaymentsByAccountId(int id)
        {
            return await paymentRepository.GetPaymentsByAccountId(id);
        }

        public async Task<VnPayTransaction> GetTransactionForAccountAsync(int targetId, string paymentType)
        {
            return await vnPayTransactionRepository.GetTransactionForAccountAsync(targetId, paymentType);
        }

        //public async Task<List<BillDto>> GetAllBillByAccountIdAsync(int accountId)
        //{
        //    List<BillDto> billDtos = null;
        //    var payments = await paymentRepository.GetPaymentsByAccountId(accountId);
        //    var comboPayments = await paymentRepository.GetPaymentComboByAccountId(accountId);
        //    foreach (var pay in payments)
        //    {
        //        var transaction = await GetTransactionForAccountAsync(pay.PaymentId, "appointment");
        //        var bill = new BillDto()
        //        {   
        //            TransactionId = transaction.TransactionId,
        //            Amount = pay.Amount,
        //            CreateDate = pay.CreateDate,
        //            PaymentType = "appointment",
        //        };
        //        billDtos.Add(bill);
        //    }
        //    foreach (var combo in comboPayments)
        //    {
        //        var transaction = await GetTransactionForAccountAsync(combo.VaccineComboId, "combo");
        //        var bill = new BillDto()
        //        {
        //            TransactionId = transaction.TransactionId,
        //            Amount = combo.Amount,
        //            CreateDate = combo.CreateDate,
        //            PaymentType = "combo",
        //        };
        //        billDtos.Add(bill);
        //    }

        //    return billDtos;

        //}

        public async Task<List<BillDto>> GetAllBillByAccountIdAsync(int accountId)
        {
            var billDtos = new List<BillDto>();

            var payments = await paymentRepository.GetPaymentsByAccountId(accountId);
            var comboPayments = await paymentRepository.GetPaymentComboByAccountId(accountId);

            foreach (var pay in payments)
            {
                var transaction = await GetTransactionForAccountAsync(pay.PaymentId, "appointment");
                billDtos.Add(new BillDto
                {
                    TransactionId = transaction?.TransactionId,
                    Amount = pay.Amount,
                    CreateDate = pay.CreateDate,
                    PaymentType = "appointment"
                });
            }

            foreach (var combo in comboPayments)
            {
                var transaction = await GetTransactionForAccountAsync(combo.ComboPaymentId, "combo");
                billDtos.Add(new BillDto
                {
                    TransactionId = transaction?.TransactionId,
                    Amount = combo.Amount,
                    CreateDate = combo.CreateDate,
                    PaymentType = "combo"
                });
            }

            return billDtos;
        }



    }
}
