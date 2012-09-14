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

namespace SwfDotNet.IO.Tags {

	/// <summary>
	/// SoundStreamHead2Tag defines an sound compressed using different 
	/// compression formats that is streamed in tight synchronisation 
	/// with the movie being played.
	/// </summary>
	/// <remarks>
	/// <p>
	/// 
	/// </p>
	/// </remarks>
	public class SoundStreamHead2Tag : BaseTag 
	{
        #region Members

        private uint playbackSoundRate;
        private uint playbackSoundSize;
        private uint playbackSoundType;
        private uint streamSoundCompression;
        private uint streamSoundRate;
        private uint streamSoundSize;
        private uint streamSoundType;
		private ushort streamSoundSampleCount;
		private short latencySeek;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="SoundStreamHead2Tag"/> instance.
        /// </summary>
        public SoundStreamHead2Tag()
        {
            this._tagCode = (int)TagCodeEnum.SoundStreamHead2;
        }

        /// <summary>
        /// Creates a new <see cref="SoundStreamHead2Tag"/> instance.
        /// </summary>
        /// <param name="playbackSoundRate">Playback sound rate.</param>
        /// <param name="playbackSoundSize">Size of the playback sound.</param>
        /// <param name="playbackSoundType">Playback sound type.</param>
        /// <param name="streamSoundCompression">Stream sound compression.</param>
        /// <param name="streamSoundRate">Stream sound rate.</param>
        /// <param name="streamSoundSize">Size of the stream sound.</param>
        /// <param name="streamSoundType">Stream sound type.</param>
        /// <param name="streamSoundSampleCount">Stream sound sample count.</param>
        /// <param name="latencySeek">Latency seek.</param>
        public SoundStreamHead2Tag(uint playbackSoundRate,
            uint playbackSoundSize, uint playbackSoundType,
            uint streamSoundCompression, uint streamSoundRate,
            uint streamSoundSize, uint streamSoundType, 
            ushort streamSoundSampleCount, short latencySeek)
		{
            this.playbackSoundRate = playbackSoundRate;
            this.playbackSoundSize = playbackSoundSize;
            this.playbackSoundType = playbackSoundType;
            this.streamSoundCompression = streamSoundCompression;
            this.streamSoundRate = streamSoundRate;
            this.streamSoundSize = streamSoundSize;
            this.streamSoundType = streamSoundType;
			this.streamSoundSampleCount = streamSoundSampleCount;
			this.latencySeek = latencySeek;
			this._tagCode = (int)TagCodeEnum.SoundStreamHead2;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the playback sound rate.
        /// </summary>
        public uint PlaybackSoundRate
        {
            get { return this.playbackSoundRate;  }
            set { this.playbackSoundRate = value; }
        }

        /// <summary>
        /// Gets or sets the size of the playback sound.
        /// </summary>
        public uint PlaybackSoundSize
        {
            get { return this.playbackSoundSize;  }
            set { this.playbackSoundSize = value; }
        }

        /// <summary>
        /// Gets or sets the playback sound type.
        /// </summary>
        public uint PlaybackSoundType
        {
            get { return this.playbackSoundType;  }
            set { this.playbackSoundType = value; }
        }

        /// <summary>
        /// Gets or sets the stream sound compression.
        /// </summary>
        public uint StreamSoundCompression
        {
            get { return this.streamSoundCompression;  }
            set { this.streamSoundCompression = value; }
        }

        /// <summary>
        /// Gets or sets the stream sound rate.
        /// </summary>
        public uint StreamSoundRate
        {
            get { return this.streamSoundRate;  }
            set { this.streamSoundRate = value; }
        }

        /// <summary>
        /// Gets or sets the size of the stream sound.
        /// </summary>
        public uint StreamSoundSize
        {
            get { return this.streamSoundSize;  }
            set { this.streamSoundSize = value; }
        }

        /// <summary>
        /// Gets or sets the stream sound type.
        /// </summary>
        public uint StreamSoundType
        {
            get { return this.streamSoundType;  }
            set { this.streamSoundType = value; }
        }

        /// <summary>
        /// Gets or sets the stream sound sample count.
        /// </summary>
        public ushort StreamSoundSampleCount
        {
            get { return this.streamSoundSampleCount;  }
            set { this.streamSoundSampleCount = value; }
        }

        /// <summary>
        /// Gets or sets the latency seek.
        /// </summary>
        public short LatencySeek
        {
            get { return this.latencySeek;  }
            set { this.latencySeek = value; }
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

            binaryReader.ReadUBits(4);
            playbackSoundRate = binaryReader.ReadUBits(2);
            playbackSoundSize = binaryReader.ReadUBits(1);
            playbackSoundType = binaryReader.ReadUBits(1);
            streamSoundCompression = binaryReader.ReadUBits(4);
            streamSoundRate = binaryReader.ReadUBits(2);
            streamSoundSize = binaryReader.ReadUBits(1);
            streamSoundType = binaryReader.ReadUBits(1);

            streamSoundSampleCount = binaryReader.ReadUInt16();
            latencySeek = 0;
			
            if (streamSoundCompression == 2)
                latencySeek = binaryReader.ReadInt16();
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns></returns>
        public int GetSizeOf()
        {
            int res = 4;
            if (streamSoundCompression == 2)
                res += 2;
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

            w.WriteUBits(0, 4);
            w.WriteUBits(playbackSoundRate, 2);
            w.WriteUBits(playbackSoundSize, 1);
            w.WriteUBits(playbackSoundType, 1);
            w.SynchBits();
            w.WriteUBits(streamSoundCompression, 4);
            w.WriteUBits(streamSoundRate, 2);
            w.WriteUBits(streamSoundSize, 1);
            w.WriteUBits(streamSoundType, 1);

            w.Write(streamSoundSampleCount);
            if (streamSoundCompression == 2)
                w.Write(latencySeek);
            
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
			writer.WriteStartElement("SoundStreamHeadTag");

			writer.WriteElementString("PlaybackSoundRate", playbackSoundRate.ToString());
			writer.WriteElementString("PlaybackSoundSize", playbackSoundSize.ToString());
			writer.WriteElementString("PlaybackSoundType", playbackSoundType.ToString());
			writer.WriteElementString("StreamSoundCompression", streamSoundCompression.ToString());
			writer.WriteElementString("StreamSoundRate", streamSoundRate.ToString());
			writer.WriteElementString("StreamSoundSize", streamSoundSize.ToString());
			writer.WriteElementString("StreamSoundType", streamSoundType.ToString());
			writer.WriteElementString("StreamSoundSampleCount", streamSoundSampleCount.ToString());
			if (streamSoundCompression == 2)
				writer.WriteElementString("LatencySeek", latencySeek.ToString());

			writer.WriteEndElement();
		}
		
        #endregion
	}
}
