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
using log4net;

using SwfDotNet.IO.Utils;
using SwfDotNet.IO.Tags;

namespace SwfDotNet.IO.Tags.Types
{
    /// <summary>
    /// GlyphShapesCollection
    /// </summary>
    public class GlyphShapesCollection: CollectionBase, ISwfSerializer
    {
        #region Members

        private static readonly ILog log = LogManager.GetLogger(typeof(ShapeRecordCollection));
	
        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="GlyphShapesCollection"/> instance.
        /// </summary>
        public GlyphShapesCollection()
        {
        }

        /// <summary>
        /// Creates a new <see cref="ShapeRecordCollection"/> instance.
        /// </summary>
        /// <param name="shapes">Shapes.</param>
        public GlyphShapesCollection(ShapeRecordCollection[] shapes)
        {
            if (shapes != null)
                AddRange(shapes);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns></returns>
        public int GetSizeOf()
        {
            int res = 0;
       
            IEnumerator shapes = this.GetEnumerator();
            while (shapes.MoveNext())
                res += ((ShapeRecordCollection)shapes.Current).GetSizeOf();
            return res;
        }

        /// <summary>
        /// Writes to.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public void WriteTo(BufferedBinaryWriter writer)
        {
            IEnumerator shapes = this.GetEnumerator();
            while (shapes.MoveNext())
                ((ShapeRecordCollection)shapes.Current).WriteTo(writer);
        }

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="reader">Reader.</param>
        /// <param name="numGlyphs">Num glyphs.</param>
        public void ReadData(BufferedBinaryReader reader, ushort numGlyphs)
        {
            for (int i = 0; i < numGlyphs; i++)
            {
                ShapeRecordCollection glyphShape = new ShapeRecordCollection();
                glyphShape.ReadData(reader, ShapeType.None);
                this.Add(glyphShape);
            }
        }

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer) 
		{
            writer.WriteStartElement("GlyphShapesTable");

            IEnumerator shapes = this.GetEnumerator();
            while (shapes.MoveNext())
                ((ShapeRecordCollection)shapes.Current).Serialize(writer);
            
            writer.WriteEndElement();
		}

        #endregion

        #region Collection methods

        /// <summary>
        /// Gets the last ShapeRecordCollection of the collection.
        /// </summary>
        /// <returns></returns>
        public ShapeRecordCollection GetLastOne()
        {
            if (this.Count == 0)
                return null;

            return this[this.Count - 1];
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public ShapeRecordCollection Add(ShapeRecordCollection value)
        {
            List.Add(value as object);
            return value;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="values">Values.</param>
        public void AddRange(ShapeRecordCollection[] values)
        {
            if  (values == null)
                return;

            IEnumerator enu = values.GetEnumerator();
            while (enu.MoveNext())
                Add((ShapeRecordCollection)enu.Current);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public void Remove(ShapeRecordCollection value)
        {
            if (List.Contains(value))
                List.Remove(value as object);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="value">Value.</param>
        public void Insert(int index, ShapeRecordCollection value)
        {
            List.Insert(index, value as object);
        }

        /// <summary>
        /// Containses the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public bool Contains(ShapeRecordCollection value)
        {
            return List.Contains(value as object);
        }

        /// <summary>
        /// Gets or sets the <see cref="LineStyle"/> at the specified index.
        /// </summary>
        /// <value></value>
        public ShapeRecordCollection this[int index]
        {
            get
            {
                return ((ShapeRecordCollection)List[index]);
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
        public int IndexOf(ShapeRecordCollection value)
        {
            return List.IndexOf(value);
        }

        #endregion
    }
}
