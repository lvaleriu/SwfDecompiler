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
	
	#region Enum

	/// <summary>
	/// ShapeType enum
	/// </summary>
	public enum ShapeType
	{
		/// <summary>
		/// Classical shape
		/// </summary>
		Shape = 0,
		/// <summary>
		/// Shape 2
		/// </summary>
		Shape2 = 1,
		/// <summary>
		/// Shape 3
		/// </summary>
		Shape3 = 2,
        /// <summary>
        /// No shape define
        /// </summary>
        None = 3
	}

	#endregion

	/// <summary>
	/// <p>
	/// DefineShape is the abstract class for the DefineShape tags:
	/// <see cref="SwfDotNet.IO.Tags.DefineShapeTag"/>, 
	/// <see cref="SwfDotNet.IO.Tags.DefineShape2Tag"/>, 
	/// <see cref="SwfDotNet.IO.Tags.DefineShape3Tag"/> 
	/// This class provides the members, properties, read and write methods
	/// common to this 3 tags.
	/// </p>
	/// </summary>
	public abstract class DefineShape : BaseTag, DefineTag 
	{	
		#region Members

        /// <summary>
        /// Shape Id
        /// </summary>
		protected ushort shapeId;
		
        /// <summary>
		/// Rect
		/// </summary>
        protected Rect rect;
		
        /// <summary>
		/// Shape records
		/// </summary>
        protected ShapeWithStyle shape;
		
        /// <summary>
        /// Shape type
        /// </summary>
        protected ShapeType shapeType;

        /// <summary>
        /// Swf version shape compatibility
        /// </summary>
        protected byte versionCompatibility;

		#endregion

		#region Ctor

        /// <summary>
        /// Creates a new <see cref="DefineShape"/> instance.
        /// </summary>
        protected DefineShape()
        {
            Init();
        }

		/// <summary>
		/// Creates a new <see cref="DefineShape"/> instance.
		/// </summary>
		/// <param name="shapeId">Shape id.</param>
		/// <param name="rect">Rect.</param>
		/// <param name="shape">Shape.</param>
		protected DefineShape(ushort shapeId, Rect rect, ShapeWithStyle shape)
		{
            Init();
			this.shapeId = shapeId;
			this.rect = rect;
			this.shape = shape;
		}

        /// <summary>
        /// Inits this instance.
        /// </summary>
        private void Init()
        {
            this.rect = new Rect();
            this.shape = new ShapeWithStyle();
        }

		#endregion

		#region Properties

		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.DefineTag"/>
		/// </summary>
		public ushort CharacterId
		{
			get { return this.shapeId;  }
			set { this.shapeId = value; }
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
        /// Gets or sets the shape with style.
        /// </summary>
        /// <value></value>
		public ShapeWithStyle ShapeWithStyle
		{
			get { return this.shape;  }
            set { this.shape = value; }
		}

		#endregion

		#region Methods

        /// <summary>
        /// Gets the instance from version.
        /// </summary>
        /// <param name="version">Version.</param>
        /// <returns></returns>
        public static DefineShape GetInstanceFromVersion(uint version)
        {
            if (version == 1)
                return new DefineShapeTag();
            if (version == 2)
                return new DefineShape2Tag();
            return new DefineShape3Tag();
        }

        /// <summary>
        /// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
        /// </summary>
        public override void ReadData(byte version, BufferedBinaryReader binaryReader)
        {
            RecordHeader rh = new RecordHeader();
            rh.ReadData(binaryReader);

            shapeId = binaryReader.ReadUInt16();
            rect.ReadData(binaryReader);
            shape.ReadData(binaryReader, this.shapeType);
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
            if (shape != null)
                res += shape.GetSizeOf();
            return res;
        }

		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version)
		{
            if (version < this.versionCompatibility)
                return;
            
            MemoryStream m = new MemoryStream();
            BufferedBinaryWriter w = new BufferedBinaryWriter(m);

            RecordHeader rh = new RecordHeader(this.TagCode, GetSizeOf(), true);
            rh.WriteTo(w);
			
            w.Write(this.shapeId);
            rect.WriteTo(w);
            shape.WriteTo(w);

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
			writer.WriteAttributeString("CharacterId", CharacterId.ToString());
			this.rect.Serialize(writer);
			this.shape.Serialize(writer);
		}

		#endregion
	}
}
