﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Trainee")]
    public class TraineeController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUser _user;
        private UserManager<ApplicationUser> _userManager;
        public TraineeController()
        {
            _context = new ApplicationDbContext();

            _user = new ApplicationUser();

            _userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext())
            );
        }
        // GET: Trainee
        public ActionResult Index()
        {
            return View(_context.Courses.ToList());
        }
        public ActionResult UpdateDetails()
        {
            var currentUserId = User.Identity.GetUserId();

            var traineeDetail = _context.Users
                .OfType<Trainee>()
                .SingleOrDefault(t => t.Id == currentUserId);

            return View(traineeDetail);
        }
        [HttpPost]
        public ActionResult UpdateDetails(Trainee traineeInput)
        {
            var currentUserId = User.Identity.GetUserId();

            var traineeDetail = _context.Users
                .OfType<Trainee>()
                .SingleOrDefault(t => t.Id == currentUserId);

            traineeDetail.UserName = traineeInput.UserName;
            traineeDetail.Age = traineeInput.Age;
            traineeDetail.Education = traineeInput.Education;
            traineeDetail.PhoneNumber = traineeInput.PhoneNumber;
            traineeDetail.Date_of_birth = traineeInput.Date_of_birth;
            traineeDetail.Department = traineeInput.Department;
            traineeDetail.Exp_details = traineeInput.Exp_details;
            traineeDetail.Location = traineeInput.Location;
            traineeDetail.Main_programming_lang = traineeInput.Main_programming_lang;
            traineeDetail.TOEIC_score = traineeInput.TOEIC_score;


            _context.SaveChanges();

            return RedirectToAction("Index", "Manage");
        }
        public ActionResult ViewAssignedCourses()
        {
            var currentUserId = User.Identity.GetUserId();

            var assignedCourse = _context.TraineeCourses
                .Where(t => t.TraineeId == currentUserId)
                .Select(t => t.Course)
                .Include(t=>t.Category)
                .ToList();

            return View(assignedCourse);
        }
    }
}