using System;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class SiteMessage
    {
        public string FromPhone { get; set; }
        public string FromEmail { get; set; }
        public string From { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public int Viewed { get; set; }

        [Key]
        public int Id { get; set; }
    }
}