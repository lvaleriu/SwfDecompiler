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
	/// ProtectTag marks a file as not-readable, preventing the file 
	/// from being loaded into an editor.
	/// </summary>
	/// <remarks>
	/// <p>
	/// In order to use the debugger a password must be supplied. 
	/// When encrypted using the MD5 algorithm it must match the value stored 
	/// in the password attribute.
	/// </p>
	/// <p>
	/// IMPORTANT: this form of protection only works with Macromedia's Flash 
	/// Authoring tool. Any application that parses Flash files can choose 
	/// to ignore or delete this data structure therefore it is not safe to 
	/// use this to protect the contents of a Flash file.
	/// </p>
	/// <p>
	/// Transform will parse all Flash files containing the Protect data 
	/// structure. Since the encoded data is can be removed by trivial scripts 
	/// the level of copy-protection offered is minimal. Indeed the use of the 
	/// Protect mechanism in Flash movies may lead to a false sense of security, 
	/// putting proprietary information at risk. Sensitive information should not 
	/// be included in Flash movies.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 5.
	/// </p>
	/// </remarks>
	public class ProtectTag : BaseTag 
    {
        #region Ctor

		/// <summary>
		/// constructor.
		/// </summary>
		public ProtectTag() 
		{
			this._tagCode = (int)TagCodeEnum.Protect;
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
        }
		
		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
		{			
			if (version < 5)
				return;

			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
			RecordHeader rh = new RecordHeader(TagCode, 0);
			rh.WriteTo(w);
			
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
			writer.WriteStartElement("ProtectTag");
			writer.WriteEndElement();
		}
	
        #endregion
	}
}
