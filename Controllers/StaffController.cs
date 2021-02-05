using Microsoft.Ajax.Utilities;
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
    [Authorize(Roles = "Staff")]
    public class StaffController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUser _user;
        private UserManager<ApplicationUser> _userManager;
        public StaffController()
        {
            _context = new ApplicationDbContext();

            _user = new ApplicationUser();

            _userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext())
            );
        }
        // GET: Staff
        
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Trainer Profile Viewers
        /// </summary>
        /// <returns></returns>
        public ActionResult TrainerProfileView()
        {
            var trainers = _context.Users
                .OfType<Trainer>()
                .ToList();
            return View(trainers);
        }
        /// <summary>
        /// Edit Trainer Profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditTrainerProfile(string id)
        {
            var trainer = _context.Users
                .OfType<Trainer>()
                .SingleOrDefault(t => t.Id == id);
            return View(trainer);
        }
        [HttpPost]
        public ActionResult EditTrainerProfile(Trainer trainerViewModels)
        {
            trainerViewModels.UserName = Function.SplitUserComponents(trainerViewModels.UserName);
            var trainer = _context.Users
                .OfType<Trainer>()
                .SingleOrDefault(m => m.Id == trainerViewModels.Id);
            trainer.UserName = trainerViewModels.UserName;
            trainer.ExorInType = trainerViewModels.ExorInType;
            trainer.Working_Address = trainerViewModels.Working_Address;
            trainer.Education = trainerViewModels.Education;
            trainer.PhoneNumber = trainerViewModels.PhoneNumber;
            _context.SaveChanges();
            return RedirectToAction("TrainerProfileView", "Staff");
        }
        /// <summary>
        /// Trainer Profile Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DetailTrainer(string id)
        {
            var trainer = _context.Users
                .OfType<Trainer>()
                .FirstOrDefault(t => t.Id == id);
            return View(trainer);
        }
        /// <summary>
        /// Trainee Account Viewer
        /// </summary>
        /// <returns></returns>
        public ActionResult TraineeAccountView()
        {
            var staffaccount = _context.Users
                .OfType<Trainee>()
                .ToList();

            return View(staffaccount);
        }
        /// <summary>
        /// Delete Account Viewer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteTraineeAccount(string id)
        {
            var traineraccount = _context.Users
                .SingleOrDefault(t => t.Id == id);

            _context.Users.Remove(traineraccount);

            _context.SaveChanges();

            return RedirectToAction("TraineeAccountView");
        }
        /// <summary>
        /// Edit Trainee Account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditTraineeAccount(string id)
        {
            var trainee = _context.Users
                .OfType<Trainee>()
                .SingleOrDefault(t => t.Id == id);
            if (trainee == null)
            {
                return HttpNotFound();
            }
            var traineedetail = new ChangeTraineeAccountAndInfor()
            {
                Id = id,
                Email = trainee.Email,
                Trainee = trainee,
            };

            return View(traineedetail);
        }
        [HttpPost]
        public ActionResult EditTraineeAccount(ChangeTraineeAccountAndInfor changeTraineeDetaill)
        {
            var edit_infor = _context.Users
                .OfType<Trainee>()
                .FirstOrDefault(t => t.Id == changeTraineeDetaill.Id);
            if (!ModelState.IsValid)
            {
                changeTraineeDetaill.Email = edit_infor.Email;
                return View(changeTraineeDetaill);
            }
            edit_infor.Age = changeTraineeDetaill.Trainee.Age;
            edit_infor.Date_of_birth = changeTraineeDetaill.Trainee.Date_of_birth;
            edit_infor.Department = changeTraineeDetaill.Trainee.Department;
            edit_infor.Education = changeTraineeDetaill.Trainee.Education;
            edit_infor.Exp_details = changeTraineeDetaill.Trainee.Exp_details;
            edit_infor.TOEIC_score = changeTraineeDetaill.Trainee.TOEIC_score;
            edit_infor.Location = changeTraineeDetaill.Trainee.Location;
            edit_infor.UserName = Function.SplitUserComponents(changeTraineeDetaill.Trainee.UserName);
            edit_infor.Main_programming_lang = changeTraineeDetaill.Trainee.Main_programming_lang;

            _userManager.RemovePassword(edit_infor.Id);
            _userManager.AddPasswordAsync(edit_infor.Id, changeTraineeDetaill.Password);

            _userManager.UpdateAsync(edit_infor);
            _context.SaveChanges();

            return RedirectToAction("TraineeAccountView");
        }
        public ActionResult TraineeDetails(string id)
        {
            var traineeDetails = _context.Users
                .OfType<Trainee>()
                .FirstOrDefault(t => t.Id == id);
            return View(traineeDetails);
        }
        /// <summary>
        /// Course Viewer
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseView()
        {
            if (!ModelState.IsValid)
            {
                return HttpNotFound();
            }
            var courses = _context.Courses
                .ToList();
            
            return View(courses);
        }
        /// <summary>
        /// Create Category
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCategory(Category createCategoryViewModel)
        {
            var categories = _context.Categories.ToList();
            if (categories.Any(m =>m.CategoryName == createCategoryViewModel.CategoryName))
            {
                ModelState.AddModelError("Validation", "Unable to save this value. The database entity exists in the database.");
                return View();
            }
            _context.Categories.Add(createCategoryViewModel);
            _context.SaveChanges();
            return RedirectToAction("CreateCategory", "Staff");
        }
        public ActionResult DeleteCategory(Category categoryViewModel)
        {
            var categories = _context.Categories.FirstOrDefault(t => t.CategoryName == categoryViewModel.CategoryName);
            _context.Categories.Remove(categories);
            return View();
        }
    }

    class Function
    {
        public static string SplitUserComponents(string userName)
        {
            var nameLst = userName.Split(' ').ToList();
            string returnName = "";
            foreach (var chr in nameLst)
            {
                for (int i = 0; i < chr.Length; i++)
                {
                    if (i == 0) returnName += Char.ToUpper(chr[i]);
                    else
                        returnName += chr[i];
                }
                returnName += " ";
            }

            return returnName;
        }
    }
}