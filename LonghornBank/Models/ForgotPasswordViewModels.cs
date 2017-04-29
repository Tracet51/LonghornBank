using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parks_Carson_HW7.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}