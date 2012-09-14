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
using System.Text;
using System.Xml;

using SwfDotNet.IO.Utils;

namespace SwfDotNet.IO.Tags.Types
{
    /// <summary>
    /// ISizeStruct
    /// </summary>
    public interface ISizeStruct
    {
        /// <summary>
        /// Gets the size of the binary.
        /// </summary>
        /// <value>The size of the binary.</value>
        long BinarySize
        {
            get;
        }

        /// <summary>
        /// Sets the start point.
        /// </summary>
        void SetStartPoint(BufferedBinaryReader reader);
        
        /// <summary>
        /// Sets the end point.
        /// </summary>
        void SetEndPoint(BufferedBinaryReader reader);

        /// <summary>
        /// Serializes the size of the binary.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        void SerializeBinarySize(XmlWriter xmlWriter);
    }

    /// <summary>
    /// SizeStruct
    /// </summary>
    public class SizeStruct: ISizeStruct
    {
        private long start = 0;
        private long size = 0;

        /// <summary>
        /// Gets the size of the binary.
        /// </summary>
        /// <value>The size of the binary.</value>
        public long BinarySize
        {
            get { return this.size; }
        }

        /// <summary>
        /// Sets the start point.
        /// </summary>
        public void SetStartPoint(BufferedBinaryReader reader)
        {
            start = reader.BaseStream.Position;
        }

        /// <summary>
        /// Sets the end point.
        /// </summary>
        public void SetEndPoint(BufferedBinaryReader reader)
        {
            long end = reader.BaseStream.Position;
            size = end - start;
        }

        /// <summary>
        /// Serializes the size of the binary.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        public void SerializeBinarySize(XmlWriter xmlWriter)
        {
            xmlWriter.WriteAttributeString("BinarySize", this.BinarySize.ToString());
        }
    }
}
