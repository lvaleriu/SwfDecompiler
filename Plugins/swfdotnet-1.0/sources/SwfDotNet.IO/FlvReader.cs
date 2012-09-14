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

using SwfDotNet.IO.Tags;
using SwfDotNet.IO.Utils;

namespace SwfDotNet.IO
{
	/// <summary>
	/// FlvReader.
	/// </summary>
	public class FlvReader
	{
		#region Members

		private BufferedBinaryReader br;
        private byte version;

		#endregion

		#region Ctor & Init

		/// <summary>
		/// Creates a new <see cref="FlvReader"/> instance.
		/// </summary>
		/// <param name="stream">Stream.</param>
		public FlvReader(Stream stream)
		{
			this.br = new BufferedBinaryReader(stream, false);
		}

        /// <summary>
        /// Creates a new <see cref="FlvReader"/> instance.
        /// </summary>
        /// <param name="path">Path.</param>
		public FlvReader(string path)
		{
			Init(path, false);
		}

        /// <summary>
        /// Creates a new <see cref="SwfReader"/> instance.
        /// If useBuffer is true, all the content of 
        /// the FLV file is readed first and is parsed from the memory
        /// after. If useBuffer is false, the FLV is parsed directly from
        /// the file stream. Use a buffer is faster to parse, but use
        /// more memory.
        /// </summary>
        /// <param name="path">String path of the local flv file</param>
        /// <param name="useBuffer">Use buffer.</param>
        public FlvReader(string path, bool useBuffer)
        {
            Init(path, useBuffer);
        }

        /// <summary>
        /// Inits the stream reading process.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="useBuffer">Use buffer.</param>
        private void Init(string path, bool useBuffer)
        {
            Stream stream = File.OpenRead(path);
            if (useBuffer)
            {
                FileInfo fi = new FileInfo(path);
                
                byte[] buff = new byte[fi.Length];
                stream.Read(buff, 0, System.Convert.ToInt32(fi.Length));
                stream.Close();

                MemoryStream ms = new MemoryStream(buff);
                this.br = new BufferedBinaryReader(ms, false);
            }
            else
            {
                this.br = new BufferedBinaryReader(stream, false);
            }
        }

		#endregion

		#region Methods

		/// <summary>
		/// Reads the FLV.
		/// </summary>
		/// <returns></returns>
		public Flv ReadFlv()
		{
			FlvHeader header = new FlvHeader();
			header.ReadData(br);
            this.version = header.Version;
            br.ReadUInt32();

			FlvBaseTagCollection tags = new FlvBaseTagCollection();

			while (br.BaseStream.Position < br.BaseStream.Length) 
			{
				FlvBaseTag tag = ReadTag();
				if (tag != null)
					tags.Add(tag);
			}

			return new Flv(header, tags);
		}

        /// <summary>
        /// Reads the tag.
        /// </summary>
        /// <returns></returns>
        private FlvBaseTag ReadTag()
        {
            FlvBaseTag resTag = null;
            byte tagType = br.ReadByte();

            switch (tagType)
            {
                case (byte)FlvTagCodeEnum.Audio: resTag = new FlvAudioTag(); break;
                case (byte)FlvTagCodeEnum.Video: resTag = new FlvVideoTag(); break;
                default: break;
            }

            resTag.ReadData(this.version, this.br);
            br.ReadUInt32();

            return resTag;
        }

		#endregion
	}
}
