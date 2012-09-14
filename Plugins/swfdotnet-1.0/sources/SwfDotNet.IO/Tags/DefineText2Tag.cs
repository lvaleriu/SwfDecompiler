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
using System.Collections;

using SwfDotNet.IO.Utils;
using SwfDotNet.IO.Tags;
using SwfDotNet.IO.Tags.Types;

namespace SwfDotNet.IO.Tags {
	
	/// <summary>
	/// DefineTextTag defines one or more lines of transparent text.
	/// </summary>
	/// <remarks>
	/// <p>
	/// It extends the functionality provided by the DefineTextTag class by 
	/// supporting transparent colors. The characters, style and layout 
	/// information is defined using TextRecord objects. The DefineText2Tag 
	/// class acts as a container for the text, defining the bounding rectangle 
	/// that encloses the text along with a coordinate transform that can 
	/// be used to change the size and orientation of the text when it 
	/// is displayed.
	/// </p>
	/// <p>
	/// The bounding rectangle and matrix controls how the text is laid out. 
	/// Each TextRecord object in the TextRecordCollection specifies an offset from 
	/// the left and bottom edges of the bounding rectangle, allowing successive 
	/// lines of text to be arranged as a block or paragraph. The coordinate 
	/// transform matrix can be used to control the size and orientation of the text 
	/// when it is displayed.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 3.
	/// </p>
	/// </remarks>
	public class DefineText2Tag : DefineText 
    {
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="DefineText2Tag"/> instance.
        /// </summary>
        public DefineText2Tag()
        {
			Init();
            this._tagCode = (int)TagCodeEnum.DefineText2;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the tag.
        /// </summary>
        /// <value>The name of the tag.</value>
        protected override string TagName
        {
            get { return "DefineText2Tag"; }
        }

        /// <summary>
        /// Gets the version compatibility.
        /// </summary>
        /// <value>The version compatibility.</value>
        protected override int VersionCompatibility
        {
            get { return 3; }
        }

        #endregion
	}
}
