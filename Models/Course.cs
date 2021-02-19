using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field must be defined!")]
        public string CourseName { get; set; }
        [Required(ErrorMessage = "This field must be defined!")]
        public string CourseDetail { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}