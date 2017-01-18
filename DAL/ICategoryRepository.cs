using System.Collections.Generic;

namespace DAL
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
    }
}