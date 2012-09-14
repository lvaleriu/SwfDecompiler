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
	/// InitActionTag is used to specify a sequence of actions that are 
	/// executed to initialise a movie clip before it is displayed.
	/// </summary>
	/// <remarks>
	/// <p>
	/// It implements the #initclip pragma in the ActionScript language.
	/// </p>
	/// <p>
	/// Unlike the DoActionTag class which specifies the actions that are 
	/// executed when a particular frame is displayed the actions contained in an 
	/// InitActionTag object are executed only once, regardless of where the 
	/// object is included in a movie. If a frame containing the InitActionTag 
	/// object is played again the actions are skipped. Also there can only be 
	/// one InitActionTag object for each movie clip defined in the movie.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 6.
	/// </p>
	/// </remarks>
    public class InitActionTag : BaseTag, DefineTargetTag
	{		
		#region Members
	
		/// <summary>sprite id</summary>
		private ushort spriteId;
		
		/// <summary>bytecode block</summary>
		private byte[] actionRecord;
		
		#endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="InitActionTag"/> instance.
        /// </summary>
        public InitActionTag()
        {
            _tagCode = (int)TagCodeEnum.InitAction;
        }

        /// <summary>
        /// Creates a new <see cref="InitActionTag"/> instance.
        /// </summary>
        /// <param name="spriteId">Sprite id.</param>
        /// <param name="actionRecord">Action record.</param>
		public InitActionTag(ushort spriteId, byte[] actionRecord) 
        {	
			this.spriteId = spriteId;
			this.actionRecord = actionRecord;	
			
			_tagCode = (int)TagCodeEnum.InitAction;
		}

        #endregion
		
		#region Properties

        /// <summary>
        /// Target tag's character id
        /// </summary>
        /// <value></value>
        public ushort TargetCharacterId
        {
            get { return this.spriteId; }
            set { this.spriteId = value; }
        }

		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override int ActionRecCount 
        {
			get {
				return 1;
			}
		}
		
		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override byte[] this[int index] {
			get {
				return actionRecord;
			}
			set {
				actionRecord = value;
			}
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
			
            spriteId = binaryReader.ReadUInt16();
            int lenght = System.Convert.ToInt32(rh.TagLength-2);
            actionRecord = binaryReader.ReadBytes(lenght);
        }
		
		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
		{
			if (version < 6)
				return;

			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
			RecordHeader rh = new RecordHeader(TagCode, 2 + actionRecord.Length);
			
			rh.WriteTo(w);
			w.Write(spriteId);
			w.Write(actionRecord);
			
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
			writer.WriteStartElement("InitActionTag");
			writer.WriteElementString("SpriteId", spriteId.ToString());
			writer.WriteEndElement();
		}

        #endregion
	}
}
