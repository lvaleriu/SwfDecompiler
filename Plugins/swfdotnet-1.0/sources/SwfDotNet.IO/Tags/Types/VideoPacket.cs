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
	²
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

namespace SwfDotNet.IO.Tags.Types
{
	/// <summary>
	/// Abstract Video Packet class
	/// </summary>
	public abstract class VideoPacket
	{
        #region Abstract methods

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns>The size</returns>
        public abstract int GetSizeOf();

        /// <summary>
        /// Writes to a binary writer.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public abstract void WriteTo(BinaryWriter writer);

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        public abstract void ReadData(BufferedBinaryReader binaryReader);

        /// <summary>
        /// Serializes to the specified writer.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public abstract void Serialize(XmlWriter writer);

        #endregion
	}

	/// <summary>
	/// H263VideoPacket class
	/// </summary>
	public class H263VideoPacket: VideoPacket
	{
		#region Ctor

		/// <summary>
		/// Creates a new <see cref="H263VideoPacket"/> instance.
		/// </summary>
		public H263VideoPacket()
		{
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns></returns>
        public override int GetSizeOf()
        {
            //TODO
            return 0;
        }

		/// <summary>
		/// Writes to.
		/// </summary>
		/// <param name="writer">Writer.</param>
        public override void WriteTo(BinaryWriter writer)
        {
            //TODO
        }

		/// <summary>
		/// Reads the data.
		/// </summary>
		/// <param name="binaryReader">Binary reader.</param>
        public override void ReadData(BufferedBinaryReader binaryReader)
        {
            uint pictureStartCode = binaryReader.ReadUBits(17);
            uint version = binaryReader.ReadUBits(5);
            uint temporalRef = binaryReader.ReadUBits(8);
            
            //TODO...
        }

        /// <summary>
        /// Serializes to the specified writer.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public override void Serialize(XmlWriter writer)
        {
            //TODO
        }


		#endregion
	}

	/// <summary>
	/// ScreenVideoPacket class
	/// </summary>
	public class ScreenVideoPacket: VideoPacket
	{
		#region Members

		private ImageBlock[] blocks = null;
		private int blockWidth;
		private int blockHeight;
		private int imageWidth;
		private int imageHeight;

		#endregion

		#region Ctor

		/// <summary>
		/// Creates a new <see cref="ScreenVideoPacket"/> instance.
		/// </summary>
		public ScreenVideoPacket()
		{
		}

		/// <summary>
		/// Creates a new <see cref="ScreenVideoPacket"/> instance.
		/// </summary>
		/// <param name="blockWidth">Width of the block.</param>
		/// <param name="imageWidth">Width of the image.</param>
		/// <param name="blockHeight">Height of the block.</param>
		/// <param name="imageHeight">Height of the image.</param>
		/// <param name="blocks">Blocks.</param>
		public ScreenVideoPacket(int blockWidth, int imageWidth, 
			int blockHeight, int imageHeight, ImageBlock[] blocks)
		{
			this.blockWidth = blockWidth;
			this.blockHeight = blockHeight;
			this.imageWidth = imageWidth;
			this.imageHeight = imageHeight;
			this.blocks = blocks;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the blocks.
		/// </summary>
		public ImageBlock[] Blocks
		{
			get { return this.blocks;  }
			set { this.blocks = value; }
		}

		/// <summary>
		/// Gets or sets the height of the image.
		/// </summary>
		public int ImageHeight
		{
			get { return this.imageHeight;  }
			set { this.imageHeight = value; }
		}

		/// <summary>
		/// Gets or sets the width of the image.
		/// </summary>
		public int ImageWidth
		{
			get { return this.imageWidth;  }
			set { this.imageWidth = value; }
		}

		/// <summary>
		/// Gets or sets the height of the block.
		/// </summary>
		public int BlockHeight
		{
			get { return this.blockHeight;  }
			set { this.blockHeight = value; }
		}

		/// <summary>
		/// Gets or sets the width of the block.
		/// </summary>
		public int BlockWidth
		{
			get { return this.blockWidth;  }
			set { this.blockWidth = value; }
		}

		#endregion

		#region Methods

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns></returns>
        public override int GetSizeOf()
        {
            //TODO
            return 0;
        }

        /// <summary>
        /// Writes to.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public override void WriteTo(BinaryWriter writer)
        {
            //TODO
        }

        /// <summary>
        /// Reads the data from the binary reader.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        public override void ReadData(BufferedBinaryReader binaryReader)
        {
            byte[] b = binaryReader.ReadBytes(4);
            BitArray ba = BitParser.GetBitValues(b);
			
            blockWidth = (int)BitParser.ReadUInt32(ba, 0, 4);
            imageWidth = (int)BitParser.ReadUInt32(ba, 4, 12);
            blockHeight = (int)BitParser.ReadUInt32(ba, 16, 4);
            imageHeight = (int)BitParser.ReadUInt32(ba, 20, 12);

            int nbWBlock = 0;
            int nbBlockWInt = imageWidth / blockWidth;
            float nbBlockWDec = imageWidth / blockWidth;
            if (nbBlockWInt == nbBlockWDec)
                nbWBlock = nbBlockWInt;
            else
                nbWBlock = nbBlockWInt + 1;

            int nbHBlock = 0;
            int nbBlockHInt = imageHeight / blockHeight;
            float nbBlockHDec = imageHeight / blockHeight;
            if (nbBlockHInt == nbBlockHDec)
                nbHBlock = nbBlockHInt;
            else
                nbHBlock = nbBlockHInt + 1;

            int nbBlock = nbWBlock * nbHBlock;

            if (nbBlock > 0)
            {
                blocks = new ImageBlock[nbBlock];

                for (int i = 0; i < nbBlock; i++)
                {
                    blocks[i] = new ImageBlock();
                    blocks[i].ReadData(binaryReader);
                }
            }
        }

        /// <summary>
        /// Serializes to the specified writer.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public override void Serialize(XmlWriter writer)
        {
            //TODO
        }

		#endregion
	}

	/// <summary>
	/// Video ImageBlock class
	/// </summary>
	public class ImageBlock
	{
		#region Members

		private int dataSize;
		private byte[] data;

		#endregion

		#region Ctor
		
		/// <summary>
		/// Creates a new <see cref="ImageBlock"/> instance.
		/// </summary>
		public ImageBlock()
		{
		}

		/// <summary>
		/// Creates a new <see cref="ImageBlock"/> instance.
		/// </summary>
		/// <param name="data">Data.</param>
		public ImageBlock(byte[] data)
		{
			this.data = data;
			this.dataSize = data.Length;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		public byte[] Data
		{
			get { return this.data;  }
			set { this.data = value; }
		}
			
		#endregion

		#region Methods

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns>size of the structure</returns>
		public int GetSizeOf()
		{
			int res = 2;
			if (data != null)
				res += data.Length;
			return res;
		}

        /// <summary>
        /// Writes to a binary writer.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public void WriteTo(BinaryWriter writer)
        {
            //TODO
        }

        /// <summary>
        /// Reads the data from a binary reader.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        public void ReadData(BufferedBinaryReader binaryReader)
        {
            byte[] b = binaryReader.ReadBytes(2);
            BitArray ba = BitParser.GetBitValues(b);
			
            dataSize = (int)BitParser.ReadUInt32(ba, 0, 16);
			
            data = null;
            if (dataSize != 0)
            {
                data = new byte[dataSize];
                for (int i = 0; i < dataSize; i++)
                    data[i] = binaryReader.ReadByte();
            }
        }

		#endregion
	}
}
