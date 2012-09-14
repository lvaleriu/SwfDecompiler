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

using SwfDotNet.IO.Utils;
using SwfDotNet.IO.Tags.Types;

namespace SwfDotNet.IO.Tags
{
    #region Enum

    /// <summary>
    /// Flv Sound Format
    /// </summary>
    public enum FlvSoundFormat
    {
        /// <summary>
        /// Uncompressed
        /// </summary>
        Uncompressed = 0,
        /// <summary>
        /// ADPCM
        /// </summary>
        ADPCM = 1,
        /// <summary>
        /// Mp3
        /// </summary>
        Mp3 = 2,
        /// <summary>
        /// NellyMoser8KhzMono
        /// </summary>
        NellyMoser8KhzMono = 5,
        /// <summary>
        /// NellyMoser
        /// </summary>
        NellyMoser = 6
    }

    #endregion

	/// <summary>
	/// FlvAudioTag.
	/// </summary>
	public class FlvAudioTag: FlvBaseTag
	{
        #region Members

        private FlvSoundFormat soundFormat;
        private uint soundRate;
        private bool isSnd16Bits;
        private bool isStereo;
        private byte[] soundData;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="FlvAudioTag"/> instance.
        /// </summary>
		public FlvAudioTag()
		{
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the sound format.
        /// </summary>
        /// <value></value>
        public FlvSoundFormat SoundFormat
        {
            get { return this.soundFormat;  }
            set { this.soundFormat = value; }
        }

        /// <summary>
        /// Gets or sets the sound rate.
        /// </summary>
        /// <value></value>
        public uint SoundRate
        {
            get { return this.soundRate;  }
            set { this.soundRate = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is SND16 bits.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is SND16 bits; otherwise, <c>false</c>.
        /// </value>
        public bool IsSnd16Bits
        {
            get { return this.isSnd16Bits;  }
            set { this.isSnd16Bits = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is stereo.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is stereo; otherwise, <c>false</c>.
        /// </value>
        public bool IsStereo
        {
            get { return this.isStereo;  }
            set { this.isStereo = value; }
        }

        /// <summary>
        /// Gets or sets the sound data.
        /// </summary>
        /// <value></value>
        public byte[] SoundData
        {
            get { return this.soundData;  }
            set { this.soundData = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="version">Version.</param>
        /// <param name="binaryReader">Binary reader.</param>
        public override void ReadData(byte version, BufferedBinaryReader binaryReader)
        {
            base.ReadData(version, binaryReader);
            this.soundFormat = (FlvSoundFormat)binaryReader.ReadUBits(4);
            this.soundRate = binaryReader.ReadUBits(2);
            this.isSnd16Bits = binaryReader.ReadBoolean();
            this.isStereo = binaryReader.ReadBoolean();

            uint dataLenght = this.dataSize - 1;
            if (dataLenght > 0)
            {
                this.soundData = new byte[dataLenght];
                for (int i = 0; i < dataLenght; i++)
                    this.soundData[i] = binaryReader.ReadByte();
            }
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="version">Version.</param>
        public override void UpdateData(byte version)
        {
            base.UpdateData(version);
        }



        #endregion
	}
}
