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
    public class TablesController : Controller
    {
        private IAdvRepository _advRepository;

        public TablesController(IAdvRepository advRepository)
        {
            _advRepository = advRepository;
        }

        public ActionResult StaticTables()
        {
            return View();
        }

        public ActionResult DataTables()
        {
            return View();
        }

        public ActionResult FooTables()
        {
            return View();
        }

        public ActionResult jqGrid()
        {
            var v = new ProfileModel() { UserProfile = _advRepository.GetUser(WebSecurity.CurrentUserId) };
            return View(v);
        }
	}
}