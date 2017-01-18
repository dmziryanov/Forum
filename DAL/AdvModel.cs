using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web;


namespace DAL
{
    public class CarAdvModel : AdvModel
    {
        public  IEnumerable<KeyNamePair> _BrandsPairs;
        public  IEnumerable<KeyNameModel> _ModelPairs;
        public int MileAge { get; set; }
        public int customs { get; set; }
        public bool guarantee { get; set; }
        public string PTS { get; set; }
        public string VIN { get; set; }
        public int Year { get; set; }
        public int Brand { get; set; }
        public int Model { get; set; }
        public int CarModel { get; set; }
        public int CarType { get; set; }
    }

    public class AdvModel : BaseModel
    {

        public AdvModel()
        {
            this.Imgs = new List<ImageFile>();
        }

        public long CurrentSearchPageCount { get; set; }
        public UserProfile CurrentUser;
        public int _next;
        public int _previous;
        public AdvModel next;
        public AdvModel previous;
        public AdvModel[] _featuredList;
        
        public KeyNamePair[] _Brands;
        public KeyNameModel[] _Model;

        public string topAdsVisible
        {
            get { return IsFeatured == 3 ? "visibility:visible" : "visibility:hidden"; }
        }

        public string urgentAdsVisible
        {
            get { return IsFeatured == 2 ? "visibility:visible" : "visibility:hidden"; }
        }

        public string featuredAdsVisible
        {
            get { return IsFeatured == 4 ? "visibility:visible" : "visibility:hidden"; }
        }

        public string SaveAdsUrl
        {
            get
            {
                return

                    "return refreshedSaved(this, " + Id + ");";
        }
        }

        public string SavedText
        {

            get
            {
                var list = (HashSet<int>)HttpContext.Current.Session["favorites"];
                if (list != null)
                {
                    if (list.Contains(Id))
                        return "Удалить сохранение";
                    else
                        return "Сохранить";
                }
                else
                    return "Сохранить";
            }
        }

        public string SavedAdsClass
        {
            get
            {
                var list = (HashSet<int>)HttpContext.Current.Session["favorites"];
                if (list != null)
                {
                    if (list.Contains(Id))
                        return "btn btn-danger btn-sm make-favorite";
                    else
                        return "btn btn-default btn-sm make-favorite";
                }
                else
                    return "btn btn-default btn-sm make-favorite";
            }
        }
        
        public IEnumerable<CategoryCount> Popular;
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastColumn = "last-column";
        public IEnumerable<AdvModel> _simalarList;
        public string strName;

        public string Description { get; set; }
        public string Currency { get; set; }
        public string AdvTypeShortName
        {
            get { return Convert.ToString(type.GetDescription()[0]); }
        }

        public string AdvTypeFullName
        {
            get { return type.GetDescription(); }
        }

        public int[] ImgIds { get; set; }

        public List<ImageFile> Imgs { get; set; }
        public double Price { get; set; }
        public string PriceLabel
        {
            get
            {
                if (Price > 0)
                {
                    NumberFormatInfo nfi = (NumberFormatInfo)
                        CultureInfo.InvariantCulture.NumberFormat.Clone();
                    nfi.NumberGroupSeparator = " ";
                    return

                        Price.ToString("N0", nfi) + " " + Currency;
                }
                else
                {
                    return "По договоренности";
                }
            }
        }

        public int Country { get; set; }

        public int Location { get; set; }
        public string LocationName { get; set; }
        public int IsFeatured { get; set; }
        public int? SellerId { get; set; }
        public string SellerName { get; set; }
        public string SellerDistrict { get; set; }
        

        public DateTime JoinedDate { get; set; }

        public string SellerPhone { get; set; }

        public string SellerEmail { get; set; }


        public string CategoryName { get; set; }
        
        [Range(10, 1000, ErrorMessage = "Заполните категорию")]
        public int Category { get; set; }
        public int SubCategory { get; set; }
        public string adsurl
        {
            get { return "/Adv/AdvDetails/" + Id; }
        }

        public string firstphoto
        {
            get { return ImgIds != null && ImgIds.Length > 0 ? "/Photo/Thumb/" + ImgIds[0] : ""; }
        }
        public bool Negotiable { get; set; }
        public DateTime Created { get; set; }

        public string CreatedText
        {
            get { return Created.ToString(); }
        }
        public AdvCondition condition { get; set; }
        public AdvType type { get; set; }

        public bool? IsTrusted { get; set; }
        public Review TopReview { get; set; }
        public Blog TopBlog { get; set; }
        public int ViewCount { get; set; }
        public string KeyWords { get; set; }
        public string CountryCode { get; set; }
    }

    public class KeyNamePair
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class KeyNameModel
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
    }
}