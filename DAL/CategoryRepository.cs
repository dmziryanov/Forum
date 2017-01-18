using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DAL
{
    public class CategoryRepository : ICategoryRepository
    {
        private AdvContext _advContext;

        public CategoryRepository()
        {
            _advContext = new AdvContext();
        }

        public IEnumerable<Category> GetAll()
        {
            return null;// _advContext.Categories;
        }

        public IEnumerable<CarBrand> GetBrands()
        {
            return _advContext.CarBrand.ToList();
        }

        public IEnumerable<CarModel> GetModels(int id)
        {
            return _advContext.CarModel.Where(x=>x.BrandId == id).ToList();
        }

        public int SaveModel(CarModel carModel)
        {
            _advContext.CarModel.Add(carModel);
            _advContext.SaveChanges();
            return carModel.Id;
        }

        public int SaveBrand(CarBrand carModel)
        {
            _advContext.CarBrand.Add(carModel);
            _advContext.SaveChanges();
            return carModel.Id;
        }
    }

    public class CarBrand
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CarModel
    {
        [Key]
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
    }
}
