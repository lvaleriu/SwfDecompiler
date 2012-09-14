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
using System.Xml;
using System.Collections;

using SwfDotNet.IO.Utils;

namespace SwfDotNet.IO.Tags.Types
{
    /// <summary>
    /// ShortCollection class
    /// </summary>
    public class ShortCollection: CollectionBase
    {
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="ShortCollection"/> instance.
        /// </summary>
        public ShortCollection()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        /// <param name="numGlyphs">Num glyphs.</param>
        public void ReadData(BufferedBinaryReader binaryReader, ushort numGlyphs)
        {
            for (int i = 0; i < numGlyphs; i++)
                this.Add(binaryReader.ReadInt16());
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns></returns>
        public int GetSizeOf()
        {
            return this.Count * 2;
        }

        /// <summary>
        /// Writes to.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public void WriteTo(BufferedBinaryWriter writer)
        {
            IEnumerator shorts = this.GetEnumerator();
            while (shorts.MoveNext())
                writer.Write((short)shorts.Current);
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public short Add(short value)
        {
            List.Add(value as object);
            return value;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="values">Values.</param>
        public void AddRange(short[] values)
        {
            foreach(short ip in values)
                Add(ip);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public void Remove(short value)
        {
            if (List.Contains(value))
                List.Remove(value as object);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="value">Value.</param>
        public void Insert(int index, short value)
        {
            List.Insert(index, value as object);
        }

        /// <summary>
        /// Containses the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public bool Contains(short value)
        {
            return List.Contains(value as object);
        }

        /// <summary>
        /// Gets or sets the <see cref="TextRecord"/> at the specified index.
        /// </summary>
        /// <value></value>
        public short this[int index]
        {
            get
            {
                return ((short)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Get the index of.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public int IndexOf(short value)
        {
            return List.IndexOf(value);
        }

        #endregion
    }
}
