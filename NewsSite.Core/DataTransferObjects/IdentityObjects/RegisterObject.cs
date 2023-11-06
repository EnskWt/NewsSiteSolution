using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NewsSite.Core.DataTransferObjects.IdentityObjects
{
    public class RegisterObject
    {
        [Required(ErrorMessage = "First name can't be blank.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name can't be blank.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Username can't be blank.")]
        [Remote(action: "IsUsernameFree", controller: "Account", ErrorMessage = "A user with this username already exists.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Email can't be blank.")]
        [Remote(action: "IsEmailFree", controller: "Account", ErrorMessage = "A user with this email already exists.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone number can't be blank.")]
        [Remote(action: "IsPhoneNumberFree", controller: "Account", ErrorMessage = "A user with this phone number already exists.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Password can't be blank.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm password can't be blank.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm password doesn't match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
