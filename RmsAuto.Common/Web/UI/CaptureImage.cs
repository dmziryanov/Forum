using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Web;

namespace RmsAuto.Common.Web.UI
{
    public class CaptureImage : IDisposable
    {
        #region Persistense

        public const int CaptureStringLength = 6;
        public const string DefaultCapturePicker = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ123456789";

        private static readonly object _locker = new object();
                
        private static string CreateNewCaptureString(int length, string pickfrom)
        {
            string result = String.Empty;
            int picklen = pickfrom.Length - 1;
            int index = 0;
            for (int i = 0; i < length; i++)
            {
                index = _rnd.Next(picklen);
                result = result + pickfrom.Substring(index, 1);
            }

            return result;
        }
        public static string CaptureString
        {
            get
            {
                return HttpContext.Current.Session[CaptureStringKey] as string;
            }
        }
        public static void InitCaptureText()
        {
            InitCaptureText(CaptureStringLength, DefaultCapturePicker);
        }
        
        public static void InitCaptureText(int length, string pickfrom)
        {
            if (HttpContext.Current.Session[CaptureStringKey] == null)
            {
                lock (_locker)
                {
                    if (HttpContext.Current.Session[CaptureStringKey] == null)
                    {
                        HttpContext.Current.Session[CaptureStringKey] = CreateNewCaptureString(length, pickfrom);
                    }
                }
            }
        }

        public static bool CheckCapture(string toCheck)
        {
            string curValue = HttpContext.Current.Session[CaptureStringKey] as string;
            return String.IsNullOrEmpty(curValue) || toCheck == null ? false : toCheck.Equals(curValue, StringComparison.OrdinalIgnoreCase);
        }

        public static void RefreshCapture()
        {
            HttpContext.Current.Session[CaptureStringKey] = null;
            InitCaptureText();
        }

        public static readonly string CaptureStringKey = "CaptureString";

        #endregion

		static Random _rnd = new Random();

		public string Text { get; private set; }
        public string FamilyName { get; private set; }
        public Bitmap Image { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

		public CaptureImage( string s, int width, int height )
		{
			this.Text = s;
			this.SetDimensions( width, height );
			this.GenerateImage();
		}

        public CaptureImage(string s, int width, int height, string familyName)
		{
			this.Text = s;
			this.SetDimensions( width, height );
			this.SetFamilyName( familyName );
			this.GenerateImage();
		}

		private void SetDimensions( int width, int height )
		{
			if ( width <= 0 )
				throw new ArgumentOutOfRangeException( "width", width, "Argument out of range, must be greater than zero." );
			if ( height <= 0 )
				throw new ArgumentOutOfRangeException( "height", height, "Argument out of range, must be greater than zero." );
			this.Width = width;
			this.Height = height;
		}

		private void SetFamilyName( string familyName )
		{
			try
			{
				using( Font font = new Font( familyName, 12F ) )
				{
					this.FamilyName = familyName;
				}
			}
			catch
			{
				this.FamilyName = System.Drawing.FontFamily.GenericSerif.Name;
			}
		}

		private void GenerateImage()
		{
			Bitmap bitmap = new Bitmap( this.Width, this.Height, PixelFormat.Format32bppArgb );
			try
			{
				using( Graphics g = Graphics.FromImage( bitmap ) )
				using( HatchBrush hatchBrush1 = new HatchBrush( HatchStyle.SmallConfetti, Color.LightGray, Color.White ) )
				using( HatchBrush hatchBrush2 = new HatchBrush( HatchStyle.LargeConfetti, Color.LightSkyBlue, Color.DarkGray ) )
				{
					g.SmoothingMode = SmoothingMode.AntiAlias;
					Rectangle rect = new Rectangle( 0, 0, this.Width, this.Height );

					g.FillRectangle( hatchBrush1, rect );

					SizeF size;
					float fontSize = rect.Height + 1;
					Font font = null;

					do
					{
						fontSize--;
						font = new Font( this.FamilyName, fontSize, FontStyle.Bold );
						size = g.MeasureString( this.Text, font );

						if( size.Width > rect.Width )
							font.Dispose();

					} while( size.Width > rect.Width );


					using( StringFormat format = new StringFormat() )
					using( GraphicsPath path = new GraphicsPath() )
					{
						format.Alignment = StringAlignment.Center;
						format.LineAlignment = StringAlignment.Center;

						path.AddString( this.Text, font.FontFamily, (int)font.Style, font.Size, rect, format );
						float v = 4F;
						PointF[] points =
						{
							new PointF(_rnd.Next(rect.Width) / v, _rnd.Next(rect.Height) / v),
							new PointF(rect.Width - _rnd.Next(rect.Width) / v, _rnd.Next(rect.Height) / v),
							new PointF(_rnd.Next(rect.Width) / v, rect.Height - _rnd.Next(rect.Height) / v),
							new PointF(rect.Width - _rnd.Next(rect.Width) / v, rect.Height - _rnd.Next(rect.Height) / v)
						};
						using( Matrix matrix = new Matrix() )
						{
							matrix.Translate( 0F, 0F );
							path.Warp( points, rect, matrix, WarpMode.Perspective, 0F );
						}

						g.FillPath( hatchBrush2, path );

					}
					int m = Math.Max( rect.Width, rect.Height );
					for( int i = 0 ; i < (int)( rect.Width * rect.Height / 30F ) ; i++ )
					{
						int x = _rnd.Next( rect.Width );
						int y = _rnd.Next( rect.Height );
						int w = _rnd.Next( m / 50 );
						int h = _rnd.Next( m / 50 );
						g.FillEllipse( hatchBrush2, x, y, w, h );
					}

					this.Image = bitmap;
				}
			}
			catch
			{
				bitmap.Dispose();
				throw;
			}
		}

		void IDisposable.Dispose()
		{
			GC.SuppressFinalize( this );
			this.Image.Dispose();
		}
    }
}
