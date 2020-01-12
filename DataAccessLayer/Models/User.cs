using System.Collections.Generic;

namespace Dal.Models
{
    public class User
    {
        public int Id { get; set; }

        public string EmailAddress { get; set; }

        public decimal MonthlySalary { get; set; }

        public decimal MonthlyExpenses { get; set; }

        public virtual List<Account> Accounts { get; set; }
    }
}