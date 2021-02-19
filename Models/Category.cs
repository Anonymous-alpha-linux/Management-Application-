using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field must be defined!")]
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}