namespace SwfDotnet.Format.Tags
{
    using SwfDotnet.Format;
    using SwfDotnet.Format.BasicTypes;
    using System;
    using System.IO;

    public class TagDefineBitsJPEG2 : Character
    {
        public string ImageSrc;

        public TagDefineBitsJPEG2(string imageSrc) : base(0x15)
        {
            this.ImageSrc = imageSrc;
        }

        protected override void OnCompile()
        {
            this.Add(new UI16(base.CharacterID));
            FileStream stream = File.Open(this.ImageSrc, FileMode.Open, FileAccess.Read);
            this.Add(new UI8(0xff));
            this.Add(new UI8(0xd8));
            this.Add(new UI8(0xff));
            this.Add(new UI8(0xd9));
            for (int i = 0; i < stream.Length; i++)
            {
                this.Add(new UI8(stream.ReadByte()));
            }
            base.OnCompile();
        }
    }
}

