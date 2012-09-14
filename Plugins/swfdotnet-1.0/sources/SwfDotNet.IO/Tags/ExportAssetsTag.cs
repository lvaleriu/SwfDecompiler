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
	/// ExportAssetsTag  is used to export shapes and other objects 
	/// so they can be used in another Flash file.
	/// </summary>
	/// <remarks>
	/// <p>
	/// Since the identifier for an object is only unique within a given 
	/// Flash file, each object exported must be given a name so it can 
	/// referenced when it is imported.
	/// </p>
	/// <p>
	/// This tag was introduced in Flash 5.
	/// </p>
	/// </remarks>
	public class ExportAssetsTag : BaseTag, DefineTargetTagContainer
    {
        #region Members

		private AssertCollection exportedCharacters;

        #endregion

        #region Ctor
        
        /// <summary>
        /// Creates a new <see cref="ExportAssetsTag"/> instance.
        /// </summary>
        public ExportAssetsTag()
        {
            this._tagCode = (int)TagCodeEnum.ExportAssets;
            exportedCharacters = new AssertCollection();
        }

        /// <summary>
        /// Creates a new <see cref="ExportAssetsTag"/> instance.
        /// </summary>
        /// <param name="exportedCharacters">Exported characters.</param>
		public ExportAssetsTag(Assert[] exportedCharacters) 
		{
			this.exportedCharacters = new AssertCollection();
            this.exportedCharacters.AddRange(exportedCharacters);
			this._tagCode = (int)TagCodeEnum.ExportAssets;
		}

        #endregion

		#region Properties

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

            ushort count = binaryReader.ReadUInt16();
            exportedCharacters.Clear();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Assert exportedCharacter = new Assert();
                    exportedCharacter.ReadData(binaryReader);
                    this.exportedCharacters.Add(exportedCharacter);
                }
            }
        }

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns></returns>
		public int GetSizeOf()
		{
			int lenght = 2;
			if (exportedCharacters != null)
				lenght += Assert.GetSizeOf(exportedCharacters);
			return lenght;
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
				
			RecordHeader rh = new RecordHeader(TagCode, GetSizeOf());
			rh.WriteTo(w);

			if (exportedCharacters != null)
				w.Write((ushort)exportedCharacters.Count);
			else
				w.Write((ushort)0);

			if (exportedCharacters != null)
			{
                IEnumerator assertEnu = exportedCharacters.GetEnumerator();
                while (assertEnu.MoveNext())
                {
                    Assert assert = (Assert)assertEnu.Current;
                    assert.WriteTo(w);
                }					
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
			writer.WriteStartElement("ExportAssetsTag");
			foreach (Assert ass in exportedCharacters)
				ass.Serialize(writer);
			writer.WriteEndElement();
		}
		
        #endregion

        /// <summary>
        /// Targets to.
        /// </summary>
        /// <param name="characterId">The character id.</param>
        /// <param name="history">The history.</param>
        /// <returns></returns>
        public bool TargetTo(ushort characterId, Hashtable history)
        {
            IEnumerator values = this.exportedCharacters.GetEnumerator();
            while (values.MoveNext())
            {
                Assert value = (Assert)values.Current;
                if (value.TargetCharacterId == characterId && history.Contains(value) == false)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Changeds the target.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="newId">The new id.</param>
        /// <param name="history">The history.</param>
        public void ChangedTarget(ushort id, ushort newId, Hashtable history)
        {
            IEnumerator values = this.exportedCharacters.GetEnumerator();
            while (values.MoveNext())
            {
                Assert value = (Assert)values.Current;
                if (value.TargetCharacterId == id && history.Contains(value) == false)
                {
                    value.TargetCharacterId = newId;
                    history.Add(value, newId);
                }
            }
        }
	}
}
