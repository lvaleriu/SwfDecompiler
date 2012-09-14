/*
	SwfDotNet is an open source library for writing and reading 
	Macromedia Flash (SWF) bytecode.
	Copyright (C) 2005 Olivier Carpentier - Adelina foundation
	see Licence.cs for GPL full text!
		
	SwfDotNet.IO is based on the open source library SwfOp 
	writted by Florian Krüsch Copyright (C) 2004 .
	
    This class is copyright (c) 2004 Jon Skeet. All rights reserved.
    From the "Miscellaneous Utility Library", at:
    http://www.yoda.arachsys.com/csharp/

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

namespace SwfDotNet.IO.Utils
{
	/// <summary>
	/// Endianness of a converter
	/// </summary>
	public enum Endianness
	{
		/// <summary>
		/// Little endian - least significant byte first
		/// </summary>
		LittleEndian,
		/// <summary>
		/// Big endian - most significant byte first
		/// </summary>
		BigEndian
	}
}
