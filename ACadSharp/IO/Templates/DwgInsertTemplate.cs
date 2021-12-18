﻿using ACadSharp.Blocks;
using ACadSharp.Entities;
using ACadSharp.IO.DWG;
using System.Collections.Generic;

namespace ACadSharp.IO.Templates
{
	internal class DwgInsertTemplate : DwgEntityTemplate
	{
		public bool HasAtts { get; internal set; }
		public int OwnedObjectsCount { get; internal set; }
		public ulong? BlockHeaderHandle { get; internal set; }
		public ulong? FirstAttributeHandle { get; internal set; }
		public ulong? EndAttributeHandle { get; internal set; }
		public ulong SeqendHandle { get; internal set; }
		public List<ulong> OwnedHandles { get; set; } = new List<ulong>();

		public DwgInsertTemplate(Insert insert) : base(insert) { }

		public override void Build(CadDocumentBuilder builder)
		{
			base.Build(builder);

			Insert insert = this.CadObject as Insert;

			if (this.BlockHeaderHandle.HasValue)
			{
				insert.Block = builder.GetCadObject<Block>(this.BlockHeaderHandle.Value);
			}

			if (this.FirstAttributeHandle.HasValue)
			{
				var attributes = getEntitiesCollection<Entities.Attribute>(builder, FirstAttributeHandle.Value, EndAttributeHandle.Value);
				insert.Attributes.AddRange(attributes);
			}
			else
			{
				foreach (ulong handle in this.OwnedHandles)
				{
					var att = builder.GetCadObject<Entities.Attribute>(handle);
					insert.Attributes.Add(att);
				}
			}

			//TODO: DwgInsertTemplate SeqendHandle??
		}
	}
}
