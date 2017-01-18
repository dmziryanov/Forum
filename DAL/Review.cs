using System;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class Review : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public string PersonName { get; set; }

        public string PersonMail { get; set; }
        public string AboutName { get; set; }

        public DateTime Created { get; set; }
    }
}