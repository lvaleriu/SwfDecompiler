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
	/// SoundStreamBlock tag contains the sound data being streamed 
	/// to the Flash Player.
	/// </summary>
	/// <remarks>
	/// <p>
	/// Streaming sounds are played in tight synchronisation with one 
	/// SoundStreamBlockTag object defining the sound for each frame 
	/// displayed in a movie.
	/// </p>
	/// <p>
	/// When a streaming sound is played if the Flash Player cannot render 
	/// the frames fast enough to maintain synchronisation with the sound 
	/// being played then frames will be skipped. Normally the player will 
	/// reduce the frame rate so every frame of a movie is played.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 1 with support for Uncompressed 
	/// PCM encoded sounds (both Little-Endian and Big-Endian formats) and 
	/// the compressed ADPCM format. Support for MP3 was added in 
	/// Flash 3. The Nellymoser Asao format was added in Flash 6.
	/// </p>
	/// </remarks>
	public class SoundStreamBlockTag : BaseTag 
	{
        #region Members

		private byte[] soundData = null;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="SoundStreamBlockTag"/> instance.
        /// </summary>
        public SoundStreamBlockTag() 
        {
            this._tagCode = (int)TagCodeEnum.SoundStreamBlock;
        }

		/// <summary>
		/// Creates a new <see cref="SoundStreamBlockTag"/> instance.
		/// </summary>
		/// <param name="soundData">Sound data. 
		/// This is the encoded sound data for a single frame in a movie. 
		/// The format for the sound is defined by an SoundStreamHeadTag object. 
		/// Sounds may be encoded using the uncompressed PCM (big or 
		/// little endian byte order), compressed ADPCM,
		/// compressed MP3 or NELLYMOSER</param>
		public SoundStreamBlockTag(byte[] soundData) 
		{
			this.soundData = soundData;
			this._tagCode = (int)TagCodeEnum.SoundStreamBlock;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the sound data.
		/// This is the encoded sound data for a single frame in a movie. 
		/// The format for the sound is defined by an SoundStreamHeadTag object. 
		/// Sounds may be encoded using the uncompressed PCM (big or 
		/// little endian byte order), compressed ADPCM,
		/// compressed MP3 or NELLYMOSER.
        /// </summary>
        public byte[] SoundData
        {
            get { return this.soundData;  }
            set { this.soundData = value; }
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

            soundData = new byte[rh.TagLength];
            for (uint i = 0; i < rh.TagLength; i++)
                soundData[i] = binaryReader.ReadByte();
        }
		
		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
		{			
			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
			int length = 0;
			if (soundData != null)
				length += soundData.Length;
			RecordHeader rh = new RecordHeader(TagCode, length);
			rh.WriteTo(w);

			if (soundData != null)
				w.Write(soundData);
			
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
			writer.WriteStartElement("SoundStreamBlockTag");
			writer.WriteEndElement();
		}
		
        #endregion
	}
}
