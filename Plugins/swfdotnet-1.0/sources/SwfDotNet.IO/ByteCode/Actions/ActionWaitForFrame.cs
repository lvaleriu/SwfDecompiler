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

using System.IO;
using System;

namespace SwfDotNet.IO.ByteCode.Actions
{	
	
	/// <summary>
	/// bytecode instruction object
	/// </summary>

	public class ActionWaitForFrame : MultiByteAction {
		
		private short waitFrame;
		private byte skipCount;
		
		/// <summary>
		/// count of byte to skip
		/// </summary>
		public int SkipCount {
			get {
				return Convert.ToInt32(skipCount);
			}
			set {
				skipCount = Convert.ToByte(value);
			}
		}
		
		/// <summary>
		/// constructor
		/// </summary>
		public ActionWaitForFrame(short wait,byte skip):base(ActionCode.WaitForFrame)
		{
			waitFrame = wait;
			skipCount = skip;
		}
		
		/// <see cref="SwfDotNet.IO.ByteCode.Actions.BaseAction.ByteCount "/>
		public override int ByteCount {
			get {
				return 6;
			}
		}
		
		/// <summary>overriden ToString method</summary>
		public override string ToString() {
			return String.Format("WaitForFrame {0}",waitFrame.ToString());
		}
		
		/// <see cref="SwfDotNet.IO.ByteCode.Actions.BaseAction.Compile"/>
		public override void Compile(BinaryWriter w) 
{
			base.Compile(w);
			w.Write(waitFrame);
			w.Write(skipCount);
		}
	}
	
}

