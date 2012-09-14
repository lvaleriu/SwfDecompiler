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
	/// PlaceObjectTag is used to add an object (shape, button, etc.) 
	/// to the Flash Player's display list.
	/// </summary>
	/// <remarks>
	/// <p>
	/// Every class that defines a shape, button etc. is assigned a unique 
	/// identifier. This is an integer in the range 1..65535 and is used to 
	/// refer to objects when performing actions such as adding or removing 
	/// them from the display list.
	/// </p>
	///	<p>
	///	The display list contains all the objects that are currently visible 
	///	on the Flash Player's screen. The display list is ordered in layers, 
	///	with one (and only one) object displayed on each layer. The Layer 
	///	defines the order in which objects are displayed. Objects with a 
	///	higher layer number are displayed in front of objects on a lower layer.
	///	</p>
	///	<p>
	///	The coordinate transform is principally used to specify the location 
	///	of the object when it is drawn on the screen however more complex 
	///	coordinate transforms can also be specified such as rotating or scaling 
	///	the object without changing the original definition.
	///	</p>
	/// <p>
	/// Similarly the color transform allows the color of the object to be 
	/// changed when it is displayed without changing the original definition. 
	/// The PlaceObjectTag class only supports opaque colors so although the 
	/// CXForm supports transparent colors this information is ignored 
	/// by the Flash Player. The color transform is optional and may be set 
	/// to the null object.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 1 and is superceded by the PlaceObject2 
	/// tag which was added in Flash 3.
	/// </p>
	/// </remarks>
    public class PlaceObjectTag : BaseTag, DefineTargetTag
	{
        #region Members

		private ushort characterId;
		private ushort depth;
		private Matrix matrix = null;
		private CXForm colorTransform = null;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="PlaceObjectTag"/> instance.
        /// </summary>
        public PlaceObjectTag()
        {
            this._tagCode = (int)TagCodeEnum.PlaceObject;
        }

        /// <summary>
        /// Creates a new <see cref="PlaceObjectTag"/> instance.
        /// </summary>
        /// <param name="characterId">Character id.</param>
        /// <param name="depth">Depth.</param>
        /// <param name="xLocation">X location.</param>
        /// <param name="yLocation">Y location.</param>
        public PlaceObjectTag(ushort characterId, ushort depth, int xLocation, int yLocation)
        {
            this.characterId = characterId;
            this.depth = depth;
            this.matrix = new Matrix(xLocation, yLocation);
            this._tagCode = (int)TagCodeEnum.PlaceObject; 
        }

        /// <summary>
        /// Creates a new <see cref="PlaceObjectTag"/> instance.
        /// </summary>
        /// <param name="characterId">Character id.</param>
        /// <param name="depth">Depth.</param>
        /// <param name="matrix">Matrix.</param>
        public PlaceObjectTag(ushort characterId, ushort depth, Matrix matrix)
        {
            this.characterId = characterId;
            this.depth = depth;
            this.matrix = matrix;
            this._tagCode = (int)TagCodeEnum.PlaceObject; 
        }

        /// <summary>
        /// Creates a new <see cref="PlaceObjectTag"/> instance.
        /// </summary>
        /// <param name="characterId">Character id.</param>
        /// <param name="depth">Depth.</param>
        /// <param name="matrix">Matrix.</param>
        /// <param name="colorTransform">Color transform.</param>
		public PlaceObjectTag(ushort characterId, ushort depth, Matrix matrix, CXForm colorTransform) 
		{
			this.characterId = characterId;
			this.depth = depth;
			this.matrix = matrix;
			this.colorTransform = colorTransform;
            this._tagCode = (int)TagCodeEnum.PlaceObject;
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets or sets the character id.
        /// </summary>
        public ushort TargetCharacterId
        {
            get { return this.characterId;  }
            set { this.characterId = value; }
        }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        public ushort Depth
        {
            get { return this.depth;  }
            set { this.depth = value; }
        }

        /// <summary>
        /// Gets or sets the matrix.
        /// </summary>
        public Matrix Matrix
        {
            get { return this.matrix;  }
            set { this.matrix = value; }
        }

        /// <summary>
        /// Gets or sets the color transform.
        /// </summary>
        public CXForm ColorTransform
        {
            get { return this.colorTransform;  }
            set { this.colorTransform = value; }
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

            long initPos = binaryReader.BaseStream.Position;
            
            characterId = binaryReader.ReadUInt16();
            depth = binaryReader.ReadUInt16();
            matrix = new Matrix();
            matrix.ReadData(binaryReader);
            
            long pos = binaryReader.BaseStream.Position - initPos;
            if (pos < rh.TagLength)
            {
                colorTransform = new CXForm();
                colorTransform.ReadData(binaryReader);
            }
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns></returns>
        public int GetSizeOf()
        {
            int length = 4;
            if (this.matrix != null)
                length += this.matrix.GetSizeOf();
            if (this.colorTransform != null)
                length += this.colorTransform.GetSizeOf();
            return length;
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
			
			w.Write(characterId);
			w.Write(depth);
			if (this.matrix != null)
				this.matrix.WriteTo(w);
			if (this.colorTransform != null)
				this.colorTransform.WriteTo(w);
			
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
			writer.WriteStartElement("PlaceObjectTag");
			writer.WriteElementString("CharacterId", characterId.ToString());
			writer.WriteElementString("Depth", depth.ToString());
			if (this.matrix != null)
				this.matrix.Serialize(writer);
			if (this.colorTransform != null)
				this.colorTransform.Serialize(writer);
			writer.WriteEndElement();
		}
	
        #endregion
	}
}
