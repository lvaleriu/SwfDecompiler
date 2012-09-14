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
using System.Text;
using System.Collections;

using SwfDotNet.IO.Utils;
using SwfDotNet.IO.Tags;

namespace SwfDotNet.IO.Tags {

	/// <summary>
	/// FrameLabelTag defines a name for the current frame in a 
	/// movie or movie clip.
	/// </summary>
	/// <remarks>
	/// <p>
	/// The name can be referenced from other objects such as ActionGotoFrame2 
	/// to simplify the creation of scripts to control movies by using 
	/// a predefined name rather than the frame number. The label assigned 
	/// to a particular frame should be unique. Frames may also be 
	/// reference externally when specifying the movie to play using a 
	/// URL - similar to the way names links are used in HTML. 
	/// When the Flash Player loaded a movie it will begin playing at 
	/// the frame specified in the URL.
	/// </p>
	/// <p>
	/// The name is assigned to a particular frame when the FrameLabelTag 
	/// object is defined prior to the ShowFrameTag object that displays 
	/// all the objects on the display list. The frame can then be 
	/// referenced by its name once it has been defined. A frame cannot 
	/// be referenced before the Player has loaded and displayed the frame 
	/// that contains the corresponding FrameLabelTag object.
	/// </p>
	/// <p>
	/// To start playing the movie at the frame labeled "StartHere" specify 
	/// the label using the same format a HTML anchors:<br/>
	/// <pre>http://www.mysite.com/flash/movie.swf#StartHere</pre>
	/// </p>
	/// <p>
	/// This may be used either in a browser window to load a file or form 
	/// within a movie using the ActionGetUrl or ActionGetUrl2 actions.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 3. In Flash 6 the label can support 
	/// named anchors which allows a frame to be specified as the starting 
	/// point when displaying a Flash movie in a web browser.
	/// </p>
	/// </remarks>
	public class FrameLabelTag : BaseTag 
	{	
        #region Members

		private string name;

        #endregion

        #region Ctor
        
        /// <summary>
        /// Creates a new <see cref="FrameLabelTag"/> instance.
        /// </summary>
        public FrameLabelTag()
        {
            this._tagCode = (int)TagCodeEnum.FrameLabel;
        }

		/// <summary>
		/// constructor.
		/// </summary>
		/// <param name="name">label for frame</param>
		public FrameLabelTag(string name) 
		{
			this.name = name;
			this._tagCode = (int)TagCodeEnum.FrameLabel;
		}

        #endregion

        #region Properties

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name
		{
			get { return this.name;  }
			set { this.name = value; }
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
            
            uint max = rh.TagLength;
            if (version >= 6)
                max--;
            name = binaryReader.ReadString(max);
            if (version >= 6)
               binaryReader.ReadByte();
        }

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns></returns>
		public int GetSizeOf(byte version)
		{
			int res = 0;
			if (name != null)
				res += name.Length;
            if (version >= 6)
                res++;
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
			
			RecordHeader rh = new RecordHeader(TagCode, GetSizeOf(version));
			rh.WriteTo(w);
			
			if (name != null)
				w.WriteString(name, (uint)name.Length);
			if (version >= 6)
				w.Write((byte)1);
			
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
			writer.WriteStartElement("FrameLabelTag");
			writer.WriteAttributeString("Name", name);
			writer.WriteEndElement();
		}

        #endregion
	}
}
