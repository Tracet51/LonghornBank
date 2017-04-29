using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace LonghornBank.Models
{

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //TODO:  Add any fields that you need for creating a new user
        //For example, first name
        [Required]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Required(ErrorMessage = "A Last Name is Required")]
        [Display(Name = "Last Name")]
        public String LName { get; set; }

        [Display(Name = "Middle Initial")]
        public Char? MiddleInitial { get; set; }

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

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "DOB is Required")]
        [Display(Name = "Birthdate")]
        public DateTime DOB { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number is required")]
        [Display(Name = "Phone Number")]
        public String PhoneNumber { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Year of Birth")]
        public String Birthday { get; set; }
    }

    // GET: /Profile/Details
    // Displays the details for the customer's profile
    public class CustomerProfileDetails
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //TODO:  Add any fields that you need for creating a new user
        //For example, first name
        [Required]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Required(ErrorMessage = "A Last Name is Required")]
        [Display(Name = "Last Name")]
        public String LName { get; set; }

        [Display(Name = "Middle Initial")]
        public Char? MiddleInitial { get; set; }

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

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "DOB is Required")]
        [Display(Name = "Birthdate")]
        public DateTime DOB { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number is required")]
        [Display(Name = "Phone Number")]
        public String PhoneNumber { get; set; }
    }

    public class CustomerProfileEdit
    {

        //TODO:  Add any fields that you need for creating a new user
        //For example, first name
        [Required]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Required(ErrorMessage = "A Last Name is Required")]
        [Display(Name = "Last Name")]
        public String LName { get; set; }

        [Display(Name = "Middle Initial")]
        public Char? MiddleInitial { get; set; }

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
