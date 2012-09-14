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
using System.Xml;
using System.Collections;

using SwfDotNet.IO.Utils;

namespace SwfDotNet.IO.Tags.Types
{
	/// <summary>
	/// GlyphShapesTable.
	/// </summary>
	public class GlyphShapesTable: Hashtable
	{
        #region Members

        private bool isWideCodes = false;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="GlyphShapesTable"/> instance.
        /// </summary>
		public GlyphShapesTable()
		{
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="ShapeRecordCollection"/> with the specified character.
        /// </summary>
        /// <value></value>
        public ShapeRecordCollection this[char character]
        {
            get { return base[character] as ShapeRecordCollection; }
            set { base[character] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is wide codes.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is wide codes; otherwise, <c>false</c>.
        /// </value>
        public bool IsWideCodes
        {
            get { return this.isWideCodes;  }
            set { this.isWideCodes = value; }
        }

        #endregion

        #region Collection Methods

        /// <summary>
        /// Adds the specified character.
        /// </summary>
        /// <param name="character">Character.</param>
        /// <param name="glyphShapes">Glyph shapes.</param>
        public void Add(char character, ShapeRecordCollection glyphShapes)
        {
            base.Add (character, glyphShapes);
        }

        /// <summary>
        /// Removes the specified character.
        /// </summary>
        /// <param name="character">Character.</param>
        public void Remove(char character)
        {
            base.Remove(character);
        }

        /// <summary>
        /// Test if containses the specified character.
        /// </summary>
        /// <param name="character">Character.</param>
        /// <returns></returns>
        public bool Contains(char character)
        {
            return base.Contains(character);
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
            res += GetCodesSizeOf();
            res += GetGlyphsSizeOf();
            return res;
        }

        /// <summary>
        /// Gets the codes size of.
        /// </summary>
        /// <returns></returns>
        public int GetCodesSizeOf()
        {
            if (isWideCodes)
                return Keys.Count * 2;
            else
                return Keys.Count;
        }

        /// <summary>
        /// Gets the glyphs size of.
        /// </summary>
        /// <returns></returns>
        public int GetGlyphsSizeOf()
        {
            int res = 0;
            ShapeWithStyle.NumFillBits = 0;
            ShapeWithStyle.NumLineBits = 0;
            IEnumerator glyphShapesCollections = this.Values.GetEnumerator();
            while (glyphShapesCollections.MoveNext())
            {
                ShapeRecordCollection glyphs = (ShapeRecordCollection)glyphShapesCollections.Current;
                ShapeWithStyle.NumFillBits = 1;
                res += glyphs.GetSizeOf();
            }
            return res;
        }

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        /// <param name="numGlyphs">Num glyphs.</param>
        public void ReadData(BufferedBinaryReader binaryReader, ushort numGlyphs)
        {
            if (numGlyphs == 0)
                return;

            ShapeWithStyle.NumFillBits = 0;
            ShapeWithStyle.NumLineBits = 0;
            ShapeRecordCollection[] shapes = new ShapeRecordCollection[numGlyphs];
            for (int i = 0; i < numGlyphs; i++)
            {
                ShapeRecordCollection glyphShape = new ShapeRecordCollection();
                glyphShape.ReadData(binaryReader, ShapeType.None);
                shapes[i] = glyphShape;
            }

            for (int i = 0; i < numGlyphs; i++)
            {
                char c; 
                if (isWideCodes)
                    c = (char)binaryReader.ReadUInt16();
                else
                    c = (char)binaryReader.ReadByte();
                this[c] = shapes[i];
            }
            shapes = null;
        }

        /// <summary>
        /// Gets the ordered glyphs.
        /// </summary>
        /// <param name="orderedCodes">Ordered codes.</param>
        public ShapeRecordCollection[] GetOrderedGlyphs(char[] orderedCodes)
        {
            ShapeRecordCollection[] res = new ShapeRecordCollection[orderedCodes.Length];
            
            IEnumerator codesEnum = orderedCodes.GetEnumerator();
            for (int i = 0; codesEnum.MoveNext(); i++)
                res[i] = this[(char)codesEnum.Current];
            
            return res;
        }

        /// <summary>
        /// Gets the ordered codes.
        /// </summary>
        /// <returns></returns>
        public char[] GetOrderedCodes()
        {
            char[] keys = new char[Keys.Count];
            this.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                char ic = (char)keys[i];
                int tmpIndex = i;
                char tmpChar = ic;
                for (int j = i + 1; j < keys.Length; j++)
                {
                    if ((char)keys[j] < tmpChar)
                    {
                        tmpIndex = j;
                        tmpChar = (char)keys[j];
                    }
                }
                if (tmpIndex != i)
                {
                    keys[tmpIndex] = keys[i];
                    keys[i] = tmpChar;
                }
            }
            return keys;
        }

        /// <summary>
        /// Writes to.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public void WriteTo(BufferedBinaryWriter writer)
        {
            char[] codes = GetOrderedCodes();
            ShapeRecordCollection[] glyphs = GetOrderedGlyphs(codes);

            ShapeWithStyle.NumFillBits = 0;
            ShapeWithStyle.NumLineBits = 0;

            IEnumerator glyphsEnum = glyphs.GetEnumerator();
            while (glyphsEnum.MoveNext())
            {
                ShapeWithStyle.NumFillBits = 1;
                ((ShapeRecordCollection)glyphsEnum.Current).WriteTo(writer);
            }
            
            IEnumerator chars = codes.GetEnumerator();
            while (chars.MoveNext())
            {
                char c = (char)chars.Current;
                if (isWideCodes)
                    writer.Write((ushort)c);
                else
                    writer.Write((byte)c);
            }
        }

        /// <summary>
        /// Gets the char index.
        /// </summary>
        /// <param name="character">Character.</param>
        /// <returns>char index if found, -1 else</returns>
        public int GetCharIndex(char character)
        {
            char[] codes = GetOrderedCodes();
            IEnumerator chars = codes.GetEnumerator();
            for (int i = 0; chars.MoveNext(); i++)
            {
                char c = (char)chars.Current;
                if (c == character)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Serializes to the specified writer.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public void Serialize(XmlWriter writer)
        {
            //Write glyphs
            char[] codes = GetOrderedCodes();
            ShapeRecordCollection[] glyphs = GetOrderedGlyphs(codes);
            
            writer.WriteStartElement("GlyphShapesTable");
            IEnumerator glyphsEnum = glyphs.GetEnumerator();
			while (glyphsEnum.MoveNext())
			{
				// To set the first fillStyle0 to 1.
				ShapeWithStyle.NumFillBits = 1; 
				((ShapeRecordCollection)glyphsEnum.Current).Serialize(writer);
			}
            writer.WriteEndElement();

            //Write codes
            writer.WriteStartElement("CodeTable");
            IEnumerator chars = codes.GetEnumerator();
            while (chars.MoveNext())
            {
                char code = (char)chars.Current;
                writer.WriteStartElement("Code");
                writer.WriteAttributeString("Value", ((ushort)code).ToString());
                writer.WriteAttributeString("Char", ((char)code).ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        #endregion
	}
}
