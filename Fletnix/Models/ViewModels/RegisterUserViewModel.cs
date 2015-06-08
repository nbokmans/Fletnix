using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FletnixDatabase.Models;

namespace Fletnix.Models.ViewModels
{
    public class RegisterUserViewModel
    {
        public User User { get; set; } 
        public SelectList Countries { get; set; }

        [Required(ErrorMessage = "Email address is a required field.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(255)]
        [Display(Name = "Email address")]
        public String EmailAddress { get; set; }

        [Required(ErrorMessage = "Username is a required field.")]
        [DataType(DataType.Text, ErrorMessage = "Please enter a valid username.")]
        [StringLength(256)]
        [Display(Name = "Username")]
        public String Username { get; set; }

        [Required(ErrorMessage = "Password is a required field.")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password.")]
        [StringLength(256)]
        [MinLength(6, ErrorMessage = ("Password must be a minimum of 6 characters."))]
        [Display(Name = "Password")]
        public String Password { get; set; }

        [Required(ErrorMessage = "Confirm password is a required field.")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The passwords do not match.")]
        [Display(Name = "Confirm password")]
        public String ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First name is a required field.")]
        [DataType(DataType.Text, ErrorMessage = "Please enter a valid first name")]
        [StringLength(256)]
        [Display(Name = "First name")]
        public String Firstname { get; set; }

        [Required(ErrorMessage = "Last name is a required field.")]
        [DataType(DataType.Text, ErrorMessage = "Please enter a valid last name.")]
        [StringLength(256)]
        [Display(Name = "Last name")]
        public String Lastname { get; set; }

        [Required(ErrorMessage = "Country is a required field.")]
        [Display(Name = "Country")]
        public String CountryCode { get; set; }

        [Required(ErrorMessage = "Bankaccount number is a required field.")]
        [DataType(DataType.Text, ErrorMessage = "Please enter a valid bankaccount number.")]
        [StringLength(34)]
        [Display(Name = "Bankaccount number")]
        public String BankAccountNumber { get; set; }
        
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date of birth.")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Gender")]
        [DataType(DataType.Date, ErrorMessage = "Please select a gender.")]
        public String Gender { get; set; }
    }


    public class LoginUserViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [StringLength(255)]
        [Display(Name = "Username")]
        public String Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 6)]
        [Display(Name = "Password")]
        public String Password { get; set; }
    }
}