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
	/// DefineButton2Tag defines the appearance and actions 
	/// of push and menu buttons.
	/// </summary>
	/// <remarks>
    /// <p>
    /// It provides a more sophisticated model for creating buttons:
    /// <ul>
    /// <li>Two types of button are supported, <B>Push</B> and <B>Menu</B>.</li>
    /// <li>The number of events that a button can respond to is increased.</li>
    /// <li>Actions can be executed for any button event.</li>
    /// </ul>
    /// </p>
    /// <p>
    /// Push and Menu buttons behave slightly differently in tracking 
    /// mouse movements when the button is clicked. A Push button 'captures' 
    /// the mouse so if the cursor is dragged outside of the active 
    /// area of the button and the mouse click is released then the 
    /// Release Outside event is still sent to the button. A Menu button 
    /// does not 'capture' the mouse so if the cursor is dragged out of 
    /// the active area the button returns to its 'inactive' state.
    /// </p>
    /// <p>
    /// A DefineButton2Tag object must contain at least one ButtonRecord object. 
    /// If more than one button record is defined for a given button state then 
    /// each shape will be displayed by the button. The order in which the shapes 
    /// are displayed is determined by the layer assigned to each button record.
    /// </p>
    /// <p>
    /// Each ButtonRecord object can contain an CXFormWithAlphaData object which 
    /// can be used to change the color of the shape being displayed without 
    /// changing the original definition.
    /// </p>
    /// <p>
    /// Actions do not need to be specified for every button event. Indeed 
    /// actions do not need to be specified at all.
    /// </p>
	/// <p>
	/// This tag was introduced in Flash 3.
	/// </p>
	/// </remarks>
	public class DefineButton2Tag : BaseTag, DefineTag 
	{
        #region Members

		private ushort buttonId;
		private bool trackAsMenu;
		private ushort actionOffset;
		private ButtonRecordCollection characters;
		private ButtonCondactionCollection actions;

        #endregion

		#region Ctor & Init

        /// <summary>
        /// Creates a new <see cref="DefineButton2Tag"/> instance.
        /// </summary>
        public DefineButton2Tag()
        {
            Init();
        }

		/// <summary>
		/// Inits this instance.
		/// </summary>
		private void Init()
		{
			characters = new ButtonRecordCollection();
			_tagCode = (int)TagCodeEnum.DefineButton2;
		}
	
        #endregion

        #region Properties

		/// <summary>
		/// Gets the characters.
		/// </summary>
		/// <value></value>
		public ButtonRecordCollection Characters
		{
			get { return this.characters; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether [track as menu].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [track as menu]; otherwise, <c>false</c>.
		/// </value>
		public bool TrackAsMenu
		{
			get { return this.trackAsMenu;  }
			set { this.trackAsMenu = value; }
		}

        /// <summary>
        /// see <see cref="SwfDotNet.IO.Tags.DefineTag"/>
        /// </summary>
        public ushort CharacterId
        {
            get { return this.buttonId;  }
            set { this.buttonId = value; }
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

            long startPosition = binaryReader.BaseStream.Position;

            buttonId = binaryReader.ReadUInt16();
            binaryReader.ReadUBits(7); //reserved
            trackAsMenu = binaryReader.ReadBoolean();
            
            long startPos = binaryReader.BaseStream.Position;

            actionOffset = binaryReader.ReadUInt16();

			if (characters == null)
				characters = new ButtonRecordCollection();
			else
				characters.Clear();

            bool characterEndFlag = false;
            while (!characterEndFlag)
            {
                byte first = binaryReader.ReadByte();
                if (first == 0)
                    characterEndFlag = true;
                else
                {
                    ButtonRecord buttRecord = new ButtonRecord();
                    buttRecord.ReadData(binaryReader, first, TagCodeEnum.DefineButton2);
                    characters.Add(buttRecord);
                }
            }

            long curr = startPos + actionOffset;
			
            actions = new ButtonCondactionCollection();
            bool lastCondAction = false;
            if (actionOffset == 0)
                lastCondAction = true;

            while (!lastCondAction)
            {
                long readedBytes = binaryReader.BaseStream.Position - startPosition;
                ushort condActionSize = binaryReader.ReadUInt16();
                if (condActionSize == 0)
                {
                    lastCondAction = true;
                    condActionSize = (ushort)(rh.TagLength - readedBytes);
                }
                ButtonCondaction buttCond = new ButtonCondaction();
                buttCond.ReadData(binaryReader, condActionSize);
                actions.Add(buttCond);
            }	 
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns>Size of this object.</returns>
        protected int GetSizeOf()
        {
            int res = 5;
            if (characters != null)
            {
				IEnumerator butts = characters.GetEnumerator();
				while (butts.MoveNext())
					res += ((ButtonRecord)butts.Current).GetSizeOf();
            }
            res++;
            if (actions != null)
            {
				IEnumerator buttConds = actions.GetEnumerator();
				while (buttConds.MoveNext())
					res += ((ButtonCondaction)buttConds.Current).GetSizeOf();
            }
            return res;
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
			
            RecordHeader rh = new RecordHeader(TagCode, GetSizeOf());
			
            rh.WriteTo(w);
            w.Write(this.buttonId);
            w.WriteUBits(0, 7);
            w.WriteBoolean(trackAsMenu);
            w.Write(this.actionOffset);
            if (characters != null)
            {
				IEnumerator butts = characters.GetEnumerator();
				while (butts.MoveNext())
				{
					((ButtonRecord)butts.Current).WriteTo(w, TagCodeEnum.DefineButton2);
					w.SynchBits();
				}
            }
            w.Write((byte)0);
            if (actions != null)
            {
				for (int i = 0; i < actions.Count; i++)
				{
					ButtonCondaction buttCon = actions[i];
					if (i == actions.Count - 1)
						w.Write((ushort)0);
					else
					{
						int size = buttCon.GetSizeOf();
						w.Write((ushort)size);
					}
					buttCon.WriteTo(w);
				}
            }

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
			writer.WriteStartElement("DefineButton2Tag");
			writer.WriteAttributeString("ButtonId", buttonId.ToString());
            writer.WriteElementString("TrackAsMenu", trackAsMenu.ToString());
			writer.WriteElementString("ActionOffset", actionOffset.ToString());
            if (characters != null)
            {
				IEnumerator butts = characters.GetEnumerator();
				while (butts.MoveNext())
					((ButtonRecord)butts.Current).Serialize(writer);
            }
            //TODO: Add button condactions
            writer.WriteEndElement();
		}

        #endregion
	}
}
