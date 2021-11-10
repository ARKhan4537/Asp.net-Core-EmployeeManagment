using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Utilities;

namespace WebApplication3.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action:"IsEmailInUse", controller:"Account")]
        [ValidEmailDomain(allowedDomain: "gmail.com", ErrorMessage="Email Domain Must be gmail.com ")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [Display(Name ="Confirm Password")]
        [Compare("Password",ErrorMessage="Password and Confirmation password do not match")]
        public String ConfirmPassword { get; set; }
        public string City { get; set; }
    }
}