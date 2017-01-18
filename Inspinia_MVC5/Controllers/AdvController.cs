using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using BootStrap.Models;
using DAL;

using WebMatrix.WebData;
using CacheItemPriority = System.Web.Caching.CacheItemPriority;

namespace AdvSpareAuto.Controllers
{
    [XmlRoot("auto-catalog")]
    public class auto_catalog
    {
        [XmlElement("creation-date")]
        public string creation_date;

        [XmlElement("host")]
        public string host;


        public object offers { get; set; }
    }


    public class ParameterBinder : IModelBinder
    {
        public string ActualParameter { get; private set; }

        public ParameterBinder(string actualParameter)
        {
            this.ActualParameter = actualParameter;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object id = controllerContext.RouteData.Values[this.ActualParameter];
            return id;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class BindParameterAttribute : CustomModelBinderAttribute
    {
        public string ActualParameter { get; private set; }

        public BindParameterAttribute(string actualParameter)
        {
            this.ActualParameter = actualParameter;
        }

        public override IModelBinder GetBinder()
        {
            return new ParameterBinder(this.ActualParameter);
        }
    }

    public class AdvController : Controller
    {

        private IAdvRepository _advRepository;
        private int pageSize = 12;
        private int _carCategory = 49;
        private int _maxPageCount = 15;

        public AdvController(IAdvRepository advRepository)
        {
            _advRepository = advRepository;
        }

        /*[HttpGet]
        public ActionResult PostAdv()
        {
            ViewBag.Message = "разместить объявление о продаже; подать объявление; Москва, Санкт-Петербург, Екатеринбург, Краснодар, Новосибирск";
            var model = _advRepository.Get(0);
            model.CurrentUser = _advRepository.GetUser(WebSecurity.CurrentUserId);
            if (model.CurrentUser == null)
                model.CurrentUser = new RegisterModel();
            return View(model);
        }*/

        public void SaveClickIP()
        {
            using (var ctx = new AdvContext())
            {
                ctx.Database.ExecuteSqlCommand("Insert into ClickIP (IP) values ('"+Request.UserHostAddress+"')");
                
            }
        }

        public ActionResult CarAdvTips()
        {
            ViewBag.Message = "разместить объявление о продаже; подать объявление; Москва, Санкт-Петербург, Екатеринбург, Краснодар, Новосибирск";
            

            return View();
        }



        public ActionResult PostResume()
        {
            ViewBag.Message = "разместить объявление о продаже; подать объявление; Москва, Санкт-Петербург, Екатеринбург, Краснодар, Новосибирск";
            var model = _advRepository.Get(0);
            model.CurrentUser = _advRepository.GetUser(WebSecurity.CurrentUserId);
            



            if (model.CurrentUser == null)
                model.CurrentUser = new UserProfile();
            return View(model);
        }

        [HttpGet]
        public ActionResult PostCarAdv()
        {
            ViewBag.Message = "разместить объявление о продаже; подать объявление; Москва, Санкт-Петербург, Екатеринбург, Краснодар, Новосибирск";
            var model = _advRepository.Get(0);
            model.CurrentUser = _advRepository.GetUser(WebSecurity.CurrentUserId);
            

            if (model.CurrentUser == null)
                model.CurrentUser = new UserProfile();
            return View(model);
        }

        [HttpPost]
        public ActionResult PostAdv(AdvModel model)
        {
            return RedirectToAction("CreateAdv");
        }

        public ActionResult Help(AdvModel model)
        {
            return View();
        }

        public ActionResult Delete(int Id)
        {
            var model = _advRepository.Get(Id);
            if (model.SellerId == WebSecurity.CurrentUserId)
            {
                _advRepository.Delete(Id);
                return View("Delete");
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Edit(int Id)
        {
            var model = _advRepository.Get(Id);
            model.CurrentUser = _advRepository.GetUser(WebSecurity.CurrentUserId);
            if (model.SellerId == WebSecurity.CurrentUserId)
            {
                return View(model);
            }
            else
            {
                return View("Error");
            }
        }


        [HttpPost]
        public ActionResult CreateAdv(CarAdvModel model)
        {
            if (model.Category == 0) return View("Error", new ErrorInfo() { message = "Заполните категорию" });

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

                        model.Imgs.Add(new ImageFile() { FileBody = b, FileName = file.FileName, FileSize = b.Length, Created = DateTime.Now });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { Message = "Error in saving file" });
                }
            }
            _advRepository.Save(model);

            return View("Success");
        }


        [HttpPost]
        public void Report(int page, string couse, string text)
        {
            using (var a = new AdvContext())
            {
                a.Database.ExecuteSqlCommand("insert into ReportAbuse (AdvId, couse, text) values ({0}, {1}, {2})", page, couse, text);
            }
        }


        [HttpPost]
        public void SiteMessage(int page, int userId, string phone, string mail, string text)
        {
            using (var a = new AdvContext())
            {
                a.Database.ExecuteSqlCommand("insert into Sitemessage (AdvId,FromId,  FromPhone, FromEmail, Text) values ({0}, {1}, {2}, {3}, {4})", page, userId, phone, mail, text);
            }
        }



        [OutputCache(Location = System.Web.UI.OutputCacheLocation.Any, Duration = 60)]
        public ActionResult AdvDetails([BindParameter("id")] string AdvNo)
        {
            var adv = _advRepository.Get(Convert.ToInt32(AdvNo)) ?? new AdvModel();
            var l = (List<AdvModel>)Session["LastSearch"];

            if (l != null)
            {
                if (l.Any(x => x.Id == adv.Id))
                {
                    adv._next = l.Where(x => x.Id == adv.Id).FirstOrDefault()._next;
                    adv._previous = l.Where(x => x.Id == adv.Id).FirstOrDefault()._previous;
                    if (adv._next > 0)
                        adv.next = _advRepository.Get(adv._next);
                    if (adv._previous > 0)
                        adv.previous = _advRepository.Get(adv._previous);
                }
            }

            if (adv.SubCategory == _carCategory)
            {
                var tmp = _advRepository.GetCar(Convert.ToInt32(AdvNo));

                
                

                if (tmp != null)
                {
                    if (AdvRepository._BrandsPairs.FirstOrDefault(x => x.Id == tmp.Brand) != null && AdvRepository._ModelPairs.FirstOrDefault(x => x.Id == tmp.Model) != null)
                        tmp.strName = (AdvRepository._BrandsPairs.FirstOrDefault(x => x.Id == tmp.Brand).Name) + " " + (AdvRepository._ModelPairs.FirstOrDefault(x => x.Id == tmp.Model).Name);

                    tmp._featuredList = adv._featuredList;
                    tmp._simalarList = adv._simalarList;
                    adv = tmp;
                }
            }



            adv.CurrentUser = _advRepository.GetUser(adv.SellerId.HasValue ? adv.SellerId.Value : 0);
            _advRepository.IncreaseViewCount(Convert.ToInt32(AdvNo), adv.ViewCount);

            //SEO
            List<int> cats = new List<int>();
            cats.Add(19);
            var CatSeoPrefix = cats.Contains(adv.SubCategory) ? "Найти работу " : "Купить ";
            ViewBag.Message = string.Format("{2} {0} в {1}", adv.Name + " " + adv.condition.GetDescription(), adv.LocationName, CatSeoPrefix);
            ViewBag.Keywords = string.Format("{2} {0} в {1}", adv.Name + " " + adv.condition.GetDescription(), adv.LocationName, CatSeoPrefix);
            ViewBag.Description = adv.Description;

            return View(adv);
        }

        public sealed class StringWriterWithEncoding : StringWriter
        {
            private readonly Encoding encoding;

            public StringWriterWithEncoding(Encoding encoding)
            {
                this.encoding = encoding;
            }

            public override Encoding Encoding
            {
                get { return encoding; }
            }
        }


     

        public ActionResult MyAdv()
        {
            List<AdvModel> adv = _advRepository.GetByUserId(WebSecurity.CurrentUserId);
            ViewBag.Message = "Мои объявления";
            return View(adv);
        }

        private void GetBooksRequestParams(out SortBy sortBy, out string keywords, out int currentPage)
        {
            sortBy = SortBy.None;

            if (Request.QueryString["SortBy"] == "По возрастанию")
            {
                sortBy = SortBy.PriceAsc;
            }

            if (Request.QueryString["SortBy"] == "По убыванию")
            {
                sortBy = SortBy.PriceDesc;
            }

            keywords = Request.QueryString["keywords"];

            if (!(int.TryParse(Request.QueryString["currentPage"], out currentPage)))
                currentPage = 1;
        }

        [HttpGet, ActionName("GetData")]
        public JsonResult GetData(String sortBy, string keywords, int currentPage, int location, string country, string category, string sminPrice, string smaxPrice, string type, int condition)
        {

            // GetBooksRequestParams(out SortBy sortBy, out string keywords, out int currentPage);

            var res = (List<AdvModel>)HttpRuntime.Cache.Get(sortBy + keywords + location + country + category + sminPrice + smaxPrice + type + condition + "&&" + currentPage + "&&" + pageSize);
            if (res != null)
                return Json(res.ToList(), JsonRequestBehavior.AllowGet);
            else
                return Json(new List<AdvModel>(), JsonRequestBehavior.AllowGet);
        }

        public void Share(int page)
        {

            // GetBooksRequestParams(out SortBy sortBy, out string keywords, out int currentPage);

            using (var a = new AdvContext())
            {
                a.Database.ExecuteSqlCommand("insert into SocialAdvShare (AdvId) values ({0})", page);
            }

        }

        [HttpGet, ActionName("GetPagesData")]
        public JsonResult GetPagesData(String sortBy, string keywords, int currentPage, int location, string country, string category, string sminPrice, string smaxPrice, string type, int condition)
        {
            AdvModel adv;
            adv = _advRepository.Get(Convert.ToInt32(0));
            AdvType atype = 0;
            if (type == "allAds")
                atype = AdvType.All;

            if (type == "businessAds")
                atype = AdvType.Business;

            if (type == "personalAds")
                atype = AdvType.Private;

            string OrderExpr = " ";
            if (sortBy.Contains("от большей к меньшей"))
                OrderExpr = "Order by Price Desc";

            if (sortBy.Contains("от меньшей к большей"))
                OrderExpr = "Order by Price asc";

            if (sortBy.Contains("сначала новые"))
                OrderExpr = "Order by created desc";

            if (sortBy.Contains("сначала старые"))
                OrderExpr = "Order by created asc";

            List<AdvModel> result = new List<AdvModel>();

            double minPrice = 0;
            double maxPrice = 0;

            double.TryParse(sminPrice, out minPrice);
            double.TryParse(smaxPrice, out maxPrice);


            int i = 0;
            if (int.TryParse(category, out i) && i > 0)
            {
                result =
                    _advRepository.GetFilteredSortedPageResult(atype, (AdvCondition)condition, i, keywords, country, location, pageSize, currentPage, OrderExpr, minPrice, maxPrice).ToList();
            }
            else
            {
                result =
                    _advRepository.GetFilteredSortedPageResult(atype, (AdvCondition)condition, keywords, country, location, pageSize, currentPage, OrderExpr, minPrice, maxPrice).ToList();

                if (result.Count() == 0)
                {
                    List<int> cat_id = null;
                    if (!string.IsNullOrEmpty(keywords))
                        //Если ничего не нашли в заголовке то ищем по категориям
                        cat_id = adv._subCategories.Where(x => x.Name.ToUpper().Contains(keywords.ToUpper())).Select(x => x.ID).ToList(); // Засплитить и поискать по всем словам



                    result = _advRepository.GetFilteredSortedPageResult(atype, (AdvCondition)condition, cat_id, country, location, pageSize, currentPage, OrderExpr, minPrice, maxPrice).ToList();

                }
            }

            //Очень важно, этот запрос выполняется для всех!
            Array.ForEach(result.ToArray(), model =>
            {
                using (var _advContext = new AdvContext())
                {
                    model.ImgIds =
                        _advContext.Database.SqlQuery<int>("select PhotoId from dbo.advPhoto where AdvId ={0}",
                            model.Id)
                            .ToArray();
                }

                model.LocationName = adv._locations.FirstOrDefault(x => x.CityId == model.Location).Name;
                model.CategoryName = adv._subCategories.FirstOrDefault(x => x.ID == model.Category).Name;
            });

            List<AdvModel> tmp = new List<AdvModel>();
            tmp = tmp.Concat(result.ToList()).ToList();
            for (var k = 0; k < result.Count() - 1; k++)
            {
                if (k > 0)
                    tmp[k]._previous = tmp[k - 1].Id;

                tmp[k]._next = tmp[k + 1].Id;
            }

            Session["LastSearch"] = tmp;
            HttpRuntime.Cache.Add(sortBy + keywords + location + country + category + sminPrice + smaxPrice + type + condition + "&&" + currentPage + "&&" + pageSize, result, null, DateTime.Now + TimeSpan.FromMinutes(1), TimeSpan.Zero, CacheItemPriority.High, null);

            var count = result.Count > 0 ? result.FirstOrDefault().CurrentSearchPageCount : 0;

            var pagecount = (int)(count % pageSize == 0 ? count / pageSize : count / pageSize + 1);

            //Limit max pages
            pagecount = pagecount < _maxPageCount ? pagecount : _maxPageCount;
            return Json(Enumerable.Range(0, pagecount).ToList().Select(x => new { pagenum = x + 1 }), JsonRequestBehavior.AllowGet);
        }

        public ViewResult Favorite()
        {
            HashSet<int> list = _advRepository.GetFavorite(Request.ServerVariables["REMOTE_ADDR"]);
            if (list != null)
            {
                var result = _advRepository.Get(list.Distinct().ToArray());
                Array.ForEach(result.ToArray(), model =>
    {
        using (var _advContext = new AdvContext())
        {
            model.ImgIds =
                _advContext.Database.SqlQuery<int>("select PhotoId from dbo.advPhoto where AdvId ={0}",
                    model.Id)
                    .ToArray();
        }

        model.LocationName = AdvRepository._locations.FirstOrDefault(x => x.CityId == model.Location).Name;
        model.CategoryName = AdvRepository._subCategories.FirstOrDefault(x => x.ID == model.Category).Name;
    });
                return View(result);
            }
            else
            {
                return View("Error", new ErrorInfo() { message = (Session["favorites"] == null).ToString() });
            }

        }

        public JsonResult GetSavedAdv()
        {
            var list = _advRepository.GetFavorite(Request.ServerVariables["REMOTE_ADDR"]);
            if (list != null)
            {
                return Json(list.Select(x => x).Distinct(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(Enumerable.Range(0, 0).ToList().Select(x => new { pagenum = x + 1 }), JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult hideChosenWarn()
        {
            var list = _advRepository.GetChosenAdded(Request.ServerVariables["REMOTE_ADDR"]);
            if (list != null)
            {
                return Json(list.Select(x => x).Distinct(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(Enumerable.Range(0, 0).ToList().Select(x => new { pagenum = x + 1 }), JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult checkChosenWarn()
        {
            var list = _advRepository.CheckChosenAdded(Request.ServerVariables["REMOTE_ADDR"]);
            if (list != null)
            {
                return Json(list.Select(x => x).Distinct(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(Enumerable.Range(0, 0).ToList().Select(x => new { pagenum = x + 1 }), JsonRequestBehavior.AllowGet);
            }

        }

        public bool SaveAdv(int id)
        {
            var list = _advRepository.GetFavorite(Request.ServerVariables["REMOTE_ADDR"]);
            if (list != null)
            {
                if (list.Contains(id))
                {
                    _advRepository.Remove(Request.ServerVariables["REMOTE_ADDR"], id);
                    if (WebSecurity.CurrentUserId > -1)
                    {
                        //ToDO: remove из базы
                    }
                    return false;
                }
                else
                {
                    _advRepository.AddFavorite(Request.ServerVariables["REMOTE_ADDR"], id);
                    if (WebSecurity.CurrentUserId > -1)
                    {
                        //ToDO: сделать сохранение в базу
                    }
                    return true;
                }
            }
            else
            {

                _advRepository.AddFavorite(Request.ServerVariables["REMOTE_ADDR"], id);


                if (WebSecurity.CurrentUserId > -1)
                {
                    //ToDO: сделать сохранение в базу
                }

                return true;
            }
        }

        [HttpGet, HttpPost, ActionName("AddToFavotites")]
        public ActionResult AddToFavotites(int id)
        {

            return View();
        }

        [ValidateInput(false)]
        public ActionResult CreateCarAdv(CarAdvModel model)
        {
            model.Category = _carCategory;

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

                        model.Imgs.Add(new ImageFile() { FileBody = b, FileName = file.FileName, FileSize = b.Length, Created = DateTime.Now });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { Message = "Error in saving file" });
                }
            }
            var mfg = AdvRepository._BrandsPairs.FirstOrDefault(x => x.Id == model.Brand).Name;
            var i = AdvRepository._ModelPairs.FirstOrDefault(x => x.Id == model.CarModel).Name;
            
            model.Name = mfg + " " + i;

            _advRepository.SaveCar(model);

            return View("Success");
        }
    }


}
