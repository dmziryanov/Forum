using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using DAL;
using RmsAuto.Store.Cms.Misc.Thumbnails;

namespace AdvSpareAuto.Controllers
{
    public class PhotoController : Controller
    {
        private IImageRepository _imageRepository;

        public PhotoController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        //
        // GET: /Photo/


        public void Index(int id)
        {
            int fileID = id;
            string thumbnailGeneratorKey = "largephoto";

            ThumbnailInfo thumbnail = ThumbnailGenerator.CropImageForThumb(fileID, thumbnailGeneratorKey);
            if (thumbnail == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Not Found");
            }
            else
            {
                string etag = thumbnail.LastModifiedDate.GetHashCode().ToString("x");

                Response.ContentType = thumbnail.ContentType;
                Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
                Response.Cache.SetETag(etag);

                if (Request.Headers["If-None-Match"] != etag)
                {
                    Response.WriteFile(thumbnail.FilePath);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.NotModified;
                }
            }
        }

        public void File(int id)
        {
            var imageData = _imageRepository.GetFileName(id);
            if (imageData == null) return;

            Response.ContentType = "application/octet-stream";
            Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
            Response.AddHeader("content-disposition", @"attachment;filename='"+imageData.FileName+"'");
            Response.BinaryWrite(imageData.FileBody);
        }


        public void Thumb(int id)
        {
            byte[] imageData = _imageRepository.Get(id);
            if (imageData == null) return;


            int fileID = id;
            string thumbnailGeneratorKey = "photo";



            ThumbnailInfo thumbnail = ThumbnailGenerator.GetThumbnail(fileID, thumbnailGeneratorKey);
            if (thumbnail == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Not Found");
            }
            else
            {
                string etag = thumbnail.LastModifiedDate.GetHashCode().ToString("x");

                Response.ContentType = thumbnail.ContentType;
                Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
                Response.Cache.SetETag(etag);
                //context.Response.Cache.SetMaxAge( new TimeSpan( 0, 10, 0 ) );

                if (Request.Headers["If-None-Match"] != etag)
                {
                    Response.WriteFile(thumbnail.FilePath);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.NotModified;
                }
            }
        }

        private ActionResult FileResult(Image objImage)
        {
            Stream ms = new MemoryStream();

            objImage.Save(ms, ImageFormat.Jpeg);


            return File(ms, "image/jpeg");

        }

        /*       private Bitmap WaterMark(Bitmap image) {


          /*  using (Bitmap watermarkImage = Bitmap.(Server.MapPath("~Images/watermark.png")))
            {
                using (Graphics imageGraphics = Graphics.FromImage(image))
                {
                    watermarkImage.SetResolution(imageGraphics.DpiX, imageGraphics.DpiY);

        int x = ((image.Width - watermarkImage.Width) / 2);
        int y = ((image.Height - watermarkImage.Height) / 2);

        imageGraphics.DrawImage(watermarkImage, x, y, watermarkImage.Width, watermarkImage.Height);
            }#1#
        

    
    

        
    
        }*/

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public Image Crop(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height - 30);
            var destImage = new Bitmap(width, height - 30);


            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(destImage, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    return destImage;
                }
            }


        }

        public void ReleaseContext()
        {
            _imageRepository.ReleaseContext();
        }
    }


}
