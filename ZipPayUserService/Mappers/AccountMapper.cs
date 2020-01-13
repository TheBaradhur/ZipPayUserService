using ZipPay.User.Infrastructure.Models;
using ZipPay.User.Web.ApiModels;

namespace ZipPay.User.Web.Mappers
{
    public static class AccountMapper
    {
        public static Account ToApiModel(this AccountEntity accountEntity)
        {
            return new Account
            {
                Id = accountEntity.Id,
                CreditCreationDate = accountEntity.CreditCreationDate,
                UserId = accountEntity.UserId,
                OriginalCreditAmount = accountEntity.OriginalCreditAmount,
                CurrentBalance = accountEntity.CurrentBalance
            };
        }
    }
}