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
	/// StartSound tag instructs the player to start or stop 
	/// playing a sound defined using the DefineSoundTag class.
	/// </summary>
	/// <remarks>
	/// <p>
	/// 
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 1.
	/// </p>
	/// </remarks>
	public class StartSoundTag : BaseTag 
    {
        #region Members

		private ushort soundId;
		private SoundInfo soundInfo = null;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="StartSoundTag"/> instance.
        /// </summary>
        public StartSoundTag() 
        {
            this._tagCode = (int)TagCodeEnum.StartSound;
        }

        /// <summary>
        /// Creates a new <see cref="StartSoundTag"/> instance.
        /// </summary>
        /// <param name="soundId">Sound id.</param>
        /// <param name="soundInfo">Sound info.</param>
		public StartSoundTag(ushort soundId, SoundInfo soundInfo) 
		{
			this.soundId = soundId;
			this.soundInfo = soundInfo;
			this._tagCode = (int)TagCodeEnum.StartSound;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the sound id.
        /// </summary>
        /// <value></value>
        public ushort SoundId
        {
            get { return this.soundId;  }
            set { this.soundId = value; }
        }

        /// <summary>
        /// Gets or sets the sound info.
        /// </summary>
        /// <value></value>
        public SoundInfo SoundInfo
        {
            get { return this.soundInfo;  }
            set { this.soundInfo = value; }
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
            soundInfo = new SoundInfo();
            soundInfo.ReadData(binaryReader);
        }

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns></returns>
		public int GetSizeOf()
		{
			int length = 2;
			if (soundInfo != null)
				length += soundInfo.GetSizeOf();
			return length;
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
			if (this.soundInfo != null)
				this.soundInfo.WriteTo(w);

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
			writer.WriteStartElement("StartSound");
			writer.WriteElementString("SoundId", soundId.ToString());
			if (this.soundInfo != null)
				this.soundInfo.Serialize(writer);
			writer.WriteEndElement();
		}
	
        #endregion
	}
}
