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
using System.Collections;

using SwfDotNet.IO.Exceptions;

namespace SwfDotNet.IO.Utils
{
	/// <summary>
	/// BufferedBinaryWriter class.
    /// This class extends a binaryWriter to provide
    /// the way to read bit per bit a binary stream.
    /// This class use a buffer to do it.
    /// ATTENTION: By default, this writer works with 
    /// LittleEndian mode (for x86).
	/// </summary>
	public class BufferedBinaryWriter: BinaryWriter
	{
        #region Members
    
        /// <summary>
        /// Bit buffer current position 
        /// </summary>
        private int bitPos = 0;
        
        /// <summary>
        /// Bit buffer
        /// </summary>
        private int bitBuf = 0;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="BufferedBinaryWriter"/> instance.
        /// </summary>
        /// <param name="stream">Stream.</param>
		public BufferedBinaryWriter(Stream stream): base(stream)
		{
		}

        /// <summary>
        /// Creates a new <see cref="BufferedBinaryWriter"/> instance.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="encodingMode">Encoding mode.</param>
        public BufferedBinaryWriter(Stream stream, Encoding encodingMode): base(stream, encodingMode)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Synchronizes the bits.
        /// </summary>
        public void SynchBits()
        {
            if (bitPos != 0)
			    base.Write((byte)this.bitBuf);
            bitBuf = 0;
            bitPos = 0;
        }

        /// <summary>
        /// Gets the num bits.
        /// </summary>
        /// <param name="floatValue">Value.</param>
        /// <returns></returns>
        public static uint GetNumBits(float floatValue)
        {
			floatValue = floatValue * 65536.0f;
			return GetNumBits((int)floatValue, true);
        }

		/// <summary>
		/// Gets the num bits.
		/// </summary>
		/// <param name="number">Number.</param>
		/// <returns></returns>
		public static uint GetNumBits(ushort number)
		{
			return GetNumBits((long)number, false);
		}

		/// <summary>
		/// Gets the num bits.
		/// </summary>
		/// <param name="number">Number.</param>
		/// <returns></returns>
		public static uint GetNumBits(short number)
		{
			return GetNumBits((long)number, true);
		}

		/// <summary>
		/// Gets the num bits.
		/// </summary>
		/// <param name="number">Number.</param>
		/// <returns></returns>
		public static uint GetNumBits(uint number)
		{
			return GetNumBits((long)number, false);
		}

		/// <summary>
		/// Gets the num bits.
		/// </summary>
		/// <param name="number">Number.</param>
		/// <returns></returns>
		public static uint GetNumBits(int number)
		{
			return GetNumBits((long)number, true);
		}

		/// <summary>
		/// Gets the num bits.
		/// </summary>
		/// <param name="number">Number.</param>
		/// <returns></returns>
		public static uint GetNumBits(long number)
		{
			return GetNumBits(number, number < 0);
		}

		/// <summary>
		/// Gets the num bits.
		/// </summary>
		/// <param name="number">Number.</param>
		/// <param name="signed">Signed.</param>
		/// <returns></returns>
		public static uint GetNumBits(long number, bool signed) 
		{
			number = Math.Abs(number);

			long x = 1;
			uint i;

			for(i = 1; i <= 64; i++) 
			{
				x <<= 1;
				if (x > number) 
					break;
			}

			return (uint)(i + ((signed) ? 1 : 0));
		}

        /// <summary>
        /// Writes the string.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="size">Size.</param>
        public void WriteString(string value, uint size)
        {
            SynchBits();
            for (int i = 0; i < size; i++)
            {
                if (i < value.Length)
                    Write((byte)value[i]);
                else
                    Write((byte)(' '));
            }
        }

        /// <summary>
        /// Writes the string.
        /// </summary>
        /// <param name="value">Value.</param>
        public void WriteString(string value)
        {
            if (value == null)
                return;

            SynchBits();

            for (int i = 0; i < value.Length; i++)
                base.Write((byte)value[i]);

            base.Write((byte)'\0');
        }

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public override void Flush()
        {
            SynchBits();
            base.Flush ();
        }

		/// <summary>
		/// Writes the boolean.
		/// </summary>
		/// <param name="value">Value.</param>
		public void WriteBoolean(bool value)
		{
			WriteUBits((long)((value) ? 1 : 0), 1);
		}

		/// <summary>
		/// Writes unsigned bits.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="numberOfBits">Number of bits.</param>
		public void WriteUBits(uint value, uint numberOfBits)
		{
			WriteUBits((long)value, numberOfBits);
		}

		/// <summary>
		/// Writes unsigned bits.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="numberOfBits">Number of bits.</param>
		public void WriteUBits(ushort value, uint numberOfBits)
		{
			WriteUBits((long)value, numberOfBits);
		}

		/// <summary>
		/// Writes unsigned bits.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="numberOfBits">Number of bits.</param>
		public void WriteUBits(long value, uint numberOfBits)
		{
			if (numberOfBits == 0)
				return;
			if (bitPos == 0) 
				bitPos = 8;

			int bitNum = (int)numberOfBits;

			while (bitNum > 0) 
			{
				while ((bitPos > 0) && (bitNum > 0)) 
				{
					long or = (value & (1L << (bitNum - 1)));
					int shift = bitPos - bitNum;
					if (shift < 0) 
						or >>= -shift;
					else 
						or <<= shift;
					bitBuf |= (int)or;

					bitNum--;
					bitPos--;
				}

				if( bitPos == 0 )
				{
					base.Write((byte)bitBuf);
					bitBuf = 0;
					if (bitNum > 0) 
						bitPos = 8;
				}
			}
		}

