﻿using System;

namespace Query.Models
{
    public class Invoice
    {
        public int invoiceId { get; set; }
        public int subscriptionId { get; set; }
        public int userId { get; set; }
        public double subtotal { get; set; }
        public double tax { get; set; }
        public double total { get; set; }
        public string memo { get; set; }
        public DateTime datedue { get; set; }
        public DateTime datecreated { get; set; }
    }
}