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
using System.Data;
using System.Drawing;
using System.Collections;

namespace SwfDotNet.IO
{
	/// <summary>
	/// SwfCollection is a typed collection of Swf objects.
	/// </summary>
	/// <remarks>
	/// This class is not used by other classes of the
	/// SwfDotNet.IO package, but be usefull.
	/// </remarks>
	public class SwfCollection: CollectionBase
	{	
		#region Ctor

		/// <summary>
		/// Constructor
		/// </summary>
		public SwfCollection()
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
		/// Add a base tag
		/// </summary>
		/// <param name="value">base tag to add</param>
		/// <returns>base tag added</returns>
		public Swf Add(Swf value)
		{
			List.Add(value as object);
			return value;
		}

		/// <summary>
		/// Add an array of swf
		/// </summary>
		/// <param name="values">swf array</param>
		public void AddRange(Swf[] values)
		{
			foreach(Swf ip in values)
				Add(ip);
		}

		/// <summary>
		/// Remove a swf
		/// </summary>
		/// <param name="value"></param>
		public void Remove(Swf value)
		{
			if (List.Contains(value))
				List.Remove(value as object);
		}

		/// <summary>
		/// Insert a swf at
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="value">swf</param>
		public void Insert(int index, Swf value)
		{
			List.Insert(index, value as object);
		}

		/// <summary>
		/// Test if list contain a swf
		/// </summary>
		/// <param name="value">swf</param>
		/// <returns>contain result</returns>
		public bool Contains(Swf value)
		{
			return List.Contains(value as object);
		}

		/// <summary>
		/// Access swf list by index
		/// </summary>
		public Swf this[int index]
		{
			get
			{
				return ((Swf)List[index]);
			}
			set
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// Get index of a swf
		/// </summary>
		/// <param name="value">swf</param>
		/// <returns>swf index if is contain, -1 else.</returns>
		public int IndexOf(Swf value)
		{
			return List.IndexOf(value);
		}

		#endregion

		#region Methods

        /// <summary>
        /// Gets the last swf of the collection.
        /// </summary>
        /// <returns></returns>
        public Swf GetLastOne()
        {
            if (this.Count == 0)
                return null;

            return this[this.Count - 1];
        }

		/// <summary>
		/// Convert collection to array
		/// </summary>
		/// <returns>swf array</returns>
		public Swf[] ToArray()
		{
			Swf[] res = null;
            
            if (this.Count > 0)
            {
                res = new Swf[this.Count];
                for (int i = 0; i < this.Count; i++)
                    res[i] = this[i];
            }
			return res;
		}

		#endregion

	}
}
