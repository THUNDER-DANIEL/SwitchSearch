using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlannerRedo.Models
{
    public class User
    {
        // auto-implemented properties need to match the columns in your table
        // the [Key] attribute is used to mark the Model property being used for your table's Primary Key
        [Key]
        public int UserId {get;set;}
// =====================================================================
        [Required(ErrorMessage="First Name cannot be blank")]
        [MinLength(2, ErrorMessage="First Name must be at least 2 characters")]
        [Display(Name="First Name")]
        public string FirstName {get;set;}
// =====================================================================
        [Required(ErrorMessage="Last Name cannot be blank")]
        [MinLength(2, ErrorMessage="Last Name must be at least 2 characters")]
        [Display(Name="Last Name")]
        public string LastName {get;set;}
// =====================================================================
        [Required(ErrorMessage="Email cannot be blank")]
        [EmailAddress]
        [Display(Name="Email")]
        public string Email {get;set;}
// =====================================================================
        [Required]
        [MinLength(8, ErrorMessage="Password must be at least 8 letters long")]
        [DataType(DataType.Password)]
        public string Password {get;set;}
// =====================================================================
        [NotMapped]
        [Compare("Password", ErrorMessage="Passwords must match")]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        public string Confirm {get;set;}
// =====================================================================
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}