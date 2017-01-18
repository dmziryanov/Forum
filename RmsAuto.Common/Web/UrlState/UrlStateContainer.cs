using System.Collections.Specialized;
using System.Collections.Generic;
using RmsAuto.Common.Misc;
using System.Text;
using System;
using System.Web;

namespace RmsAuto.Common.Web.UrlState
{
	public class UrlStateContainer : NameValueCollection
	{
		Dictionary<string, bool> _isBaseParameter = new Dictionary<string, bool>();
		Uri _url;

		public UrlStateContainer( Uri url )
		{
			_url = url;
		}

		public void MarkBaseParameter( string name )
		{
			_isBaseParameter[ name ] = true;
		}

		public void AddBaseParameterFromRequest( string name )
		{
			if( HttpContext.Current.Request.QueryString.GetValues( name ) != null )
			{
				foreach( string value in HttpContext.Current.Request.QueryString.GetValues( name ) )
					this.Add( name, value );
			}
			MarkBaseParameter( name );
		}

		public string GetPageUrl()
		{
			return GetUrl( this );
		}

		public string GetBasePageUrl()
		{
			NameValueCollection col = new NameValueCollection();
			for( int i = 0 ; i < this.Count ; ++i )
			{
				string key = this.GetKey( i );
				if( _isBaseParameter.ContainsKey( key ) && _isBaseParameter[ key ] )
				{
					foreach( string value in this.GetValues( i ) )
						col.Add( key, value );
				}
			}
			return GetUrl( col );
		}

		string GetUrl( NameValueCollection parms )
		{
			StringBuilder sb = new StringBuilder();
			sb.Append( _url.AbsolutePath );

			string qs = parms.ToWwwQueryString();
			if( !string.IsNullOrEmpty( qs ) )
			{
				sb.Append( '?' );
				sb.Append( qs );
			}

			return sb.ToString();
		}
	}
}
