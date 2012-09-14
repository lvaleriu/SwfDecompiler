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

namespace SwfDotNet.IO.Tags 
{
	/// <summary>
	/// DefineButtonSoundTag defines the sounds that are played when an 
	/// event occurs in a button.
	/// </summary>
	/// <remarks>
    /// <p>
    /// A sound is played for only a subset of the events that a button 
    /// responds to:
    /// <ul>
    /// <li>RollOver: The cursor enters the active area of the button.</li>
    /// <li>RollOut: The cursor exits the active area of the button.</li>
    /// <li>Press: The mouse button is clicked and the cursor is inside the 
    /// active area of the button.</li>
    /// <li>Release: The mouse button is released while the cursor is inside 
    /// the active area of the button.</li>
    /// </ul>
    /// </p>
   	/// <p>
	/// This tag was introduced in Flash 2.
	/// </p>
	/// </remarks>
	public class DefineButtonSoundTag : BaseTag, DefineTag 
    {
        #region Members

		private ushort buttonId;
		private ushort buttonSoundChar;
		private SoundInfo buttonSoundInfo;
		private ushort buttonSoundChar1;
		private SoundInfo buttonSoundInfo1;
		private ushort buttonSoundChar2;
		private SoundInfo buttonSoundInfo2;
		private ushort buttonSoundChar3;
		private SoundInfo buttonSoundInfo3;

        #endregion
		
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="DefineButtonSoundTag"/> instance.
        /// </summary>
        public DefineButtonSoundTag()
        {
            _tagCode = (int)TagCodeEnum.DefineButtonSound;
		}

