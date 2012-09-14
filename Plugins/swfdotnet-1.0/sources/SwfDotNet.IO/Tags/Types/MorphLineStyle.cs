/*
	SwfDotNet is an open source library for writing and reading 
	Macromedia Flash (SWF) bytecode.
	Copyright (C) 2005 Olivier Carpentier - Adelina foundation
	see Licence.cs for GPL full text!
		
	SwfDotNet.IO uses a part of the open source library SwfOp actionscript 
	byte code management, writted by Florian Krüsch, Copyright (C) 2004 .
	
	This library is free software; you can redistribute it and/or
	modify it under the terms of the GNU General Public
	License as published by the Free Software Foundation; either
	version 2.1 of the License, or (at your option) any later version.
	
	This library is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
	General Public License for more details.
	
	You should have received a copy of the GNU General Public
	License along with this library; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using System;
using System.IO;
using System.Xml;
using System.Collections;

using SwfDotNet.IO.Utils;

namespace SwfDotNet.IO.Tags.Types
{
	/// <summary>
	/// MorphLineStyle
	/// </summary>
	public class MorphLineStyle: ISwfSerializer
	{
		#region Members

		private ushort startWith;
		private ushort endWith;
		private RGBA startColor;
		private RGBA endColor;

		#endregion

		#region Ctor

		/// <summary>
		/// Creates a new <see cref="MorphLineStyle"/> instance.
		/// </summary>
		public MorphLineStyle()
		{
		}

		/// <summary>
		/// Creates a new <see cref="MorphLineStyle"/> instance.
		/// </summary>
		/// <param name="startWith">Start with.</param>
		/// <param name="endWith">End with.</param>
		/// <param name="startColor">Color of the start.</param>
		/// <param name="endColor">Color of the end.</param>
		public MorphLineStyle(ushort startWith, ushort endWith, RGBA startColor, RGBA endColor)
		{
			this.startWith = startWith;
			this.endWith = endWith;
			this.startColor = startColor;
			this.endColor = endColor;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Reads the data.
		/// </summary>
		/// <param name="binaryReader">Binary reader.</param>
		public void ReadData(BufferedBinaryReader binaryReader)
		{
			startWith = binaryReader.ReadUInt16();
			endWith = binaryReader.ReadUInt16();
			startColor = new RGBA();
			startColor.ReadData(binaryReader);
			endColor = new RGBA();
			endColor.ReadData(binaryReader);
		}

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns>Size of this object</returns>
		public static int GetSizeOf()
		{
			return (2 * 2) + (2 * 4);
		}

		/// <summary>
		/// Writes to a binary writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void WriteTo(BinaryWriter writer)
		{
			writer.Write(this.startWith);
			writer.Write(this.endWith);
			this.startColor.WriteTo(writer);
			this.endColor.WriteTo(writer);
		}

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("MorphLineStyle");
			writer.WriteElementString("StartWith", startWith.ToString());
			writer.WriteElementString("EndWith", endWith.ToString());
			this.startColor.Serialize(writer);
			this.endColor.Serialize(writer);
			writer.WriteEndElement();
		}

		#endregion
	}

	/// <summary>
	/// MorphLineStyleCollection
	/// </summary>
	public class MorphLineStyleCollection: CollectionBase, ISwfSerializer
	{
		#region Ctor

		/// <summary>
		/// Creates a new <see cref="MorphLineStyleCollection"/> instance.
		/// </summary>
		public MorphLineStyleCollection()
		{
		}

		/// <summary>
		/// Creates a new <see cref="MorphLineStyleCollection"/> instance.
		/// </summary>
		/// <param name="morphLineStyles">Morph line styles.</param>
		public MorphLineStyleCollection(MorphLineStyle[] morphLineStyles) 
		{ 
			AddRange(morphLineStyles);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Reads the data.
		/// </summary>
		/// <param name="binaryReader">Binary reader.</param>
		public void ReadData(BufferedBinaryReader binaryReader)
		{
			uint count = 0;

			byte lineStyleCount = binaryReader.ReadByte();
			count = lineStyleCount;
            
			ushort lineStyleCountExtended = 0;
			if (lineStyleCount == 0xFF)
			{
				lineStyleCountExtended = binaryReader.ReadUInt16();
				count = lineStyleCountExtended;
			}
			
			if (count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					MorphLineStyle morphLineStyle = new MorphLineStyle();
					morphLineStyle.ReadData(binaryReader);
					this.Add(morphLineStyle);
				}
			}
		}

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns>Size of this object</returns>
		public int GetSizeOf()
		{
			int res = 1;
			int count = this.Count;
			if (count >= 0xFF)
				res += 2;
			res += this.Count * MorphLineStyle.GetSizeOf();
		    return res;
        }

		/// <summary>
		/// Writes to a binary writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void WriteTo(BinaryWriter writer)
		{
			int count = this.Count;
			if (count < 0xFF)
				writer.Write((byte)count);
			else
			{
				writer.Write((byte)(0xFF));
				writer.Write((ushort)count);
			}
			foreach (MorphLineStyle lineStyle in this)
				lineStyle.WriteTo(writer);
		}

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("MorphLineStyleCollection");
			foreach (MorphLineStyle lineStyle in this)
				lineStyle.Serialize(writer);
			writer.WriteEndElement();
		}

		#endregion

		#region Collection methods

		/// <summary>
		/// Adds the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns></returns>
		public MorphLineStyle Add(MorphLineStyle value)
		{
			List.Add(value as object);
			return value;
		}

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="values">Values.</param>
		public void AddRange(MorphLineStyle[] values)
		{
			foreach(MorphLineStyle ip in values)
				Add(ip);
		}

		/// <summary>
		/// Removes the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		public void Remove(MorphLineStyle value)
		{
			if (List.Contains(value))
				List.Remove(value as object);
		}

		/// <summary>
		/// Inserts the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="value">Value.</param>
		public void Insert(int index, MorphLineStyle value)
		{
			List.Insert(index, value as object);
		}

		/// <summary>
		/// Containses the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns></returns>
		public bool Contains(MorphLineStyle value)
		{
			return List.Contains(value as object);
		}

		/// <summary>
		/// Gets or sets the <see cref="LineStyle"/> at the specified index.
		/// </summary>
		/// <value></value>
		public MorphLineStyle this[int index]
		{
			get
			{
				return ((MorphLineStyle)List[index]);
			}
			set
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// Get the index of.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns></returns>
		public int IndexOf(MorphLineStyle value)
		{
			return List.IndexOf(value);
		}

		#endregion
	}
}
