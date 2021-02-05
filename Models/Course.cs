using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseDetail { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}