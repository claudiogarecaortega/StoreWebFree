using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BassinoLibrary.Resource;

namespace BassinoBase.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class PersonViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }

    public class UserExtendedViewModel
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public string FullName { get; set; }
        public string ValidationToken { get; set; }
        public string NombreApellido { get; set; }
        public int userId { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
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
    public class ProfileViewModel
    {
        public int id { get; set; }
        public string IdAsp { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Phone")]
        public string PhoneNumber { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Phone")]
        public string CellPhoneNumber { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "LastName")]
        public string LastName { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Email")]
        public string Email { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "UserName")]
        public string UserName { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Description { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "BirthDate")]
        public string BornDate { get; set; }
        public string Adresse { get; set; }
        public string Ubication { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Position")]
        public string Position { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Roles")]
        public string Roles { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Roles")]
        public int RolesId { get; set; }
        
    }
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
       // [EmailAddress]
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
        [Display(ResourceType = typeof(Resources), Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "ErrorLengthValidation",
            ErrorMessageResourceType = typeof(Resources), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = "ConfirmPassword")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "ErrorPasswordNoMatch",
            ErrorMessageResourceType = typeof(Resources))]
        public string ConfirmPassword { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Roles")]
        public string Roles { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Roles")]
        public string RolesId { get; set; }
        
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "ErrorLengthValidation",
            ErrorMessageResourceType = typeof(Resources), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = "ConfirmPassword")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "ErrorPasswordNoMatch",
            ErrorMessageResourceType = typeof(Resources))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof(Resources), Name = "Email")]
        public string Email { get; set; }
    }
}
