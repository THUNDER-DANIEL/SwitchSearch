// forms the relationship between our models and the database

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace WeddingPlannerRedo.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        // "users" table is represented by this DbSet "Users
        public DbSet<User> Users { get; set; }
        public DbSet<WeddingPlan> WeddingPlans { get; set; }
        public DbSet<WeddingGuest> WeddingGuests { get; set; }
    }
}