using System.Collections.Generic;

namespace DAL
{
    public class ReviewModel : BaseModel
    {
        public List<Review> All { get; set; }
        public string AboutName { get; set; }
        public string PersonName { get; set; }
        public string Text { get; set; }
    }
}