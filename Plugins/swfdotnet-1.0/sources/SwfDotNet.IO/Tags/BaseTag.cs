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

namespace SwfDotNet.IO.Tags {
	
	/// <summary>
	/// Base class for swf tag objects
	/// </summary>
	public class BaseTag : IEnumerable, ISwfSerializer 
	{	
		#region Members

		/// <summary>
		/// raw tag data
		/// </summary>
		protected byte[] _data;		
		
		/// <summary>
		/// swf tag code
		/// </summary>
		protected int _tagCode = -1;

		#endregion
		
		#region Ctor

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="data">raw tag data</param>
		public BaseTag (byte[] data) 
		{
			_data = data;
			bytecodeHolder = new BytecodeHolder(this);
		}
		
		/// <summary>
		/// constructor
		/// </summary>
		public BaseTag () 
		{
			_data = new byte[0];
			bytecodeHolder = new BytecodeHolder(this);
		}

		#endregion

		#region Properties

		/// <summary>
		/// swf tag code property
		/// </summary>
		public int TagCode 
		{
			get {
				return _tagCode;
			}
		}

		/// <summary>
		/// raw tag data property
		/// </summary>
		public byte[] Data {
			get {
				return _data;
			}
		}
		
		/// <summary>
		/// count of action records / raw bytecode blocks in tag
		/// </summary>
		public virtual int ActionRecCount {
			get {
				return 0;
			}
		}

		/// <summary>
		/// indexer for accessing bytecode blocks
		/// </summary>
		public virtual byte[] this[int index] 
		{
			get 
			{
				return null;
			}
			set 
			{
				
			}
		}

		private BytecodeHolder bytecodeHolder;		
		
		/// <summary>
		/// alias for the bytecode block indexer
		/// </summary>
		public BytecodeHolder Bytecode 
		{
			get 
			{
				return bytecodeHolder;
			}
		}

		#endregion

        /// <summary>
        /// swf tag code property
        /// </summary>
        public TagCodeEnum GetTagCode()
        {
            TagCodeEnum val = TagCodeEnum.Unknown;
            if (this.TagCode != -1)
            {
                val = TagCodeEnum.DefineBits;
                val = (TagCodeEnum)System.Enum.Parse(val.GetType(), this.TagCode.ToString());
            }
            return val;
        }
     
		/// <summary>
		/// Resolves the specified SWF.
		/// </summary>
		/// <param name="swf">SWF.</param>
		public virtual void Resolve(Swf swf)
		{
		}

        /// <summary>
        /// Rebuild tag data for swf compilation
        /// </summary>
        /// <param name="version">Version.</param>
        public virtual void UpdateData(byte version)
        {
        }

        /// <summary>
        /// Reads the data for the swf decompilation.
        /// </summary>
        /// <param name="version">Version.</param>
        /// <param name="binaryReader">Binary reader.</param>
        public virtual void ReadData(byte version, BufferedBinaryReader binaryReader)
        {
        }

		/// <summary>
		/// <see cref="ISwfSerializer.Serialize"/>
		/// </summary>
		/// <param name="writer">Writer.</param>
		public virtual void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("BaseTag");
			writer.WriteEndElement();
		}

		/// <summary>
		/// Adds the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		public void Add(object value)
		{
		}
		
		/// <summary>
		/// get swf bytecode block enumerator for foreach-loops
		/// </summary>
		public virtual IEnumerator GetEnumerator() {
			return (IEnumerator) new BytecodeEnumerator(this);
		}				
				
		/// <summary>
		/// inner class for bytecode block collection
		/// </summary>
		public class BytecodeHolder {
			
			/// <summary>
			/// bytecode block indexer
			/// </summary>
			public byte[] this[int index] {
				get {
					return tag[index];
				}
			}
			
			/// <summary>
			/// bytecode count
			/// </summary>
			public int Count {
				get {
					return tag.ActionRecCount;
				}
			}
			
			private BaseTag tag;
			
			/// <summary>
			/// constructor. internal, since only used by BaseTag class
			/// </summary>
			/// <param name="t">the BaseTag, who´s bytecode is being held</param>
			internal BytecodeHolder(BaseTag t) {
				tag = t;
			}
		}
		
		/// <summary>
		/// inner class, swf bytecode block enumerator for 'foreach' loops
		/// </summary>
		public class BytecodeEnumerator : IEnumerator {
			
			private int index = -1;
			private BaseTag tag;
			
			/// <summary>
			/// constructor. internal, since only used by BaseTag class
			/// </summary>
			/// <param name="tag">the BaseTag, who´s bytecode is being held</param>
			internal BytecodeEnumerator(BaseTag tag) {
				this.tag = tag;
				this.index = -1;
			}	
			
			/// <summary>
			/// satisfy IEnumerator interface
			/// </summary>
			public void Reset() {
				index = -1;
			}
			
			/// <summary>
			/// satisfy IEnumerator interface
			/// </summary>
			public bool MoveNext() {
				if (index>tag.ActionRecCount) throw new InvalidOperationException();
				return ++index < tag.ActionRecCount;
			}
			
			/// <summary>
			/// typed access to current object
			/// </summary>
			public byte[] Current {
				get {
					if (index>=tag.ActionRecCount) throw new InvalidOperationException();				
					return (tag[index]);
				}
			}
			
			/// <summary>
			/// satisfy IEnumerator interface
			/// </summary>
			object IEnumerator.Current {
				get { return this.Current; }
			}
		}

	}
}
