using System;

namespace ZipPay.User.Web.ApiModels
{
    public class Account
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime CreditCreationDate { get; set; }

        public decimal OriginalCreditAmount { get; set; }

        public decimal CurrentBalance { get; set; }
    }
}