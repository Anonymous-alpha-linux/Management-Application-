using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Trainer:ApplicationUser
    {
        public string Education { get; set; }
        public string Working_Address { get; set; }
        public string ExorInType { get; set; }
    }
}