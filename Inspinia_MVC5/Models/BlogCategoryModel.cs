using System.Collections.Generic;
using DAL;


namespace BootStrap.Models
{
    public class BlogCategoryModel
    {
        public IEnumerable<BlogPost> Posts { get; set; }
        public IEnumerable<Category> categories { get; set; }

        public IEnumerable<BlogPost> Recent { get; set; }
        public int Category_ID { get; set; }
        public string KeyWords { get; set; }
    }
}