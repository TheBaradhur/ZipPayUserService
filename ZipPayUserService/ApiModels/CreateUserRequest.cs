using System;
using System.ComponentModel.DataAnnotations;

namespace ZipPay.User.Web.ApiModels
{
    public class CreateUserRequest
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [Range(0.1, Double.MaxValue, ErrorMessage = "Only positive number allowed.")]
        public decimal MonthlySalary { get; set; }

        [Required]
        [Range(0.1, Double.MaxValue, ErrorMessage = "Only positive number allowed.")]
        public decimal MonthlyExpenses { get; set; }
    }
}