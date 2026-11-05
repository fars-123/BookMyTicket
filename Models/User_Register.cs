using System.ComponentModel.DataAnnotations;

namespace BookMyTicket.Models
{
    public class User_Register
    {

        public int User_Id { get; set; }

        [Required(ErrorMessage = "Enter User Name")]
        public string? User_Name { get; set; }
        
        [Required(ErrorMessage = "Enter Full Name")]

        public string? FullName { get; set; }

        [Required(ErrorMessage = "Enter email")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Enter Address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must contain exactly 10 digits")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Enter Role")]
        public string? Role { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm your password")]

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }

        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }

    }

    public class ForgotPassword_Model
    {
        [Required(ErrorMessage = "Enter your email")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string? Email { get; set; }
    }

    public class ResetPassword_Model
    {
        [Required]
        public string? Token { get; set; }

        public string? Email { get; set; }

        [Required(ErrorMessage = "Enter new password")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }
    }
}
