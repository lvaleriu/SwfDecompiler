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

using SwfDotNet.IO;
using SwfDotNet.IO.Utils;

namespace SwfDotNet.IO.Tags.Types
{
	/// <summary>
	/// GlyphEntry
	/// </summary>
	public class GlyphEntry: ISwfSerializer
	{
        #region Members

		private char glyphCharacter = '\0';
		private uint glyphIndex;
        private int glyphAdvance;

        #endregion
        
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="GlyphEntry"/> instance.
        /// </summary>
        public GlyphEntry()
        {
        }

        /// <summary>
        /// Creates a new <see cref="GlyphEntry"/> instance.
        /// </summary>
        /// <param name="glyphIndex">Glyph index.</param>
        /// <param name="glyphAdvance">Glyph advance.</param>
		public GlyphEntry(uint glyphIndex, int glyphAdvance)
		{
            this.glyphIndex = glyphIndex;
            this.glyphAdvance = glyphAdvance;
		}

        /// <summary>
        /// Creates a new <see cref="GlyphEntry"/> instance.
        /// </summary>
        /// <param name="character">Character.</param>
        public GlyphEntry(char character)
        {
            this.glyphCharacter = character;
        }

        #endregion

        #region Properties

		/// <summary>
		/// Gets or sets the glyph character.
		/// </summary>
		/// <value></value>
		public char GlyphCharacter
		{
			get { return this.glyphCharacter;  }
			set { this.glyphCharacter = value; }
		}

        /// <summary>
        /// Gets or sets the glyph index.
        /// </summary>
        public uint GlyphIndex
        {
            get { return this.glyphIndex;  }
            set { this.glyphIndex = value; }
        }

        /// <summary>
        /// Gets or sets the glyph advance.
        /// </summary>
        public int GlyphAdvance
        {
            get { return this.glyphAdvance;  }
            set { this.glyphAdvance = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        public void ReadData(BufferedBinaryReader binaryReader)
        {
            glyphIndex = binaryReader.ReadUBits(TextRecordCollection.GLYPH_BITS);
            glyphAdvance = binaryReader.ReadSBits(TextRecordCollection.ADVANCE_BITS);
        }

        /// <summary>
        /// Gets the size of in bits number
        /// </summary>
        /// <returns></returns>
        public int GetBitsSizeOf()
        {
            int res = 0;
            res += TextRecordCollection.GLYPH_BITS;
            res += TextRecordCollection.ADVANCE_BITS;
            return res;
        }

        /// <summary>
        /// Writes to.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public void WriteTo(BufferedBinaryWriter writer)
        {
            writer.WriteUBits(glyphIndex, TextRecordCollection.GLYPH_BITS);
            writer.WriteSBits(glyphAdvance, TextRecordCollection.ADVANCE_BITS);
        }

		/// <summary>
		/// Serializes to the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("GlyphEntry");
			writer.WriteAttributeString("GlyphBits", TextRecordCollection.GLYPH_BITS.ToString());
			writer.WriteAttributeString("GlyphIndex", glyphIndex.ToString());
			writer.WriteAttributeString("AdvanceBits", TextRecordCollection.ADVANCE_BITS.ToString());
			writer.WriteAttributeString("GlyphAdvance", glyphAdvance.ToString());
			writer.WriteEndElement();
		}


        #endregion
	}

