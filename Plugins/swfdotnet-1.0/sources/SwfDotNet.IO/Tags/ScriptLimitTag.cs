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
    /// ScriptLimit tag is used to define the execution environment 
    /// of the Flash Player, limiting the resources available when 
    /// executing actions.
	/// </summary>
	/// <remarks>
	/// <p>
	/// It can be used to limit the maximum recursion depth and limit 
	/// the time a sequence of actions can execute for. This provides a 
	/// rudimentary mechanism for people viewing a movie to regain control 
	/// of the Flash Player should a script fail.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 7.
	/// </p>
	/// </remarks>
	public class ScriptLimitTag : BaseTag 
    {
        #region Members

		private ushort recursion;
		private ushort timeout;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="ScriptLimitTag"/> instance.
        /// </summary>
        public ScriptLimitTag()
        {
            _tagCode = (int)TagCodeEnum.ScriptLimit;
        }
		
		/// <summary>
		/// Creates a new <see cref="ScriptLimitTag"/> instance.
		/// </summary>
		/// <param name="recursion">Recursion depth. That's the maximum depth, in the range 1..65535, that a sequence of actions can recurse to.</param>
		/// <param name="timeout">Specified timeout. That's the maximum time, in seconds, that a sequence of actions will execute before the Flash Player present a dialog box asking whether the script should be terminated.</param>
		public ScriptLimitTag(ushort recursion, ushort timeout) 
        {	
			this.recursion = recursion;
			this.timeout = timeout;
			
			_tagCode = (int)TagCodeEnum.ScriptLimit;
		}

        #endregion
		
        #region Properties

		/// <summary>
        /// Gets or sets the recursion.
        /// That's the maximum depth, in the range 1..65535, that 
        /// a sequence of actions can recurse to.
        /// </summary>
        public ushort Recursion 
        {
			get { return recursion;  }
            set { recursion = value; }
		}
		
        /// <summary>
        /// Gets or sets the time out.
        /// That's the maximum time, in seconds, that a sequence 
        /// of actions will execute before the Flash Player present 
        /// a dialog box asking whether the script should be terminated.
        /// </summary>
        public ushort TimeOut 
        {
			get { return timeout;  }
			set { timeout = value; }
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

            recursion = binaryReader.ReadUInt16();
            timeout = binaryReader.ReadUInt16();
        }
		
		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
        {
			if (version < 7)
				return;

			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
			RecordHeader rh = new RecordHeader(TagCode, 4);
			
			rh.WriteTo(w);
			w.Write(recursion);
			w.Write(timeout);
			
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
			writer.WriteStartElement("ScriptLimitTag");
			writer.WriteElementString("Recursion", recursion.ToString());
			writer.WriteElementString("Timeout", timeout.ToString());
			writer.WriteEndElement();
		}

        #endregion
	}
}
