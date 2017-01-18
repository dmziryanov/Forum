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
    public class GraphsController : Controller
    {
        private IAdvRepository _advRepository;

        public GraphsController(IAdvRepository advRepository)
        {
            _advRepository = advRepository;
        }

        public ActionResult Flot()
        {
            if (WebSecurity.CurrentUserId < 0) return RedirectToAction("LogOn", "Account");
            var v = new ProfileModel() { UserProfile = _advRepository.GetUser(WebSecurity.CurrentUserId) };
            return View(v);
        }

        public ActionResult Morris()
        {
            if (WebSecurity.CurrentUserId < 0) return RedirectToAction("LogOn", "Account");
            var v = new ProfileModel() { UserProfile = _advRepository.GetUser(WebSecurity.CurrentUserId) };
            return View(v);
        }

        public ActionResult Rickshaw()
        {
            if (WebSecurity.CurrentUserId < 0) return RedirectToAction("LogOn", "Account");
            var v = new ProfileModel() { UserProfile = _advRepository.GetUser(WebSecurity.CurrentUserId) };
            return View(v);
        }

        public ActionResult Chartjs()
        {
            if (WebSecurity.CurrentUserId < 0) return RedirectToAction("LogOn", "Account");
            var v = new ProfileModel() { UserProfile = _advRepository.GetUser(WebSecurity.CurrentUserId) };
            return View(v);
        }
        public ActionResult Chartist()
        {
            var v = new ProfileModel() { UserProfile = _advRepository.GetUser(WebSecurity.CurrentUserId) };
            return View(v);
        }
        public ActionResult Peity()
        {
            var v = new ProfileModel() { UserProfile = _advRepository.GetUser(WebSecurity.CurrentUserId) };
            return View(v);
        }

        public ActionResult Sparkline()
        {
            var v = new ProfileModel() { UserProfile = _advRepository.GetUser(WebSecurity.CurrentUserId) };
            return View(v);
        }

        public ActionResult C3charts()
        {
            var v = new ProfileModel() { UserProfile = _advRepository.GetUser(WebSecurity.CurrentUserId) };
            return View(v);
        }
    }
}