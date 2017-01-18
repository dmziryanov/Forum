using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO = System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using DAL;


namespace RmsAuto.Store.Cms.Misc.Thumbnails
{
    public class ThumbnailGenerator
    {
        public static ThumbnailInfo GetThumbnail(int fileID, string thumbnailGeneratorKey)
        {
            //В этом событии все плохо  DCFactory получает параметр из SiteContext, а он еще не готов 
            ThumbnailGeneratorConfigItem config = ThumbnailGeneratorConfig.Default.Items.GetItem(thumbnailGeneratorKey);


            ImageFile file;
            using (var _advContext = new AdvContext())
            {
                file = _advContext.ImageFiles.Where<ImageFile>(x => x.FileID == fileID).FirstOrDefault();

            }

            ThumbnailInfo res = new ThumbnailInfo();
            res.ContentType = "image/jpeg";
            string dir = IO.Path.Combine(ThumbnailGeneratorConfig.Default.ThumbnailCachePath);
            string filename = string.Format("{0}.jpg", file.FileID);
            res.FilePath = IO.Path.Combine(dir, filename);
            res.LastModifiedDate = new IO.FileInfo(res.FilePath).LastWriteTime;

            if (!IO.File.Exists(res.FilePath) || res.LastModifiedDate < file.Created)
            {
                IO.Directory.CreateDirectory(dir);
                GenerateThumbnail(res.FilePath, CropImage(fileID, thumbnailGeneratorKey), config);
                res.LastModifiedDate = new IO.FileInfo(res.FilePath).LastWriteTime;
            }
            return res;
        }

        public static ThumbnailInfo CropImage(int fileID, string thumbnailGeneratorKey)
        {
            //В этом событии все плохо  DCFactory получает параметр из SiteContext, а он еще не готов 
            ThumbnailGeneratorConfigItem config = ThumbnailGeneratorConfig.Default.Items.GetItem(thumbnailGeneratorKey);


            ImageFile file;
            using (var _advContext = new AdvContext())
            {
                file = _advContext.ImageFiles.Where<ImageFile>(x => x.FileID == fileID).FirstOrDefault();

            }

            ThumbnailInfo res = new ThumbnailInfo();
            res.ContentType = "image/jpeg";
            string dir = IO.Path.Combine(ThumbnailGeneratorConfig.Default.ImageCachePath);
            string filename = string.Format("{0}.jpg", file.FileID);
            res.FilePath = IO.Path.Combine(dir, filename);
            res.LastModifiedDate = new IO.FileInfo(res.FilePath).LastWriteTime;

            if (!IO.File.Exists(res.FilePath) || res.LastModifiedDate < file.Created)
            {
                IO.Directory.CreateDirectory(dir);
                Crop(res.FilePath, file, config);
                res.LastModifiedDate = new IO.FileInfo(res.FilePath).LastWriteTime;
            }
            return res;
        }

        public static ThumbnailInfo CropImageForThumb(int fileID, string thumbnailGeneratorKey)
        {
            //В этом событии все плохо  DCFactory получает параметр из SiteContext, а он еще не готов 
            ThumbnailGeneratorConfigItem config = ThumbnailGeneratorConfig.Default.Items.GetItem(thumbnailGeneratorKey);


            ImageFile file;
            using (var _advContext = new AdvContext())
            {
                file = _advContext.ImageFiles.Where<ImageFile>(x => x.FileID == fileID).FirstOrDefault();

            }

            ThumbnailInfo res = new ThumbnailInfo();
            res.ContentType = "image/jpeg";
            string dir = IO.Path.Combine(ThumbnailGeneratorConfig.Default.ThumbCachePath);
            string filename = string.Format("{0}.jpg", file.FileID);
            res.FilePath = IO.Path.Combine(dir, filename);
            res.LastModifiedDate = new IO.FileInfo(res.FilePath).LastWriteTime;

            if (!IO.File.Exists(res.FilePath) || res.LastModifiedDate < file.Created)
            {
                IO.Directory.CreateDirectory(dir);
                Crop(res.FilePath, file, config);
                res.LastModifiedDate = new IO.FileInfo(res.FilePath).LastWriteTime;
            }
            return res;
        }

