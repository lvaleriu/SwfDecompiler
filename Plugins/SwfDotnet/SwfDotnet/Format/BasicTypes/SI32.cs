namespace SwfDotnet.Format.BasicTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SI32 : IBytes
    {
        private int _value;
        public SI32(int val)
        {
            this._value = val;
        }

        public byte[] GetBytes
        {
            get
            {
                AB ab = new AB(4);
                ab.Append(new SB(0x20, this._value));
                return ab.GetBytes;
            }
        }
        public int Length
        {
            get
            {
                return 4;
            }
        }
        public static implicit operator SI32(int x)
        {
            return new SI32(x);
        }

        public static implicit operator int(SI32 x)
        {
            return x._value;
        }
    }
}

