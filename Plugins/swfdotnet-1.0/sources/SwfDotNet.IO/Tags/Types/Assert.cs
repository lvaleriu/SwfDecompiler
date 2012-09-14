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

namespace SwfDotNet.IO.Tags.Types
{
	/// <summary>
	/// Assert
	/// </summary>
	public class Assert: ISwfSerializer, DefineTargetTag
	{
		#region Members

		/// <summary>
		/// Tag
		/// </summary>
		private ushort tag;
		
		/// <summary>
		/// Name
		/// </summary>
		private string name;

		#endregion

		#region Ctor

		/// <summary>
		/// Creates a new <see cref="Assert"/> instance.
		/// </summary>
		public Assert() 
		{ 
		}

		/// <summary>
		/// Creates a new <see cref="Assert"/> instance.
		/// </summary>
		/// <param name="tag">Tag.</param>
		/// <param name="name">Name.</param>
		public Assert(ushort tag, string name)
		{
			this.tag = tag;
			this.name = name;
		}

		#endregion

		#region Properties
	
		/// <summary>
		/// Gets or sets the tag.
		/// </summary>
		public ushort TargetCharacterId
		{
			get { return this.tag;  }
			set { this.tag = value; }
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name
		{
			get { return this.name;  }
			set { this.name = value; }
		}

		#endregion

		#region Methods

        /// <summary>
        /// Reads the data from a binary stream reader.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        public void ReadData(BufferedBinaryReader binaryReader)
        {
            tag = binaryReader.ReadUInt16();
            name = binaryReader.ReadString();
        }

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <param name="asserts">Asserts.</param>
		/// <returns>size of the list</returns>
		public static int GetSizeOf(AssertCollection asserts)
		{
			int total = 0;

            IEnumerator assertsEnu = asserts.GetEnumerator();
            while (assertsEnu.MoveNext())
            {
                Assert assert = (Assert)assertsEnu.Current;
                total += 2 + assert.Name.Length + 1;
            }
				
			return total;
		}

        /// <summary>
        /// Writes to.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public void WriteTo(BufferedBinaryWriter writer)
        {
            writer.Write(tag);
            writer.WriteString(name);
        }

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("Assert");
			writer.WriteElementString("Tag", tag.ToString());
			writer.WriteElementString("Name", name.ToString());
			writer.WriteEndElement();
		}

		#endregion
	}

    /// <summary>
    /// Assert collection
    /// </summary>
    public class AssertCollection : CollectionBase
    {
        #region Ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public AssertCollection()
        {
        }

        #endregion

        #region Collection Methods

        /// <summary>
        /// Clear collection
        /// </summary>
        public new void Clear()
        {
            List.Clear();
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Assert Add(Assert value)
        {
            List.Add(value as object);
            return value;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="values">The values.</param>
        public void AddRange(AssertCollection values)
        {
            IEnumerator enums = values.GetEnumerator();
            while (enums.MoveNext())
                Add((Assert)enums.Current);
        }


        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="values">The values.</param>
        public void AddRange(Assert[] values)
        {
            IEnumerator enums = values.GetEnumerator();
            while (enums.MoveNext())
                Add((Assert)enums.Current);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Remove(Assert value)
        {
            if (List.Contains(value))
                List.Remove(value as object);
        }

        /// <summary>
        /// Inserts at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public void Insert(int index, Assert value)
        {
            List.Insert(index, value as object);
        }

        /// <summary>
        /// Determines whether [contains] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(Assert value)
        {
            return List.Contains(value as object);
        }

        /// <summary>
        /// Gets or sets the <see cref="T:Assert"/> at the specified index.
        /// </summary>
        /// <value></value>
        public Assert this[int index]
        {
            get
            {
                return ((Assert)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int IndexOf(Assert value)
        {
            return List.IndexOf(value);
        }

        #endregion
    }
}
