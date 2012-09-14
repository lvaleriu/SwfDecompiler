namespace SwfDotnet.Format.ActionScript
{
    using SwfDotnet.Format.BasicTypes;
    using SwfDotnet.Format.UtilTypes;
    using System;

    internal sealed class Action : ArrayData
    {
        public int _code;
        public bool Jump;
        public int JumpOffset = 0;

        public Action(int tipo)
        {
            this._code = tipo;
        }

        protected override void OnCompile()
        {
            if (this.Jump)
            {
                this.Add(new UI16(this.JumpOffset));
            }
            int length = this.Length;
            if (this._code >= 0x80)
            {
                this.Insert(0, new UI16(length));
            }
            this.Insert(0, new UI8(this._code));
            base.OnCompile();
        }
    }
}

