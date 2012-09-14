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

namespace SwfDotNet.IO.Tags 
{	
	/// <summary>
	/// DefineFontInfoTag defines the name and face of a font and maps the codes 
	/// for a given character set to the glyphs that are drawn to represent 
	/// each character.
	/// </summary>
	/// <remarks>
	/// <p>
	/// The ANSI character set is used for Latin languages, SJIS is used for Japanese 
	/// language characters and Unicode is used for any character set. The encoding 
	/// attributes uses the constants ANSI, SJIS or Unicode 
	/// for each character set type.
	/// </p>
	/// <p>
	/// The index of each entry in the codes array matches the index in the corresponding 
	/// glyph in the shapes array of an DefineFontTag object, allowing a given character 
	/// code to be mapped to a given glyph.
	/// </p>
	/// <p>
	/// The class allows the font associated with a Flash file to be mapped to a font 
	/// installed on the device where the Flash Player displaying the file is hosted. 
	/// The use of a font from a device is not automatic but is determined by the HTML 
	/// tag option <i>deviceFont</i> which is passed to the Flash Player when it is first 
	/// started. If a device does not support a given font then the glyphs in the DefineFontTag 
	/// class are used to render the characters.
	/// </p>
	/// <p>
	/// An important distinction between the host device to specify the font and using 
	/// the glyphs in an DefineFontTag object is that the device is not anti-aliased and 
	/// the rendering is dependent on the host device. The glyphs in an DefineFontTag 
	/// object are anti-aliased and are guaranteed to look identical on every device the 
	/// text is displayed.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 1.
	/// </p>
	/// </remarks>
	public class DefineFontInfoTag : BaseTag, DefineTag 
    {	
        #region Members

		private ushort fontId;
		private string fontName;
		private uint[] codeTable = null;
        private bool fontFlagsSmallText;
        private bool fontFlagsShiftJIS;
        private bool fontFlagsAINSI;
        private bool fontFlagsItalic;
        private bool fontFlagsBold;
        private bool fontFlagsWildCodes;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="DefineFontInfoTag"/> instance.
        /// </summary>
        public DefineFontInfoTag()
        {
            _tagCode = (int)TagCodeEnum.DefineFontInfo;		
        }
        
