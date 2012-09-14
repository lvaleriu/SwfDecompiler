/*
	SwfDotNet is an open source library for writing and reading 
	Macromedia Flash(c) (SWF) bytecode.
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
	/// DefineBitsJpeg3Tag is used to define a transparent 
	/// JPEG encoded image.
	/// </summary>
	/// <remarks>
	/// <p>
	/// It extends the DefineBitsJpeg2Tag class by including a 
	/// separate zlib compressed table of alpha channel values. 
	/// This allows the transparency of existing JPEG encoded 
	/// images to be changed without re-encoding the 
	/// original image.
	/// </p>
	/// <p>
	/// Although the encoding table defines how the image is 
	/// compressed it is not essential. If a DefineBitsJpeg3Tag 
	/// object is created with an empty encoding table then the 
	/// Flash Player will still display the JPEG image correctly. 
	/// The empty encoding table is not a null object. 
	/// It contains four bytes: 0xFF, 0xD9, 0xFF, 0xD8. 
	/// Note however that this is reversed from 
	/// StartOfImage (SOI, 0xFFD8) and EndOfImage (EOI, 0xFFD9) 
	/// tags defined in the JPEG file format specification. 
	/// This appears to be a bug in Flash. However the order is 
	/// preserved to ensure compatibility although code has been 
	/// tested with the normal order for the tags and the images 
	/// were displayed correctly.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 3.
	/// </p>
	/// </remarks>
	public class DefineBitsJpeg3Tag : DefineBitsJpeg2Tag 
    {
        #region Members

		private byte[] alphaData;
		
        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="DefineBitsJpeg3Tag"/> instance.
        /// </summary>
        public DefineBitsJpeg3Tag()
        {
            this._tagCode = (int)TagCodeEnum.DefineBitsJpeg3;
        }

        /// <summary>
        /// Creates a new <see cref="DefineBitsJpeg3Tag"/> instance.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="image">Image.</param>
        /// <param name="alpha">Alpha.</param>
		public DefineBitsJpeg3Tag(ushort id, byte[] image, byte[] alpha):
            base(id, image)
        {
			alphaData = alpha;
            this._tagCode = (int)TagCodeEnum.DefineBitsJpeg3;
		}

        #endregion
		
        #region Properties

		/// <summary>
		/// alpha Data
		/// </summary>
		public byte[] AlphaData 
        {
			get { return alphaData;  }
			set { alphaData = value; }
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
            int imgLen = Convert.ToInt32(binaryReader.ReadUInt32());
			if (imgLen > 0)
			{
				jpegData = binaryReader.ReadBytes(imgLen);			
				alphaData = binaryReader.ReadBytes(tl - 6 - imgLen);
			}
        }

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns></returns>
		public new int GetSizeOf()
		{
			int res = 6;
			if (jpegData != null)
				res += jpegData.Length;
			if (alphaData != null)
				res += alphaData.Length;
			return res;
		}
		
		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
        {	
			if (version < 3)
				return;

			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
			RecordHeader rh = new RecordHeader(TagCode, GetSizeOf());
			
			rh.WriteTo(w);
			w.Write(characterId);
			if (jpegData != null)
				w.Write(Convert.ToUInt32(jpegData.Length));
			else
				w.Write((int)0);
			if (jpegData != null)
				w.Write(jpegData);
			if (alphaData != null)
				w.Write(alphaData);
			
            w.Flush();
			// write to data array
			_data = m.ToArray();			
		}

        #endregion

		#region Compile Methods

		/// <summary>
		/// Construct a new DefineBitsJpeg3Tag object 
		/// from a file.
		/// </summary>
		/// <param name="characterId">Character id.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <returns></returns>
		public new static DefineBitsJpeg3Tag FromFile(ushort characterId, string fileName)
		{
			FileStream stream = File.OpenRead(fileName);
			DefineBitsJpeg3Tag res = FromStream(characterId, stream);
			stream.Close();
			return res;
		}

		/// <summary>
		/// Construct a new DefineBitsJpeg3Tag object 
		/// from a stream.
		/// </summary>
		/// <param name="characterId">Character id.</param>
		/// <param name="stream">Stream.</param>
		/// <returns></returns>
		public new static DefineBitsJpeg3Tag FromStream(ushort characterId, Stream stream)
		{
			DefineBitsJpeg3Tag jpegTag = new DefineBitsJpeg3Tag();
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
		/// Construct a new DefineBitsJpeg3Tag object 
		/// from an image object.
		/// </summary>
		/// <param name="characterId">Character id.</param>
		/// <param name="image">Image.</param>
		/// <returns></returns>
		public new static DefineBitsJpeg3Tag FromImage(ushort characterId, Image image)
		{
			if (image == null)
				return null;
			
			MemoryStream ms = new MemoryStream();
			image.Save(ms, ImageFormat.Jpeg);
			image.Dispose();
			byte[] buffer = ms.GetBuffer();
			ms.Close();
            
			MemoryStream stream = new MemoryStream(buffer);
			return FromStream(characterId, stream);
		}

		#endregion
	}
}
