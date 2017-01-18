using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace DAL
{
    public class ReviewRepository : IReviewRepository
    {
        private AdvContext _advContext;
        private readonly CommonContext _commonContext;
        private static List<Location> _locations;


        public ReviewRepository()
        {
            _commonContext = new CommonContext();
            _advContext = new AdvContext();
            //_locations = _commonContext.Database.SqlQuery<Location>("select * from dbo.Cities").OrderBy(x => x.Name).GetAllUsers();
        }

        public Review Get(int id)
        {
            using (_advContext = new AdvContext())
            {
                var res = _advContext.Database.SqlQuery<Review>("Select * From dbo.Review where Id = {0}", id).FirstOrDefault() ?? new Review();
                res._locations = _locations;
                return res;
            }
        }

        public List<Review> GetAll()
        {
            using (_advContext = new AdvContext())
            {
                var res = _advContext.Database.SqlQuery<Review>("Select * From dbo.Review").ToList();
                return res;
            }
        }

        public void Add(Review review)
        {
            using (_advContext = new AdvContext())
            {
                try
                {
                    _advContext.Reviews.Add(review);
                    _advContext.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var failure in ex.EntityValidationErrors)
                    {
                        sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                        foreach (var error in failure.ValidationErrors)
                        {
                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                            sb.AppendLine();
                        }
                    }

                    throw new DbEntityValidationException(
                        "Entity Validation Failed - errors follow:\n" +
                        sb.ToString(), ex
                    ); // Add the original exception as the innerException
                }

          
            }
        }

        public Review First()
        {
            using (_advContext = new AdvContext())
            {
                var res = _advContext.Database.SqlQuery<Review>("Select * From dbo.Review").FirstOrDefault();
                return res;
            }
        }

        public IEnumerable<Review> GetByUserId(int id)
        {
            using (_advContext = new AdvContext())
            {
                var res = _advContext.Database.SqlQuery<Review>("Select * From dbo.Review where Userid = {0}", id).ToList();
                return res;
            }
        }

        public void ReleaseContext()
        {
            //_advContext.Dispose();
        }

        ~ReviewRepository()
        {

        }
    }
}