        /// <summary>
        /// Creates a new <see cref="DefineButtonSoundTag"/> instance.
        /// </summary>
        /// <param name="buttonId">Button id.</param>
        /// <param name="buttonSoundChar">Button sound char.</param>
        /// <param name="buttonSoundInfo">Button sound info.</param>
        /// <param name="buttonSoundChar1">Button sound char1.</param>
        /// <param name="buttonSoundInfo1">Button sound info1.</param>
        /// <param name="buttonSoundChar2">Button sound char2.</param>
        /// <param name="buttonSoundInfo2">Button sound info2.</param>
        /// <param name="buttonSoundChar3">Button sound char3.</param>
        /// <param name="buttonSoundInfo3">Button sound info3.</param>
		public DefineButtonSoundTag(ushort buttonId,
				 ushort buttonSoundChar,
				 SoundInfo buttonSoundInfo,
				 ushort buttonSoundChar1,
				 SoundInfo buttonSoundInfo1,
				 ushort buttonSoundChar2,
				 SoundInfo buttonSoundInfo2,
				 ushort buttonSoundChar3,
				 SoundInfo buttonSoundInfo3) 
		{
			this.buttonId = buttonId;
			this.buttonSoundChar = buttonSoundChar;
			this.buttonSoundInfo = buttonSoundInfo;
			this.buttonSoundChar1 = buttonSoundChar1;
			this.buttonSoundInfo1 = buttonSoundInfo1;
			this.buttonSoundChar2 = buttonSoundChar2;
			this.buttonSoundInfo2 = buttonSoundInfo2;
			this.buttonSoundChar3 = buttonSoundChar3;
			this.buttonSoundInfo3 = buttonSoundInfo3;
			_tagCode = (int)TagCodeEnum.DefineButtonSound;
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
        /// Gets or sets the button sound char.
        /// </summary>
        public ushort ButtonSoundChar
        {
            get { return this.buttonSoundChar;  }
            set { this.buttonSoundChar = value; }
        }

        /// <summary>
        /// Gets or sets the button sound info.
        /// </summary>
        public SoundInfo ButtonSoundInfo           
        {
            get { return this.buttonSoundInfo;  }
            set { this.buttonSoundInfo = value; }
        }

        /// <summary>
        /// Gets or sets the button sound char1.
        /// </summary>
        public ushort ButtonSoundChar1
        {
            get { return this.buttonSoundChar1;  }
            set { this.buttonSoundChar1 = value; }
        }

        /// <summary>
        /// Gets or sets the button sound info1.
        /// </summary>
        public SoundInfo ButtonSoundInfo1
        {
            get { return this.buttonSoundInfo1;  }
            set { this.buttonSoundInfo1 = value; }
        }

        /// <summary>
        /// Gets or sets the button sound char2.
        /// </summary>
        public ushort ButtonSoundChar2
        {
            get { return this.buttonSoundChar2;  }
            set { this.buttonSoundChar2 = value; }
        }

        /// <summary>
        /// Gets or sets the button sound info2.
        /// </summary>
        public SoundInfo ButtonSoundInfo2
        {
            get { return this.buttonSoundInfo2;  }
            set { this.buttonSoundInfo2 = value; }
        }

        /// <summary>
        /// Gets or sets the button sound char3.
        /// </summary>
        public ushort ButtonSoundChar3
        {
            get { return this.buttonSoundChar3;  }
            set { this.buttonSoundChar3 = value; }
        }

        /// <summary>
        /// Gets or sets the button sound info3.
        /// </summary>
        public SoundInfo ButtonSoundInfo3
        {
            get { return this.buttonSoundInfo3;  }
            set { this.buttonSoundInfo3 = value; }
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
			
            buttonSoundChar = binaryReader.ReadUInt16();
            buttonSoundInfo = null;
            if (buttonSoundChar != 0)
            {
                buttonSoundInfo = new SoundInfo();
                buttonSoundInfo.ReadData(binaryReader);
            }
            buttonSoundChar1 = binaryReader.ReadUInt16();
            buttonSoundInfo1 = null;
            if (buttonSoundChar1 != 0)
            {
                buttonSoundInfo1 = new SoundInfo();
                buttonSoundInfo1.ReadData(binaryReader);
            }
            buttonSoundChar2 = binaryReader.ReadUInt16();
            buttonSoundInfo2 = null;
            if (buttonSoundChar2 != 0)
            {
                buttonSoundInfo2 = new SoundInfo();
                buttonSoundInfo2.ReadData(binaryReader);
            }
            buttonSoundChar3 = binaryReader.ReadUInt16();
            buttonSoundInfo3 = null;
            if (buttonSoundChar3 != 0)
            {
                buttonSoundInfo3 = new SoundInfo();
                buttonSoundInfo3.ReadData(binaryReader);
            }
        }
		
        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns>Size of this object</returns>
        protected int GetSizeOf()
        {
            int res = 10;
            if (buttonSoundChar != 0 && buttonSoundInfo != null)
                res += buttonSoundInfo.GetSizeOf();
            if (buttonSoundChar1 != 0 && buttonSoundInfo1 != null)
                res += buttonSoundInfo1.GetSizeOf();
            if (buttonSoundChar2 != 0 && buttonSoundInfo2 != null)
                res += buttonSoundInfo2.GetSizeOf();
            if (buttonSoundChar3 != 0 && buttonSoundInfo3 != null)
                res += buttonSoundInfo3.GetSizeOf();

            return res;
        }

		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
        {			
			if (version < 2)
				return;

            MemoryStream m = new MemoryStream();
            BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
            RecordHeader rh = new RecordHeader(TagCode, GetSizeOf());
			
            rh.WriteTo(w);
            w.Write(this.buttonId);
            
            w.Write(this.buttonSoundChar);
            if (buttonSoundChar != 0 && buttonSoundInfo != null)
                buttonSoundInfo.WriteTo(w);

            w.Write(this.buttonSoundChar1);
            if (buttonSoundChar1 != 0 && buttonSoundInfo1 != null)
                buttonSoundInfo1.WriteTo(w);

            w.Write(this.buttonSoundChar2);
            if (buttonSoundChar2 != 0 && buttonSoundInfo2 != null)
                buttonSoundInfo2.WriteTo(w);  
          
            w.Write(this.buttonSoundChar3);
            if (buttonSoundChar3 != 0 && buttonSoundInfo3 != null)
                buttonSoundInfo3.WriteTo(w); 

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
            writer.WriteStartElement("DefineButtonSoundTag");
            writer.WriteElementString("ButtonId", buttonId.ToString());

            writer.WriteElementString("ButtonSoundChar", buttonSoundChar.ToString());
            if (buttonSoundChar != 0 && buttonSoundInfo != null)
                buttonSoundInfo.Serialize(writer);

            writer.WriteElementString("ButtonSoundChar1", buttonSoundChar1.ToString());
            if (buttonSoundChar1 != 0 && buttonSoundInfo1 != null)
                buttonSoundInfo1.Serialize(writer);

            writer.WriteElementString("ButtonSoundChar2", buttonSoundChar2.ToString());
            if (buttonSoundChar2 != 0 && buttonSoundInfo2 != null)
                buttonSoundInfo2.Serialize(writer); 
          
            writer.WriteElementString("ButtonSoundChar3", buttonSoundChar3.ToString());
            if (buttonSoundChar3 != 0 && buttonSoundInfo3 != null)
                buttonSoundInfo3.Serialize(writer);
            writer.WriteEndElement();
        }
		
        #endregion
	}
}
