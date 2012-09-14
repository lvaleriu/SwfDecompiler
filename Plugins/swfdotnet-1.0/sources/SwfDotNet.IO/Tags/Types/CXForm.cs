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

namespace SwfDotNet.IO.Tags.Types {

    /// <summary>
    /// The CXForm is used to change the colour of a shape or button without 
    /// changing the values in the original definition of the object.
	/// </summary>
	/// <remarks>
	/// <p>
	/// Two types of transformation are supported: <b>Add</b> and <b>Multiply</b>
	/// </p>
	/// <p>
	/// In Add transformations a value is added to each colour channel:
	/// <code lang="C#">
    /// newRed = red + addRedTerm
    /// newGreen = green + addGreenTerm
    /// newBlue = blue + addBlueTerm
    /// newAlpha = alpha + addAlphaTerm
	/// </code>
	/// </p>
	/// <p>
	/// In Multiply transformations each colour channel is multiplied by 
	/// a given value:
	/// <code lang="C#">
    /// newRed = red * multiplyRedTerm
    /// newGreen = green * multiplyGreenTerm
    /// newBlue = blue * multiplyBlueTerm
    /// newAlpha = alpha * multiplyAlphaTerm
	/// </code>
	/// </p>
	/// <p>
	/// The CXForm was introduced in Flash 1.
	/// </p>
	/// </remarks>
	public class CXForm: ISwfSerializer
	{
        #region Members

        private bool hasAddTerms = false;
        private bool hasMultTerms = false;
        private int redMultTerms = 0;
        private int greenMultTerms = 0;
        private int blueMultTerms = 0;
        private int redAddTerms = 0;
        private int greenAddTerms = 0;
        private int blueAddTerms = 0;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="CXForm"/> instance.
        /// </summary>
		public CXForm()
		{
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the red mult terms.
        /// </summary>
        public int RedMultTerms
        {
            get { return this.redMultTerms;  }
            set { this.redMultTerms = value; }
        }

        /// <summary>
        /// Gets or sets the green mult terms.
        /// </summary>
        public int GreenMultTerms
        {
            get { return this.greenMultTerms;  }
            set { this.greenMultTerms = value; }
        }

        /// <summary>
        /// Gets or sets the blue mult terms.
        /// </summary>
        public int BlueMultTerms
        {
            get { return this.blueMultTerms;  }
            set { this.blueMultTerms = value; }
        }

        /// <summary>
        /// Gets or sets the red add terms.
        /// </summary>
        public int RedAddTerms
        {
            get { return this.redAddTerms;  }
            set { this.redAddTerms = value; }
        }

        /// <summary>
        /// Gets or sets the green add terms.
        /// </summary>
        public int GreenAddTerms
        {
            get { return this.greenAddTerms;  }
            set { this.greenAddTerms = value; }
        }

        /// <summary>
        /// Gets or sets the blue add terms.
        /// </summary>
        public int BlueAddTerms
        {
            get { return this.blueAddTerms;  }
            set { this.blueAddTerms = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        public void ReadData(BufferedBinaryReader binaryReader)
        {
            hasAddTerms = binaryReader.ReadBoolean();
            hasMultTerms = binaryReader.ReadBoolean();
            uint nBits = binaryReader.ReadUBits(4);

            if (hasMultTerms)
            {
                int redMultTerms = binaryReader.ReadSBits(nBits);
                int greenMultTerms = binaryReader.ReadSBits(nBits);
                int blueMultTerms = binaryReader.ReadSBits(nBits);
            }

            if (hasAddTerms)
            {
                int redAddTerms = binaryReader.ReadSBits(nBits);
                int greenAddTerms = binaryReader.ReadSBits(nBits);
                int blueAddTerms = binaryReader.ReadSBits(nBits);
            }
        }

		/// <summary>
		/// Gets the num bits.
		/// </summary>
		/// <returns></returns>
		private uint GetNumBits()
		{
			uint max = 0;
			uint tmp = 0;
			if (hasMultTerms)
			{
				tmp = BufferedBinaryWriter.GetNumBits(redMultTerms);
				if (tmp > max)
					max = tmp;
				tmp = BufferedBinaryWriter.GetNumBits(greenMultTerms);
				if (tmp > max)
					max = tmp;
				tmp = BufferedBinaryWriter.GetNumBits(blueMultTerms);
				if (tmp > max)
					max = tmp;
			}
			if (hasAddTerms)
			{
				tmp = BufferedBinaryWriter.GetNumBits(redAddTerms);
				if (tmp > max)
					max = tmp;
				tmp = BufferedBinaryWriter.GetNumBits(greenAddTerms);
				if (tmp > max)
					max = tmp;
				tmp = BufferedBinaryWriter.GetNumBits(blueAddTerms);
				if (tmp > max)
					max = tmp;
			}
			return max;
		}

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns></returns>
        public int GetSizeOf()
        {
            uint res = 6;
            uint nBits = GetNumBits();
			if (hasMultTerms)
				res += nBits * 3;
			if (hasAddTerms)
				res += nBits * 3;
            return (int)Math.Ceiling((double)res / 8.0);
        }

        /// <summary>
        /// Writes to.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public void WriteTo(BufferedBinaryWriter writer)
        {
            writer.SynchBits();
			writer.WriteBoolean(hasAddTerms);
			writer.WriteBoolean(hasMultTerms);
			uint nBits = GetNumBits();
			writer.WriteUBits(nBits, 4);
			
			if (hasMultTerms)
			{
				writer.WriteSBits(redMultTerms, nBits);
				writer.WriteSBits(greenMultTerms, nBits);
				writer.WriteSBits(blueMultTerms, nBits);
			}
			if (hasAddTerms)
			{
				writer.WriteSBits(redAddTerms, nBits);
				writer.WriteSBits(greenAddTerms, nBits);
				writer.WriteSBits(blueAddTerms, nBits);
			}
            writer.SynchBits();
        }

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("CXForm");
			if (hasMultTerms)
			{
				writer.WriteElementString("RedMultTerms", redMultTerms.ToString());
				writer.WriteElementString("GreenMultTerms", greenMultTerms.ToString());
				writer.WriteElementString("BlueMultTerms", blueMultTerms.ToString());
			}
			if (hasAddTerms)
			{
				writer.WriteElementString("RedAddTerms", redAddTerms.ToString());
				writer.WriteElementString("GreenAddTerms", greenAddTerms.ToString());
				writer.WriteElementString("BlueAddTerms", blueAddTerms.ToString());
			}
			writer.WriteEndElement();
		}

        #endregion
	}
}
