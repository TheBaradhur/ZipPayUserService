namespace ZipPay.User.Infrastructure.Models
{
    public class UserEntity
    {
        public int Id { get; set; }

        public string EmailAddress { get; set; }

        public decimal MonthlySalary { get; set; }

        public decimal MonthlyExpenses { get; set; }

        public decimal OverallCreditAllowance => MonthlySalary - MonthlyExpenses;
    }
}