using System;
using System.ComponentModel.DataAnnotations;

namespace Rallypoint.Models
{
    public class RegisterViewModel : BaseEntity
    {
        [Required(ErrorMessage = "First name is required")]
        [MinLength(2, ErrorMessage = "First name must be at least two characters long")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only consist of letters")]
        public string first_name { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MinLength(2, ErrorMessage = "Last name must be at least two characters long")]
        public string last_name { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least three characters long")]
        public string username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MinLength(5, ErrorMessage = "Email is too short")]
        public string email { get; set; }

        [Required(ErrorMessage = "Set a password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Compare("password", ErrorMessage = "Passwords do not match")]
        public string password_confirmation { get; set; }
    }

    public class LoginViewModel : BaseEntity{

        [Required(ErrorMessage = "Username or email is required")]

        public string identity {get; set;}

       
        [Required(ErrorMessage = "Password is required")]
        [MinLength(5)]
        [DataType(DataType.Password)]

        public string password {get; set;}
    }
}
