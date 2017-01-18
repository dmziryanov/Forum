using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomRoutes.Models;
using DAL;
using WebMatrix.WebData;

namespace Inspinia_MVC5.Controllers
{
    public class DashboardsController : Controller
    {
        private IAdvRepository _advRepository;

        public DashboardsController(IAdvRepository advRepository)
        {
            _advRepository = advRepository;
        }

        public ActionResult Dashboard_1()
        {
            var v = new ProfileModel() { UserProfile = _advRepository.GetUser(WebSecurity.CurrentUserId) };
            return View(v);
        }

        public ActionResult Dashboard_2()
        {
            return View();
        }

        public ActionResult Dashboard_3()
        {
            return View();
        }
        
        public ActionResult Dashboard_4()
        {
            return View();
        }
        
        public ActionResult Dashboard_4_1()
        {
            return View();
        }

        public ActionResult Dashboard_5()
        {
            return View();
        }

    }
}