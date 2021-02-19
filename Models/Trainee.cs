using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Trainee:ApplicationUser
    {
        public int Age { get; set; }
        [DataType(DataType.Date)]
        public string Date_of_birth { get; set; }
        public string Main_programming_lang { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public float TOEIC_score { get; set; }
        public string Exp_details { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Education { get; set; }
    }
}