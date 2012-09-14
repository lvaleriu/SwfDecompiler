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
	/// CXFormWithAlphaData.
	/// </summary>
	public class CXFormWithAlphaData: ISwfSerializer
	{
        #region Members

        private int redMultTerms;
        private int greenMultTerms;
        private int blueMultTerms;
        private int alphaMultTerms;
        private int redAddTerms;
        private int greenAddTerms;
        private int blueAddTerms;
        private int alphaAddTerms;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="CXFormWithAlphaData"/> instance.
        /// </summary>
		public CXFormWithAlphaData()
		{
		}

		/// <summary>
		/// Creates a new <see cref="CXFormWithAlphaData"/> instance.
		/// </summary>
		/// <param name="redMultTerms">Red mult terms.</param>
		/// <param name="greenMultTerms">Green mult terms.</param>
		/// <param name="blueMultTerms">Blue mult terms.</param>
		/// <param name="alphaMultTerms">Alpha mult terms.</param>
		public CXFormWithAlphaData(int redMultTerms, int greenMultTerms, int blueMultTerms, int alphaMultTerms)
		{
			this.redMultTerms = redMultTerms;
			this.greenMultTerms = greenMultTerms;
			this.blueMultTerms = blueMultTerms;
			this.alphaMultTerms = alphaMultTerms;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the red add terms.
        /// </summary>
        /// <value></value>
        public int RedAddTerms
        {
            get { return this.redAddTerms;  }
            set { this.redAddTerms = value; }
        }

        /// <summary>
        /// Gets or sets the green add terms.
        /// </summary>
        /// <value></value>
        public int GreenAddTerms
        {
            get { return this.greenAddTerms;  }
            set { this.greenAddTerms = value; }
        }

        /// <summary>
        /// Gets or sets the blue add terms.
        /// </summary>
        /// <value></value>
        public int BlueAddTerms
        {
            get { return this.blueAddTerms;  }
            set { this.blueAddTerms = value; }
        }

        /// <summary>
        /// Gets or sets the alpha add terms.
        /// </summary>
        /// <value></value>
        public int AlphaAddTerms
        {
            get { return this.alphaAddTerms;  }
            set { this.alphaAddTerms = value; }
        }


        /// <summary>
        /// Gets or sets the red mult terms.
        /// </summary>
        /// <value></value>
        public int RedMultTerms
        {
            get { return this.redMultTerms;  }
            set { this.redMultTerms = value; }
        }

        /// <summary>
        /// Gets or sets the green mult terms.
        /// </summary>
        /// <value></value>
        public int GreenMultTerms
        {
            get { return this.greenMultTerms;  }
            set { this.greenMultTerms = value; }
        }

        /// <summary>
        /// Gets or sets the blue mult terms.
        /// </summary>
        /// <value></value>
        public int BlueMultTerms
        {
            get { return this.blueMultTerms;  }
            set { this.blueMultTerms = value; }
        }

        /// <summary>
        /// Gets or sets the alpha mult terms.
        /// </summary>
        /// <value></value>
        public int AlphaMultTerms
        {
            get { return this.alphaMultTerms;  }
            set { this.alphaMultTerms = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a value indicating whether this instance has add terms.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has add terms; otherwise, <c>false</c>.
        /// </value>
        private bool HasAddTerms
        {
            get
            {
                return redAddTerms != 0 || greenAddTerms != 0 ||
                    blueAddTerms != 0 || alphaAddTerms != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has mult terms.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has mult terms; otherwise, <c>false</c>.
        /// </value>
        private bool HasMultTerms
        {
            get
            {
                return redMultTerms != 0 || greenMultTerms != 0 ||
                    blueMultTerms != 0 || alphaMultTerms != 0;
            }
        }

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        public void ReadData(BufferedBinaryReader binaryReader)
        {
            bool hasAddTerms = binaryReader.ReadBoolean();
            bool hasMultTerms = binaryReader.ReadBoolean();
            uint nBits = binaryReader.ReadUBits(4);

            if (hasMultTerms)
            {
                redMultTerms = binaryReader.ReadSBits(nBits);
                greenMultTerms = binaryReader.ReadSBits(nBits);
                blueMultTerms = binaryReader.ReadSBits(nBits);
                alphaMultTerms = binaryReader.ReadSBits(nBits);
            }

            if (hasAddTerms)
            {
                redAddTerms = binaryReader.ReadSBits(nBits);
                greenAddTerms = binaryReader.ReadSBits(nBits);
                blueAddTerms = binaryReader.ReadSBits(nBits);
                alphaAddTerms = binaryReader.ReadSBits(nBits);
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
			if (HasMultTerms)
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
				tmp = BufferedBinaryWriter.GetNumBits(alphaMultTerms);
				if (tmp > max)
					max = tmp;
			}
			if (HasAddTerms)
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
				tmp = BufferedBinaryWriter.GetNumBits(alphaAddTerms);
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
			int res = 6;
			uint nBits = GetNumBits();
			if (HasMultTerms)
				res += (int)(nBits * 4);
			if (HasAddTerms)
				res += (int)(nBits * 4);
			return (int)Math.Ceiling((double)res / 8.0);
        }

        /// <summary>
        /// Writes to.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public void WriteTo(BufferedBinaryWriter writer)
        {
			writer.WriteBoolean(HasAddTerms);
			writer.WriteBoolean(HasMultTerms);
			uint nBits = GetNumBits();
			writer.WriteUBits(nBits, 4);
			
			if (HasMultTerms)
			{
				writer.WriteSBits(redMultTerms, nBits);
				writer.WriteSBits(greenMultTerms, nBits);
				writer.WriteSBits(blueMultTerms, nBits);
				writer.WriteSBits(alphaMultTerms, nBits);
			}
			if (HasAddTerms)
			{
				writer.WriteSBits(redAddTerms, nBits);
				writer.WriteSBits(greenAddTerms, nBits);
				writer.WriteSBits(blueAddTerms, nBits);
				writer.WriteSBits(alphaAddTerms, nBits);
			}
        }

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("CXFormWithAlphaData");
			writer.WriteAttributeString("HasMultTerms", HasMultTerms.ToString());
			writer.WriteAttributeString("HasAddTerms", HasAddTerms.ToString());
			writer.WriteAttributeString("Nbits", GetNumBits().ToString());
			if (HasMultTerms)
			{
				writer.WriteElementString("RedMultTerms", redMultTerms.ToString());
				writer.WriteElementString("GreenMultTerms", greenMultTerms.ToString());
				writer.WriteElementString("BlueMultTerms", blueMultTerms.ToString());
				writer.WriteElementString("AlphaMultTerms", alphaMultTerms.ToString());
			}
			if (HasAddTerms)
			{
				writer.WriteElementString("RedAddTerms", redAddTerms.ToString());
				writer.WriteElementString("GreenAddTerms", greenAddTerms.ToString());
				writer.WriteElementString("BlueAddTerms", blueAddTerms.ToString());
				writer.WriteElementString("AlphaAddTerms", alphaAddTerms.ToString());
			}
			writer.WriteEndElement();
		}

        #endregion
	}
}
