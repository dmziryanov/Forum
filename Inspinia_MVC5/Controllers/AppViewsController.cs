using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomRoutes.Models;
using DAL;
using WebMatrix.WebData;


namespace Inspinia_MVC5.Controllers
{
    public class AppViewsController : Controller
    {
        private IAdvRepository _advRepository;

        public AppViewsController(IAdvRepository advRepository)
        {
            _advRepository = advRepository;
        }

        public ActionResult Contacts()
        {
            return View();
        }
     
        public ActionResult Profile()
        {
            var v = new ProfileModel() { UserProfile = _advRepository.GetUser(WebSecurity.CurrentUserId) };

            return View(v);
        }

        public ActionResult Contacts2()
        {
            return View();
        }

        public ActionResult Profile2()
        {

            var v = new ProfileModel() { UserProfile = _advRepository.GetUser(WebSecurity.CurrentUserId)};
        
            
            return View(v);
        }
        
        public ActionResult Projects()
        {
            return View();
        }
       
        public ActionResult ProjectDetail()
        {
            return View();
        }
        
        public ActionResult FileManager()
        {
            return View();
        }
        
        public ActionResult Calendar()
        {
            return View();
        }
        
        public ActionResult FAQ()
        {
            return View();
        }
        
        public ActionResult Timeline()
        {
            return View();
        }
        
        public ActionResult PinBoard()
        {
            return View();
        }

        public ActionResult TeamsBoard()
        {
            return View();
        }

        public ActionResult SocialFeed()
        {
            return View();
        }

        public ActionResult Clients()
        {
            return View();
        }

        public ActionResult OutlookView()
        {
            return View();
        }

        public ActionResult IssueTracker()
        {
            return View();
        }

        public ActionResult Blog()
        {
            return View();
        }

        public ActionResult Article()
        {
            return View();
        }

        public ActionResult VoteList()
        {
            return View();
        }

	}
}