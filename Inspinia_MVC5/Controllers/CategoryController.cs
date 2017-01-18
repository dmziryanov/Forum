using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using DAL;
using WebMatrix.WebData;

namespace AdvSpareAuto.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private IAdvRepository _advRepository;
        private string countryCode;

        public CategoryController(ICategoryRepository categoryRepository, IAdvRepository advRepository)
        {
            _categoryRepository = categoryRepository;
            _advRepository = advRepository;
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.Any, Duration = 60)]
        public ActionResult ReserveGoogleAdvView()
        {
            return View();
        }

        [OutputCache(Location = System.Web.UI.OutputCacheLocation.Any, Duration = 60)]
        public ActionResult RightGoogleAdvView()
        {
            using (var ctx = new AdvContext())
            {
                var cnt = ctx.Database.SqlQuery<int>("select count(*) FROM clickIP where clickdate > {0} and IP = '" + HttpContext.Request.UserHostAddress + "'", DateTime.Now.AddDays(-1)).FirstOrDefault();
                return View(cnt);
            }
        }

       //[OutputCache(Location = System.Web.UI.OutputCacheLocation.Any, Duration = 60)]
        public ActionResult BottomGoogleAdvView()
        {
            using (var ctx = new AdvContext())
            {
                var cnt = ctx.Database.SqlQuery<int>("select count(*) FROM clickIP where clickdate > {0} and IP = '"+HttpContext.Request.UserHostAddress+"'", DateTime.Now.AddDays(-1)).FirstOrDefault();
                return View(cnt);
            }
        }

        public JsonResult Index()
        {
            return Json(_categoryRepository.GetAll(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ModelList(int id)
        {
            return View(BaseRepository._ModelPairs.Where(x => x.BrandId == id).ToArray());
        }

        [ValidateInput(false)]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.Any, VaryByParam = "category;location;keywords", Duration = 60)]
        public ActionResult Search(string keywords, string location, string category)
        {
            //В этом методе только заполняется модель и берется первое объявление, основной список заполняется асинхронно в  AdvController
            
            
              Thread t = new Thread(() =>
                {
                    if (string.IsNullOrEmpty(countryCode))
                        try
                        {



                            var httpClient = new WebClient();

                            countryCode =
                                System.Text.Encoding.UTF8.GetString(
                                    httpClient.DownloadData(("http://freegeoip.net/csv/" +
                                                             HttpContext.Request.ServerVariables["REMOTE_ADDR"])))
                                    .Split(',')[1];

                        }
                        catch (Exception)
                        {


                        }
                });
                
            _advRepository.SaveSearch(WebSecurity.CurrentUserId, keywords, location);
            var adv = _advRepository.Get(0);
            adv.KeyWords = keywords;
            if (!string.IsNullOrEmpty(location))
            {
                if (adv._locations.Where(x => x.Name.Contains(location)).FirstOrDefault() != null)
                    adv.LocationName = adv._locations.Where(x => x.Name.Contains(location)).FirstOrDefault().Name;
                else
                {
                    int i = 0;
                    if (int.TryParse(location, out i))
                    {
                        if (adv._locations.Where(x => x.CityId == i).FirstOrDefault() != null)
                            adv.LocationName =
                                adv._locations.Where(x => x.CityId == Convert.ToInt32(location)).FirstOrDefault().Name;
                    }
                    else
                    {
                        adv.LocationName = "Такого города нет базе данных, выберите другой город, ближайший к вам";
                    }
                }
            }

            int j = 0;
            if (int.TryParse(category, out j))
            {

                var catname = "";
                if (adv._subCategories.Where(x => x.ID == Convert.ToInt32(category)).FirstOrDefault() != null)
                {
                    adv.Category = adv._subCategories.Where(x => x.ID == Convert.ToInt32(category)).FirstOrDefault().ID;
                    catname = adv._subCategories.Where(x => x.ID == Convert.ToInt32(category)).FirstOrDefault().Name;
                }

                var cityname = "";
                int cityId = 0;
                if (int.TryParse(location, out cityId))
                {
                    if (adv._locations.Where(x => x.CityId == Convert.ToInt32(location)).FirstOrDefault() != null)
                    cityname = adv._locations.Where(x => x.CityId == Convert.ToInt32(location)).FirstOrDefault().Name;
                }

                //SEO
                List<int> cats= new List<int>();
                cats.Add(19);
                    var CatSeoPrefix = cats.Contains(j) ? "Найти работу " : "Купить ";
                if (j == 86)
                    CatSeoPrefix = "Разместить";
                
                ViewBag.Message = CatSeoPrefix + catname + "  " + cityname;
                
            }
            if (string.IsNullOrEmpty(ViewBag.Message))
                ViewBag.Message = "Найти товары и услуги";
            ViewBag.Keywords = "объявления,отзывы,блоги,создать блог, подать объявление, объявления о продаже,объявления по продаже,продажа авто бу";
            adv.CountryCode = !string.IsNullOrEmpty(countryCode) ? countryCode : "RU";
            return View(adv);
        }

        public ActionResult Advanced()
        {
            ViewBag.Keywords = "объявления,отзывы,блоги,создать блог, подать объявление, объявления о продаже,объявления по продаже,продажа авто бу";
            return View();
        }
    }
}
