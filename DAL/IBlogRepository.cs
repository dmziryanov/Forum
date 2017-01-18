using System.Collections.Generic;

namespace DAL
{
    public interface IBlogRepository
    {
        Blog Get(int i);
        List<Blog> GetAll();
        void Add(Blog review);

        void AddPost(BlogPost review);
        Blog First();
        BlogPost GetPost(int id);
        List<BlogPost> GetByUserId(int id);

        List<Blog> GetBlogsByUserId(int id);

        List<BlogComment> GetComments(int postId);
        IEnumerable<BlogPost> GetRecent();
        void AddComment(BlogComment model);
        string DeletePost(int id);
        void Update(Blog model);

        IEnumerable<BlogPost> GetByKeyWords(string keywords);
    }
}