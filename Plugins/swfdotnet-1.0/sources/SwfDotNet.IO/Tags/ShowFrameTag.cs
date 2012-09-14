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
	/// ShowFrameTag tag is used to instruct the Flash Player to 
	/// display a single frame in a movie or movie clip.
	/// </summary>
	/// <remarks>
	/// <p>
	/// When a frame is displayed the Flash Player performs the following:
	/// <ul>
	/// <li>The contents of the Flash Player's display list are drawn on the screen.</li>
	/// <li>Any actions defined using a DoActionTag object are executed.</li>
	/// </ul>
	/// </p>
	/// <p>
	/// Objects are placed in the display list using the PlaceObjectTag and 
	/// PlaceObject2Tag classes and removed using the RemoveObjectTag and 
	/// RemoveObject2Tag classes. An object which has been added to the 
	/// display list will be displayed in each frame until it is explicitly 
	/// removed. There is no need to repeatedly add it to the display list.
	/// </p>
	/// <p>
	/// The scope of a frame is delineated by successive ShowFrameTag objects. 
	/// All the commands that affect change the state of the display list or 
	/// define actions to be executed take effect when the Flash Player displays 
	/// the frame. All the objects displayed in a frame must be defined before 
	/// they can be displayed. The movie is displayed as it is downloaded so 
	/// displaying objects that are defined later in a movie is not allowed.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 1.
	/// </p>
	/// </remarks>
	/// <example>
    /// <p>
    /// <code lang="C#">
    /// Swf swf = new Swf();
    /// // Frame 1 - starts from the beginning of the file....
    /// swf.Add(new ShowFrameTag());
    /// // Frame 2 - starts when the previous frame is displayed.
    /// // All displayable objects are referenced using unique identifier.
    /// int identifer = swf.newIdentifier();
    /// // Define a shape to be displayed.
    /// swf.Add(new DefineShapeTag(identifer, ......));
    /// // Add the shape to the display list - on layer 1 at coordinates (400, 400)
    /// swf.Add(new PlaceObjectTag(identifier, 1, 400, 400));
    /// // Add some actions
    /// DoActionTag frameActions = new DoActionTag(); 
    /// frameActions.add(anAction);
    /// frameActions.add(anotherAction);
    /// swf.Add(frameActions);
    /// // The shape is displayed and the actions executed when the FSShowFrame command is executed.
    /// swf.Add(new ShowFrameTag());
    /// </code>
    /// </p>
	/// </example>
	public class ShowFrameTag : BaseTag 
    {
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="ShowFrameTag"/> instance.
        /// </summary>
		public ShowFrameTag() 
		{
			this._tagCode = (int)TagCodeEnum.ShowFrame;
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
        }
		
		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
		{			
			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
			RecordHeader rh = new RecordHeader(TagCode, 0);
			rh.WriteTo(w);
			
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
			writer.WriteStartElement("ShowFrameTag");
			writer.WriteEndElement();
		}

		#endregion
	}
}
