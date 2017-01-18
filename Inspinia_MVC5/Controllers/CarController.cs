using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;


namespace AdvSpareAuto.Controllers
{
    public class CarController : Controller
    {
        public static List<KeyNamePair> mfg;
        public static Dictionary<int, List<KeyNameModel>> m = new Dictionary<int, List<KeyNameModel>>();
        
        //
        // GET: /Car/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Brand(int? id)
        {
            if (mfg == null)
                mfg = AdvRepository._BrandsPairs.ToList();
                
            return View(mfg);
        }

        public ActionResult Model(int id)
        {
            List<KeyNameModel> n = new List<KeyNameModel>();
            if (m.TryGetValue(id, out n))
                return View(n);
            else
            {
                m.Add(id, AdvRepository._ModelPairs.Where(x => x.BrandId == id).ToList());
                m.TryGetValue(id, out n);
                return View(n);
            }
        }
        public ActionResult CarType(int id)
        {
        /*    List<CarType> n = new List<CarType>();
            if (ct.TryGetValue(id, out n))
                return View(n);
            else
            {
                ct.Add(id, Facade.ListModifications(id));
                ct.TryGetValue(id, out n);
                return View(n);*/
            return null;
        }

        public ActionResult CarDescription(int id)
        {
            /*var n = Facade.GetModification(id, true, new List<int>());
            return View(n);*/
            return null;
        }
    }
}
