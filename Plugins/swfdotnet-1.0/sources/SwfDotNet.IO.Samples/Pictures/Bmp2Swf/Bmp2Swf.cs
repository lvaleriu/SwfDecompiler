/*
	SwfDotNet is an open source library for writing and reading 
	Macromedia Flash (SWF) bytecode.
	Copyright (C) 2005 Olivier Carpentier.
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
using System.Drawing;

using SwfDotNet.IO;
using SwfDotNet.IO.Tags;
using SwfDotNet.IO.Tags.Types;

namespace SwfDotNet.IO.Samples.Pictures.Bmp2Swf
{
	/// <summary>
	/// Bmp2Swf
	/// </summary>
	class Bmp2Swf
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//Picture to transform
			string imgPath = "Untitled-1.bmp";
			//File name of the result swf file
			string path = "test.swf";

			//Load the picture to a GDI image
			Image img = Image.FromFile(imgPath);
			int posX = 0;
			int posY = 0;
			int imgWidth = img.Width;
			int imgHeight = img.Height;
            
			//Create a new Swf instance
			Swf swf = new Swf();
			swf.Size = new Rect(0, 0, (posX + imgWidth) * 20, (posY + imgHeight) * 20);
			swf.Version = 5;

			//Set the background color tag
			swf.Tags.Add(new SetBackgroundColorTag(255, 255, 255));

			//Set the jpeg tag
			ushort jpegId = swf.GetNewDefineId();
			//Load the jped from an image
			swf.Tags.Add(DefineBitsLossLessTag.FromImage(jpegId, img));
			
			//Define the picture's shape tag
			DefineShapeTag shapeTag = new DefineShapeTag();
			shapeTag.CharacterId = swf.GetNewDefineId();
			shapeTag.Rect = new Rect(posX * 20 - 1, posY * 20 - 1, (posX + imgWidth) * 20 - 1, (posY + imgHeight) * 20 - 1); 
			FillStyleCollection fillStyles = new FillStyleCollection();
			fillStyles.Add(new BitmapFill(FillStyleType.ClippedBitmapFill, ushort.MaxValue, new Matrix(0, 0, 20, 20)));
			fillStyles.Add(new BitmapFill(FillStyleType.ClippedBitmapFill, jpegId, new Matrix(posX * 20 - 1, posY * 20 - 1, 20, 20)));
			LineStyleCollection lineStyles = new LineStyleCollection();
			ShapeRecordCollection shapes = new ShapeRecordCollection();
			shapes.Add(new StyleChangeRecord(posX * 20 - 1, posY * 20 - 1, 2));
			shapes.Add(new StraightEdgeRecord(imgWidth * 20, 0));
			shapes.Add(new StraightEdgeRecord(0, imgHeight * 20));
			shapes.Add(new StraightEdgeRecord(-imgWidth * 20, 0));
			shapes.Add(new StraightEdgeRecord(0, -imgHeight * 20));
			shapes.Add(new EndShapeRecord());
			shapeTag.ShapeWithStyle = new ShapeWithStyle(fillStyles, lineStyles, shapes);
			swf.Tags.Add(shapeTag);

			//Place the picture
			swf.Tags.Add(new PlaceObject2Tag(shapeTag.CharacterId, 1, 0, 0));
			//Add a frame
			swf.Tags.Add(new ShowFrameTag());
			swf.Tags.Add(new EndTag());

			//Write the swf to a file
			SwfWriter writer = new SwfWriter(path);
			writer.Write(swf);
			writer.Close();
			img.Dispose();
		}
	}
}
