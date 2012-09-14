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
	/// ImportAssetsTag is used to import shapes and other objects from 
	/// another SWF file.
	/// </summary>
	/// <remarks>
	/// <p>
	/// Since the identifier for an object is only unique within a given 
	/// Flash file, imported objects are referenced by a name assigned when 
	/// the object is exported.
	/// </p>
	/// <p>
	/// To provide a degree of security the Flash Player will only import files 
	/// that originate from the same domain as the file that it is currently 
	/// playing. For example if the Flash file being shown was loaded from 
	/// www.mydomain.com/flash.swf then the file contains the exported objects 
	/// must reside somewhere at www.mydomain.com. This prevents a malicious 
	/// Flash file from loading files from an unknown third party.
    /// </p>
    /// <p>
    /// This tag was introduced in Flash 5.
    /// </p>
	/// </remarks>
	/// <example>
	/// <p>
    /// <code lang="C#">
	/// // To export an object from a Flash file:
	/// DefineShapeTag shape = new DefineShapeTag(...);
	/// swf.Add(shape);
	/// swf.Add(new ExportAssetsTag(shape.getIdentifier(), "Shape"));
	/// ...
	/// // The object can then be imported in another movie:
	/// swf.Add(new ImportAssetsTag("exportFile.swf", "Shape"));
	/// </code>
	/// </p>
    /// </example>
	public class ImportAssetsTag : BaseTag 
	{
        #region Members

		private string url;
		private AssertCollection exportedCharacters = null;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="ImportAssetsTag"/> instance.
        /// </summary>
        public ImportAssetsTag()
        {
            this._tagCode = (int)TagCodeEnum.ExportAssets;
            exportedCharacters = new AssertCollection();
        }

        /// <summary>
        /// Creates a new <see cref="ImportAssetsTag"/> instance.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="exportedCharacters">Exported characters.</param>
        public ImportAssetsTag(string url, Assert[] exportedCharacters) 
		{
			this.url = url;
            this.exportedCharacters = new AssertCollection();
            this.exportedCharacters.AddRange(exportedCharacters);
			this._tagCode = (int)TagCodeEnum.ExportAssets;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        public string Url
        {
            get { return this.url;  }
            set { this.url = value; }
        }

        /// <summary>
        /// Gets or sets the exported characters.
        /// </summary>
        public AssertCollection ExportedCharacters
        {
            get { return this.exportedCharacters;  }
        }

        #endregion

        #region Methods
		
        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="version">Version.</param>
        /// <param name="binaryReader">Binary reader.</param>
        public override void ReadData(byte version, BufferedBinaryReader binaryReader)
        {
            RecordHeader rh = new RecordHeader();
            rh.ReadData(binaryReader);

            url = binaryReader.ReadString();
            ushort count = binaryReader.ReadUInt16();
            exportedCharacters.Clear();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Assert exportedCharacter = new Assert();
                    exportedCharacter.ReadData(binaryReader);
                    exportedCharacters.Add(exportedCharacter);
                }
            }
        }

		/// <summary>
		/// see <see cref="SwfDotNet.IO.Tags.BaseTag">base class</see>
		/// </summary>
		public override void UpdateData(byte version) 
		{		
			if (version < 5)
				return;

			MemoryStream m = new MemoryStream();
			BufferedBinaryWriter w = new BufferedBinaryWriter(m);
			
			int lenght = 2 + url.Length;
			if (exportedCharacters != null)
				lenght += Assert.GetSizeOf(exportedCharacters);

			RecordHeader rh = new RecordHeader(TagCode, lenght);
			rh.WriteTo(w);

			w.Write(url);
			w.Write((ushort)exportedCharacters.Count);

            IEnumerator asserts = exportedCharacters.GetEnumerator();
            while (asserts.MoveNext())
            {
                Assert assert = (Assert)asserts.Current;
                assert.WriteTo(w);
            }
			
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
			writer.WriteStartElement("ImportAssetsTag");
			writer.WriteElementString("Url", url);
			foreach (Assert ass in exportedCharacters)
				ass.Serialize(writer);
			writer.WriteEndElement();
		}

        #endregion
	}
}
