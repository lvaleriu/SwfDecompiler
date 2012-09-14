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

namespace SwfDotNet.IO.Tags 
{
	
	// action tags:	
	// 12: DoAction
	// 26: PlaceObject2
	// 39: DefineSprite
	// 59: InitAction
	//  7: DefineButton
	// 34: DefineButton2
	
	/// <summary>
	/// enumeration of tag codes of tags containing bytecode.
	/// </summary>
	public enum TagCodeEnum 
	{
        /// <summary>Unknown tag</summary>
        Unknown = -1,
		/// <summary>End tag</summary>
		End = 0,
		/// <summary>ShowFrame tag</summary>
		ShowFrame = 1,
		/// <summary>DefineShape tag</summary>
		DefineShape = 2,
		/// <summary>PlaceObject tag</summary>
		PlaceObject = 3,
		/// <summary>RemoveObject tag</summary>
		RemoveObject = 4,
		/// <summary>DefineBits tag</summary>
		DefineBits = 6,
		/// <summary>DefineButton tag</summary>
		DefineButton = 7,
		/// <summary>JPEGTable tag</summary>
		JpegTable = 8,
		/// <summary>SetBackgroundColor tag</summary>
		SetBackgroundColor = 9,		
		/// <summary>DefineFont tag</summary>
		DefineFont = 10,
		/// <summary>DefineText tag</summary>
		DefineText = 11,
		/// <summary>DoAction tag</summary>
		DoAction = 12,
		/// <summary>DefineFontInfo tag</summary>
		DefineFontInfo = 13,
		/// <summary>DefineSound tag</summary>
		DefineSound = 14,
		/// <summary>StartSound tag</summary>
		StartSound = 15,
		/// <summary>DefineButtonSound tag</summary>
		DefineButtonSound = 17,
		/// <summary>SoundStreamHead tag</summary>
		SoundStreamHead = 18,
		/// <summary>SoundStreamBlock tag</summary>
		SoundStreamBlock = 19,
		/// <summary>DefineBitsLossLess tag</summary>
		DefineBitsLossLess = 20,
		/// <summary>DefineBitsJPEG2 tag</summary>
		DefineBitsJpeg2 = 21,
		/// <summary>DefineShape2 tag</summary>
		DefineShape2 = 22,
		/// <summary>DefineButtonCxForm tag</summary>
		DefineButtonCxForm = 23,
		/// <summary>Protect tag</summary>
		Protect = 24,
		/// <summary>PlaceObject2 tag</summary>
		PlaceObject2 = 26,
		/// <summary>RemoveObject2 tag</summary>
		RemoveObject2 = 28,
		/// <summary>DefineShape3 tag</summary>
		DefineShape3 = 32,
        /// <summary>DefineText2 tag</summary>
		DefineText2 = 33,
		/// <summary>DefineButton2 tag</summary>
		DefineButton2 = 34,
		/// <summary>DefineBitsJPEG3 tag</summary>
		DefineBitsJpeg3 = 35,
		/// <summary>DefineBitsLossLess2 tag</summary>
		DefineBitsLossLess2 = 36,
		/// <summary>DefineEditText tag</summary>
		DefineEditText = 37,
		/// <summary>DefineSprite tag</summary>
		DefineSprite = 39,
		/// <summary>FrameLabel tag</summary>
		FrameLabel = 43,
		/// <summary>SoundStreamHead2 tag</summary>
		SoundStreamHead2 = 45,
		/// <summary>DefineMorphShape tag</summary>
		DefineMorphShape = 46,
		/// <summary>DefineFont2 tag</summary>
		DefineFont2 = 48,
		/// <summary>ExportAssets tag</summary>
		ExportAssets = 56,
		/// <summary>ImportAssets tag</summary>
		ImportAssets = 57,
		/// <summary>EnableDebugger tag</summary>
		EnableDebugger = 58,
		/// <summary>InitAction tag</summary>
		InitAction = 59,
		/// <summary>DefineVideoStream tag</summary>
		DefineVideoStream = 60,
		/// <summary>VideoFrame tag</summary>
		VideoFrame = 61,
		/// <summary>DefineFontInfo2 tag</summary>
		DefineFontInfo2 = 62,
		/// <summary>EnableDebugger2 tag</summary>
		EnableDebugger2 = 64,
		/// <summary>ScriptLimit tag</summary>
		ScriptLimit = 65,
		/// <summary>SetTabIndex tag</summary>
		SetTabIndex	= 66
	}
}
