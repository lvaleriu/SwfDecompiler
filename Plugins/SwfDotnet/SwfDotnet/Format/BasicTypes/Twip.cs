namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Twip
    {
        private int _value;
        public Twip(int val)
        {
            this._value = val * 20;
        }

        public static implicit operator Twip(int x)
        {
            return new Twip(x);
        }

        public static implicit operator int(Twip x)
        {
            return x._value;
        }
    }
}

