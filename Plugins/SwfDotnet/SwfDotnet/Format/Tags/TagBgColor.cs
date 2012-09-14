namespace SwfDotnet.Format.Tags
{
    using SwfDotnet.Format.BasicTypes;
    using System;

    public class TagBgColor : BaseTag
    {
        public TagBgColor(RGB Color) : base(9)
        {
            this.Add(Color);
        }
    }
}

