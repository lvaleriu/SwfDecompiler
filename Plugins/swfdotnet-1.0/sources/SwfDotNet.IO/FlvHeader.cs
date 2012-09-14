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

namespace SwfDotNet.IO
{
	/// <summary>
	/// FlvHeader.
	/// </summary>
	public class FlvHeader
	{
		#region Members

		private string signature;
		private byte version;
		private bool hasAudio;
		private bool hasVideo;

		#endregion

		#region Ctor

		/// <summary>
		/// Creates a new <see cref="FlvHeader"/> instance.
		/// </summary>
		public FlvHeader()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether this instance has video.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has video; otherwise, <c>false</c>.
		/// </value>
		public bool HasVideo
		{
			get { return this.hasVideo;  }
			set { this.hasVideo = value; }
		}	

		/// <summary>
		/// Gets or sets a value indicating whether this instance has audio.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has audio; otherwise, <c>false</c>.
		/// </value>
		public bool HasAudio
		{
			get { return this.hasAudio;  }
			set { this.hasAudio = value; }
		}

		/// <summary>
		/// Gets or sets the signature.
		/// </summary>
		/// <value></value>
		public string Signature
		{
			get { return this.signature;  }
			set { this.signature = value; }
		}

		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		/// <value></value>
		public byte Version
		{
			get { return this.version;  }
			set { this.version = value; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Reads the data.
		/// </summary>
		/// <param name="reader">Reader.</param>
		public void ReadData(BufferedBinaryReader reader)
		{
			this.signature = reader.ReadString(3);
			this.version = reader.ReadByte();
			reader.ReadUBits(5);
			this.hasAudio = reader.ReadBoolean();
			reader.ReadBoolean();
			this.hasVideo = reader.ReadBoolean();
			reader.ReadUInt32();
		}

		#endregion
	}
}
