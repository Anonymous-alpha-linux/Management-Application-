using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private UserManager<ApplicationUser> _userManager;
        public AdminController()
        {
            _context = new ApplicationDbContext();

            _user = new ApplicationUser();

            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }
        // GET: Admin
        public ActionResult Index()
        {
            var users = _context.Users.Where(t => t.Roles.Any(m => m.RoleId == "2" || m.RoleId == "3") == true).ToList();
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
            var staffaccount = _context.Users.Where(t => t.Roles.Any(y => y.RoleId == "2")).ToList();

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
            var traineraccount = _context.Users.Where(t => t.Roles.Any(y => y.RoleId == "3")).ToList();

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
                .SingleOrDefault(t => t.Id == id);

            _context.Users.Remove(traineraccount);

            _context.SaveChanges();
            return RedirectToAction("TrainerAccountView");
        }
        /// <summary>
        /// Delete Trainer or Staff Account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteAccount(string id)
        {
            var traineraccount = _context.Users
                .SingleOrDefault(t => t.Id == id);

            _context.Users.Remove(traineraccount);

            _context.SaveChanges();
            return RedirectToAction("Index");
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
                .SingleOrDefault(t => t.Id == id);
            if (staffaccount == null)
            {
                return HttpNotFound();
            }
            var registeredacocunt = new AdminChangePassViewModel()
            {
                Id = id,
                Email = staffaccount.Email,
            };
            return View(registeredacocunt);
        }
        [HttpPost]
        public ActionResult EditStaffAccount(AdminChangePassViewModel adminChangePassViewModel)
        {
            var edit_infor = _context.Users
                .OfType<Staff>()
                .SingleOrDefault(t => t.Id == adminChangePassViewModel.Id);
            if (!ModelState.IsValid)
            {
                return View(adminChangePassViewModel);
            }
            _userManager.RemovePasswordAsync(edit_infor.Id);
            _userManager.AddPasswordAsync(edit_infor.Id, adminChangePassViewModel.Password);
            _context.SaveChanges();

            return RedirectToAction("StaffAccountView");
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
                Id = id,
                Email = traineraccount.Email,
            };
            return View(registermodel);
        }
        [HttpPost]
        public ActionResult EditTrainerAccount(AdminChangePassViewModel adminChangePassViewModel)
        {
            var edit_infor = _context.Users
                .OfType<Trainer>()
                .SingleOrDefault(t => t.Id == adminChangePassViewModel.Id);
            if (!ModelState.IsValid)
            {
                return View(adminChangePassViewModel);
            }
            _userManager.RemovePasswordAsync(edit_infor.Id);
            _userManager.AddPasswordAsync(edit_infor.Id, adminChangePassViewModel.Password);
            _context.SaveChanges();
            return RedirectToAction("TrainerAccountView");
        }
    }
}