    /// <summary>
    /// GlyphEntryCollection
    /// </summary>
    public class GlyphEntryCollection: CollectionBase
    {
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="GlyphEntryCollection"/> instance.
        /// </summary>
        public GlyphEntryCollection()
        {
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

            IEnumerator glyphs = this.GetEnumerator();
            while (glyphs.MoveNext())
                res += ((GlyphEntry)glyphs.Current).GetBitsSizeOf();

            return Convert.ToInt32(Math.Ceiling((double)res / 8.0)); 
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public GlyphEntry Add(GlyphEntry value)
        {
            List.Add(value as object);
            return value;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="values">Values.</param>
        public void AddRange(GlyphEntry[] values)
        {
            foreach(GlyphEntry ip in values)
                Add(ip);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public void Remove(GlyphEntry value)
        {
            if (List.Contains(value))
                List.Remove(value as object);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="value">Value.</param>
        public void Insert(int index, GlyphEntry value)
        {
            List.Insert(index, value as object);
        }

        /// <summary>
        /// Containses the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public bool Contains(GlyphEntry value)
        {
            return List.Contains(value as object);
        }

        /// <summary>
        /// Gets or sets the <see cref="LineStyle"/> at the specified index.
        /// </summary>
        /// <value></value>
        public GlyphEntry this[int index]
        {
            get
            {
                return ((GlyphEntry)List[index]);
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
        public int IndexOf(GlyphEntry value)
        {
            return List.IndexOf(value);
        }

        #endregion
    }

	/// <summary>
	/// Text Record class
	/// </summary>
	public class TextRecord: ISwfSerializer
	{
        #region Members

		private ushort fontId;
        private RGBColor textColor;
        private short xOffset;
        private short yOffset;
        private ushort textHeight;        
        private GlyphEntryCollection glyphEntries;

        #endregion
		
        #region Ctor & Init

        /// <summary>
        /// Creates a new <see cref="TextRecord"/> instance.
        /// </summary>
        public TextRecord()
        {
            Init();
        }

        /// <summary>
        /// Inits this instance.
        /// </summary>
        protected void Init()
        {
            glyphEntries = new GlyphEntryCollection();
            xOffset = short.MinValue;
            yOffset = short.MinValue;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the font id.
        /// </summary>
        public ushort FontId
        {
            get { return this.fontId;  }
            set { this.fontId = value; }
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        public RGBColor TextColor
        {
            get { return this.textColor;  }
            set { this.textColor = value; }
        }

        /// <summary>
        /// Gets or sets the X offset.
        /// </summary>
        public short XOffset
        {
            get { return this.xOffset;  }
            set { this.xOffset = value; }
        }

        /// <summary>
        /// Gets or sets the Y offset.
        /// </summary>
        public short YOffset
        {
            get { return this.yOffset;  }
            set { this.yOffset = value; }
        }

        /// <summary>
        /// Gets or sets the height of the text.
        /// </summary>
        public ushort TextHeight
        {
            get { return this.textHeight;  }
            set { this.textHeight = value; }
        }

        /// <summary>
        /// Gets the glyph entries.
        /// </summary>
        public GlyphEntryCollection GlyphEntries
        {
            get { return this.glyphEntries;  }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        /// <param name="endOfRecordsFlag">End of records flag.</param>
        /// <param name="tagCodeEnum">Tag code enum.</param>
        public void ReadData(BufferedBinaryReader binaryReader, ref bool endOfRecordsFlag, 
            TagCodeEnum tagCodeEnum)
        {
            binaryReader.SynchBits();
            bool textRecordType = binaryReader.ReadBoolean();
            binaryReader.ReadUBits(3);
            
            bool styleFlagsHasFont = binaryReader.ReadBoolean();
            bool styleFlagsHasColor = binaryReader.ReadBoolean();
            bool styleFlagsHasYOffset = binaryReader.ReadBoolean();
            bool styleFlagsHasXOffset = binaryReader.ReadBoolean();

            if (textRecordType == false)
            {
                endOfRecordsFlag = true;
                return;
            }

            fontId = 0;
            if (styleFlagsHasFont)
                fontId = binaryReader.ReadUInt16(); 
			
            textColor = null;
            if (styleFlagsHasColor)
            {
                if (tagCodeEnum == TagCodeEnum.DefineText2)
                {
                    textColor = new RGBA();
                    textColor.ReadData(binaryReader);
                }
                else
                {
                    textColor = new RGB();
                    textColor.ReadData(binaryReader);
                }
            }

            xOffset = 0;
            if (styleFlagsHasXOffset)
                xOffset = binaryReader.ReadInt16();

            yOffset = 0;
            if (styleFlagsHasYOffset)
                yOffset = binaryReader.ReadInt16();
			
            textHeight = 0; 
            if (styleFlagsHasFont)
                textHeight = binaryReader.ReadUInt16();

            byte glyphCount = binaryReader.ReadByte();
            if (glyphCount > 0)
            {
                if (glyphEntries == null)
                    glyphEntries = new GlyphEntryCollection();
                else
                    glyphEntries.Clear();

                for (int i = 0; i < glyphCount; i++)
                {
                    GlyphEntry glyphEntry = new GlyphEntry();
                    glyphEntry.ReadData(binaryReader);
                    glyphEntries.Add(glyphEntry);
                }
            }
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns>SIze of this object</returns>
        public int GetSizeOf()
        {
            bool styleFlagsHasFont = HasFont();
            bool styleFlagsHasColor = HasColor();
            bool styleFlagsHasYOffset = HasYOffset();
            bool styleFlagsHasXOffset = HasXOffset();

            int res = 1;
            if (styleFlagsHasFont)
                res += 2; 
            if (styleFlagsHasColor)
                res += textColor.GetSizeOf();
            if (styleFlagsHasXOffset)
                res += 2;
            if (styleFlagsHasYOffset)
                res += 2;		
            if (styleFlagsHasFont)
                res += 2;
            res++;
            if (this.glyphEntries != null)
                res += glyphEntries.GetSizeOf();
            return res;
        }

        /// <summary>
        /// Writes to a binary writer.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public void WriteTo(BufferedBinaryWriter writer)
        {
            writer.SynchBits();

            writer.WriteBoolean(true);
            writer.WriteUBits(0, 3);
            
            bool styleFlagsHasFont = HasFont();
            bool styleFlagsHasColor = HasColor();
            bool styleFlagsHasYOffset = HasYOffset();
            bool styleFlagsHasXOffset = HasXOffset();

            writer.WriteBoolean(styleFlagsHasFont);
            writer.WriteBoolean(styleFlagsHasColor);
            writer.WriteBoolean(styleFlagsHasYOffset);
            writer.WriteBoolean(styleFlagsHasXOffset);
            
            if (styleFlagsHasFont)
                writer.Write(this.fontId);
            
            if (styleFlagsHasColor)
                this.textColor.WriteTo(writer);
            if (styleFlagsHasXOffset)
                writer.Write(this.xOffset);
            if (styleFlagsHasYOffset)
                writer.Write(this.yOffset);	
            if (styleFlagsHasFont)
                writer.Write(this.textHeight);
            writer.Write((byte)this.glyphEntries.Count);

            if (this.glyphEntries != null)
            {
                IEnumerator glyphs = this.glyphEntries.GetEnumerator();
                while (glyphs.MoveNext())
                    ((GlyphEntry)glyphs.Current).WriteTo(writer);
            }
        }

		/// <summary>
		/// Serializes with the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("TextRecord");
			writer.WriteElementString("StyleFlagsHasFont", HasFont().ToString());
			writer.WriteElementString("StyleFlagsHasColor", HasColor().ToString());
			writer.WriteElementString("StyleFlagsHasYOffset", HasYOffset().ToString());
			writer.WriteElementString("StyleFlagsHasXOffset", HasXOffset().ToString());

			if (HasFont())
				writer.WriteElementString("FontId", fontId.ToString());

			if (HasColor())
				this.textColor.Serialize(writer);
			
			if (HasXOffset())
				writer.WriteElementString("XOffset", xOffset.ToString());

			if (HasYOffset())
				writer.WriteElementString("YOffset", yOffset.ToString());
			
			if (HasFont())
				writer.WriteElementString("TextHeight", textHeight.ToString());

			if (this.glyphEntries != null)
			{
				writer.WriteElementString("GlyphCount", this.glyphEntries.Count.ToString());
				writer.WriteStartElement("GlyphEntries");

                IEnumerator glyphs = this.glyphEntries.GetEnumerator();
                while (glyphs.MoveNext())
                    ((GlyphEntry)glyphs.Current).Serialize(writer);

				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

        /// <summary>
        /// Determines whether this instance has color.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance has color; otherwise, <c>false</c>.
        /// </returns>
        private bool HasColor()
        {
            return textColor != null;
        }

        /// <summary>
        /// Determines whether this instance has font.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance has font; otherwise, <c>false</c>.
        /// </returns>
        private bool HasFont()
        {
            return fontId != 0 && textHeight != 0;
        }

        /// <summary>
        /// Determines whether [has X offset].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has X offset]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasXOffset()
        {
            return this.xOffset != short.MinValue;
        }

        /// <summary>
        /// Determines whether [has Y offset].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has Y offset]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasYOffset()
        {
            return this.yOffset != short.MinValue;
        }

        /// <summary>
        /// Determines whether this instance has style.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance has style; otherwise, <c>false</c>.
        /// </returns>
        private bool HasStyle()
        {
            return HasFont() || HasColor() || HasXOffset() || HasYOffset();
        }

        /// <summary>
        /// Gets the glyph bits.
        /// </summary>
        /// <returns></returns>
        public byte GetGlyphBits()
        {
            byte numberOfBits = 0;
		
            IEnumerator glyphs = glyphEntries.GetEnumerator();
            while (glyphs.MoveNext())
            {
                uint numBits = BufferedBinaryWriter.GetNumBits(((GlyphEntry)glyphs.Current).GlyphIndex);
                numberOfBits = (byte)Math.Max(numberOfBits, numBits);
            }
            return numberOfBits;
        }
	
        /// <summary>
        /// Gets the advance bits.
        /// </summary>
        /// <returns></returns>
        public byte GetAdvanceBits()
        {
            byte numberOfBits = 0;
		
            IEnumerator glyphs = glyphEntries.GetEnumerator();
            while (glyphs.MoveNext())
            {
                uint numBits = BufferedBinaryWriter.GetNumBits(((GlyphEntry)glyphs.Current).GlyphAdvance);
                numberOfBits = (byte)Math.Max(numberOfBits, numBits);
            }
            return numberOfBits;
        }

        #endregion
	}

    /// <summary>
    /// TextRecordCollection class
    /// </summary>
    public class TextRecordCollection: CollectionBase
    {
        #region Static Members
        
        /// <summary>
        /// Glyph bits number
        /// </summary>
        public static byte GLYPH_BITS = 0;
        
        /// <summary>
        /// Advance bits number
        /// </summary>
        public static byte ADVANCE_BITS = 0;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="TextRecordCollection"/> instance.
        /// </summary>
        public TextRecordCollection()
        {
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public TextRecord Add(TextRecord value)
        {
            List.Add(value as object);
            return value;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="values">Values.</param>
        public void AddRange(TextRecord[] values)
        {
            foreach(TextRecord ip in values)
                Add(ip);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public void Remove(TextRecord value)
        {
            if (List.Contains(value))
                List.Remove(value as object);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="value">Value.</param>
        public void Insert(int index, TextRecord value)
        {
            List.Insert(index, value as object);
        }

        /// <summary>
        /// Containses the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public bool Contains(TextRecord value)
        {
            return List.Contains(value as object);
        }

        /// <summary>
        /// Gets or sets the <see cref="TextRecord"/> at the specified index.
        /// </summary>
        /// <value></value>
        public TextRecord this[int index]
        {
            get
            {
                return ((TextRecord)List[index]);
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
        public int IndexOf(TextRecord value)
        {
            return List.IndexOf(value);
        }

        #endregion
    }
}
