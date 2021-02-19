using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.UI;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    
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
        [Authorize(Roles = "Staff")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Trainer Profile Viewers
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Staff")]
        public ActionResult TrainerProfileView(string searchString)
        {
            var trainers = _context.Users
                .OfType<Trainer>()
                .Where(t=>t.UserName.Contains(searchString))
                .ToList();
            if (String.IsNullOrEmpty(searchString))
            {
                trainers = _context.Users
                .OfType<Trainer>()
                .ToList();
            }
            return View(trainers);
        }
        /// <summary>
        /// Edit Trainer Profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Staff")]
        public ActionResult EditTrainerProfile(string id)
        {
            var trainer = _context.Users
                .OfType<Trainer>()
                .SingleOrDefault(t => t.Id == id);
            return View(trainer);
        }
        [Authorize(Roles = "Staff")]
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
        [Authorize(Roles = "Staff,Trainer")]
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
        [Authorize(Roles = "Staff")]
        public ActionResult TraineeAccountView(string searchString,string option)
        {
            var staffaccount = _context.Users
                    .OfType<Trainee>()
                    .ToList();
            if (option == "Name") {
                 staffaccount = _context.Users
                    .OfType<Trainee>()
                    .Where(t => t.UserName.Contains(searchString))
                    .ToList();
            }
            if (option == "TOEIC")
            {
                 staffaccount = _context.Users
                    .OfType<Trainee>()
                    .Where(t => t.TOEIC_score.ToString().StartsWith(searchString))
                    .ToList();
            }
            if (option == "Programming Language")
            {
                 staffaccount = _context.Users
                    .OfType<Trainee>()
                    .Where(t => t.Main_programming_lang.Contains(searchString))
                    .ToList();
            }
            if (option == "Email")
            {
                staffaccount = _context.Users
                   .OfType<Trainee>()
                   .Where(t => t.Email.Contains(searchString))
                   .ToList();
            }
            if (option == "Age")
            {
                staffaccount = _context.Users
                   .OfType<Trainee>()
                   .Where(t => t.Age.ToString().Equals(searchString))
                   .ToList();
            }
            if (String.IsNullOrEmpty(searchString))
            {
                staffaccount = _context.Users
                .OfType<Trainee>()
                .ToList();
            }
            
            return View(staffaccount);
        }
        /// <summary>
        /// Delete Account Viewer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Staff")]
        public ActionResult DeleteTraineeAccount(string id)
        {
            var traineeaccount = _context.Users
                .OfType<Trainee>()
                .FirstOrDefault(t => t.Id == id);
            var traineeCourse = _context.TraineeCourses
                .Where(t => t.TraineeId == id)
                .ToList();
            _context.Users.Remove(traineeaccount);
            if (traineeCourse != null)
            {
                foreach (var trainee in traineeCourse)
                {
                    _context.TraineeCourses.Remove(trainee);
                }
            }
            _context.SaveChanges();

            return RedirectToAction("TraineeAccountView");
        }
        /// <summary>
        /// Edit Trainee Account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Staff")]
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
        [Authorize(Roles = "Staff")]
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

            _userManager.RemovePassword(changeTraineeDetaill.Id);
            _userManager.AddPassword(changeTraineeDetaill.Id, changeTraineeDetaill.Password);

            _userManager.Update(edit_infor);
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
        /// View Categories
        /// </summary>
        /// <returns></returns>
        ///
        [Authorize(Roles = "Staff")]
        public ActionResult CategoryView(string searchString)
        {
            var categories = _context.Categories
                .ToList();
            if(searchString != null)
            {
                categories = _context.Categories
                .Where(t => t.CategoryName.Contains(searchString))
                .ToList();
            }
            return View(categories);
        }
        /// <summary>
        /// Create Category
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Staff")]
        public ActionResult CreateCategory()
        {
            return View();
        }
        [Authorize(Roles = "Staff")]
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
            return RedirectToAction("CategoryView", "Staff");
        }
        [Authorize(Roles = "Staff")]
        public ActionResult EditCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(t => t.Id == id);

            return View(category);
        }
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult EditCategory(Category item)
        {
            var category = _context.Categories.Where(t => t.Id == item.Id).FirstOrDefault();
            
            if (_context.Categories.Any(m => m.CategoryName == item.CategoryName && m.CategoryName != category.CategoryName))
            {
                ModelState.AddModelError("Validation", "Unable to save this value. The database entity exists in the database.");
                return View(category);
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Detected fault","Cannot Update this value");
                return View(category);
            }
            category.CategoryName = item.CategoryName;
            category.Description = item.Description;
            _context.SaveChanges();

            return RedirectToAction("CategoryView","Staff");
        }
        [Authorize(Roles = "Staff")]
        public ActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(t => t.Id == id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("CategoryView");
        }
        /// <summary>
        /// Course Viewer
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Staff,Trainee")]
        public ActionResult CourseView(string searchString,string option)
        {
            if (!ModelState.IsValid)
            {
                return HttpNotFound();
            }
            var courses = _context.Courses
               .Include(t => t.Category)
               .ToList();
            if(option == "Name")
            {
                courses = _context.Courses
               .Where(t => t.CourseName.Contains(searchString))
               .Include(t => t.Category)
               .ToList();
            }
            if (option == "Category")
            {
                courses = _context.Courses
               .Where(t => t.Category.CategoryName.Contains(searchString))
               .Include(t => t.Category)
               .ToList();
            }
            if (option == "Description")
            {
                courses = _context.Courses
               .Where(t => t.CourseDetail.Contains(searchString))
               .Include(t => t.Category)
               .ToList();
            }
            return View(courses);
        }
        /// <summary>
        /// Create Course 
        /// </summary>
        /// <returns></returns>
        /// 

        [Authorize(Roles = "Staff")]
        public ActionResult CreateCourse()
        { 
            var model = new CreateCourseViewModel()
            {
                Categories = _context.Categories.ToList(),
            };
            return View(model);
        }

        [Authorize(Roles = "Staff")]
        [HttpPost]
        public ActionResult CreateCourse(CreateCourseViewModel model)
        {
            var created_course = new Course();
            created_course.CourseName = model.Course.CourseName;
            created_course.CourseDetail = model.Course.CourseDetail;
            created_course.CategoryId = model.Course.CategoryId;

            _context.Courses.Add(created_course);
            _context.SaveChanges();
            return RedirectToAction("CourseView","Staff");
        }

        [Authorize(Roles = "Staff")]
        public ActionResult DeleteCourse(int id)
        {
            var removedCourse = _context.Courses.SingleOrDefault(t => t.Id == id);
            _context.Courses.Remove(removedCourse);
            _context.SaveChanges();
            return RedirectToAction("CourseView","Staff");
        }

        [Authorize(Roles = "Staff")]
        public ActionResult EditCourse(int id)
        {
            var course = _context.Courses.SingleOrDefault(t => t.Id == id);
            var model = new CreateCourseViewModel()
            {
                Id = id,
                Course = course,
                Categories = _context.Categories.ToList(),
            };
            return View(model);
        }

        [Authorize(Roles = "Staff")]
        [HttpPost]
        public ActionResult EditCourse(CreateCourseViewModel model)
        {
            var created_course = _context.Courses.SingleOrDefault(t => t.Id == model.Id);
            created_course.CourseName = model.Course.CourseName;
            created_course.CourseDetail = model.Course.CourseDetail;
            created_course.CategoryId = model.Course.CategoryId;

            _context.SaveChanges();
            return RedirectToAction("CourseView", "Staff");
        }

        [Authorize(Roles = "Staff,Trainee")]
        public ActionResult DetailCourse(int id)
        {
            var trainerNoNull = _context.TrainerCourses
                .Any(t => t.CourseId == id);
            var traineeNoNull = _context.TraineeCourses
                .Any(t => t.CourseId == id);
            var course = new CourseViewModel();
            course.CourseId = id;
            course.Course = _context.Courses.Include(t=>t.Category).SingleOrDefault(t => t.Id == id);
            if (trainerNoNull) 
                course.Trainer = _context.TrainerCourses
                    .Include(t=>t.Trainer)
                    .SingleOrDefault(t => t.CourseId == id);
            if (traineeNoNull) 
                course.Trainees = _context.TraineeCourses
                    .Where(t => t.CourseId == id)
                    .Include(t=>t.Trainee)
                    .ToList();
            return View(course);
        }
        /// <summary>
        /// Assign Trainer to Course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Staff")]
        public ActionResult AssignCourseToTrainer(int id)
        {
            var trainercourses = new AssignTrainerViewModel()
            {
                CourseId = id,
                Course = _context.Courses.SingleOrDefault(t => t.Id == id),
                Trainers = _context.Users.OfType<Trainer>().ToList(),
            };
            return View(trainercourses);
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        public ActionResult AssignCourseToTrainer(AssignTrainerViewModel model)
        {
            if (_context.TrainerCourses.Select(t => t.CourseId).Contains(model.CourseId))
            {
                var old_trainer = _context.TrainerCourses
                    .SingleOrDefault(t => t.CourseId == model.CourseId);

                old_trainer.CourseId = model.CourseId;
                old_trainer.TrainerId = model.TrainerId;
                _context.SaveChanges();
                return RedirectToAction("DetailCourse", "Staff",new { @id = model.CourseId});
            }

            var assignCourse = new TrainerCourses();

            assignCourse.CourseId = model.CourseId;
            assignCourse.TrainerId = model.TrainerId;

            _context.TrainerCourses.Add(assignCourse);
            
            _context.SaveChanges();
            return RedirectToAction("DetailCourse", "Staff", new { @id = model.CourseId });
        }
        public ActionResult ResignTrainerFromCourse(int id)
        {
            var trainerCourses = _context.TrainerCourses
                .SingleOrDefault(t => t.Id == id);
            _context.TrainerCourses.Remove(trainerCourses);
            _context.SaveChanges();

            return RedirectToAction("DetailCourse", "Staff", new { @id = trainerCourses.CourseId });
        }
        /// <summary>
        /// Assign Trainee to Course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Staff")]
        public ActionResult AssignCourseToTrainee(int id)
        {
            var traineecourses = new AssignTraineeViewModel()
            {
                CourseId = id,
                Course = _context.Courses.SingleOrDefault(t => t.Id == id),
                Trainees = _context.Users.OfType<Trainee>().ToList(),
            };
            return View(traineecourses);
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        public ActionResult AssignCourseToTrainee(int CourseId, AssignTraineeViewModel model)
        {
            var assignCourse = new TraineeCourses();
            assignCourse.CourseId = model.CourseId;
            assignCourse.TraineeId = model.TraineeId;
            if (!_context.TraineeCourses
                .Where(t => t.CourseId == model.CourseId)
                .Select(t=>t.TraineeId)
                .Contains(model.TraineeId))
            {
                _context.TraineeCourses.AddOrUpdate(assignCourse);
                _context.SaveChanges();
                ViewBag.Message = "Assign successfully";
            }
            else
            {
                ModelState.AddModelError("Validation","This trainee has been assigned already");
            }

            return RedirectToAction("DetailCourse", "Staff", new { @id = CourseId });
        }
        [Authorize(Roles = "Staff")]
        public ActionResult ResignTraineeFromCourse(int id)
        {
            var traineeCourses = _context.TraineeCourses
                .SingleOrDefault(t => t.Id == id);
            _context.TraineeCourses.Remove(traineeCourses);
            _context.SaveChanges();

            return RedirectToAction("DetailCourse", "Staff", new { @id = traineeCourses.CourseId});
        }
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
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