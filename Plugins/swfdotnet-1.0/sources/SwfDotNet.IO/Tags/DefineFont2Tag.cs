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
using SwfDotNet.IO.Tags.Types;

namespace SwfDotNet.IO.Tags {
	
	/// <summary>
	/// DefineFont2Tag defines the shapes and layout of the glyphs 
	/// used in a font. 
	/// </summary>
	/// <remarks>
	/// <p>
	/// It extends the functionality provided by the FSDefineFont class by:
	/// <ul>
	/// <li>Allowing more than 65535 glyphs in a particular font.</li>
	/// <li>Including the functionality provided by the DefineFontInfoTag class.</li>
	/// <li>Specifying ascent, descent and leading layout information for the font.</li>
	/// <li>Specifying advances for each glyph.</li>
	/// <li>Specifying bounding rectangles for each glyph.</li>
	/// <li>Specifying kerning pairs defining the distance between pairs of glyphs.</li>
	/// </ul>
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 2. Support for spoken languages was added 
	/// in Flash 6. Support for small point size fonts was added in Flash 7.
	/// </p>
	/// </remarks>
	public class DefineFont2Tag : BaseTag, DefineTag, IDefineFont
	{	
        #region Members

		private ushort fontId;
        private bool fontFlagsShiftJIS;
		private bool fontFlagsSmallText;
		private bool fontFlagsANSI;
        private bool fontFlagsItalic;
        private bool fontFlagsBold;
        private string fontName;
        private short fontAscent;
        private short fontDescent;
        private short fontLeading;
        private LanguageCode languageCode;
        private ShortCollection fontAdvanceTable;
        private RectCollection fontBoundsTable;
        private KerningRecordCollection fontKerningTable;
        private GlyphShapesTable glyphShapesTable;

        #endregion

        #region Ctor & Init

        /// <summary>
        /// Creates a new <see cref="DefineFont2Tag"/> instance.
        /// </summary>
        public DefineFont2Tag()
        {
            Init();
        }

