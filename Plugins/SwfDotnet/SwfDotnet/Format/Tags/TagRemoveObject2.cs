namespace SwfDotnet.Format.Tags
{
    using SwfDotnet.Format;
    using SwfDotnet.Format.BasicTypes;
    using System;

    public class TagRemoveObject2 : BaseTag
    {
        private Character _character;

        public TagRemoveObject2(Character Character) : base(0x1c)
        {
            this._character = Character;
        }

        protected override void OnCompile()
        {
            this.Add(new UI16(this._character.Depth));
            base.OnCompile();
        }
    }
}

