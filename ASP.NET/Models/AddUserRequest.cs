using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortfolioWebApi.Models.Request
{
    public class AddUserRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]

        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#\$%\^&\*])(?!.*\s).{8,20}$")]
        public string Password { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 7)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}