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
using SwfDotNet.IO.Exceptions;

namespace SwfDotNet.IO.Tags 
{
	/// <summary>
	/// DefineBitsJpeg2Tag is used to define a JPEG encoded image 
	/// with an integrated encoding table.
	/// </summary>
	/// <remarks>
	/// <p>
	/// It extends the DefineBitsTag class by including a separate 
	/// encoding table, rather than using an JpegTable object 
	/// to store the encoding table. This allows multiple JPEG images 
	/// with different amounts of compression to be included within 
	/// a Flash file.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 2.
	/// </p>
	/// </remarks>
	/// <example>
	/// <p>
	/// <u>Transform jpeg to swf</u>
	/// <code lang="C#">
	/// 
	/// </code>
	/// </p>
	/// </example>
	public class DefineBitsJpeg2Tag : SwfDotNet.IO.Tags.BaseTag, DefineTag 
    {
        #region Members

        /// <summary>
        /// The jpeg data bytes
        /// </summary>
		protected byte[] jpegData;

        /// <summary>
        /// The character Id
        /// </summary>
		protected ushort characterId;
		
        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="DefineBitsJpeg2Tag"/> instance.
        /// </summary>
        public DefineBitsJpeg2Tag()
        {
            this._tagCode = (int)TagCodeEnum.DefineBitsJpeg2;
        }

        /// <summary>
        /// Creates a new <see cref="DefineBitsJpeg2Tag"/> instance.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="image">Image.</param>
		public DefineBitsJpeg2Tag(ushort id, byte[] image) 
        {
			characterId = id;
			jpegData = image;	
		    this._tagCode = (int)TagCodeEnum.DefineBitsJpeg2;
		}

        #endregion

        #region Properties
		
        /// <summary>
        /// Gets or sets the JPEG data.
        /// </summary>
        /// <value></value>
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
			if (tl - 2 > 0)
				jpegData = binaryReader.ReadBytes(tl - 2);			
        }

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns></returns>
		public int GetSizeOf()
		{
			int res = 2;
			if (jpegData != null)
				res += jpegData.Length;
			return res;
		}
		
		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
        {		
			if (version < 2)
				return;

			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
			RecordHeader rh = new RecordHeader(TagCode, GetSizeOf());
			rh.WriteTo(w);
			
            w.Write(characterId);
			if (jpegData != null)
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
			writer.WriteStartElement("DefineBitsJepg2Tag");
			writer.WriteAttributeString("CharacterId", CharacterId.ToString());
			
			writer.WriteEndElement();
		}


        #endregion

		#region Compile & Decompile Methods

		/// <summary>
		/// Construct a new DefineBitsJpeg2Tag object 
		/// from a file.
		/// </summary>
		/// <param name="characterId">Character id.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <returns></returns>
		public static DefineBitsJpeg2Tag FromFile(ushort characterId, string fileName)
		{
			FileStream stream = File.OpenRead(fileName);
			DefineBitsJpeg2Tag res = FromStream(characterId, stream);
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
		public static DefineBitsJpeg2Tag FromStream(ushort characterId, Stream stream)
		{
			DefineBitsJpeg2Tag jpegTag = new DefineBitsJpeg2Tag();
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
		public static DefineBitsJpeg2Tag FromImage(ushort characterId, Image image)
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

		/// <summary>
		/// Decompiles to file.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
        public void DecompileToFile(string fileName)
        {
            Stream stream = File.OpenWrite(fileName);
			DecompileToStream(stream);
			stream.Close();
        }

		/// <summary>
		/// Decompiles to image.
		/// </summary>
		/// <returns></returns>
        public Image DecompileToImage()
        {
			MemoryStream stream = new MemoryStream();
			DecompileToStream(stream);
			return Image.FromStream(stream);           
        }

		/// <summary>
		/// Decompiles to stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
        private void DecompileToStream(Stream stream)
        {
			BinaryWriter writer = new BinaryWriter(stream);

            byte[] data = null;

            int offset = 0;
            if (JpegData[0] != 0xFF || JpegData[1] != 0xD8)
                offset = 4; //Offset for the JFIF format

            data = new byte[JpegData.Length - offset];
            for (int i = offset, j = 0; i < JpegData.Length; i++, j++)
                data[j] = JpegData[i];
            
            writer.Write(data);
            writer.Flush();
        }

        #endregion
    }
}


