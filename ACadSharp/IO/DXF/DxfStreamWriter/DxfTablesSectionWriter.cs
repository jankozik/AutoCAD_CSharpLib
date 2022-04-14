﻿using ACadSharp.Tables;
using ACadSharp.Tables.Collections;
using System;
using System.Linq;

namespace ACadSharp.IO.DXF
{
	internal class DxfTablesSectionWriter : DxfSectionWriterBase
	{
		public override string SectionName { get { return DxfFileToken.TablesSection; } }

		public DxfTablesSectionWriter(IDxfStreamWriter writer, CadDocument document) : base(writer, document)
		{
		}

		protected override void writeSection()
		{
			this.writeTable(this._document.VPorts);
			this.writeTable(this._document.LineTypes);
			this.writeTable(this._document.Layers);
			this.writeTable(this._document.TextStyles);
			this.writeTable(this._document.Views);
			this.writeTable(this._document.UCSs);
			this.writeTable(this._document.AppIds);
			this.writeTable(this._document.DimensionStyles);
			this.writeTable(this._document.BlockRecords);
		}

		private void writeTable<T>(Table<T> table)
			where T : TableEntry
		{
			if (table.Any())
				return;

			this._writer.Write(DxfCode.Start, DxfFileToken.EntityTable);
			this._writer.Write(DxfCode.SymbolTableName, table.ObjectName);

			this.writeCommonObjectData(table);

			this._writer.Write(DxfCode.Subclass, DxfSubclassMarker.Table);

			this._writer.Write(DxfCode.Int16, table.Count);

			foreach (T item in table)
			{

			}

			this._writer.Write(DxfCode.Start, DxfFileToken.EndTable);
		}
	}
}
