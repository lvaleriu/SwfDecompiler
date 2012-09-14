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
	/// DefineButtonCxFormTag defines the colour transform that is applied to 
	/// each shape that is used to draw a button.
	/// </summary>
	/// <remarks>
	/// <p>
	/// This class is only used in conjunction with DefineButtonTag. 
	/// DefineButton2Tag allows colour transforms to be specified in the ButtonRecord 
	/// object that identifies each shape that is displayed for a given button state.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 2.
	/// </p>
	/// </remarks>
	public class DefineButtonCxFormTag : BaseTag, DefineTag
	{
        #region Member

		private ushort buttonId;
		private CXForm buttonColorTransform;

        #endregion
		
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="DefineButtonCxFormTag"/> instance.
        /// </summary>
        public DefineButtonCxFormTag()
        {
            this._tagCode = (int)TagCodeEnum.DefineButtonCxForm;
        }

		/// <summary>
		/// Creates a new <see cref="DefineButtonCxFormTag"/> instance.
		/// </summary>
		/// <param name="buttonId">Button id.</param>
		/// <param name="buttonColorTransform">Button color transform.</param>
		public DefineButtonCxFormTag(ushort buttonId, CXForm buttonColorTransform) 
		{
			this.buttonId = buttonId;
			this.buttonColorTransform = buttonColorTransform;
			this._tagCode = (int)TagCodeEnum.DefineButtonCxForm;
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
        /// Gets or sets the button color transform.
        /// </summary>
        public CXForm ButtonColorTransform
        {
            get { return this.buttonColorTransform;  }
            set { this.buttonColorTransform = value; }
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
            buttonColorTransform = new CXForm();
            buttonColorTransform.ReadData(binaryReader);
        }
		
        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns>Size of this object</returns>
        protected int GetSizeOf()
        {
            int res = 2;
            if (buttonColorTransform != null)
                res += buttonColorTransform.GetSizeOf();
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
            if (buttonColorTransform != null)
                buttonColorTransform.WriteTo(w);
            
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
            writer.WriteStartElement("DefineButtonCxFormTag");
            writer.WriteElementString("ButtonId", buttonId.ToString());
            if (buttonColorTransform != null)
                buttonColorTransform.Serialize(writer);
            writer.WriteEndElement();
        }

        #endregion
	}
}
