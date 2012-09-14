namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Bit
    {
        private int _value;
        public Bit(int val)
        {
            this._value = val * 20;
        }

        public static implicit operator Bit(int x)
        {
            return new Bit(x);
        }

        public static implicit operator Bit(bool x)
        {
            return new Bit(x ? 1 : 0);
        }

        public static implicit operator int(Bit x)
        {
            return ((x._value == 0) ? 0 : 1);
        }
    }
}

