using Dal.Models;
using ZipPayUserService.ApiModels;

namespace ZipPayUserService.Mappers
{
    public static class UserMapper
    {
        public static User ToApiModel(this UserEntity userEntity)
        {
            return new User
            {
                Id = userEntity.Id,
                EmailAddress = userEntity.EmailAddress,
                MonthlySalary = userEntity.MonthlySalary,
                MonthlyExpenses = userEntity.MonthlyExpenses,
            };
        }
    }
}