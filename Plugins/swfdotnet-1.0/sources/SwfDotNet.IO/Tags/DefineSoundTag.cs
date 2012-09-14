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
    #region Enum

    /// <summary>
    /// Available Sound Codecs enumeration
    /// </summary>
    public enum SoundCodec
    {
        /// <summary>
        /// Uncompressed audio data
        /// </summary>
        Uncompressed = 0,

        /// <summary>
        /// ADPCM compression
        /// </summary>
        ADPCM = 1,

        /// <summary>
        /// MP3 compression
        /// </summary>
        MP3 = 2,

        /// <summary>
        /// Uncompressed audio data with little-endian
        /// bits inversion
        /// </summary>
        UncompressedLittleEndian = 3,

        /// <summary>
        /// NellyMoser compression
        /// </summary>
        NellyMoser = 6 
    }

    #endregion

	/// <summary>
	/// DefineSoundTag is used to define a sound that will be played 
	/// when a given event occurs.
	/// </summary>
	/// <remarks>
	/// <p>
	/// Three different types of object are used to play an event sound:
	/// <ul>
	/// <li>The DefineSoundTag object that contains the sampled sound.</li>
	/// <li>A SoundInfo object that defines how the sound fades in and out, 
	/// whether it repeats and also defines an envelope for more sophisticated 
	/// control over how the sound is played</li>
	/// <li>A StartSoundTag object that signals the Flash Player to begin 
	/// playing the sound.</li>
	/// </ul>
	/// </p>
	/// <p>
	/// Five encoded formats for the sound data are supported:
	/// <ul>
	/// <li>NATIVE_PCM - uncompressed Pulse Code Modulated: samples are either 1 
	/// or 2 bytes. For two-byte samples the byte order is dependent on the 
	/// platform on which the Flash Player is hosted. Sounds created on a 
	/// platform which supports big-endian byte order will not be played 
	/// correctly when listened to on a platform which supports little-endian 
	/// byte order.</li>
	/// <li>PCM - uncompressed Pulse Code Modulated: samples are 
	/// either 1 or 2 bytes with the latter presented in Little-Endian byte order. 
	/// This ensures that sounds can be played across different platforms.</li>
	/// <li>ADPCM - compressed ADaptive Pulse Code Modulated: samples are 
	/// encoded and compressed by comparing the difference between successive 
	/// sound sample which dramatically reduces the size of the encoded sound 
	/// when compared to the uncompressed PCM formats. Use this format 
	/// whenever possible.</li>
	/// <li>MP3 - compressed MPEG Audio Layer-3.</li>
	/// <li>NELLYMOSER - compressed Nellymoser Asao format supporting low bit-rate 
	/// sound for improving synchronisation between the sound and frame 
	/// rate of movies.</li>
	/// </ul>
	/// </p>
	/// <p>
	/// <b>This tag was introduced in Flash 1. Flash 3 added support for MP3 
	/// and the Nellymoser Asao format was added in Flash 6.</b>
	/// </p>
	/// </remarks>
	public class DefineSoundTag : BaseTag, DefineTag 
	{		
		#region Members

		private ushort soundId;
        private uint soundFormat;
        private uint soundRate;
        private uint soundSize;
        private uint soundType;
		private uint soundSampleCount;
		private byte[] soundData = null;

		#endregion

		#region Ctor

		/// <summary>
		/// Creates a new <see cref="DefineSoundTag"/> instance.
		/// </summary>
		public DefineSoundTag()
		{
			this._tagCode = (int)TagCodeEnum.DefineSound;
		}

		/// <summary>
		/// Creates a new <see cref="DefineSoundTag"/> instance.
		/// </summary>
		/// <param name="soundId">Sound id.</param>
		/// <param name="soundFormat">Sound format.</param>
		/// <param name="soundRate">Sound rate.</param>
		/// <param name="soundSize">Size of the sound.</param>
		/// <param name="soundType">Sound type.</param>
		/// <param name="soundSampleCount">Sound sample count.</param>
		/// <param name="soundData">Sound data.</param>
		public DefineSoundTag(ushort soundId, uint soundFormat, 
            uint soundRate, uint soundSize, uint soundType,
            uint soundSampleCount, byte[] soundData) 
		{
			this.soundId = soundId;
			this.soundFormat = soundFormat;
            this.soundRate = soundRate;
            this.soundSize = soundSize;
            this.soundType = soundType;
			this.soundSampleCount = soundSampleCount;
			this.soundData = soundData;
			this._tagCode = (int)TagCodeEnum.DefineSound;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the sound sample count.
		/// Average number of samples in each 
		/// StreamSoundBlock.
		/// </summary>
		/// <value></value>
		public uint SoundSampleCount
		{
			get { return this.soundSampleCount;  }
			set { this.soundSampleCount = value; }
		}

		/// <summary>
		/// Gets or sets the sound type.
		/// Mono or stéreo sound: 0 = mono, 1 = stereo
		/// </summary>
		public uint SoundType
		{
			get { return this.soundType;  }
			set { this.soundType = value; }
		}

		/// <summary>
		/// Gets or sets the sound rate.
		/// The rate the sound will be played in KHertz: 0 = 5.512 KHz, 
		/// 1 = 11.025 KHz, 2 = 22.050 KHz or 3 = 44.100 KHz
		/// </summary>
		public uint SoundRate
		{
			get { return this.soundRate;  }
			set { this.soundRate = value; }
		}

		/// <summary>
		/// Gets or sets the size of the sound.
		/// This is the size of each sample: 0 = snd8bits,
		/// 1 = snd16bits
		/// </summary>
		public uint SoundSize
		{
			get { return this.soundSize;  }
			set { this.soundSize = value; }
		}

		/// <summary>
		/// Gets or sets the sound data.
		/// </summary>
		public byte[] SoundData
		{
			get { return this.soundData;  }
			set { this.soundData = value; }
		}

		/// <summary>
		/// Gets or sets the sound format.
		/// </summary>
		public SoundCodec SoundFormat
		{
			get { return (SoundCodec)this.soundFormat;  }
			set { this.soundFormat = (uint)value; }
		}

		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.DefineTag"/>
		/// </summary>
		public ushort CharacterId
		{
			get { return this.soundId;  }
			set { this.soundId = value; }
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

            soundId = binaryReader.ReadUInt16();

            soundFormat = binaryReader.ReadUBits(4);
            soundRate = binaryReader.ReadUBits(2);
            soundSize = binaryReader.ReadUBits(1);
            soundType = binaryReader.ReadUBits(1);
			
            soundSampleCount = binaryReader.ReadUInt32();

            uint size = rh.TagLength - 2 - 1 - 4;
            soundData = new byte[size];
            for (uint i = 0; i < size; i++)
                soundData[i] = binaryReader.ReadByte();
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns></returns>
        public int GetSizeOf()
        {
            int res = 0;
            res += 2;
            res++;
            res += 4;
            if (soundData != null)
                res += soundData.Length;
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

			w.Write(this.soundId);
            w.WriteUBits(soundFormat, 4);
            w.WriteUBits(soundRate, 2);
            w.WriteUBits(soundSize, 1);
            w.WriteUBits(soundType, 1);
            
            w.Write(this.soundSampleCount);
            if (soundData != null)
                w.Write(this.soundData);
            
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
            writer.WriteStartElement("DefineSoundTag");
            writer.WriteAttributeString("SoundId", this.soundId.ToString());
            writer.WriteElementString("SoundFormat", soundFormat.ToString());
            writer.WriteElementString("SoundRate", soundRate.ToString());
            writer.WriteElementString("SoundSize", soundSize.ToString());
            writer.WriteElementString("SoundType", soundType.ToString());
            writer.WriteElementString("SoundSampleCount", soundSampleCount.ToString());
            writer.WriteEndElement();
        }


		#endregion

		#region Compile & Decompile Methods

		/// <summary>
		/// Construct a new DefineSoundTag object 
        /// from a file. This method assigns file stream
        /// data and character Id, but not get audio file
        /// properties (as format, bitrate, etc.). 
		/// </summary>
		/// <param name="characterId">Character id.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <returns></returns>
		public static DefineSoundTag FromFile(ushort characterId, string fileName)
		{
			FileStream stream = File.OpenRead(fileName);
			DefineSoundTag res = FromStream(characterId, stream);
			stream.Close();
			return res;
		}

		/// <summary>
		/// Construct a new DefineSoundTag object 
		/// from a stream. This method assigns file stream
		/// data and character Id, but not get audio file
		/// properties (as format, bitrate, etc.). 
		/// </summary>
		/// <param name="characterId">Character id.</param>
		/// <param name="stream">Stream.</param>
		/// <returns></returns>
		public static DefineSoundTag FromStream(ushort characterId, Stream stream)
		{
			DefineSoundTag sound = new DefineSoundTag();
            sound.CharacterId = characterId;
			
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);

            sound.SoundData = buffer;
			return sound;
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
		/// Decompiles to stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		private void DecompileToStream(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(SoundData);
			writer.Flush();
		}

		#endregion
	}
}
