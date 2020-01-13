using ZipPay.User.Infrastructure.Models;

namespace ZipPay.User.Web.Mappers
{
    public static class UserMapper
    {
        public static ApiModels.User ToApiModel(this UserEntity userEntity)
        {
            return new ApiModels.User
            {
                Id = userEntity.Id,
                EmailAddress = userEntity.EmailAddress,
                MonthlySalary = userEntity.MonthlySalary,
                MonthlyExpenses = userEntity.MonthlyExpenses,
            };
        }
    }
}