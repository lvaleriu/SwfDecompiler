namespace SwfDotnet.Format.Tags
{
    using SwfDotnet.Format.BasicTypes;
    using System;

    public class EnableDebugger2 : BaseTag
    {
        public EnableDebugger2() : base(0x40)
        {
            this.Add(new STRING("", true));
        }
    }
}

