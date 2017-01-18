using System.Collections.Generic;

namespace DAL
{
    public class BaseModel
    {
        public IEnumerable<Location> _locations;
        public IEnumerable<Category> _subCategories;
        public IEnumerable<Category> _categories;
        public IEnumerable<Country> _countries;
    }

    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public string Code { get; set; }
    }
}