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
using System.Drawing;
using System.Collections;

using SwfDotNet.IO.Utils;

namespace SwfDotNet.IO.Tags.Types
{
    #region Enum

	/// <summary>
	/// BitmapFormat
	/// </summary>
	public enum BitmapFormat
	{
        /// <summary>
        /// Pix 15 bits stockage format
        /// </summary>
		Pix15 = 0,

        /// <summary>
        /// Pix 24 bits stockage format
        /// </summary>
		Pix24 = 1
	}

    #endregion

	/// <summary>
	/// BitmapColorData
	/// </summary>
	public class BitmapColorData
	{
        #region Members

        /// <summary>
        /// Bitmap pixel data, stored with Pix15 format
        /// </summary>
		public Pix15[] bitmapPixelDataPix15 = null;

        /// <summary>
        /// Bitmap pixel data, stored with Pix24 format
        /// </summary>
        public Pix24[] bitmapPixelDataPix24 = null;

        #endregion

        #region Ctor

		/// <summary>
		/// Creates a new <see cref="BitmapColorData"/> instance.
		/// </summary>
		/// <param name="bitmapPixelData">Bitmap pixel data.</param>
		public BitmapColorData(Pix15[] bitmapPixelData) 
		{ 
			this.bitmapPixelDataPix15 = bitmapPixelData;
		}

		/// <summary>
		/// Creates a new <see cref="BitmapColorData"/> instance.
		/// </summary>
		/// <param name="bitmapPixelData">Bitmap pixel data.</param>
		public BitmapColorData(Pix24[] bitmapPixelData) 
		{ 
			this.bitmapPixelDataPix24 = bitmapPixelData;
		}

        #endregion

        #region Methods

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns>size of this structure</returns>
		public int GetSizeOf()
		{
			if (bitmapPixelDataPix15 != null)
				return bitmapPixelDataPix15.Length * Pix15.GetSizeOf();
			else if (bitmapPixelDataPix24 != null)
				return bitmapPixelDataPix24.Length * Pix24.GetSizeOf();
			return 0;
		}

		/// <summary>
		/// Writes to a binary writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void WriteTo(BufferedBinaryWriter writer)
		{
			if (bitmapPixelDataPix15 != null)
			{
				foreach (Pix15 pix in bitmapPixelDataPix15)
					pix.WriteTo(writer);
			}
			else if (bitmapPixelDataPix24 != null)
			{
				foreach (Pix24 pix in bitmapPixelDataPix24)
					pix.WriteTo(writer);
			}
		}

        #endregion
	}
}
