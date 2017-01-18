using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        public int Category_Id { get; set; }
        
        public int Count { get; set; }
        public string IconName { get; set; }
        public string Name { get; set; }
    }
}