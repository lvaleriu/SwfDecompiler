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
using System.Text;
using System.Xml;

using SwfDotNet.IO.Utils;
using SwfDotNet.IO.Tags;
using SwfDotNet.IO.Tags.Types;
using SwfDotNet.IO.Exceptions;

namespace SwfDotNet.IO 
{	
	/// <summary>
	/// Swf file header object contains main informations
	/// about the animation. 
	/// </summary>
	/// <remarks>
    /// <p>
    /// The header contains those informations:
    /// <ul>
    /// <li>File signature, to indicate of the file is compressed or not</li>
    /// <li>Swf version (1 to 11)</li>
    /// <li>Lenght of entire file in bytes</li>
    /// <li>Frame size in twips</li>
    /// <li>Frame delay in 8.8 fixed number of frame per second</li>
    /// <li>Total number of frames in the movie</li>
    /// </ul>
    /// </p>
	/// </remarks>
	public class SwfHeader: ISwfSerializer 
	{
		#region Const Members

		/// <summary>
		/// Maximum swf version supported
		/// </summary>
		public static int MAX_VERSION = 11; 

		#endregion

        #region Members

		/// <summary>
		/// Signature property ('FWS' or 'CWS').
		/// </summary>
		public string signature;
		
		/// <summary>
		/// Private Version.
		/// </summary>
		private byte version;
		
		/// <summary>
		/// Private FileSize.
		/// </summary>
		private uint fileSize;
		
		/// <summary>
		/// Private rect containing swf dimensions.
		/// </summary>
		private Rect rect;			
		
		/// <summary>
		/// Private frames per second.
		/// </summary>
		private float fps;
		
		/// <summary>
		/// Private total number of frames.
		/// </summary>
		private ushort frames;

        #endregion
		
        #region Ctor

		/// <summary>
		/// Constructor.
		/// </summary>
		public SwfHeader()
		{
			fps = 12.0f;
			frames = 0;
			rect = new Rect(0, 0, 550 * 20, 400 * 20);
			version = (byte)MAX_VERSION;
			signature = "FWS";
		}

		/// <summary>
		/// Creates a new <see cref="SwfHeader"/> instance.
		/// </summary>
		/// <param name="signature">Signature.</param>
		/// <param name="version">Version.</param>
		/// <param name="fileSize">Size of the file.</param>
		/// <param name="dimensions">Dimensions.</param>
		/// <param name="fps">FPS.</param>
		/// <param name="frames">Frames.</param>
		public SwfHeader(string signature, byte version, 
						 uint fileSize, Rect dimensions, 
						 float fps, ushort frames) 
		{
			this.signature = signature;
			Version = version;
			this.fileSize = fileSize;
			this.rect = dimensions;	
			this.fps = fps;
			this.frames = frames;			
		}

        #endregion

        #region Properties

		/// <summary>
		/// Get signature as string.
		/// "FWS" for not compress files.
		/// "CWS" for compressed file.
		/// </summary>
		public string Signature
		{
			get { return this.signature;  }
			set { this.signature = value; }
		}

        /// <summary>
        /// Gets or sets the frames count.
        /// </summary>
        public ushort Frames
        {
            get { return this.frames;  }
            set { this.frames = value; }
        }

        /// <summary>
        /// Gets or sets the FPS (frames per second)
        /// </summary>
        public float Fps
        {
            get { return this.fps;  }
            set { this.fps = value; }
        }

        /// <summary>
        /// Gets or sets the swf dimensions bound.
        /// </summary>
        public Rect Size
        {
            get { return this.rect;  }
			set { this.rect = value; }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width
        {
            get 
            {
                if (this.rect == null)
                    return 0;
                return this.rect.Rectangle.Width; 
            }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height
        {
            get 
            { 
                if (this.rect == null)
                    return 0;
                return this.rect.Rectangle.Height; 
            }
        }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        public uint FileSize
        {
            get { return this.fileSize;  }
            set { this.fileSize = value; }
        }
        
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public byte Version
        {
            get { return this.version;  }
            set 
			{
				this.version = value; 
				if (this.version > MAX_VERSION)
					throw new InvalidSwfVersionException();
			} 
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data from a binary file
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        public void ReadData(BufferedBinaryReader binaryReader)
        {
            this.signature = binaryReader.ReadString(3);
            this.version = binaryReader.ReadByte();
			
			if (this.version > MAX_VERSION)
				throw new InvalidSwfVersionException(this.version, MAX_VERSION);
				
            this.fileSize = binaryReader.ReadUInt32();
            this.rect = new Rect();
            this.rect.ReadData(binaryReader);
            binaryReader.SynchBits();
            this.fps = binaryReader.ReadFloatWord(8, 8);
            this.frames = binaryReader.ReadUInt16();
        }

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("SwfHeader");
			writer.WriteElementString("Signature", this.signature);
			writer.WriteElementString("Version", this.version.ToString());
			this.rect.Serialize(writer);
			writer.WriteElementString("Fps", this.fps.ToString());
			writer.WriteElementString("Frames", this.frames.ToString());
			writer.WriteEndElement();
		}

		#endregion
	}
}