        /// <summary>
        /// Creates a new <see cref="DefineFontInfoTag"/> instance.
        /// </summary>
        /// <param name="fontId">Font id.</param>
        /// <param name="fontName">Name of the font.</param>
        /// <param name="fontFlagsSmallText">Font flags small text.</param>
        /// <param name="fontFlagsShiftJIS">Font flags shift JIS.</param>
        /// <param name="fontFlagsAINSI">Font flags AINSI.</param>
        /// <param name="fontFlagsItalic">Font flags italic.</param>
        /// <param name="fontFlagsBold">Font flags bold.</param>
        /// <param name="codeTable">Code table.</param>
		public DefineFontInfoTag(ushort fontId, string fontName, 
			bool fontFlagsSmallText, bool fontFlagsShiftJIS, bool fontFlagsAINSI,
            bool fontFlagsItalic, bool fontFlagsBold, uint[] codeTable) 
		{
			this.fontId = fontId;
			this.fontName = fontName;
			this.fontFlagsSmallText = fontFlagsSmallText;
            this.fontFlagsShiftJIS = fontFlagsShiftJIS;
            this.fontFlagsAINSI = fontFlagsAINSI;
            this.fontFlagsItalic = fontFlagsItalic;
            this.fontFlagsBold = fontFlagsBold;
            this.codeTable = codeTable;
			_tagCode = (int)TagCodeEnum.DefineFontInfo;		
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        public string FontName
        {
            get { return this.fontName;  }
            set { this.fontName = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [font flags AINSI].
        /// </summary>
        public bool FontFlagsAINSI
        {
            get { return this.fontFlagsAINSI;  }
            set { this.fontFlagsAINSI = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [font flags shift JIS].
        /// </summary>
        public bool FontFlagsShiftJIS
        {
            get { return this.fontFlagsShiftJIS;  }
            set { this.fontFlagsShiftJIS = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [font flags bold].
        /// </summary>
        public bool FontFlagsBold
        {
            get { return this.fontFlagsBold;  }
            set { this.fontFlagsBold = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [font flags italic].
        /// </summary>
        public bool FontFlagsItalic
        {
            get { return this.fontFlagsItalic;  }
            set { this.fontFlagsItalic = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [font flags small text].
        /// </summary>
        public bool FontFlagsSmallText
        {
            get { return this.fontFlagsSmallText;  }
            set { this.fontFlagsSmallText = value; }
        }

        /// <summary>
        /// see <see cref="SwfDotNet.IO.Tags.DefineTag"/>
        /// </summary>
        public ushort CharacterId
        {
            get { return this.fontId;  }
            set { this.fontId = value; }
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
            byte fontNameLen = binaryReader.ReadByte();
            fontName = binaryReader.ReadString(fontNameLen);
			
            binaryReader.ReadUBits(2); //reserved
            fontFlagsSmallText = binaryReader.ReadBoolean();
            fontFlagsShiftJIS = binaryReader.ReadBoolean();
            fontFlagsAINSI = binaryReader.ReadBoolean();
            fontFlagsItalic = binaryReader.ReadBoolean();
            fontFlagsBold = binaryReader.ReadBoolean();
            fontFlagsWildCodes = binaryReader.ReadBoolean();
            
            uint codeTableLenght = rh.TagLength - 4 - fontNameLen;
            
            if (!fontFlagsWildCodes)
            {
                codeTable = new uint[codeTableLenght];
                for (int i = 0; i < codeTableLenght; i++)
                    codeTable[i] = (uint)binaryReader.ReadByte();
            }
            else
            {
                codeTable = new uint[codeTableLenght / 2];
                for (int i = 0; i < codeTableLenght / 2; i++)
                    codeTable[i] = (uint)binaryReader.ReadUInt16();
            }
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns>Size of this object</returns>
        protected int GetSizeOf()
        {
            int res = 4;
            if (fontName != null)
                res += fontName.Length;
            if (!fontFlagsWildCodes)
                res += codeTable.Length;
            else
                res += codeTable.Length * 2;
            return res;
        }

		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
        {
            MemoryStream m = new MemoryStream();
            BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
            RecordHeader rh = new RecordHeader(TagCode, GetSizeOf());
		    rh.WriteTo(w);
            
            w.Write(fontId);
            w.Write((byte)fontName.Length);
            if (fontName != null)
                w.WriteString(fontName, (uint)fontName.Length);
           
            w.WriteUBits(0, 2); //reserved
            w.WriteBoolean(fontFlagsSmallText);
            w.WriteBoolean(fontFlagsShiftJIS);
            w.WriteBoolean(fontFlagsAINSI);
            w.WriteBoolean(fontFlagsItalic);
            w.WriteBoolean(fontFlagsBold);
            w.WriteBoolean(fontFlagsWildCodes);
            
            if (!fontFlagsWildCodes)
            {
                for (int i = 0; i < codeTable.Length; i++)
                    w.Write((byte)codeTable[i]);
            }
            else
            {
                for (int i = 0; i < codeTable.Length; i++)
                    w.Write((ushort)codeTable[i]);
            }

            w.Flush();
            // write to data array
            _data = m.ToArray();
		}	
		
		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("DefineFontInfoTag");
			writer.WriteElementString("FontId", fontId.ToString());
			if (fontName != null)
				writer.WriteElementString("FontName", fontName.ToString());
			writer.WriteElementString("FontFlagsSmallText", fontFlagsSmallText.ToString());
			writer.WriteElementString("FontFlagsShiftJIS", fontFlagsShiftJIS.ToString());
			writer.WriteElementString("FontFlagsAINSI", fontFlagsAINSI.ToString());
			writer.WriteElementString("FontFlagsItalic", fontFlagsItalic.ToString());
			writer.WriteElementString("FontFlagsBold", fontFlagsBold.ToString());
			writer.WriteElementString("FontFlagsWildCodes", fontFlagsWildCodes.ToString());
			writer.WriteEndElement();
		}

        #endregion
	}
}
