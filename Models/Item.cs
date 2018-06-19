using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Auctions.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public string Product { get; set; }
        public double StartBid { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Bid> Bids { get; set; }
        public User user { get; set; }

        public Item()
        {
            Bids = new List<Bid>();
        }
    }
}