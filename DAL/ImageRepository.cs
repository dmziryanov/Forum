using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace DAL
{
    public class ImageRepository : IImageRepository
    {
        private AdvContext _advContext;


        public ImageRepository()
        {
        
        }

        public byte[] Get(int id)
        {
            using (_advContext = new AdvContext())
            {
                var res =_advContext.Database.SqlQuery<ImageFile>("Select * From dbo.ImageFile where FileId = {0}", id).FirstOrDefault();
                //var img = System.Drawing.Image.FromStream(new MemoryStream(res));
                
                return res.FileBody;
            }
        }

        public ImageFile GetFileName(int id)
        {
            using (_advContext = new AdvContext())
            {
                var res = _advContext.Database.SqlQuery<ImageFile>("Select * From dbo.ImageFile where FileId = {0}", id).FirstOrDefault();
                //var img = System.Drawing.Image.FromStream(new MemoryStream(res));

                return res;
            }
        }

        public void ReleaseContext()
        {
            
        }

        public int Save(ImageFile imageFile)
        {
            using (_advContext = new AdvContext())
            {
                (_advContext as IObjectContextAdapter).ObjectContext.CommandTimeout = 120;
                _advContext.ImageFiles.Add(imageFile);
                _advContext.SaveChanges();
                return imageFile.FileID;
            }
        }

        ~ImageRepository()
        {
            
        }
    }
}