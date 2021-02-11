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
    [Authorize(Roles = "Trainer")]
    public class TrainerController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUser _user;
        private UserManager<ApplicationUser> _userManager;
        public TrainerController()
        {
            _context = new ApplicationDbContext();

            _user = new ApplicationUser();

            _userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext())
            );
        }
        // GET: Trainer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateDetails()
        {
            var currentUserId = User.Identity.GetUserId();

            var trainerDetail = _context.Users
                .OfType<Trainer>()
                .SingleOrDefault(t => t.Id == currentUserId);

            return View(trainerDetail);
        }
        [HttpPost]
        public ActionResult UpdateDetails(Trainer trainerInput)
        {
            var currentUserId = User.Identity.GetUserId();

            var trainerDetail = _context.Users
                .OfType<Trainer>()
                .SingleOrDefault(t => t.Id == currentUserId);

            trainerDetail.UserName = trainerInput.UserName;
            trainerDetail.Working_Address = trainerInput.Working_Address;
            trainerDetail.Education = trainerInput.Education;
            trainerDetail.PhoneNumber = trainerInput.PhoneNumber;

            _context.SaveChanges();

            return RedirectToAction("Index", "Manage");
        }
        public ActionResult ViewAssignedCourses()
        {
            var currentUserId = User.Identity.GetUserId();

            var assignedCourse = _context.TrainerCourses
                .Where(t => t.TrainerId == currentUserId)
                .Select(t => t.Course)
                .Include(t=>t.Category)
                .ToList();

            return View(assignedCourse);
        }
    }
}