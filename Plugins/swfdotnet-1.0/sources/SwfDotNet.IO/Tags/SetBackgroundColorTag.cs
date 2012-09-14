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
	/// SetBackgroundColorTag object defines a background color 
	/// of the swf file. It sets the background color displayed 
	/// in every frame in the movie.
	/// </summary>
	/// <remarks>
	/// <p>
	/// Although the color is specified using an RGB object the 
	/// colour displayed is completely opaque.
	/// </p>
	/// <p>
	/// The background color must be set before the first frame is
	/// displayed otherwise the background color defaults to white.
	/// This is typically the first object in a coder. 
	/// If more than one SetBackgroundColorTag object is added to a 
	/// swf then only first one sets the background color. 
	/// Subsequent objects are ignored.
	/// </p>
	/// </remarks>
	/// <example>
    /// <code lang="C#">
    /// Swf swf = new Swf();
    /// swf.FrameSize = new Rect(0, 0, 8000, 8000)); // in twips = 400 x 400 in pixels
    /// swf.FrameRate = 1.0; 1 frame per second.
    /// swf.Add(new SetBackgroundColorTag(new RGB(0, 0, 255))); // Blue
    /// </code>
	/// </example>
	public class SetBackgroundColorTag : BaseTag 
	{
		#region Members

		private RGB rgb;

		#endregion

		#region Ctor

		/// <summary>
		/// Creates a new <see cref="SetBackgroundColorTag"/> instance.
		/// </summary>
		public SetBackgroundColorTag() 
		{
			this._tagCode = (int)TagCodeEnum.SetBackgroundColor;
		}

		/// <summary>
		/// Creates a new <see cref="SetBackgroundColorTag"/> instance.
		/// </summary>
		/// <param name="rgbColor">Color of the RGB.</param>
		public SetBackgroundColorTag(RGB rgbColor) 
		{
			this.rgb = rgbColor;
			this._tagCode = (int)TagCodeEnum.SetBackgroundColor;
		}

        /// <summary>
        /// Creates a new <see cref="SetBackgroundColorTag"/> instance.
        /// </summary>
        /// <param name="red">Red.</param>
        /// <param name="green">Green.</param>
        /// <param name="blue">Blue.</param>
        public SetBackgroundColorTag(byte red, byte green, byte blue)
        {
            this.rgb = new RGB(red, green, blue);   
            this._tagCode = (int)TagCodeEnum.SetBackgroundColor;
		}

		#endregion
		
		#region Properties

		/// <summary>
		/// Gets or sets the RGB color.
		/// </summary>
		public RGB RGB
		{
			get { return this.rgb;  }
			set { this.rgb = value; }
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

            rgb = new RGB();
            rgb.ReadData(binaryReader);
        }

		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
		{			
			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
			RecordHeader rh = new RecordHeader(TagCode, rgb.GetSizeOf());
			rh.WriteTo(w);
			rgb.WriteTo(w);
			
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
			writer.WriteStartElement("SetBackgroundColorTag");
			this.rgb.Serialize(writer);
			writer.WriteEndElement();
		}
		
		#endregion
	}
}
