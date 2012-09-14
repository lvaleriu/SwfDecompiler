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
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

using SwfDotNet.IO.Utils;
using SwfDotNet.IO.Tags;
using SwfDotNet.IO.Tags.Types;
using SwfDotNet.IO.Exceptions;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace SwfDotNet.IO.Tags 
{
	/// <summary>
	/// DefineBitsLossLessTag  is used to define an image compressed using the 
	/// lossless zlib compression algorithm. 
	/// </summary>
	/// <remarks>
	/// <p>
	/// The class supports color-mapped images where the image data contains
	/// an index into a color table or images where the image data specifies 
	/// the color directly.
	/// </p>
	/// <p>
	/// For color-mapped images the color table contains up to 256, 24-bit colors. 
	/// The image contains one byte for each pixel which is an index into the table 
	/// to specify the color for that pixel. The color table and the image data 
	/// are compressed as a single block, with the color table placed before the image.
	/// </p>
	/// <p>
	/// For images where the color is specified directly, the image data contains 
	/// either 16 or 24 bit color values. For 16-bit color values the most significant 
	/// bit is zero followed by three, 5-bit fields for the red, green and blue channels.
	/// </p>
	/// <p>
	/// Four bytes are used to represent 24-bit colors. The first byte is always set to 
	/// zero and the following bytes contain the color values for the red, green and 
	/// blue color channels.
	/// </p>
	/// <p>
	/// The number of bytes in each row of an image must be aligned to a 32-bit word 
	/// boundary. For example if an image if an icon is 25 pixels wide, then for an 8-bit 
	/// color mapped image an additional three bytes (0x00) must be used to pad each row; 
	/// for a 16-bit direct mapped color image an additional two bytes must be used as 
	/// padding.
	/// </p>
	/// <p>
	/// The image data is stored in zlib compressed form within the object. For 
	/// color-mapped images the compressed data contains the color table followed by the 
	/// image data. The color table is omitted for direct-mapped images.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 2.
	/// </p>
	/// </remarks>
	public class DefineBitsLossLessTag : BaseTag, DefineTag
	{
		#region Members

		private ushort _characterId;
		private byte _bitmapFormat;
		private ushort _bitmapWidth;
		private ushort _bitmapHeight;
		private byte _bitmapColorTableSize;
		private ColorMapData _colorMapData = null;
		private BitmapColorData _bitmapColorData = null;

		#endregion

		#region Ctor
		
		/// <summary>
		/// Creates a new <see cref="DefineBitsLossLessTag"/> instance.
		/// </summary>
		public DefineBitsLossLessTag()
		{
			_tagCode = (int)TagCodeEnum.DefineBitsLossLess;
		}

		/// <summary>
		/// constructor.
		/// </summary>
		/// <param name="characterId">id for this character</param>
		/// <param name="bitmapFormat">Format of compressed data</param>
		/// <param name="bitmapWidth">Width of bitmap image</param>
		/// <param name="bitmapHeight">Height of bitmap image</param>
		/// <param name="bitmapColorTableSize">actual number of colors in the color table</param>
		public DefineBitsLossLessTag(ushort characterId, byte bitmapFormat, ushort bitmapWidth,
			ushort bitmapHeight, byte bitmapColorTableSize) 
		{
			_characterId = characterId;
			_bitmapFormat = bitmapFormat;
			_bitmapWidth = bitmapWidth;
			_bitmapHeight = bitmapHeight;
			_bitmapColorTableSize = bitmapColorTableSize;
			_tagCode = (int)TagCodeEnum.DefineBitsLossLess;
		}

		/// <summary>
		/// constructor.
		/// </summary>
		/// <param name="characterId">id for this character</param>
		/// <param name="bitmapFormat">Format of compressed data</param>
		/// <param name="bitmapWidth">Width of bitmap image</param>
		/// <param name="bitmapHeight">Height of bitmap image</param>
		/// <param name="bitmapColorTableSize">actual number of colors in the color table</param>
		/// <param name="zlibBitmapData">zlib compressed bitmap data</param>
		public DefineBitsLossLessTag(ushort characterId, byte bitmapFormat, ushort bitmapWidth,
			ushort bitmapHeight, byte bitmapColorTableSize, ColorMapData zlibBitmapData) 
		{
			_characterId = characterId;
			_bitmapFormat = bitmapFormat;
			_bitmapWidth = bitmapWidth;
			_bitmapHeight = bitmapHeight;
			_bitmapColorTableSize = bitmapColorTableSize;
			_colorMapData = zlibBitmapData;
			_tagCode = (int)TagCodeEnum.DefineBitsLossLess;
		}

		/// <summary>
		/// constructor.
		/// </summary>
		/// <param name="characterId">id for this character</param>
		/// <param name="bitmapFormat">Format of compressed data</param>
		/// <param name="bitmapWidth">Width of bitmap image</param>
		/// <param name="bitmapHeight">Height of bitmap image</param>
		/// <param name="bitmapColorTableSize">actual number of colors in the color table</param>
		/// <param name="zlibBitmapData">zlib compressed bitmap data</param>
		public DefineBitsLossLessTag(ushort characterId, byte bitmapFormat, ushort bitmapWidth,
			ushort bitmapHeight, byte bitmapColorTableSize, BitmapColorData zlibBitmapData) 
		{
			_characterId = characterId;
			_bitmapFormat = bitmapFormat;
			_bitmapWidth = bitmapWidth;
			_bitmapHeight = bitmapHeight;
			_bitmapColorTableSize = bitmapColorTableSize;
			_bitmapColorData = zlibBitmapData;
			_tagCode = (int)TagCodeEnum.DefineBitsLossLess;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the bitmap format.
		/// </summary>
		public byte BitmapFormat
		{
			get { return this._bitmapFormat;  }
			set { this._bitmapFormat = value; }
		}
    
		/// <summary>
		/// Gets or sets the color map data.
		/// </summary>
		public ColorMapData ColorMapData
		{
			get { return this._colorMapData;  }
			set { this._colorMapData = value; }
		}

		/// <summary>
		/// Gets or sets the bitmap data.
		/// </summary>
		public BitmapColorData BitmapData
		{
			get { return this._bitmapColorData;  }
			set { this._bitmapColorData = value; }
		}

		/// <summary>
		/// Gets or sets the width of the bitmap.
		/// </summary>
		public ushort BitmapWidth
		{
			get { return this._bitmapWidth;  }
			set { this._bitmapWidth = value; }
		}

		/// <summary>
		/// Gets or sets the height of the bitmap.
		/// </summary>
		public ushort BitmapHeight
		{
			get { return this._bitmapHeight;  }
			set { this._bitmapHeight = value; }
		}

		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.DefineTag"/>
		/// </summary>
		public ushort CharacterId
		{
			get { return this._characterId;  }
			set { this._characterId = value; }
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

			int beforePos = (int)binaryReader.BaseStream.Position;
			int toReaded = (int)rh.TagLength - 7;
			
			_characterId = binaryReader.ReadUInt16();
			_bitmapFormat = binaryReader.ReadByte();
			_bitmapWidth = binaryReader.ReadUInt16();
			_bitmapHeight = binaryReader.ReadUInt16();
			_bitmapColorTableSize = 0;

			if (_bitmapFormat == 3)
			{
				_bitmapColorTableSize = binaryReader.ReadByte();
				toReaded--;
			}

			if (_bitmapFormat == 3)
			{
				_colorMapData = new ColorMapData();
				_colorMapData.ReadData(binaryReader, _bitmapColorTableSize, _bitmapWidth, _bitmapHeight, toReaded);
			}
			else if (_bitmapFormat == 4 || _bitmapFormat == 5)
			{
				int imageSize = _bitmapWidth * _bitmapHeight;
				int uncompressedSize = imageSize;
				if (_bitmapFormat == 4)
					uncompressedSize *= 2;
				else
					uncompressedSize *= 4;
				
				byte[] uncompressed = new byte[uncompressedSize];
				byte[] compressed = binaryReader.ReadBytes(toReaded);	
				Inflater zipInflator = 	new Inflater();
				zipInflator.SetInput(compressed);
				zipInflator.Inflate(uncompressed, 0, uncompressedSize);

				_bitmapColorData = null;
				if (_bitmapFormat == 4)
				{
					Pix15[] bitmapPixelData = new Pix15[imageSize];
					for (int i = 0, j = 0; i < imageSize; i++, j += 2)
					{
						byte[] data = new byte[2] {uncompressed[j], uncompressed[j+1]};
						bitmapPixelData[i] = new Pix15(data);
					}
					_bitmapColorData = new BitmapColorData(bitmapPixelData);
				}
				else
				{
					Pix24[] bitmapPixelData = new Pix24[imageSize];
					for (int i = 0, j = 0; i < imageSize; i++, j += 4)
					{
						byte reserved = uncompressed[j];
						byte red = uncompressed[j + 1];
						byte green = uncompressed[j + 2];
						byte blue = uncompressed[j + 3];
						bitmapPixelData[i] = new Pix24(red, green, blue);
					}
					_bitmapColorData = new BitmapColorData(bitmapPixelData);
				}
			}
        }

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns></returns>
		public int GetSizeOf(int sizeOfCompressedData)
		{
			int length = 7;
			if (this._bitmapFormat == 3)
			{
				length++;
				length += sizeOfCompressedData;
			}
			else if (this._bitmapFormat == 4 || this._bitmapFormat == 5)
			{
				length += sizeOfCompressedData;
			}
			return length;
		}
		
		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
		{			
			if (version < 2)
				return;

            // Compression process
            int lenghtOfCompressedBlock = 0;
            byte[] compressArray = null;
            MemoryStream unCompressedStream = new MemoryStream();
            BufferedBinaryWriter unCompressedWriter = new BufferedBinaryWriter(unCompressedStream);
            
            if (this._bitmapFormat == 3)
            {
                this._colorMapData.WriteTo(unCompressedWriter);
            }
            else if (this._bitmapFormat == 4 || this._bitmapFormat == 5)
            {
                this._bitmapColorData.WriteTo(unCompressedWriter);
			}

            MemoryStream compressedStream = new MemoryStream();
            DeflaterOutputStream ouput = new DeflaterOutputStream(compressedStream);
            byte[] unCompressArray = unCompressedStream.ToArray();
            ouput.Write(unCompressArray, 0, unCompressArray.Length);
            ouput.Finish();
            compressArray = compressedStream.ToArray();
            lenghtOfCompressedBlock = compressArray.Length;
            ouput.Close();
            unCompressedStream.Close();

            //Writing process
			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
			RecordHeader rh = new RecordHeader(TagCode, GetSizeOf(lenghtOfCompressedBlock));
			
			rh.WriteTo(w);
			w.Write(this._characterId);
			w.Write(this._bitmapFormat);
			w.Write(this._bitmapWidth);
			w.Write(this._bitmapHeight);
			
            if (this._bitmapFormat == 3)
            {
                w.Write(this._bitmapColorTableSize);
                w.Write(compressArray);
            }
            else if (this._bitmapFormat == 4 || this._bitmapFormat == 5)
            {
                w.Write(compressArray);
            }

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
			writer.WriteStartElement("DefineBitsLossLessTag");
			writer.WriteAttributeString("CharacterId", this._characterId.ToString());
			writer.WriteElementString("BitmapFormat", this._bitmapFormat.ToString());
			writer.WriteElementString("BitmapWidth", this._bitmapWidth.ToString());
			writer.WriteElementString("BitmapHeight", this._bitmapHeight.ToString());
			writer.WriteEndElement();
		}

		#endregion

		#region Compile & Decompile Methods

		/// <summary>
		/// Construct a new DefineBitsLossLessTag object 
		/// from a file.
		/// </summary>
		/// <param name="characterId">Character id.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <returns></returns>
		public static DefineBitsLossLessTag FromFile(ushort characterId, string fileName)
		{
			FileStream stream = File.OpenRead(fileName);
			DefineBitsLossLessTag res = FromImage(characterId, Image.FromStream(stream));
			stream.Close();
			return res;
		}

		/// <summary>
		/// Construct a new DefineBitsLossLessTag object 
		/// from a stream.
		/// </summary>
		/// <param name="characterId">Character id.</param>
		/// <param name="stream">Stream.</param>
		/// <returns></returns>
		public static DefineBitsLossLessTag FromStream(ushort characterId, Stream stream)
		{
			DefineBitsLossLessTag res = FromImage(characterId, Image.FromStream(stream));
			return res;
		}

		/// <summary>
		/// Construct a new DefineBitsLossLessTag object 
		/// from an image object.
		/// </summary>
		/// <param name="characterId">Character id.</param>
		/// <param name="image">Image.</param>
		/// <returns></returns>
		public static DefineBitsLossLessTag FromImage(ushort characterId, Image image)
		{
			if (image.RawFormat.Equals(ImageFormat.Bmp) == false &&
				image.RawFormat.Equals(ImageFormat.MemoryBmp) == false)
				throw new InvalidImageFormatException();
			
			Bitmap bitmap = (Bitmap)image;
			byte format = 0;
			PixelFormat pxFormat = bitmap.PixelFormat;
			if (pxFormat.Equals(PixelFormat.Format8bppIndexed))
				format = 3;
			else if (pxFormat.Equals(PixelFormat.Format16bppRgb555) ||
				pxFormat.Equals(PixelFormat.Format16bppRgb565))
				format = 4;
			else if (pxFormat.Equals(PixelFormat.Format24bppRgb))
				format = 5;
			else
				throw new InvalidPixelFormatException();

			DefineBitsLossLessTag bmp = new DefineBitsLossLessTag();
			bmp.CharacterId = characterId;
			bmp.BitmapFormat = format;
			bmp.BitmapWidth = (ushort)bitmap.Width;
			bmp.BitmapHeight = (ushort)bitmap.Height;

			int imageSize = bitmap.Width * bitmap.Height;

			if (bmp.BitmapFormat == 3)
			{
				//TODO
			}
			else if (bmp.BitmapFormat == 4)
			{
				Pix15[] bitmapPixelData = new Pix15[imageSize];
				int k = 0;
				for (int i = 0; i < bitmap.Height; i++)
				{
					for (int j = 0; j < bitmap.Width; j++)
					{
						Color color = bitmap.GetPixel(j, i);
						bitmapPixelData[k] = new Pix15((byte)color.R, (byte)color.G, (byte)color.B);
						k++;
					}
				}
				bmp.BitmapData = new BitmapColorData(bitmapPixelData);
			}
			else if	(bmp.BitmapFormat == 5)
			{
				Pix24[] bitmapPixelData = new Pix24[imageSize];
				int k = 0;
				for (int i = 0; i < bitmap.Height; i++)
				{
					for (int j = 0; j < bitmap.Width; j++)
					{
						Color color = bitmap.GetPixel(j, i);
						bitmapPixelData[k] = new Pix24((byte)color.R, (byte)color.G, (byte)color.B);
						k++;
					}
				}
				bmp.BitmapData = new BitmapColorData(bitmapPixelData);
			}

			return bmp;
		}

		/// <summary>
		/// Decompiles to file.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		public void DecompileToFile(string fileName)
		{
			Stream stream = File.OpenWrite(fileName);
			DecompileToStream(stream);
			stream.Close();
		}

		/// <summary>
		/// Decompiles to image.
		/// </summary>
		/// <returns></returns>
		public Image DecompileToImage()
		{
			MemoryStream stream = new MemoryStream();
			DecompileToStream(stream);
			return Image.FromStream(stream);           
		}

		/// <summary>
		/// Decompiles to stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		private void DecompileToStream(Stream stream)
		{
			if (BitmapFormat == 3)
			{
				//ColorMapData
				ColorMapData mapData = ColorMapData;
				Bitmap bmp = new Bitmap((int)BitmapWidth, (int)BitmapHeight);
				int k = 0;
				for (int i = 0; i < (int)BitmapHeight; i++)
				{
					for (int j = 0; j < (int)BitmapWidth; j++)
					{
						int index = (int)mapData.ColorMapPixelData[k];
						if (index >= 0 && index < mapData.ColorTableRGB.Length)
							bmp.SetPixel(j, i, mapData.ColorTableRGB[index].ToWinColor());
						else
							bmp.SetPixel(j, i, Color.Black);
						k++;
					}
				}
				bmp.Save(stream, ImageFormat.Bmp); 
				bmp.Dispose();
			}
			else
			{
				//BitmapData
				BitmapColorData bitmapData = BitmapData;
				Pix[] data = null;
				if (BitmapFormat == 5)
					data = bitmapData.bitmapPixelDataPix24;
				else
					data = bitmapData.bitmapPixelDataPix15;

				if (data == null)
					return;
                
				Bitmap bmp = new Bitmap((int)BitmapWidth, (int)BitmapHeight);
				int k = 0;
				for (int i = 0; i < (int)BitmapHeight; i++)
				{
					for (int j = 0; j < (int)BitmapWidth; j++)
					{
						if (k < data.Length)
							bmp.SetPixel(j, i, data[k].PixelColor);
						else
							bmp.SetPixel(j, i, Color.Black);
						k++;
					}
				}
				bmp.Save(stream, ImageFormat.Bmp); 
				bmp.Dispose();
			}
		}

		#endregion
	}
}
