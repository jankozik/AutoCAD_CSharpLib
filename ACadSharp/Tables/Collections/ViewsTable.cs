﻿using ACadSharp.IO.Templates;

namespace ACadSharp.Tables.Collections
{
	public class ViewsTable : Table<View>
	{
		public override ObjectType ObjectType => ObjectType.VIEW_CONTROL_OBJ;

		public ViewsTable() { }
		internal ViewsTable(DxfTableTemplate template) : base(template) { }
	}
}