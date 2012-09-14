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
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace SwfDotNet.IO.Tags.Types
{
	/// <summary>
	/// ColorMapData
	/// </summary>
	public class ColorMapData: ISwfSerializer
	{
		#region Members

		private RGB[] colorTableRGB = null;
		private byte[] colorMapPixelData = null;

		#endregion

		#region Ctor

		/// <summary>
		/// Creates a new <see cref="ColorMapData"/> instance.
		/// </summary>
		public ColorMapData()
		{
		}

		/// <summary>
		/// Creates a new <see cref="ColorMapData"/> instance.
		/// </summary>
		/// <param name="colorTableRGB">Color table RGB.</param>
		/// <param name="colorMapPixelData">Color map pixel data.</param>
		public ColorMapData(RGB[] colorTableRGB, byte[] colorMapPixelData) 
		{ 
			this.colorTableRGB = colorTableRGB;
			this.colorMapPixelData = colorMapPixelData;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the color map pixel data.
		/// </summary>
		public byte[] ColorMapPixelData
		{
			get { return this.colorMapPixelData;  }
			set { this.colorMapPixelData = value; }
		}

		/// <summary>
		/// Gets or sets the color table RGB.
		/// </summary>
		public RGB[] ColorTableRGB
		{
			get { return this.colorTableRGB;  }
			set { this.colorTableRGB = value; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Reads the data.
		/// </summary>
		/// <param name="reader">Reader.</param>
		/// <param name="bitmapColorTableSize">Size of the bitmap color table.</param>
		/// <param name="bitmapWidth">Width of the bitmap.</param>
		/// <param name="bitmapHeight">Height of the bitmap.</param>
		/// <param name="toRead">To read.</param>
		public void ReadData(BufferedBinaryReader reader, byte bitmapColorTableSize, 
			ushort bitmapWidth, ushort bitmapHeight, int toRead)
		{
			int size = ((bitmapColorTableSize + 1) * 3) + (bitmapWidth * bitmapHeight);
			byte[] uncompressed = new byte[size];

			byte[] compressed = reader.ReadBytes(toRead);	
			Inflater zipInflator = 	new Inflater();
			zipInflator.SetInput(compressed);
			zipInflator.Inflate(uncompressed, 0, size);
            
			int readed = 0;
			int offset = size;

			colorTableRGB = new RGB[bitmapColorTableSize + 1];
			for (int i = 0; i < bitmapColorTableSize + 1; i++)
			{
				byte red = uncompressed[readed];
				readed++;
				byte green = uncompressed[readed];
				readed++;
				byte blue = uncompressed[readed];
				readed++;
				colorTableRGB[i] = new RGB(red, green, blue);
				offset -= 3;
			}

			colorMapPixelData = new byte[offset];
			for (int i = 0; i < offset; i++, readed++)
				colorMapPixelData[i] = uncompressed[readed];
		}

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns>size of this type</returns>
		public int GetSizeOf()
		{
			int res = 0;
			if (colorTableRGB != null)
				res += colorTableRGB.Length * 3;
			if (colorMapPixelData != null)
				res += colorMapPixelData.Length * 1;
			return res;
		}

		/// <summary>
		/// Writes to a binary writer
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void WriteTo(BinaryWriter writer)
		{
            if (colorTableRGB != null)
			{
                IEnumerator enums = colorTableRGB.GetEnumerator();
                while(enums.MoveNext())
                    ((RGB)enums.Current).WriteTo(writer);
			}
			writer.Write(colorMapPixelData);
		}

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("ColorMapData");
			writer.WriteEndElement();
		}

		#endregion
	}

	/// <summary>
	/// AlphaColorMapData
	/// </summary>
	public class AlphaColorMapData: ISwfSerializer
	{
        #region Members

		private RGBA[] colorTableRgb;
		private byte[] colorMapPixelData;
		
        #endregion
            
        #region Ctor

		/// <summary>
		/// Creates a new <see cref="AlphaColorMapData"/> instance.
		/// </summary>
		public AlphaColorMapData() 
		{ 
		}

		/// <summary>
		/// Creates a new <see cref="AlphaColorMapData"/> instance.
		/// </summary>
		/// <param name="colorTableRgb">Color table RGB.</param>
		/// <param name="colorMapPixelData">Color map pixel data.</param>
		public AlphaColorMapData(RGBA[] colorTableRgb, byte[] colorMapPixelData) 
		{ 
			this.colorTableRgb = colorTableRgb;
			this.colorMapPixelData = colorMapPixelData;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color table RGBA.
        /// </summary>
        public RGBA[] ColorTableRgb
        {
            get { return this.colorTableRgb;  }
            set { this.colorTableRgb = value; }
        }

        /// <summary>
        /// Gets or sets the color map pixel data.
        /// </summary>
        public byte[] ColorMapPixelData
        {
            get { return this.colorMapPixelData;  }
            set { this.colorMapPixelData = value; }
        }

        #endregion

        #region Methods

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns>size of this type</returns>
		public int GetSizeOf()
		{
			int res = 0;
			if (colorTableRgb != null)
				res += colorTableRgb.Length * 4;
			if (colorMapPixelData != null)
				res += colorMapPixelData.Length * 1;
			return res;
		}

		/// <summary>
		/// Writes to a binary writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void WriteTo(BinaryWriter writer)
		{
			if (colorTableRgb != null)
			{
                IEnumerator enums = colorTableRgb.GetEnumerator();
                while(enums.MoveNext())
                    ((RGBA)enums.Current).WriteTo(writer);
			}
			writer.Write(colorMapPixelData);
		}

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("AlphaColorMapData");
			//foreach (RGBA rgb in colorTableRgb)
			//	rgb.Serialize(writer);
			writer.WriteEndElement();
		}

        #endregion
	}

	/// <summary>
	/// AlphaBitmapData
	/// </summary>
	public class AlphaBitmapData: ISwfSerializer
	{
        #region Members

		private RGBA[] bitmapPixelData;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="AlphaBitmapData"/> instance.
        /// </summary>
		public AlphaBitmapData() 
        { 
        }

        /// <summary>
        /// Creates a new <see cref="AlphaBitmapData"/> instance.
        /// </summary>
        /// <param name="bitmapPixelData">Bitmap pixel data.</param>
        public AlphaBitmapData(RGBA[] bitmapPixelData)
        {
            this.bitmapPixelData = bitmapPixelData;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the bitmap pixel data.
        /// </summary>
        public RGBA[] BitmapPixelData
        {
            get { return this.bitmapPixelData;  }
            set { this.bitmapPixelData = value; }
        }

        #endregion

        #region Methods

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns>size of this type</returns>
		public int GetSizeOf()
		{
			int res = 0;
			if (bitmapPixelData != null)
				res += bitmapPixelData.Length * 4;
			return res;
		}

		/// <summary>
		/// Writes to a binary writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void WriteTo(BinaryWriter writer)
		{
			if (bitmapPixelData != null)
			{
                IEnumerator enums = bitmapPixelData.GetEnumerator();
				while(enums.MoveNext())
                    ((RGBA)enums.Current).WriteTo(writer);
			}
		}

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("AlphaBitmapData");
            /*
            if (bitmapPixelData != null)
            {
                IEnumerator enums = bitmapPixelData.GetEnumerator();
                while(enums.MoveNext())
                    ((RGBA)enums.Current).Serialize(writer);
            }
            */
			writer.WriteEndElement();
		}

        #endregion
	}
}
