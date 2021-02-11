using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CreateCourseViewModel
    {
        public int Id { get; set; }
        public Course Course { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
    public class AssignTrainerViewModel
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string TrainerId { get; set; }
        public IEnumerable<Trainer> Trainers { get; set; }
    }
    public class AssignTraineeViewModel
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string TraineeId { get; set; }
        public IEnumerable<Trainee> Trainees { get; set; }
    }
    public class ResignTraineeViewModel
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public IEnumerable<TraineeCourses> Trainees { get; set; }
    }
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string TrainerId { get; set; }
        public TrainerCourses Trainer { get; set; }
        public string TraineeId { get; set; }
        public List<TraineeCourses> Trainees { get; set; }
    }
}