﻿using ACadSharp.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ACadSharp
{
	public abstract class CadObject
	{
		/// <summary>
		/// Get the object type.
		/// </summary>
		public abstract ObjectType ObjectType { get; }

		/// <summary>
		/// The AutoCAD class name of an object.
		/// </summary>
		public virtual string ObjectName { get; } = DxfFileToken.Undefined;

		/// <summary>
		/// The handle of the entity.
		/// </summary>
		/// <remarks>
		/// If the value is 0 the object doesn't belong to any document.
		/// </remarks>
		[DxfCodeValue(5)]
		public ulong Handle { get; internal set; }

		/// <summary>
		/// Soft-pointer ID/handle to owner object
		/// </summary>
		[DxfCodeValue(330)]
		public CadObject Owner { get; internal set; }

		[Obsolete("Will be replaced for the actual owner")]
		public ulong? OwnerHandle { get; internal set; }

		/// <summary>
		/// Objects that are attached to this entity.
		/// </summary>
		public Dictionary<ulong, CadObject> Reactors { get; set; } = new Dictionary<ulong, CadObject>();

		//TODO: Extended data

		/// <summary>
		/// Document where this element belongs
		/// </summary>
		public virtual CadDocument Document { get; internal set; }

		internal void AssignDxfValue(DxfCode dxfCode, object value)
		{
			foreach (PropertyInfo p in this.GetType().GetProperties())
			{
				DxfCodeValueAttribute att = p.GetCustomAttribute<DxfCodeValueAttribute>();
				if (att == null)
					continue;

				//Set the codes to the map
				if (att.ValueCodes.Length == 1 && att.ValueCodes.First() == dxfCode)
				{
					p.SetValue(this, value);
				}
			}
		}

		/// <summary>
		/// Get a map of the object using dxf codes in each field.
		/// </summary>
		/// <returns></returns>
		//TODO: Create the mab based on each type in the hirearchy
		internal Dictionary<DxfCode, object> GetCadObjectMap()
		{
			Dictionary<DxfCode, object> map = new Dictionary<DxfCode, object>();

			foreach (PropertyInfo p in this.GetType().GetProperties())
			{
				DxfCodeValueAttribute att = p.GetCustomAttribute<DxfCodeValueAttribute>();
				if (att == null)
					continue;

				//Set the codes to the map
				foreach (DxfCode code in att.ValueCodes)
				{
					map.Add(code, null);
				}
			}

			return map;
		}

		internal static Dictionary<int, object> GetCadObjectMap(Type type)
		{
			Dictionary<int, object> map = new Dictionary<int, object>();

			foreach (PropertyInfo p in type.GetProperties())
			{
				DxfCodeValueAttribute att = p.GetCustomAttribute<DxfCodeValueAttribute>();
				if (att == null)
					continue;

				//Set the codes to the map
				foreach (DxfCode code in att.ValueCodes)
				{
					map.Add((int)code, null);
				}
			}

			return map;
		}

		/// <summary>
		/// Build the entity using a map with the dxf codes and the values.
		/// </summary>
		/// <param name="map"></param>
		internal virtual void Build(Dictionary<DxfCode, object> map)
		{
			var a = this.GetType().GetProperties();

			foreach (PropertyInfo p in this.GetType().GetProperties())
			{
				DxfCodeValueAttribute att = p.GetCustomAttribute<DxfCodeValueAttribute>();
				if (att == null)
					continue;

				//Get the parameters or value to build the property
				List<object> parameters = new List<object>();
				foreach (DxfCode code in att.ValueCodes)
				{
					if (map.TryGetValue(code, out object par))
					{
						parameters.Add(par);
					}
				}

				//Check for invalid values
				while (parameters.Contains(null))
					parameters.Remove(null);

				if (!parameters.Any())
					continue;

				//Create an object with the same type of the property type
				object value = null;

				//Check if has a constructor with parameters
				ConstructorInfo constr = p.PropertyType.GetConstructor(parameters.Select(o => o.GetType()).ToArray());

				//Fill the value
				if (p.PropertyType.IsEnum)
				{
					value = Enum.ToObject(p.PropertyType, parameters.First());
				}
				else if (constr == null)
				{
					value = Convert.ChangeType(parameters.First(), p.PropertyType);
				}
				else
				{
					value = constr.Invoke(parameters.ToArray());
				}

				//Set the value if it has any
				if (value != null)
				{
					p.SetValue(this, value);
				}
			}
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{this.ObjectName}:{this.ObjectType}";
		}
	}
}
