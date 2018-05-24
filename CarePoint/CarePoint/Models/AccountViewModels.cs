using CarePoint.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System;

namespace CarePoint.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email or Phone")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w)+)+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "Please enter a valid email address or phone number")]
        public string EmailOrPhone { get; set; }

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
        [Display(Name = "First Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [RegularExpression("^[a-zA-Z\u0621-\u064A]*$", ErrorMessage = "First Name must contain alphabetical characters only!")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Middle Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [RegularExpression("^[a-zA-Z\u0621-\u064A]*$", ErrorMessage = "First Name must contain alphabetical characters only!")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [RegularExpression("^[a-zA-Z\u0621-\u064A]*$", ErrorMessage = "First Name must contain alphabetical characters only!")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression("[0-9]{11}$",ErrorMessage = "Phone must contain 11 numbers only!")]
        [Remote("IsPhoneNumberExists", "Account", ErrorMessage = "Phone Number Already Exist.")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [Remote("IsEmailExists", "Account", ErrorMessage = "Email Already Exist.")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Day")]
        [CalendarDay("Month", "Year", ErrorMessage = "The Birthdate is not a valid date !")]
        public int Day { get; set; }

        [Required]
        [Display(Name = "Month")]
        public int Month { get; set; }

        [Required]
        [Display(Name = "Year")]
        public int Year { get; set; }

        public DateTime DateOfBirth {
            get
            {
                return new DateTime(Year,Month,Day);
            }
            private set { }
        }


        [Required]
        [Display(Name = "Gender")]
        public bool IsMale { get; set; }

        [Required]
        [Display(Name = "National ID Photo")]
        public HttpPostedFileWrapper NationalIDPhoto { get; set; }

        [Required]
        [Display(Name = "National ID Number")]
        [RegularExpression("[0-9]{14}$", ErrorMessage = "National Id Number must contain 14 numbers only!")]
        [Remote("IsNationalIDExists", "Account", ErrorMessage = "National Id Already Exist.")]
        public string NationalIDNumber { get; set; }

        [Required]
        [Display(Name = "Blood Type")]
        public long BloodTypeID { get; set; }

        [Display(Name = "Profession License")]
        public HttpPostedFileWrapper License { get; set; }

        [Required]
        [Display(Name = "Speciality")]
        public int SpecialityID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public List<SelectListItem> Days { get; set; }
        public List<SelectListItem> Months { get; set; }
        public List<SelectListItem> Years { get; set; }
        public List<SelectListItem> Specialities { get; set; }
        public List<SelectListItem> BloodTypes { get; set; }
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
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
