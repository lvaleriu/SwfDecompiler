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
using SwfDotNet.IO.ByteCode;

namespace SwfDotNet.IO.ByteCode.Actions
{
	///<summary>base class for multibyte actions (>0x80)</summary>
	public abstract class MultiByteAction : BaseAction {		
		
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="c">action code as occuring in swf. Codes are listed in this <see cref="SwfDotNet.IO.ByteCode.ActionCode">enumeration</see></param>
		public MultiByteAction(ActionCode c) : base(c) {			
		}
		
		/// <see cref="SwfDotNet.IO.ByteCode.Actions.BaseAction.Compile"/>
		public override void Compile(BinaryWriter writer) {
			base.Compile(writer);
			writer.Write(Convert.ToUInt16(ByteCount-3));
		}
	}
}
