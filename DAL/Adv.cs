using System;
using System.Xml.Serialization;

namespace DAL
{
    public class Adv
    {
        [XmlIgnore]
        public string Currency { get; set; }
        [XmlIgnore]
        public int Id { get; set; }

        [XmlIgnore]
        public string Name { get; set; }

        [XmlIgnore]
        public string Description { get; set; }

        [XmlIgnore]
        public double Price { get; set; }

        [XmlIgnore]
        public int Location { get; set; }

        [XmlIgnore]
        public int Category { get; set; }

        [XmlIgnore]
        public bool Negotiable { get; set; }

        [XmlIgnore]
        public int Condition { get; set; }

        [XmlIgnore]
        public int type { get; set; }

        [XmlIgnore]
        public int SubCategory { get; set; }
        
        [XmlIgnore]
        public int IsFeatured { get; set; }

        [XmlIgnore]
        public string SellerName { get; set; }
        
        [XmlIgnore]
        public string SellerPhone { get; set; }

        [XmlIgnore]
        public string SellerEmail { get; set; }

        [XmlIgnore]
        public string SellerDistrict { get; set; }
        
        [XmlIgnore]
        public int? SellerId { get; set; }
        
        [XmlIgnore]
        public int Country { get; set; }

         [XmlIgnore]
        public DateTime Created { get; set; }
    }

    public class CarAdv : Adv
    {
        public int MileAge { get; set; }
        public int Year { get; set; }
        public int customs { get; set; }
        public bool guarantee { get; set; }
        public string PTS { get; set; }
        public string VIN { get; set; }
        public int Brand { get; set; }
        public int Model { get; set; }

        public int CarType { get; set; }
        public int CarModel { get; set; }
        
    }


    public class PhotoAdv
    {
        
        public int PhotoId { get; set; }

        public int AdvId { get; set; }
    }


    
}