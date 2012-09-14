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
using System.Collections;

using SwfDotNet.IO.Utils;
using SwfDotNet.IO.Tags;
using SwfDotNet.IO.Tags.Types;

namespace SwfDotNet.IO.Tags {
	
	/// <summary>
	/// VideoFrameTag contains the video data displayed in a 
	/// single frame of a Flash movie.
	/// </summary>
	/// <remarks>
	/// <p>
	/// Each frame of video is displayed whenever display list is 
	/// updated using the ShowFrameTag object - any timing 
	/// information stored within the video data is ignored. 
	/// Since the video is updated at the same time as the display 
	/// list the frame rate of the video may be the same or less 
	/// than the frame rate of the Flash movie but not higher.
	/// </p>
	/// <p>
	/// This tag was added in Flash 6 with support for the Sorenson 
	/// modified H263 format. Support for Macromedia's Screen Video 
	/// format was added in Flash 7.
	/// </p>
	/// </remarks>
	public class VideoFrameTag : BaseTag 
	{
		#region Members

		private ushort streamId;
        private ushort codecId;
		private ushort frameNum;
		private VideoPacket video = null;
		
		#endregion

		#region Ctor

		/// <summary>
		/// Creates a new <see cref="VideoFrameTag"/> instance.
		/// </summary>
		public VideoFrameTag()
		{
			this._tagCode = (int)TagCodeEnum.VideoFrame;
		}

		/// <summary>
		/// Creates a new <see cref="VideoFrameTag"/> instance.
		/// </summary>
		/// <param name="streamId">Stream id.</param>
		/// <param name="frameNum">Frame num.</param>
		/// <param name="video">Video.</param>
		public VideoFrameTag(ushort streamId, ushort frameNum, H263VideoPacket video) 
		{
			this._tagCode = (int)TagCodeEnum.VideoFrame;
			this.streamId = streamId;
			this.frameNum = frameNum;
			this.video = video;
		}
		
		/// <summary>
		/// Creates a new <see cref="VideoFrameTag"/> instance.
		/// </summary>
		/// <param name="streamId">Stream id.</param>
		/// <param name="frameNum">Frame num.</param>
		/// <param name="video">Video.</param>
		public VideoFrameTag(ushort streamId, ushort frameNum, ScreenVideoPacket video) 
		{
			this._tagCode = (int)TagCodeEnum.VideoFrame;
			this.streamId = streamId;
			this.frameNum = frameNum;
			this.video = video;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the stream id.
		/// </summary>
		public ushort StreamId
		{
			get { return this.streamId;  }
			set { this.streamId = value; }
		}

		/// <summary>
		/// Gets or sets the frame num.
		/// </summary>
		public ushort FrameNum
		{
			get { return this.frameNum;  }
			set { this.frameNum = value; }
		}

		/// <summary>
		/// Gets or sets the H263 video packet.
		/// </summary>
		public VideoPacket VideoPacket
		{
			get { return this.video;  }
			set { this.video = value; }
		}

        /// <summary>
        /// Gets or sets the codec id.
        /// </summary>
        /// <value></value>
        public ushort CodecId
        {
            get { return this.codecId;  }
            set { this.codecId = value; }
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

			streamId = binaryReader.ReadUInt16();
			frameNum = binaryReader.ReadUInt16();

			if (codecId == 2)
			{
				video = new H263VideoPacket();
				video.ReadData(binaryReader);
			}
			else if (codecId == 3)
			{
				video = new ScreenVideoPacket();
				video.ReadData(binaryReader);
			}
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns></returns>
        public int GetSizeOf()
        {
            int res = 4;
            if (video != null)
                res += video.GetSizeOf();
            return res;
        }

		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
		{	
			if (version < 6)
				return;

			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
			RecordHeader rh = new RecordHeader(TagCode, GetSizeOf());
			rh.WriteTo(w);
			w.Write(this.streamId);
            w.Write(this.frameNum);
            if (video != null)
                video.WriteTo(w);

            // write to data array
			w.Flush();
            _data = m.ToArray();
		}
		
		#endregion
	}
}
