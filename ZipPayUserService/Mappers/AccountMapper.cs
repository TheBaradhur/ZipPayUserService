using Dal.Models;

namespace ZipPayAccountService.Mappers
{
    public static class UserMapper
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