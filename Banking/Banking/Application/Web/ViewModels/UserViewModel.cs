using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Application.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class UserViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}