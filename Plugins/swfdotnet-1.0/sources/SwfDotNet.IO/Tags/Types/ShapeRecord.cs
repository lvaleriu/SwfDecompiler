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
	/// ShapeRecord class
	/// </summary>
	public abstract class ShapeRecord: SizeStruct, ISwfSerializer
	{
        #region Members

        /// <summary>
        /// Type of shape record, stored as a flag
        /// </summary>
		protected bool typeFlag;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="ShapeRecord"/> instance.
        /// </summary>
        protected ShapeRecord()
        {
        }

		/// <summary>
		/// Creates a new <see cref="ShapeRecord"/> instance.
		/// </summary>
		/// <param name="typeFlag">Type flag.</param>
		protected ShapeRecord(bool typeFlag)
		{
			this.typeFlag = typeFlag;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Gets the bit size of.
        /// </summary>
        /// <param name="currentLength">Length of the current.</param>
        /// <returns></returns>
        public virtual int GetBitSizeOf(int currentLength)
		{
			return 1;
		}

        /// <summary>
        /// Writes to a binary writer.
        /// </summary>
        /// <param name="writer">Writer.</param>
		public virtual void WriteTo(BufferedBinaryWriter writer)
		{
			writer.WriteBoolean(typeFlag);
		}

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public virtual void Serialize(XmlWriter writer)
		{
		}

        #endregion
	}

	/// <summary>
	/// NonEdgeRecord class
	/// </summary>
	public abstract class NonEdgeRecord: ShapeRecord
	{	
        #region Protected Ctor

        /// <summary>
        /// Creates a new <see cref="NonEdgeRecord"/> instance.
        /// </summary>
        protected NonEdgeRecord(): base(false)
        {
        }

        #endregion
	}

	/// <summary>
	/// EndShapeRecord defines the end of a shape sequence.
	/// </summary>
	/// <remarks>
	/// <p>
	/// When this object is readed by the Flash Player, it stop to 
	/// draw the current shape records.
	/// </p>
    /// <p>
    /// This tag was introduced in Flash 1.
    /// </p>
	/// </remarks>
	public class EndShapeRecord: NonEdgeRecord
	{
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="EndShapeRecord"/> instance.
        /// </summary>
		public EndShapeRecord(): base()
		{
		}

        #endregion

        #region Methods

        /// <summary>
        /// Gets the bit size of.
        /// </summary>
        /// <param name="currentLength">Length of the current.</param>
        /// <returns></returns>
        public override int GetBitSizeOf(int currentLength)
        {
            return 5 + base.GetBitSizeOf(currentLength);
        }

        /// <summary>
        /// Writes to.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public override void WriteTo(BufferedBinaryWriter writer)
        {
            base.WriteTo(writer);
            writer.WriteUBits(0, 5);
        }

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("EndShapeRecord");			
			writer.WriteEndElement();
		}

        #endregion
	}

	/// <summary>
	/// StyleChangeRecord is used to change the drawing environment 
	/// when a shape is drawn. 
	/// </summary>
	/// <remarks>
    /// <p>
    /// Three operations can be performed:
    /// <ul>
    /// <li>Select a line style or fill style.</li>
    /// <li>Move the current drawing point.</li>
    /// <li>Define a new set of line and fill styles.</li>
    /// </ul>
    /// </p>
    /// <p>
    /// An StyleChangeRecord object can specify one or more of the operations 
    /// rather than specifying them in separate StyleChangeRecord objects - compacting 
    /// the size of the binary data when the object is encoded. Conversely if 
    /// an operation is not defined then the values may be omitted.
    /// </p>
    /// <p>
    /// A new drawing point is specified using the absolute x and y coordinates. 
    /// If an StyleChangeRecord object is the first in a shape then the current 
    /// drawing point is the origin of the shape (0,0).
    /// </p>
    /// <p>
    /// New fill and line styles can be added to the StyleChangeRecord object 
    /// to change the way shapes are drawn.
    /// </p>
    /// <p>
    /// This was introduced in Flash 1.
    /// </p>
    /// </remarks>
    /// <example>
    /// 
    /// </example>
	public class StyleChangeRecord: NonEdgeRecord
	{
        #region Members

        private int moveDeltaX = int.MinValue;
        private int moveDeltaY = int.MinValue;
        private int fillStyle0 = int.MinValue;
        private int fillStyle1 = int.MinValue;
        private int lineStyle = int.MinValue;
        private FillStyleCollection fillStyles = null;
        private LineStyleCollection lineStyles = null;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="StyleChangeRecord"/> instance.
        /// </summary>
        public StyleChangeRecord()
        {    
        }

		/// <summary>
		/// Creates a new <see cref="StyleChangeRecord"/> instance.
		/// </summary>
		/// <param name="moveX">Move X.</param>
		/// <param name="moveY">Move Y.</param>
		public StyleChangeRecord(int moveX, int moveY)
		{    
			this.moveDeltaX = moveX;
			this.moveDeltaY = moveY;
		}

		/// <summary>
		/// Creates a new <see cref="StyleChangeRecord"/> instance.
		/// </summary>
		/// <param name="moveX">Move X.</param>
		/// <param name="moveY">Move Y.</param>
		/// <param name="fillStyle1">Fill style1.</param>
		public StyleChangeRecord(int moveX, int moveY, ushort fillStyle1)
		{    
			this.moveDeltaX = moveX;
			this.moveDeltaY = moveY;
			this.fillStyle1 = fillStyle1;
		}

		/// <summary>
		/// Creates a new <see cref="StyleChangeRecord"/> instance.
		/// </summary>
		/// <param name="lineStyle">Line style.</param>
		/// <param name="fillStyle0">Fill style0.</param>
		/// <param name="fillStyle1">Fill style1.</param>
		public StyleChangeRecord(ushort lineStyle, ushort fillStyle0, ushort fillStyle1)
		{
			this.lineStyle = (int)lineStyle;
			this.fillStyle0 = (int)fillStyle0;
			this.fillStyle1 = (int)fillStyle1;
		}

		/// <summary>
		/// Creates a new <see cref="StyleChangeRecord"/> instance.
		/// </summary>
		/// <param name="fillStyles">Fill styles.</param>
		/// <param name="lineStyles">Line styles.</param>
		public StyleChangeRecord(FillStyleCollection fillStyles, LineStyleCollection lineStyles)
		{
			this.fillStyles = fillStyles;
			this.lineStyles = lineStyles;
		}

        /// <summary>
        /// Creates a new <see cref="StyleChangeRecord"/> instance.
        /// </summary>
        /// <param name="lineStyle">Line style.</param>
        /// <param name="fillStyle0">Fill style0.</param>
        /// <param name="fillStyle1">Fill style1.</param>
        /// <param name="moveX">Move X.</param>
        /// <param name="moveY">Move Y.</param>
        public StyleChangeRecord(ushort lineStyle, ushort fillStyle0, 
            ushort fillStyle1, int moveX, int moveY)
        {
            this.lineStyle = (int)lineStyle;
            this.fillStyle0 = (int)fillStyle0;
            this.fillStyle1 = (int)fillStyle1;
            this.moveDeltaX = moveX;
            this.moveDeltaY = moveY;
        }

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets or sets the fill styles.
        /// </summary>
        /// <value></value>
        public FillStyleCollection FillStyles
        {
            get { return this.fillStyles;  }
            set { this.fillStyles = value; }
        }

        /// <summary>
        /// Gets or sets the line styles.
        /// </summary>
        /// <value></value>
        public LineStyleCollection LineStyles
        {
            get { return this.lineStyles;  }
            set { this.lineStyles = value; }
        }

        /// <summary>
        /// Gets or sets the line style.
        /// </summary>
        /// <value></value>
        public int LineStyle
        {
            get { return this.lineStyle;  }
            set { this.lineStyle = value; }
        }

        /// <summary>
        /// Gets or sets the fill style1.
        /// </summary>
        /// <value></value>
        public int FillStyle1
        {
            get { return this.fillStyle1;  }
            set { this.fillStyle1 = value; }
        }

        /// <summary>
        /// Gets or sets the fill style0.
        /// </summary>
        /// <value></value>
        public int FillStyle0
        {
            get { return this.fillStyle0;  }
            set { this.fillStyle0 = value; }
        }

        /// <summary>
        /// Gets or sets the move delta X.
        /// </summary>
        /// <value></value>
        public int MoveDeltaX
        {
            get { return this.moveDeltaX;  }
            set { this.moveDeltaX = value; }
        }

        /// <summary>
        /// Gets or sets the move delta Y.
        /// </summary>
        /// <value></value>
        public int MoveDeltaY
        {
            get { return this.moveDeltaY;  }
            set { this.moveDeltaY = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        /// <param name="flags">Flags.</param>
        /// <param name="numFillBits">Num fill bits.</param>
        /// <param name="numLineBits">Num line bits.</param>
        /// <param name="shapeType">Shape type.</param>
        public void ReadData(BufferedBinaryReader binaryReader, byte flags,
            ref byte numFillBits, ref byte numLineBits, ShapeType shapeType)
        {
            base.SetStartPoint(binaryReader);
            bool stateNewStyle = ((flags & 0x10) != 0);
            bool stateLineStyle = ((flags & 0x08) != 0);
            bool stateFillStyle1 = ((flags & 0x04) != 0);
            bool stateFillStyle0 = ((flags & 0x02) != 0);
            bool stateMoveTo = ((flags & 0x01) != 0);
                        
            if  (stateMoveTo)
            {
                uint bits = binaryReader.ReadUBits(5);
                moveDeltaX = binaryReader.ReadSBits(bits);
                moveDeltaY = binaryReader.ReadSBits(bits);
            }
            if (stateFillStyle0)
            {
                fillStyle0 = (int)binaryReader.ReadUBits(numFillBits);
            }
            if (stateFillStyle1)
            {
                fillStyle1 = (int)binaryReader.ReadUBits(numFillBits);
            }
            if (stateLineStyle)
            {
                lineStyle = (int)binaryReader.ReadUBits(numLineBits);
            }
                        
            fillStyles = null;
            lineStyles = null;
                        
            if (stateNewStyle)
            {
                fillStyles = new FillStyleCollection();
                fillStyles.ReadData(binaryReader, shapeType);
                lineStyles = new LineStyleCollection();
                lineStyles.ReadData(binaryReader, shapeType);
                            
                numFillBits = (byte)binaryReader.ReadUBits(4);
                numLineBits = (byte)binaryReader.ReadUBits(4);
            }
            base.SetEndPoint(binaryReader);
        }

        /// <summary>
        /// see <see cref="ShapeRecord.GetBitSizeOf"/>
        /// </summary>
        public override int GetBitSizeOf(int currentLength)
        {
            int res = base.GetBitSizeOf(currentLength);
            res += 5;
            
            if (HasMoveTo())
            {
                uint moveBitsNum = GetMoveNumBits();
                res += Convert.ToInt32(5 + (2 * moveBitsNum));
            }
            if (HasFillStyle0())
                res += (int)ShapeWithStyle.NumFillBits;
            if (HasFillStyle1())
                res += (int)ShapeWithStyle.NumFillBits;
            if (HasLineStyle())
                res += (int)ShapeWithStyle.NumLineBits;
            if (HasNewStyle())
            {
                if ((res + currentLength) % 8 != 0)
                    res += 8 - ((res + currentLength) % 8);
                ShapeWithStyle.NumFillBits = BufferedBinaryWriter.GetNumBits((uint)fillStyles.Count);
                ShapeWithStyle.NumLineBits = BufferedBinaryWriter.GetNumBits((uint)lineStyles.Count);
                res += fillStyles.GetSizeOf() * 8;
                res += lineStyles.GetSizeOf() * 8;
                res += 8;
            }
            return res;
        }

        /// <summary>
        /// Determines whether [has new style].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has new style]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasNewStyle()
        {
            return (lineStyles != null && lineStyles.Count > 0) || (fillStyles != null && fillStyles.Count > 0);
        }

        /// <summary>
        /// Determines whether [has line style].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has line style]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasLineStyle()
        {
            return lineStyle != int.MinValue;
        }

        /// <summary>
        /// Determines whether [has fill style1].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has fill style1]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasFillStyle1()
        {
            return fillStyle1 != int.MinValue;
        }

        /// <summary>
        /// Determines whether [has fill style0].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has fill style0]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasFillStyle0()
        {
            return fillStyle0 != int.MinValue;
        }

        /// <summary>
        /// Determines whether [has move to].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has move to]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasMoveTo()
        {
            return moveDeltaX != int.MinValue && moveDeltaY != int.MinValue;
        }
        
        /// <summary>
        /// Writes to.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public override void WriteTo(BufferedBinaryWriter writer)
        {
            base.WriteTo(writer);
            bool stateNewStyle = HasNewStyle();
            bool stateLineStyle = HasLineStyle();
            bool stateFillStyle0 = HasFillStyle0();
            bool stateFillStyle1 = HasFillStyle1();
            bool stateMoveTo = HasMoveTo();

            writer.WriteBoolean(stateNewStyle);
            writer.WriteBoolean(stateLineStyle);
            writer.WriteBoolean(stateFillStyle1);
            writer.WriteBoolean(stateFillStyle0);
            writer.WriteBoolean(stateMoveTo);

            if (stateMoveTo)
            {
                uint moveBitsNum = GetMoveNumBits();
                writer.WriteUBits(moveBitsNum, 5);
                writer.WriteSBits(moveDeltaX, moveBitsNum);
                writer.WriteSBits(moveDeltaY, moveBitsNum);
            }

            if (stateFillStyle0)
            {
                writer.WriteUBits((uint)fillStyle0, ShapeWithStyle.NumFillBits);
            }
            if (stateFillStyle1)
            {
                writer.WriteUBits((uint)fillStyle1, ShapeWithStyle.NumFillBits);
            }
            if (stateLineStyle)
            {
                writer.WriteUBits((uint)lineStyle, ShapeWithStyle.NumLineBits);
            }

            if (stateNewStyle)
            {
                fillStyles.WriteTo(writer);
                lineStyles.WriteTo(writer);
                ShapeWithStyle.NumFillBits = BufferedBinaryWriter.GetNumBits((uint)fillStyles.Count);
                ShapeWithStyle.NumLineBits = BufferedBinaryWriter.GetNumBits((uint)lineStyles.Count);
                writer.WriteUBits(ShapeWithStyle.NumFillBits, 4);
                writer.WriteUBits(ShapeWithStyle.NumLineBits, 4);
            }
        }

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("StyleChangeRecord");
            base.SerializeBinarySize(writer);
            writer.WriteElementString("StateNewStyle", HasNewStyle().ToString());;
			writer.WriteElementString("StateLineStyle", HasLineStyle().ToString());
			writer.WriteElementString("StateFillStyle1", HasFillStyle1().ToString());
			writer.WriteElementString("StateFillStyle0", HasFillStyle0().ToString());
			writer.WriteElementString("StateMoveTo", HasMoveTo().ToString());
			if (HasMoveTo())
			{
				writer.WriteElementString("MoveDeltaX", moveDeltaX.ToString());
				writer.WriteElementString("MoveDeltaY", moveDeltaY.ToString());
			}
			if (HasFillStyle0())
			{
				writer.WriteElementString("FillStyle0", fillStyle0.ToString());
			}
			if (HasFillStyle1())
			{
				writer.WriteElementString("FillStyle1", fillStyle1.ToString());
			}
			if (HasLineStyle())
			{
				writer.WriteElementString("LineStyle", lineStyle.ToString());
			}
			if (HasNewStyle())
			{
				fillStyles.Serialize(writer);
				lineStyles.Serialize(writer);
				ShapeWithStyle.NumFillBits = BufferedBinaryWriter.GetNumBits((uint)fillStyles.Count);
				ShapeWithStyle.NumLineBits = BufferedBinaryWriter.GetNumBits((uint)lineStyles.Count);
				writer.WriteElementString("NumFillBits", ShapeWithStyle.NumFillBits.ToString());
				writer.WriteElementString("NumLineBits", ShapeWithStyle.NumLineBits.ToString());
			}
			writer.WriteEndElement();
		}


        /// <summary>
        /// Gets the move num bits.
        /// </summary>
        /// <returns></returns>
        private uint GetMoveNumBits()
        {
            uint res = 0;
            uint tmp = 0;
            tmp = BufferedBinaryWriter.GetNumBits(moveDeltaX);
            if (tmp > res)
                res = tmp;
            tmp = BufferedBinaryWriter.GetNumBits(moveDeltaY);
            if (tmp > res)
                res = tmp;
            return res;
        }

        #endregion
	}

	/// <summary>
	/// EdgeRecord
	/// </summary>
	public abstract class EdgeRecord: ShapeRecord
	{ 	
        #region Members

        /// <summary>
        /// Flag to know if the edge record is a straight record
        /// </summary>
		protected bool straightFlag;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="EdgeRecord"/> instance.
        /// </summary>
        protected EdgeRecord(): base(true)
        {
        }

		/// <summary>
		/// Creates a new <see cref="EdgeRecord"/> instance.
		/// </summary>
		/// <param name="straightFlag">Straight flag.</param>
		protected EdgeRecord(bool straightFlag): base(true)
		{
			this.straightFlag = straightFlag;
		}

        #endregion

        #region Methods

        /// <summary>
        /// see <see cref="ShapeRecord.GetBitSizeOf"/>
        /// </summary>
        public override int GetBitSizeOf(int currentLength)
        {
            return 1 + base.GetBitSizeOf(currentLength);
        }

        /// <summary>
        /// Writes to.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public override void WriteTo(BufferedBinaryWriter writer)
        {
            base.WriteTo(writer);
            writer.WriteBoolean(this.straightFlag);
        }

        #endregion
	}

	/// <summary>
	/// StraightEdgeRecord defines a straight line.
	/// </summary>
	/// <remarks>
	/// <p>
	/// The line is drawn from the current drawing point to the end 
	/// point specified in the StraightEdgeRecord object which is specified 
	/// relative to the current drawing point. Once the line is drawn, 
	/// the end of the line is now the current drawing point.
	/// </p>
	/// <p>
	/// The relative coordinates are specified in twips 
	/// (where 20 twips = 1 pixel) and must be in the range -65536..65535.
	/// </p>
	/// <p>
	/// Lines are drawn with rounded corners and line ends. Different 
	/// join and line end styles can be created by drawing line segments 
	/// as a sequence of filled shapes. With 1 twip equal to 1/20th 
	/// of a pixel this technique can easily be used to draw the 
	/// narrowest of visible lines.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 1.
	/// </p>
	/// </remarks>
	public class StraightEdgeRecord: EdgeRecord
	{
        #region Members

		private int deltaX;
        private int deltaY;
        
        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="StraightEdgeRecord"/> instance.
        /// </summary>
        public StraightEdgeRecord(): base(true)
        {
        }

		/// <summary>
		/// Creates a new <see cref="StraightEdgeRecord"/> instance.
		/// </summary>
		/// <param name="deltaX">Delta X.</param>
		/// <param name="deltaY">Delta Y.</param>
		public StraightEdgeRecord(int deltaX, int deltaY): base(true)
		{
			this.deltaX = deltaX;
			this.deltaY = deltaY;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the delta Y.
        /// That's y-coordinate of the end point of the line, relative 
        /// to the current drawing point.
        /// </summary>
        public int DeltaY
        {
            get { return this.deltaY;  }
            set { this.deltaY = value; }
        }

        /// <summary>
        /// Gets or sets the delta X.
        /// That's x-coordinate of the end point of the line, relative 
        /// to the current drawing point.
        /// </summary>
        public int DeltaX
        {
            get { return this.deltaX;  }
            set { this.deltaX = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        /// <param name="flags">Flags.</param>
        public void ReadData(BufferedBinaryReader binaryReader, byte flags)
        {
            base.SetStartPoint(binaryReader);
            byte numBits = (byte)(flags & 0x0F);
            bool generalLineFLag = binaryReader.ReadBoolean();
            deltaX = 0;
            deltaY = 0;

            if (generalLineFLag)
            {
                deltaX = binaryReader.ReadSBits((uint)(numBits + 2));
                deltaY = binaryReader.ReadSBits((uint)(numBits + 2));
            }
            else
            {
                bool vertLineFlag = binaryReader.ReadBoolean();
                if (!vertLineFlag)
                    deltaX = binaryReader.ReadSBits((uint)(numBits + 2));
                else
                    deltaY = binaryReader.ReadSBits((uint)(numBits + 2));
            }
            base.SetEndPoint(binaryReader);
        }

        /// <summary>
        /// Gets the num bits.
        /// </summary>
        /// <returns></returns>
        private uint GetNumBits()
        {
            uint res = 0;
            uint tmp = 0;
            tmp = BufferedBinaryWriter.GetNumBits(deltaX);
            if (tmp > res)
                res = tmp;
            tmp = BufferedBinaryWriter.GetNumBits(deltaY);
            if (tmp > res)
                res = tmp;
            return res;
        }

        /// <summary>
        /// Determines whether [has general line].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has general line]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasGeneralLine()
        {
            return deltaX != 0 && deltaY != 0;
        }
    
        /// <summary>
        /// Determines whether [has vertical line].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has vertical line]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasVerticalLine()
        {
            return deltaX == 0;
        }

        /// <summary>
        /// see <see cref="ShapeRecord.GetBitSizeOf"/>
        /// </summary>
        public override int GetBitSizeOf(int currentLength)
        {
            int res = base.GetBitSizeOf(currentLength);
            res += 5; //numbits + generalLineFlag

            uint numBits = GetNumBits();
            if (HasGeneralLine())
            {
                res += Convert.ToInt32(numBits * 2);
            }
            else
            {
                res++;
                res += Convert.ToInt32(numBits);
            }
            return res;
        }

        /// <summary>
        /// Writes to.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public override void WriteTo(BufferedBinaryWriter writer)
        {
            base.WriteTo(writer);
            uint numBits = GetNumBits();
            writer.WriteUBits(numBits - 2, 4);

			bool generalLineFlag = HasGeneralLine();
            writer.WriteBoolean(generalLineFlag);

            if (generalLineFlag)
            {
                writer.WriteSBits(deltaX, numBits);
                writer.WriteSBits(deltaY, numBits);
            }
            else
            {
				bool vertLineFlag = HasVerticalLine();
                writer.WriteBoolean(vertLineFlag);
                if (!vertLineFlag)
                    writer.WriteSBits(deltaX, numBits);
                else
                    writer.WriteSBits(deltaY, numBits);
            }
        }

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("StraightEdgeRecord");
            base.SerializeBinarySize(writer);
			bool generalLineFlag = HasGeneralLine();
			if (generalLineFlag)
			{
				writer.WriteElementString("DeltaX", deltaX.ToString());
				writer.WriteElementString("DeltaY", deltaY.ToString());
			}
			else
			{
				bool vertLineFlag = HasVerticalLine();
				if (!vertLineFlag)
					writer.WriteElementString("DeltaX", deltaX.ToString());
				else
					writer.WriteElementString("DeltaY", deltaY.ToString());
			}
			writer.WriteEndElement();
		}

        #endregion
	}

	/// <summary>
	/// CurvedEdgeRecord is used to define a curve. 
	/// Curved lines are constructed using a Quadratic Bezier curve.
	/// </summary>
	/// <remarks>
	/// <p>
	/// The curve is specified using two points relative to the current 
	/// drawing position, an off-curve control point and an on-curve anchor 
	/// point which defines the end-point of the curve.
	/// </p>
	/// <p>
	/// To define a curve the points are defined as pairs of relative coordinates. 
	/// The control point is specified relative to the current drawing point 
	/// and the anchor point is specified relative to the control point. 
	/// Once the line is drawn, the anchor point becomes the current drawing 
	/// point.
	/// </p>
	/// <p>
	/// The relative coordinates are specified in twips (where 20 twips = 1 pixel) 
	/// and must be in the range -65536..65535.
	/// </p>
	/// <p>
	/// The CurvedEdge record was introduced in Flash 1.
	/// </p>
	/// </remarks>
	public class CurvedEdgeRecord: EdgeRecord
	{
        #region Members

        private int controlDeltaX;
        private int controlDeltaY;
        private int anchorDeltaX;
        private int anchorDeltaY;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="CurvedEdgeRecord"/> instance.
        /// </summary>
        public CurvedEdgeRecord(): base(false)
        {
        }
		
        /// <summary>
        /// Creates a new <see cref="CurvedEdgeRecord"/> instance.
        /// </summary>
        /// <param name="controlDeltaX">The x-coordinate of the control point relative to the current drawing point.</param>
        /// <param name="controlDeltaY">The y-coordinate of the control point relative to the current drawing point.</param>
        /// <param name="anchorDeltaX">The x-coordinate of the anchor point relative to the control point.</param>
        /// <param name="anchorDeltaY">The y-coordinate of the anchor point relative to the control point.</param>
		public CurvedEdgeRecord(int controlDeltaX, 
            int controlDeltaY, int anchorDeltaX, int anchorDeltaY)
			: base(false)
		{
            this.controlDeltaX = controlDeltaX;
            this.controlDeltaY = controlDeltaY;
            this.anchorDeltaX = anchorDeltaX;
            this.anchorDeltaY = anchorDeltaY;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the anchor delta Y.
        /// This is the y-coordinate of the anchor point relative 
        /// to the control point.
        /// </summary>
        public int AnchorDeltaY
        {
            get { return this.anchorDeltaY;  }
            set { this.anchorDeltaY = value; }
        }

        /// <summary>
        /// Gets or sets the anchor delta X.
        /// This is the x-coordinate of the anchor point relative 
        /// to the control point.
        /// </summary>
        public int AnchorDeltaX
        {
            get { return this.anchorDeltaX;  }
            set { this.anchorDeltaX = value; }
        }

        /// <summary>
        /// Gets or sets the control delta X.
        /// This is the x-coordinate of the control point relative 
        /// to the current drawing point.
        /// </summary>
        public int ControlDeltaX
        {
            get { return this.controlDeltaX;  }
            set { this.controlDeltaX = value; }
        }

        /// <summary>
        /// Gets or sets the control delta Y.
        /// This is the y-coordinate of the control point relative 
        /// to the current drawing point.
        /// </summary>
        public int ControlDeltaY
        {
            get { return this.controlDeltaY;  }
            set { this.controlDeltaY = value; }
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        /// <param name="flags">Flags.</param>
        public void ReadData(BufferedBinaryReader binaryReader, byte flags)
        {
            base.SetStartPoint(binaryReader);
            byte numBits = (byte)(flags & 0x0F);

            controlDeltaX = binaryReader.ReadSBits((uint)(numBits + 2));
            controlDeltaY = binaryReader.ReadSBits((uint)(numBits + 2));
            anchorDeltaX = binaryReader.ReadSBits((uint)(numBits + 2));
            anchorDeltaY = binaryReader.ReadSBits((uint)(numBits + 2));
            base.SetEndPoint(binaryReader);
        }

        /// <summary>
        /// Gets the num bits.
        /// </summary>
        /// <returns></returns>
        private uint GetNumBits()
        {
            uint res = 0;
            uint tmp = 0;

            tmp = BufferedBinaryWriter.GetNumBits(controlDeltaX);
            if (tmp > res)
                res = tmp;
            tmp = BufferedBinaryWriter.GetNumBits(controlDeltaY);
            if (tmp > res)
                res = tmp;
            tmp = BufferedBinaryWriter.GetNumBits(anchorDeltaX);
            if (tmp > res)
                res = tmp;
            tmp = BufferedBinaryWriter.GetNumBits(anchorDeltaY);
            if (tmp > res)
                res = tmp;
            return res;
        }

        /// <summary>
        /// see <see cref="ShapeRecord.GetBitSizeOf"/>
        /// </summary>
        public override int GetBitSizeOf(int currentLength)
        {
            int res = base.GetBitSizeOf(currentLength);
            res += 4;
            res += Convert.ToInt32(GetNumBits() * 4);
            return res;
        }

		/// <summary>
		/// Writes to a binary writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void WriteTo(BufferedBinaryWriter writer)
		{
            base.WriteTo(writer);
            uint numBits = GetNumBits();
            writer.WriteUBits(numBits - 2, 4);
            writer.WriteSBits(controlDeltaX, numBits);
            writer.WriteSBits(controlDeltaY, numBits);
            writer.WriteSBits(anchorDeltaX, numBits);
            writer.WriteSBits(anchorDeltaY, numBits);
		}

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("CurvedEdgeRecord");
            base.SerializeBinarySize(writer);
			writer.WriteElementString("ControlDeltaX", controlDeltaX.ToString());
			writer.WriteElementString("ControlDeltaY", controlDeltaY.ToString());
			writer.WriteElementString("AnchorDeltaX", anchorDeltaX.ToString());
			writer.WriteElementString("AnchorDeltaY", anchorDeltaY.ToString());
			writer.WriteEndElement();
		}

        #endregion
	}
}
