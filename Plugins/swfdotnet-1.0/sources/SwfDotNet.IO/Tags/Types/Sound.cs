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
using SwfDotNet.IO.Utils;

namespace SwfDotNet.IO.Tags.Types
{
	/// <summary>
	/// SoundInfo
	/// </summary>
	public class SoundInfo: ISwfSerializer
	{
		#region Members

        private bool syncStop;
        private bool syncNoMultiple;
		private uint inPoint;
		private uint outPoint;
		private ushort loopCount;
        private SoundEnvelopeCollection envelopeRecord = null;

		#endregion

		#region Ctor

        /// <summary>
        /// Creates a new <see cref="SoundInfo"/> instance.
        /// </summary>
		public SoundInfo() 
        { 
            envelopeRecord = new SoundEnvelopeCollection();
        } 

		#endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance has out point.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has out point; otherwise, <c>false</c>.
        /// </value>
        private bool HasOutPoint
        {
            get { return this.outPoint != 0;  }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has in point.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has in point; otherwise, <c>false</c>.
        /// </value>
        private bool HasInPoint
        {
            get { return this.inPoint != 0;  }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has loops.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has loops; otherwise, <c>false</c>.
        /// </value>
        private bool HasLoops
        {
            get { return this.loopCount != 0; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has envelope.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has envelope; otherwise, <c>false</c>.
        /// </value>
        private bool HasEnvelope
        {
            get 
            { 
                return this.envelopeRecord != null && 
                      this.envelopeRecord.Count != 0;  
            }
        }

        /// <summary>
        /// Gets the envelope record.
        /// </summary>
        /// <value></value>
        public SoundEnvelopeCollection EnvelopeRecord
        {
            get { return this.envelopeRecord;  }
        }

        /// <summary>
        /// Gets or sets the out point.
        /// </summary>
        /// <value></value>
        public uint OutPoint
        {
            get { return this.outPoint;  }
            set { this.outPoint = value; }
        }

        /// <summary>
        /// Gets or sets the in point.
        /// </summary>
        /// <value></value>
        public uint InPoint
        {
            get { return this.inPoint;  } 
            set { this.inPoint = value; }
        }

        /// <summary>
        /// Gets or sets the loop count.
        /// </summary>
        /// <value></value>
        public ushort LoopCount
        {
            get { return this.loopCount;  }
            set { this.loopCount = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [sync no multiple].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [sync no multiple]; otherwise, <c>false</c>.
        /// </value>
        public bool SyncNoMultiple
        {
            get { return this.syncNoMultiple;  }
            set { this.syncNoMultiple = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [sync stop].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [sync stop]; otherwise, <c>false</c>.
        /// </value>
        public bool SyncStop
        {
            get { return this.syncStop;  }
            set { this.syncStop = value; }
        }

        #endregion

		#region Methods

        /// <summary>
        /// Gets the size of.
        /// </summary>
        /// <returns>Size of this object.</returns>
        public int GetSizeOf()
        {
            int res = 1;
            if (HasInPoint)
                res += 4;
            if (HasOutPoint)
                res += 4;
            if (HasLoops)
                res += 2;
            if (HasEnvelope)
            {
                res++;
                if (envelopeRecord != null)
                    res += envelopeRecord.Count * SoundEnvelope.GetSizeOf();
            }
            return res;
        }

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        public void ReadData(BufferedBinaryReader binaryReader)
        {
            binaryReader.ReadUBits(2);

            syncStop = binaryReader.ReadBoolean();
            syncNoMultiple = binaryReader.ReadBoolean();
            bool hasEnvelope = binaryReader.ReadBoolean();
            bool hasLoops = binaryReader.ReadBoolean();
            bool hasOutPoint = binaryReader.ReadBoolean();
            bool hasInPoint = binaryReader.ReadBoolean();

            if (hasInPoint)
                inPoint = binaryReader.ReadUInt32();
            if (hasOutPoint)
                outPoint = binaryReader.ReadUInt32();
            if (hasLoops)
                loopCount = binaryReader.ReadUInt16();
            if (hasEnvelope)
            {
                byte envPoints = binaryReader.ReadByte();
                if (envPoints != 0)
                {
                    envelopeRecord.Clear();
                    for (int i = 0; i < envPoints; i++)
                    {
                        SoundEnvelope envelope = new SoundEnvelope();
                        envelope.ReadData(binaryReader); 
                        envelopeRecord.Add(envelope);
                    }
                }
            }
        }

        /// <summary>
        /// Writes to binary writer.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public void WriteTo(BufferedBinaryWriter writer)
        {
			writer.WriteUBits(0, 2);
			writer.WriteBoolean(syncStop);
			writer.WriteBoolean(syncNoMultiple);
			writer.WriteBoolean(HasEnvelope);
			writer.WriteBoolean(HasLoops);
			writer.WriteBoolean(HasOutPoint);
			writer.WriteBoolean(HasInPoint);

			if (HasInPoint)
				writer.Write(inPoint);
			if (HasOutPoint)
				writer.Write(outPoint);
			if (HasLoops)
				writer.Write(loopCount);
            
            if (HasEnvelope)
            {
                writer.Write((byte)envelopeRecord.Count);
                IEnumerator envelopes = envelopeRecord.GetEnumerator();
                while (envelopes.MoveNext())
                    ((SoundEnvelope)envelopes.Current).WriteTo(writer);
            }
        }

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("SoundInfo");
			writer.WriteElementString("SyncStop", syncStop.ToString());
			writer.WriteElementString("SyncNoMultiple", syncNoMultiple.ToString());
			writer.WriteElementString("HasEnvelope", HasEnvelope.ToString());
			writer.WriteElementString("HasLoops", HasLoops.ToString());
			writer.WriteElementString("HasOutPoint", HasOutPoint.ToString());
			writer.WriteElementString("HasInPoint", HasInPoint.ToString());

			if (HasInPoint)
				writer.WriteElementString("InPoint", inPoint.ToString());
			if (HasOutPoint)
				writer.WriteElementString("OutPoint", outPoint.ToString());
			if (HasLoops)
				writer.WriteElementString("LoopCount", loopCount.ToString());
            
			if (HasEnvelope)
			{
                IEnumerator envelopes = envelopeRecord.GetEnumerator();
                while (envelopes.MoveNext())
                    ((SoundEnvelope)envelopes.Current).Serialize(writer);
			}
			writer.WriteEndElement();
		}

		#endregion
	}

	/// <summary>
	/// SoundEnvelope
	/// </summary>
	public class SoundEnvelope: ISwfSerializer
	{
		#region Members

		private uint pos44;
		private ushort leftLevel;
		private ushort rightLevel;

		#endregion

		#region Ctor

		/// <summary>
		/// Creates a new <see cref="SoundEnvelope"/> instance.
		/// </summary>
		public SoundEnvelope() 
		{ 
		}

		/// <summary>
		/// Creates a new <see cref="SoundEnvelope"/> instance.
		/// </summary>
		/// <param name="pos44">Pos44.</param>
		/// <param name="leftLevel">Left level.</param>
		/// <param name="rightLevel">Right level.</param>
		public SoundEnvelope(uint pos44, ushort leftLevel, ushort rightLevel)
		{
			this.pos44 = pos44;
			this.leftLevel = leftLevel;
			this.rightLevel = rightLevel;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the right level.
		/// </summary>
		/// <value></value>
		public ushort RightLevel
		{
			get { return this.rightLevel;  }
			set { this.rightLevel = value; }
		}

		/// <summary>
		/// Gets or sets the left level.
		/// </summary>
		/// <value></value>
		public ushort LeftLevel
		{
			get { return this.leftLevel;  }
			set { this.leftLevel = value; }
		}

		/// <summary>
		/// Gets or sets the pos44.
		/// </summary>
		/// <value></value>
		public uint Pos44
		{
			get { return this.pos44;  }
			set { this.pos44 = value; }
		}

		#endregion

		#region Methods

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="binaryReader">Binary reader.</param>
        public void ReadData(BufferedBinaryReader binaryReader)
        {
            this.pos44 = binaryReader.ReadUInt32();
            this.leftLevel = binaryReader.ReadUInt16();
            this.rightLevel = binaryReader.ReadUInt16();
        }

		/// <summary>
		/// Gets the size of.
		/// </summary>
		/// <returns>size of this object</returns>
		public static int GetSizeOf()
		{
			return 4 + 2 + 2;
		}

		/// <summary>
		/// Writes to a binary writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void WriteTo(BinaryWriter writer)
		{
			writer.Write(this.pos44);
			writer.Write(this.leftLevel);
			writer.Write(this.rightLevel);
		}

		/// <summary>
		/// Serializes the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Serialize(XmlWriter writer)
		{
			writer.WriteStartElement("SoundEnvelope");
			writer.WriteElementString("Pos44", pos44.ToString());
			writer.WriteElementString("LeftLevel", leftLevel.ToString());
			writer.WriteElementString("RightLevel", rightLevel.ToString());
			writer.WriteEndElement();
		}

		#endregion
	}

    /// <summary>
    /// SoundEnvelopeCollection class
    /// </summary>
    public class SoundEnvelopeCollection: CollectionBase, ISwfSerializer
    {
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="SoundEnvelopeCollection"/> instance.
        /// </summary>
        public SoundEnvelopeCollection()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Serializes the specified writer.
        /// </summary>
        /// <param name="writer">Writer.</param>
        public void Serialize(XmlWriter writer)
        {
            writer.WriteStartElement("SoundEnvelopeCollection");
            
            IEnumerator envs = this.GetEnumerator();
            while (envs.MoveNext())
                ((SoundEnvelope)envs.Current).Serialize(writer);

            writer.WriteEndElement();
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public SoundEnvelope Add(SoundEnvelope value)
        {
            List.Add(value as object);
            return value;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="values">Values.</param>
        public void AddRange(SoundEnvelope[] values)
        {
            IEnumerator val = values.GetEnumerator();
            while (val.MoveNext())
               Add((SoundEnvelope)val.Current);
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public void Remove(SoundEnvelope value)
        {
            if (List.Contains(value))
                List.Remove(value as object);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="value">Value.</param>
        public void Insert(int index, SoundEnvelope value)
        {
            List.Insert(index, value as object);
        }

        /// <summary>
        /// Containses the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public bool Contains(SoundEnvelope value)
        {
            return List.Contains(value as object);
        }

        /// <summary>
        /// Gets or sets the <see cref="SoundEnvelope"/> at the specified index.
        /// </summary>
        /// <value></value>
        public SoundEnvelope this[int index]
        {
            get
            {
                return ((SoundEnvelope)List[index]);
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
        public int IndexOf(SoundEnvelope value)
        {
            return List.IndexOf(value);
        }

        #endregion
    }
}
