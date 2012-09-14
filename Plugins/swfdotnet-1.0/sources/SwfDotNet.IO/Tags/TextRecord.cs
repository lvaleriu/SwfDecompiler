using System;
using System.Collections;

namespace SwfDotNet.Data.Tags
{
	/// <summary>
	/// Summary description for TextRecord.
	/// </summary>
	public class TextRecord
	{
		public BitArray flags;
		public ushort fontId;
		public byte red;
		public byte green;
		public byte blue;
		public byte alpha;
		public ushort textHeight;
		public byte glyphCount;
		public short xOffset;
		public short yOffset;
		public GlyphEntry[] glyphEntries;

		public TextRecord() { }
	}

	public struct GlyphEntry
	{
		BitArray glyphIndex;
		BitArray glyphAdvance;
	}
}
