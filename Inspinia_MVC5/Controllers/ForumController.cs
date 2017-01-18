using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootStrap.Models;
using DAL;
using Inspinia_MVC5.Infrastracture;
using WebGrease.Css.Extensions;
using WebMatrix.WebData;


namespace AdvSpareAuto.Controllers
{
    public class ForumController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IImageRepository _imageRepository;
        private IAdvRepository _advRepository;
        

        public ForumController(IBlogRepository blogRepository, IImageRepository imageRepository, IAdvRepository advRepository)
        {
            _blogRepository = blogRepository;
            _imageRepository = imageRepository;
            _advRepository = advRepository;
        }

        //
        // GET: /Blog/

        public ActionResult Index()
        {
            ViewBag.Message = "Блог, блоги, создать блог";
            return View(_blogRepository.GetAll().Where(x => x.IsDeleted == 0));
        }

        public ActionResult Create()
        {
            ViewBag.Message = "Создать тему для форума";
            var b = new BlogModel() { Blog = new Blog(), _categories = BaseRepository._categories};
            return View(b);
        }

        public ActionResult PostBlog(Blog model)
        {
            foreach (var fileKey in Request.Files.AllKeys)
            {
                var file = Request.Files[fileKey];
                try
                {
                    if (file != null && file.InputStream.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var b = new byte[file.InputStream.Length];
                        file.InputStream.Read(b, 0, (int)file.InputStream.Length);

                        model.ImageId = _imageRepository.Save((new ImageFile() { FileBody = b, FileName = file.FileName, FileSize = b.Length, Created = DateTime.Now}));
                        model.UserId = WebSecurity.CurrentUserId;
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { Message = "Error in saving file" });
                }
            }
            
            _blogRepository.Add(model);
            return View("Success", model);
        }

