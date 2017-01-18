using System;
using System.Collections.Generic;
using System.Linq;
using WebMatrix.WebData;
using EntityState = System.Data.Entity.EntityState;

namespace DAL
{
    class BlogRepository : BaseRepository, IBlogRepository
    {
        private AdvContext _advContext;

        public BlogRepository()
        {
            
        }

        public Blog Get(int id)
        {
            using (_advContext = new AdvContext())
            {
                var res = _advContext.Database.SqlQuery<Blog>("Select * From dbo.Blog where Id = {0}", id).FirstOrDefault();
                res.Posts = _advContext.Database.SqlQuery<BlogPost>("Select a.*, b.FirstName + ' ' + b.LastName as UserName From dbo.BlogPost a, UserProfile b where a.UserId = b.UserId and BlogId = {0} order by LastEditDate desc", id).ToList();
                return res;
            }
        }

        public List<Blog> GetAll()
        {
            using (_advContext = new AdvContext())
            {
               return _advContext.Database.SqlQuery<Blog>("Select * From dbo.Blog").ToList();
            }
        }

        public void Add(Blog model)
        {
            using (_advContext = new AdvContext())
            {
                model.UserId = WebSecurity.CurrentUserId;
                _advContext.Blogs.Add(model);
                _advContext.SaveChanges();
            }
        }

        public void AddPost(BlogPost model)
        {
            using (_advContext = new AdvContext())
            {
                model.LastEditDate = DateTime.Now;
                _advContext.Posts.Add(model);
                _advContext.SaveChanges();
            }
        }


        public Blog First()
        {
            using (_advContext = new AdvContext())
            {
                var res = _advContext.Database.SqlQuery<Blog>("Select * From dbo.Blog").FirstOrDefault();
                return res;
            }
        }

        public BlogPost GetPost(int id)
        {
            using (_advContext = new AdvContext())
            {
                return _advContext.Database.SqlQuery<BlogPost>(@"Select a.*,  b.FirstName + ' ' + b.LastName as UserName From dbo.BlogPost a, UserProfile b where a.UserId = b.UserId and Id = {0}", id).FirstOrDefault();
            }
        }

        public List<BlogPost> GetByUserId(int id)
        {
            using (_advContext = new AdvContext())
            {
                return _advContext.Database.SqlQuery<BlogPost>(@"Select a.*,  b.FirstName + ' ' + b.LastName as UserName From dbo.BlogPost a, UserProfile b where a.UserId = b.UserId and UserId = {0}", id).ToList();
            }
        }

        public List<Blog> GetBlogsByUserId(int id)
        {
            using (_advContext = new AdvContext())
            {
                return _advContext.Database.SqlQuery<Blog>("Select * From dbo.Blog where UserId = {0}", id).ToList();
            }
        }

        public List<BlogComment> GetComments(int postId)
        {
            using (_advContext = new AdvContext())
            {
                return _advContext.Database.SqlQuery<BlogComment>("Select * From dbo.BlogComment where postId = {0}", postId).ToList();
            }
        }

        public IEnumerable<BlogPost> GetRecent()
        {
            using (_advContext = new AdvContext())
            {
                return _advContext.Database.SqlQuery<BlogPost>("Select TOP 10 a.[Id], [Topic], [Body], [LastEditDate], SeoTopic, [MainPhotoId], [BlogId], c.FirstName + ' ' + c.LastName as UserName, b.UserId From dbo.BlogPost a, dbo.blog b, dbo.UserProfile c where c.Userid = b.UserId and a.BlogId = b.Id and b.IsDeleted = 0 order by LastEditDate desc").ToList();
            }
        }

        public void AddComment(BlogComment model)
        {
            using (_advContext = new AdvContext())
            {
                _advContext.Comments.Add(model);
                _advContext.SaveChanges();
            }
        }

        public string DeletePost(int id)
        {
            using (_advContext = new AdvContext())
            {
                _advContext.Database.ExecuteSqlCommand("delete From dbo.BlogPost where Id = {0}", id);
                return "";
            }
        }

        public void Update(Blog model)
        {
            using (_advContext = new AdvContext())
            {
                model.UserId = WebSecurity.CurrentUserId;
                _advContext.Blogs.Attach(model);
                _advContext.Entry(model).State = EntityState.Modified; 
                
                _advContext.SaveChanges();
            }
        }

        public IEnumerable<BlogPost> GetByKeyWords(string keywords)
        {
            using (_advContext = new AdvContext())
            {
                return _advContext.Database.SqlQuery<BlogPost>("Select a.*, b.FirstName + ' ' + b.LastName as UserName From dbo.BlogPost a, UserProfile b, blog c where c.Id = a.BlogId and a.UserId = b.UserId and a.Topic like '%" + keywords + "%' order by LastEditDate desc").ToList();
            }
        }
    }
}