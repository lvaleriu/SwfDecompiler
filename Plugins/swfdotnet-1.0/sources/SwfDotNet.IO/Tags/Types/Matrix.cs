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

namespace SwfDotNet.IO.Tags.Types
{
	/// <summary>
    /// Matrix is used to specify two-dimensional coordinate transforms, 
    /// allowing an object to be scaled, rotated or moved without changing 
    /// the original definition of how the object is drawn.
	/// </summary>
	/// <remarks>
	/// <p>
	/// A two-dimensional transform is defined using a 3x3 matrix and the 
	/// new values for a pair of coordinates (x, y) are calculated using a 
	/// matrix multiplication (see Macromedia specifications).
	/// </p>
	/// <p>
	/// Different transformations such as scaling, rotation, shearing and 
	/// translation can be performed using the above matrix multiplication. 
	/// More complex transformations can be defined by performing successive 
	/// matrix multiplications in a process known as compositing. This allows 
	/// a complex transformations to performed on an object.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 1.
	/// </p>
	/// </remarks>
	/// <example>
	/// <p>
	/// The Matrix provides a set of methods for generating the 
	/// matrices that will perform specific transformations. Methods are 
	/// provided that represent matrices for performing translation, scaling, 
	/// rotation and shearing transformations.
	/// <code lang="C#">
    /// Matrix transform = new Matrix();
    /// transform.Scale(2.0, 2.0); // scale(x,y)
    /// transform.Rotate(30.0);  // rotate(degrees)
    /// transform.Shear(1.2, 0.9);  // shear(x, y)
	/// </code>
    /// </p>
    /// <p>
    /// The composite method can be used to multiply two matrices together to 
    /// create complex transformations though successive compositing steps. 
    /// For example to place a new object on the screen first rotating it by 
    /// 30 degrees and scaling it to twice its original size the required transform 
    /// can be constructed using the following steps:
    /// <code lang="C#">
    /// Matrix transform = new Matrix();
    /// transform.Scale(2.0, 2.0);
    /// transform.Rotate(30.0);
    /// int layer = 1;
    /// ushort identifier = swf.GetNewDefineId();
    /// DefineShapeTag shape = new DefineShapeTag(identifier, ...);
    /// PlaceObject2Tag placeShape = new PlaceObject2Tag(identifier, layer, transform);
    /// </code>
    /// </p>
    /// <p>
    /// Compositing transforms are not commutative, the order in which 
    /// transformations are applied will affect the final result. 
    /// For example consider the following pair if transforms:
    /// <code lang="C#">
    /// Matrix transform = new Matrix();
    /// transform.Scale(2.0, 2.0);
    /// transform.Translate(100, 100);
    /// </code>
    /// The composite transform places an object at the coordinates (100,100) then 
    /// scales it to twice its original size. If the transform was composited in 
    /// the opposite order, then the coordinates for the object's location would 
    /// also be scaled, placing the object at (200,200).
    /// </p>
    /// </example>
	public class Matrix: SizeStruct, ISwfSerializer
	{
		#region Members

        private float[,] matrix = new float[3, 3] { 
                {1.0f, 0.0f, 0.0f}, 
                {0.0f, 1.0f, 0.0f}, 
                {0.0f, 0.0f, 1.0f} 
        };

		#endregion

		#region Ctor

		/// <summary>
		/// Creates a new <see cref="Matrix"/> instance.
		/// </summary>
		public Matrix()
		{
		}

