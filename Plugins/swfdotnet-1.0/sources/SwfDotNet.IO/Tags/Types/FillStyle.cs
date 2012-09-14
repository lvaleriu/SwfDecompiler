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
    #region Enum

	/// <summary>
	/// FillStyleType
	/// </summary>
	public enum FillStyleType
	{
        /// <summary>
        /// Solid Fill
        /// </summary>
		SolidFill = 0x00,
		/// <summary>
		/// Linear Gradient Fill
		/// </summary>
        LinearGradientFill = 0x10,
		/// <summary>
		/// Radial Gradient Fill
		/// </summary>
        RadialGradientFill = 0x12,
		/// <summary>
		/// Repeating Bitmap Fill
		/// </summary>
        RepeatingBitmapFill = 0x40,
		/// <summary>
		/// Clipped Bitmap Fill
		/// </summary>
        ClippedBitmapFill = 0x41,
		/// <summary>
		/// Non-Smoothed Repeating Bitmap
		/// </summary>
        NonSmoothedRepeatingBitmap = 0x42,
		/// <summary>
		/// Non-Smoothed Clipped Bitmap
		/// </summary>
        NonSmoothedClippedBitmap = 0x43
	}

    #endregion
	
	/// <summary>
	/// FillStyle is the abstract class for the following classes:
    /// <see cref="SolidFill"/>, <see cref="GradientFill"/> and
    /// <see cref="BitmapFill"/>
	/// </summary>
	public abstract class FillStyle: SizeStruct, ISwfSerializer
	{
		#region Members

        /// <summary>
        /// Fill Style type
        /// </summary>
		protected byte fillStyleType;
        /// <summary>
        /// Solid fill color
        /// </summary>
		protected RGBColor rgbColor;
        /// <summary>
        /// Gradient fill matrix
        /// </summary>
		protected Matrix gradientMatrix;
        /// <summary>
        /// Gradients
        /// </summary>
		protected GradientRecordCollection gradient;
        /// <summary>
        /// Bitmap fill bitmap Id
        /// </summary>
		protected ushort bitmapId;
        /// <summary>
        /// Bitmap fill transform matrix
        /// </summary>
		protected Matrix bitmapMatrix;

		#endregion

		#region Methods

		/// <summary>
		/// Reads the data.
		/// </summary>
		/// <param name="binaryReader">Binary reader.</param>
		/// <param name="shapeType">Shape type.</param>
		public void ReadData(BufferedBinaryReader binaryReader, ShapeType shapeType)
		{
            base.SetStartPoint(binaryReader);

			fillStyleType = binaryReader.ReadByte();
			rgbColor = null;
			gradientMatrix = null;
			bitmapId = 0;
			bitmapMatrix = null;
			gradient = null;

			if (fillStyleType == (byte)FillStyleType.SolidFill)
			{
				if (shapeType == ShapeType.Shape3)
				{
					rgbColor = new RGBA();
					rgbColor.ReadData(binaryReader);
				}
				else if (shapeType == ShapeType.Shape2 || shapeType == ShapeType.Shape)
				{
					rgbColor = new RGB();
					rgbColor.ReadData(binaryReader);
				}
			}

			if (fillStyleType == (byte)FillStyleType.RadialGradientFill ||
				fillStyleType == (byte)FillStyleType.LinearGradientFill)
			{
				gradientMatrix = new Matrix();
				gradientMatrix.ReadData(binaryReader);
				gradient = new GradientRecordCollection();
				gradient.ReadData(binaryReader, shapeType);
			}

			if (fillStyleType == (byte)FillStyleType.RepeatingBitmapFill ||
				fillStyleType == (byte)FillStyleType.ClippedBitmapFill ||
				fillStyleType == (byte)FillStyleType.NonSmoothedClippedBitmap ||
				fillStyleType == (byte)FillStyleType.NonSmoothedRepeatingBitmap)
			{
				bitmapId = binaryReader.ReadUInt16();
				bitmapMatrix = new Matrix();
				bitmapMatrix.ReadData(binaryReader);
			}

            base.SetEndPoint(binaryReader);
		}

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns>size of the structure</returns>
		public int GetSizeOf()
		{
			int res = 1;
            
            if (fillStyleType == (byte)FillStyleType.SolidFill && rgbColor != null)
                res += rgbColor.GetSizeOf();

			if (fillStyleType == (byte)FillStyleType.RadialGradientFill ||
				fillStyleType == (byte)FillStyleType.LinearGradientFill)
			{
				if (gradientMatrix != null)
					res += gradientMatrix.GetSizeOf();
				if (gradient != null)
					res += gradient.GetSizeOf();
			}

			if (fillStyleType == (byte)FillStyleType.RepeatingBitmapFill ||
				fillStyleType == (byte)FillStyleType.ClippedBitmapFill ||
				fillStyleType == (byte)FillStyleType.NonSmoothedClippedBitmap ||
				fillStyleType == (byte)FillStyleType.NonSmoothedRepeatingBitmap)
			{
				res += 2;
				if (bitmapMatrix != null)
					res += bitmapMatrix.GetSizeOf();
			}
			return res;
		}

		/// <summary>
		/// Writes to a binary writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void WriteTo(BufferedBinaryWriter writer)
		{
			writer.Write(this.fillStyleType);
    
            if (fillStyleType == (byte)FillStyleType.SolidFill && rgbColor != null)
                rgbColor.WriteTo(writer);

			if (fillStyleType == (byte)FillStyleType.RadialGradientFill ||
				fillStyleType == (byte)FillStyleType.LinearGradientFill)
			{
                if (gradientMatrix != null)
				    gradientMatrix.WriteTo(writer);
				if (gradient != null)
                    gradient.WriteTo(writer);
			}

			if (fillStyleType == (byte)FillStyleType.RepeatingBitmapFill ||
				fillStyleType == (byte)FillStyleType.ClippedBitmapFill ||
				fillStyleType == (byte)FillStyleType.NonSmoothedClippedBitmap ||
				fillStyleType == (byte)FillStyleType.NonSmoothedRepeatingBitmap)
			{
                writer.Write(this.bitmapId);
				if (this.bitmapMatrix != null)
                    this.bitmapMatrix.WriteTo(writer);
			}
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
    /// GradientFill defines how a color changes across an area to 
    /// be filled with color.
    /// </summary>
    public class GradientFill: FillStyle
    {
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="GradientFill"/> instance.
        /// </summary>
        public GradientFill(FillStyleType fillStyleType)
        {
            this.fillStyleType = (byte)fillStyleType;
        }

        /// <summary>
        /// Creates a new <see cref="GradientFill"/> instance.
        /// </summary>
        /// <param name="fillStyleType">Fill style type.</param>
        /// <param name="gradientTransform">Gradient transform.</param>
        /// <param name="gradients">Gradients.</param>
        public GradientFill(FillStyleType fillStyleType, Matrix gradientTransform, GradientRecordCollection gradients)
        {
            this.gradientMatrix = gradientTransform;
            this.gradient = gradients;
            this.fillStyleType = (byte)fillStyleType;
        }

        /// <summary>
        /// Creates a new <see cref="GradientFill"/> instance.
        /// </summary>
        public GradientFill(byte fillStyleType)
        {
            this.fillStyleType = fillStyleType;
        }

        /// <summary>
        /// Creates a new <see cref="GradientFill"/> instance.
        /// </summary>
        /// <param name="fillStyleType">Fill style type.</param>
        /// <param name="gradientTransform">Gradient transform.</param>
        /// <param name="gradients">Gradients.</param>
        public GradientFill(byte fillStyleType, Matrix gradientTransform, GradientRecordCollection gradients)
        {
            this.gradientMatrix = gradientTransform;
            this.gradient = gradients;
            this.fillStyleType = fillStyleType;
        }

        #endregion

		#region Methods

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("GradientFill");
            base.SerializeBinarySize(writer);
			this.gradientMatrix.Serialize(writer);
			this.gradient.Serialize(writer);
			writer.WriteEndElement();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the gradient transform.
		/// </summary>
		/// <value></value>
		public Matrix GradientTransform
		{
			get { return this.gradientMatrix;  }
			set { this.gradientMatrix = value; }
		}

		/// <summary>
		/// Gets or sets the gradients.
		/// </summary>
		/// <value></value>
		public GradientRecordCollection Gradients
		{
			get { return this.gradient;  }
			set { this.gradient = value; }
		}

		#endregion
    }

    /// <summary>
    /// SolidFill  defines a solid color that is used to fill an 
    /// enclosed area in a shape.
    /// </summary>
    public class SolidFill: FillStyle
    {
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="SolidFill"/> instance.
        /// </summary>
        public SolidFill()
        {
            fillStyleType = (byte)FillStyleType.SolidFill;
        }

        /// <summary>
        /// Creates a new <see cref="SolidFill"/> instance.
        /// </summary>
        /// <param name="fillColor">Color of the fill.</param>
        public SolidFill(RGB fillColor)
        {
            this.rgbColor = fillColor;
            fillStyleType = (byte)FillStyleType.SolidFill;
        }

        /// <summary>
        /// Creates a new <see cref="SolidFill"/> instance.
        /// </summary>
        /// <param name="fillColor">Color of the fill.</param>
        public SolidFill(RGBA fillColor)
        {
            this.rgbColor = fillColor;
            fillStyleType = (byte)FillStyleType.SolidFill;
        }

        #endregion

		#region Methods

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("SolidFill");
            base.SerializeBinarySize(writer);
			rgbColor.Serialize(writer);
			writer.WriteEndElement();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the color of the fill.
		/// </summary>
		/// <value></value>
		public RGBColor FillColor
		{
			get { return this.rgbColor;  }
			set { this.rgbColor = value; }
		}

		#endregion
    }

    /// <summary>
    /// BitmapFill is used to fill a shape with an image. 
    /// </summary>
    /// <remarks>
    /// <p>
    /// Two type of bitmap fill are supported:
    /// <ul>
    /// <li>
    /// <b>Clipped:</b> If the area to be filled is larger than the image 
    /// then the colour at the edge of the image is used to fill the 
    /// remainder of the shape.
    /// </li>
    /// <li>
    /// <b>Tiled:</b> If the area to be filled is larger than the image 
    /// then the image is tiled across the area to be filled.
    /// </li>
    /// </ul>
    /// </p>
    /// <p>
    /// When an image is loaded its width and height default to twips 
    /// rather than pixels. An image 300 x 200 pixels will be displayed as
    /// 300 x 200 twips (15 x 10 pixels). Scaling the image by 20 
    /// (20 twips = 1 pixel) using the <see cref="Matrix"/> object will restore 
    /// it to its original size.
    /// </p>
    /// <p>
    /// The image is drawn with the top left corner placed at the origin 
    /// (0, 0) of the shape being filled. Depending on how the shape is drawn 
    /// the coordinate transform may also need to specify a translation to 
    /// place the image at the desired location.
    /// </p>
    /// <p>
    /// <b>When To Use ?</b>
    /// </p>
    /// <p>
    /// In addition to filling shapes with images that represent textures 
    /// or complex fill patterns the BitmapFill class is required to 
    /// display any image in a Flash file - image can only be displayed 
    /// in Flash inside a shape definition. The classes that define images 
    /// only contain the image data, not how it is displayed. To simply 
    /// display an image, the selected line style of a shape may be set 
    /// to zero so that the image is displayed without a border.
    /// </p>
    /// </remarks>
    /// <example>
    /// <p>
    /// The following code fragment centres the image in the centre of 
    /// the shape - assuming  the shape is drawn with the origin at the 
    /// centre. The Matrix constructor allows the translation and 
    /// scaling transform to be specified in a single step rather than 
    /// compositing the transforms.
    /// <code lang="C#">
    /// Matrix composite = new Matrix(-imageWidth/2, -imageHeight/2, 20.0, 20.0);
    /// fillStyles.Add(new BitmapFill(FillStyleType.ClippedBitmapFill, imageId, composite));
    /// </code>
    /// </p>
    /// </example>
    public class BitmapFill: FillStyle
    {
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="BitmapFill"/> instance.
        /// </summary>
        /// <param name="fillStyleType">Fill style type.</param>
        public BitmapFill(FillStyleType fillStyleType)
        {
            this.fillStyleType = (byte)fillStyleType;
        }

        /// <summary>
        /// Creates a new <see cref="BitmapFill"/> instance.
        /// </summary>
        /// <param name="fillStyleType">Fill style type.</param>
        /// <param name="bitmapId">Bitmap id.</param>
        /// <param name="bitmapTransform">Bitmap transform.</param>
        public BitmapFill(FillStyleType fillStyleType, ushort bitmapId, Matrix bitmapTransform)
        {
            this.fillStyleType = (byte)fillStyleType;
            this.bitmapId = bitmapId;
            this.bitmapMatrix = bitmapTransform;
        }

        /// <summary>
        /// Creates a new <see cref="BitmapFill"/> instance.
        /// </summary>
        /// <param name="fillStyleType">Fill style type.</param>
        public BitmapFill(byte fillStyleType)
        {
            this.fillStyleType = fillStyleType;
        }

        /// <summary>
        /// Creates a new <see cref="BitmapFill"/> instance.
        /// </summary>
        /// <param name="fillStyleType">Fill style type.</param>
        /// <param name="bitmapId">Bitmap id.</param>
        /// <param name="bitmapTransform">Bitmap transform.</param>
        public BitmapFill(byte fillStyleType, ushort bitmapId, Matrix bitmapTransform)
        {
            this.fillStyleType = fillStyleType;
            this.bitmapId = bitmapId;
            this.bitmapMatrix = bitmapTransform;
        }

        #endregion

		#region Methods

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public override void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("BitmapFill");
            base.SerializeBinarySize(writer);
			writer.WriteElementString("BitmapId", this.bitmapId.ToString());
			bitmapMatrix.Serialize(writer);
			writer.WriteEndElement();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the bitmap id.
		/// </summary>
		/// <value></value>
		public ushort BitmapId
		{
			get { return this.bitmapId;  }
			set { this.bitmapId = value; }
		}

		/// <summary>
		/// Gets or sets the bitmap transform.
		/// </summary>
		/// <value></value>
		public Matrix BitmapTransform
		{
			get { return this.bitmapMatrix;  }
			set { this.bitmapMatrix = value; }
		}

		#endregion
    }

	/// <summary>
	/// FillStyleCollection is the typed collection
	/// for the FillStyle extended objects:
	/// <see cref="SolidFill"/>, <see cref="GradientFill"/> and
	/// <see cref="BitmapFill"/>
	/// </summary>
	public class FillStyleCollection: CollectionBase, ISwfSerializer
	{
		#region Ctor

		/// <summary>
		/// Creates a new <see cref="FillStyleCollection"/> instance.
		/// </summary>
		public FillStyleCollection()
		{
		}

        /// <summary>
        /// Creates a new <see cref="FillStyleCollection"/> instance.
        /// </summary>
        /// <param name="fillStyles">Fill styles.</param>
		public FillStyleCollection(FillStyle[] fillStyles)
		{
			if (fillStyles != null)
				AddRange(fillStyles);
		}

		#endregion

		#region Methods

        /// <summary>
        /// Gets the fill style from type.
        /// </summary>
        /// <param name="fillStyleType">Fill style type.</param>
        /// <returns></returns>
        private FillStyle GetFillStyleFromType(byte fillStyleType)
        {
            if (fillStyleType == (byte)FillStyleType.SolidFill)
                return new SolidFill();
            
            if (fillStyleType == (byte)FillStyleType.RadialGradientFill ||
                fillStyleType == (byte)FillStyleType.LinearGradientFill)
            {
                return new GradientFill(fillStyleType); 
            }

            if (fillStyleType == (byte)FillStyleType.RepeatingBitmapFill ||
                fillStyleType == (byte)FillStyleType.ClippedBitmapFill ||
                fillStyleType == (byte)FillStyleType.NonSmoothedClippedBitmap ||
                fillStyleType == (byte)FillStyleType.NonSmoothedRepeatingBitmap)
            {
               return new BitmapFill(fillStyleType);
            }
            return null;
        }

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        /// <param name="shapeType">Shape type.</param>
        public void ReadData(BufferedBinaryReader binaryReader, ShapeType shapeType)
        {
            int count = 0;

            byte fillStyleCount = binaryReader.ReadByte();
            count = System.Convert.ToInt32(fillStyleCount);
            ushort fillStyleCountExtended = 0;
            if (fillStyleCount == 0xFF && (shapeType == ShapeType.Shape2 || shapeType == ShapeType.Shape3))
            {
                fillStyleCountExtended = binaryReader.ReadUInt16();
                count = System.Convert.ToInt32(fillStyleCountExtended);
            }

            if (count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    byte fillStyleType = binaryReader.PeekByte();
                    FillStyle fillStyle = GetFillStyleFromType(fillStyleType);
                    if (fillStyle != null)
                    {
                        fillStyle.ReadData(binaryReader, shapeType);
                        this.Add(fillStyle);
                    }
                }
            }
        }

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns>size of this object</returns>
		public int GetSizeOf()
		{
			int res = 1;
			int count = this.Count;
            if (count >= 0xFF)
                res += 2;

            IEnumerator fillStyles = this.GetEnumerator();
            while (fillStyles.MoveNext())
                res += ((FillStyle)fillStyles.Current).GetSizeOf();
			return res;
		}

		/// <summary>
		/// Writes to a binary writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void WriteTo(BufferedBinaryWriter writer)
		{
			int count = this.Count;
            if (count < 0xFF)
                writer.Write((byte)count);
            else
            {
                writer.Write((byte)(0xFF));
                writer.Write((ushort)count);
            }

            IEnumerator fillStyles = this.GetEnumerator();
            while (fillStyles.MoveNext())
                ((FillStyle)fillStyles.Current).WriteTo(writer);
		}

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("FillStyleArray");
            IEnumerator fillStyles = this.GetEnumerator();
            while (fillStyles.MoveNext())
                ((FillStyle)fillStyles.Current).Serialize(writer);
			writer.WriteEndElement();
		}

		#endregion

		#region Collection methods

		/// <summary>
		/// Adds the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns></returns>
		public FillStyle Add(FillStyle value)
		{
			List.Add(value as object);
			return value;
		}

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="values">Values.</param>
		public void AddRange(FillStyle[] values)
		{
			foreach(FillStyle ip in values)
				Add(ip);
		}

		/// <summary>
		/// Removes the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		public void Remove(FillStyle value)
		{
			if (List.Contains(value))
				List.Remove(value as object);
		}

		/// <summary>
		/// Inserts the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="value">Value.</param>
		public void Insert(int index, FillStyle value)
		{
			List.Insert(index, value as object);
		}

		/// <summary>
		/// Containses the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns></returns>
		public bool Contains(FillStyle value)
		{
			return List.Contains(value as object);
		}

		/// <summary>
		/// Gets or sets the <see cref="LineStyle"/> at the specified index.
		/// </summary>
		/// <value></value>
		public FillStyle this[int index]
		{
			get
			{
				return ((FillStyle)List[index]);
			}
			set
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// Get the index of.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns></returns>
		public int IndexOf(FillStyle value)
		{
			return List.IndexOf(value);
		}

		#endregion
	}
}
