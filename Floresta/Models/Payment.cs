using MimeKit.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int MarkerId { get; set; }
        public Marker Marker { get; set; }
        public int SeedlingId { get; set; }
        public Seedling Seedling { get; set; }
        public int PurchasedAmount { get; set; }
        public double Price { get; set; }
        public bool IsPaymentSucceded { get; set; } = false;
    }
}
