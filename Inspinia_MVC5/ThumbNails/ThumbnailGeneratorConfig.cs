using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace RmsAuto.Store.Cms.Misc.Thumbnails
{
	public class ThumbnailGeneratorConfig : ConfigurationSection
	{
		public static ThumbnailGeneratorConfig Default
		{
			get { return (ThumbnailGeneratorConfig)ConfigurationManager.GetSection( "cms.thumbnailGenerator" ); }
		}

		[ConfigurationProperty( "cachePath", IsRequired = true )]
		public string ThumbnailCachePath
		{
			get { return Convert.ToString( this[ "cachePath" ] ); }
		}

        [ConfigurationProperty("ImageCachePath", IsRequired = true)]
        public string ImageCachePath
        {
            get { return Convert.ToString(this["ImageCachePath"]); }
        }

        [ConfigurationProperty("ThumbCachePath", IsRequired = true)]
        public string ThumbCachePath
        {
            get { return Convert.ToString(this["ThumbCachePath"]); }
        }

		[ConfigurationProperty( "", IsDefaultCollection = true )]
		public ThumbnailGeneratorConfigCollection Items
		{
			get { return (ThumbnailGeneratorConfigCollection)this[ "" ]; }
		}

	}

	[ConfigurationCollection( typeof( ThumbnailGeneratorConfigItem ) )]
	public class ThumbnailGeneratorConfigCollection : ConfigurationElementCollection
	{

		public ThumbnailGeneratorConfigItem GetItem( string thumbnailGeneratorKey )
		{
			return (ThumbnailGeneratorConfigItem)this.BaseGet( thumbnailGeneratorKey );
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new ThumbnailGeneratorConfigItem();
		}

		protected override object GetElementKey( ConfigurationElement element )
		{
			ThumbnailGeneratorConfigItem item = (ThumbnailGeneratorConfigItem)element;
			return item.Key;
		}
	}


	public class ThumbnailGeneratorConfigItem : ConfigurationElement
	{
		[ConfigurationProperty( "key", IsRequired = true, IsKey = true )]
		public string Key
		{
			get { return Convert.ToString( this[ "key" ] ); }
		}


		[ConfigurationProperty( "width", IsRequired = false, DefaultValue = 0 )]
		public int Width
		{
			get { return Convert.ToInt32( this[ "width" ] ); }
		}

		[ConfigurationProperty( "height", IsRequired = false, DefaultValue = 0 )]
		public int Height
		{
			get { return Convert.ToInt32( this[ "height" ] ); }
		}

        [ConfigurationProperty("pixelCrop", IsRequired = false, DefaultValue = 0)]
        public int PixelCrop
        {
            get { return Convert.ToInt32(this["pixelCrop"]); }
        }

		[ConfigurationProperty( "jpegQuality", IsRequired = true )]
		public long JpegQuality
		{
			get { return Convert.ToInt64( this[ "jpegQuality" ] ); }
		}

	}

}
