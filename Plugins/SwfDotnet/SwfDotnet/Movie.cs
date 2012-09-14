namespace SwfDotnet
{
    using SwfDotnet.Format;
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.Tags;
    using SwfDotnet.Format.UtilTypes;
    using System;
    using System.Collections;
    using System.IO;

    public class Movie : ArrayData
    {
        private int _charID = 0;
        private RGB BackColor;
        private Hashtable Dictionary = new Hashtable();
        public int FrameRate = 12;
        public FrameArray Frames;
        public int Height;
        public SwfDotnet.Layers Layers = new SwfDotnet.Layers();
        public int Width;

        public Movie(int width, int height, RGB backColor)
        {
            this.BackColor = backColor;
            this.Height = height;
            this.Width = width;
            this.Frames = new FrameArray();
        }

        private void AdjustDephts()
        {
            int depth = 1;
            for (int i = 1; i <= this.Frames.Count; i++)
            {
                depth += this.Frames[i].AdjustDephts(depth);
            }
        }

        public void Define(Character character)
        {
            if (!this.IsDefined(character))
            {
                this.Dictionary.Add(character.GetHashCode(), null);
                this._charID++;
                character.CharacterID = this._charID;
                this.Add(character);
            }
        }

        public void Define(SwfFont font)
        {
            this._charID++;
            font.FontID = this._charID;
            this.Add(font);
        }

        public bool IsDefined(Character character)
        {
            return this.Dictionary.ContainsKey(character.GetHashCode());
        }

        protected override void OnCompile()
        {
            this.Add(this.Frames);
            this.Add(new TagEnd());
            this.Insert(0, new EnableDebugger2());
            this.Insert(0, new TagBgColor(this.BackColor));
            this.Insert(0, new Header(this.Width, this.Height, this.Length, this.Frames.Length, this.FrameRate));
        }

        public void SaveToFile(string path)
        {
            FileStream output = File.Open(path, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(output);
            this.AdjustDephts();
            writer.Write(this.GetBytes);
            writer.Flush();
            writer.Close();
            output.Close();
        }
    }
}

