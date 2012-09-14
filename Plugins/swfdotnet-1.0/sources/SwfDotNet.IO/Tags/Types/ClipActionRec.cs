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

using SwfDotNet.IO.Utils;

namespace SwfDotNet.IO.Tags.Types 
{
	/// <summary>
	/// The ClipActionRecord class. 
	/// </summary>
	public class ClipActionRec 
	{	
        #region Members

		private byte[] header;
		private int keyCode = -1;
		private byte[] actionRec;
		
        #endregion

        #region Ctor
    
        /// <summary>
        /// Creates a new <see cref="ClipActionRec"/> instance.
        /// </summary>
        public ClipActionRec()
        {
        }

		/// <summary>
        /// Creates a new <see cref="ClipActionRec"/> instance.
        /// </summary>
		/// <param name="header">Data preceding bytecode block that doesn´t need to get parsed.</param>
		/// <param name="key">Key code.</param>
		/// <param name="actionRec">Raw bytecode.</param>
		public ClipActionRec(byte[] header, int key, byte[] actionRec) 
		{
			this.header = header;
			this.keyCode = key;
			this.actionRec = actionRec;
		}

        #endregion
		
        #region Properties

        /// <summary>
        /// Property for accessing raw bytecode block.
        /// </summary>
        public byte[] ActionRecord 
        {
            get 
            {
                return actionRec;
            }
            set 
            {
                actionRec = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="br">Br.</param>
        /// <param name="version">Version.</param>
        /// <returns>true if ok, false otherwise</returns>
        public bool ReadData(BinaryReader br, byte version)
        {
            // different behaviour for Flash 5 and Flash 6+
            if (version >= 6) 
            {
                if (br.ReadInt32()==0) 
                    return false;
                br.BaseStream.Position -= 4;
            } 
            else 
            {				
                if (br.ReadInt16() == 0) 
                    return false;
                br.BaseStream.Position -= 2;
            }
						
            int size;
			
            // different behaviour for Flash 5 and Flash 6+
            if (version >= 6) 
            {
                header = br.ReadBytes(4);				
				
                // swf events
                byte f6Events = header[2];
                bool hasKeyPress = ( (f6Events & 0x02) != 0);
                bool hasConstruct = ( (f6Events & 0x04) != 0); // Flash 7 +

                size = br.ReadInt32();
				
                if (hasKeyPress) 
                {					
                    keyCode = Convert.ToInt32(br.ReadByte());
                    size--;					
                } 
                else { keyCode = -1; }

                actionRec = br.ReadBytes(size);
            } 
            else 
            {
                header = br.ReadBytes(2);
                size = br.ReadInt32();				
                keyCode = -1;				
                actionRec = br.ReadBytes(size);
            }
            return true;
        }

		/// <summary>
		/// Get binary data of ClipAction Record for swf compilation.
		/// </summary>
		public byte[] GetData(byte version) 
		{
			int offset = actionRec.Length;
			if (keyCode>-1) offset++;
					
			MemoryStream m = new MemoryStream();
			BinaryWriter w = new BinaryWriter(m);
			
			w.Write(header);
			w.Write(offset);
			
			if (keyCode>-1) w.Write(Convert.ToByte(keyCode));
			
			w.Write(actionRec);
			
			return m.ToArray();
		}		

        #endregion
	}
}
