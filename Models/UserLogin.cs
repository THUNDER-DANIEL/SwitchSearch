using System.ComponentModel.DataAnnotations;

namespace WeddingPlannerRedo.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage="Email is required")]
        [EmailAddress]
        [Display(Name="Email")]
        [RegularExpression(@"^[a-zA-Z0-9.+_-]+@[a-zA-Z0-9._-]+\.[a-zA-Z]+$", ErrorMessage="Invalid Email/Password Model")]
        public string Email {get;set;}
// =====================================================================
        [Required(ErrorMessage="Password is required")]
        [Display(Name="Password")]
        [DataType(DataType.Password)]
        public string Password {get;set;}
    }
}