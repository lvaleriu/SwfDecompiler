namespace SwfDotnet.Format.Tags
{
    using SwfDotnet.Format;
    using SwfDotnet.Format.BasicTypes;
    using System;

    public class TagDefineFont2 : SwfFont
    {
        public Bit FontBold;
        public Bit FontItalic;
        public string FontName;

        public TagDefineFont2(string fontName) : base(0x30)
        {
            this.FontBold = 0;
            this.FontItalic = 0;
            this.FontName = fontName;
        }

        protected override void OnCompile()
        {
            this.Add(new UI16(base.FontID));
            AB item = new AB(2);
            item.Append(new UB(1, 0));
            item.Append(new UB(1, 0));
            item.Append(new UB(1, 0));
            item.Append(new UB(1, 0));
            item.Append(new UB(1, 0));
            item.Append(new UB(1, 1));
            item.Append(new UB(1, 0));
            item.Append(new UB(1, (int) this.FontBold));
            item.Append(new UB(8, (int) this.FontItalic));
            this.Add(item);
            this.Add(new UI8(this.FontName.Length));
            this.Add(new STRING(this.FontName, false));
            this.Add(new UI16(0));
            base.OnCompile();
        }
    }
}

