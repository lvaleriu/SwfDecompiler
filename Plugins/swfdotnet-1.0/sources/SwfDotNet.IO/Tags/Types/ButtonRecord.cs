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

namespace SwfDotNet.IO.Tags.Types
{
	/// <summary>
	/// ButtonRecord class
	/// </summary>
	public class ButtonRecord: ISwfSerializer, DefineTargetTag
	{
        #region Members

        private bool buttonStateHitTest;
        private bool buttonStateDown;
        private bool buttonStateOver;
        private bool buttonStateUp;
		private ushort characterId;
		private ushort placeDepth;
		private Matrix placeMatrix = null;
		private CXFormWithAlphaData colorTransform = null;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="ButtonRecord"/> instance.
        /// </summary>
        public ButtonRecord()
        {
        }

        /// <summary>
        /// Creates a new <see cref="ButtonRecord"/> instance.
        /// </summary>
        /// <param name="buttonStateHitTest">Button state hit test.</param>
        /// <param name="buttonStateDown">Button state down.</param>
        /// <param name="buttonStateOver">Button state over.</param>
        /// <param name="buttonStateUp">Button state up.</param>
        /// <param name="characterId">Character id.</param>
        /// <param name="placeDepth">Place depth.</param>
        /// <param name="placeMatrix">Place matrix.</param>
        /// <param name="colorTransform">Color transform.</param>
        public ButtonRecord(bool buttonStateHitTest, bool buttonStateDown,
            bool buttonStateOver, bool buttonStateUp, ushort characterId,
            ushort placeDepth, Matrix placeMatrix, 
            CXFormWithAlphaData colorTransform)
		{
            this.buttonStateHitTest = buttonStateHitTest;
            this.buttonStateDown = buttonStateDown;
            this.buttonStateOver = buttonStateOver;
            this.buttonStateUp = buttonStateUp;
			this.characterId = characterId;
			this.placeDepth = placeDepth;
			this.placeMatrix = placeMatrix;
			this.colorTransform = colorTransform;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [button state hit test].
        /// </summary>
        public bool ButtonStateHitTest
        {
            get { return this.buttonStateHitTest;  }
            set { this.buttonStateHitTest = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [button state down].
        /// </summary>
        public bool ButtonStateDown
        {
            get { return this.buttonStateDown;  }
            set { this.buttonStateDown = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [button state over].
        /// </summary>
        public bool ButtonStateOver
        {
            get { return this.buttonStateOver;  }
            set { this.buttonStateOver = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [button state up].
        /// </summary>
        public bool ButtonStateUp
        {
            get { return this.buttonStateUp;  }
            set { this.buttonStateUp = value; }
        }

        /// <summary>
        /// Gets or sets the character id.
        /// </summary>
        public ushort TargetCharacterId
        {
            get { return this.characterId;  }
            set { this.characterId = value; }
        }

        /// <summary>
        /// Gets or sets the place depth.
        /// </summary>
        public ushort PlaceDepth
        {
            get { return this.placeDepth;  }
            set { this.placeDepth = value; }
        }

        /// <summary>
        /// Gets or sets the place matrix.
        /// </summary>
        public Matrix PlaceMatrix
        {
            get { return this.placeMatrix;  }
            set { this.placeMatrix = value; }
        }

        /// <summary>
        /// Gets or sets the color transform.
        /// </summary>
        public CXFormWithAlphaData ColorTransform
        {
            get { return this.colorTransform;  }
            set { this.colorTransform = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        /// <param name="firstByte">First byte.</param>
        /// <param name="buttonType">Button type.</param>
        public void ReadData(BufferedBinaryReader binaryReader,
            byte firstByte, TagCodeEnum buttonType)
        {
            BitArray ba = BitParser.GetBitValues(new byte[1]{firstByte});
            buttonStateHitTest = ba.Get(4);
            buttonStateDown = ba.Get(5);
            buttonStateOver = ba.Get(6);
            buttonStateUp = ba.Get(7);
            
			characterId = binaryReader.ReadUInt16();
			placeDepth = binaryReader.ReadUInt16();
			placeMatrix = new Matrix();
            placeMatrix.ReadData(binaryReader);
			colorTransform = null;

            if (buttonType == TagCodeEnum.DefineButton2)
            {
                colorTransform = new CXFormWithAlphaData();
                colorTransform.ReadData(binaryReader);
            }
        }

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <returns>Size</returns>
		public int GetSizeOf()
		{
			int res = 5;
			if (placeMatrix != null)
				res += placeMatrix.GetSizeOf();
			if (colorTransform != null)
				res += colorTransform.GetSizeOf();
			return res;
		}

        /// <summary>
        /// Writes to a binary writer.
        /// </summary>
        /// <param name="writer">Writer.</param>
        /// <param name="buttonType">Button type.</param>
		public void WriteTo(BufferedBinaryWriter writer, TagCodeEnum buttonType)
		{
			writer.WriteUBits(0, 4);
			writer.WriteBoolean(buttonStateHitTest);
			writer.WriteBoolean(buttonStateDown);
			writer.WriteBoolean(buttonStateOver);
			writer.WriteBoolean(buttonStateUp);
			
			writer.Write(this.characterId);
            writer.Write(this.placeDepth);
			if (placeMatrix != null)
				placeMatrix.WriteTo(writer);
			if (colorTransform != null && buttonType == TagCodeEnum.DefineButton2)
				colorTransform.WriteTo(writer);
		}

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("ButtonRecord");
			writer.WriteElementString("ButtonStateHitTest", buttonStateHitTest.ToString());
			writer.WriteElementString("ButtonStateDown", buttonStateDown.ToString());
			writer.WriteElementString("ButtonStateOver", buttonStateOver.ToString());
			writer.WriteElementString("ButtonStateUp", buttonStateUp.ToString());
			writer.WriteElementString("CharacterId", characterId.ToString());
			writer.WriteElementString("PlaceDepth", placeDepth.ToString());
			if (placeMatrix != null)
				placeMatrix.Serialize(writer);
			if (colorTransform != null)
				colorTransform.Serialize(writer);
			writer.WriteEndElement();
		}

        #endregion
	}

    /// <summary>
    /// ButtonRecordCollection class
    /// </summary>
    public class ButtonRecordCollection: CollectionBase, ISwfSerializer
    {
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="ButtonRecordCollection"/> instance.
        /// </summary>
        public ButtonRecordCollection()
        {
        }

        #endregion

		#region Methods

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("ButtonRecordCollection");

            IEnumerator buttonRecords = this.GetEnumerator();
            while (buttonRecords.MoveNext())
                ((ButtonRecord)buttonRecords.Current).Serialize(writer);
			
            writer.WriteEndElement();
		}

		#endregion

        #region Collection methods

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public ButtonRecord Add(ButtonRecord value)
        {
            List.Add(value as object);
            return value;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="values">Values.</param>
        public void AddRange(ButtonRecord[] values)
        {
            IEnumerator val = values.GetEnumerator();
            while (val.MoveNext())
                Add((ButtonRecord)val.Current);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public void Remove(ButtonRecord value)
        {
            if (List.Contains(value))
                List.Remove(value as object);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="value">Value.</param>
        public void Insert(int index, ButtonRecord value)
        {
            List.Insert(index, value as object);
        }

        /// <summary>
        /// Containses the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public bool Contains(ButtonRecord value)
        {
            return List.Contains(value as object);
        }

        /// <summary>
        /// Gets or sets the <see cref="LineStyle"/> at the specified index.
        /// </summary>
        /// <value></value>
        public ButtonRecord this[int index]
        {
            get
            {
                return ((ButtonRecord)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Get the index of.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public int IndexOf(ButtonRecord value)
        {
            return List.IndexOf(value);
        }

        #endregion
    }
}
