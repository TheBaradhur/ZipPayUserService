using Dal.Models;
using ZipPayUserService.ApiModels;

namespace ZipPayUserService.Mappers
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