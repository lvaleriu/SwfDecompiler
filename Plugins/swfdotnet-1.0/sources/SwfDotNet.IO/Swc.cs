/*
	SwfDotNet.Components is an open source library for reading 
	Flash components (SWC files).
	Copyright (C) 2005 Olivier Carpentier - Adelina foundation
	see Licence.cs for GPL full text!
		
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

using SwfDotNet.IO;

namespace SwfDotNet.IO
{
	/// <summary>
	/// Swc class.
	/// </summary>
	public class Swc
	{
        #region Members

        private ComponentCollection components;
        
        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="Swc"/> instance.
        /// </summary>
		public Swc()
		{	
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the components.
        /// </summary>
        /// <value></value>
        public ComponentCollection Components
        {
            get { return this.components;  }
            set { this.components = value; }
        }

        #endregion
	}
}
