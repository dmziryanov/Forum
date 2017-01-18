using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RmsAuto.Common.Data
{
	public class PartKey : IEquatable<PartKey>, IComparable<PartKey>
	{
		string _partNumber;
		string _normalizedPartNumber;
		public string PartNumber
		{
			get { return _partNumber; }
			set
			{
				_partNumber = value;
				_normalizedPartNumber = NormalizePartNumber( value );
			}
		}
		public string Manufacturer { get; set; }

		public PartKey( string mfr, string pn )
		{
			if( pn == null )
			{
				throw new ArgumentNullException( "PartNumber" );
			}

			this.PartNumber = pn;
			this.Manufacturer = mfr;
		}

		public override string ToString()
		{
			return String.Format( "{0} ({1})", this.PartNumber, this.Manufacturer );
		}

		public override bool Equals( object obj )
		{
			return this.Equals( (PartKey)obj );
		}

		public override int GetHashCode()
		{
            if (this.Manufacturer != null)
                return this.Manufacturer.ToLower().GetHashCode() ^ _normalizedPartNumber.ToLower().GetHashCode();
            else
                return _normalizedPartNumber.ToLower().GetHashCode();

		}

		#region IEquatable<PartKey> Members

		public bool Equals( PartKey other )
		{
			return other != null &&
					this.Manufacturer.Equals( other.Manufacturer, StringComparison.CurrentCultureIgnoreCase ) &&
					this._normalizedPartNumber.Equals( other._normalizedPartNumber, StringComparison.CurrentCultureIgnoreCase );
		}

		#endregion

		#region IComparable<PartKey> Members

		public int CompareTo( PartKey other )
		{
			if( other != null )
			{
				int res = String.Compare( this.Manufacturer, other.Manufacturer, true );
				if( res == 0 )
					res = String.Compare( _normalizedPartNumber, other._normalizedPartNumber, true );
				return res;
			}
			return 1;
		}

		#endregion

		/// <summary>
		/// Нормализует партнамбер (убирает пробелы и проч лабуду) - формат.
		/// </summary>
		/// <param name="partNumber">Партнамбер</param>
		/// <returns>Нормализованный партнамбер</returns>
		public static string NormalizePartNumber( string partNumber )
		{
			return partNumber == null ? null : Regex.Replace( partNumber, @"[^\w]", "", RegexOptions.Singleline );
		}

        // deas 06.05.2011 task4045 Замена русских букв в поиске
        /// <summary>
        /// Замена русских букв в поиске
        /// </summary>
        /// <param name="partNumber">Наименование позиции</param>
        /// <returns>Были ли изменения</returns>
        public static bool ConvertToEng( ref string partNumber )
        {
            if ( partNumber != null )
            {
                if ( Regex.Match( partNumber.ToUpper(), "[ЕТОРАНКХСВМ]" ).Success )
                {
                    Dictionary<string, string> transl = new Dictionary<string, string>();
                    transl.Add( "Е", "E" );
                    transl.Add( "Т", "T" );
                    transl.Add( "О", "O" );
                    transl.Add( "Р", "P" );
                    transl.Add( "А", "A" );
                    transl.Add( "Н", "H" );
                    transl.Add( "К", "K" );
                    transl.Add( "Х", "X" );
                    transl.Add( "С", "C" );
                    transl.Add( "В", "B" );
                    transl.Add( "М", "M" );
                    foreach ( var str in transl )
                    {
                        partNumber = partNumber.Replace( str.Key, str.Value );
                        partNumber = partNumber.Replace( str.Key.ToLower(), str.Value.ToLower() );
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

	}
}
