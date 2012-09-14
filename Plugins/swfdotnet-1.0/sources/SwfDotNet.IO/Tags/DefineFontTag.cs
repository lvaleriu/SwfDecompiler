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
	/// DefineFontTag defines the glyphs that are drawn when text 
	/// characters are rendered in a particular font.
	/// </summary>
	/// <remarks>
	/// <p>
	/// A complete definition of a font is created using the DefineFontTag 
	/// object for the glyphs along with an DefineFontInfoTag object which 
	/// contains the name of the font, whether the font face is bold 
	/// or italics and a table that maps character codes to the glyphs 
	/// that is drawn to represent the character.
	/// </p>
	/// <p>
	/// When defining a font only the glyphs used from a particular 
	/// font are included. Unused glyphs can be omitted greatly reducing 
	/// the amount of information that is encoded.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 1.
	/// </p>
	/// </remarks>
	public class DefineFontTag : BaseTag, DefineTag, IDefineFont 
	{
		#region Members

		private ushort fontId;
		private ushort[] offsetTable; //pas util à la construction
		private GlyphShapesCollection glyphShapesTable;

		#endregion

        #region Ctor & Init

		/// <summary>
		/// Creates a new <see cref="DefineFontTag"/> instance.
		/// </summary>
		public DefineFontTag()
		{
		    Init();
        }
		
        /// <summary>
        /// Inits this instance.
        /// </summary>
        protected void Init()
        {
            this.glyphShapesTable = new GlyphShapesCollection();
            this._tagCode = (int)TagCodeEnum.DefineFont;
        }

		#endregion

		#region Properties

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
		public GlyphShapesCollection GlyphShapesTable
		{
			get { return this.glyphShapesTable;  }
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
			
			ushort firstOffset = binaryReader.ReadUInt16();
			int nGlyhs = System.Convert.ToInt32( Math.Ceiling((double) firstOffset / 2 ) );
			
			offsetTable = new ushort[nGlyhs];
			offsetTable[0] = firstOffset;
			for (int i = 1; i < nGlyhs; i++)
				offsetTable[i] = binaryReader.ReadUInt16();

            ShapeWithStyle.NumFillBits = 0;
            ShapeWithStyle.NumLineBits = 0;
            glyphShapesTable.ReadData(binaryReader, (ushort)nGlyhs);
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns>Size of this object</returns>
        public int GetSizeOf()
        {
            int res = 2;
            if (offsetTable != null)
                res += offsetTable.Length * 2;
            ShapeWithStyle.NumFillBits = 0;
            ShapeWithStyle.NumLineBits = 0;
            res += glyphShapesTable.GetSizeOf();
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
            w.Write(this.fontId);

            //TODO: calcule offset !!
            if (offsetTable != null)
            {
                IEnumerator offsets = offsetTable.GetEnumerator();
                while (offsets.MoveNext())
                    w.Write((ushort)offsets.Current);
            }

            ShapeWithStyle.NumFillBits = 0;
            ShapeWithStyle.NumLineBits = 0;
            glyphShapesTable.WriteTo(w);

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
			writer.WriteStartElement("DefineFontTag");
			writer.WriteElementString("FontId", fontId.ToString());
            ShapeWithStyle.NumFillBits = 0;
            ShapeWithStyle.NumLineBits = 0;
			this.glyphShapesTable.Serialize(writer);
			writer.WriteEndElement();
		}

		#endregion
    
/*
		/// <summary>
		/// raw header data
		/// </summary>
		private byte[] header;	
		
		/// <summary>
		/// inner tags
		/// </summary>
		private BaseTag[] tagList;
		
		/// <summary>
		/// action count including inner tags´ actions
		/// </summary>
		private int _actionCount;
		
		/// <summary>
		/// tag index for action block index
		/// </summary>
		private int[] tagForAction;
		
		/// <summary>
		/// contains action block counts for inner tags
		/// </summary>
		private int[] tagOffset;
		
		/// <summary>
		/// constructor.
		/// </summary>
		/// <param name="header">header data</param>
		/// <param name="tags">list of inner tags</param>
		public DefineFontTag(byte[] header, BaseTag[] tags) {
			
			this.header = header;
			this.tagList = tags;	
			
			_tagCode = (int)TagCodeEnum.DefineShape;
			
			_actionCount = 0;			
			foreach (BaseTag b in tagList) {
				_actionCount += b.ActionRecCount;
			}
			
			tagForAction = new int[_actionCount];
			tagOffset = new int[tagList.Length];
			
			int actionIdx = 0;
			for (int i=0; i<tagList.Length; i++) {				
				
				tagOffset[i] = actionIdx;
				int count = ((BaseTag)tagList[i]).ActionRecCount;
				if (count>0) {					
					for (int j=0; j<count; j++) {
						tagForAction[actionIdx+j]=i;						
					}
					actionIdx+=count;					
				}			
			}
		}
		
		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override int ActionRecCount {
			get {
				return _actionCount;
			}
		}
		
		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override byte[] this[int index] {
			
			get {
				if ((index<0) || (index>=ActionRecCount)) return null;
				
				int offset = index-tagOffset[tagForAction[index]];
				return tagList[tagForAction[index]] [offset];
			}
			
			set {
				if ((index>-1) && (index<ActionRecCount)) {
					int offset = index-tagOffset[tagForAction[index]];
					tagList[tagForAction[index]][offset] = value;
				}
			}
		}
		
		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) {			
			
			// update inner tags
			int len = 0;
			for (int i=0; i<tagList.Length; i++) {
				BaseTag tag = (BaseTag) tagList[i];
				tag.UpdateData(version);
				len += tag.Data.Length;				
			}				
			
			MemoryStream m = new MemoryStream();
			BinaryWriter w = new BinaryWriter(m);
						
			RecordHeader rh = new RecordHeader(TagCode, len + header.Length ,true);
			
			rh.WriteTo(w);
			w.Write(header);
			for (int i=0; i<tagList.Length; i++) {
				BaseTag tag = (BaseTag) tagList[i];
				w.Write(tag.Data);
			}
			
			// update data
			_data = m.ToArray();
		}			
*/

	}
}
