using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WeddingPlannerRedo.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace WeddingPlannerRedo.Controllers
{
    public class HomeController : Controller
    {
        private Context dbContext;
     
        // here we can "inject" our context service into the constructor
        public HomeController(Context context)
        {
            dbContext = context;
        }
// ===================================================================== Register & Login Page
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }
// ===================================================================== Post Register Form
        [HttpPost("register")]
        public IActionResult Register(ViewModel NewUser)
        {
            User RegisterUser = NewUser.User;
            //Validation Check
            if(ModelState.IsValid)
            {
                //Check DB for existing email.
                if(dbContext.Users.Any(u => u.Email == RegisterUser.Email))
                {
                    ModelState.AddModelError("User.Email", "Email already taken");
                    return View("Index");
                }
                //Hash Password
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                RegisterUser.Password = hasher.HashPassword(RegisterUser, RegisterUser.Password);
                dbContext.Add(RegisterUser);
                dbContext.SaveChanges();

                // Set Session Upon registering
                HttpContext.Session.SetInt32("ID", RegisterUser.UserId);
                int? UserId = HttpContext.Session.GetInt32("ID");
                System.Console.WriteLine($"\n\n\n{UserId}");
                return RedirectToAction("Dashboard");
            }
            //Validations Failed: Return to Index
            return View("Index");
        }
// ===================================================================== Post Login Form
        [HttpPost("login")]
        public IActionResult Login(UserLogin userlogin)
        {
            //Check Validations
            if(ModelState.IsValid)
            {
                //If userInDB not found in DB: Error out
                User userInDB = dbContext.Users.FirstOrDefault(u => u.Email == userlogin.Email);
                if(userInDB == null)
                {
                    ModelState.AddModelError("Userlogin.Email", "Invalid Email/Password");
                    return View("Index");
                }
                // Compare password hasher with DB hash
                var hasher = new PasswordHasher<UserLogin>();
                var result = hasher.VerifyHashedPassword(userlogin, userInDB.Password, userlogin.Password);
                // If result is 0: Error out
                if(result == 0)
                {
                    ModelState.AddModelError("Userlogin.Email", "Invalid Email/Passowrd");
                    return View("Index");
                }
                // Set Session Upon logging in
                HttpContext.Session.SetInt32("ID", userInDB.UserId);
                int? UserId = HttpContext.Session.GetInt32("ID");
                System.Console.WriteLine($"\n\n\n{UserId}");

                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }
// ===================================================================== Get: DashBoard 
        [HttpGet("dashboard")]
        public IActionResult DashBoard()
        {
            // Checks Session if stored
            if(HttpContext.Session.GetInt32("ID") == null)
            {
                return RedirectToAction("Index");
            }
            // Set int id to equal session ID
            int? id = HttpContext.Session.GetInt32("ID") ?? default(int);
            // CurrentUser set to the id in the DB
            ViewBag.CurrentUser = dbContext.Users.FirstOrDefault(u => u.UserId == id);

            List<WeddingPlan> allplans = dbContext.WeddingPlans
            .Include(c=>c.Creator)
            .Include(w=>w.Guests).OrderByDescending(w => w.CreatedAt).ToList();
            return View(allplans);
        }
// ===================================================================== Get: New Wedding
        [HttpGet("new")]
        public IActionResult NewWedding()
        {
            // Checks Session if stored
            if(HttpContext.Session.GetInt32("ID") == null)
            {
                return RedirectToAction("Index");
            }
            // Set int id to equal session ID
            int id = HttpContext.Session.GetInt32("ID") ?? default(int);
            // CurrentUser set to the id in the DB
            ViewBag.CurrentUser = dbContext.Users.FirstOrDefault(u => u.UserId == id);
            return View();
        }
// ===================================================================== Post: Create New Wedding
        [HttpPost("create-wedding")]
        public IActionResult CreateWeddingPlan(WeddingPlan weddingplan)
        {
            if(ModelState.IsValid)
            {
                // Creator ID
                ViewBag.CurrentUser = HttpContext.Session.GetInt32("ID");
                dbContext.WeddingPlans.Add(weddingplan);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            // Set CurrentUser to session ID
            ViewBag.CurrentUser = HttpContext.Session.GetInt32("ID");
            ViewBag.users = dbContext.Users;
            return View("NewWedding");
        }
// ===================================================================== View Wedding Details
        [HttpGet("activity/{PlanId}")]
        public IActionResult WeddingDetails(int PlanId)
        {
            // Checks Session if stored
            if(HttpContext.Session.GetInt32("ID") == null)
            {
                return RedirectToAction("Index");
            }
            WeddingPlan CurrentWedding = dbContext.WeddingPlans
            .Include(w => w.Guests)
            .ThenInclude(w => w.User)
            .SingleOrDefault(w => w.PlanId == PlanId);
            return View(CurrentWedding);
        }
// ===================================================================== Get RSVP
        [HttpGet("RSVP/{PlanId}")]
        public IActionResult RSVP(int PlanId)
        {
            if(HttpContext.Session.GetInt32("ID") == null)
            {
                return RedirectToAction("Index");
            }
            User CurrentUser = dbContext.Users
            .SingleOrDefault(u => u.UserId == HttpContext.Session.GetInt32("ID"));

            WeddingPlan CurrentWedding = dbContext.WeddingPlans
            .Include(u => u.Guests)
            .ThenInclude(u => u.User)
            .SingleOrDefault(u => u.PlanId == PlanId);

            WeddingGuest thisguest = dbContext
            .WeddingGuests.Where(w => w.PlanId == PlanId && w.UserId == CurrentUser.UserId)
            .FirstOrDefault();

            if(thisguest != null)
            {
                CurrentWedding.Guests.Remove(thisguest);
            }
            else
            {
                WeddingGuest NewGuest = new WeddingGuest {
                    UserId = CurrentUser.UserId,
                    PlanId = CurrentWedding.PlanId,
                };
                CurrentWedding.Guests.Add(NewGuest);
            }
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }


// ===================================================================== Delete Current Users Plan
        [HttpGet("delete/{id}")]
        public IActionResult DeletePlan(int id)
        {
            // Session Check
            if(HttpContext.Session.GetInt32("ID") == null)
            {
                return RedirectToAction("Index");
            }
            // Delete PlanId
            WeddingPlan deleteitem = dbContext.WeddingPlans
            .FirstOrDefault(p => p.PlanId == id);
            // Delete only on Current Session ID
            // if(deleteitem.PlanId != HttpContext.Session.GetInt32("ID"))
            // {
            //     return RedirectToAction("Dashboard");
            // }
            dbContext.WeddingPlans.Remove(deleteitem);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

// ===================================================================== Logout: Clear Session
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
