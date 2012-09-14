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
	/// RemoveObject2Tag removes an object from the display list.
	/// </summary>
	/// <remarks>
	/// <p>
	/// The RemoveObject2Tag class only requires the layer number to identify 
	/// a particular object, unlike the RemoveObjectTag class which also 
	/// requires the object's identifier although only one object can be placed 
	/// on a given layer.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 3.
	/// </p>
	/// </remarks>
	public class RemoveObject2Tag : BaseTag 
    {
        #region Members
	
		private ushort depth;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="RemoveObject2Tag"/> instance.
        /// </summary>
        public RemoveObject2Tag() 
        {
            this._tagCode = (int)TagCodeEnum.RemoveObject2;
        }

		/// <summary>
		/// Creates a new <see cref="RemoveObject2Tag"/> instance.
		/// </summary>
		/// <param name="depth">depth of character</param>
		public RemoveObject2Tag(ushort depth) 
		{
			this.depth = depth;
			this._tagCode = (int)TagCodeEnum.RemoveObject2;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        public ushort Depth
        {
            get { return this.depth;  }
            set { this.depth = value; }
        }

        #endregion
		
        #region Methods

        /// <summary>
        /// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
        /// </summary>
        public override void ReadData(byte version, BufferedBinaryReader binaryReader)
        {
            RecordHeader rh = new RecordHeader();
            rh.ReadData(binaryReader);

            depth = binaryReader.ReadUInt16();
        }

		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
		{		
			if (version < 3)
				return;

			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
			RecordHeader rh = new RecordHeader(TagCode, 2);
			rh.WriteTo(w);
			
			w.Write(depth);

            w.Flush();
			// write to data array
			_data = m.ToArray();
		}		

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("RemoveObject2Tag");
			writer.WriteElementString("Depth", depth.ToString());
			writer.WriteEndElement();
		}
	
        #endregion
	}
}
