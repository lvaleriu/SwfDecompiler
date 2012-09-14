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

using SwfDotNet.IO.Tags;

namespace SwfDotNet.IO
{
	/// <summary>
	/// Dictionary class.
	/// The dictionary is a repository of tags 
	/// associated with unique character ID.
	/// </summary>
	public class Dictionary: Hashtable
	{
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="Dictionary"/> instance.
        /// </summary>
        public Dictionary()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified define tag.
        /// </summary>
        /// <param name="defineTag">Define tag.</param>
        public void Add(DefineTag defineTag)
        {
            Add(defineTag.CharacterId, defineTag);
        }

        /// <summary>
        /// Adds the specified character id.
        /// </summary>
        /// <param name="characterId">Character id.</param>
        /// <param name="defineTag">Define tag.</param>
        public void Add(ushort characterId, DefineTag defineTag)
        {
            if (characterId != 0 && !base.Contains(characterId))
                base.Add(characterId, defineTag);
        }
    
        /// <summary>
        /// Removes the specified define tag.
        /// </summary>
        /// <param name="defineTag">Define tag.</param>
        public void Remove(DefineTag defineTag)
        {
            base.Remove (defineTag.CharacterId);
        }

        /// <summary>
        /// Removes the specified character id.
        /// </summary>
        /// <param name="characterId">Character id.</param>
        public void Remove(ushort characterId)
        {
            base.Remove(characterId);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="DefineTag"/> with the specified character id.
        /// </summary>
        public DefineTag this[ushort characterId]
        {
            get
            {
                return base[characterId] as DefineTag;
            }
            set
            {
                base[characterId] = (object)value;
            }
        }

        #endregion

	}
}
