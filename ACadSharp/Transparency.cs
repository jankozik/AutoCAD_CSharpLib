﻿#region copyright
//Copyright 2021, Albert Domenech.
//All rights reserved. 
//This source code is licensed under the MIT license. 
//See LICENSE file in the project root for full license information.
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace ACadSharp
{
	public struct Transparency
	{
		public static Transparency ByLayer { get { return new Transparency(-1); } }
		
		public static Transparency ByBlock { get { return new Transparency(100); } }

		public static Transparency Opaque { get { return new Transparency(0); } }

		/// <summary>
		/// Defines if the transparency is defined by layer.
		/// </summary>
		public bool IsByLayer
		{
			get { return _value == -1; }
		}
		
		/// <summary>
		/// Defines if the transparency is defined by block.
		/// </summary>
		public bool IsByBlock
		{
			get { return _value == 100; }
		}

		/// <summary>
		/// Gets or sets the transparency value.
		/// </summary>
		/// <remarks>
		/// Transparency values must be in range from 0 to 90, the reserved values -1 and 100 represents ByLayer and ByBlock.
		/// </remarks>
		public short Value
		{
			get { return _value; }
			set
			{
				if (value == -1)
				{
					_value = value;
					return;
				}

				if (value == 100)
				{
					_value = value;
					return;
				}

				if (value < 0 || value > 90)
					throw new ArgumentOutOfRangeException(nameof(value), value, "Transparency must be in range from 0 to 90.");

				_value = value;
			}
		}
		private short _value;

		public Transparency(short value)
		{
			_value = -1;
			this.Value = value;
		}

		/// <summary>
		/// Gets the transparency value within range of a valid value
		/// </summary>
		/// <param name="value">A transparency value</param>
		/// <returns>A <see cref="Transparency"></see></returns>
		public static Transparency FromValue(int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			short alpha = (short)(100 - (bytes[0] / 255.0) * 100);

			if (alpha == -1)
			{
				return ByLayer;
			}

			if (alpha == 100)
			{
				return ByBlock;
			}

			if (alpha < 0)
			{
				return new Transparency(0);
			}

			if (alpha > 90)
			{
				return new Transparency(90);
			}

			return new Transparency(alpha);
		}
	}
}
