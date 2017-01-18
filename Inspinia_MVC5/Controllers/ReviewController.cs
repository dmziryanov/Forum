using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;

namespace BootStrap.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public ActionResult PostReview()
        {
            var model = _reviewRepository.Get(0);
            return View("CreateReview", model);
        }


        public ActionResult CreateReview(Review model)
        {
            model.Created = DateTime.Now;
            _reviewRepository.Add(model);
            return View("Success");
        }

        public ActionResult All()
        {
            ViewBag.Message = "Отзывы, оставить отзыв";
            return View("All", _reviewRepository.GetAll());
        }

        public ActionResult MyReviews(int Id)
        {

            return View("MyReviews", _reviewRepository.GetByUserId(Id));
        }

        
    }

}
