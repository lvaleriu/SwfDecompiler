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

namespace SwfDotNet.IO.Tags
{
	/// <summary>
	/// DefineText is the abstract class of the <see cref="DefineTextTag"/> 
	/// and the <see cref="DefineText2Tag"/> object.
	/// </summary>
	public abstract class DefineText: BaseTag, DefineTag 
	{
		#region Members
	
		private ushort characterId;
		private Rect rect;
		private Matrix matrix;
		private TextRecordCollection textRecords;

		#endregion

		#region Init

		/// <summary>
		/// Inits this instance.
		/// </summary>
		protected void Init()
		{
			textRecords = new TextRecordCollection();
            rect = new Rect();
            matrix = new Matrix();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the text records.
		/// </summary>
		/// <value></value>
		public TextRecordCollection TextRecords
		{
			get { return this.textRecords; }
		}

		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.DefineTag"/>
		/// </summary>
		public ushort CharacterId
		{
			get { return this.characterId;  }
			set { this.characterId = value; }
		}

        /// <summary>
        /// Gets the rect.
        /// </summary>
        /// <value></value>
		public Rect Rect
		{
			get { return this.rect;  }
		}

        /// <summary>
        /// Gets the matrix.
        /// </summary>
        /// <value></value>
		public Matrix Matrix
		{
			get { return this.matrix;  }
		}

        /// <summary>
        /// Gets the name of the tag.
        /// </summary>
        /// <value>The name of the tag.</value>
        protected abstract string TagName
        {
            get;
        }

        /// <summary>
        /// Gets the version compatibility.
        /// </summary>
        /// <value>The version compatibility.</value>
        protected abstract int VersionCompatibility
        {
            get;
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

			characterId = binaryReader.ReadUInt16();
			if (rect == null)
                rect = new Rect();
			rect.ReadData(binaryReader);

			if (matrix == null)
                matrix = new Matrix();
			matrix.ReadData(binaryReader);

			TextRecordCollection.GLYPH_BITS = binaryReader.ReadByte();
			TextRecordCollection.ADVANCE_BITS = binaryReader.ReadByte();

			if (textRecords == null)
				textRecords = new TextRecordCollection();
			else
				textRecords.Clear();
			bool endOfRecordsFlag = false;
			while (!endOfRecordsFlag)
			{
				TextRecord textRecord = new TextRecord();
				textRecord.ReadData(binaryReader, ref endOfRecordsFlag, (TagCodeEnum)this.TagCode);
				if (!endOfRecordsFlag)
					textRecords.Add(textRecord);
			}
		}
		
		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns></returns>
		public int GetSizeOf()
		{
			TextRecordCollection.GLYPH_BITS = GetGlyphBits();
			TextRecordCollection.ADVANCE_BITS = GetAdvanceBits();

			int res = 2;        
			if (rect != null)
				res += rect.GetSizeOf();
			if (matrix != null)
				res += matrix.GetSizeOf();
			res += 2;
			if (textRecords != null)
			{
				IEnumerator records = textRecords.GetEnumerator();
				while (records.MoveNext())
					res += ((TextRecord)records.Current).GetSizeOf();
			}
			res++;
			return res;     
		}

		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
		{	
			if (version < VersionCompatibility)
				return;
            
			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);

			RecordHeader rh = new RecordHeader(TagCode, GetSizeOf());
			rh.WriteTo(w);

			w.Write(this.characterId);
			if (rect != null)
				rect.WriteTo(w);
			if (matrix != null)
				matrix.WriteTo(w);

			w.Write(TextRecordCollection.GLYPH_BITS);
			w.Write(TextRecordCollection.ADVANCE_BITS);
            
			if (textRecords != null)
			{
				IEnumerator records = textRecords.GetEnumerator();
				while (records.MoveNext())
					((TextRecord)records.Current).WriteTo(w);
			}
			w.Write((byte)0);

			w.Flush();
			// write to data array
			_data = m.ToArray();
		}		

		/// <summary>
		/// Gets the glyph bits.
		/// </summary>
		/// <returns></returns>
		private byte GetGlyphBits()
		{
			byte numberOfBits = 0;
		
			IEnumerator records = textRecords.GetEnumerator();
			while (records.MoveNext())
				numberOfBits = (byte)Math.Max(numberOfBits, ((TextRecord)records.Current).GetGlyphBits());

			return numberOfBits;
		}
	
		/// <summary>
		/// Gets the advance bits.
		/// </summary>
		/// <returns></returns>
		private byte GetAdvanceBits()
		{
			byte numberOfBits = 0;
		
			IEnumerator records = textRecords.GetEnumerator();
			while (records.MoveNext())
				numberOfBits = (byte)Math.Max(numberOfBits, ((TextRecord)records.Current).GetAdvanceBits());

			return numberOfBits;
		}

		/// <summary>
		/// Serializes with the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void Serialize(XmlWriter writer)
		{
			TextRecordCollection.GLYPH_BITS = GetGlyphBits();
			TextRecordCollection.ADVANCE_BITS = GetAdvanceBits();

			writer.WriteStartElement(TagName);
            
			writer.WriteAttributeString("CharacterId", characterId.ToString());
			if (this.rect != null)
				this.rect.Serialize(writer);
			if (this.matrix != null)
				this.matrix.Serialize(writer);
			
			writer.WriteElementString("GlyphBits", TextRecordCollection.GLYPH_BITS.ToString());
			writer.WriteElementString("AdvanceBits", TextRecordCollection.ADVANCE_BITS.ToString());
            
			writer.WriteStartElement("TextRecords");
			if (this.textRecords != null)
			{
				IEnumerator records = textRecords.GetEnumerator();
				while (records.MoveNext())
					((TextRecord)records.Current).Serialize(writer);
			}
			writer.WriteEndElement();

			writer.WriteEndElement();
		}

		/// <summary>
		/// Resolves method.
		/// This method provides the way to update the textrecords glyph indexes
		/// from Font object contained by the Swf Dictionnary.
		/// </summary>
		/// <param name="swf">SWF.</param>
		public override void Resolve(Swf swf)
		{
			IEnumerator records = textRecords.GetEnumerator();
			while (records.MoveNext())
			{
				TextRecord textRecord = (TextRecord)records.Current;
				IEnumerator glyphs = textRecord.GlyphEntries.GetEnumerator();
				while (glyphs.MoveNext())
				{
					GlyphEntry glyph = (GlyphEntry)glyphs.Current;
					if (glyph.GlyphCharacter != '\0')
					{
						object font = swf.Dictionary[textRecord.FontId];
						if (font != null && font is DefineFont2Tag)
						{
							int glyphIndex = ((DefineFont2Tag)font).GlyphShapesTable.GetCharIndex(glyph.GlyphCharacter);
							glyph.GlyphIndex = (uint)glyphIndex;
						}
						//TODO: For DefineFont
					}
				}
			}
		}

		#endregion
	}
}
