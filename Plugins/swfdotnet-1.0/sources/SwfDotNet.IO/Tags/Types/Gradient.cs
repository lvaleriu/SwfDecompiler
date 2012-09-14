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
using SwfDotNet.IO.Tags;

namespace SwfDotNet.IO.Tags.Types
{
	/// <summary>
	/// GradientRecordCollection class
	/// </summary>
	public class GradientRecordCollection: CollectionBase, ISwfSerializer
	{
		#region Ctor

		/// <summary>
		/// Creates a new <see cref="GradientRecordCollection"/> instance.
		/// </summary>
		public GradientRecordCollection()
		{
		}

		/// <summary>
		/// Creates a new <see cref="GradientRecordCollection"/> instance.
		/// </summary>
		/// <param name="gradientRecords">Gradient records.</param>
		public GradientRecordCollection(GradientRecord[] gradientRecords)
		{
			this.AddRange(gradientRecords);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Reads the data.
		/// </summary>
		/// <param name="binaryReader">Binary reader.</param>
		/// <param name="shapeType">Shape type.</param>
		public void ReadData(BufferedBinaryReader binaryReader, ShapeType shapeType)
		{
			byte numGradients = binaryReader.ReadByte();
			
			if (numGradients > 0)
			{
				for (int i = 0; i < numGradients; i++)
				{
					GradientRecord gradientRecords = new GradientRecord();
					gradientRecords.ReadData(binaryReader, shapeType);
					this.Add(gradientRecords);
				}
			}
		}

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns>size of this object</returns>
		public int GetSizeOf()
		{
			int res = 1;
            IEnumerator gradients = this.GetEnumerator();
            while (gradients.MoveNext())
                res += ((GradientRecord)gradients.Current).GetSizeOf();
			return res;
		}

		/// <summary>
		/// Writes to a binary file.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void WriteTo(BinaryWriter writer)
		{
			writer.Write((byte)this.Count);

            IEnumerator gradients = this.GetEnumerator();
            while (gradients.MoveNext())
                ((GradientRecord)gradients.Current).WriteTo(writer);
		}

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer) 
		{
			writer.WriteStartElement("GradientCollection");
            IEnumerator gradients = this.GetEnumerator();
            while (gradients.MoveNext())
                ((GradientRecord)gradients.Current).Serialize(writer);
			writer.WriteEndElement();
		}

		#endregion

		#region Collection methods

		/// <summary>
		/// Adds the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns></returns>
		public GradientRecord Add(GradientRecord value)
		{
			List.Add(value as object);
			return value;
		}

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="values">Values.</param>
		public void AddRange(GradientRecord[] values)
		{
			foreach(GradientRecord ip in values)
				Add(ip);
		}

		/// <summary>
		/// Removes the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		public void Remove(GradientRecord value)
		{
			if (List.Contains(value))
				List.Remove(value as object);
		}

		/// <summary>
		/// Inserts the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="value">Value.</param>
		public void Insert(int index, GradientRecord value)
		{
			List.Insert(index, value as object);
		}

		/// <summary>
		/// Containses the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns></returns>
		public bool Contains(GradientRecord value)
		{
			return List.Contains(value as object);
		}

		/// <summary>
		/// Gets or sets the <see cref="LineStyle"/> at the specified index.
		/// </summary>
		/// <value></value>
		public GradientRecord this[int index]
		{
			get
			{
				return ((GradientRecord)List[index]);
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
		public int IndexOf(GradientRecord value)
		{
			return List.IndexOf(value);
		}

		#endregion
	}

	/// <summary>
	/// Gradient Record
	/// </summary>
	public class GradientRecord: SizeStruct, ISwfSerializer
	{
		#region Members

		private byte ratio;
		private RGBColor color;

		#endregion

		#region Ctor

		/// <summary>
		/// Creates a new <see cref="GradientRecord"/> instance.
		/// </summary>
		public GradientRecord()
		{
		}

		/// <summary>
		/// Creates a new <see cref="GradientRecord"/> instance.
		/// </summary>
		/// <param name="ratio">Ratio.</param>
		/// <param name="color">Color.</param>
		public GradientRecord(byte ratio, RGBColor color)
		{
			this.ratio = ratio;
			this.color = color;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the ratio.
		/// </summary>
		public byte Ratio
		{
			get { return this.ratio;  }
			set { this.ratio = value; }
		}

		/// <summary>
		/// Gets or sets the color.
		/// </summary>
		public RGBColor Color
		{
			get { return this.color;  }
			set { this.color = value; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Reads the data.
		/// </summary>
		/// <param name="binaryReader">Binary reader.</param>
		/// <param name="shapeType">Shape type.</param>
		public void ReadData(BufferedBinaryReader binaryReader, ShapeType shapeType)
		{
            base.SetStartPoint(binaryReader);
			ratio = binaryReader.ReadByte();
			color = null;
			if (shapeType == ShapeType.Shape3)
			{
				color = new RGBA();
				color.ReadData(binaryReader);
			}
			else if (shapeType == ShapeType.Shape || shapeType == ShapeType.Shape2)
			{
				color = new RGB();
				color.ReadData(binaryReader);
			}
            base.SetEndPoint(binaryReader);
		}

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns>size of this object type</returns>
		public int GetSizeOf()
		{
            int res = 1;
            if (color != null)
                res += color.GetSizeOf();
			return res;
		}

		/// <summary>
		/// Writes to a binary writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void WriteTo(BinaryWriter writer)
		{
			writer.Write(this.ratio);
            if (color != null)
			    this.color.WriteTo(writer);
		}

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer) 
		{
			writer.WriteStartElement("GradientRecord");
            base.SerializeBinarySize(writer);
			writer.WriteAttributeString("Ratio", this.ratio.ToString());
			this.color.Serialize(writer);
			writer.WriteEndElement();
		}

		#endregion
	}
}