        /// <summary>
        /// Inits this instance.
        /// </summary>
        protected void Init()
        {
            this.glyphShapesTable = new GlyphShapesTable();
            this.fontBoundsTable = new RectCollection();
            this.fontKerningTable = new KerningRecordCollection();
            this.fontAdvanceTable = new ShortCollection();
            _tagCode = (int)TagCodeEnum.DefineFont2;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        /// <value></value>
        public LanguageCode LanguageCode
        {
            get { return this.languageCode;  }
            set { this.languageCode = value; }
        }

        /// <summary>
        /// Gets or sets the font leading height.
        /// </summary>
        /// <value></value>
        public short Leading
        {
            get { return this.fontLeading;  }
            set { this.fontLeading = value; }
        }

        /// <summary>
        /// Gets or sets the font descender height.
        /// </summary>
        /// <value></value>
        public short Descent
        {
            get { return this.fontDescent;  }
            set { this.fontDescent = value; }
        }

        /// <summary>
        /// Gets or sets the font ascender height.
        /// </summary>
        /// <value></value>
        public short Ascent
        {
            get { return this.fontAscent;  }
            set { this.fontAscent = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DefineFont2Tag"/> is ANSI encoded.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if ANSI; otherwise, <c>false</c>.
        /// </value>
        public bool ANSI
        {
            get { return this.fontFlagsANSI;  }
            set { this.fontFlagsANSI = value; }
        }

        /// <summary>
        /// Gets or sets if text is small. Character
        /// glyphs are aligned on pixel boundaries for dynamic and
        /// input text.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [small text]; otherwise, <c>false</c>.
        /// </value>
        public bool SmallText
        {
            get { return this.fontFlagsSmallText;  }
            set { this.fontFlagsSmallText = value; }
        }

        /// <summary>
        /// Gets or sets if Shift JIS encoding is on.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [shift JIS]; otherwise, <c>false</c>.
        /// </value>
        public bool ShiftJIS
        {
            get { return this.fontFlagsShiftJIS;  }
            set { this.fontFlagsShiftJIS = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DefineFont2Tag"/> is italic.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if italic; otherwise, <c>false</c>.
        /// </value>
        public bool Italic
        {
            get { return this.fontFlagsItalic;  }
            set { this.fontFlagsItalic = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DefineFont2Tag"/> is bold.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if bold; otherwise, <c>false</c>.
        /// </value>
        public bool Bold
        {
            get { return this.fontFlagsBold;  }
            set { this.fontFlagsBold = value; }
        }

        /// <summary>
        /// see <see cref="SwfDotNet.IO.Tags.DefineTag"/>
        /// </summary>
        public ushort CharacterId
        {
            get { return this.fontId;  }
            set { this.fontId = value; }
        }

        /// <summary>
        /// Gets or sets the glyph shape table.
        /// </summary>
        public GlyphShapesTable GlyphShapesTable
        {
            get { return this.glyphShapesTable;  }
        }

        /// <summary>
        /// Gets the bounds table.
        /// Not used through the version 7, but must be present.
        /// </summary>
        /// <value></value>
        public RectCollection BoundsTable
        {
            get { return this.fontBoundsTable;  }
        }

        /// <summary>
        /// Gets the kerning table.
        /// </summary>
        /// <value></value>
        public KerningRecordCollection KerningTable
        {
            get { return this.fontKerningTable;  }
        }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value></value>
        public string FontName
        {
            get { return this.fontName;  }
            set { this.fontName = value; }
        }

        /// <summary>
        /// Gets the advance table to be used for each glyph
        /// in dynamic glyph text.
        /// </summary>
        /// <value></value>
        public ShortCollection AdvanceTable
        {
            get { return this.fontAdvanceTable; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
        /// </summary>
        public override void ReadData(byte version, BufferedBinaryReader binaryReader)
        {
            RecordHeader rh = new RecordHeader();
            rh.ReadData(binaryReader);

            fontId = binaryReader.ReadUInt16();

            bool fontFlagsHasLayout = binaryReader.ReadBoolean();
            fontFlagsShiftJIS = binaryReader.ReadBoolean();
            fontFlagsSmallText = binaryReader.ReadBoolean();
            fontFlagsANSI = binaryReader.ReadBoolean();
            bool fontFlagsWideOffsets = binaryReader.ReadBoolean();
            bool fontFlagsWideCodes = binaryReader.ReadBoolean();
            fontFlagsItalic = binaryReader.ReadBoolean();
            fontFlagsBold = binaryReader.ReadBoolean();
            languageCode = (LanguageCode)binaryReader.ReadByte();
            byte fontNameLength = binaryReader.ReadByte();
            
            fontName = binaryReader.ReadString(fontNameLength);

            ushort numGlyphs = binaryReader.ReadUInt16();

            if (numGlyphs > 0)
            {
                uint[] offsetTable = new uint[numGlyphs];
                for (int i = 0; i < numGlyphs; i++)
                {
                    if (fontFlagsWideOffsets)
                        offsetTable[i] = binaryReader.ReadUInt32();
                    else
                        offsetTable[i] = (uint)binaryReader.ReadUInt16();
                }
            }
            uint codeTableOffset = 0;
            if (fontFlagsWideOffsets)
                codeTableOffset = binaryReader.ReadUInt32();
            else
                codeTableOffset = (uint)binaryReader.ReadUInt16();

            if (numGlyphs > 0)
            {
                this.glyphShapesTable.IsWideCodes = fontFlagsWideCodes;
                this.glyphShapesTable.ReadData(binaryReader, numGlyphs);
            }

            if (fontFlagsHasLayout)
            {
                fontAscent = binaryReader.ReadInt16();
                fontDescent = binaryReader.ReadInt16();
                fontLeading = binaryReader.ReadInt16();

                if (numGlyphs > 0)
                {
                    fontAdvanceTable.ReadData(binaryReader, numGlyphs);
                    fontBoundsTable.ReadData(binaryReader, numGlyphs);
                    fontKerningTable.ReadData(binaryReader, fontFlagsWideCodes);
                }
            }
        }

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns></returns>
		public int GetSizeOf()
		{
            bool fontFlagsWideOffsets = HasWideOffsets();
            int res = 5;
			if (fontName != null)
				res += fontName.Length + 1;
			res += 2;
            
            int numGlyphs = GetNumGlyphs();

			if (numGlyphs > 0)
			{
				if (fontFlagsWideOffsets)
					res += numGlyphs * 4;
				else
					res += numGlyphs * 2;
			}

            if (fontFlagsWideOffsets)
                res += 4;
            else
                res += 2;

            if (glyphShapesTable != null)
                res += glyphShapesTable.GetSizeOf();
            
            if (HasLayoutInfo())
			{
				res += 6;
				res += fontAdvanceTable.GetSizeOf();
				res += fontBoundsTable.GetSizeOf();
				res += fontKerningTable.GetSizeOf(); 
			}
			return res;
		}

        /// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
        {			
			if (version < 2)
				return;
            
            bool fontFlagsWideOffsets = HasWideOffsets();
            bool fontFlagsHasLayout = HasLayoutInfo();
            bool fontFlagsWideCodes = HasWideCodes(version);
            if (glyphShapesTable != null)
                glyphShapesTable.IsWideCodes = fontFlagsWideCodes;

			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);

            RecordHeader rh = new RecordHeader(TagCode, GetSizeOf());
			rh.WriteTo(w);

			w.Write(fontId);

			w.WriteBoolean(fontFlagsHasLayout);
			w.WriteBoolean(fontFlagsShiftJIS);
			w.WriteBoolean(fontFlagsSmallText);
			w.WriteBoolean(fontFlagsANSI);
			w.WriteBoolean(fontFlagsWideOffsets);
			w.WriteBoolean(fontFlagsWideCodes);
			w.WriteBoolean(fontFlagsItalic);
			w.WriteBoolean(fontFlagsBold);
            if (version >= 6)
			    w.Write((byte)languageCode);
            else
                w.Write((byte)0);
			w.Write((byte)(this.fontName.Length + 1));
			w.WriteString(fontName);  
			
			int numGlyph = GetNumGlyphs();
			w.Write((ushort)numGlyph);

            glyphShapesTable.IsWideCodes = fontFlagsWideCodes;
            
            //Create the codetableoffset and offsettable
            int offsetTableSize = 0;
            if (fontFlagsWideOffsets)
                offsetTableSize = (numGlyph * 4) + 4;
            else
                offsetTableSize = (numGlyph * 2) + 2;

            char[] codes = glyphShapesTable.GetOrderedCodes();
            IEnumerator glyphsEnum = glyphShapesTable.GetOrderedGlyphs(codes).GetEnumerator();
            int currentOffset = 0;
            for (int i = 0; glyphsEnum.MoveNext(); i++)
            {
                long offset = offsetTableSize + currentOffset;
                if (fontFlagsWideOffsets)
                    w.Write((uint)offset);
                else
                    w.Write((ushort)offset);

                ShapeRecordCollection shapes = ((ShapeRecordCollection)glyphsEnum.Current);
                int shapeSize = shapes.GetSizeOf();
                currentOffset += shapeSize;
            }

            if (fontFlagsWideOffsets)
                w.Write((uint)(offsetTableSize + currentOffset));
            else
                w.Write((ushort)(offsetTableSize + currentOffset));
            
            glyphShapesTable.WriteTo(w);
            
			if (fontFlagsHasLayout)
			{
				w.Write(fontAscent);
				w.Write(fontDescent);
				w.Write(fontLeading);

				if (numGlyph > 0)
				{
					fontAdvanceTable.WriteTo(w);
                    fontBoundsTable.WriteTo(w);
                    if (version >= 7)
                        w.Write((ushort)0);
					else
                        fontKerningTable.WriteTo(w);       
				}
			}

			w.Flush();
			// write to data array
			_data = m.ToArray();
		}		

		/// <summary>
		/// Serializes with the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("DefineFont2Tag");
			writer.WriteAttributeString("FontId", fontId.ToString());

            bool fontFlagsWideCodes = this.glyphShapesTable.IsWideCodes;
            bool fontFlagsWideOffsets = HasWideOffsets();
            bool fontFlagsHasLayout = HasLayoutInfo();
            writer.WriteElementString("FontFlagsHasLayout", fontFlagsHasLayout.ToString());
			writer.WriteElementString("FontFlagsShiftJIS", fontFlagsShiftJIS.ToString());
			writer.WriteElementString("FontFlagsANSI", fontFlagsANSI.ToString());
			writer.WriteElementString("FontFlagsWideOffsets", fontFlagsWideOffsets.ToString());
			writer.WriteElementString("FontFlagsWideCodes", fontFlagsWideCodes.ToString());
			writer.WriteElementString("FontFlagsItalic", fontFlagsItalic.ToString());
			writer.WriteElementString("FontFlagsBold", fontFlagsBold.ToString());
			writer.WriteElementString("LanguageCode", languageCode.ToString());
			writer.WriteElementString("FontNameLength", fontName.Length.ToString());
			writer.WriteElementString("FontName", fontName.ToString());
			
            //if (offsetTable != null)
			//	writer.WriteElementString("OffsetTableLenght", offsetTable.Length.ToString());
			//writer.WriteElementString("CodeTableOffset", codeTableOffset.ToString());
            
			if (glyphShapesTable != null)
                glyphShapesTable.Serialize(writer);
            
			if (fontFlagsHasLayout)
			{
				writer.WriteElementString("FontAscent", fontAscent.ToString());
				writer.WriteElementString("FontDescent", fontDescent.ToString());
				writer.WriteElementString("FontLeading", fontLeading.ToString());
				writer.WriteElementString("FontAdvanceTable", fontAdvanceTable.Count.ToString());
                fontBoundsTable.Serialize(writer);
				fontKerningTable.Serialize(writer);
			}
			writer.WriteEndElement();
		}

		/// <summary>
		/// Gets the num glyphs.
		/// </summary>
		/// <returns></returns>
		private int GetNumGlyphs()
		{
			int numGlyph = 0;
			if (glyphShapesTable != null)
				numGlyph = glyphShapesTable.Count;
			else if (fontAdvanceTable != null)
				numGlyph = fontAdvanceTable.Count;
			return numGlyph;
		}

		/// <summary>
		/// Determines whether has wide codes.
		/// </summary>
		/// <param name="version">Version.</param>
		/// <returns>
		/// 	<c>true</c> if has wide codes; otherwise, <c>false</c>.
		/// </returns>
		private bool HasWideCodes(byte version)
		{
			if (version >= 6)
				return true;
			return !fontFlagsANSI;
		}

		/// <summary>
		/// Determines whether [has layout info].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [has layout info]; otherwise, <c>false</c>.
		/// </returns>
		private bool HasLayoutInfo()
		{
			bool layout = false;
        
			layout = layout || fontAscent != 0;
			layout = layout || fontDescent != 0;
			layout = layout || fontLeading != 0;
			layout = layout || ((fontAdvanceTable != null) && fontAdvanceTable.Count > 0);
			layout = layout || ((fontBoundsTable != null) && fontBoundsTable.Count > 0);
			layout = layout || ((fontKerningTable != null) && fontKerningTable.Count > 0);
			return layout;
		}

        /// <summary>
        /// Determines whether [has wide offsets].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has wide offsets]; otherwise, <c>false</c>.
        /// </returns>
		private bool HasWideOffsets()
		{
			bool wideOffsets = false;
        
			int glyphLength = 0;
			glyphLength += this.glyphShapesTable.GetGlyphsSizeOf();
		
			if ((glyphShapesTable.Count * 2 + glyphLength) > 65535)
				wideOffsets = true;

			return wideOffsets;
		}

        #endregion
	}
}
