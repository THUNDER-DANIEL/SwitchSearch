using System.ComponentModel.DataAnnotations;

namespace WeddingPlannerRedo.Models
{
    public class WeddingGuest
    {
        [Key]
        public int GuestId {get;set;}
        public int UserId {get;set;}
        public int PlanId {get;set;}
        public string IsAttending {get;set;}
        public User User {get;set;}
        public WeddingPlan WeddingPlan {get;set;}
    }
}