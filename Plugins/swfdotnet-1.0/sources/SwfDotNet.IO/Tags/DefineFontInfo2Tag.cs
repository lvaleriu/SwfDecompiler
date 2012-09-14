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
using System.Text;

using SwfDotNet.IO.Utils;
using SwfDotNet.IO.Tags;

namespace SwfDotNet.IO.Tags 
{
	/// <summary>
	/// DefineFontInfo2Tag describes the mapping of codes for a given character 
	/// set to the glyphs that are drawn to represent the character.
	/// </summary>
	/// <remarks>
	/// <p>
	/// It extends the functionality provided by DefineFontInfo2Tag by adding a language 
	/// attribute which is support to support line-breaking when displaying text in 
	/// different spoken languages. Support for small fonts was added in Flash 7.
	/// </p>
	/// <p>
	/// The class allows the font associated with a Flash file to be mapped to a font 
	/// installed on the device where the Flash Player displaying the file is hosted. 
	/// The use of a font from a device is not automatic but is determined by the HTML 
	/// tag option <i>deviceFont</i> which is passed to the Flash Player when it is 
	/// first started. If a device does not support a given font then the glyphs in 
	/// the DefineFontTag class are used to render the characters.
	/// </p>
	/// <p>
	/// An important distinction between the host device to specify the font and 
	/// using the glyphs in an DefineFontTag object is that the device is not anti-aliased 
	/// and the rendering is dependent on the host device. The glyphs in an DefineFontTag 
	/// object are anti-aliased and are guaranteed to look identical on every device the 
	/// text is displayed.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 6. Support for small fonts was added in Flash 7.
	/// </p>
	/// </remarks>
	public class DefineFontInfo2Tag : BaseTag, DefineTag 
    {
        #region Members

		private ushort fontId;
		private string fontName;
		private byte languageCode;
		private ushort[] codeTable;
        private bool fontFlagsSmallText;
        private bool fontFlagsItalic;
        private bool fontFlagsBold;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="DefineFontInfo2Tag"/> instance.
        /// </summary>
        public DefineFontInfo2Tag()
        {
            _tagCode = (int)TagCodeEnum.DefineFontInfo2;		
        }
        
        #endregion
        
        #region Properties

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
        /// Gets or sets the code table.
        /// </summary>
        public ushort[] CodeTable
        {
            get { return this.codeTable;  }
            set { this.codeTable = value; }
        }

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        public byte LanguageCode
        {
            get { return this.languageCode;  }
            set { this.languageCode = value; }
        }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        public string FontName
        {
            get { return this.fontName;  }
            set { this.fontName = value; }
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
            binaryReader.ReadUBits(2); //not used
            fontFlagsItalic = binaryReader.ReadBoolean();
            fontFlagsBold = binaryReader.ReadBoolean();
            binaryReader.ReadBoolean(); //not used
            
            languageCode = binaryReader.ReadByte();

            long codeTableLenght = rh.TagLength - 5 - fontNameLen;
            codeTable = null;
            codeTable = new ushort[codeTableLenght];
            for (int i = 0; i < codeTableLenght / 2; i++)
                codeTable[i] = binaryReader.ReadUInt16();
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns>Size of this object</returns>
        protected int GetSizeOf()
        {
            int res = 5;
            if (fontName != null)
                res += fontName.Length;
            if (codeTable != null)
                res += codeTable.Length * 2;
            return res;
        }

		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
        {			
			if (version < 6)
				return;

            MemoryStream m = new MemoryStream();
            BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
            RecordHeader rh = new RecordHeader(TagCode, GetSizeOf(), true);
			
            rh.WriteTo(w);
            w.Write(this.fontId);
            w.Write((byte)this.fontName.Length);
            if (fontName != null)
                w.WriteString(this.fontName, (uint)this.fontName.Length);
            
			w.WriteUBits(0, 2);
			w.WriteBoolean(fontFlagsSmallText);
			w.WriteUBits(0, 2);			
			w.WriteBoolean(fontFlagsItalic);
			w.WriteBoolean(fontFlagsBold);
			w.WriteBoolean(true);
            
			w.Write(this.languageCode);
            if (codeTable != null)
            {
                IEnumerator glyphs = codeTable.GetEnumerator();
                while (glyphs.MoveNext())
                    w.Write((ushort)glyphs.Current);
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
			writer.WriteStartElement("DefineFontInfo2Tag");
			writer.WriteElementString("FontId", fontId.ToString());
			if (fontName != null)
				writer.WriteElementString("FontName", fontName.ToString());
			writer.WriteElementString("FontFlagsSmallText", fontFlagsSmallText.ToString());
			writer.WriteElementString("FontFlagsItalic", fontFlagsItalic.ToString());
			writer.WriteElementString("FontFlagsBold", fontFlagsBold.ToString());
			writer.WriteElementString("LanguageCode", languageCode.ToString());
			writer.WriteEndElement();
		}
	
        #endregion
	}
}
