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
	/// DefineMorphShapeTag defines a shape that will morph 
	/// from one form into another.
	/// </summary>
	/// <remarks>
	/// <p>
	/// Only the start and end shapes are defined. The Flash Player 
	/// will perform the interpolation that transforms the shape 
	/// at each staging in the morphing process.
	/// </p>
	/// <p>
	/// Morphing can be applied to any shape, however there are 
	/// a few restrictions:
	/// <ul>
	/// <li>The start and end shapes must have the same number of 
	/// edges (StraightEdgeRecord and CurvedEdgeRecord objects).</li>
	/// <li>The fill style (Solid, Bitmap or Gradient) must be the 
	/// same in the start and end shape.</li>
	/// <li>If a bitmap fill style is used then the same image must 
	/// be used in the start and end shapes.</li>
	/// <li>If a gradient fill style is used then the gradient must 
	/// contain the same number of points in the start and end 
	/// shape.</li>
	/// <li>The start and end shape must contain the same set of 
	/// ShapeStyle objects.</li>
	/// </ul>
	/// </p>
	/// <p>
	/// To perform the morphing of a shape the shape is placed in 
	/// the display list using a PlaceObject2Tag object. The ratio 
	/// attribute in the PlaceObject2Tag object defines the progress 
	/// of the morphing process. The ratio ranges between 0.0 and 
	/// 1.0 where 0 represents the start of the morphing process 
	/// and 1.0, the end.
	/// </p>
	/// <p>
	/// The edges may change their type when a shape is morphed. 
	/// Straight edges can become curves and vice versa.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 3.
	/// </p>
	/// </remarks>
	public class DefineMorphShapeTag : BaseTag, DefineTag 
	{	
        #region Members

		private ushort characterId;
		private Rect startBounds;
		private Rect endBounds;
		private uint offset;
		private MorphFillStyleCollection morphFillStyles;
		private MorphLineStyleCollection morphLineStyles;
		private ShapeRecordCollection startEdges;
		private ShapeRecordCollection endEdges;

        #endregion

        #region Ctor

		/// <summary>
		/// Creates a new <see cref="DefineMorphShapeTag"/> instance.
		/// </summary>
		public DefineMorphShapeTag()
		{
			this._tagCode = (int)TagCodeEnum.DefineMorphShape;
		}

		/// <summary>
		/// Creates a new <see cref="DefineMorphShapeTag"/> instance.
		/// </summary>
		/// <param name="characterId">Character id.</param>
		/// <param name="startBounds">Start bounds.</param>
		/// <param name="endBounds">End bounds.</param>
		/// <param name="offset">Offset.</param>
		/// <param name="morphFillStyles">Morph fill styles.</param>
		/// <param name="morphLineStyles">Morph line styles.</param>
		/// <param name="startEdges">Start edges.</param>
		/// <param name="endEdges">End edges.</param>
		public DefineMorphShapeTag(ushort characterId, Rect startBounds, Rect endBounds, uint offset, 
			MorphFillStyleCollection morphFillStyles, MorphLineStyleCollection morphLineStyles, ShapeRecordCollection startEdges, ShapeRecordCollection endEdges)
		{
			this.characterId = characterId;
			this.startBounds = startBounds;
			this.endBounds = endBounds;
			this.offset = offset;
			this.morphFillStyles = morphFillStyles;
			this.morphLineStyles = morphLineStyles;
			this.startEdges = startEdges;
			this.endEdges = endEdges;

            this._tagCode = (int)TagCodeEnum.DefineMorphShape;
		}

        #endregion

        #region Properties

        /// <summary>
        /// see <see cref="SwfDotNet.IO.Tags.DefineTag"/>
        /// </summary>
        public ushort CharacterId
        {
            get { return this.characterId;  }
            set { this.characterId = value; }
        }

        /// <summary>
        /// Gets or sets the start bounds.
        /// </summary>
        public Rect StartBounds
        {
            get { return this.startBounds;  }
            set { this.startBounds = value; }
        }

        /// <summary>
        /// Gets or sets the end bounds.
        /// </summary>
        public Rect EndBounds
        {
            get { return this.endBounds;  }
            set { this.endBounds = value; }
        }

        /// <summary>
        /// Gets or sets the morph fill styles.
        /// </summary>
        public MorphFillStyleCollection MorphFillStyles
        {
            get { return this.morphFillStyles;  }
            set { this.morphFillStyles = value; }
        }

        /// <summary>
        /// Gets or sets the morph line styles.
        /// </summary>
        public MorphLineStyleCollection MorphLineStyles
        {
            get { return this.morphLineStyles;  }
            set { this.morphLineStyles = value; }
        }

        /// <summary>
        /// Gets or sets the start edges.
        /// </summary>
        public ShapeRecordCollection StartEdges
        {
            get { return this.startEdges;  }
            set { this.startEdges = value; }
        }

        /// <summary>
        /// Gets or sets the end edges.
        /// </summary>
        public ShapeRecordCollection EndEdges
        {
            get { return this.endEdges;  }
            set { this.endEdges = value; }
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
            binaryReader.SynchBits();
            
            startBounds = new Rect();
            startBounds.ReadData(binaryReader);

            binaryReader.SynchBits();
            endBounds = new Rect();
            endBounds.ReadData(binaryReader);
            binaryReader.SynchBits();
            
            offset = binaryReader.ReadUInt32();
			
            morphFillStyles = new MorphFillStyleCollection();
            morphFillStyles.ReadData(binaryReader);
            
            morphLineStyles = new MorphLineStyleCollection();
            morphLineStyles.ReadData(binaryReader);
            
            ShapeWithStyle.NumFillBits = (uint)morphFillStyles.Count;
            ShapeWithStyle.NumLineBits = (uint)morphLineStyles.Count;
			
            startEdges = new ShapeRecordCollection();
            startEdges.ReadData(binaryReader, ShapeType.None);

            ShapeWithStyle.NumFillBits = (uint)morphFillStyles.Count;
            ShapeWithStyle.NumLineBits = (uint)morphLineStyles.Count;

            endEdges = new ShapeRecordCollection();
            endEdges.ReadData(binaryReader, ShapeType.None);
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns>Size of this object</returns>
        protected int GetSizeOf()
        {
            int res = 6;
            if (startBounds != null)
                res += startBounds.GetSizeOf();
            if (endBounds != null)
                res += endBounds.GetSizeOf();
            if (morphFillStyles != null)
                res += morphFillStyles.GetSizeOf();
            if (morphLineStyles != null)
                res += morphLineStyles.GetSizeOf();
            
            ShapeWithStyle.NumFillBits = (uint)morphFillStyles.Count;
            ShapeWithStyle.NumLineBits = (uint)morphLineStyles.Count;

            if (startEdges != null)
                res += startEdges.GetSizeOf();
            if (endEdges != null)
                res += endEdges.GetSizeOf();
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
            w.Write(this.characterId);

            w.SynchBits();
            if (this.startBounds != null)
                this.startBounds.WriteTo(w);
            w.SynchBits();
            if (this.endBounds != null)
                this.endBounds.WriteTo(w);

            w.Write(this.offset);
            if (this.morphFillStyles != null)
                this.morphFillStyles.WriteTo(w);
            if (this.morphLineStyles != null)
                this.morphLineStyles.WriteTo(w);

            ShapeWithStyle.NumFillBits = (uint)morphFillStyles.Count;
            ShapeWithStyle.NumLineBits = (uint)morphLineStyles.Count;

            if (this.startEdges != null)
                this.startEdges.WriteTo(w);
            if (this.endEdges != null)
                this.endEdges.WriteTo(w);

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
            writer.WriteStartElement("DefineMorphShapeTag");
            
            writer.WriteAttributeString("CharacterId", this.characterId.ToString());
            if (this.startBounds != null)
                this.startBounds.Serialize(writer);
            if (this.endBounds != null)
                this.endBounds.Serialize(writer);
            writer.WriteElementString("Offset", this.offset.ToString()); 
            if (this.morphFillStyles != null)
                this.morphFillStyles.Serialize(writer);
            if (this.morphLineStyles != null)
                this.morphLineStyles.Serialize(writer);
            
            ShapeWithStyle.NumFillBits = (uint)morphFillStyles.Count;
            ShapeWithStyle.NumLineBits = (uint)morphLineStyles.Count;

            if (this.startEdges != null)
                this.startEdges.Serialize(writer);
            if (this.endEdges != null)
                this.endEdges.Serialize(writer);

            writer.WriteEndElement();
        }

        #endregion
	}
}
