using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL;

namespace DAL
{
    public class BlogModel : BaseModel
    {
        public Blog Blog { get; set; }
        public IEnumerable<BlogPost> Recent { get; set; }

        public IEnumerable<BlogPost> UserPosts { get; set; }
    }
}