using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Reflection;

namespace RmsAuto.Common.DataAnnotations
{
	public class FieldGenerator : IAutoFieldGenerator
	{
		// Fields
		private Control _dataBoundControl;
		private MetaTable _table;
		DataBoundControlMode _mode;
		List<MetaColumn> _columns;

		// Methods
		public FieldGenerator( MetaTable table, Control dataBoundControl, DataBoundControlMode mode )
		{
			this._table = table;
			this._dataBoundControl = dataBoundControl;
			this._mode = mode;

			List<MetaColumn> columns = new List<MetaColumn>( _table.Columns );

			//упорядочить колонки в порядке следования в мета-классе.
			var metadataTypeAttribute = (MetadataTypeAttribute)Attribute.GetCustomAttribute( _table.EntityType, typeof( MetadataTypeAttribute ), false );
			if( metadataTypeAttribute != null )
			{
				int index = 0;
				foreach( var memberInfo in metadataTypeAttribute.MetadataClassType.GetMembers().Where( m => m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property ) )
				{
					int pos = columns.FindIndex( c => c.Name == memberInfo.Name );
					if( pos >= 0 )
					{
						MetaColumn column = columns[ pos ];
						columns.RemoveAt( pos );
						columns.Insert( index++, column );
					}
				}
			}
			_columns = columns;
		}

		public ICollection GenerateFields( Control control )
		{
			List<DynamicField> list = new List<DynamicField>();
			foreach( MetaColumn column in this._columns )
			{
				if( ( column.Scaffold && ( !column.IsLongString || !( control is GridView ) ) ) && ( ( this._mode != DataBoundControlMode.Insert ) || !( column is MetaChildrenColumn ) ) )
				{
					DynamicField item = new DynamicField();
					item.DataField = column.Name;
					list.Add( item );
				}
			}
			return list;
		}

	}

}
