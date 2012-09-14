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
	/// RemoveObject Tag removes an object from the Flash Player's 
	/// Display List.
	/// </summary>
	/// <remarks>
	/// <p>
	/// An object placed on the display list is displayed in every frame 
	/// of a movie until it is explicitly removed. Objects must also 
	/// be removed if its location or appearance is changed using either 
	/// the RemoveObjectTag or RemoveObject2Tag classes.
	/// </p>
	/// <p>
	/// Although only one object can be placed on any layer in the display 
	/// list both the object's unique identifier and the layer number 
	/// must be specified. The RemoveObjectTag class is superceded in 
	/// Flash 3 by the RemoveObject2Tag class which lifts this requirement 
	/// allowing an object to be referenced by the layer number it occupies 
	/// in the display list.
	/// </p>
	/// <p>
	/// <b>It was introduced in Flash 1 and is superceded by the 
	/// RemoveObject2 tag which was added in Flash 3.</b>
	/// </p>
	/// </remarks>
	/// <example>
    /// <p>
    /// <u>Sample 1: Remove an object.</u><br/>
    /// To remove an object from the display list the object's identifier 
    /// and the layer number using when the object was placed is used.
    /// <code lang="C#">
    /// // Place a shape to the display list for one frame.
    /// swf.Add(new PlaceObjectTag(shape.CharacterId, 1, 400, 400));
    /// swf.Add(new ShowFrameTag());
    /// // now remove it.
    /// swf.Add(new RemoveObjectTag(shape.CharacterId, 1));
    /// swf.Add(new ShowFrameTag());
    /// </code>
    /// </p>
    /// <p>
    /// <u>Sample 2: Move an object.</u><br/>
    /// To move an object it first must be removed from the display list 
    /// and repositioned at its new location. Adding the object, with a new 
    /// location, on the same layer, although only one object can be 
    /// displayed on a given layer, will not work. The object will be 
    /// displayed twice.
    /// <code lang="C#">
    /// // Add the shape to the display list.
    /// swf.Add(new PlaceObjectTag(shape.CharacterId, 1, 400, 400));
    /// swf.Add(new ShowFrameTag());
    /// // Move shape to a new location, removing the original so it does not get displayed twice.
    /// swf.Add(new RemoveObjectTag(shape.CharacterId, 1));
    /// swf.Add(new PlaceObjectTag(shape.CharacterId, 1, 250, 300));
    /// swf.Add(new ShowFrameTag());
    /// </code>
    /// </p>
    /// </example>
	public class RemoveObjectTag : BaseTag 
	{	
        #region Members

		private ushort characterId;
		private ushort depth;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="RemoveObjectTag"/> instance.
        /// </summary>
        public RemoveObjectTag() 
        {
            this._tagCode = (int)TagCodeEnum.RemoveObject;
        }

		/// <summary>
		/// Creates a new <see cref="RemoveObjectTag"/> instance.
		/// </summary>
		/// <param name="characterId">ID of character to remove. It's the unique identifier, in the range 1..65535, of the object.</param>
		/// <param name="depth">depth of character. It's the layer at which the object is placed in the Display List.</param>
		public RemoveObjectTag(ushort characterId, ushort depth) 
		{
			this.characterId = characterId;
			this.depth = depth;
			this._tagCode = (int)TagCodeEnum.RemoveObject;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the character id.
        /// That's the unique identifier, in the range 1..65535, 
        /// of the object to remove.
        /// </summary>
        public ushort CharacterId
        {
            get { return this.characterId;  }
            set { this.characterId = value; }
        }

        /// <summary>
        /// Gets or sets the depth.
        /// That's the layer at which the object to remove
        /// is placed in the Display List.
        /// </summary>
        public ushort Depth
        {
            get { return this.depth;  }
            set { this.depth = value; }
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
            characterId = binaryReader.ReadUInt16();
            depth = binaryReader.ReadUInt16();
        }
		
		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
		{
			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
			RecordHeader rh = new RecordHeader(TagCode, 4);
			rh.WriteTo(w);
			
			w.Write(characterId);
			w.Write(depth);
			
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
			writer.WriteStartElement("RemoveObjectTag");
			writer.WriteElementString("CharacterId", characterId.ToString());
			writer.WriteElementString("Depth", depth.ToString());
			writer.WriteEndElement();
		}
		
        #endregion
	}
}
