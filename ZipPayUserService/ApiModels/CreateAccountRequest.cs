using System;
using System.ComponentModel.DataAnnotations;

namespace ZipPayUserService.ApiModels
{
    public class CreateAccountRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Account id must be positive.")]
        public int UserId { get; set; }

        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Requested credit amount must be strictly positive.")]
        public decimal RequestedCreditAmount { get; set; }
    }
}