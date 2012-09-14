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

namespace SwfDotNet.IO.Exceptions 
{
	/// <summary>
	/// Invalid Swf version exception
	/// </summary>
	public sealed class InvalidSwfVersionException : ApplicationException 
	{	
		/// <summary>
		/// Creates a new <see cref="InvalidSwfVersionException"/> instance.
		/// </summary>
		public InvalidSwfVersionException() : base("InvalidSwfVersionException") 
		{
		}

        /// <summary>
        /// Creates a new <see cref="InvalidSwfVersionException"/> instance.
        /// </summary>
        /// <param name="version">Version.</param>
        /// <param name="maxSupported">Max supported.</param>
        public InvalidSwfVersionException(byte version, int maxSupported): base("Invalid swf version: " + version + ", the max supported version is " + maxSupported)
        {
        }
	}	
}
