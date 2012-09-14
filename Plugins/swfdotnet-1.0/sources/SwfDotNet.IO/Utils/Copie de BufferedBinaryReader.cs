/*
	SwfDotNet is an open source library for writing and reading 
	Macromedia Flash (SWF) bytecode.
	Copyright (C) 2005 Olivier Carpentier.
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
using System.Text;

namespace SwfDotNet.IO.Utils
{
	/// <summary>
	/// BufferedBinaryReader.
	/// This class extends a binaryReader to provide
	/// the way to read bit per bit a binary stream.
	/// This class use a buffer to do it.
	/// ATTENTION: By default, this class works
	/// with the LittleEndian mode (for x86).
	/// </summary>
	public class BufferedBinaryReader: BinaryReader
	{
        #region Members

        /// <summary>
        /// Bit buffer current position 
        /// </summary>
        private int bitPos = 0;
        
        /// <summary>
        /// Bit buffer
        /// </summary>
        private uint bitBuf = 0;

        /// <summary>
        /// Use littleEndian or big endian
        /// </summary>
        private bool littleEndian = true;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="BufferedBinaryReader"/> instance.
        /// </summary>
        /// <param name="stream">Stream.</param>
        public BufferedBinaryReader(Stream stream): base(stream)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating if this binary
        /// reader works with the littleEndian encoding.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [little endian]; otherwise, <c>false</c>.
        /// </value>
        public bool LittleEndian
        {
            get { return this.littleEndian;  }
            set { this.littleEndian = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Synchronizes the bits.
        /// </summary>
        public void SynchBits()
        {
            bitBuf = 0;
            bitPos = 0;
        }

        /// <summary>
        /// Reads the byte.
        /// </summary>
        /// <returns></returns>
        public override byte ReadByte()
        {
            SynchBits();
            return base.ReadByte();
        }

        /// <summary>
        /// Reads an Unsigned int16.
        /// </summary>
        /// <returns></returns>
        public override ushort ReadUInt16()
        {
            SynchBits();
            return base.ReadUInt16();
        }
 
        /// <summary>
        /// Reads an Unsigned int32.
        /// </summary>
        /// <returns></returns>
        public override uint ReadUInt32()
        {
            SynchBits();
            return base.ReadUInt32();
        }

        /// <summary>
        /// Reads an Unsigned int64.
        /// </summary>
        /// <returns></returns>
        public override ulong ReadUInt64()
        {
            SynchBits();
            return base.ReadUInt64();
        }

		/// <summary>
		/// Peeks the byte.
		/// </summary>
		/// <returns></returns>
		public byte PeekByte()
		{
			byte val = ReadByte();
			base.BaseStream.Seek(-1, System.IO.SeekOrigin.Current);
			return val;
		}

        /// <summary>
        /// Peeks the bytes.
        /// </summary>
        /// <param name="numToPeek">Num to peek.</param>
        public byte[] PeekBytes(uint numToPeek)
        {
            if (numToPeek <= 0)
                return null;
            byte[] res = new byte[numToPeek];
            for (int i = 0; i < numToPeek; i++)
                res[i] = PeekByte();
            return res;
        }

        /// <summary>
        /// Peeks an Unsigned int16.
        /// </summary>
        /// <returns></returns>
        public ushort PeekUInt16()
        {
            ushort val = ReadUInt16();
            base.BaseStream.Seek(-2, System.IO.SeekOrigin.Current);
            return val;
        }

        /// <summary>
        /// Peeks an Unsigned int32.
        /// </summary>
        /// <returns></returns>
        public uint PeekUInt32()
        {
            uint val = ReadUInt32();
            base.BaseStream.Seek(-4, System.IO.SeekOrigin.Current);
            return val;
        }

        /// <summary>
        /// Reads the char.
        /// </summary>
        /// <returns></returns>
        public override char ReadChar()
        {
            SynchBits();
            return (char)(ReadByte());
        }

        /// <summary>
        /// Reads the int16.
        /// </summary>
        /// <returns></returns>
        public override short ReadInt16()
        {
            SynchBits();

            return base.ReadInt16();
        }

        /// <summary>
        /// Reads the int32.
        /// </summary>
        /// <returns></returns>
        public override int ReadInt32()
        {
            SynchBits();
            
            return base.ReadInt32();
        }

        /// <summary>
        /// Reads the int64.
        /// </summary>
        /// <returns></returns>
        public override long ReadInt64()
        {
            SynchBits();
            return base.ReadInt64();
        }

        /// <summary>
        /// Reads the float.
        /// </summary>
        /// <returns></returns>
        public float ReadFloat()
        {
            SynchBits();
            return base.ReadSingle();
        }

        /// <summary>
        /// Reads the float.
        /// </summary>
        /// <param name="numBits">Bits number.</param>
        /// <returns></returns>
        public float ReadFloat(uint numBits)
        {
            float value = 0;
            float divisor = (float)(1 << 16);
	
            value = ((float)ReadSBits(numBits)) / divisor;
            return value;
        }

        /// <summary>
        /// Reads the float.
        /// </summary>
        /// <param name="numBits">Num bits.</param>
        /// <param name="fractionSize">Size of the fraction.</param>
        /// <returns></returns>
        public float ReadFloat(uint numBits, int fractionSize)
        {
            float value = 0;
            float divisor = (float)(1 << fractionSize);
	
            value = ((float)ReadSBits(numBits)) / divisor;
            return value;
        }

        /// <summary>
        /// Reads the float word.
        /// </summary>
        /// <param name="mantissaSize">Size of the mantissa.</param>
        /// <param name="fractionSize">Size of the fraction.</param>
        /// <returns></returns>
        public float ReadFloatWord(uint mantissaSize, uint fractionSize)
        {
            int mantissa = 0;
            int fraction = 0;
            
            float value = 0;
            float divisor = (float)(1 << (int)fractionSize);
    	
            fraction = (int)ReadUBits(fractionSize);
            mantissa = (int)ReadSBits(mantissaSize);
            
            mantissa <<= (int)fractionSize;   
            value = (mantissa + fraction) / divisor;
    	    
            return value;
        }

        /// <summary>
        /// Reads Unsigned bits.
        /// </summary>
        /// <param name="bits">Bits.</param>
        /// <returns></returns>
        public uint ReadUBits(uint bits)
        {
            uint v = 0;

            while (true)
            {
                int s = (int)(bits - bitPos);

                if (s > 0)
                {
                    v |= bitBuf << s;
                    bits -= (uint)bitPos;

                    bitBuf = ReadByte();
                    bitPos = 8;
                }
                else
                {
                    v |= bitBuf >> -s;

                    bitPos -= (int)bits;
                    bitBuf &= (uint)(0xff >> (8 - bitPos));

                    return v;
                }
            }
        }

        /// <summary>
        /// Reads Signed bits.
        /// </summary>
        /// <param name="bits">Bits.</param>
        /// <returns></returns>
        public int ReadSBits(uint bits)
        {
            int v = (int)(ReadUBits(bits));

            if ((v & (1L << (int)(bits - 1))) > 0)
            {
                v |= -1 << (int)bits;
            }

            return v;
        }

        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <returns></returns>
        public override string ReadString()
        {
            SynchBits();

            StringBuilder sb = new StringBuilder();

            char c = 'a';
            while ((c = ReadChar()) != 0)
                sb.Append(c);
            
            return sb.ToString();
        }

        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <param name="numChars">Num chars to read.</param>
        /// <returns></returns>
        public string ReadString(uint numChars)
        {
            SynchBits();

            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < numChars; i++)
            {
                char c = ReadChar();
                if (c != '\0')
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Reads the bool.
        /// </summary>
        /// <returns></returns>
        public override bool ReadBoolean()
        {
            return (ReadUBits(1) == 1) ? true : false;
        }

        /// <summary>
        /// Skips the specified bytes.
        /// </summary>
        /// <param name="bytes">Bytes.</param>
        public void Skip(uint bytes)
        {
            SynchBits();

            BaseStream.Seek(bytes, System.IO.SeekOrigin.Current);
        }

        /// <summary>
        /// Seeks the specified position.
        /// </summary>
        /// <param name="position">Position.</param>
        public void Seek(uint position)
        {
            SynchBits();

            BaseStream.Seek(position, System.IO.SeekOrigin.Begin);
        }

        #endregion
	}
}
