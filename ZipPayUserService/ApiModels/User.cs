namespace ZipPay.User.Web.ApiModels
{
    public class User
    {
        public int Id { get; set; }

        public string EmailAddress { get; set; }

        public decimal MonthlySalary { get; set; }

        public decimal MonthlyExpenses { get; set; }

        public decimal OverallCreditAllowance => MonthlySalary - MonthlyExpenses;
    }
}