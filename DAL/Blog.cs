using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }

        public int ImageId { get; set; }

        public int CategoryId { get; set; }
        public List<BlogPost> Posts { get; set; }
        public int IsDeleted { get; set; }
    }

    public class BlogPost
    {
        [Key]
        public int Id { get; set; }

        public int MainPhotoId { get; set; }

        public int[] PhotoIds { get; set; }
        public int BlogId { get; set; }

        public int CommentsCount;

        public DateTime LastEditDate { get; set; }
        public string Body { get; set; }
        public string Topic { get; set; }

        public string SeoTopic { get; set; }

        public int UserId { get; set; }
        public string UserName;
        public int[] ImgIds { get; set; }
        public int[] FileIds { get; set; }

        public string[] FileNames { get; set; }
    }

    public class BlogPostModel : BlogModel
    {
        public BlogPost Post { get; set; }

        public IEnumerable<BlogPost> Recent { get; set; }

        public IEnumerable<BlogPost> UserPosts { get; set; }

        public List<BlogCommentModel> BlogCommentModels { get; set; }
        public int CommentCount { get; set; }
        public string UserName { get; set; }
        public Blog Blog { get; set; }
    }

    public class BlogCommentModel
    {
        public BlogComment Comment { get; set; }

        public int? UserAvatarId { get; set; }

        public string UserName { get; set; }
    }

    public class BlogComment
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public int PostId { get; set; }

        public int UserId { get; set; }

        public string Text { get; set; }

        public string PostComment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}