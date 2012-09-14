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
using SwfDotNet.IO.Tags.Types;

namespace SwfDotNet.IO.Tags {

	/// <summary>
    /// DefineButtonTag defines the appearance of a button and 
    /// the actions performed when the button is clicked.
	/// </summary>
	/// <remarks>
	/// <p>
	/// A DefineButtonTag object must contain at least one ButtonRecord object. 
	/// If more than one button record is defined for a given button 
	/// state then each shape will be displayed by the button. 
	/// The order in which the shapes are displayed is determined by 
	/// the layer assigned to each ButtonRecord object.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 1.
	/// </p>
	/// </remarks>
	public class DefineButtonTag : BaseTag, DefineTag
	{
        #region Members

		private ushort buttonId;
		private ButtonRecordCollection characters;
        private byte[] actions;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="DefineButtonTag"/> instance.
        /// </summary>
        public DefineButtonTag()
        {
            _tagCode = (int)TagCodeEnum.DefineButton;
        }

        /// <summary>
        /// Creates a new <see cref="DefineButtonTag"/> instance.
        /// </summary>
        /// <param name="buttonId">Button id.</param>
        /// <param name="characters">Characters.</param>
        /// <param name="actions">Actions.</param>
		public DefineButtonTag(ushort buttonId, ButtonRecordCollection characters, byte[] actions)
		{
			this.buttonId = buttonId;
			this.characters = characters;
			this.actions = actions;
			_tagCode = (int)TagCodeEnum.DefineButton;
		}

        #endregion

        #region Properties

        /// <summary>
        /// see <see cref="SwfDotNet.IO.Tags.DefineTag"/>
        /// </summary>
        public ushort CharacterId
        {
            get { return this.buttonId;  }
            set { this.buttonId = value; }
        }

        /// <summary>
        /// Gets or sets the characters.
        /// </summary>
        public ButtonRecordCollection Characters
        {
            get { return this.characters;  }
            set { this.characters = value; }
        }

        /// <summary>
        /// Gets or sets the actions byte code.
        /// </summary>
        public byte[] ActionsByteCode
        {
            get { return this.actions;  }
            set { this.actions = value; }
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

            buttonId = binaryReader.ReadUInt16();
            characters = new ButtonRecordCollection();

            bool characterEndFlag = false;
            while (!characterEndFlag)
            {
                byte first = binaryReader.ReadByte();
                if (first == 0)
                    characterEndFlag = true;
                else
                {
                    ButtonRecord buttRecord = new ButtonRecord();
                    buttRecord.ReadData(binaryReader, first, TagCodeEnum.DefineButton);
                    characters.Add(buttRecord);
                }
            }
      	
            int offset = 2;
            foreach (ButtonRecord butRec in characters)
                offset += butRec.GetSizeOf();

            int lenght = System.Convert.ToInt32(rh.TagLength) - offset - 1; 
            //-1 for the ActionEndFlag
            actions = binaryReader.ReadBytes(lenght);
            //Read ActionEndFlag
            binaryReader.ReadByte();
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns>Size of this object</returns>
        protected int GetSizeOf()
        {
            int res = 4;
            if (characters != null)
            {
                foreach (ButtonRecord buttRec in characters)
                    res += buttRec.GetSizeOf();
            }
            if (actions != null)
                res += actions.Length;

            return res;
        }

        /// <summary>
        /// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
        /// </summary>
        public override void UpdateData(byte version) 
        {			
            MemoryStream m = new MemoryStream();
            BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
            RecordHeader rh = new RecordHeader(TagCode, GetSizeOf());
			
            rh.WriteTo(w);
            w.Write(this.buttonId);
            if (characters != null)
            {
                foreach (ButtonRecord buttRec in characters)
                    buttRec.WriteTo(w, TagCodeEnum.DefineButton);
            }
            byte end = 0;
            w.Write(end);
            if (actions != null)
                w.Write(actions);
            w.Write(end);

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
            writer.WriteStartElement("DefineButtonTag");
            writer.WriteElementString("ButtonId", buttonId.ToString());
            if (characters != null)
            {
                foreach (ButtonRecord buttRec in characters)
                    buttRec.Serialize(writer);
            }
            writer.WriteEndElement();
        }

        #endregion        
	}
}
