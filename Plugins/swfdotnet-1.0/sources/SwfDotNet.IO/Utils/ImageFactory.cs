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
using System.Drawing;
using System.Drawing.Imaging;

using SwfDotNet.IO.Tags;
using SwfDotNet.IO.Exceptions;

namespace SwfDotNet.IO.Utils
{
	/// <summary>
	/// ImageFactory provides methods to get the adapted tag object
	/// from an image.
	/// </summary>
	/// <remarks>
	/// Swf binary format has some different tags to
	/// include a picture in animation: DefineBitJpeg2 to
	/// include jpeg, DefineBitJpeg3 to include transparent
	/// jpeg, DefineBitsTag, DefineBitsLossLessTag to include
	/// bitmap and DefineBitsLossLessTag to include transparent
	/// bitmap. This class provides the way to automatically create
	/// the good tag object from picture files. 
	/// </remarks>
	/// <example>
	/// <code lang="C#">
	/// Image img = Image.FromFile("mypicture.jpeg");
	/// BaseTag tag = ImageFactory.GetPictureTag(img, swf.GetNewDefineId());
	/// </code>
	/// </example>
	public class ImageFactory
	{
        /// <summary>
        /// Gets the picture tag.
        /// </summary>
        /// <param name="image">Image.</param>
        /// <param name="characterId">Character id.</param>
        /// <returns></returns>
        public static BaseTag GetPictureTag(Image image, ushort characterId)
        {
            return DefineBitsJpeg2Tag.FromImage(characterId, image);
			
			//return DefineBitsJpeg2Tag.FromImage(characterId, image);
			/*
            if (image.RawFormat.Equals(ImageFormat.Jpeg))
                return DefineBitsJpeg2Tag.FromImage(characterId, image);
            else if (image.RawFormat.Equals(ImageFormat.Bmp) ||
                image.RawFormat.Equals(ImageFormat.MemoryBmp))
                return DefineBitsLossLess2Tag.FromImage(characterId, image);
            else
                throw new InvalidImageFormatException();
			*/
        }

        /// <summary>
        /// Gets the picture bits tag.
        /// </summary>
        /// <param name="image">Image.</param>
        /// <param name="characterId">Character id.</param>
        /// <returns></returns>
        public static BaseTag GetPictureBitsTag(Image image, ushort characterId)
        {
            return DefineBitsTag.FromImage(characterId, image);
        }
    }
}
