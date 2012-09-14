namespace SwfDotnet.Format.Tags
{
    using SwfDotnet.Format.ActionScript;
    using SwfDotnet.Format.BasicTypes;
    using System;

    public class TagDoAction : BaseTag
    {
        public TagDoAction(Script ActioScript) : base(12)
        {
            this.Add(ActioScript);
            this.Add(new UI8(0));
        }
    }
}

