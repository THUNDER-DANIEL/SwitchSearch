using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WeddingPlannerRedo.Models
{
    public class WeddingPlan
    {
        [Key]
        public int PlanId {get;set;}
// =====================================================================
        [Required(ErrorMessage="Title must be filled out")]
        // [MinLength(2, ErrorMessage="Title must be at least 3 in charactersa")]
        [Display(Name="Title")]
        public string WedderOne {get;set;}
// =====================================================================
        // [Required(ErrorMessage="Wedder Two must be filled out")]
        // [Display(Name="Wedder Two")]
        // public string WedderTwo {get;set;}
// =====================================================================
        [Required(ErrorMessage="Date field must be selected")]
        [Display(Name="Date")]
        public DateTime? PlayDate {get;set;}
// =====================================================================
        [Required(ErrorMessage="Duration field must be selected")]
        // [MinLength(2, ErrorMessage="Duration must be at least 1 in length")]
        [Display(Name="Duration")]
        public string Duration {get;set;}
// =====================================================================
        [Required(ErrorMessage="Time field must be selected")]
        [Display(Name="Time")]
        public DateTime? PlayTime {get;set;}
// =====================================================================
        [Required(ErrorMessage="Description field must be filled out")]
        [Display(Name="Description")]
        public string WeddingAddress {get;set;}
// =====================================================================
        public int CreatorId {get;set;}
        public User Creator {get;set;}

// =====================================================================
        public List<WeddingGuest> Guests {get; set;}
// =====================================================================
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;


    }
}