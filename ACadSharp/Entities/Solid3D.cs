﻿using ACadSharp.Attributes;
using ACadSharp.IO.Templates;
using CSMath;

namespace ACadSharp.Entities
{
	public class Solid3D : Entity
	{
		public override ObjectType ObjectType => ObjectType.SOLID3D;
		public override string ObjectName => DxfFileToken.Entity3DSolid;

		//100	Subclass marker(AcDbTrace)

		[DxfCodeValue(10, 20, 30)]
		public XYZ FirstCorner { get; set; }

		[DxfCodeValue(11, 21, 31)]
		public XYZ SecondCorner { get; set; }

		[DxfCodeValue(12, 22, 32)]
		public XYZ ThirdCorner { get; set; }
		
		//Fourth corner.If only three corners are entered to define the SOLID, then the fourth corner coordinate is the same as the third.
		[DxfCodeValue(13, 23, 33)]
		public XYZ FourthCorner { get; set; }

		/// <summary>
		/// Specifies the distance a 2D AutoCAD object is extruded above or below its elevation.
		/// </summary>
		[DxfCodeValue(39)]
		public double Thickness { get; set; } = 0.0;

		/// <summary>
		/// Specifies the three-dimensional normal unit vector for the object.
		/// </summary>
		[DxfCodeValue(210, 220, 230)]
		public XYZ Normal { get; set; } = XYZ.AxisZ;

		public Solid3D() : base() { }
	}
}
