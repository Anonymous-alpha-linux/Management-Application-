using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUser _user;
        private UserManager<IdentityUser> _userManager;
        public AdminController()
        {
            _context = new ApplicationDbContext();

            _user = new ApplicationUser();

            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
        }
        // GET: Admin
        public ActionResult Index()
        {
            var users = _context.Users
                .Where(t => t.Roles
                    .Any(y => y.RoleId == "2" || y.RoleId == "3"))
                .ToList();
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }
        /// <summary>
        /// View Staff Account
        /// </summary>
        /// <returns></returns>
        public ActionResult StaffAccountView()
        {
            var staffaccount = _context.Users
                .OfType<Staff>()
                .ToList();

            if (staffaccount == null)
            {
                return HttpNotFound();
            }

            return View(staffaccount);
        }
        /// <summary>
        /// View Trainer Account
        /// </summary>
        /// <returns></returns>
        public ActionResult TrainerAccountView()
        {
            var traineraccount = _context.Users
                .OfType<Trainer>()
                .ToList();

            if (traineraccount == null)
            {
                return HttpNotFound();
            }
            return View(traineraccount);
        }
        /// <summary>
        /// Delete Staff Account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteStaffAccount(string id)
        {
            var staffaccount = _context.Users
                .SingleOrDefault(t => t.Id == id);

            _context.Users.Remove(staffaccount);

            _context.SaveChanges();

            return RedirectToAction("StaffAccountView");
        }
        /// <summary>
        /// Delete Trainer Account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteTrainerAccount(string id)
        {
            var traineraccount = _context.Users
                .OfType<Trainer>()
                .SingleOrDefault(t => t.Id == id);

            var trainerCourse = _context.TrainerCourses
                .Where(t => t.TrainerId == id)
                .ToList();

            foreach (var trainer in trainerCourse)
            {
                _context.TrainerCourses.Remove(trainer);
            }

            _context.Users.Remove(traineraccount);

            _context.SaveChanges();
            return RedirectToAction("TrainerAccountView");
        }
        /// <summary>
        /// Get elements of IdentityUser that sastifies the specific id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditStaffAccount(string id)
        {
            var staffaccount = _context.Users
                .OfType<Staff>()
                .FirstOrDefault(t => t.Id == id);
            if (staffaccount == null)
            {
                return HttpNotFound();
            }
            var registeredacocunt = new AdminChangePassViewModel()
            {
                Id = staffaccount.Id,
                Email = staffaccount.Email,
            };
            return View(registeredacocunt);
        }
        [HttpPost]
        public ActionResult EditStaffAccount(AdminChangePassViewModel model)
        {
            var staff = _context.Users
                .OfType<Staff>()
                .SingleOrDefault(t => t.Id == model.Id);
            staff.Email = model.Email;
            if (!ModelState.IsValid)
            {
                
                return View(model);
            }

            if (staff.PasswordHash != null)
            {
                _userManager.RemovePassword(staff.Id);
            }
            _userManager.AddPassword(staff.Id, model.Password);
            _context.SaveChanges();

            return RedirectToAction("StaffAccountView","Admin");
        }

        /// <summary>
        /// Get elements of IdentityUser that sastifies the specific id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditTrainerAccount(string id)
        {
            var traineraccount = _context.Users
                .OfType<Trainer>()
                .SingleOrDefault(t => t.Id == id);
            if (traineraccount == null)
            {
                return HttpNotFound();
            }

            var registermodel = new AdminChangePassViewModel()
            {
                Id = traineraccount.Id,
                Email = traineraccount.Email,
            };
            return View(registermodel);
        }
        [HttpPost]
        public ActionResult EditTrainerAccount(AdminChangePassViewModel model)
        {
            var trainer = _context.Users
                .OfType<Trainer>()
                .SingleOrDefault(t => t.Id == model.Id);
            trainer.Email = model.Email;

            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            if (trainer.PasswordHash != null)
            {
                _userManager.RemovePassword(trainer.Id);
            }
            _userManager.AddPassword(trainer.Id, model.Password);

            _context.SaveChanges();
            
            return RedirectToAction("TrainerAccountView", "Admin");
        }
    }
}