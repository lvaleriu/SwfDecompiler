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

using SwfDotNet.IO.Utils;
using SwfDotNet.IO.Tags;

namespace SwfDotNet.IO.Tags.Types
{
	/// <summary>
	/// ShapeWithStyle class
	/// </summary>
    public class ShapeWithStyle : SizeStruct, ISwfSerializer
	{
		#region Members

		private FillStyleCollection fillStyleArray = null;
		private LineStyleCollection lineStyleArray = null;
		private ShapeRecordCollection shapes = null;

        /// <summary>
        /// Current number of fill bits
        /// </summary>
        public static uint NumFillBits;

        /// <summary>
        /// Current number of line bits
        /// </summary>
        public static uint NumLineBits;

		#endregion

        #region Ctor & Init

		/// <summary>
		/// Creates a new <see cref="ShapeWithStyle"/> instance.
		/// </summary>
		public ShapeWithStyle()
		{
            Init();
		}
			
		/// <summary>
		/// Creates a new <see cref="ShapeWithStyle"/> instance.
		/// </summary>
		/// <param name="fillStyleArray">Fill style array.</param>
		/// <param name="lineStyleArray">Line style array.</param>
		/// <param name="shapes">Shapes.</param>
		public ShapeWithStyle(FillStyleCollection fillStyleArray, 
			LineStyleCollection lineStyleArray, ShapeRecordCollection shapes)
		{
			this.fillStyleArray = fillStyleArray;
			this.lineStyleArray = lineStyleArray;
			this.shapes = shapes;
		}

        /// <summary>
        /// Inits this instance.
        /// </summary>
        private void Init()
        {
            this.fillStyleArray = new FillStyleCollection();
            this.lineStyleArray = new LineStyleCollection();
            this.shapes = new ShapeRecordCollection();
        }

		#endregion

        #region Properties

        /// <summary>
        /// Gets the fill style array.
        /// </summary>
        /// <value></value>
        public FillStyleCollection FillStyleArray
        {
            get { return this.fillStyleArray;  }
        }
        
        /// <summary>
        /// Gets the line style array.
        /// </summary>
        /// <value></value>
        public LineStyleCollection LineStyleArray
        {
            get { return this.lineStyleArray;  }
        }

        /// <summary>
        /// Gets the shapes.
        /// </summary>
        /// <value></value>
        public ShapeRecordCollection Shapes
        {
            get { return this.shapes;  }
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

            fillStyleArray = new FillStyleCollection();
            fillStyleArray.ReadData(binaryReader, shapeType);

            lineStyleArray = new LineStyleCollection();
            lineStyleArray.ReadData(binaryReader, shapeType);
			
            shapes = new ShapeRecordCollection();
            shapes.ReadData(binaryReader, shapeType);

            base.SetEndPoint(binaryReader);
		}

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns>Size of this object.</returns>
		public int GetSizeOf()
		{
			int res = 0;
			if (fillStyleArray != null)
				res += fillStyleArray.GetSizeOf();
			if (lineStyleArray != null)
				res += lineStyleArray.GetSizeOf();
            if (shapes != null)
            {
                ShapeWithStyle.NumFillBits = 0;
                ShapeWithStyle.NumLineBits = 0;
                if (fillStyleArray != null && fillStyleArray.Count != 0)
                    ShapeWithStyle.NumFillBits = BufferedBinaryWriter.GetNumBits((uint)fillStyleArray.Count);
                if (lineStyleArray != null && lineStyleArray.Count != 0)
                    ShapeWithStyle.NumLineBits = BufferedBinaryWriter.GetNumBits((uint)lineStyleArray.Count);
                res += shapes.GetSizeOf();
                ShapeWithStyle.NumFillBits = 0;
                ShapeWithStyle.NumLineBits = 0;
            }
            return res;
		}

		/// <summary>
		/// Writes to a binary writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void WriteTo(BufferedBinaryWriter writer)
		{
			if (fillStyleArray != null)
				fillStyleArray.WriteTo(writer);
			if (lineStyleArray != null)
				lineStyleArray.WriteTo(writer);
            if (shapes != null)
            {
                ShapeWithStyle.NumFillBits = 0;
                ShapeWithStyle.NumLineBits = 0;
                if (fillStyleArray != null && fillStyleArray.Count != 0)
                    ShapeWithStyle.NumFillBits = BufferedBinaryWriter.GetNumBits((uint)fillStyleArray.Count);
                if (lineStyleArray != null && lineStyleArray.Count != 0)
                    ShapeWithStyle.NumLineBits = BufferedBinaryWriter.GetNumBits((uint)lineStyleArray.Count);
                shapes.WriteTo(writer);
                ShapeWithStyle.NumFillBits = 0;
                ShapeWithStyle.NumLineBits = 0;
            }
		}

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("ShapeWithStyle");
            base.SerializeBinarySize(writer);

            if (fillStyleArray != null)
				fillStyleArray.Serialize(writer);
			if (lineStyleArray != null)
				lineStyleArray.Serialize(writer);
			if (shapes != null)
			{
				ShapeWithStyle.NumFillBits = 0;
				ShapeWithStyle.NumLineBits = 0;
				if (fillStyleArray != null && fillStyleArray.Count != 0)
					ShapeWithStyle.NumFillBits = BufferedBinaryWriter.GetNumBits((uint)fillStyleArray.Count);
				if (lineStyleArray != null && lineStyleArray.Count != 0)
					ShapeWithStyle.NumLineBits = BufferedBinaryWriter.GetNumBits((uint)lineStyleArray.Count);
                
				shapes.Serialize(writer);

				ShapeWithStyle.NumFillBits = 0;
				ShapeWithStyle.NumLineBits = 0;
			}
			writer.WriteEndElement();
		}

		#endregion
	}
}
