using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Auctions.Models
{
    public class Bid
    {
        [Key]
        public int BidId { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public double UserBid { get; set; }
        public User user { get; set; }
        public Item item { get; set; }
    }
}