using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Auctions.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public double Wallet { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Item> Items { get; set; }
        public List<Bid> Bids { get; set; }
        public User()
        {
            Items = new List<Item>();
            Bids = new List<Bid>();
        }
    }
}