using System;
using SwfDotNet.IO;
using SwfDotNet.IO.Tags;
using SwfDotNet.IO.Tags.Types;

namespace SwfDotNet.IO.Samples.Simples.BgColor
{
	/// <summary>
	/// SetBackgroundColorTag test.
	/// </summary>
	class BgColor
	{
        private const string path = "test.swf";

		/// <summary>
		/// Point d'entrée principal de l'application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Swf swf = new Swf();
            swf.Version = 5;
            swf.Tags.Add(new SetBackgroundColorTag(new RGB(0, 0, 255)));
            swf.Tags.Add(new ShowFrameTag());
            swf.Tags.Add(new EndTag());

            SwfWriter writer = new SwfWriter(path);
            writer.Write(swf);
            writer.Close();
		}
	}
}
