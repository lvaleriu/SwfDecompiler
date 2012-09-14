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
using System.Collections;

using SwfDotNet.IO.Tags;
using SwfDotNet.IO.Utils;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace SwfDotNet.IO 
{
	/// <summary>
	/// SwfWriter provides the way to write a Swf to a file or
	/// a stream.
    /// </summary>
    /// <remarks>
    /// This class writes an swf to a stream using the Write method.
	/// Headers and the tags list of the swf are compiled as
	/// bytecode and writed in the stream.
	/// </remarks>
	/// <example>
	/// <p>
	/// <u>Create a Swf animation and write it in a file</u>
	/// <code lang="C#">
	/// Swf swf = new Swf();
	/// swf.Tags.Add(new SetBackgroundColorTag(255, 0, 0)); //Define background as blue
	/// swf.Tags.Add(new ShowFrameTag());
	/// 
	/// SwfWriter writer = new SwfWriter("myfile.swf");
	/// writer.Write(swf);
	/// writer.Close();
    /// 
	/// </code>
	/// </p>
	/// </example>
	public class SwfWriter 
	{
		#region Members

		/// <summary>
		/// BaseStream where swf gets written to using Write(..)
		/// </summary>
		private Stream baseStream = null;

		/// <summary>
		/// Private file name member
		/// </summary>
		private string fileName = null;

		#endregion

		#region Ctor

		/// <summary>
		/// constructor.
		/// </summary>
		/// <param name ="stream">Stream, the swf shall be written to.</param>
		public SwfWriter(Stream stream) 
		{	
			baseStream = stream;
		}

		/// <summary>
		/// Creates a new <see cref="SwfWriter"/> instance.
		/// </summary>
		/// <param name="fileName">Name of the file where the swf shall be written to</param>
		public SwfWriter(string fileName)
		{
			this.FileName = fileName;
		}

		#endregion
		
		#region Methods

		/// <summary>
		/// Closes the opened writted stream
		/// </summary>
		public void Close()
		{
			if (baseStream != null)
			{
				baseStream.Close();
				baseStream = null;
			}
		}

		/// <summary>
		/// Writes the (compressed or uncompressed) swf data to a stream.
		/// The stream gets flushed and closed afterwards.
		/// </summary>
		/// <param name="swf">Swf</param>
		public void Write(Swf swf) 
		{            
            if (swf == null)
                return;

            // add EndTag is is not the last one
            BaseTag lastTag = swf.Tags.GetLastOne();
            if (lastTag == null || !(lastTag is EndTag))
                swf.Tags.Add(new EndTag());

            // update tag lengths to adapt to bytecode length	
            swf.UpdateData();
            SwfHeader header = swf.Header;

			// ASCII seems to be ok for Flash 5 and 6+ as well	
			BufferedBinaryWriter writer = new BufferedBinaryWriter(baseStream, System.Text.Encoding.GetEncoding("ascii"));	
			BufferedBinaryWriter dataWriter = writer;

			bool isCompressed = (header.Signature[0] == 'C');
	
			if (isCompressed && swf.Version >= 6) 
            {
				// SharpZipLib makes it easy for us, simply 
                // chain a Deflater into the stream
				DeflaterOutputStream def = new DeflaterOutputStream(baseStream);
				dataWriter = new BufferedBinaryWriter(def);	
			}			
			
			// writer header data, always uncompressed
			writer.WriteString(header.Signature, 3);		
			writer.Write(swf.Version);  
            writer.Write(swf.ByteCount);
			writer.Flush();
			
			// write header data pt.2, using either 
            // original stream or deflater stream
			header.Size.WriteTo(dataWriter);
            dataWriter.SynchBits();
			dataWriter.WriteFWord(header.Fps, 8, 8);
			dataWriter.Write(header.Frames);
				
			// write tags data
            IEnumerator tags = swf.Tags.GetEnumerator();
            while (tags.MoveNext())
            {
				BaseTag tagToWrite = (BaseTag)tags.Current;
				dataWriter.Write(tagToWrite.Data);
            }
			
            // flush + close
			dataWriter.Flush();
			dataWriter.Close();	
		}
	
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the stream where the swf 
		/// shall be written to.
		/// </summary>
		public Stream Stream
		{
			get { return this.baseStream;  }
			set { this.baseStream = value; }
		}

		/// <summary>
		/// Gets or sets the name of the file where the swf 
		/// shall be written to.
		/// </summary>
		public string FileName
		{
			get { return this.fileName;  }
			set 
			{ 
				this.fileName = value;
				this.baseStream = new FileStream(fileName, FileMode.Create);
			}
		}	

		#endregion
	}
}
