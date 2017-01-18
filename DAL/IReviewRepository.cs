using System.Collections.Generic;

namespace DAL
{
    public interface IReviewRepository
    {
        Review Get(int i);
        List<Review> GetAll();

        void Add(Review review);
        Review First();
        IEnumerable<Review> GetByUserId(int id);
    }
}