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

namespace SwfDotNet.IO.Tags.Types
{
	/// <summary>
	/// ButtonCondaction
	/// </summary>
	public class ButtonCondaction
	{
        #region Members

		private byte condO;
		private byte condKey;
		private byte[] actions;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="ButtonCondaction"/> instance.
        /// </summary>
        public ButtonCondaction()
        {
        }

		/// <summary>
		/// Creates a new <see cref="ButtonCondaction"/> instance.
		/// </summary>
		/// <param name="condO">Cond O.</param>
		/// <param name="condKey">Cond key.</param>
		/// <param name="actions">Actions.</param>
		public ButtonCondaction(byte condO, byte condKey, byte[] actions)
		{
			this.condO = condO;
			this.condKey = condKey;
			this.actions = actions;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        /// <param name="condActionSize">Size of the cond action.</param>
        public void ReadData(BufferedBinaryReader binaryReader, 
            ushort condActionSize)
        {
            int offset = condActionSize - 5;
            condO = binaryReader.ReadByte();
            condKey = binaryReader.ReadByte();
			
            actions = binaryReader.ReadBytes(offset);
            byte end = binaryReader.ReadByte();
        }

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns>size of this object</returns>
		public int GetSizeOf()
		{
			int res = 4;
			if (actions != null)
				res += actions.Length;
			res++;
			return res;
		}

		/// <summary>
		/// Writes to a binary writer
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void WriteTo(BinaryWriter writer)
		{
            writer.Write(this.condO);
			writer.Write(this.condKey);
			if (actions != null)
				writer.Write(this.actions);
			writer.Write((byte)0);
		}

        #endregion
	}

    /// <summary>
    /// ButtonCondactionCollection
    /// </summary>
    public class ButtonCondactionCollection: CollectionBase
    {
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="ButtonCondactionCollection"/> instance.
        /// </summary>
        public ButtonCondactionCollection()
        {
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public ButtonCondaction Add(ButtonCondaction value)
        {
            List.Add(value as object);
            return value;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="values">Values.</param>
        public void AddRange(ButtonCondaction[] values)
        {
            foreach(ButtonCondaction ip in values)
                Add(ip);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public void Remove(ButtonCondaction value)
        {
            if (List.Contains(value))
                List.Remove(value as object);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="value">Value.</param>
        public void Insert(int index, ButtonCondaction value)
        {
            List.Insert(index, value as object);
        }

        /// <summary>
        /// Containses the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public bool Contains(ButtonCondaction value)
        {
            return List.Contains(value as object);
        }

        /// <summary>
        /// Gets or sets the <see cref="ButtonCondaction"/> at the specified index.
        /// </summary>
        /// <value></value>
        public ButtonCondaction this[int index]
        {
            get
            {
                return ((ButtonCondaction)List[index]);
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
        public int IndexOf(ButtonCondaction value)
        {
            return List.IndexOf(value);
        }

        #endregion
    }
}
