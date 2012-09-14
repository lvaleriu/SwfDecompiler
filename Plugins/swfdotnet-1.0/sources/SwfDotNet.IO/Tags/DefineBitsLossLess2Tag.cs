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
using SwfDotNet.IO.Tags.Types;
using SwfDotNet.IO.Exceptions;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace SwfDotNet.IO.Tags 
{
	/// <summary>
	/// DefineBitsLossLess2Tag is used to define a transparent image compressed 
	/// using the lossless zlib compression algorithm.
	/// </summary>
	/// <remarks>
	/// <p>
	/// The class supports color-mapped images where the image data contains an 
	/// index into a color table or images where the image data specifies the color 
	/// directly. It extends FSDefineImage by including alpha channel information 
	/// for the color table and pixels in the image.
	/// </p>
	/// <p>
	/// For color-mapped images the color table contains up to 256, 32-bit colors. 
	/// The image contains one byte for each pixel which is an index into the table 
	/// to specify the color for that pixel. The color table and the image data are 
	/// compressed as a single block, with the color table placed before the image.
	/// </p>
	/// <p>
	/// For images where the color is specified directly, the image data contains 
	/// 32 bit color values.
	/// </p>
	/// <p>
	/// The image data is stored in zlib compressed form within the object. For 
	/// color-mapped images the compressed data contains the color table followed 
	/// by the image data.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 3.
	/// </p>
	/// </remarks>
	public class DefineBitsLossLess2Tag : BaseTag, DefineTag
	{
		#region Members

		private ushort _characterId;
		private byte _bitmapFormat;
		private ushort _bitmapWidth;
		private ushort _bitmapHeight;
		private ushort _bitmapColorTableSize;
		private AlphaColorMapData _alphaColorMapData;
		private AlphaBitmapData _alphaBitmapData;
		
		#endregion

		#region Ctor

		/// <summary>
		/// Creates a new <see cref="DefineBitsLossLess2Tag"/> instance.
		/// </summary>
		public DefineBitsLossLess2Tag()
		{
			_tagCode = (int)TagCodeEnum.DefineBitsLossLess2;
		}

		/// <summary>
		/// Creates a new <see cref="DefineBitsLossLess2Tag"/> instance.
		/// </summary>
		/// <param name="characterId">id for this character</param>
		/// <param name="bitmapFormat">Format of compressed data</param>
		/// <param name="bitmapWidth">Width of bitmap image</param>
		/// <param name="bitmapHeight">Height of bitmap image</param>
		/// <param name="bitmapColorTableSize">actual number of colors in the color table</param>
		public DefineBitsLossLess2Tag(ushort characterId, byte bitmapFormat, ushort bitmapWidth,
			ushort bitmapHeight, ushort bitmapColorTableSize) 
		{
			_characterId = characterId;
			_bitmapFormat = bitmapFormat;
			_bitmapWidth = bitmapWidth;
			_bitmapHeight = bitmapHeight;
			_bitmapColorTableSize = bitmapColorTableSize;
			_tagCode = (int)TagCodeEnum.DefineBitsLossLess2;
		}

		/// <summary>
		/// Creates a new <see cref="DefineBitsLossLess2Tag"/> instance.
		/// </summary>
		/// <param name="characterId">id for this character</param>
		/// <param name="bitmapFormat">Format of compressed data</param>
		/// <param name="bitmapWidth">Width of bitmap image</param>
		/// <param name="bitmapHeight">Height of bitmap image</param>
		/// <param name="bitmapColorTableSize">actual number of colors in the color table</param>
		/// <param name="zlibBitmapData">zlib compressed bitmap data</param>
		public DefineBitsLossLess2Tag(ushort characterId, byte bitmapFormat, ushort bitmapWidth,
			ushort bitmapHeight, ushort bitmapColorTableSize, AlphaColorMapData zlibBitmapData) 
		{
			_characterId = characterId;
			_bitmapFormat = bitmapFormat;
			_bitmapWidth = bitmapWidth;
			_bitmapHeight = bitmapHeight;
			_bitmapColorTableSize = bitmapColorTableSize;
			_alphaColorMapData = zlibBitmapData;
			_tagCode = (int)TagCodeEnum.DefineBitsLossLess2;
		}

		/// <summary>
		/// Creates a new <see cref="DefineBitsLossLess2Tag"/> instance.
		/// </summary>
		/// <param name="characterId">id for this character</param>
		/// <param name="bitmapFormat">Format of compressed data</param>
		/// <param name="bitmapWidth">Width of bitmap image</param>
		/// <param name="bitmapHeight">Height of bitmap image</param>
		/// <param name="bitmapColorTableSize">actual number of colors in the color table</param>
		/// <param name="zlibBitmapData">zlib compressed bitmap data</param>
		public DefineBitsLossLess2Tag(ushort characterId, byte bitmapFormat, ushort bitmapWidth,
			ushort bitmapHeight, ushort bitmapColorTableSize, AlphaBitmapData zlibBitmapData) 
		{
			_characterId = characterId;
			_bitmapFormat = bitmapFormat;
			_bitmapWidth = bitmapWidth;
			_bitmapHeight = bitmapHeight;
			_bitmapColorTableSize = bitmapColorTableSize;
			_alphaBitmapData = zlibBitmapData;
			_tagCode = (int)TagCodeEnum.DefineBitsLossLess2;
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
		/// Gets or sets the alpha color map data.
		/// </summary>
		public AlphaColorMapData AlphaColorMapData
		{
			get { return this._alphaColorMapData;  }
			set { this._alphaColorMapData = value; }
		}
        
		/// <summary>
		/// Gets or sets the alpha bitmap data.
		/// </summary>
		public AlphaBitmapData AlphaBitmapData
		{
			get { return this._alphaBitmapData;  }
			set { this._alphaBitmapData = value; }
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

			int imageSize = _bitmapWidth * _bitmapHeight;
			
			if (_bitmapFormat == 3)
			{
				int uncompressedSize = imageSize + ((_bitmapColorTableSize + 1) * 4);
				byte[] uncompressed = new byte[uncompressedSize];
				byte[] compressed = binaryReader.ReadBytes(toReaded);
				Inflater zipInflator = 	new Inflater();
				zipInflator.SetInput(compressed);
				zipInflator.Inflate(uncompressed, 0, uncompressedSize);

				_alphaColorMapData = new AlphaColorMapData();
				_alphaColorMapData.ColorTableRgb = new RGBA[_bitmapColorTableSize + 1];
				int offset = 0;
				for (int i = 0; i < _bitmapColorTableSize + 1; i++, offset += 4)
				{
					byte red = uncompressed[offset];
					byte green = uncompressed[offset + 1];
					byte blue = uncompressed[offset + 2];
					byte alpha = uncompressed[offset + 3];
					_alphaColorMapData.ColorTableRgb[i] = new RGBA(red, green, blue, alpha);
				}
				_alphaColorMapData.ColorMapPixelData = new byte[uncompressedSize - offset];
				for (int i = 0; i < uncompressedSize - offset; i++, offset++)
					_alphaColorMapData.ColorMapPixelData[i] = uncompressed[offset];
			}
			else if (_bitmapFormat == 4 || _bitmapFormat == 5)
			{
				int uncompressedSize = imageSize * 4;
				byte[] uncompressed = new byte[uncompressedSize];
				byte[] compressed = binaryReader.ReadBytes(toReaded);	
				Inflater zipInflator = 	new Inflater();
				zipInflator.SetInput(compressed);
				zipInflator.Inflate(uncompressed, 0, uncompressedSize);

				_alphaBitmapData = new AlphaBitmapData();
				_alphaBitmapData.BitmapPixelData = new RGBA[imageSize];
				for (int i = 0, j = 0; i < imageSize; i++, j += 4)
				{
					byte red = uncompressed[j];
					byte green = uncompressed[j + 1];
					byte blue = uncompressed[j + 2];
					byte alpha = uncompressed[j + 3];
					_alphaBitmapData.BitmapPixelData[i] = new RGBA(red, green, blue, alpha);
				}
			}			
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <param name="lengthOfCompressedData">Length of compressed data.</param>
        /// <returns></returns>
        public int GetSizeOf(int lengthOfCompressedData)
        {
            int length = 7;
            if (this._bitmapFormat == 3)
            {
                length++;
                length += lengthOfCompressedData;
            }
            else if (this._bitmapFormat == 4 || this._bitmapFormat == 5)
            {
                length += lengthOfCompressedData;
            }
            return length;
        }
		
		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
		{			
			if (version < 3)
				return;

            //Compression process
            int lenghtOfCompressedBlock = 0;
            byte[] compressArray = null;
            MemoryStream unCompressedStream = new MemoryStream();
            BufferedBinaryWriter unCompressedWriter = new BufferedBinaryWriter(unCompressedStream);
            
            if (this._bitmapFormat == 3)
            {
                this._alphaColorMapData.WriteTo(unCompressedWriter);
            }
            else if (this._bitmapFormat == 4 || this._bitmapFormat == 5)
            {
                this._alphaBitmapData.WriteTo(unCompressedWriter);
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
			writer.WriteStartElement("DefineBitsLossLess2Tag");
			writer.WriteAttributeString("CharacterId", this._characterId.ToString());
			writer.WriteElementString("BitmapFormat", this._bitmapFormat.ToString());
			writer.WriteElementString("BitmapWidth", this._bitmapWidth.ToString());
			writer.WriteElementString("BitmapHeight", this._bitmapHeight.ToString());
			
            if (this._bitmapFormat == 4 || this._bitmapFormat == 5)
                this.AlphaBitmapData.Serialize(writer);
            
            writer.WriteEndElement();
		}
		
		#endregion

		#region Compile & Decompile Methods

		/// <summary>
		/// Construct a new DefineBitsLossLess2Tag object 
		/// from a file.
		/// </summary>
		/// <param name="characterId">Character id.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <returns></returns>
		public static DefineBitsLossLess2Tag FromFile(ushort characterId, string fileName)
		{
			FileStream stream = File.OpenRead(fileName);
			DefineBitsLossLess2Tag res = FromImage(characterId, Image.FromStream(stream));
			stream.Close();
			return res;
		}

		/// <summary>
		/// Construct a new DefineBitsLossLess2Tag object 
		/// from a stream.
		/// </summary>
		/// <param name="characterId">Character id.</param>
		/// <param name="stream">Stream.</param>
		/// <returns></returns>
		public static DefineBitsLossLess2Tag FromStream(ushort characterId, Stream stream)
		{
			DefineBitsLossLess2Tag res = FromImage(characterId, Image.FromStream(stream));
			return res;
		}

		/// <summary>
		/// Construct a new DefineBitsLossLess2Tag object 
		/// from an image object.
		/// </summary>
		/// <param name="characterId">Character id.</param>
		/// <param name="image">Image.</param>
		/// <returns></returns>
		public static DefineBitsLossLess2Tag FromImage(ushort characterId, Image image)
		{
            if (image.RawFormat.Equals(ImageFormat.Bmp) == false &&
                image.RawFormat.Equals(ImageFormat.MemoryBmp) == false)
                throw new InvalidImageFormatException();
			
            Bitmap bitmap = (Bitmap)image;

			byte format = 0;
			PixelFormat pxFormat = image.PixelFormat;
			if (pxFormat == PixelFormat.Format8bppIndexed)
				format = 3;
			else if (pxFormat == PixelFormat.Format16bppRgb555 ||
				pxFormat == PixelFormat.Format16bppRgb565)
				format = 4;
			else if (pxFormat == PixelFormat.Format24bppRgb)
				format = 5;
			else
				throw new InvalidPixelFormatException();

			DefineBitsLossLess2Tag bmp = new DefineBitsLossLess2Tag();
			bmp.CharacterId = characterId;
			bmp.BitmapFormat = format;
			bmp.BitmapWidth = (ushort)image.Width;
			bmp.BitmapHeight = (ushort)image.Height;

            int imageSize = bitmap.Width * bitmap.Height;

			if (bmp.BitmapFormat == 3)
			{
				//TODO
			}
			else if (bmp.BitmapFormat == 4 ||
				bmp.BitmapFormat == 5)
			{
                RGBA[] bitmapPixelData = new RGBA[imageSize];
                int k = 0;
                for (int i = 0; i < bitmap.Height; i++)
                {
                    for (int j = 0; j < bitmap.Width; j++)
                    {
                        Color color = bitmap.GetPixel(j, i);
                        bitmapPixelData[k] = new RGBA((byte)color.R, (byte)color.G, (byte)color.B, (byte)color.A);
                        k++;
                    }
                }
                bmp.AlphaBitmapData = new AlphaBitmapData(bitmapPixelData);
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
				//AlphaColorMapData
				AlphaColorMapData mapData = AlphaColorMapData;
				Bitmap bmp = new Bitmap((int)BitmapWidth, (int)BitmapHeight);
				int k = 0;
				for (int i = 0; i < (int)BitmapHeight; i++)
				{
					for (int j = 0; j < (int)BitmapWidth; j++)
					{
						int index = (int)mapData.ColorMapPixelData[k];
						if (index >= 0 && index < mapData.ColorTableRgb.Length)
							bmp.SetPixel(j, i, mapData.ColorTableRgb[index].ToWinColor());
						else
							bmp.SetPixel(j, i, Color.Black);
						k++;
					}
				}
				bmp.Save(stream, ImageFormat.Bmp); 
			}
			else
			{
				//AlphaBitmapData
				AlphaBitmapData bitmapData = AlphaBitmapData;
				RGBA[] data = data = bitmapData.BitmapPixelData;
                
				Bitmap bmp = new Bitmap((int)BitmapWidth, (int)BitmapHeight);
				int k = 0;
				for (int i = 0; i < (int)BitmapHeight; i++)
				{
					for (int j = 0; j < (int)BitmapWidth; j++)
					{
						if (k < data.Length)
							bmp.SetPixel(j, i, data[k].ToWinColor());
						else
							bmp.SetPixel(j, i, Color.Black);
						k++;
					}
				}
				bmp.Save(stream, ImageFormat.Bmp); 
			}
		}

		#endregion
	}
}
