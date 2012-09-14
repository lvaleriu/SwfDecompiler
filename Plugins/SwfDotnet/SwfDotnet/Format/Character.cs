namespace SwfDotnet.Format
{
    using SwfDotnet.Format.Tags;
    using System;

    public class Character : BaseTag
    {
        public int CharacterID;
        public int Depth;

        public Character(int tagcode) : base(tagcode)
        {
        }
    }
}

