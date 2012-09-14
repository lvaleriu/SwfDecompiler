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
using System.Drawing;
using System.Drawing.Imaging;

using SwfDotNet.IO.Utils;
using SwfDotNet.IO.Tags;

namespace SwfDotNet.IO.Tags 
{
	/// <summary>
	/// DefineBits tag for Jpeg images in swf
	/// </summary>
	public class DefineBitsTag : BaseTag, DefineTag 
    {	
        #region Members

		private byte[] jpegData;
		private ushort characterId;
		
        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="DefineBitsTag"/> instance.
        /// </summary>
        public DefineBitsTag()
        {
            this._tagCode = (int)TagCodeEnum.DefineBits;
        }

        /// <summary>
        /// Creates a new <see cref="DefineBitsTag"/> instance.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="image">Image.</param>
		public DefineBitsTag(ushort id, byte[] image) 
        {
			characterId = id;
			jpegData = image;	
		    this._tagCode = (int)TagCodeEnum.DefineBits;
		}

        #endregion
		
        #region Properties

		/// <summary>
		/// JPEG Data
		/// </summary>
		public byte[] JpegData 
        {
			get { return jpegData;  }
			set { jpegData = value; }
		}

        /// <summary>
        /// see <see cref="SwfDotNet.IO.Tags.DefineTag"/>
        /// </summary>
        public ushort CharacterId
        {
            get { return this.characterId;  }
            set { this.characterId = value; }
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

            int tl = System.Convert.ToInt32(rh.TagLength);
            characterId = binaryReader.ReadUInt16();			
            jpegData = binaryReader.ReadBytes(tl - 2);			
        }

		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) {
				
			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
			RecordHeader rh = new RecordHeader(TagCode, 2 + jpegData.Length);
			
			rh.WriteTo(w);
			w.Write(characterId);
			w.Write(jpegData);
			
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
			writer.WriteStartElement("DefineBitsTag");
			writer.WriteAttributeString("CharacterId", this.characterId.ToString());
			if (this.jpegData != null)
                writer.WriteAttributeString("JpegDataLenght", this.jpegData.Length.ToString());
            writer.WriteEndElement();
		}

        #endregion

        #region Methods: Compile & Decompile

        /// <summary>
        /// Construct a new DefineBitsJpeg2Tag object 
        /// from a file.
        /// </summary>
        /// <param name="characterId">Character id.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static DefineBitsTag FromFile(ushort characterId, string fileName)
        {
            FileStream stream = File.OpenRead(fileName);
            DefineBitsTag res = FromStream(characterId, stream);
            stream.Close();
            return res;
        }

        /// <summary>
        /// Construct a new DefineBitsJpeg2Tag object 
        /// from a stream.
        /// </summary>
        /// <param name="characterId">Character id.</param>
        /// <param name="stream">Stream.</param>
        /// <returns></returns>
        public static DefineBitsTag FromStream(ushort characterId, Stream stream)
        {
            DefineBitsTag jpegTag = new DefineBitsTag();
            jpegTag.CharacterId = characterId;

            byte[] buffer = new byte[(int)stream.Length]; 
            stream.Read(buffer, 0, (int)stream.Length);
            
            byte[] buffer2 = new byte[buffer.Length + 4];
            buffer2[0] = 0xFF;
            buffer2[1] = 0xD9;
            buffer2[2] = 0xFF;
            buffer2[3] = 0xD8;
            for (int i = 0; i < buffer.Length; i++)
                buffer2[i + 4] = buffer[i];

            jpegTag.JpegData = buffer2;

            return jpegTag;
        }

        /// <summary>
        /// Construct a new DefineBitsJpeg2Tag object 
        /// from an image object.
        /// </summary>
        /// <param name="characterId">Character id.</param>
        /// <param name="image">Image.</param>
        /// <returns></returns>
        public static DefineBitsTag FromImage(ushort characterId, Image image)
        {
            if (image == null)
                return null;
			
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            byte[] buffer = ms.GetBuffer();
            ms.Close();
            
            MemoryStream stream = new MemoryStream(buffer);
            return FromStream(characterId, stream);
        }

        #endregion
	}
}
