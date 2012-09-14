namespace SwfDotnet.Format.Tags
{
    using SwfDotnet.Format.BasicTypes;
    using System;

    public class TagRemoveObject : BaseTag
    {
        public TagRemoveObject(int CharacterID, int Deph) : base(5)
        {
            this.Add(new UI16(CharacterID));
            this.Add(new UI16(Deph));
        }
    }
}

