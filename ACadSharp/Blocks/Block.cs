﻿using ACadSharp.Attributes;
using ACadSharp.Entities;
using ACadSharp.Tables;
using CSMath;

namespace ACadSharp.Blocks
{
	/// <summary>
	/// Represents a <see cref="Block"/> entity
	/// </summary>
	/// <remarks>
	/// Object name <see cref="DxfFileToken.Block"/> <br/>
	/// Dxf class name <see cref="DxfSubclassMarker.BlockBegin"/>
	/// </remarks>
	[DxfName(DxfFileToken.Block)]
	[DxfSubClass(DxfSubclassMarker.BlockBegin)]
	public class Block : Entity, INamedCadObject
	{
		/// <inheritdoc/>
		public override ObjectType ObjectType => ObjectType.BLOCK;

		/// <inheritdoc/>
		public override string ObjectName => DxfFileToken.Block;

		/// <summary>
		/// Block record that owns this entity
		/// </summary>
		public BlockRecord BlockOwner { get { return this.Owner as BlockRecord; } }

		/// <summary>
		/// Specifies the name of the object.
		/// </summary>
		[DxfCodeValue(2, 3)]
		public string Name
		{
			get { return BlockOwner.Name; }
			set { BlockOwner.Name = value; }
		}

		/// <summary>
		/// Block active flags.
		/// </summary>
		[DxfCodeValue(70)]
		public BlockTypeFlags Flags { get; set; }

		/// <summary>
		/// Specifies the insert point of the block.
		/// </summary>
		[DxfCodeValue(10, 20, 30)]
		public XYZ BasePoint { get; set; } = XYZ.Zero;

		/// <summary>
		/// Gets the path of the block, document, application, or external reference.
		/// </summary>
		[DxfCodeValue(1)]
		public string XrefPath { get; internal set; }

		/// <summary>
		/// Specifies the comments for the block or drawing.
		/// </summary>
		[DxfCodeValue(4)]
		public string Comments { get; set; }

		public Block(BlockRecord record) : base()
		{
			this.Owner = record;
		}

		/// <inheritdoc/>
		/// <remarks>
		/// Cloning a block will also unatach it from the record
		/// </remarks>
		public override object Clone()
		{
			Block clone = new Block(new BlockRecord(this.BlockOwner.Name));

			this.createCopy(clone);

			return clone;
		}

		protected override void createCopy(CadObject clone)
		{
			base.createCopy(clone);

			Block block = clone as Block;

			block.Flags = this.Flags;
			block.BasePoint = this.BasePoint;
			block.XrefPath = this.XrefPath;
			block.Comments = this.Comments;
		}
	}
}
