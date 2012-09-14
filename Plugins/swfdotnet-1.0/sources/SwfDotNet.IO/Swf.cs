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

using SwfDotNet.IO.Tags;
using SwfDotNet.IO.Tags.Types;

namespace SwfDotNet.IO 
{
	/// <summary>
	/// The Swf class is basically a data structure for swf data, 
	/// containing header informations and a collection of swf tags.
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	/// <example>
    /// <p>
	/// Follow this example to create a new Swf objet :
	/// <code lang="C#">
	/// // Create a new swf
	/// Swf swf = new Swf();
	/// swf.Size = new Rect(0, 0, 400 * 20, 400 * 20); // Size in twips
	/// swf.Version = 7; // You can specify the swf version
	/// swf.Add(new ShowFrameTag());
	/// 
	/// // Save it in a file
	/// SwfWriter swfWriter = new SwfWriter("myfile.swf");
	/// swfWriter.Write(swf);
	/// swfWriter.Close();
	/// </code>
    /// </p>
    /// </example>
	public class Swf: IBaseTagContainer
	{
		#region Members

		/// <summary>
		/// An array that contains BaseTag objects.
		/// </summary>
		private BaseTagCollection tagList;
		
		/// <summary>
		/// The total number of bytecode blocks within all tags.
		/// </summary>
		private int _actionCount;	
		
		/// <summary>
		/// Swf header.
		/// </summary>
		private SwfHeader header;

        /// <summary>
        /// Swf define tags dictionnary
        /// </summary>
        private Dictionary dictionnary;

		#endregion

        #region Statics members

        /// <summary>
        /// Flash movie mime-type
        /// </summary>
        public static readonly string MIME_TYPE = "application/x-shockwave-flash";

        #endregion

        #region Ctor & Init

        /// <summary>
		/// Creates a new <see cref="Swf"/> instance.
		/// </summary>
		public Swf()
		{
			this.header = new SwfHeader();
			this.tagList = new BaseTagCollection();
			this.tagList.TagAdded += new TagAddedEvent(tagList_TagAdded);
			Init();
		}

		/// <summary>
		/// Creates a new <see cref="Swf"/> instance.
		/// </summary>
		/// <param name="header">Header.</param>
		public Swf(SwfHeader header)
		{
			this.header = header;
			this.tagList = new BaseTagCollection();
			this.tagList.TagAdded += new TagAddedEvent(tagList_TagAdded);
			Init();
		}

		/// <summary>
		/// Creates a new <see cref="Swf"/> instance.
		/// </summary>
		/// <param name="header">Swf Header.</param>
		/// <param name="tagList">Swf Tag list sequence.</param>
		public Swf(SwfHeader header, BaseTagCollection tagList) 
		{
			this.header = header;
			this.tagList = tagList;
			Init();
            
			// count actions
			_actionCount = 0;
			foreach (BaseTag b in tagList) 
			{
				_actionCount += b.ActionRecCount;
				
                if (b is DefineTag)
                    dictionnary.Add(b as DefineTag);
			}
		}

		/// <summary>
		/// Inits this instance.
		/// </summary>
		protected void Init()
		{
			dictionnary = new Dictionary();
		}

		#endregion

        #region Properties

		/// <summary>
		/// Gets the tags.
		/// </summary>
		public BaseTagCollection Tags
		{
			get { return this.tagList; }
		}

		/// <summary>
		/// Gets or sets the size.
		/// This property is the same as swf.Header.Size, 
		/// used only to simplify the writing process.
		/// </summary>
		public Rect Size
		{
			get { return this.header.Size;  }
			set { this.header.Size = value; }
		}

        /// <summary>
        /// Swf version property.
        /// This property is the same as swf.Header.Version, 
        /// used only to simplify the writing process.
        /// </summary>
        public byte Version 
        {
            get { return header.Version;  }
            set { header.Version = value; }
        }

        /// <summary>
        /// Gets the dictionary.
        /// The dictionary contains all the
        /// <see cref="SwfDotNet.IO.Tags.DefineTag">DefineTag</see>
        /// extended tags.
        /// More over, all the define tags are placed into the
        /// main movie clip. 
        /// </summary>
        public Dictionary Dictionary
        {
            get { return this.dictionnary;  }
        }
		
        /// <summary>
        /// Uncompressed swf byte count.
        /// This value is good only if the method
        /// UpdateData has been called before.
        /// </summary>
        public int ByteCount 
        {	
            get 
            {
                int len = 0;
                foreach (BaseTag tag in tagList) 
                {
                    len += tag.Data.Length;
                }
                int size = 12 + header.Size.GetSizeOf() + len;
                return size;
            }
        }
		
        /// <summary>
        /// Accessor for total count of swf bytecode blocks.
        /// </summary>
        public int ActionCount 
        {
            get { return _actionCount; }
        }

        /// <summary>
        /// Accessor for swf header.
        /// </summary>
        public SwfHeader Header 
        {
            get { return this.header; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Injects a range of tags in the current Swf.
        /// </summary>
        /// <param name="values">The values.</param>
        public void InjectTags(BaseTagCollection values)
        {
            IEnumerator enums = values.GetEnumerator();

            for (int i = 0; enums.MoveNext(); i++)
            {
                BaseTag baseTag = (BaseTag)enums.Current;
                Add(baseTag);
            }
        }

        /// <summary>
        /// Gets a new define character id.
        /// Each Define type tag must have an unique
        /// character Id. Use this method to get a new Id
        /// that is not used for the moment.  
        /// </summary>
        public ushort GetNewDefineId()
        {
            bool available = false;
            ushort i = 0;
            while (!available)
            {
                i++;
                available = true;
                IEnumerator keys = this.dictionnary.Keys.GetEnumerator();
                while (keys.MoveNext())
                {
                    ushort key = (ushort)keys.Current;
                    if (key == i)
                    {
                        available = false;
                        continue;
                    }
                }
            }
            return i;
        }

		/// <summary>
		/// Adds the specified tag to the Swf tag collection.
		/// This methods is the same as swf.Tags.Add method, used
		/// only to simplify the writing process.
		/// </summary>
		/// <param name="tag">Tag.</param>
		public void Add(BaseTag tag)
		{
			this.Tags.Add(tag);
		}

        /// <summary>
        /// Re-calc swf binary data.
        /// </summary>
        public void UpdateData() 
        {	
            IEnumerator tags = tagList.GetEnumerator();
            while (tags.MoveNext())
            {
                BaseTag tag = (BaseTag)tags.Current;
                tag.Resolve(this);
                tag.UpdateData(header.Version);
            }
        }	

		/// <summary>
		/// A new tag was added to the tag collection
		/// </summary>
		/// <param name="tagAdded">Tag added.</param>
		private void tagList_TagAdded(BaseTag tagAdded)
		{
			if (tagAdded is DefineTag)
				dictionnary.Add((DefineTag)tagAdded);

			if (tagAdded is ShowFrameTag)
				this.header.Frames++;
		}

		/// <summary>
		/// Serializes the swf object in a
		/// XmlWriter.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("Swf");
			this.header.Serialize(writer);

            IEnumerator tags = tagList.GetEnumerator();
            while (tags.MoveNext())
            {
                BaseTag tag = (BaseTag)tags.Current;
                tag.Serialize(writer);
            }

			writer.WriteEndElement();
		}

		#endregion
	}
}