		/// <summary>
		/// Writes signed bits.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="numberOfBits">Number of bits.</param>
		public void WriteSBits(int value, uint numberOfBits)
		{
			WriteSBits((long)value, numberOfBits);
		}

		/// <summary>
		/// Writes signed bits.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="numberOfBits">Number of bits.</param>
		public void WriteSBits(short value, uint numberOfBits)
		{
			WriteSBits((long)value, numberOfBits);
		}

        /// <summary>
        /// Writes signed bits.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="numberOfBits">Number of bits.</param>
        public void WriteSBits(long value, uint numberOfBits)
        {
			long tmp = value & 0x7FFFFFFF;

			if (value < 0)
				tmp |= (1L << (int)numberOfBits - 1);
			
            WriteUBits(tmp, numberOfBits);
        }
       
        /// <summary>
        /// Writes the Fixed bits.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="numberOfBits">Number of bits.</param>
        public void WriteFBits(float value, uint numberOfBits)
        {
			long tmp = (long)(value * 0x10000);
			WriteSBits(tmp, numberOfBits);
        }

        /// <summary>
        /// Writes the Fixed word.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="mantissaSize">Size of the mantissa.</param>
        /// <param name="fractionSize">Size of the fraction.</param>
        public void WriteFWord(float value, uint mantissaSize, uint fractionSize)
        {
            int mantissa = 0;
            uint fraction = 0;
        
            float multiplier = (float)(1 << (int)fractionSize);
	
            fraction = (uint)(value * multiplier);
            mantissa = (int)value;

            WriteUBits(fraction, fractionSize);
            WriteSBits(mantissa, mantissaSize);
        }

        #endregion

        #region Overrided methods
        
        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public override void Write(byte value)
        {
            this.SynchBits();
            base.Write (value);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public override void Write(sbyte value)
        {
            this.SynchBits();
            base.Write (value);
        }
        
        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public override void Write(short value)
        {
            this.SynchBits();
            base.Write (value);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public override void Write(ushort value)
        {
            this.SynchBits();
            base.Write (value);
        }

        /// <summary>
        /// Writes at.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="position">Position.</param>
        public void WriteAt(ushort value, long position)
        {
            this.SynchBits();
            long currentPosition = this.BaseStream.Position;
            this.BaseStream.Position = position;
            Write(value);
            this.BaseStream.Position = currentPosition;
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public override void Write(int value)
        {
            this.SynchBits();
            base.Write (value);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public override void Write(uint value)
        {
            this.SynchBits();
            base.Write (value);
        }

        /// <summary>
        /// Writes at.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="position">Position.</param>
        public void WriteAt(uint value, long position)
        {
            this.SynchBits();
            long currentPosition = this.BaseStream.Position;
            this.BaseStream.Position = position;
            Write(value);
            this.BaseStream.Position = currentPosition;
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public override void Write(string value)
        {
            this.SynchBits();
            base.Write (value);
        }
        
        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public override void Write(decimal value)
        {
            this.SynchBits();
            base.Write (value);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public override void Write(double value)
        {
            this.SynchBits();
            base.Write (value);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public override void Write(float value)
        {
            this.SynchBits();
            base.Write (value);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public override void Write(long value)
        {
            this.SynchBits();
            base.Write (value);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public override void Write(ulong value)
        {
            this.SynchBits();
            base.Write (value);
        }

        /// <summary>
        /// Writes the specified ch.
        /// </summary>
        /// <param name="ch">Ch.</param>
        public override void Write(char ch)
        {
            this.SynchBits();
            base.Write (ch);
        }

        /// <summary>
        /// Writes the specified chars.
        /// </summary>
        /// <param name="chars">Chars.</param>
        public override void Write(char[] chars)
        {
            this.SynchBits();
            base.Write (chars);
        }

        /// <summary>
        /// Writes the specified chars.
        /// </summary>
        /// <param name="chars">Chars.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        public override void Write(char[] chars, int index, int count)
        {
            this.SynchBits();
            base.Write (chars, index, count);
        }

        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        public override void Write(byte[] buffer, int index, int count)
        {
            this.SynchBits();
            base.Write (buffer, index, count);
        }

        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        public override void Write(byte[] buffer)
        {
            this.SynchBits();
            base.Write (buffer);
        }

        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        public void Write(sbyte[] buffer)
        {
            this.SynchBits();

            if (buffer == null)
                return;
            
            foreach (sbyte sb in buffer)
                this.Write(sb);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public override void Write(bool value)
        {
            this.SynchBits();
            base.Write(value);
        }

        /// <summary>
        /// Seeks the specified offset.
        /// </summary>
        /// <param name="offset">Offset.</param>
        /// <param name="origin">Origin.</param>
        /// <returns></returns>
        public override long Seek(int offset, SeekOrigin origin)
        {
            this.SynchBits();
            return base.Seek (offset, origin);
        }


        #endregion
	}
}
