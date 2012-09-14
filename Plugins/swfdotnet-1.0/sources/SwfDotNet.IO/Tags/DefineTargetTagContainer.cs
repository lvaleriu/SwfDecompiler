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
using System.Collections;
using System.Text;

namespace SwfDotNet.IO.Tags
{
    /// <summary>
    /// DefineTargetTagContainer
    /// </summary>
    public interface DefineTargetTagContainer
    {
        /// <summary>
        /// Targets to.
        /// </summary>
        /// <param name="characterId">The character id.</param>
        /// <param name="history">The history.</param>
        /// <returns></returns>
        bool TargetTo(ushort characterId, Hashtable history);

        /// <summary>
        /// Changeds the target.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="newId">The new id.</param>
        /// <param name="history">The history.</param>
        void ChangedTarget(ushort id, ushort newId, Hashtable history);
    }
}
