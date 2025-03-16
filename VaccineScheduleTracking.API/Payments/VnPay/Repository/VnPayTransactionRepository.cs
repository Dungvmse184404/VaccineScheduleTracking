using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API_Test.Payments.VnPay.Models;

namespace VaccineScheduleTracking.API_Test.Payments.VnPay.Repository
{
    public class VnPayTransactionRepository : IVnPayTransactionRepository
    {
        private readonly VaccineScheduleDbContext dbContext;

        public VnPayTransactionRepository(VaccineScheduleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<VnPayTransaction> AddTransactionAsync(VnPayTransaction vnPayTransaction)
        {
            await dbContext.VnPayTransactions.AddAsync(vnPayTransaction);
            await dbContext.SaveChangesAsync();
            return vnPayTransaction;
        }

        public async Task<VnPayTransaction?> GetTransactionByPymentIdAsync(int paymentId)
        {
            return await dbContext.VnPayTransactions.FirstOrDefaultAsync(x => x.PaymentId == paymentId);
        }
    }
}
