namespace SwfDotnet.Format
{
    using SwfDotnet.Format.Tags;
    using System;

    public class SwfFont : BaseTag
    {
        public int FontID;

        public SwfFont(int tagcode) : base(tagcode)
        {
        }
    }
}

