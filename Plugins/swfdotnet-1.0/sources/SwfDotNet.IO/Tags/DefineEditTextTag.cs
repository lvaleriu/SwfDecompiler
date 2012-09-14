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
	/// DefineEditTextTag defines an editable text field.
	/// </summary>
	/// <remarks>
	/// <p>
	/// The value entered into the text field is assigned to a specified 
	/// variable allowing the creation of forms to accept values entered 
	/// by a person viewing the Flash file.
	/// </p>
	/// <p>
	/// The class contains a complex set of attributes which allows a high 
	/// degree of control over how a text field is displayed.
	/// </p>
	/// <p>
	/// Additional layout information for the spacing of the text relative 
	/// to the text field borders can also be specified.
	/// </p>
	/// <p>
	/// Setting the HTML flag to true allows text marked up with a limited 
	/// set of HTML tags to be displayed in the text field.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 4.
	/// </p>
	/// </remarks>
	public class DefineEditTextTag : BaseTag, DefineTag, DefineTargetTag
	{
        #region Members

		private ushort characterId;
		private Rect rect;
		private bool wordWrap;
		private bool multiline;
		private bool password;
		private bool readOnly;
		private bool autoSize;
		private bool noSelect;
		private bool border;
		private bool html;
		private bool usedOutlines;
		private ushort fontId;
		private ushort fontHeight;
		private RGBA textColor;
		private ushort maxLenght;
		private byte align;
		private ushort leftMargin;
		private ushort rightMargin;
		private ushort indent;
		private ushort leading;
		private string variableName;
		private string initialText;

        #endregion
		
		#region Ctor & Init

		/// <summary>
		/// constructor.
		/// </summary>
		public DefineEditTextTag() 
		{
			Init();
		}

		/// <summary>
		/// Inits this instance.
		/// </summary>
		protected void Init()
		{
			 _tagCode = (int)TagCodeEnum.DefineEditText;
			this.variableName = string.Empty;
		}

        #endregion

        #region Properties

		/// <summary>
		/// Gets or sets the max lenght.
		/// </summary>
		/// <value></value>
		public ushort MaxLenght
		{
			get { return this.maxLenght;  }
			set { this.maxLenght = value; }
		}

		/// <summary>
		/// Gets or sets the left margin.
		/// </summary>
		/// <value></value>
		public ushort LeftMargin
		{
			get { return this.leftMargin;  }
			set { this.leftMargin = value; }
		}

		/// <summary>
		/// Gets or sets the right margin.
		/// </summary>
		/// <value></value>
		public ushort RightMargin
		{
			get { return this.rightMargin;  }
			set { this.rightMargin = value; }
		}

		/// <summary>
		/// Gets or sets the indent.
		/// </summary>
		/// <value></value>
		public ushort Indent
		{
			get { return this.indent;  }
			set { this.indent = value; }
		}

		/// <summary>
		/// Gets or sets the leading.
		/// </summary>
		/// <value></value>
		public ushort Leading
		{
			get { return this.leading;  }
			set { this.leading = value; }
		}

		/// <summary>
		/// Gets or sets the name of the variable.
		/// </summary>
		/// <value></value>
		public string VariableName
		{
			get { return this.variableName;  }
			set { this.variableName = value; }
		}

		/// <summary>
		/// Gets or sets the align.
		/// </summary>
		/// <value></value>
		public byte Align
		{
			get { return this.align;  }
			set { this.align = value; }
		}

		/// <summary>
		/// Gets or sets the height of the font.
		/// </summary>
		/// <value></value>
		public ushort FontHeight
		{
			get { return this.fontHeight;  }
			set { this.fontHeight = value; }
		}

		/// <summary>
		/// Gets or sets the font id.
		/// </summary>
		/// <value></value>
		public ushort FontId
		{
			get { return this.fontId;  }
			set { this.fontId = value; }
		}

        /// <summary>
        /// Target tag's character id
        /// </summary>
        /// <value></value>
        public ushort TargetCharacterId
        {
            get { return this.fontId; }
            set { this.fontId = value; }
        }

		/// <summary>
		/// Gets or sets the rect.
		/// </summary>
		/// <value></value>
		public Rect Rect
		{
			get { return this.rect;  }
			set { this.rect = value; }
		}

		/// <summary>
		/// Gets or sets the color of the text.
		/// </summary>
		/// <value></value>
		public RGBA TextColor
		{
			get { return this.textColor;  }
			set { this.textColor = value; }
		}

		/// <summary>
		/// Gets or sets the initial text.
		/// </summary>
		/// <value></value>
		public string InitialText
		{
			get { return this.initialText;  }
			set { this.initialText = value; }
		}

        /// <summary>
        /// see <see cref="SwfDotNet.IO.Tags.DefineTag"/>
        /// </summary>
        public ushort CharacterId
        {
            get { return this.characterId;  }
            set { this.characterId = value; }
        }

		/// <summary>
		/// Gets or sets a value indicating whether [word wrap].
		/// </summary>
		public bool WordWrap
		{
			get { return this.wordWrap;  }
			set { this.wordWrap = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="DefineEditTextTag"/> is multiline.
		/// </summary>
		public bool Multiline
		{
			get { return this.multiline;  }
			set { this.multiline = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="DefineEditTextTag"/> is password.
		/// </summary>
		public bool Password
		{
			get { return this.password;  }
			set { this.password = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether [read only].
		/// </summary>
		public bool ReadOnly
		{
			get { return this.readOnly;  }
			set { this.readOnly = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether [auto size].
		/// </summary>
		public bool AutoSize
		{
			get { return this.autoSize;  }
			set { this.autoSize = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether [no select].
		/// </summary>
		public bool NoSelect
		{
			get { return this.noSelect;  }
			set { this.noSelect = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="DefineEditTextTag"/> is border.
		/// </summary>
		public bool Border
		{
			get { return this.border;  }
			set { this.border = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="DefineEditTextTag"/> is HTML.
		/// </summary>
		public bool Html
		{
			get { return this.html;  }
			set { this.html = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether [used outlines].
		/// </summary>
		public bool UsedOutlines
		{
			get { return this.usedOutlines;  }
			set { this.usedOutlines = value; }
		}

        #endregion

        #region Methods

		/// <summary>
		/// Gets a value indicating whether this instance has max length.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has max length; otherwise, <c>false</c>.
		/// </value>
		private bool HasMaxLength
		{
			get { return this.maxLenght != 0;  }
		}

		/// <summary>
		/// Gets a value indicating whether this instance has text.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has text; otherwise, <c>false</c>.
		/// </value>
		private bool HasText
		{
			get { return this.initialText != null;  }
		}

		/// <summary>
		/// Gets a value indicating whether this instance has font.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has font; otherwise, <c>false</c>.
		/// </value>
		private bool HasFont
		{
			get { return this.fontId != 0 && this.fontHeight != 0;  }
		}

		/// <summary>
		/// Gets a value indicating whether this instance has text color.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has text color; otherwise, <c>false</c>.
		/// </value>
		private bool HasTextColor
		{
			get { return this.textColor != null; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance has layout.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has layout; otherwise, <c>false</c>.
		/// </value>
		private bool HasLayout
		{
			get
			{
				return this.align != 0 || this.leftMargin != 0 ||
					this.rightMargin != 0 || this.indent != 0 ||
					this.leading != 0;
			}
		}

        /// <summary>
        /// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
        /// </summary>
        public override void ReadData(byte version, BufferedBinaryReader binaryReader)
        {
            RecordHeader rh = new RecordHeader();
            rh.ReadData(binaryReader);

            characterId = binaryReader.ReadUInt16();
            rect = new Rect();
            rect.ReadData(binaryReader);

			BitArray ba = BitParser.GetBitValues(new byte[1]{ binaryReader.ReadByte() });

			bool hasText = ba.Get(0); //binaryReader.ReadBoolean();
			wordWrap = ba.Get(1); //binaryReader.ReadBoolean();
			multiline = ba.Get(2); //binaryReader.ReadBoolean();
			password = ba.Get(3); //binaryReader.ReadBoolean();
			readOnly = ba.Get(4); //binaryReader.ReadBoolean();
			bool hasTextColor = ba.Get(5); //binaryReader.ReadBoolean();
			bool hasMaxLength = ba.Get(6); //binaryReader.ReadBoolean();
			bool hasFont = ba.Get(7); //binaryReader.ReadBoolean();
			//binaryReader.SynchBits();

			ba = BitParser.GetBitValues(new byte[1]{ binaryReader.ReadByte() });
			//binaryReader.ReadBoolean(); //Reserved
			autoSize = ba.Get(1); //binaryReader.ReadBoolean();
			bool hasLayout = ba.Get(2); //binaryReader.ReadBoolean();
			noSelect = ba.Get(3); //binaryReader.ReadBoolean();
			border = ba.Get(4); //binaryReader.ReadBoolean();
			//binaryReader.ReadBoolean(); //Reserved
			html = ba.Get(6); //binaryReader.ReadBoolean();
			usedOutlines = ba.Get(7); //binaryReader.ReadBoolean();

            if (hasFont)
            {
                fontId = binaryReader.ReadUInt16();
                fontHeight = binaryReader.ReadUInt16();
            }

            if (hasTextColor)
            {
                textColor = new RGBA();
                textColor.ReadData(binaryReader);
            }

            if (hasMaxLength)
                maxLenght = binaryReader.ReadUInt16();
            
            if (hasLayout)
            {
                align = binaryReader.ReadByte();
                leftMargin = binaryReader.ReadUInt16();
                rightMargin = binaryReader.ReadUInt16();
                indent = binaryReader.ReadUInt16();
                leading = binaryReader.ReadUInt16();
            }

            variableName = binaryReader.ReadString();
            if (hasText)
                initialText = binaryReader.ReadString();
        }

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns></returns>
		public int GetSizeOf()
		{
			int res = 2;
			if (rect != null)
				res += rect.GetSizeOf();
			res += 2;
			if (HasFont)
				res += 4;			
			if (HasTextColor)
				res += textColor.GetSizeOf();
			if (HasMaxLength)
				res += 2;
   			if (HasLayout)
				res += 9;
			res += variableName.Length + 1;
			if (HasText)
				res += initialText.Length + 1;
			return res;
		}
		
		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
		{			
			if (version < 4)
				return;

			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			RecordHeader rh = new RecordHeader(TagCode, GetSizeOf());
			rh.WriteTo(w);

			w.Write(this.characterId);
			this.rect.WriteTo(w);
			
            w.SynchBits();
			if (initialText != null && initialText.Length > 0)
				w.WriteBoolean(true);
			else
				w.WriteBoolean(false);
			w.WriteBoolean(wordWrap);
			w.WriteBoolean(multiline);
			w.WriteBoolean(password);
			w.WriteBoolean(readOnly);
			if (textColor != null)
				w.WriteBoolean(true);
			else
				w.WriteBoolean(false);
			w.WriteBoolean(HasMaxLength);
			w.WriteBoolean(HasFont);
            w.SynchBits();

			w.WriteBoolean(false);
			w.WriteBoolean(autoSize);
			w.WriteBoolean(HasLayout);
			w.WriteBoolean(noSelect);
			w.WriteBoolean(border);
			w.WriteBoolean(false);
			w.WriteBoolean(html);
			w.WriteBoolean(usedOutlines);

			if (HasFont)
			{
				w.Write(fontId);
				w.Write(fontHeight);
			}
			if (HasTextColor)
				textColor.WriteTo(w);
			if (HasMaxLength)
				w.Write(maxLenght);
			if (HasLayout)
			{
				w.Write(align);
				w.Write(leftMargin);
				w.Write(rightMargin);
				w.Write(indent);
				w.Write(leading);
			}
			w.WriteString(variableName);
			if (HasText)
				w.WriteString(initialText);

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
			writer.WriteStartElement("DefineEditTextTag");
			writer.WriteElementString("CharacterId", characterId.ToString());
			this.rect.Serialize(writer);
			writer.WriteElementString("WordWrap", wordWrap.ToString());
			writer.WriteElementString("Multiline", multiline.ToString());
			writer.WriteElementString("Password", password.ToString());
			writer.WriteElementString("ReadOnly", readOnly.ToString());
			writer.WriteElementString("HasMaxLength", HasMaxLength.ToString());
			writer.WriteElementString("HasFont", HasFont.ToString());
			writer.WriteElementString("AutoSize", autoSize.ToString());
			writer.WriteElementString("HasLayout", HasLayout.ToString());
			writer.WriteElementString("NoSelect", noSelect.ToString());
			writer.WriteElementString("Border", border.ToString());
			writer.WriteElementString("Html", html.ToString());
			writer.WriteElementString("UsedOutlines", usedOutlines.ToString());
			
			if (HasFont)
			{
				writer.WriteElementString("FontId", fontId.ToString());
				writer.WriteElementString("FontHeight", fontHeight.ToString());
			}
			if (HasTextColor)
				textColor.Serialize(writer);
			if (HasMaxLength)
				writer.WriteElementString("MaxLenght", maxLenght.ToString());
			if (HasLayout)
			{
				writer.WriteElementString("Align", align.ToString());
				writer.WriteElementString("LeftMargin", leftMargin.ToString());
				writer.WriteElementString("RightMargin", rightMargin.ToString());
				writer.WriteElementString("Indent", indent.ToString());
				writer.WriteElementString("Leading", leading.ToString());
			}
			writer.WriteElementString("VariableName", variableName.ToString());
			if (HasText)
				writer.WriteElementString("InitialText", initialText.ToString());
			
			writer.WriteEndElement();
		}

        #endregion
	}
}
