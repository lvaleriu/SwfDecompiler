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

using SwfDotNet.IO.Tags;

namespace SwfDotNet.IO
{
	/// <summary>
	/// Flv.
	/// </summary>
	public class Flv
	{
		#region Members

		private FlvHeader header;
		private FlvBaseTagCollection tags;

		#endregion

		#region Ctor

		/// <summary>
		/// Creates a new <see cref="Flv"/> instance.
		/// </summary>
		public Flv()
		{
		}

		/// <summary>
		/// Creates a new <see cref="Flv"/> instance.
		/// </summary>
		/// <param name="header">Header.</param>
		public Flv(FlvHeader header)
		{
			this.header = header;
		}

		/// <summary>
		/// Creates a new <see cref="Flv"/> instance.
		/// </summary>
		/// <param name="header">Header.</param>
		/// <param name="flvTags">FLV tags.</param>
		public Flv(FlvHeader header, FlvBaseTagCollection flvTags)
		{
			this.header = header;
			this.tags = flvTags;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the header.
		/// </summary>
		/// <value></value>
		public FlvHeader Header
		{
			get { return this.header;  }
			set { this.header = value; }
		}

		/// <summary>
		/// Gets or sets the tags.
		/// </summary>
		/// <value></value>
		public FlvBaseTagCollection Tags
		{
			get { return this.tags;  }
			set { this.tags = value; }
		}

		#endregion
	}
}
