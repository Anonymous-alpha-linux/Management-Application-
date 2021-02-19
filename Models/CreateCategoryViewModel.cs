using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CreateCategoryViewModel
    {
        public Category Category { get; set; }

        public IEnumerable<Category> Categories { get; set; }
    }
}