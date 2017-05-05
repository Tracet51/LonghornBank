using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace LonghornBank.Models
{
    public class EditViewModel
    {

    }

    public class EmployeeEditEmployee
    {
        [Required(ErrorMessage = "A Street Address is Required")]
        [Display(Name = "Street Address")]

        public String StreetAddress { get; set; }
        [Required(ErrorMessage = "City is Required")]
        [Display(Name = "City")]

        public String City { get; set; }
        [Required(ErrorMessage = "State is Required")]
        [Display(Name = "State")]
        public String State { get; set; }

        [Display(Name = "Zip Code")]
        [Required(ErrorMessage = "Zip Code is Required")]
        public String Zip { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number is required")]
        [Display(Name = "Phone Number")]
        public String PhoneNumber { get; set; }


    }

    public class EmployeeChangePassword
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangeEmployeePassword
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public String EmployeeFName { get; set; }

        public String EmployeeLName { get; set; }

        public String EmployeeID { get; set; }
    }

    public class ManagerEditManager
    {
        [Required(ErrorMessage = "A Street Address is Required")]
        [Display(Name = "Street Address")]

        public String StreetAddress { get; set; }
        [Required(ErrorMessage = "City is Required")]
        [Display(Name = "City")]

        public String City { get; set; }
        [Required(ErrorMessage = "State is Required")]
        [Display(Name = "State")]
        public String State { get; set; }

        [Display(Name = "Zip Code")]
        [Required(ErrorMessage = "Zip Code is Required")]
        public String Zip { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number is required")]
        [Display(Name = "Phone Number")]
        public String PhoneNumber { get; set; }
    }
}