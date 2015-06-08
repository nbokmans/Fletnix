using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foolproof;
using FletnixDatabase.Models;
using System.ComponentModel.DataAnnotations;

namespace Fletnix.Models.ViewModels
{
    public class UserProfileViewModel
    {
         public SelectList Countries { get; set; }

        public List<PurchaseHistory> UserPurchaseHistory { get; set; }
        public List<WatchHistory> UserWatchHistory { get; set; }

        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password.")]
        [StringLength(256)]
        [MinLength(6, ErrorMessage = ("Password must be a minimum of 6 characters."))]
        [Display(Name = "Password")]
        public String NewPassword { get; set; }

        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password.")]
        [StringLength(256)]
        [MinLength(6, ErrorMessage = ("Password must be a minimum of 6 characters."))]
        [Display(Name = "Confirm password")]
        [EqualTo("NewPassword", ErrorMessage = "The new passwords do not match.")]
        public String CheckNewPassword { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid EmailAdress.")]
        [StringLength(256)]
        [Display(Name = "Emailadress")]
        public String emailadress { get; set; }

        [DataType(DataType.Text, ErrorMessage = "Please enter a valid bankaccount number.")]
        [StringLength(34)]
        [Display(Name = "Bankaccount number")]
        public String BankAccountNumber { get; set; }

        [DataType(DataType.Text, ErrorMessage = "Please enter a valid first name")]
        [StringLength(256)]
        [Display(Name = "First name")]
        public String Firstname { get; set; }

        [DataType(DataType.Text, ErrorMessage = "Please enter a valid last name.")]
        [StringLength(256)]
        [Display(Name = "Last name")]
        public String Lastname { get; set; }

        [Display(Name = "Gender")]
        public String Gender { get; set; }
    }

}