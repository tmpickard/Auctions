using System;
using System.ComponentModel.DataAnnotations;

namespace Auctions.Models
{
    public class registerViewModel
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }
        [Required]
        [MinLength(3)]
        public string Firstname { get; set; }
        [Required]
        [MinLength(3)]
        public string Lastname { get; set; }
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string con_Password { get; set; }
    }
}