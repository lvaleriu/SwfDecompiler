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
using System.Drawing;
using System.Collections;

using SwfDotNet.IO.Tags;
using SwfDotNet.IO.Utils;

namespace SwfDotNet.IO.Tags.Types
{
	/// <summary>
	/// Rect
	/// </summary>
	public class Rect: SizeStruct, ISwfSerializer
	{
        #region Members

		private int xMin = 0;
		private int xMax = 0;
		private int yMin = 0;
		private int yMax = 0;

        #endregion

        #region Ctor
        
        /// <summary>
        /// Creates a new <see cref="Rect"/> instance.
        /// </summary>
        public Rect()
        {
        }

        /// <summary>
        /// Creates a new <see cref="Rect"/> instance.
        /// </summary>
        /// <param name="xMin">X min (in twips unit: 1 px = 20 twips).</param>
        /// <param name="yMin">Y min (in twips unit: 1 px = 20 twips).</param>
        /// <param name="xMax">X max (in twips unit: 1 px = 20 twips).</param>
        /// <param name="yMax">Y max (in twips unit: 1 px = 20 twips).</param>
        public Rect(int xMin, int yMin, int xMax, int yMax)
        {
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = yMin;
            this.yMax = yMax;
        }

        /// <summary>
        /// Creates a new <see cref="Rect"/> instance.
        /// </summary>
        /// <param name="xMax">X max (in twips unit: 1 px = 20 twips).</param>
        /// <param name="yMax">Y max (in twips unit: 1 px = 20 twips).</param>
        public Rect(int xMax, int yMax)
        {
            this.xMin = 0;
            this.xMax = xMax;
            this.yMin = 0;
            this.yMax = yMax;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the pixel rectangle.
        /// </summary>
        public Rectangle Rectangle
        {
            get 
            {
                int xMinPix = xMin / 20;
                int yMinPix = yMin / 20;
                int xMaxPix = xMax / 20;
                int yMaxPix = yMax / 20;
                return Rectangle.FromLTRB(xMinPix, yMinPix, xMaxPix, yMaxPix);
            }
            set
            {
                this.xMin = value.Left * 20;
                this.yMin = value.Top * 20;
                this.xMax = value.Right * 20;
                this.yMax = value.Bottom * 20;
            }
        }

        /// <summary>
        /// Gets or sets the X min in twips unit.
        /// </summary>
        public int XMin
        {
            get { return this.xMin;  }
            set { this.xMin = value; }
        }

        /// <summary>
        /// Gets or sets the X max in twips unit.
        /// </summary>
        public int XMax
        {
            get { return this.xMax;  }
            set { this.xMax = value; }
        }

        /// <summary>
        /// Gets or sets the Y min in twips unit.
        /// </summary>
        public int YMin
        {
            get { return this.yMin;  }
            set { this.yMin = value; }
        }

        /// <summary>
        /// Gets or sets the Y max in twips unit.
        /// </summary>
        public int YMax
        {
            get { return this.yMax;  }
            set { this.yMax = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data from a binary file
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        public void ReadData(BufferedBinaryReader binaryReader)
        {
            base.SetStartPoint(binaryReader);

			uint nBits = binaryReader.ReadUBits(5);
            xMin = binaryReader.ReadSBits(nBits);
            xMax = binaryReader.ReadSBits(nBits);
            yMin = binaryReader.ReadSBits(nBits);
            yMax = binaryReader.ReadSBits(nBits);

            base.SetEndPoint(binaryReader);
        }

        /// <summary>
        /// Gets the num bits.
        /// </summary>
        /// <returns></returns>
        private uint GetNumBits()
        {
            uint res = 0;
            uint num = BufferedBinaryWriter.GetNumBits(xMin);
            if (num > res)
                res = num;
            num = BufferedBinaryWriter.GetNumBits(xMax);
            if (num > res)
                res = num;
            num = BufferedBinaryWriter.GetNumBits(yMin);
            if (num > res)
                res = num;
            num = BufferedBinaryWriter.GetNumBits(yMax);
            if (num > res)
                res = num;
            return res;
        }

        /// <summary>
        /// Writes to a binary writer.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public void WriteTo(BufferedBinaryWriter writer)
        {
            uint nBits = GetNumBits();
            writer.WriteUBits(nBits, 5);
            writer.WriteSBits(xMin, nBits);
			writer.WriteSBits(xMax, nBits);
			writer.WriteSBits(yMin, nBits);
			writer.WriteSBits(yMax, nBits);
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns>Size of this object</returns>
        public int GetSizeOf()
        {
			int res = 5;
			uint num = GetNumBits();
			res += (int)num * 4;
            res = System.Convert.ToInt32(Math.Ceiling((double) res / 8.0));
            return res;
        }

        #endregion

		#region SwfSerializer Members

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("Rect");
            base.SerializeBinarySize(writer);
			writer.WriteAttributeString("xMin", this.xMin.ToString());
			writer.WriteAttributeString("yMin", this.yMin.ToString());
			writer.WriteAttributeString("xMax", this.xMax.ToString());
			writer.WriteAttributeString("yMax", this.yMax.ToString());
			writer.WriteEndElement();
		}

		#endregion
	}

    /// <summary>
    /// RectCollection class
    /// </summary>
    public class RectCollection: CollectionBase
    {
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="RectCollection"/> instance.
        /// </summary>
        public RectCollection()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        /// <param name="numGlyphs">Num glyphs.</param>
        public void ReadData(BufferedBinaryReader binaryReader, ushort numGlyphs)
        {
            Clear();
            for (int i = 0; i < numGlyphs; i++)
            {
                binaryReader.SynchBits();
                Rect fontBound = new Rect();
                fontBound.ReadData(binaryReader);
                Add(fontBound);
            }
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns></returns>
        public int GetSizeOf()
        {
            int res = 0;
            IEnumerator rects = this.GetEnumerator();
            while (rects.MoveNext())
                res += ((Rect)rects.Current).GetSizeOf();
            return res;
        }

        /// <summary>
        /// Writes to.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public void WriteTo(BufferedBinaryWriter writer)
        {
            IEnumerator rects = this.GetEnumerator();
            while (rects.MoveNext())
            {
                writer.SynchBits();
                ((Rect)rects.Current).WriteTo(writer);
            }
        }

        /// <summary>
        /// Serializes to the specified writer.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public void Serialize(XmlWriter writer)
        {
            writer.WriteStartElement("BoundsTable");
            IEnumerator rects = this.GetEnumerator();
            while (rects.MoveNext())
                ((Rect)rects.Current).Serialize(writer);
            writer.WriteEndElement();
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public Rect Add(Rect value)
        {
            List.Add(value as object);
            return value;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="values">Values.</param>
        public void AddRange(Rect[] values)
        {
            foreach(Rect ip in values)
                Add(ip);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public void Remove(Rect value)
        {
            if (List.Contains(value))
                List.Remove(value as object);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="value">Value.</param>
        public void Insert(int index, Rect value)
        {
            List.Insert(index, value as object);
        }

        /// <summary>
        /// Containses the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public bool Contains(Rect value)
        {
            return List.Contains(value as object);
        }

        /// <summary>
        /// Gets or sets the <see cref="Rect"/> at the specified index.
        /// </summary>
        /// <value></value>
        public Rect this[int index]
        {
            get
            {
                return ((Rect)List[index]);
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
        public int IndexOf(Rect value)
        {
            return List.IndexOf(value);
        }

        #endregion
    }
}
