namespace SwfDotnet
{
    using SwfDotnet.Format;
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.Tags;
    using System;

    public class DefineSprite : Character, IDepthChanger
    {
        public FrameArray _Frames;

        public DefineSprite() : base(0x27)
        {
            this._Frames = new FrameArray();
        }

        protected override void OnCompile()
        {
            this.Add(new UI16(base.CharacterID));
            this.Add(new UI16(this.Frames.Count));
            this.Add(this.Frames);
            this.Add(new TagEnd());
            base.OnCompile();
        }

        public int SetDepth(int depth)
        {
            int num = 0;
            for (int i = 1; i <= this.Frames.Count; i++)
            {
                num += this.Frames[i].AdjustDephts(num + depth);
            }
            return num;
        }

        public virtual FrameArray Frames
        {
            get
            {
                return this._Frames;
            }
        }
    }
}

