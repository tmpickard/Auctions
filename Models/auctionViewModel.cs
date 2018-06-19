using System;
using System.ComponentModel.DataAnnotations;

namespace Auctions.Models
{
    public class auctionViewModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        [MinLength(3)]
        public string Product { get; set; }
        [Required]
        public double StartBid { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [MinLength(10)]
        public string Description { get; set; }
    }
}