using System;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class ImageFile
    {
        [Key]
        public int FileID { get; set; }
        public string FileName { get; set; }
        public byte[] FileBody { get; set; }
        public int FileSize { get; set; }
        public DateTime Created { get; set; }

    }
}