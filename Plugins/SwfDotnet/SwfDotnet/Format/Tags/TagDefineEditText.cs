namespace SwfDotnet.Format.Tags
{
    using SwfDotnet.Format;
    using SwfDotnet.Format.BasicTypes;
    using System;

    public class TagDefineEditText : Character
    {
        private RECT _Bounds;
        private int FontID;
        public bool HasBorder;
        public int Height;
        public bool Multitile;
        public bool PasswordFiled;
        public bool ReadOnly;
        public bool Selectable;
        public string Text;
        public RGB TextColor;
        public string VarName;
        public bool WordWrap;

        public TagDefineEditText(SwfFont font, int height, RECT Bounds, string text) : base(0x25)
        {
            this.VarName = string.Empty;
            this.Text = string.Empty;
            this.Selectable = false;
            this.HasBorder = false;
            this.ReadOnly = true;
            this.WordWrap = false;
            this.Multitile = false;
            this.PasswordFiled = false;
            this.TextColor = new RGB(0, 0, 0);
            this.Height = 200;
            this._Bounds = Bounds;
            this.Height = height;
            this.FontID = font.FontID;
            this.Text = text;
        }

        protected override void OnCompile()
        {
            this.Add(new UI16(base.CharacterID));
            this.Add(this._Bounds);
            AB item = new AB(2);
            item.Append(new UB(1, 1));
            item.Append(new UB(1, Convert.ToInt32( this.WordWrap)));
            item.Append(new UB(1, Convert.ToInt32( this.Multitile)));
            item.Append(new UB(1, Convert.ToInt32( this.PasswordFiled)));
            item.Append(new UB(1, Convert.ToInt32( this.ReadOnly)));
            item.Append(new UB(1, 1));
            item.Append(new UB(1, 0));
            item.Append(new UB(1, 1));
            item.Append(new UB(2, 0));
            item.Append(new UB(1, 0));
            item.Append(new UB(1, Convert.ToInt32( !this.Selectable)));
            item.Append(new UB(1, Convert.ToInt32( this.HasBorder)));
            item.Append(new UB(2, 0));
            item.Append(new UB(1, 0));
            this.Add(item);
            this.Add(new UI16(this.FontID));
            this.Add(new UI16(this.Height));
            RGBA rgba = new RGBA(this.TextColor.Red, this.TextColor.Green, this.TextColor.Blue, 0);
            this.Add(rgba);
            this.Add(new STRING(this.VarName));
            this.Add(new STRING(this.Text));
            base.OnCompile();
        }
    }
}

