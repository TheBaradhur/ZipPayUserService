﻿using System;

namespace ZipPay.User.Infrastructure.Models
{
    public class AccountEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime CreditCreationDate { get; set; }

        public decimal OriginalCreditAmount { get; set; }

        public decimal CurrentBalance { get; set; }

        public bool IsCreditFullyRefunded => CurrentBalance == 0; 
    }
}