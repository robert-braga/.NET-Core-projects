using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Person name must be filled")]
        public string PersonName { get; set; }

        [Required(ErrorMessage = "Email must be filled")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        [Remote(action: "IsEmailAlreadyRegister", controller: "Account", ErrorMessage = "Email is already used.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number must be filled")]
        [RegularExpression("^[0-9]*$" ,ErrorMessage = "Phone number should contain digits only")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password must be filled")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password must be filled")]
        [Compare("Password", ErrorMessage = "Password and confirm password must match")]
        public string ConfirmPassword { get; set; }

    }
}