        [ValidateInput(false)]
        public ActionResult Post(BlogPost model)
        {
            int i = 0;
            foreach (var fileKey in Request.Files.AllKeys)
            
            {
                var file = Request.Files[fileKey];
                try
                {
                    if (file != null && file.InputStream.Length > 0 )
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var b = new byte[file.InputStream.Length];
                        file.InputStream.Read(b, 0, (int)file.InputStream.Length);
                        i++;
                        if (i == 1)
                        {   model.MainPhotoId =
                                _imageRepository.Save(
                                    (new ImageFile()
                                    {
                                        FileBody = b,
                                        FileName = file.FileName,
                                        FileSize = b.Length,
                                        Created = DateTime.Now
                                    }));


                        model.UserId = WebSecurity.CurrentUserId;
                        var userName = AdvRepository._users.FirstOrDefault(y => y.UserId == model.UserId).FirstName + " " + AdvRepository._users.FirstOrDefault(y => y.UserId == model.UserId).LastName;
                        model.UserName = userName;
                        model.SeoTopic = Transliteration.Front(model.Topic);

                            _blogRepository.AddPost(model);

                        }
                        else
                        {
                            var photoId =_imageRepository.Save(
                                (new ImageFile()
                                {
                                    FileBody = b,
                                    FileName = file.FileName,
                                    FileSize = b.Length,
                                    Created = DateTime.Now
                                }));
                            if (file.ContentType.StartsWith("image"))
                                _advRepository.SaveBlogImage(model.Id, photoId);
                            else
                                _advRepository.SaveBlogFile(model.Id, photoId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { Message = "Error in saving file: " + ex.Message });
                }
            }

            if (i == 0) return Json(new { Message = "Не задана картинка изображения" });

            return RedirectToAction("Topic","Forum", new { Id= model.BlogId});
        }

        public ActionResult Topic(string id)
        {

            int sid = Convert.ToInt32(id.Split('-')[0]);
            var blog = _blogRepository.Get(sid);
            
            var m = new BlogModel() {Blog = blog };
            
            m._categories = AdvRepository._categories;
            m.Recent = _blogRepository.GetRecent();
            var userName = AdvRepository._users.FirstOrDefault(y => y.UserId == blog.UserId).FirstName + " " + AdvRepository._users.FirstOrDefault(y => y.UserId == blog.UserId).LastName;
            m.Recent.ForEach(x =>
            {
                
                x.UserName 
                                          = userName;
            });
            blog.Posts.ForEach(x => { x.UserName = userName;
                                        x.CommentsCount = _blogRepository.GetComments(x.Id).Count();
            } );
            ViewBag.Message = "Блог " + blog.Name;
            return View(m);
        }

        public ActionResult Search(string id)
        {



            var m = new BlogCategoryModel();

            m.Posts = _blogRepository.GetByKeyWords(id);
            m.categories = AdvRepository._categories;
            m.Recent = _blogRepository.GetRecent();
            m.KeyWords = id;


            m.Posts.ForEach(x =>
            {
                var userName = AdvRepository._users.FirstOrDefault(y => y.UserId == x.UserId).FirstName + " " + AdvRepository._users.FirstOrDefault(y => y.UserId == x.UserId).LastName;
                x.UserName = userName;
                x.CommentsCount = _blogRepository.GetComments(x.Id).Count();
            });

            ViewBag.Message = "Themes by keywords " + id;
            return View(m);
        }

        public ActionResult MyBlog(int id)
        {

            var blogs = _blogRepository.GetBlogsByUserId(id);

            ViewBag.Message = "Администрирование разделов сайта";
            return View("MyBlog", blogs);
        }


        public ActionResult MyThemes(int id)
        {

            var blogs = _blogRepository.GetBlogsByUserId(id);

            ViewBag.Message = "Мои Блоги";
            return View("MyBlog", blogs);
        }

        public ActionResult BlogDelete(int id)
        {
            
            var post = _blogRepository.GetPost(id); //To do get blog by post id

            
            var blog = _blogRepository.Get(post.BlogId);
            if (blog.UserId == WebSecurity.CurrentUserId)       
            {
                var blogs = _blogRepository.DeletePost(id);
            }
            
            blog.UserId = WebSecurity.CurrentUserId;
            var m = new BlogModel() { Blog = blog };
            m._categories = AdvRepository._categories;
            m.Recent = _blogRepository.GetRecent();
            m.Recent.ForEach(x => x.UserName
                = AdvRepository._users.FirstOrDefault(y => y.UserId == x.UserId).FirstName + " " +
                  AdvRepository._users.FirstOrDefault(y => y.UserId == x.UserId).LastName);
            ViewBag.Message = "Блог " + blog.Name;
            return View("Topic", m);
        }
        
        public ActionResult CreatePost(int id)
        {
            ViewBag.Message = "Создать тему";
            return View(new BlogPost() { BlogId = id});
        }

        public ActionResult PostComment(int PostId, string PostComment)
        {
            var model = new BlogComment()
            {
                CreatedDate = DateTime.Now,
                PostId = PostId,
                ParentId = 0,
                PostComment = PostComment,
                UserId = WebSecurity.CurrentUserId
            };

            _blogRepository.AddComment(model);
            var post = _blogRepository.GetPost(PostId); //To do get blog by post id
            var _blog = _blogRepository.Get(post.BlogId);
            var userName = AdvRepository._users.FirstOrDefault(y => y.UserId == _blog.UserId).FirstName + " "+
                           AdvRepository._users.FirstOrDefault(y => y.UserId == _blog.UserId).LastName;
            post.UserName = userName;
            var BlogComments = _blogRepository.GetComments(model.PostId);
            var BlogCommentModels =
                BlogComments.Select(x => new BlogCommentModel() { Comment = x, UserAvatarId = AdvRepository._users.FirstOrDefault(y => y.UserId == _blog.UserId).UserAvatarId, UserName = userName }).ToList();
            var blogPostModel = new BlogPostModel() { Post = post, BlogCommentModels = BlogCommentModels, Blog = _blog };
            blogPostModel.Post.ImgIds = _advRepository.GetPhotoIds(blogPostModel.Post.Id);
            blogPostModel.Post.FileIds = _advRepository.GetFileIds(blogPostModel.Post.Id);
            blogPostModel.Post.FileNames = _advRepository.GetFileNames(blogPostModel.Post.Id);
            blogPostModel.Recent = _blogRepository.GetRecent();
            blogPostModel._categories = AdvRepository._categories;
            blogPostModel.Post.ImgIds = (new int[] { blogPostModel.Post.MainPhotoId }).Concat(blogPostModel.Post.ImgIds).ToArray();
            ViewBag.Message = blogPostModel.Blog.Name + " " + blogPostModel.Post.Topic;
            return View("BlogPost", blogPostModel);

        }


        public ActionResult BlogPost(int id)
        {
            var post = _blogRepository.GetPost(id); //To do get blog by post id
            var _blog = _blogRepository.Get(post.BlogId);
            var userName = AdvRepository._users.FirstOrDefault(y => y.UserId == _blog.UserId).FirstName + " " +
                        AdvRepository._users.FirstOrDefault(y => y.UserId == _blog.UserId).LastName;
            post.UserName = userName;
            var BlogComments = _blogRepository.GetComments(id);
            var _blogCommentModels =
                BlogComments.Select(x =>
                {
                    var xuserName = AdvRepository._users.FirstOrDefault(y => y.UserId == x.UserId).FirstName + " " +
                        AdvRepository._users.FirstOrDefault(y => y.UserId == x.UserId).LastName;
                    return new BlogCommentModel()
                    {
                        Comment = x,
                        UserAvatarId = AdvRepository._users.FirstOrDefault(y => y.UserId == x.UserId).UserAvatarId,
                        UserName = xuserName
                    };
                }).ToList();
            var model = new BlogPostModel() { Post = post, BlogCommentModels = _blogCommentModels, Blog = _blog };
            model.Recent = _blogRepository.GetRecent();
            model._categories = AdvRepository._categories;
            model.Post.ImgIds = _advRepository.GetPhotoIds(model.Post.Id);
            model.Post.FileIds = _advRepository.GetFileIds(model.Post.Id);
            model.Post.FileNames = _advRepository.GetFileNames(model.Post.Id);
            model.Post.ImgIds =(new int[] { model.Post.MainPhotoId}).Concat(model.Post.ImgIds).ToArray();
            ViewBag.Message = model.Blog.Name + " " + model.Post.Topic;

            return View(model);
        }

        public ActionResult UserBlog(int id)
        {
            return View("BlogDetails", new BlogModel() { UserPosts = _blogRepository.GetByUserId(id) });
        }

        public ActionResult EditBlog(int id)
        {
            var _blog = _blogRepository.Get(id);
            return View(_blog);
        }

        public ActionResult PostEditBlog(Blog model)
        {
            foreach (var fileKey in Request.Files.AllKeys)
            {
                var file = Request.Files[fileKey];
                try
                {
                    if (file != null && file.InputStream.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var b = new byte[file.InputStream.Length];
                        file.InputStream.Read(b, 0, (int)file.InputStream.Length);

                        model.ImageId = _imageRepository.Save((new ImageFile() { FileBody = b, FileName = file.FileName, FileSize = b.Length, Created = DateTime.Now }));
                        model.UserId = WebSecurity.CurrentUserId;
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { Message = "Error in saving file" });
                }
            }

            _blogRepository.Update(model);
            return View("Success", model);
        }

        public void WholeBlogDelete(int id)
        {
            var _blog = _blogRepository.Get(id);
            _blog.IsDeleted = 1;
            _blogRepository.Update(_blog);
            
        }

        public void WholeBlogRestore(int id)
        {
            var _blog = _blogRepository.Get(id);
            _blog.IsDeleted = 0;
            _blogRepository.Update(_blog);
        }
    }
}