        /// <summary>
        /// Creates a new <see cref="Matrix"/> instance.
        /// </summary>
        /// <param name="x">X.</param>
        /// <param name="y">Y.</param>
        public Matrix(int x, int y)
        {
            float xValue = (float)x;
            float yValue = (float)y;

            matrix[0, 2] = xValue;
            matrix[1, 2] = yValue;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix"/> instance.
        /// </summary>
        /// <param name="x">X.</param>
        /// <param name="y">Y.</param>
        /// <param name="scaleX">Scale X.</param>
        /// <param name="scaleY">Scale Y.</param>
        public Matrix(int x, int y, double scaleX, double scaleY)
        {
            matrix[0, 0] = (float)scaleX;
            matrix[1, 1] = (float)scaleY;
            matrix[0, 2] = (float)x;
            matrix[1, 2] = (float)y;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix"/> instance.
        /// </summary>
        /// <param name="matrix">Matrix.</param>
        public Matrix(float[,] matrix)
        {
            if (matrix == null)
                return;

            for(int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    this.matrix[i, j] = matrix[i, j];
        }

		#endregion

		#region Properties

        /// <summary>
        /// Gets the matrix data.
        /// </summary>
        /// <value></value>
        public float[,] MatrixData
        {
            get { return this.matrix; }
        }

		#endregion

		#region Methods

        /// <summary>
        /// Sets the translation points of the transform.
        /// </summary>
        /// <param name="x">The x-coordinate where the object will be displayed.</param>
        /// <param name="y">The y-coordinate where the object will be displayed.</param>
        public void Translate(int x, int y)
        {
            float[,] m = new float[3, 3] { 
                    {1.0f, 0.0f, (float)x}, 
                    {0.0f, 1.0f, (float)y}, 
                    {0.0f, 0.0f, 1.0f}
                };
            Composite(m);
        }

        /// <summary>
        /// Sets the scaling factor for the transform.
        /// </summary>
        /// <param name="x">Value to scale the object in the x direction.</param>
        /// <param name="y">Value to scale the object in the y direction.</param>
        public void Scale(double x, double y)
        {
            float[,] m = new float[3, 3] { 
                    {(float)x, 0.0f, 0.0f}, 
                    {0.0f, (float)y, 0.0f}, 
                    {0.0f, 0.0f, 1.0f}
                };
            Composite(m);
        }

        /// <summary>
        /// Converts to radians.
        /// </summary>
        /// <param name="angle">Angle.</param>
        /// <returns></returns>
        private double ConvertToRadians(double angle)
        {
            double radians = angle * (Math.PI / 180);
            return radians;
        }

        /// <summary>
        /// Sets the angle which the transform will rotate an object.
        /// </summary>
        /// <param name="angle">Angle value, in degrees, to rotate the object clockwise.</param>
        public void Rotate(double angle)
        {
            float[,] m = new float[3, 3] { 
                    {1.0f, 0.0f, 0.0f}, 
                    {0.0f, 1.0f, 0.0f}, 
                    {0.0f, 0.0f, 1.0f}
                };
		
            m[0, 0] = (float)Math.Cos(ConvertToRadians(angle));
            m[0, 1] = -(float)Math.Sin(ConvertToRadians(angle));
            m[1, 0] = (float)Math.Sin(ConvertToRadians(angle));
            m[1, 1] = (float)Math.Cos(ConvertToRadians(angle));

            Composite(m);
        }

        /// <summary>
        /// Sets the shearing factor for the transform.
        /// </summary>
        /// <param name="x">Value to shear the object in the x direction.</param>
        /// <param name="y">Value to shear the object in the y direction.</param>
        public void Shear(double x, double y)
        {
            float[,] m = new float[3, 3] { 
                    {1.0f, (float)y, 0.0f}, 
                    {(float)x, 1.0f, 0.0f}, 
                    {0.0f, 0.0f, 1.0f}
                };		
            Composite(m);
        }

        /// <summary>
        /// Applies the transformation to the coordinates of a point.
        /// </summary>
        /// <param name="x">X-coordinate of a point.</param>
        /// <param name="y">Y-coordinate of a point.</param>
        /// <returns>An array containing the transformed point.</returns>
        public int[] TransformPoint(int x, int y)
        {
            float[] point = new float[] { (float)x, (float)y, 1.0f };
            int[] result = new int[2];
		
            result[0] = (int)(matrix[0, 0] * point[0] +  matrix[0, 1] * point[1] +  matrix[0, 2] * point[2]);
            result[1] = (int)(matrix[1, 0] * point[0] +  matrix[1, 1] * point[1] +  matrix[1, 2] * point[2]);
		
            return result;
        }

        /// <summary>
        /// Get if the matrix transformation will not change the 
        /// physical appearance or location of a shape.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is unity transform]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUnityTransform()
        {
            return ! (HasScale() || HasRotate() || HasTranslate());
        }
	
        /// <summary>
        /// Determines whether this instance has scale.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance has scale; otherwise, <c>false</c>.
        /// </returns>
        private bool HasScale() 
        { 
            return matrix[0, 0] != 1.0f || matrix[1, 1] != 1.0f;
        }
     
        /// <summary>
        /// Determines whether this instance has rotate.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance has rotate; otherwise, <c>false</c>.
        /// </returns>
        private bool HasRotate() 
        {
            return matrix[1, 0] != 0.0f || matrix[0, 1] != 0.0f;
        } 

        /// <summary>
        /// Determines whether this instance has translate.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance has translate; otherwise, <c>false</c>.
        /// </returns>
        private bool HasTranslate()
        {
            return matrix[0, 2] != 0.0f || matrix[1, 2] != 0.0f;
        }

        /// <summary>
        /// Composites the specified m.
        /// </summary>
        /// <param name="m">Matrix.</param>
        private void Composite(float[,] m)
        {
            matrix[0, 0] = matrix[0, 0] * m[0, 0] + matrix[0, 1] * m[1, 0] + matrix[0, 2] * m[2, 0];
            matrix[0, 1] = matrix[0, 0] * m[0, 1] + matrix[0, 1] * m[1, 1] + matrix[0, 2] * m[2, 1];
            matrix[0, 2] = matrix[0, 0] * m[0, 2] + matrix[0, 1] * m[1, 2] + matrix[0, 2] * m[2, 2];

            matrix[1, 0] = matrix[1, 0] * m[0, 0] + matrix[1, 1] * m[1, 0] + matrix[1, 2] * m[2, 0];
            matrix[1, 1] = matrix[1, 0] * m[0, 1] + matrix[1, 1] * m[1, 1] + matrix[1, 2] * m[2, 1];
            matrix[1, 2] = matrix[1, 0] * m[0, 2] + matrix[1, 1] * m[1, 2] + matrix[1, 2] * m[2, 2];

            matrix[2, 0] = matrix[2, 0] * m[0, 0] + matrix[2, 1] * m[1, 0] + matrix[2, 2] * m[2, 0];
            matrix[2, 1] = matrix[2, 0] * m[0, 1] + matrix[2, 1] * m[1, 1] + matrix[2, 2] * m[2, 1];
            matrix[2, 2] = matrix[2, 0] * m[0, 2] + matrix[2, 1] * m[1, 2] + matrix[2, 2] * m[2, 2];
        }

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        public void ReadData(BufferedBinaryReader binaryReader)
        {
            base.SetStartPoint(binaryReader);
            binaryReader.SynchBits();
            bool hasScale = binaryReader.ReadBoolean();
            if (hasScale)
            {
                uint nScaleBits = binaryReader.ReadUBits(5);
                matrix[0, 0] = binaryReader.ReadFloat(nScaleBits);
                matrix[1, 1] = binaryReader.ReadFloat(nScaleBits);
            }
            bool hasRotate = binaryReader.ReadBoolean();
            if (hasRotate)
            {
                uint nRotateBits = binaryReader.ReadUBits(5);
                matrix[1, 0] = binaryReader.ReadFloat(nRotateBits);
                matrix[0, 1] = binaryReader.ReadFloat(nRotateBits);
            }
            uint nTranslateBits = binaryReader.ReadUBits(5);
            matrix[0, 2] = (float)binaryReader.ReadSBits(nTranslateBits);
            matrix[1, 2] = (float)binaryReader.ReadSBits(nTranslateBits);
            binaryReader.SynchBits();
            base.SetEndPoint(binaryReader);
        }

        /// <summary>
        /// Gets the scale bits num.
        /// </summary>
        /// <returns></returns>
        private uint GetScaleBitsNum()
        {
            uint res = 0;
            uint tmp = 0;
            tmp = BufferedBinaryWriter.GetNumBits(matrix[0, 0]);
            if (tmp > res)
                res = tmp;
            tmp = BufferedBinaryWriter.GetNumBits(matrix[1, 1]);
            if (tmp > res)
                res = tmp;
            return res;
        }

        /// <summary>
        /// Gets the rotate bits num.
        /// </summary>
        /// <returns></returns>
        private uint GetRotateBitsNum()
        {
            uint res = 0;
            uint tmp = 0;
            tmp = BufferedBinaryWriter.GetNumBits(matrix[1, 0]);
            if (tmp > res)
                res = tmp;
            tmp = BufferedBinaryWriter.GetNumBits(matrix[0, 1]);
            if (tmp > res)
                res = tmp;
            return res;
        }

        /// <summary>
        /// Gets the translate bits num.
        /// </summary>
        /// <returns></returns>
        private uint GetTranslateBitsNum()
        {
            uint res = 0;
            if (HasTranslate())
            {
                uint tmp = 0;
                tmp = BufferedBinaryWriter.GetNumBits((int)matrix[0, 2]);
                if (tmp > res)
                    res = tmp;
                tmp = BufferedBinaryWriter.GetNumBits((int)matrix[1, 2]);
                if (tmp > res)
                    res = tmp;
            }
            return res;
        }

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns></returns>
        public int GetSizeOf()
        {
            uint numberOfBits = 7 + (GetTranslateBitsNum() * 2);
	
            if (HasScale())
                numberOfBits += 5 + (GetScaleBitsNum() * 2);
	
            if (HasRotate())
                numberOfBits += 5 + (GetRotateBitsNum() * 2);
	
            numberOfBits += (numberOfBits % 8 > 0) ? 8 - (numberOfBits % 8) : 0;
	
            return Convert.ToInt32(numberOfBits >> 3);
        }

		/// <summary>
		/// Writes to.
		/// </summary>
		/// <param name="binaryWriter">Binary writer.</param>
		public void WriteTo(BufferedBinaryWriter binaryWriter)
		{
            binaryWriter.SynchBits();
            bool hasScale = HasScale();
            bool hasRotate = HasRotate();

            binaryWriter.WriteBoolean(hasScale);
            if (hasScale)
            {
                uint nScaleBits = GetScaleBitsNum();
                binaryWriter.WriteUBits(nScaleBits, 5);
                binaryWriter.WriteFBits(matrix[0, 0], nScaleBits);
                binaryWriter.WriteFBits(matrix[1, 1], nScaleBits);
            }

            binaryWriter.WriteBoolean(hasRotate);
            if (hasRotate)
            {
                uint nRotateBits = GetRotateBitsNum();
                binaryWriter.WriteUBits(nRotateBits, 5);
                binaryWriter.WriteFBits(matrix[1, 0], nRotateBits);
                binaryWriter.WriteFBits(matrix[0, 1], nRotateBits);
            }
            
            uint nTranslateBits = GetTranslateBitsNum();
            binaryWriter.WriteUBits(nTranslateBits, 5);
            binaryWriter.WriteSBits((int)matrix[0, 2], nTranslateBits);
            binaryWriter.WriteSBits((int)matrix[1, 2], nTranslateBits);
            binaryWriter.SynchBits();
        }

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer) 
		{
			writer.WriteStartElement("Matrix");
            base.SerializeBinarySize(writer);
			if (HasScale())
			{
				writer.WriteElementString("ScaleX", matrix[0, 0].ToString());
				writer.WriteElementString("ScaleY", matrix[1, 1].ToString());
			}
			if (HasRotate())
			{
				writer.WriteElementString("RotateX", matrix[1, 0].ToString());
				writer.WriteElementString("RotateY", matrix[0, 1].ToString());
			}
			writer.WriteElementString("TranslateX", matrix[0, 2].ToString());
			writer.WriteElementString("TranslateY", matrix[1, 2].ToString());
			writer.WriteEndElement();
        }

		#endregion
	}
}