        private static void Crop(string filePath, ImageFile file, ThumbnailGeneratorConfigItem thumbnailConfig)
        {
            using (IO.MemoryStream largeImageStream = new IO.MemoryStream(file.FileBody.ToArray()))
            using (Image largeImage = Image.FromStream(largeImageStream))
            {

                double ratio;
                if (thumbnailConfig.Width > 0 && thumbnailConfig.Height > 0)
                    ratio = Math.Min(((float)thumbnailConfig.Width) / largeImage.Width, ((float)thumbnailConfig.Height) / largeImage.Height);
                else if (thumbnailConfig.Width > 0 && thumbnailConfig.Height <= 0)
                    ratio = ((float)thumbnailConfig.Width) / largeImage.Width;
                else if (thumbnailConfig.Width <= 0 && thumbnailConfig.Height > 0)
                    ratio = ((float)thumbnailConfig.Height) / largeImage.Height;
                else
                    ratio = 1;

                int width = (int)(largeImage.Width * ratio);
                int height = (int)(largeImage.Height * ratio);
                if (width + 1 == thumbnailConfig.Width) ++width;
                if (height + 1 == thumbnailConfig.Height) ++height;

                using (Bitmap smallImage = new Bitmap(width, height))
                {
                    using (Graphics g = Graphics.FromImage(smallImage))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                        SolidBrush blueBrush = new SolidBrush(Color.White);

                        g.FillRectangle(blueBrush, 0, 0, width, height);

                        g.DrawImage(largeImage, 0, 0, width, height);


                    }

                    using (Bitmap crImage = new Bitmap(width, height - thumbnailConfig.PixelCrop))
                    {
                        using (Graphics g = Graphics.FromImage(crImage))
                        {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                            SolidBrush blueBrush = new SolidBrush(Color.White);

                            g.FillRectangle(blueBrush, 0, 0, width, height);

                            g.DrawImageUnscaledAndClipped(smallImage,
                                new Rectangle(0, 0, width, height - thumbnailConfig.PixelCrop));
                        }



                        using (IO.FileStream stm = new IO.FileStream(filePath, IO.FileMode.Create))
                        {
                            ImageCodecInfo jpegCodec =
                                ImageCodecInfo.GetImageEncoders().Where(c => c.MimeType == "image/jpeg").First();
                            EncoderParameters ep = new EncoderParameters();
                            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,
                                thumbnailConfig.JpegQuality);
                            crImage.Save(stm, jpegCodec, ep);
                        }
                    }
                }
            }
        }

        private static void GenerateThumbnail(string filePath, ThumbnailInfo file, ThumbnailGeneratorConfigItem thumbnailConfig)
        {

            using (IO.MemoryStream largeImageStream = new IO.MemoryStream(IO.File.ReadAllBytes(file.FilePath)))
            using (Image largeImage = Image.FromStream(largeImageStream))
            {
                double ratio;
                if (thumbnailConfig.Width > 0 && thumbnailConfig.Height > 0)
                    ratio = Math.Min(((float)thumbnailConfig.Width) / largeImage.Width, ((float)thumbnailConfig.Height) / largeImage.Height);
                else if (thumbnailConfig.Width > 0 && thumbnailConfig.Height <= 0)
                    ratio = ((float)thumbnailConfig.Width) / largeImage.Width;
                else if (thumbnailConfig.Width <= 0 && thumbnailConfig.Height > 0)
                    ratio = ((float)thumbnailConfig.Height) / largeImage.Height;
                else
                    ratio = 1;

                int width = (int)(largeImage.Width * ratio);
                int height = (int)(largeImage.Height * ratio);
                if (width + 1 == thumbnailConfig.Width) ++width;
                if (height + 1 == thumbnailConfig.Height) ++height;

                using (Bitmap smallImage = new Bitmap(width, height))
                {
                    using (Graphics g = Graphics.FromImage(smallImage))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                        SolidBrush blueBrush = new SolidBrush(Color.White);

                        g.FillRectangle(blueBrush, 0, 0, width, height);
                        g.DrawImage(largeImage, 0, 0, width, height);
                    }

                    using (IO.FileStream stm = new IO.FileStream(filePath, IO.FileMode.Create))
                    {
                        ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageEncoders().Where(c => c.MimeType == "image/jpeg").First();
                        EncoderParameters ep = new EncoderParameters();
                        ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, thumbnailConfig.JpegQuality);
                        smallImage.Save(stm, jpegCodec, ep);
                    }
                }
            }
        }
    }
}
