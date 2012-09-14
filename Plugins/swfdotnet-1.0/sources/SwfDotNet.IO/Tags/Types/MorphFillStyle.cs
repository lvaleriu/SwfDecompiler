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
	#region Enum

	/// <summary>
	/// MorphFillStyleType enumeration
	/// </summary>
	public enum MorphFillStyleType
	{
		/// <summary>
		/// Solid Fill
		/// </summary>
        SolidFill = 0x00,
        /// <summary>
        /// Liear Gradient Fill
        /// </summary>
		LinearGradientFill = 0x10,
        /// <summary>
        /// Radial Gradient Fill
        /// </summary>
		RadialGradientFill = 0x12,
        /// <summary>
        /// Repeating Bitmap
        /// </summary>
		RepeatingBitmap = 0x40,
		/// <summary>
		/// Clipped Bitmap Fill
		/// </summary>
        ClippedBitmapFill = 0x41,
        /// <summary>
        /// Non-Smoothed Repeating Bitmap
        /// </summary>
		NonSmoothedRepeatingBitmap = 0x42,
        /// <summary>
        /// Non-Smoothed Clipped Bitmap
        /// </summary>
		NonSmoothedClippedBitmap = 0x43
	}

	#endregion

	/// <summary>
	/// MorphFillStyle
	/// </summary>
	public abstract class MorphFillStyle: ISwfSerializer
	{
        #region Members
		
		/// <summary>
		/// Fill Style Type
		/// </summary>
		protected byte fillStyleType;
		/// <summary>
		/// Start Color
		/// </summary>
		protected RGBA startColor;
		/// <summary>
		/// End Color
		/// </summary>
		protected RGBA endColor;
		/// <summary>
		/// Start Gradient Matrix
		/// </summary>
		protected Matrix startGradientMatrix = null;
		/// <summary>
		/// End Gradient Matrix
		/// </summary>
		protected Matrix endGradientMatrix = null;
		/// <summary>
		/// Morph Gradients collection
		/// </summary>
		protected MorphGradientCollection gradient;
		/// <summary>
		/// Bitmap Id
		/// </summary>
		protected ushort bitmapId;
		/// <summary>
		/// Start Bitmap Matrix
		/// </summary>
		protected Matrix startBitmapMatrix = null; 
		/// <summary>
		/// End Bitmap Matrix
		/// </summary>
		protected Matrix endBitmapMatrix = null; 

        #endregion
    
        #region Properties

        /// <summary>
        /// Gets or sets the fill style type.
        /// </summary>
        public MorphFillStyleType FillStyleType
        {
            get { return (MorphFillStyleType)this.fillStyleType;  }
            set { this.fillStyleType = (byte)value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        public void ReadData(BufferedBinaryReader binaryReader)
        {
            fillStyleType = binaryReader.ReadByte();
			
            startColor = null;
            endColor = null;
            if (fillStyleType == (byte)MorphFillStyleType.SolidFill)
            {
                startColor = new RGBA();
                startColor.ReadData(binaryReader);
                endColor = new RGBA();
                endColor.ReadData(binaryReader);
            }

            startGradientMatrix = null;
            endGradientMatrix = null;
            MorphGradientCollection gradient = null;
            if (fillStyleType == (byte)MorphFillStyleType.LinearGradientFill || 
                fillStyleType == (byte)MorphFillStyleType.RadialGradientFill)
            {
                startGradientMatrix = new Matrix();
                startGradientMatrix.ReadData(binaryReader);
                endGradientMatrix = new Matrix();
                endGradientMatrix.ReadData(binaryReader);
                gradient = new MorphGradientCollection();
                gradient.ReadData(binaryReader);
            }

            bitmapId = 0;
            startBitmapMatrix = null;
            endBitmapMatrix = null;
            if (fillStyleType == (byte)MorphFillStyleType.RepeatingBitmap || 
                fillStyleType == (byte)MorphFillStyleType.ClippedBitmapFill ||
                fillStyleType == (byte)MorphFillStyleType.NonSmoothedClippedBitmap ||
                fillStyleType == (byte)MorphFillStyleType.NonSmoothedRepeatingBitmap)
            {
                bitmapId = binaryReader.ReadUInt16();
                startBitmapMatrix = new Matrix();
                startBitmapMatrix.ReadData(binaryReader);
                endBitmapMatrix = new Matrix();
                endBitmapMatrix.ReadData(binaryReader);
            }
        }

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns>size of this object</returns>
		public int GetSizeOf()
		{
			int res = 1;
			if (fillStyleType == (byte)MorphFillStyleType.SolidFill)
			{
				if (startColor != null)
					res += this.startColor.GetSizeOf();
				if (endColor != null)
					res += this.endColor.GetSizeOf();
			}
			if (fillStyleType == (byte)MorphFillStyleType.LinearGradientFill || 
				fillStyleType == (byte)MorphFillStyleType.RadialGradientFill)
			{
				if (startGradientMatrix != null)
					res += startGradientMatrix.GetSizeOf();
				if (endGradientMatrix != null)
					res += endGradientMatrix.GetSizeOf();
				if (gradient != null)
					res += gradient.GetSizeOf();
			}
			if (fillStyleType == (byte)MorphFillStyleType.LinearGradientFill || 
				fillStyleType == (byte)MorphFillStyleType.RadialGradientFill ||
				fillStyleType == (byte)MorphFillStyleType.RadialGradientFill ||
				fillStyleType == (byte)MorphFillStyleType.RadialGradientFill)
			{
				res += 2;
				if (startBitmapMatrix != null)
					res += startBitmapMatrix.GetSizeOf();
				if (endBitmapMatrix != null)
					res += endBitmapMatrix.GetSizeOf();
			}
			return res;
		}

		/// <summary>
		/// Writes to a binary writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void WriteTo(BufferedBinaryWriter writer)
		{
			writer.Write(this.fillStyleType);
			if (fillStyleType == (byte)MorphFillStyleType.SolidFill)
			{
				if (startColor != null)
					this.startColor.WriteTo(writer);
				if (endColor != null)
					this.endColor.WriteTo(writer);
			}
			if (fillStyleType == (byte)MorphFillStyleType.LinearGradientFill || 
				fillStyleType == (byte)MorphFillStyleType.RadialGradientFill)
			{
				if (startGradientMatrix != null)
					startGradientMatrix.WriteTo(writer);
				if (endGradientMatrix != null)
					endGradientMatrix.WriteTo(writer);
				if (gradient != null)
					gradient.WriteTo(writer);
			}
			if (fillStyleType == (byte)MorphFillStyleType.LinearGradientFill || 
				fillStyleType == (byte)MorphFillStyleType.RadialGradientFill ||
				fillStyleType == (byte)MorphFillStyleType.RadialGradientFill ||
				fillStyleType == (byte)MorphFillStyleType.RadialGradientFill)
			{
				writer.Write(this.bitmapId);
				if (startBitmapMatrix != null)
					startBitmapMatrix.WriteTo(writer);
				if (endBitmapMatrix != null)
					endBitmapMatrix.WriteTo(writer);
			}
		}

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public abstract void Serialize(XmlWriter writer);

        #endregion
	}

	/// <summary>
	/// MorphSolidFill
	/// </summary>
	public class MorphSolidFill: MorphFillStyle
	{
		#region Ctor

		/// <summary>
		/// Creates a new <see cref="MorphSolidFill"/> instance.
		/// </summary>
		public MorphSolidFill()
		{
			this.fillStyleType = (byte)MorphFillStyleType.SolidFill;
		}

		/// <summary>
		/// Creates a new <see cref="MorphSolidFill"/> instance.
		/// </summary>
		/// <param name="startColor">Color of the start.</param>
		/// <param name="endColor">Color of the end.</param>
		public MorphSolidFill(RGBA startColor, RGBA endColor)
		{
			this.startColor = startColor;
			this.endColor = endColor;
			this.fillStyleType = (byte)MorphFillStyleType.SolidFill;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the color of the start.
		/// </summary>
		public RGBA StartColor
		{
			get { return this.startColor;  }
			set { this.startColor = value; }
		}

		/// <summary>
		/// Gets or sets the color of the end.
		/// </summary>
		public RGBA EndColor
		{
			get { return this.endColor;  }
			set { this.endColor = value; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("MorphSolidFill");
			if (this.startColor != null)
				this.startColor.Serialize(writer);
			if (this.endColor != null)
				this.endColor.Serialize(writer);
			writer.WriteEndElement();
		}

		#endregion
	}

	/// <summary>
	/// MorphGradientFill
	/// </summary>
	public class MorphGradientFill: MorphFillStyle
	{
		#region Ctor

		/// <summary>
		/// Creates a new <see cref="MorphGradientFill"/> instance.
		/// </summary>
		/// <param name="fillStyleType">Fill style type.</param>
		public MorphGradientFill(MorphFillStyleType fillStyleType)
		{
			this.fillStyleType = (byte)fillStyleType;
		}

		/// <summary>
		/// Creates a new <see cref="MorphGradientFill"/> instance.
		/// </summary>
		/// <param name="fillStyleType">Fill style type.</param>
		public MorphGradientFill(byte fillStyleType)
		{
			this.fillStyleType = fillStyleType;
		}

		/// <summary>
		/// Creates a new <see cref="MorphGradientFill"/> instance.
		/// </summary>
		/// <param name="fillStyleType">Fill style type.</param>
		/// <param name="startGradientMatrix">Start gradient matrix.</param>
		/// <param name="endGradientMatrix">End gradient matrix.</param>
		/// <param name="gradients">Gradients.</param>
		public MorphGradientFill(MorphFillStyleType fillStyleType, Matrix startGradientMatrix,
			Matrix endGradientMatrix, MorphGradientCollection gradients)
		{
			this.fillStyleType = (byte)fillStyleType;
			this.startGradientMatrix = startGradientMatrix;
			this.endGradientMatrix = endGradientMatrix;
			this.gradient = gradients;
		}

		/// <summary>
		/// Creates a new <see cref="MorphGradientFill"/> instance.
		/// </summary>
		/// <param name="fillStyleType">Fill style type.</param>
		/// <param name="startGradientMatrix">Start gradient matrix.</param>
		/// <param name="endGradientMatrix">End gradient matrix.</param>
		/// <param name="gradients">Gradients.</param>
		public MorphGradientFill(byte fillStyleType, Matrix startGradientMatrix,
			Matrix endGradientMatrix, MorphGradientCollection gradients)
		{
			this.fillStyleType = fillStyleType;
			this.startGradientMatrix = startGradientMatrix;
			this.endGradientMatrix = endGradientMatrix;
			this.gradient = gradients;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the start gradient matrix.
		/// </summary>
		public Matrix StartGradientMatrix
		{
			get { return this.startGradientMatrix;  }
			set { this.startGradientMatrix = value; }
		}

		/// <summary>
		/// Gets or sets the end gradient matrix.
		/// </summary>
		public Matrix EndGradientMatrix
		{
			get { return this.endGradientMatrix;  }
			set { this.endGradientMatrix = value; }
		}

		/// <summary>
		/// Gets or sets the gradient.
		/// </summary>
		public MorphGradientCollection Gradients
		{
			get { return this.gradient;  }
			set { this.gradient = value; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("MorphGradientFill");
			if (this.startGradientMatrix != null)
				this.startGradientMatrix.Serialize(writer);
			if (this.endGradientMatrix != null)
				this.endGradientMatrix.Serialize(writer);
			if (this.gradient != null)
				this.gradient.Serialize(writer);
			writer.WriteEndElement();
		}

		#endregion
	}

	/// <summary>
	/// MorphBitmapFill
	/// </summary>
	public class MorphBitmapFill: MorphFillStyle
	{
		#region Ctor

		/// <summary>
		/// Creates a new <see cref="MorphBitmapFill"/> instance.
		/// </summary>
		/// <param name="fillStyleType">Fill style type.</param>
		public MorphBitmapFill(MorphFillStyleType fillStyleType)
		{
			this.fillStyleType = (byte)fillStyleType;
		}

		/// <summary>
		/// Creates a new <see cref="MorphBitmapFill"/> instance.
		/// </summary>
		/// <param name="fillStyleType">Fill style type.</param>
		public MorphBitmapFill(byte fillStyleType)
		{
			this.fillStyleType = fillStyleType;
		}

		/// <summary>
		/// Creates a new <see cref="MorphBitmapFill"/> instance.
		/// </summary>
		/// <param name="fillStyleType">Fill style type.</param>
		/// <param name="bitmapId">Bitmap id.</param>
		/// <param name="startBitmapMatrix">Start bitmap matrix.</param>
		/// <param name="endBitmapMatrix">End bitmap matrix.</param>
		public MorphBitmapFill(MorphFillStyleType fillStyleType, ushort bitmapId, 
			Matrix startBitmapMatrix, Matrix endBitmapMatrix) 
		{
			this.fillStyleType = (byte)fillStyleType;
			this.bitmapId = bitmapId;
			this.startBitmapMatrix = startBitmapMatrix;
			this.endBitmapMatrix = endBitmapMatrix;
		}

		/// <summary>
		/// Creates a new <see cref="MorphBitmapFill"/> instance.
		/// </summary>
		/// <param name="fillStyleType">Fill style type.</param>
		/// <param name="bitmapId">Bitmap id.</param>
		/// <param name="startBitmapMatrix">Start bitmap matrix.</param>
		/// <param name="endBitmapMatrix">End bitmap matrix.</param>
		public MorphBitmapFill(byte fillStyleType, ushort bitmapId, 
			Matrix startBitmapMatrix, Matrix endBitmapMatrix) 
		{
			this.fillStyleType = fillStyleType;
			this.bitmapId = bitmapId;
			this.startBitmapMatrix = startBitmapMatrix;
			this.endBitmapMatrix = endBitmapMatrix;
		}
	
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the bitmap id.
		/// </summary>
		public ushort BitmapId
		{
			get { return this.bitmapId;  }
			set { this.bitmapId = value; }
		}

		/// <summary>
		/// Gets or sets the start bitmap matrix.
		/// </summary>
		public Matrix StartBitmapMatrix
		{
			get { return this.startBitmapMatrix;  }
			set { this.startBitmapMatrix = value; }
		}

		/// <summary>
		/// Gets or sets the end bitmap matrix.
		/// </summary>
		public Matrix EndBitmapMatrix
		{
			get { return this.endBitmapMatrix;  }
			set { this.endBitmapMatrix = value; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("MorphBitmapFill");
			writer.WriteElementString("BitmapId", bitmapId.ToString());
			if (this.startBitmapMatrix != null)
				this.startBitmapMatrix.Serialize(writer);
			if (this.endBitmapMatrix != null)
				this.endBitmapMatrix.Serialize(writer);
			writer.WriteEndElement();
		}

		#endregion
	}

	/// <summary>
	/// MorphFillStyleCollection
	/// </summary>
	public class MorphFillStyleCollection: CollectionBase, ISwfSerializer
	{
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="MorphFillStyleCollection"/> instance.
        /// </summary>
        public MorphFillStyleCollection()
        {
        }

        /// <summary>
        /// Creates a new <see cref="MorphFillStyleCollection"/> instance.
        /// </summary>
        /// <param name="fillStyles">Fill styles.</param>
		public MorphFillStyleCollection(MorphFillStyle[] fillStyles) 
		{ 
			if (fillStyles != null)
                AddRange(fillStyles);
		}

        #endregion

        #region Methods

		/// <summary>
		/// Gets the moprh fill style object from type.
		/// </summary>
		/// <param name="fillStyleType">Fill style type.</param>
		/// <returns></returns>
		private MorphFillStyle GetMorphFillStyleFromType(byte fillStyleType)
		{
			if (fillStyleType == (byte)MorphFillStyleType.SolidFill)
				return new MorphSolidFill();
            
			if (fillStyleType == (byte)MorphFillStyleType.RadialGradientFill ||
				fillStyleType == (byte)MorphFillStyleType.LinearGradientFill)
			{
				return new MorphGradientFill(fillStyleType); 
			}

			if (fillStyleType == (byte)MorphFillStyleType.RepeatingBitmap ||
				fillStyleType == (byte)MorphFillStyleType.ClippedBitmapFill ||
				fillStyleType == (byte)MorphFillStyleType.NonSmoothedClippedBitmap ||
				fillStyleType == (byte)MorphFillStyleType.NonSmoothedRepeatingBitmap)
			{
				return new MorphBitmapFill(fillStyleType);
			}
			return null;
		}

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        public void ReadData(BufferedBinaryReader binaryReader)
        {
            uint count = 0;
            byte fillStyleCount = binaryReader.ReadByte();
            count = fillStyleCount;

            ushort fillStyleCountExtended = 0;
            if (fillStyleCount == 0xFF)
            {
                fillStyleCountExtended = binaryReader.ReadUInt16();
                count = fillStyleCountExtended;
            }

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
					byte fillStyleType = binaryReader.PeekByte();
					MorphFillStyle morphFillStyle = GetMorphFillStyleFromType(fillStyleType);
					if (morphFillStyle != null)
					{
						morphFillStyle.ReadData(binaryReader);
						this.Add(morphFillStyle);
					}
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
			int count = this.Count;
			if (count >= 0xFF)
				res += 2;
			foreach (MorphFillStyle mFillStyle in this)
				res += mFillStyle.GetSizeOf();
            return res;
		}

		/// <summary>
		/// Writes to binary writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void WriteTo(BufferedBinaryWriter writer)
		{
			int count = this.Count;
			if (count < 0xFF)
				writer.Write((byte)count);
			else
			{
				writer.Write((byte)(0xFF));
				writer.Write((ushort)count);
			}
			foreach (MorphFillStyle mFillStyle in this)
				mFillStyle.WriteTo(writer);
		}

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("MorphFillStyleCollection");
			foreach (MorphFillStyle mFillStyle in this)
				mFillStyle.Serialize(writer);
			writer.WriteEndElement();
		}

        #endregion

        #region Collection methods

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public MorphFillStyle Add(MorphFillStyle value)
        {
            List.Add(value as object);
            return value;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="values">Values.</param>
        public void AddRange(MorphFillStyle[] values)
        {
            foreach(MorphFillStyle ip in values)
                Add(ip);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public void Remove(MorphFillStyle value)
        {
            if (List.Contains(value))
                List.Remove(value as object);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="value">Value.</param>
        public void Insert(int index, MorphFillStyle value)
        {
            List.Insert(index, value as object);
        }

        /// <summary>
        /// Containses the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public bool Contains(MorphFillStyle value)
        {
            return List.Contains(value as object);
        }

        /// <summary>
        /// Gets or sets the <see cref="LineStyle"/> at the specified index.
        /// </summary>
        /// <value></value>
        public MorphFillStyle this[int index]
        {
            get
            {
                return ((MorphFillStyle)List[index]);
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
        public int IndexOf(MorphFillStyle value)
        {
            return List.IndexOf(value);
        }

        #endregion
	}
}
